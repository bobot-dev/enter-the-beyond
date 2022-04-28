using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = EnemyAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using EnemyAPI;

namespace BotsMod
{
	public class BeyondKin : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:beyond_kin";
		private static tk2dSpriteCollectionData BeyondKinCollection;


		public static void Init()
		{

				BeyondKin.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e");
				prefab = EnemyBuilder.BuildPrefab("BeyondKin", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), true);
				var companion = prefab.AddComponent<EnemyBehavior>();;
				
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 2f;
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
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e").CorpseObject;
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
				aiAnimator.HitAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"hit_right",
						"hit_left"
					}
				};

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayVertical,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_front",
						"idle_back"
					}
				};

				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[3],
					AnimNames = new string[]
					{

						"move_back_right",
						"move_front_right",

						"move_front_left",
						//"move_front_left",
						"move_back_left",
						
					}
				};
				
				bool flag3 = BeyondKinCollection == null;
				if (flag3)
				{
					BeyondKinCollection = SpriteBuilder.ConstructCollection(prefab, "Beyond_Kin_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BeyondKinCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BeyondKinCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						0,
						1

					}, "idle_back", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						2,
						3
						

					}, "idle_front", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						10,
						11,
						12,
						13,
						14,
						15


					}, "move_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						22,
						23,
						24,
						25,
						26,
						27


					}, "move_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						4,
						5,
						6,
						7,
						8,
						9

					}, "move_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						16,
						17,
						18,
						19,
						20,
						21


					}, "move_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;






					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						 28,
						 29,
						 30,



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						31,
						32,
						33

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						35


					}, "hit_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, BeyondKinCollection, new List<int>
					{

						34


					}, "hit_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
				}

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e").behaviorSpeculator;

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
					LineOfSight = false,
					WeaponType = WeaponType.AIShooterProjectile,
					OverrideBulletName = "default",	
					BulletScript = new CustomBulletScriptSelector(typeof(SkellScript)),
					FixTargetDuringAttack = true,
					StopDuringAttack = true,
					LeadAmount = 0,
					LeadChance = 1,
					RespectReload = true,
					MagazineCapacity = 6,
					ReloadSpeed = 0.78f,
					EmptiesClip = true,
					SuppressReloadAnim = false,
					TimeBetweenShots = -1,
					PreventTargetSwitching = true,
					OverrideAnimation = null,
					OverrideDirectionalAnimation = null,
					HideGun = false,
					UseLaserSight = false,
					UseGreenLaser = false,
					PreFireLaserTime = -1,
					AimAtFacingDirectionWhenSafe = false,
					Cooldown = 0.07f,
					CooldownVariance = 0,
					AttackCooldown = 0,
					GlobalCooldown = 0,
					InitialCooldown = 0,
					InitialCooldownVariance = 0,
					GroupName = null,
					GroupCooldown = 0,
					MinRange = 0,
					Range = 16,
					MinWallDistance = 0,
					MaxEnemiesInRoom = 0,
					MinHealthThreshold = 0,
					MaxHealthThreshold = 1,
					HealthThresholds = new float[0],
					AccumulateHealthThresholds = true,
					targetAreaStyle = null,
					IsBlackPhantom = false,
					resetCooldownOnDamage = null,
					RequiresLineOfSight = true,
					MaxUsages = 0,

				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 7f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
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
				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), 62, m_CachedGunAttachPoint.transform);
			
				Game.Enemies.Add("bot:beyond_kin", companion.aiActor);


			}
		}
		public class SkellScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				AkSoundEngine.PostEvent("Play_WPN_stickycrossbow_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1a78cfb776f54641b832e92c44021cf2").bulletBank.GetBullet("sweep"));
				}
				for (int i = -2; i <= 2; i++)
				{
					this.Fire(new Direction((float)(i * 9), DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new SkellBullet());
				}
				yield break;
			}
		}


		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{

			}
		}


		private static string[] spritePaths = new string[]
		{
			
			//idles
			"BotsMod/sprites/BeyondKin/ash_bulletman_idle_front_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_idle_front_002",//1

			"BotsMod/sprites/BeyondKin/ash_bulletman_idle_back_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_idle_back_002",//3
			//run
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_002",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_003",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_004",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_005",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_left_006",//9

			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_002",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_003",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_004",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_005",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_back_right_006",//15

			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_002",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_003",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_004",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_005",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_left_006",//21

			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_002",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_003",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_004",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_005",
			"BotsMod/sprites/BeyondKin/ash_bulletman_move_front_right_006",//27

			//death
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",//33
			//hit
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_left_001",
			"BotsMod/sprites/BeyondKin/ash_bulletman_hit_right_001",//35
				};

		public class EnemyBehavior : BraveBehaviour
		{
			
			private RoomHandler m_StartRoom;
			private void Update() {
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

	





	

