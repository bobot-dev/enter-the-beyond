using BotsMod;
using Dungeonator;
using System;
using UnityEngine;

public class EmpSpearBuff : AppliedEffectBase
{

	public EmpSpearBuff()
	{
		this.m_cachedSourceVector = Vector2.zero;
	}
	private void InitializeSelf(EmpSpearBuff source)
	{
		if (!source)
		{
			return;
		}
		//this.explosionData = source.explosionData;
		this.hh = base.GetComponent<HealthHaver>();
		if (this.hh != null)
		{
			Projectile component = source.GetComponent<Projectile>();
			if (component.PossibleSourceGun != null)
			{
				this.m_attachedGun = component.PossibleSourceGun;
				this.m_player = (component.PossibleSourceGun.CurrentOwner as PlayerController);
				Gun possibleSourceGun = component.PossibleSourceGun;
				//possibleSourceGun.OnReloadPressed += this.ExplodeOnReload;
				if (this.m_player)
				{
					this.m_player.GunChanged += this.GunChanged;
				}
			}
			else if (component && component.Owner && component.Owner.CurrentGun)
			{
				this.m_attachedGun = component.Owner.CurrentGun;
				this.m_player = (component.Owner as PlayerController);
				Gun currentGun = component.Owner.CurrentGun;
				//currentGun.OnReloadPressed += this.ExplodeOnReload;
				if (this.m_player)
				{
					this.m_player.GunChanged += this.GunChanged;
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}
	private void Disconnect()
	{
		if (this.m_player)
		{
			this.m_player.GunChanged -= this.GunChanged;
		}
		if (this.m_attachedGun)
		{
			Gun attachedGun = this.m_attachedGun;
			//attachedGun.OnReloadPressed -= this.ExplodeOnReload;
		}
	}
	private void GunChanged(Gun arg1, Gun arg2, bool newGun)
	{
		this.Disconnect();
		this.DoEffect();
	}

	private void ExplodeOnReload(PlayerController arg1, Gun arg2, bool actual)
	{
		this.Disconnect();
		this.DoEffect();
	}
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is EmpSpearBuff)
		{
			EmpSpearBuff EmpSpearBuff = source as EmpSpearBuff;
			this.InitializeSelf(EmpSpearBuff);
			if (EmpSpearBuff.vfx != null)
			{
				this.instantiatedVFX = SpawnManager.SpawnVFX(EmpSpearBuff.vfx, base.transform.position, Quaternion.identity, true);
				tk2dSprite component = this.instantiatedVFX.GetComponent<tk2dSprite>();
				tk2dSprite component2 = base.GetComponent<tk2dSprite>();
				if (component != null && component2 != null)
				{
					component2.AttachRenderer(component);
					component.HeightOffGround = 0.1f;
					component.IsPerpendicular = true;
					component.usesOverrideMaterial = true;
				}
				BuffVFXAnimator component3 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
				if (component3 != null)
				{
					Projectile component4 = source.GetComponent<Projectile>();
					if (component4 && component4.LastVelocity != Vector2.zero)
					{
						this.m_cachedSourceVector = component4.LastVelocity;
						component3.InitializePierce(base.GetComponent<GameActor>(), component4.LastVelocity);
					}
					else
					{
						component3.Initialize(base.GetComponent<GameActor>());
					}
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		if (this.IsSynergyContingent)
		{
			Projectile component = base.GetComponent<Projectile>();
			if (!component || !(component.Owner is PlayerController))
			{
				return;
			}
		}
		EmpSpearBuff EmpSpearBuff = target.AddComponent<EmpSpearBuff>();
		EmpSpearBuff.Initialize(this);
	}

	private void DoEffect()
	{
		if (this.hh)
		{
			//float force = this.explosionData.force / 4f;
			//this.explosionData.force = 0f;
			empEffect = new EmpActorEffect
			{
				AffectsEnemies = true,
				AffectsPlayers = false,
				AppliesDeathTint = false,
				AppliesOutlineTint = true,
				OutlineTintColor = new Color(0.414f, 1.8f, 1.545f, 1),
				AppliesTint = false,
				DamageMultiplier = 2,
				duration = 5,
				DamagePerSecondToEnemies = 0f,
				stackMode = GameActorEffect.EffectStackingMode.Refresh,
				resistanceType = EffectResistanceType.None,
				effectIdentifier = "emp",
				SpeedMultiplier = 0.8f,
				CooldownMultiplier = 1.7f,
				flameNumPerSquareUnit = 10,
			};

			var boom = (PickupObjectDatabase.GetById(593) as Gun).DefaultModule.projectiles[0].GetComponent<ExplosiveModifier>().explosionData.effect;
			if (this.instantiatedVFX != null)
			{
				Exploder.DoDistortionWave(this.instantiatedVFX.GetComponent<tk2dBaseSprite>().WorldCenter + this.m_cachedSourceVector.normalized, 0.3f, 0.6f, 2, 0.4f);

				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();

				absoluteRoom?.ApplyActionToNearbyEnemies(this.instantiatedVFX.GetComponent<tk2dBaseSprite>().WorldCenter + this.m_cachedSourceVector.normalized, 2, new Action<AIActor, float>(this.ApplyDebuff));
				SpawnManager.SpawnVFX(boom, (Vector3)this.instantiatedVFX.GetComponent<tk2dBaseSprite>().WorldCenter + (Vector3)this.m_cachedSourceVector.normalized + new Vector3(0, 0, 4), Quaternion.identity);
				//Exploder.Explode(this.instantiatedVFX.GetComponent<tk2dBaseSprite>().WorldCenter + this.m_cachedSourceVector.normalized * -0.5f, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			}
			else
			{
				Exploder.DoDistortionWave(this.hh.aiActor.CenterPosition, 0.3f, 0.6f, 2, 0.4f);


				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();

				absoluteRoom?.ApplyActionToNearbyEnemies(this.hh.aiActor.CenterPosition, 2, new Action<AIActor, float>(this.ApplyDebuff));
				SpawnManager.SpawnVFX(boom, (Vector3)this.hh.aiActor.CenterPosition + new Vector3(0,0,4), Quaternion.identity);
				//EmpActorEffect
				//Exploder.Explode(this.hh.aiActor.CenterPosition, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			}
			if (this.hh.knockbackDoer && this.m_cachedSourceVector != Vector2.zero)
			{
				//this.hh.knockbackDoer.ApplyKnockback(this.m_cachedSourceVector.normalized, force, false);
			}
		}
		if (this.instantiatedVFX)
		{
			UnityEngine.Object.Destroy(this.instantiatedVFX);
		}
		UnityEngine.Object.Destroy(this);
	}


	void ApplyDebuff(AIActor enemy, float distance)
    {
		enemy.ApplyEffect(empEffect);

	}

	private void OnDestroy()
	{
		this.Disconnect();
	}
	public bool IsSynergyContingent;
	[SerializeField]
	public EmpActorEffect empEffect;
	public GameObject vfx;
	private GameObject instantiatedVFX;
	private PlayerController m_player;
	private Gun m_attachedGun;
	private HealthHaver hh;
	private Vector2 m_cachedSourceVector;
}