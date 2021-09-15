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

namespace BotsMod
{
	public class BeyondScout : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:beyond_scout";
		private static tk2dSpriteCollectionData BeyondScoutCollection;


		public static void Init()
		{

				BeyondScout.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e");
				prefab = EnemyBuilder.BuildPrefab("BeyondScout", guid, "BotsMod/sprites/tempScout", new IntVector2(0, 0), new IntVector2(8, 9), true);
				var companion = prefab.AddComponent<EnemyBehavior>();;
				
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 2f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = true;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = false;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				companion.aiActor.CollisionKnockbackStrength = 5f;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(55f, null, false);
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;

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

				companion.gameObject.AddAnimation("idle", "BotsMod/sprites/tempScout", 3, AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

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
					new TeleportBehavior()
					{

							AttackableDuringAnimation = true,
							AllowCrossRoomTeleportation = true,
							teleportRequiresTransparency = false,
							hasOutlinesDuringAnim = true,
							ManuallyDefineRoom = false,
							MaxHealthThreshold = 1f,
							StayOnScreen = false,
							AvoidWalls = true,
							GoneTime = 1f,
							OnlyTeleportIfPlayerUnreachable = false,
							MinDistanceFromPlayer = 4f,
							MaxDistanceFromPlayer = -1f,
							teleportInAnim = "",
							teleportOutAnim = "",
							AttackCooldown = 1f,
							InitialCooldown = 0f,
							RequiresLineOfSight = false,
							roomMax = new Vector2(0,0),
							roomMin = new Vector2(0,0),
							
							GlobalCooldown = 0.5f,
							Cooldown = 4f,
							CooldownVariance = 1f,
							InitialCooldownVariance = 0f,
							goneAttackBehavior = null,
							IsBlackPhantom = false,


							GroupName = null,
							GroupCooldown = 0,
							MinRange = 0,
							Range = 0,
							MinHealthThreshold = 0,
							MaxEnemiesInRoom = 1,
							MaxUsages = 0,
							AccumulateHealthThresholds = true,
							//shadowInAnim = null,
							//shadowOutAnim = null,
							targetAreaStyle = null,
							HealthThresholds = new float[0],
							MinWallDistance = 0,
						//resetCooldownOnDamage = null,
						//shadowSupport = (TeleportBehavior.ShadowSupport)1,
					},


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
						TimeBetweenShots = 0.3f,
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
			
				Game.Enemies.Add("bot:beyond_scout", companion.aiActor);


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

	





	

