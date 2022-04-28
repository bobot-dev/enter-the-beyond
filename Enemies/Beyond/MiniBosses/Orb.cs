using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;

using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;

using static BotsMod.RandomComps;
using EnemyAPI;

namespace BotsMod
{

	class Orb : AIActor
	{
		public static GameObject orbPrefab;
		public static readonly string guid = "bot:anomaly";
		public static GameObject shootpoint;
		
		public static void Init()
		{
			Orb.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = orbPrefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{

				orbPrefab = BossBuilder.BuildPrefab("Anomaly", guid, "BotsMod/sprites/Enemies/Orb/the_orb_001", new IntVector2(0, 0), new IntVector2(8, 9), false, true);

				orbPrefab.layer = 28;

				var orb = UnityEngine.Object.Instantiate<GameObject>(BeyondPrefabs.AHHH.LoadAsset<GameObject>("Spike"), orbPrefab.transform);
				orb.AddComponent<MakeObjSpin>();

				

				var enemy = orbPrefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.knockbackDoer.weight = 800;
				enemy.aiActor.MovementSpeed = 6f;
				enemy.aiActor.healthHaver.PreventAllDamage = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.HasShadow = true;
				enemy.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;

				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.aiAnimator.HitReactChance = 0.05f;
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.PreventFallingInPitsEver = true;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(550);
				enemy.aiActor.healthHaver.SetHealthMaximum(550);
				enemy.aiActor.CollisionKnockbackStrength = 1f;
				enemy.aiActor.procedurallyOutlined = true;
				enemy.aiActor.CanTargetPlayers = true;

				

				//enemy.aiActor.MovementSpeed = 0f;

				BotsModule.Strings.Enemies.Set("#THE_ORB", "???");
				BotsModule.Strings.Enemies.Set("#BOT_????", "???");
				BotsModule.Strings.Enemies.Set("#BOT_ORB_SUBTITLE", "Other Worldly Horror");
				BotsModule.Strings.Enemies.Set("#BOT_QUOTE", "ahhhh");
				enemy.aiActor.healthHaver.overrideBossName = "#THE_ORB";
				enemy.aiActor.OverrideDisplayName = "#THE_ORB";
				enemy.aiActor.ActorName = "#THE_ORB";
				enemy.aiActor.name = "#THE_ORB";
				orbPrefab.name = enemy.aiActor.OverrideDisplayName;


				GenericIntroDoer miniBossIntroDoer = orbPrefab.AddComponent<GenericIntroDoer>();
				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Boss_Theme_Beholster";
				miniBossIntroDoer.PreventBossMusic = false;
				miniBossIntroDoer.InvisibleBeforeIntroAnim = true;
				miniBossIntroDoer.preIntroAnim = string.Empty;
				miniBossIntroDoer.preIntroDirectionalAnim = string.Empty;
				miniBossIntroDoer.introAnim = "intro";
				miniBossIntroDoer.introDirectionalAnim = string.Empty;
				miniBossIntroDoer.continueAnimDuringOutro = false;
				miniBossIntroDoer.cameraFocus = null;
				miniBossIntroDoer.roomPositionCameraFocus = Vector2.zero;
				miniBossIntroDoer.restrictPlayerMotionToRoom = false;
				miniBossIntroDoer.fusebombLock = false;
				miniBossIntroDoer.AdditionalHeightOffset = 0;
				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
				{
					bossNameString = "#THE_ORB",
					bossSubtitleString = "#BOT_ORB_SUBTITLE",
					bossQuoteString = "#BOT_QUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = new Color32(163, 0, 106, 255)
				};
				/*
				if (BossCardTexture)
				{
					miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
					miniBossIntroDoer.SkipBossCard = false;
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				}
				else
				{
					miniBossIntroDoer.SkipBossCard = true;
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				}*/
				miniBossIntroDoer.SkipBossCard = true;
				enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
				//BehaviorSpeculator aIActor = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").behaviorSpeculator;
				//Tools.DebugInformation(aIActor);

				/////

				SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Enemies/MiniBosses/AnomalyIcon", BeyondPrefabs.ammonomiconCollection, "AnomalyIcon");
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "bot:anomaly";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = "";
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "AnomalyIcon";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = null;

				//This fast and ruthless  is known to disorient all who challenge them.
				BotsModule.Strings.Enemies.Set("#BOT_ORB_LONGDESC", "This should not exists");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#THE_ORB";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#BOT_ORB_SUBTITLE";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#BOT_ORB_LONGDESC";
				Tools.AddEnemyToDatabase2(enemy.gameObject, "bot:anomaly", true, true);
				EnemyDatabase.GetEntry("bot:anomaly").ForcedPositionInAmmonomicon = 10000;
				EnemyDatabase.GetEntry("bot:anomaly").isInBossTab = true;
				EnemyDatabase.GetEntry("bot:anomaly").isNormalEnemy = true;




				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
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
					ManualRightY = 0,
				});
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
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
					ManualRightY = 0,



				});

				orb.transform.position = enemy.aiActor.specRigidbody.UnitCenter;


				enemy.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				enemy.aiActor.PreventBlackPhantom = true;
				orbPrefab.GetOrAddComponent<AIAnimator>().OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();

				orbPrefab.GetOrAddComponent<AIAnimator>().OtherVFX = EnemyDatabase.GetOrLoadByGuid("19b420dec96d4e9ea4aebc3398c0ba7a").gameObject.GetComponent<AIAnimator>().OtherVFX;


				orbPrefab.AddAnimation("idle", "BotsMod/sprites/Enemies/Orb", 3, EnemyBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

				var bs = orbPrefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = enemy.transform;
				GameObject m_CachedGunAttachPoint = enemy.transform.Find("attach").gameObject;


				bs.TargetBehaviors = new List<TargetBehaviorBase>
				{
					new TargetPlayerBehavior
					{
						Radius = 35f,
						LineOfSight = false,
						ObjectPermanence = true,
						SearchInterval = 0.25f,
						PauseOnTargetSwitch = false,
						PauseTime = 0.25f
					}
				};

				bs.MovementBehaviors = new List<MovementBehaviorBase>() {
					new SeekTargetBehavior() {
						StopWhenInRange = true,
						CustomRange = 6,
						LineOfSight = true,
						ReturnToSpawn = true,
						SpawnTetherDistance = 0,
						PathInterval = 0.5f,
						SpecifyRange = false,
						MinActiveRange = 3,
						MaxActiveRange = 10
					}
				};


				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior
						{
							

							ShootPoint = m_CachedGunAttachPoint,
							BulletScript = new CustomBulletScriptSelector(typeof(OrbScript)),
							LeadAmount = 0f,
							AttackCooldown = 1.2f,
							TellAnimation = "",
							FireAnimation = "",
							RequiresLineOfSight = false,
							Cooldown = 3f,

							StopDuring = ShootBehavior.StopType.Attack,
							Uninterruptible = true



						},
				
						NickName = "Shoot lots",
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new ShootBehavior
						{


							ShootPoint = m_CachedGunAttachPoint,
							BulletScript = new CustomBulletScriptSelector(typeof(HomingScript)),
							LeadAmount = 0f,
							AttackCooldown = 1.2f,
							TellAnimation = "",
							FireAnimation = "",
							RequiresLineOfSight = false,
							Cooldown = 3f,

							StopDuring = ShootBehavior.StopType.Attack,
							Uninterruptible = true



						},

						NickName = "Homing",
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,

						Behavior = new SequentialAttackBehaviorGroup()
						{
							
							AttackBehaviors = new List<AttackBehaviorBase>
							{
								new TeleportToMiddleBehavior()
								{

									AttackableDuringAnimation = true,
									AllowCrossRoomTeleportation = false,
									teleportRequiresTransparency = false,
									hasOutlinesDuringAnim = true,
									ManuallyDefineRoom = false,
									MaxHealthThreshold = 1f,
									GoneTime = 1f,
									OnlyTeleportIfPlayerUnreachable = false,
									teleportInAnim = "",
									teleportOutAnim = "",
									AttackCooldown = 1f,
									InitialCooldown = 1f,
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
								new ShootBehavior
								{


									ShootPoint = m_CachedGunAttachPoint,
									BulletScript = new CustomBulletScriptSelector(typeof(RainHellFireUponThePlayer)),
									LeadAmount = 0f,
									AttackCooldown = 1.2f,
									TellAnimation = "",
									FireAnimation = "",
									RequiresLineOfSight = false,
									Cooldown = 3f,

									StopDuring = ShootBehavior.StopType.Attack,
									Uninterruptible = true



								
								},
							},
							OverrideCooldowns = new List<float>
							{
								0
							},
							RunInClass = false,

						},
						NickName = "Beam Teleport Magic bs"

					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,
						Behavior = new SummonEnemyViaWaveBehavior
						{

							GlobalCooldown = 0.5f,
							Cooldown = 4f,
							CooldownVariance = 1f,
							InitialCooldownVariance = 0f,
							IsBlackPhantom = false,
							GroupName = null,
							GroupCooldown = 0,
							MinRange = 0,
							Range = 0,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 0.1f,
							MaxEnemiesInRoom = 1,
							MaxUsages = 1,
							AccumulateHealthThresholds = true,
							//shadowInAnim = null,
							//shadowOutAnim = null,
							targetAreaStyle = null,
							HealthThresholds = new float[0],
							MinWallDistance = 0,
							AttackCooldown = 1.2f,						
							RequiresLineOfSight = false,
						},

						NickName = "spawn lots of things coz the player is having fun >:(",
					},

				};

				//companion.bulletBank
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;

				GameObject light = new GameObject("light");
				AdditionalBraveLight braveLight = light.AddComponent<AdditionalBraveLight>();
				light.transform.position = enemy.sprite.WorldCenter;
				light.transform.parent = enemy.transform;
				braveLight.LightColor = new Color32(255, 69, 248, 255);
				braveLight.LightIntensity = 4.25f;
				braveLight.LightRadius = 5.4375f;

				Game.Enemies.Add("bot:anomaly", enemy.aiActor);

			}
		}

		public class HomingScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{

				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				}
				for (int z = 0; z < 3; z++)
				{
					var num = UnityEngine.Random.Range(8, 10);
					for (int i = 0; i < num; i++)
					{

						this.Fire(new Direction((360f / num) *i, DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(5f, 8), SpeedType.Absolute), new Homing());
						yield return this.Wait(2);
					}
					yield return this.Wait(16);
				}
				
				yield break;
			}
		}

		public class Homing : Bullet
		{
			public Homing() : base("sweep", false, false, false)
			{

			}
			protected override IEnumerator Top()
			{



				for (int i = 0; i < 90; i++)
				{
					//ETGModConsole.Log("AAAAAAA");
					float aim = this.GetAimDirection(1f, 16f);
					float delta = BraveMathCollege.ClampAngle180(aim - this.Direction);
					if (Mathf.Abs(delta) > 100f)
					{
						yield break;
					}
					this.Direction += Mathf.MoveTowards(0f, delta, 3f);
					yield return this.Wait(1);
				}
				yield break;
			}
		}

		public class OrbScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				}

				for (int i = 0; i < 150; i++)
				{
					this.Fire(new Direction(UnityEngine.Random.Range(0f, 360f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(9f, 13), SpeedType.Absolute), new OrbBullet(UnityEngine.Random.value >= 0.7f ? true : false));
					yield return this.Wait(1);
				}
				yield break;
			}
		}

		public class OrbBullet : Bullet
		{
			public OrbBullet(bool jammed) : base("sweep", false, false, jammed)
			{

			}
			
		}

		public class RainHellFireUponThePlayer : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4d164ba3f62648809a4a82c90fc22cae").bulletBank.GetBullet("big_one"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4d164ba3f62648809a4a82c90fc22cae").bulletBank.GetBullet("default"));
				}
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(0f, 8)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(6f, 6)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(8f, 0)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(6f, -6)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(0f, -8)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(-6, -6)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(-8f, 0)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());
				yield return Wait(16);
				this.Fire(Offset.OverridePosition((Vector2)this.BulletBank.aiActor.transform.position + new Vector2(0f, 30f) + new Vector2(-6f, 6)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new BigBullet());



				
				yield break;
			}
		}

		private class BigBullet : Bullet
		{
			public BigBullet() : base("big_one", false, false, false)
			{
			}

			public override void Initialize()
			{
				this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
				base.Initialize();
			}

			protected override IEnumerator Top()
			{
				this.Projectile.specRigidbody.CollideWithTileMap = false;
				this.Projectile.specRigidbody.CollideWithOthers = false;
				yield return this.Wait(45);
				this.Speed = 0f;
				this.Projectile.spriteAnimator.Play();
				float startingAngle = this.RandomAngle();
				for (int j = 0; j < 39; j++)
				{
					float startAngle = startingAngle;
					int numBullets = 39;
					int i2 = j;
					bool offset = false;
					float direction = this.SubdivideCircle(startAngle, numBullets, i2, 1f, offset);
					this.Fire(new Direction(direction, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet(10f, 17, -1));
				}
				yield return this.Wait(30);
				this.Vanish(true);
				yield break;
			}

			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (preventSpawningProjectiles)
				{
					return;
				}
			}
		}

		public class EnemyBehavior : BraveBehaviour
		{

			void Update()
            {
				base.sprite.renderer.enabled = false;
				SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite);
			}


			private void Start()
			{
				
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					base.aiActor.AdditionalSafeItemDrops.Add(PickupObjectDatabase.GetById(BotsItemIds.Relic1));
					base.aiActor.ParentRoom?.HandleRoomAction(RoomEventTriggerAction.END_TERRIFYING_AND_DARK);
				};
				base.healthHaver.healthHaver.OnDeath += (obj) =>
				{


				};

				base.aiActor.healthHaver.OnDamaged += (float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection) =>
				{
					this.GetComponentInChildren<MakeObjSpin>().GetComponent<Renderer>().material.SetFloat("_noisestep", Mathf.Clamp(base.aiActor.healthHaver.GetCurrentHealthPercentage() * 1.1f, 0.3f, 1.1f));
				};

				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

			}


		}


	}
}








