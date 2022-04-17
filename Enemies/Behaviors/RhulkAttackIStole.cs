using System;
using System.Collections.Generic;
using UnityEngine;
using Dungeonator;
using FullInspector;
using System.Collections;
using ItemAPI;
using BotsMod;

public class RhulkAttackIStole : BasicAttackBehavior
{
	public override void Start()
	{
		base.Start();
		//this.m_beholsterController = this.m_aiActor.GetComponent<AddBeholsterBeamComponent>();
		this.m_aiActor.healthHaver.OnDeath += (obj) =>
		{
			if (this.m_laserBeam != null)
			{
				this.m_laserBeam.DestroyBeam();
				this.m_laserBeam = null;
			}
		};
		//FuckYouEdmund = FakePrefab.Clone((GameObject)BraveResources.Load("Global VFX/VFX_LaserSight", ".prefab")); 
	}

	//GameObject FuckYouEdmund;

	private bool ShowSpecificBeamShooter()
	{
		return this.beamSelection == ShootBeamBehavior.BeamSelection.Specify;
	}
	public Vector2 LaserFiringCenter
	{
		get
		{
			return this.m_aiActor.transform.position.XY();
		}
	}

	public bool FiringLaser
	{
		get
		{
			return this.IsfiringLaser;
		}
	}
	public bool LaserActive
	{
		get
		{
			return this.m_laserActive;
		}
	}


	public void SetLaserAngle(float alaserAngle)
	{
		this.AlaserAngle = alaserAngle;
		this.AlaserAngle2 = -alaserAngle;
		if (this.IsfiringLaser)
		{
			this.m_aiActor.aiAnimator.FacingDirection = alaserAngle;
		}
	}

	public BasicBeamController LaserBeam
	{
		get
		{
			return this.m_laserBeam;
		}
	}

	public bool m_laserActive;
	public bool IsfiringLaser;
	public float AlaserAngle;
	public float AlaserAngle2;
	public BasicBeamController m_laserBeam;

