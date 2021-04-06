using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using FrostAndGunfireItems;
using static BotsMod.PirmalShotgrub;

namespace BotsMod
{
	public class BabyGoodMistake : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "bot:ministake"; //give your companion some unique guid
		float damageBuff = -1;

		public static void Init()
		{
			string itemName = "Ministake";
			string resourceName = "BotsMod/sprites/mistakecharm";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<BabyGoodMistake>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "wip";
			string longDesc = "These strange creatures will apear if they find your skills in combat exaptable, all though they will weaken you in exchange for there deffence.";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.C;
			item.CompanionGuid = guid; //this will be used by the item later to pull your companion from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
			//item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 5f);
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, -0.05f, StatModifier.ModifyMethod.ADDITIVE);
			item.CompanionGuid = BabyGoodMistake.guid;
			item.CanBeDropped = false;
			BuildPrefab();
			item.PlaceItemInAmmonomiconAfterItemById(664);

		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			//prefab = CompanionBuilder.BuildPrefab("mistake", guid, "BotsMod/sprites/Pets/mistake/Idle_front/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
			prefab = CompanionBuilder.BuildPrefab("mistake", guid, "BotsMod/sprites/ministake/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a companion component to the prefab (could be a custom class)
			var companion = prefab.AddComponent<CompanionController>();
			companion.aiActor.MovementSpeed = 5f;

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			//prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_right", "BotsMod/sprites/ministake/idle_right", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle_left", "BotsMod/sprites/ministake/idle_right", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that

			AIActor aIActor = EnemyDatabase.Instance.InternalGetByGuid("3a077fa5872d462196bb9a3cb1af02a3");

			//companion.aiActor.CompanionSettings

			var bs = prefab.GetComponent<BehaviorSpeculator>();

			bs.MovementBehaviors.Add(new BotsCompanionFollowPlayerBehavior() {

				PathInterval = 0.25f,
				DisableInCombat = true,
				IdealRadius = 3,
				CatchUpRadius = 8,
				CatchUpAccelTime = 5,
				CatchUpSpeed = 5,
				CatchUpMaxSpeed = 10,
				//CatchUpAnimation = "idle",
				//CatchUpOutAnimation = "idle",
				CanRollOverPits = false,
				TemporarilyDisabled = false,

				IdleAnimations = new string[] { "idle" }

			});
			bs.MovementBehaviors.Add(new SeekTargetBehavior()
			{
				StopWhenInRange = true,
				CustomRange = 7,
				LineOfSight = true,
				ReturnToSpawn = true,
				SpawnTetherDistance = 0,
				PathInterval = 0.25f,
				ExternalCooldownSource = false,
				SpecifyRange = false,
				MaxActiveRange = 0,
				MinActiveRange = 0

			});

			ShootGunBehavior shootGunBehavior2 = new ShootGunBehavior()
			{
				GroupCooldownVariance = 0.2f,
				LineOfSight = true,
				WeaponType = WeaponType.BulletScript,
				OverrideBulletName = "default_test",
				BulletScript = new CustomBulletScriptSelector(typeof(PrimalShotgrubScrip)),
				FixTargetDuringAttack = false,
				StopDuringAttack = true,
				LeadAmount = 0f,
				LeadChance = 1f,
				RespectReload = true,
				MagazineCapacity = 1f,
				ReloadSpeed = 3f,
				EmptiesClip = false,
				SuppressReloadAnim = false,
				TimeBetweenShots = -1f,
				PreventTargetSwitching = false,
				OverrideAnimation = null,
				OverrideDirectionalAnimation = null,
				HideGun = false,
				UseLaserSight = false,
				UseGreenLaser = false,
				PreFireLaserTime = -1f,
				AimAtFacingDirectionWhenSafe = false,
				Cooldown = 3.5f,
				CooldownVariance = 0f,
				AttackCooldown = 0f,
				GlobalCooldown = 0f,
				InitialCooldown = 0f,
				InitialCooldownVariance = 0f,
				GroupName = null,
				GroupCooldown = 0f,
				MinRange = 0f,
				Range = 20f,
				MinWallDistance = 0f,
				MaxEnemiesInRoom = 0f,
				MinHealthThreshold = 0f,
				MaxHealthThreshold = 1f,
				HealthThresholds = new float[0],
				AccumulateHealthThresholds = true,
				targetAreaStyle = null,
				IsBlackPhantom = false,
				resetCooldownOnDamage = null,
				RequiresLineOfSight = false,
				MaxUsages = 0,

			};

			ShootGunBehavior shootGunBehavior = (new ShootGunBehavior() 
			{ 
				GroupCooldownVariance = 0,
				LineOfSight = true,
				WeaponType = WeaponType.AIShooterProjectile,
				OverrideBulletName = "default_test",
				FixTargetDuringAttack = false,
				StopDuringAttack = false,
				LeadAmount = 0.5f,
				LeadChance = 1,
				RespectReload = true,
				MagazineCapacity = 32,
				ReloadSpeed = 2,
				EmptiesClip = true,
				SuppressReloadAnim = false,
				TimeBetweenShots = 0.04f,
				PreventTargetSwitching = false,
				HideGun = false,
				UseLaserSight = true,
				UseGreenLaser = true,
				PreFireLaserTime = 0.5f,
				AimAtFacingDirectionWhenSafe = true,
				Cooldown = 0f,
				CooldownVariance = 0,
				AttackCooldown = 0,
				GlobalCooldown = 0,
				InitialCooldown = 0,
				InitialCooldownVariance = 0,
				//GroupName = "",
				GroupCooldown = 0,
				MinRange = 0,
				Range = 0,
				MinWallDistance = 0,
				MaxEnemiesInRoom = 0,
				MaxHealthThreshold = 1,
				AccumulateHealthThresholds = true,
				IsBlackPhantom = true,
				RequiresLineOfSight = true,

			});
			

			//companion.CanInterceptBullets = true;
			//companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = true;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = false;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(1f);
			companion.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
			companion.aiActor.specRigidbody.PixelColliders.Clear();
			companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				//CollisionLayer = CollisionLayer.PlayerCollider,
				CollisionLayer = CollisionLayer.EnemyBulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});

			var shootpoint = new GameObject("attach");
			shootpoint.transform.parent = companion.transform;
			shootpoint.transform.position = companion.sprite.WorldCenter;
			GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;

			bs.AttackBehaviors.Add(shootGunBehavior);



			EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiActor.aiShooter, aIActor.aiActor.GetComponent<AIBulletBank>(), 299, m_CachedGunAttachPoint.transform, null, null);

			var bullet = (PickupObjectDatabase.GetById(100) as Gun).DefaultModule.projectiles[0];
			companion.aiActor.bulletBank.Bullets.Add(new AIBulletBank.Entry
			{
				Name = "default_test",
				BulletObject = bullet.gameObject,
				OverrideProjectile = true,
				ProjectileData = new ProjectileData
				{
					damage = bullet.baseData.damage,
					speed = bullet.baseData.speed,
					range = bullet.baseData.range,
					force = bullet.baseData.force,
					damping = bullet.baseData.damping,
					UsesCustomAccelerationCurve = bullet.baseData.UsesCustomAccelerationCurve,
					AccelerationCurve = bullet.baseData.AccelerationCurve,
					CustomAccelerationCurveDuration = bullet.baseData.CustomAccelerationCurveDuration,
					onDestroyBulletScript = bullet.baseData.onDestroyBulletScript,
					IgnoreAccelCurveTime = bullet.baseData.IgnoreAccelCurveTime
				},
				PlayAudio = false,
				AudioSwitch = "",
				AudioEvent = "",
				AudioLimitOncePerFrame = false,
				AudioLimitOncePerAttack = false,
				MuzzleFlashEffects = new VFXPool
				{
					effects = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].MuzzleFlashEffects.effects,
					type = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].MuzzleFlashEffects.type
				},
				MuzzleLimitOncePerFrame = true,
				MuzzleInheritsTransformDirection = false,
				ShellTransform = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellTransform,
				ShellPrefab = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellPrefab,
				ShellForce = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellForce,
				ShellForceVariance = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellForceVariance,
				DontRotateShell = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].DontRotateShell,
				ShellGroundOffset = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellGroundOffset,
				ShellsLimitOncePerFrame = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].ShellsLimitOncePerFrame,
				rampBullets = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].rampBullets,
				conditionalMinDegFromNorth = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].conditionalMinDegFromNorth,
				forceCanHitEnemies = true,
				suppressHitEffectsIfOffscreen = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].suppressHitEffectsIfOffscreen,
				preloadCount = aIActor.aiActor.GetComponent<AIBulletBank>().Bullets[0].preloadCount
			});


		}
		protected override void Update()
		{
			base.Update();
		}
		public override void Pickup(PlayerController player)
		{
			
			base.Pickup(player);
		}


		// Token: 0x060001B4 RID: 436 RVA: 0x00010334 File Offset: 0x0000E534
		private void CreateNewCompanion(PlayerController player)
		{


			//AkSoundEngine.PostEvent("Play_OBJ_smallchest_spawn_01", base.gameObject);
			bool flag = this.companionsSpawned.Count + 1 == this.MaxNumberOfCompanions;
			bool flag2 = flag;
			bool flag3 = !flag2;
			if (flag3)
			{

				bool flag4 = this.companionsSpawned.Count >= this.MaxNumberOfCompanions;
				bool flag5 = !flag4;
				bool flag6 = flag5;
				if (flag6)
				{

					float curDamage = base.Owner.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
					float newDamage = curDamage - 0.05f;
					base.Owner.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, base.Owner);
					damageBuff = newDamage - curDamage;

					AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
					Vector3 vector = player.transform.position;
					bool flag7 = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
					bool flag8 = flag7;
					bool flag9 = flag8;
					if (flag9)
					{
						vector += new Vector3(1.125f, -0.3125f, 0f);
					}
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
					CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
					this.companionsSpawned.Add(orAddComponent);
					orAddComponent.Initialize(player);
					bool flag10 = orAddComponent.specRigidbody;
					bool flag11 = flag10;
					bool flag12 = flag11;
					if (flag12)
					{
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
					}
					orAddComponent.aiAnimator.PlayUntilFinished("spawn", false, null, -1f, false);
				}
			}
		}
		float killCount = 0;
		public int MaxNumberOfCompanions = 10;
		private List<CompanionController> companionsSpawned = new List<CompanionController>();
	}
}







