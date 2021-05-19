using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using FrostAndGunfireItems;
//using EnemyAPI;

namespace BotsMod
{
	class PirmalShotgrub : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:primalshotgrub";
		private static tk2dSpriteCollectionData PrimalShotgrubCollection;


		public static void Init()
		{
			PirmalShotgrub.BuildPrefab();
		}

		public static void BuildPrefab()
		{

			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{

				//AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c");

				//AIActor aiactor = NotABotsEnemies.GetOrLoadByGuid_Orig("044a9f39712f456597b9762893fbc19c");
				AIActor aIActor = EnemyDatabase.Instance.InternalGetByGuid("044a9f39712f456597b9762893fbc19c");
				
				prefab = EnemyBuilder.BuildPrefab("PrimalShotgrub", guid, spritePaths[1], new IntVector2(0, 0), new IntVector2(8, 9), true);
				EnemyBehavior companion = prefab.AddComponent<EnemyBehavior>(); ;
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 15f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(55f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();

				companion.healthHaver.spawnBulletScript = true;
				companion.healthHaver.chanceToSpawnBulletScript = 1f;
				companion.healthHaver.bulletScriptType = HealthHaver.BulletScriptType.OnPreDeath;

				companion.healthHaver.bulletScript = new CustomBulletScriptSelector(typeof(PrimalShotgrubDeathScrip));



				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider


				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 15,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});

				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{
								"die_right",
								"die_left"

							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
						{
						"run_right",
						"run_left"
						}
				};

				ETGModConsole.Log("3");

				bool flag3 = PrimalShotgrubCollection == null;
				if (flag3)
				{
					PrimalShotgrubCollection = SpriteBuilder.ConstructCollection(prefab, "PrimalShotgrub_Collection");
					UnityEngine.Object.DontDestroyOnLoad(PrimalShotgrubCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], PrimalShotgrubCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

					0,
					1,
					2,
					3

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

					4,
					5,
					6,
					7


					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

					8,
					9,
					10,
					11,
					12,
					13

					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

					14,
					15,
					16,
					17,
					18,
					19


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

					 20,
					 21,
					 22,
					 23,
					 24,
					 25



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, PrimalShotgrubCollection, new List<int>
					{

						26,
						27,
						28,
						29,
						30,
						31,

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;

				}

				ETGModConsole.Log("4");

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").behaviorSpeculator;

				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;


				bs.TargetBehaviors = new List<TargetBehaviorBase>
				{
					new TargetPlayerBehavior
					{
						Radius = 35f,
						LineOfSight = true,
						ObjectPermanence = true,
						SearchInterval = 0.25f,
						PauseOnTargetSwitch = false,
						PauseTime = 0.25f
					}
				};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootGunBehavior() {
					GroupCooldownVariance = 0.2f,
					LineOfSight = true,
					WeaponType = WeaponType.AIShooterProjectile,
					OverrideBulletName = null,
					//BulletScript = new CustomBulletScriptSelector(typeof(PrimalShotgrubScrip)),
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
					MaxUsages = 0

				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{


				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 6f,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//ETGModConsole.Log("5");
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("206405acad4d4c33aac6717d184dc8d4").behaviorSpeculator;
				//Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("GunAttachPoint").gameObject;


				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), 151, m_CachedGunAttachPoint.transform, null, null);
				//EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), 126, m_CachedGunAttachPoint.transform);

				var horribleMistake = prefab.AddComponent<LostsCloak>();
				//horribleMistake.quality = PickupObject.ItemQuality.S;
				//LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(531).gameObject, horribleMistake, true);



				Game.Enemies.Add("bot:primalshotgrub", companion.aiActor);
				//ETGModConsole.Log("6");

				BotsModule.WarCrime = prefab;
				BotsModule.WarCrime2 = horribleMistake;

			}
		}



		private static string[] spritePaths = new string[]
		{

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_left_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_left_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_left_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_left_004",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_right_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_right_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_right_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_idle_right_004",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_004",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_005",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_left_006",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_004",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_005",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_move_front_right_006",



			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_004",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_005",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_left_006",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_004",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_005",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_die_right_006",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_hit_001",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_corpse_001",



			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_pitfall_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_pitfall_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_pitfall_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_pitfall_004",

			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_spawn_001",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_spawn_002",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_spawn_003",
			"BotsMod/sprites/PrimalShotGrub/shotgun_grub_spawn_004"

		};

		public class PrimalShotgrubScrip : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{


			protected override IEnumerator Top()
			{

				//AkSoundEngine.PostEvent("Play_WPN_stickycrossbow_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					//base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}

				//float num2 = -22.5f;
				float num2 = 0;
				float num3 = 9f;
				for (int i = 0; i < 40; i++)
				{
					base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new ShotgrubManAttack1.GrossBullet(num2 + (float)i * num3));
				}

