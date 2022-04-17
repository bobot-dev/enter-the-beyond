using System;
using UnityEngine;

public class OrbitalProjDebuff : AppliedEffectBase
{
	private void InitializeSelf(OrbitalProjDebuff source)
	{
		if (!source)
		{
			return;
		}
		this.hh = base.GetComponent<HealthHaver>();
		if (this.hh != null)
		{
			Projectile component = source.GetComponent<Projectile>();
			if (component.PossibleSourceGun != null)
			{
				this.m_attachedGun = component.PossibleSourceGun;
				this.m_player = (component.PossibleSourceGun.CurrentOwner as PlayerController);
				
			}
			else if (component && component.Owner && component.Owner.CurrentGun)
			{
				this.m_attachedGun = component.Owner.CurrentGun;
				this.m_player = (component.Owner as PlayerController);				
			}
			hh.AllDamageMultiplier += 0.1f;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}


	public override void Initialize(AppliedEffectBase source)
	{
		if (source is OrbitalProjDebuff)
		{
			var OrbitalProjDebuff = source as OrbitalProjDebuff;
			this.InitializeSelf(OrbitalProjDebuff);
			if (OrbitalProjDebuff.vfx != null)
			{
				this.instantiatedVFX = SpawnManager.SpawnVFX(OrbitalProjDebuff.vfx, base.transform.position, Quaternion.identity, true);
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
		OrbitalProjDebuff OrbitalProjDebuff = target.AddComponent<OrbitalProjDebuff>();
		OrbitalProjDebuff.Initialize(this);
	}


	private void OnDestroy()
	{
		if (hh.AllDamageMultiplier > 1) hh.AllDamageMultiplier -= 0.1f;
	}

	public GameObject vfx;

	private GameObject instantiatedVFX;

	private PlayerController m_player;

	private Gun m_attachedGun;

	private HealthHaver hh;

	private Vector2 m_cachedSourceVector = Vector2.zero;
}
