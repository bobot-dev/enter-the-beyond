using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using FrostAndGunfireItems;

namespace BotsMod
{
	class DeadEye : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:dead_eye";
		private static tk2dSpriteCollectionData DeadEyeCollection;


		public static void Init()
		{
			//BotsMod.BotsModule.Log("hello???? can you run the code or not?");
			DeadEye.BuildPrefab();
			//BotsMod.BotsModule.Log("yes");
		}

		public static void BuildPrefab()
		{
			try
			{
				//BotsMod.BotsModule.Log("maybe");
				if (prefab == null || !EnemyBuilder.Dictionary.ContainsKey(guid))
				{
					//BotsMod.BotsModule.Log("some what");
					AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d");
					prefab = EnemyBuilder.BuildPrefab("DeadEye", guid, "BotsMod/sprites/Enemies/DeadEye/Idle/dead_eye_idle_front_001.png", new IntVector2(10, 5), new IntVector2(14, 34), true);
					var companion = prefab.AddComponent<EnemyBehavior>();

					companion.aiActor.knockbackDoer.weight = 800;
					companion.aiActor.MovementSpeed = 0f;
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
					companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").ShadowObject;


					companion.aiActor.specRigidbody.PixelColliders.Clear();



					companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

					{
						ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
						CollisionLayer = CollisionLayer.EnemyCollider,
						IsTrigger = false,
						BagleUseFirstFrameOnly = false,
						SpecifyBagelFrame = string.Empty,
						BagelColliderNumber = 0,
						ManualOffsetX = 6,
						ManualOffsetY = 1,
						ManualWidth = 14,
						ManualHeight = 34,
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
						ManualOffsetX = 6,
						ManualOffsetY = 1,
						ManualWidth = 14,
						ManualHeight = 34,
						ManualDiameter = 0,
						ManualLeftX = 0,
						ManualLeftY = 0,
						ManualRightX = 0,
						ManualRightY = 0,



					});
					companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").CorpseObject;
					companion.aiActor.PreventBlackPhantom = false;

					companion.gameObject.AddAnimation("dead_eye", "idle", "BotsMod/sprites/Enemies/DeadEye/Idle", 6, AnimationType.Idle, DirectionalAnimation.DirectionType.SixWay, DirectionalAnimation.FlipType.Mirror, tk2dSpriteAnimationClip.WrapMode.Loop);
					companion.gameObject.AddAnimation("teleport", "BotsMod/sprites/Enemies/DeadEye/StartTeleport", 6, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
					companion.gameObject.AddAnimation("teleport_end", "BotsMod/sprites/Enemies/DeadEye/EndTeleport", 6, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;


					var bs = prefab.GetComponent<BehaviorSpeculator>();
					BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").behaviorSpeculator;

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
							WeaponType = WeaponType.BulletScript,
							OverrideBulletName = "sniper",
							BulletScript = new CustomBulletScriptSelector(typeof(SniperScript)),
							FixTargetDuringAttack = true,
							StopDuringAttack = true,
							LeadAmount = 0,
							LeadChance = 1,
							RespectReload = true,
							MagazineCapacity = 1,
							ReloadSpeed = 0.78f,
							EmptiesClip = true,
							SuppressReloadAnim = false,
							TimeBetweenShots = 0.8f,
							PreventTargetSwitching = true,
							OverrideAnimation = null,
							OverrideDirectionalAnimation = null,
							HideGun = false,
							UseLaserSight = true,
							UseGreenLaser = false,
							PreFireLaserTime = 1f,
							AimAtFacingDirectionWhenSafe = false,
							Cooldown = 3f,
							CooldownVariance = 0,
							AttackCooldown = 0,
							GlobalCooldown = 0,
							InitialCooldown = 0,
							InitialCooldownVariance = 0,
							GroupName = null,
							GroupCooldown = 0,
							MinRange = 0,
							Range = 32,
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
						},
						/*new TeleportBehavior
						{
							AttackableDuringAnimation = true,
							AllowCrossRoomTeleportation = false,
							teleportRequiresTransparency = false,
							hasOutlinesDuringAnim = true,
							ManuallyDefineRoom = false,
							MaxHealthThreshold = 1f,
							StayOnScreen = false,
							AvoidWalls = true,
							GoneTime = 0.7f,
							OnlyTeleportIfPlayerUnreachable = false,
							MinDistanceFromPlayer = 0f,
							MaxDistanceFromPlayer = -1f,
							teleportInAnim = "teleport",
							teleportOutAnim = "teleport_end",
							AttackCooldown = 1f,
							InitialCooldown = 0f,
							RequiresLineOfSight = false,
							roomMax = new Vector2(0,0),
							roomMin = new Vector2(0,0),

							GlobalCooldown = 0,
							Cooldown = 3f,
							CooldownVariance = 0f,
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
							targetAreaStyle = null,
							HealthThresholds = new float[0],
							MinWallDistance = 0,
						}*/
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
					m_CachedGunAttachPoint.transform.localPosition = new Vector3(0.5625f, 0.875f, 0);
					EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), BotsItemIds.BeyondSniper, m_CachedGunAttachPoint.transform, overrideHandObject: BeyondPrefabs.basicBeyondHands);
					prefab.GetComponent<AIBulletBank>().Bullets = new List<AIBulletBank.Entry>();
					prefab.GetComponent<AIBulletBank>().Bullets.Add(EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").bulletBank.GetBullet("sniper"));
					var shooter = prefab.GetComponent<AIShooter>();

					shooter.AllowTwoHands = true;
					shooter.ForceGunOnTop = true;


					UnityEngine.Object.Destroy(prefab.transform.Find("BulletManHand(Clone)").gameObject);

					Game.Enemies.Add("bot:dead_eye", companion.aiActor);
					//ETGModConsole.Log("cool");
				}
			}
			catch (Exception message)
			{
				ETGModConsole.Log("AAAAAAAAAAAA: " + message);

			}		
		}
		public class SniperScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				AkSoundEngine.PostEvent("Play_WPN_sniperrifle_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").bulletBank.GetBullet("sniper"));
				}
				for (int i = 0; i < 2; i++)
				{
					this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(40f, SpeedType.Absolute), new SniperBullet());
					yield return Wait(5);
				}
				this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(40f, SpeedType.Absolute), new SniperBullet(true));
				yield break;
			}
		}

		public class SniperBullet : Bullet
		{
			public SniperBullet(bool burst = false) : base("sniper", false, false, false)
			{
				doBurst = burst;
			}
			bool doBurst;
			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (!preventSpawningProjectiles && doBurst)
				{
					float num = base.RandomAngle();
					float num2 = 30f;
					for (int i = 0; i < 12; i++)
					{
						base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new SniperBullet());
					}
				}
			}
		}


		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;
			private void Start()
			{
				Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
				mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
				mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
				mat.SetFloat("_EmissiveColorPower", 1.55f);
				mat.SetFloat("_EmissivePower", 50);
				aiActor.sprite.renderer.material = mat;
				base.aiActor.OverrideBlackPhantomShader = BeyondPrefabs.BeyondJammedShader;
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