				yield return base.Wait(20);

				//float num = -45f;
				float desiredAngle = this.GetAimDirection(1f, 12f);
				float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
				bool isBlackPhantom = this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.IsBlackPhantom;
				//Bullet bullet = new BurstingBullet(isBlackPhantom, num);


				for (int i = 0; i < 3; i++)
				{
					float num = -45f + (float)i * 30f;
					base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Aim), new Direction(num, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom));
				}

				yield return null;
			}
			public class BurstingBullet : Bullet
			{
				// Token: 0x060006C0 RID: 1728 RVA: 0x0001F513 File Offset: 0x0001D713
				public BurstingBullet(bool isBlackPhantom) : base("bigBullet", false, false, false)
				{

					//this.deltaAngle = deltaAngle;
					base.ForceBlackBullet = true;
					this.m_isBlackPhantom = isBlackPhantom;

				}

				private float deltaAngle;


				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
				{
					if (preventSpawningProjectiles)
					{
						return;
					}
					float num = base.RandomAngle();
					float num2 = 20f;
					for (int i = 0; i < 18; i++)
					{
						//Bullet bullet = new Bullet(null, false, false, false);

						float num3 = -22.5f;
						float num4 = 9f;

						ShotgrubManAttack1.GrossBullet bullet = new ShotgrubManAttack1.GrossBullet(num3 + (float)i * num4);
						base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), bullet);
						if (!this.m_isBlackPhantom)
						{
							bullet.ForceBlackBullet = false;
							bullet.Projectile.ForceBlackBullet = false;
							bullet.Projectile.ReturnFromBlackBullet();

						}
					}
				}

				// Token: 0x04000697 RID: 1687
				private const int NumBullets = 18;

				// Token: 0x04000698 RID: 1688
				private bool m_isBlackPhantom;
			}

		}
			public class PrimalShotgrubDeathScrip : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
			{


				protected override IEnumerator Top()
				{

					//AkSoundEngine.PostEvent("Play_WPN_stickycrossbow_shot_01", this.BulletBank.aiActor.gameObject);
					if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
					{
						//base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
						base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
						base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross"));
						base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
					}


				

				//float num = -45f;
				float desiredAngle = this.GetAimDirection(1f, 12f);
					float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
					bool isBlackPhantom = this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.IsBlackPhantom;
					//Bullet bullet = new BurstingBullet(isBlackPhantom, num);


					for (int i = 0; i < 12; i++)
					{
						float num = (float)i * 30f;
						base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Aim), new Direction(num, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom));
					}

					yield return null;
				}

				public class BurstingBullet : Bullet
				{
					// Token: 0x060006C0 RID: 1728 RVA: 0x0001F513 File Offset: 0x0001D713
					public BurstingBullet(bool isBlackPhantom) : base("bigBullet", false, false, false)
					{
						//this.deltaAngle = deltaAngle;
						base.ForceBlackBullet = true;
						this.m_isBlackPhantom = isBlackPhantom;

					}

					private float deltaAngle;


					public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
					{
						if (preventSpawningProjectiles)
						{
							return;
						}
						float num = base.RandomAngle();
						float num2 = 20f;
						for (int i = 0; i < 18; i++)
						{
							//Bullet bullet = new Bullet(null, false, false, false);

							float num3 = -22.5f;
							float num4 = 9f;

							ShotgrubManAttack1.GrossBullet bullet = new ShotgrubManAttack1.GrossBullet(num3 + (float)i * num4);
							base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), bullet);
							if (!this.m_isBlackPhantom)
							{
								bullet.ForceBlackBullet = false;
								bullet.Projectile.ForceBlackBullet = false;
								bullet.Projectile.ReturnFromBlackBullet();

							}
						}
					}

					// Token: 0x04000697 RID: 1687
					private const int NumBullets = 18;

					// Token: 0x04000698 RID: 1688
					private bool m_isBlackPhantom;
				}

			}

			public class EnemyBehavior : BraveBehaviour
			{

				private RoomHandler m_StartRoom;
				private void Update()
				{
					if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
				}
				private void CheckPlayerRoom()
				{

					if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
					{
						GameManager.Instance.StartCoroutine(LateEngage());
					}

				}

				private IEnumerator LateEngage()
				{
					yield return new WaitForSeconds(0.5f);
					base.aiActor.HasBeenEngaged = true;
					yield break;
				}
				private void Start()
				{
					m_StartRoom = aiActor.GetAbsoluteParentRoom();
					//base.aiActor.HasBeenEngaged = true;
					base.aiActor.healthHaver.OnPreDeath += (obj) =>
					{
						AkSoundEngine.PostEvent("Play_OBJ_skeleton_collapse_01", base.aiActor.gameObject);
					};
				}


			}


		}
	}