	public VFXPool chargeUpVfx;
	public VFXPool chargeDownVfx;
	public ProjectileModule beamModule;
	public Transform beamTransform;

	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_aiActor.TargetRigidbody)
		{
			this.m_targetPosition = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			this.m_backupTarget = this.m_aiActor.TargetRigidbody;
		}
		else if (this.m_backupTarget)
		{
			this.m_targetPosition = this.m_backupTarget.GetUnitCenter(ColliderType.HitBox);
		}
	}

	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.PrechargeFiringLaser();
		this.HasTriggeredScript = false;
		this.m_state = RhulkAttackIStole.State.PreCharging;
		if (LockInPlaceWhileAttacking == true)
		{
			this.m_aiActor.SuppressTargetSwitch = true;
			this.m_aiActor.ClearPath();
		}
		this.m_updateEveryFrame = true;

		var listOfBeams = m_aiActor.gameObject.GetComponents(typeof(AIBeamShooter2));

		beam1 = beamToUse[0];
		beam2 = beamToUse[1];

		if (!string.IsNullOrEmpty(this.ChargeAnimation))
		{
			this.m_aiAnimator.PlayUntilCancelled(this.ChargeAnimation, true, null, -1f, false);

		}
		/*
		for (int yeah = 0; yeah < this.m_currentBeamShooters.Count; yeah++)
		{
			AIBeamShooter aibeamShooter2 = this.m_currentBeamShooters[yeah];
			ETGModConsole.Log(aibeamShooter2.name);
		}
		*/
		//ETGModConsole.Log("ENDED PRECHARGE");
		return BehaviorResult.RunContinuous;
	}

	public override ContinuousBehaviorResult ContinuousUpdate()
	{

		base.ContinuousUpdate();
		//Vector2 b = base.m_aiActor.sprite.WorldCenter;
		if (this.m_state == RhulkAttackIStole.State.PreCharging)
		{
			if (!this.LaserActive)
			{
				this.ChargeFiringLaser(this.chargeTime);
				this.m_timer = this.chargeTime;
				this.m_state = RhulkAttackIStole.State.Charging;
			}
		}
		else
		{
			if (this.m_state == RhulkAttackIStole.State.Charging)
			{

				this.m_timer -= this.m_deltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_state = RhulkAttackIStole.State.Firing;

					this.m_aiAnimator.EndAnimation();

					if (!string.IsNullOrEmpty(this.FireAnimation))
					{
						this.m_aiAnimator.PlayForDuration(this.FireAnimation, firingTime, true, null, -1f, false);

					}

					this.StartFiringTheLaser();
					this.m_timer = this.firingTime;
				}
				return ContinuousBehaviorResult.Continue;
			}
			if (this.m_state == RhulkAttackIStole.State.Firing)
			{

				this.m_timer -= this.m_deltaTime;
				if (this.m_timer <= 0f || !this.FiringLaser)
				{
					if (this.m_bulletSource != null)
					{
						this.m_bulletSource.ForceStop();
					}
					return ContinuousBehaviorResult.Finished;
				}

				bool isFiringLaser = this.beam1.IsFiringLaser;
				if (!isFiringLaser && this.beam1.shouldRotate)
				{
					float alaserAngle;
					float alaserAngle2;
					alaserAngle = BraveMathCollege.ClampAngle360(this.AlaserAngle + ((maxTurnRate * m_deltaTime) * turnRateMultiplier));
					alaserAngle2 = BraveMathCollege.ClampAngle360(this.AlaserAngle2 + ((maxTurnRate * m_deltaTime) * -(turnRateMultiplier)));

					bool flag16 = this.IsfiringLaser == true;
					if (flag16)
					{
						this.AlaserAngle2 = alaserAngle2;
						this.AlaserAngle = alaserAngle;
					}
					return ContinuousBehaviorResult.Continue;
				}

			}
		}
		return ContinuousBehaviorResult.Continue;
	}
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.FireAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.FireAnimation);
		}
		if (!string.IsNullOrEmpty(this.ChargeAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.ChargeAnimation);
		}
		this.StopFiringLaser();
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiActor.SuppressTargetSwitch = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
		this.CleanupReticles();
	}

	public override bool IsOverridable()
	{
		return false;
	}

	public void PrechargeFiringLaser()
	{
		this.m_aiActor.aiAnimator.LockFacingDirection = true;
		//AkSoundEngine.PostEvent(EnemyChargeSound, base.gameObject);
		if (EnemyChargeSound != null)
		{
			AkSoundEngine.PostEvent(EnemyChargeSound, base.m_aiActor.gameObject);

			if (this.HasTriggeredScript != true)
			{
				this.HasTriggeredScript = true;
				if (this.BulletScript != null && !this.BulletScript.IsNull)
				{
					//ETGModConsole.Log(this.BulletScript.ToString());
					if (!this.m_bulletSource)
					{
						this.m_bulletSource = this.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
					}
					this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
					this.m_bulletSource.BulletScript = this.BulletScript;
					this.m_bulletSource.Initialize();
				}
			}

		}
		else if (UsesBaseSounds == true)
		{
			AkSoundEngine.PostEvent("Play_ENM_beholster_charging_01", base.m_aiActor.gameObject);

			if (this.HasTriggeredScript != true)
			{
				this.HasTriggeredScript = true;
				if (this.BulletScript != null && !this.BulletScript.IsNull)
				{
					//ETGModConsole.Log(this.BulletScript.ToString());
					if (!this.m_bulletSource)
					{
						this.m_bulletSource = this.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
					}
					this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
					this.m_bulletSource.BulletScript = this.BulletScript;
					this.m_bulletSource.Initialize();
				}

			}
		}

		//this.m_laserActive = true;


		//base.aiAnimator.PlayUntilCancelled("charge", true, null, -1f, false);
	}

	public void ChargeFiringLaser(float time)
	{
		//AkSoundEngine.PostEvent(BeamChargingSound, base.gameObject);
		if (BeamChargingSound != null) { AkSoundEngine.PostEvent(BeamChargingSound, base.m_aiActor.gameObject); }
		else if (UsesBaseSounds == true) { AkSoundEngine.PostEvent("Play_ENM_beholster_charging_01", base.m_aiActor.gameObject); }
		this.m_laserActive = true;
	}

	public void StartFiringTheLaser()
	{



		float facingDirection = this.m_aiActor.aiAnimator.FacingDirection;
		this.IsfiringLaser = true;
		this.SetLaserAngle(facingDirection);
		this.m_aiActor.aiAnimator.LockFacingDirection = true;

		//ETGModConsole.Log("1", false);
		//float facingDirection = base.m_aiActor.aiAnimator.FacingDirection;
		//ETGModConsole.Log("2", false);
		AkSoundEngine.PostEvent("Play_ENM_deathray_shot_01", base.m_aiActor.gameObject);
		//ETGModConsole.Log("3", false);
		//this.m_laserActive = true;
		this.IsfiringLaser = true;
		//ETGModConsole.Log("4", false);
		//ETGModConsole.Log("5", false);
		base.m_aiActor.aiAnimator.LockFacingDirection = true;
		//ETGModConsole.Log("6", false);
		//base.aiAnimator.PlayUntilCancelled("eyelaser", true, null, -1f, false);
		MonoBehaviour yes = this.m_aiActor.GetComponent<MonoBehaviour>();
		yes.StartCoroutine(this.FireBeam(beam1, 0));
		yes.StartCoroutine(this.FireBeam(beam2, 1));
		//yes.StartCoroutine(this.FireBeam(beam5, 4));

	}


	public void StopFiringLaser()
	{
		if (!this.IsfiringLaser)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.PostFireAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.PostFireAnimation, true, null, -1f, false);

		}
		//AkSoundEngine.PostEvent(StopLaserFiringSound, base.gameObject);
		if (StopLaserFiringSound != null) { AkSoundEngine.PostEvent(StopLaserFiringSound, base.m_aiActor.gameObject); }
		else if (UsesBaseSounds == true) { AkSoundEngine.PostEvent("Stop_ENM_deathray_loop_01", base.m_aiActor.gameObject); }
		this.m_laserActive = false;
		this.IsfiringLaser = false;
		this.m_aiActor.aiAnimator.LockFacingDirection = false;




		this.beam1 = null;
		this.beam2 = null;
		this.CleanupReticles();
	}

	protected IEnumerator FireBeam(AIBeamShooter2 aibeamShooter2, int j)
	{
		this.CleanupReticles();
		//""

		BasicBeamController beamCont = null;
		float enemyTickCooldown = 0f;
		//for (int j = 0; j < 6; j++)
		//{
		//BotsModule.Log(j + " :(");

		aibeamShooter2.shouldRotate = false;

		GameObject beamObject = null;
		bool usesBeamProjectileWithoutModule = this.UsesBeamProjectileWithoutModule;
		if (usesBeamProjectileWithoutModule)
		{
			beamObject = UnityEngine.Object.Instantiate<GameObject>(aibeamShooter2.beamProjectile.gameObject);
		}
		else
		{
			beamObject = UnityEngine.Object.Instantiate<GameObject>(aibeamShooter2.beamModule.GetCurrentProjectile().gameObject);
		}

		// = UnityEngine.Object.Instantiate<GameObject>(aibeamShooter2.beamModule.GetCurrentProjectile().gameObject);

		beamCont = beamObject.GetComponent<BasicBeamController>();


		List<AIActor> activeEnemies = base.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && aiactor != base.m_aiActor && aiactor.healthHaver)
			{
				beamCont.IgnoreRigidbodes.Add(aiactor.specRigidbody);
			}
		}
		beamCont.Owner = base.m_aiActor;
		beamCont.HitsPlayers = true;
		beamCont.HitsEnemies = true;
		//bool facingNorth = BraveMathCollege.ClampAngle180(base.m_aiActor.aiAnimator.FacingDirection) > 0f;

		beamCont.HeightOffset = 0f;
		beamCont.RampHeightOffset = 0;

		beamCont.ContinueBeamArtToWall = true;

		beamCont.OverrideHitChecks = delegate (SpeculativeRigidbody hitRigidbody, Vector2 dirVec)
		{
			HealthHaver healthHaver = (!hitRigidbody) ? null : hitRigidbody.healthHaver;

			if (healthHaver != null)
			{
				if (healthHaver.aiActor)
				{
					if (enemyTickCooldown <= 0f)
					{
						Projectile currentProjectile = aibeamShooter2.beamModule.GetCurrentProjectile();
						healthHaver.ApplyDamage(ProjectileData.FixedFallbackDamageToEnemies, dirVec, this.m_aiActor.GetActorName(), currentProjectile.damageTypes, DamageCategory.Normal, false, null, false);
						enemyTickCooldown = aibeamShooter2.beamModule.cooldownTime;
					}
				}
				else
				{
					Projectile currentProjectile2 = aibeamShooter2.beamModule.GetCurrentProjectile();
					healthHaver.ApplyDamage(currentProjectile2.baseData.damage, dirVec, this.m_aiActor.GetActorName(), currentProjectile2.damageTypes, DamageCategory.Normal, false, null, false);
				}
			}
			if (hitRigidbody.majorBreakable)
			{
				hitRigidbody.majorBreakable.ApplyDamage(26f * BraveTime.DeltaTime, dirVec, false, false, false);
			}
		};

		yield return new WaitForSeconds(1);
		aibeamShooter2.shouldRotate = false;
		bool firstFrame = true;
		while (beamCont != null && this.IsfiringLaser)
		{

			enemyTickCooldown = Mathf.Max(enemyTickCooldown - BraveTime.DeltaTime, 0f);
			float clampedAngle;


			//clampedAngle = BraveMathCollege.ClampAngle360((j == 1 ? -this.AlaserAngle : this.AlaserAngle )) + (180 * j);
			clampedAngle = BraveMathCollege.ClampAngle360((j == 1 || j == 3 ? this.AlaserAngle2 : this.AlaserAngle)) + (j == 1 ? (180) : (0));

			Vector3 dirVec2 = new Vector3(Mathf.Cos(clampedAngle * 0.0174532924f), Mathf.Sin(clampedAngle * 0.0174532924f), 0f) * 10f;
			Vector2 startingPoint = aibeamShooter2.beamTransform.position;
			//ETGModConsole.Log("Enemy:" + m_aiActor.transform.position.x.ToString() + "," + m_aiActor.transform.position.y.ToString());
			//ETGModConsole.Log("Beam:" + aibeamShooter2.transform.position.x.ToString() + "," + aibeamShooter2.transform.position.y.ToString());
			//float tanAngle = Mathf.Tan(clampedAngle * 0.0174532924f);
			//float sign = (float)((clampedAngle <= 90f || clampedAngle >= 270f) ? 1 : -1);
			//float denominator = Mathf.Sqrt(this.firingEllipseB * this.firingEllipseB + this.firingEllipseA * this.firingEllipseA * (tanAngle * tanAngle));
			//startingPoint.x += sign * this.firingEllipseA * this.firingEllipseB / denominator;
			//startingPoint.y += sign * this.firingEllipseA * this.firingEllipseB * tanAngle / denominator;
			beamCont.Origin = startingPoint + this.m_aiActor.sprite.WorldCenter;
			beamCont.Direction = dirVec2;
			bool flag5 = firstFrame;
			if (flag5)
			{
				yield return null;
				//if (firstFrame) yield return new WaitForSeconds(1);
				firstFrame = false;
			}
			else
			{
				//beamCont.Direction = dirVec2;

				//facingNorth = (BraveMathCollege.ClampAngle180(base.m_aiActor.aiAnimator.FacingDirection) > 0f);
				//beamCont.RampHeightOffset = (float)((!facingNorth) ? 0 : 0);
				beamCont.LateUpdatePosition(startingPoint);
				yield return null;
				if (this.IsfiringLaser && !beamCont)
				{
					this.StopFiringLaser();
					break;
				}
				while (Time.timeScale == 0f)
				{
					yield return null;
				}
			}
		}


		if (!this.IsfiringLaser && beamCont != null)
		{
			beamCont.DestroyBeam();
			beamCont = null;
		}


		yield break;
	}
	private void CleanupReticles()
	{
		for (int i = 0; i < this.m_reticles.Count; i++)
		{
			SpawnManager.Despawn(this.m_reticles[i]);
		}
		this.m_reticles.Clear();
	}

	public float initialAimOffset;

	public float chargeTime;

	public float firingTime;

	public float maxTurnRate;
	public float turnRateMultiplier;

	public float turnRateAcceleration;

	private RhulkAttackIStole.State m_state;

	private float m_timer;

	private Vector2 m_targetPosition;

	private SpeculativeRigidbody m_backupTarget;

	private BulletScriptSource m_bulletSource;
	public bool HasTriggeredScript;

	public BulletScriptSelector BulletScript;
	public Transform ShootPoint;


	public AIBeamShooter2 beam1;
	public AIBeamShooter2 beam2;

	private enum State
	{
		PreCharging,
		Charging,
		Firing
	}
	//=====
	public string ChargeAnimation;
	public string FireAnimation;
	public string PostFireAnimation;

	public ShootBeamBehavior.BeamSelection beamSelection;
	public AIBeamShooter specificBeamShooter;
	private List<AIBeamShooter> m_allBeamShooters;
	public List<AIBeamShooter2> beamToUse;
	//private readonly List<AIBeamShooter> m_currentBeamShooters = new List<AIBeamShooter>();
	//private List<AIBeamShooter2> m_currentBeamShooters2 = new List<AIBeamShooter2>();

	private List<GameObject> m_reticles = new List<GameObject>();

	public string LaserFiringSound;
	public string StopLaserFiringSound;
	public string EnemyChargeSound;
	public string BeamChargingSound;
	public bool UsesBaseSounds;
	public bool UsesBeamProjectileWithoutModule;

	public bool LockInPlaceWhileAttacking;

	public static GameObject tellThing;
}