using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.BossBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using FrostAndGunfireItems;
using System.Runtime.CompilerServices;
using BotsMod;
//using EnemyAPI;

namespace BotsMod
{
	class LostPastBoss : AIActor
	{
		public static GameObject fuckyouprefab;
		public static readonly string guid = "bot:Overseer";
		private static tk2dSpriteCollectionData StagngerCollection;
		public static GameObject shootpoint;
		private static Texture2D BossCardTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\overseer_floor\\Overseer_Bosscard_001.png");
		public static string TargetVFX;
		public static AIActor overseerAiActor;

		//make teleport ring turn around and go back the other way after like a second


		public static void Init()
		{
			
			LostPastBoss.BuildPrefab();

			GameObject overseerShield = SpriteBuilder.SpriteFromResource("BotsMod/sprites/overseer_floor/Shield/OverseerShield", new GameObject("OverseerShield"));
			overseerShield.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(overseerShield);
			UnityEngine.Object.DontDestroyOnLoad(overseerShield);
			Shader shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
			var specRigidbody = overseerShield.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(5, 1), new IntVector2(17, 34)).PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;//31
			overseerShield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.shader = shader;
			overseerShield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().materialInst.shader = shader;
			overseerShield.GetComponent<tk2dSprite>().HeightOffGround = -0.7f;
			overseerShield.GetComponent<tk2dSprite>().UpdateZDepth();

			Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.mainTexture = overseerShield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture;
			mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 100);
			//overseerShield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material = mat;

			overseerShield.GetComponent<MeshRenderer>().material = mat;

			SpriteOutlineManager.AddOutlineToSprite(overseerShield.GetComponent<tk2dSprite>(), Color.black);

			OverseerShield shield = overseerShield.gameObject.AddComponent<OverseerShield>();
			BotsModule.overseerShield = shield;
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = fuckyouprefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				
				fuckyouprefab = BossBuilder.BuildPrefab("Overseer", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
				var companion = fuckyouprefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 3.33f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = true;
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;

				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(600f);
				companion.aiActor.healthHaver.SetHealthMaximum(600f);
				companion.aiActor.CollisionKnockbackStrength = 2f;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.CanTargetPlayers = true;

				BotsModule.Strings.Enemies.Set("#THE_OVERSEER", "Overseer");
				BotsModule.Strings.Enemies.Set("#BOT_????", "???");
				BotsModule.Strings.Enemies.Set("#BOT_SUBTITLE", "He sees all...");
				BotsModule.Strings.Enemies.Set("#BOT_QUOTE", "ahhhh");
				companion.aiActor.healthHaver.overrideBossName = "#THE_OVERSEER";
				companion.aiActor.OverrideDisplayName = "#THE_OVERSEER";
				companion.aiActor.ActorName = "#THE_OVERSEER";
				companion.aiActor.name = "#THE_OVERSEER";
				fuckyouprefab.name = companion.aiActor.OverrideDisplayName;
				

				GenericIntroDoer miniBossIntroDoer = fuckyouprefab.AddComponent<GenericIntroDoer>();
				fuckyouprefab.AddComponent<LostPastBossIntro>();
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
					bossNameString = "#THE_OVERSEER",
					bossSubtitleString = "#BOT_SUBTITLE",
					bossQuoteString = "#BOT_QUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = new Color32(163, 0, 106, 255)
				};
				if (BossCardTexture)
				{
					miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
					miniBossIntroDoer.SkipBossCard = false;
					companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				}
				else
				{
					miniBossIntroDoer.SkipBossCard = true;
					companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				}
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
				//BehaviorSpeculator aIActor = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").behaviorSpeculator;
				//Tools.DebugInformation(aIActor);

				/////

				SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/OverseerIcon", SpriteBuilder.ammonomiconCollection);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "bot:Overseer";
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				companion.encounterTrackable.journalData.SuppressKnownState = false;
				companion.encounterTrackable.journalData.IsEnemy = true;
				companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				companion.encounterTrackable.ProxyEncounterGuid = "";
				companion.encounterTrackable.journalData.AmmonomiconSprite = "BotsMod/sprites/OverseerIcon";
				companion.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\overseer_floor\\ammonomicon_enemy_overseer_001.png");

				BotsModule.Strings.Enemies.Set("#BOT_OVERSEER_SHORTDESC", "He sees all...");
				BotsModule.Strings.Enemies.Set("#BOT_OVERSEER_LONGDESC", "NotABot fucking hates writing lore with every fiber of his being so you get this :)");
				companion.encounterTrackable.journalData.PrimaryDisplayName = "#THE_OVERSEER";
				companion.encounterTrackable.journalData.NotificationPanelDescription = "#BOT_OVERSEER_SHORTDESC";
				companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#BOT_OVERSEER_LONGDESC";
				Tools.AddEnemyToDatabase2(companion.gameObject, "bot:Overseer", true, true);
				EnemyDatabase.GetEntry("bot:Overseer").ForcedPositionInAmmonomicon = 10000;
				EnemyDatabase.GetEntry("bot:Overseer").isInBossTab = true;
				EnemyDatabase.GetEntry("bot:Overseer").isNormalEnemy = true;

				//GameObject EnemyPrefab = companion.gameObject;

				//if (!string.IsNullOrEmpty(EnemyPrefab.GetComponent<AIActor>().ActorName))
				//{
				//	string EnemyName = "bot:" + EnemyPrefab.GetComponent<AIActor>().ActorName.Replace(" ", "_").Replace("(", "_").Replace(")", string.Empty).ToLower();
				//	if (!Game.Enemies.ContainsID(EnemyName)) { Game.Enemies.Add(EnemyName, EnemyPrefab.GetComponent<AIActor>()); }
				//}




				companion.aiActor.healthHaver.SetHealthMaximum(1200f, null, false);
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
					ManualOffsetY = 10,
					ManualWidth = 16,
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
					ManualOffsetY = 10,
					ManualWidth = 16,
					ManualHeight = 17,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});

				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"flare",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "flare",
						anim = anim
					}
				};
				DirectionalAnimation anim2 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"brrap",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "brrap",
						anim = anim2
					}
				};
				DirectionalAnimation anim3 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					AnimNames = new string[]
					{
						"tell",

					},
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "tell",
						anim = anim3
					}
				};
				DirectionalAnimation Hurray = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "attack",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack",
						anim = Hurray
					}
				};
				DirectionalAnimation almostdone = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "intro",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "intro",
						anim = almostdone
					}
				};
				DirectionalAnimation done = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "death",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "death",
						anim = done
					}
				};
				bool flag3 = StagngerCollection == null;
				if (flag3)
				{
					StagngerCollection = SpriteBuilder.ConstructCollection(fuckyouprefab, "The_Overseer_Collection");
					UnityEngine.Object.DontDestroyOnLoad(StagngerCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], StagngerCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{

						0,
						1,
						2,
						3


					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 3f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{
						4,
						5,
						6,
						7,
						8,
						9,
						10,
						11,
0,


					}, "attack", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 60f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{

0,


					}, "brrap", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{
0,


					}, "tell", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{
0,



					}, "fire1", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{
						
						12,
						13,
						14,
						15,
						16,
						17,
						18,
						19

					}, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, StagngerCollection, new List<int>
					{
0,

					}, "death", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;

				}
				var bs = fuckyouprefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;



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
						Probability = 0.3f,
						Behavior = new SummonEnemyBehavior{
							KillSpawnedOnDeath = true,
							MaxRoomOccupancy = 3,
							MaxSummonedAtOnce = 3,
							MaxToSpawn = -1,
							NumToSpawn = 3,
							DisableDrops = true,
							StopDuringAnimation = false,
							TargetVfxLoops = true,
							DefineSpawnRadius = true,
							MinSpawnRadius = 0,
							MaxSpawnRadius = 5,
							BlackPhantomChance = 0,
							EnemeyGuids = new List<string>
							{
								"bot:beyond_kin",
							},
							selectionType = SummonEnemyBehavior.SelectionType.Random,
							OverrideCorpse = null,
							SummonTime = 2,
							HideGun = false,


						},
						NickName = "summon"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(BrrapScript)),
						LeadAmount = 0f,
						AttackCooldown = 1.5f,
						FireAnimation = "brrap",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "BRRRRRRRRAP"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(GrenadeChuck)),
						LeadAmount = 0f,
						MaxUsages = 3,
						Cooldown = 7f,
						InitialCooldown = 3f,

						AttackCooldown = 1.66f,
						TellAnimation = "tell",
						FireAnimation = "fire1",
						RequiresLineOfSight = false,
						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "Grenade Toss."
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0.6f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SheildScript)),
						LeadAmount = 0f,
						AttackCooldown = 1.2f,
						TellAnimation = "attack",
						FireAnimation = "attack",
						RequiresLineOfSight = false,
						Cooldown = 3f,

						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "shield"
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0f,
						Behavior = new RockFallBehavior{

						AttackCooldown = 1.2f,

						RequiresLineOfSight = false,
						Cooldown = 3f,

						rocksToSpawn = 5,

						},
						NickName = "rocks"
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(OverseerTwinBeams1)),
						LeadAmount = 0f,
						AttackCooldown = 1.2f,
						TellAnimation = "attack",
						FireAnimation = "attack",
						RequiresLineOfSight = false,
						Cooldown = 6f,

						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "ring"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0f,
						Behavior = new BeholsterLaserBehavior{
							trackingType = BeholsterLaserBehavior.TrackingType.Follow,
							initialAimOffset = -15,
							chargeTime = 1,
							firingTime = 5,
							maxTurnRate = 10,
							turnRateAcceleration = 24,
							useDegreeCatchUp = true,
							minDegreesForCatchUp = 15,
							degreeCatchUpSpeed = 180,
							useUnitCatchUp = true,
							unitOvershootSpeed = 10,
							Cooldown = 10,
							CooldownVariance = 0,
							AttackCooldown = 0,
							GlobalCooldown = 0,
							InitialCooldown = 0,
							InitialCooldownVariance = 0,
							GroupName = null,
							GroupCooldown = 0,
							MinRange = 0,
							Range = 0,
							MinWallDistance = 0,
							MaxEnemiesInRoom = 0,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							RequiresLineOfSight = false,
							MaxUsages = 0,
						},
						NickName = "Sniper"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1,

						Behavior = new TeleportBehavior()
						{

							AttackableDuringAnimation = true,
							AllowCrossRoomTeleportation = false,
							teleportRequiresTransparency = false,
							hasOutlinesDuringAnim = true,
							ManuallyDefineRoom = false,
							MaxHealthThreshold = 1f,
							StayOnScreen = true,
							AvoidWalls = true,
							GoneTime = 1f,
							OnlyTeleportIfPlayerUnreachable = false,
							MinDistanceFromPlayer = 4f,
							MaxDistanceFromPlayer = -1f,
							teleportInAnim = "attack",
							teleportOutAnim = "attack",
							AttackCooldown = 1f,
							InitialCooldown = 0f,
							RequiresLineOfSight = false,
							roomMax = new Vector2(0,0),
							roomMin = new Vector2(0,0),
							teleportOutBulletScript = new CustomBulletScriptSelector(typeof(OverseerTeleportStartScript)),
							//teleportInBulletScript = new CustomBulletScriptSelector(typeof(OverseerBeam)),
							//teleportInBulletScript = new CustomBulletScriptSelector(typeof(BEAM
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
						NickName = "Teleport"

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

				overseerAiActor = companion.aiActor;

				companion.gameObject.AddComponent<OverseerGlow>();
				companion.gameObject.AddComponent<BossFinalLostDeathController>();

				OverseerShield.newOwner = companion.gameActor;



				Game.Enemies.Add("bot:the_overseer", companion.aiActor);

		}
	}





		private static string[] spritePaths = new string[]
		{
			
			//idles
			"BotsMod/sprites/overseer_floor/overseerf_idle_001.png",
			"BotsMod/sprites/overseer_floor/overseerf_idle_002.png",
			"BotsMod/sprites/overseer_floor/overseerf_idle_003.png",
			"BotsMod/sprites/overseer_floor/overseerf_idle_004.png",

			//"attack"
			"BotsMod/sprites/overseer_floor/overseerf_spin_001.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_002.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_003.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_004.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_005.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_006.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_007.png",
			"BotsMod/sprites/overseer_floor/overseerf_spin_008.png",


			//intro

			"BotsMod/sprites/overseer_floor/the overseer_intro1",
			"BotsMod/sprites/overseer_floor/the overseer_intro2",
			"BotsMod/sprites/overseer_floor/the overseer_intro3",
			"BotsMod/sprites/overseer_floor/the overseer_intro4",
			"BotsMod/sprites/overseer_floor/the overseer_intro5",
			"BotsMod/sprites/overseer_floor/the overseer_intro6",
			"BotsMod/sprites/overseer_floor/the overseer_intro7",
			"BotsMod/sprites/overseer_floor/the overseer_intro8",

		};
	}




	public class OverseerTeleportStartScript : Script
	{
		// Token: 0x06000676 RID: 1654 RVA: 0x00076698 File Offset: 0x00074898
		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			for (int i = 0; i <= 36; i++)
			{
				this.Fire(new Direction((float)(i * 10), DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			}
			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}

			protected override IEnumerator Top()
			{
				var SetupTime = 5;
				this.ChangeDirection(new Direction(180f, DirectionType.Relative, -1f), 1);
				yield return this.Wait(SetupTime);

			}
		}
	}


	public class OverseerTwinBeams1 : Script
	{
		// Token: 0x06000676 RID: 1654 RVA: 0x00076698 File Offset: 0x00074898
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").bulletBank.GetBullet("default_novfx"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
			}

			this.EndOnBlank = true;
			float startDirection = this.AimDirection;
			float sign = BraveUtility.RandomSign();
			for (int i = 0; i < 210; i++)
			{
				float offset = 0f;
				if (i < 30)
				{
					offset = Mathf.Lerp(135f, 0f, (float)i / 29f);
				}
				float currentAngle = startDirection + sign * Mathf.Max(0f, (float)(i - 60) * 1f);
				for (int j = 0; j < 5; j++)
				{
					float num = offset + 20f + (float)j * 10f;
					this.Fire(new Offset("right eye"), new Direction(currentAngle + num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
					this.Fire(new Offset("left eye"), new Direction(currentAngle - num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
				}
				if (i > 30 && i % 30 == 29)
				{
					this.Fire(new Direction(currentAngle + UnityEngine.Random.Range(-1f, 1f) * 20f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new SkellBullet());
				}
				if (i >= 60)
				{
					float num2 = Vector2.Distance(this.BulletManager.PlayerPosition(), this.Position);
					float num3 = num2 / 18f * 30f;
					if (num3 > (float)(i - 60))
					{
						num3 = (float)Mathf.Max(0, i - 60);
					}
					float num4 = -sign * num3 * 1f;
					float num5 = currentAngle + 40f + num4;
					float num6 = currentAngle - 40f + num4;
					if (BraveMathCollege.ClampAngle180(num5 - this.GetAimDirection("right eye")) < 0f)
					{
						yield break;
					}
					if (BraveMathCollege.ClampAngle180(num6 - this.GetAimDirection("left eye")) > 0f)
					{
						yield break;
					}
				}


				yield return this.Wait(2);
			}
			yield break;
		}
		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{

			}
		}

		private const float CoreSpread = 20f;

		private const float IncSpread = 10f;

		private const float TurnSpeed = 1f;

		private const float BulletSpeed = 18f;
	}


	public class BossFinalLostSword1 : Script
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0000A670 File Offset: 0x00008870
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("8d441ad4e9924d91b6070d5b3438d066").bulletBank.GetBullet("default"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").bulletBank.GetBullet("default_novfx"));


			}
			this.EndOnBlank = true;
			this.m_sign = BraveUtility.RandomSign();
			this.m_doubleSwing = BraveUtility.RandomBool();
			Vector2 leftOrigin = new Vector2(this.m_sign * 2f, -0.2f);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, 0.2f), new Vector2(this.m_sign * 11f, 0.2f), 14);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 11.6f, -0.2f), new Vector2(this.m_sign * 11.6f, -0.2f), 2);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, -0.6f), new Vector2(this.m_sign * 11f, -0.6f), 14);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.1f, -1.2f), new Vector2(this.m_sign * 3.1f, 0.8f), 4);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 2.2f, -0.2f), new Vector2(this.m_sign * 2.7f, -0.2f), 2);
			yield return this.Wait(75);
			yield break;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A68C File Offset: 0x0000888C
		private void FireLine(Vector2 spawnPoint, Vector2 start, Vector2 end, int numBullets)
		{
			Vector2 a = (end - start) / (float)Mathf.Max(1, numBullets - 1);
			float num = 0.33333334f;
			for (int i = 0; i < numBullets; i++)
			{
				Vector2 a2 = (numBullets != 1) ? (start + a * (float)i) : end;
				float speed = Vector2.Distance(a2, spawnPoint) / num;
				base.Fire(new Offset(spawnPoint, 0f, string.Empty, DirectionType.Absolute), new Direction((a2 - spawnPoint).ToAngle(), DirectionType.Absolute, -1f), new Speed(speed, SpeedType.Absolute), new BossFinalLostSword1.SwordBullet(base.Position, this.m_sign, this.m_doubleSwing));
			}
		}

		// Token: 0x0400023D RID: 573
		private const int SetupTime = 20;

		// Token: 0x0400023E RID: 574
		private const int HoldTime = 30;

		// Token: 0x0400023F RID: 575
		private const int SwingTime = 25;

		// Token: 0x04000240 RID: 576
		private float m_sign;

		// Token: 0x04000241 RID: 577
		private bool m_doubleSwing;



		public class SwordBullet : Bullet
		{
			// Token: 0x06000228 RID: 552 RVA: 0x0000A73F File Offset: 0x0000893F
			public SwordBullet(Vector2 origin, float sign, bool doubleSwing) : base("default", false, false, false)
			{
				this.m_origin = origin;
				this.m_sign = sign;
				this.m_doubleSwing = doubleSwing;

				//this.Projectile.PenetratesInternalWalls = true;
				//this.Projectile.pierceMinorBreakables = true;
				//this.Projectile.specRigidbody.CollideWithTileMap = false;
				//this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true; 
			}

			// Token: 0x06000229 RID: 553 RVA: 0x0000A760 File Offset: 0x00008960
			protected override IEnumerator Top()
			{
				yield return this.Wait(20);
				float angle = (this.Position - this.m_origin).ToAngle();
				float dist = Vector2.Distance(this.Position, this.m_origin);
				this.Speed = 0f;
				yield return this.Wait(30);
				this.ManualControl = true;
				int swingtime = (!this.m_doubleSwing) ? 25 : 100;
				float swingDegrees = (float)((!this.m_doubleSwing) ? 180 : 540);
				for (int i = 0; i < swingtime; i++)
				{
					float newAngle = angle - Mathf.SmoothStep(0f, this.m_sign * swingDegrees, (float)i / (float)swingtime);
					this.Position = this.m_origin + BraveMathCollege.DegreesToVector(newAngle, dist);
					yield return this.Wait(1);
				}
				//UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position, Quaternion.identity);
				yield return this.Wait(5);
				this.Vanish(false);
				yield break;
			}

			// Token: 0x04000242 RID: 578
			private Vector2 m_origin;

			// Token: 0x04000243 RID: 579
			private float m_sign;

			// Token: 0x04000244 RID: 580
			private bool m_doubleSwing;
		}
	}


	public class ShockwaveChallengeCircleBurst : Script
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x000695F8 File Offset: 0x000677F8
		protected override IEnumerator Top()
		{
			float num = base.RandomAngle();
			float num2 = 30f;
			for (int i = 0; i < 12; i++)
			{
				base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), null);
			}
			return null;
		}

		// Token: 0x04000C7A RID: 3194
		private const int NumBullets = 12;
	}

	public class SheildScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
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

			OverseerShield shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(0, -2), Quaternion.Euler(0,0,0));
			OverseerShield shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(2, 2), Quaternion.Euler(0, 0, 45));
			OverseerShield shield3 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(-2, 2), Quaternion.Euler(0, 0, 315));
			//UnityEngine.Object.Instantiate(BotsModule.overseerShield, LostPastBoss.overseerAiActor.transform.Find("attach").position - new Vector3(1, 0, 0), Quaternion.identity);

			//UnityEngine.Object.Instantiate(BotsModule.overseerShield, LostPastBoss.overseerAiActor.transform.Find("attach").position + new Vector3(1, 0, 0), Quaternion.identity);


			shield1.self = shield1.gameObject;
			shield2.self = shield2.gameObject;
			shield3.self = shield3.gameObject;

			shield1.leader = LostPastBoss.fuckyouprefab;
			shield2.leader = LostPastBoss.fuckyouprefab;
			shield3.leader = LostPastBoss.fuckyouprefab;


			shield1.offset = new Vector3(0, -2, 0);    //-1-
			shield2.offset = new Vector3(2, 2, 0);  //---
			shield3.offset = new Vector3(-2, 2, 0);   //2-3



			//shield3.StartCoroutine(removeShield(shield3));
			for (int i = 0; i < 3; i++)
			{
				float num = 120 * (float)i;
				base.Fire(null, new Direction(num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom)); ;
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
					Bullet bullet = new Bullet(null, false, false, false);
					base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), bullet);
					if (!this.m_isBlackPhantom)
					{
						bullet.ForceBlackBullet = false;
						bullet.Projectile.ForceBlackBullet = false;
						bullet.Projectile.ReturnFromBlackBullet();
					}
				}
			}

			private const int NumBullets = 18;

			private bool m_isBlackPhantom;
		}
	}


	public class SwordScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
			}
			//this.EndOnBlank = true;
			for (int i = 0; i < 22; i++)
			{
				this.Fire(new Direction(0, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedUpSwordBullet(i));
				//base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				//base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.

				yield return Wait(1); // Wait for 20 frames

				//base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet -20 degrees from the enemy aim angle, with a bullet speed of 9.
				//base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet 20 degrees from the enemy aim angle, with a bullet speed of 9.
			}
			yield break;
		}

		private class SwordBullet : Bullet
		{
			// Token: 0x060009DC RID: 2524 RVA: 0x0002FCF0 File Offset: 0x0002DEF0
			public SwordBullet() : base("default", false, false, false)
			{
			}
		}

		public class SpeedUpSwordBullet : Bullet
		{
			
			public SpeedUpSwordBullet(float pointInLine) : base("reversible", false, false, false)
			{
				possitsion = pointInLine;
				//this.spawnTime = spawnTime;

			}
			public int spawnTime;
			float possitsion = 1;
			int SetupTime = 90;
			int BulletCount = 22;
			float QuarterPi = 0.785f;
			int SkipSetupBulletNum = 6;

			float Radius = 11f;
			int QuaterRotTime = 120;
			int SpinTime = 600;
			int PulseInitialDelay = 120;
			int PulseDelay = 120;
			int PulseCount = 4;
			int PulseTravelTime = 100;
			// Token: 0x06000A92 RID: 2706 RVA: 0x00030B48 File Offset: 0x0002ED48
			protected override IEnumerator Top()
			{
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), SetupTime);
				yield return this.Wait(SetupTime);
				this.ChangeDirection(new Direction(90f, DirectionType.Relative, -1f), 1);
				yield return this.Wait(1);
				this.ChangeSpeed(new Speed((float)this.spawnTime / (float)BulletCount * (Radius * 2f) * QuarterPi * (60f / (float)QuaterRotTime), SpeedType.Relative), 1);
				this.ChangeDirection(new Direction(90f / (float)QuaterRotTime, DirectionType.Sequence, -1f), SpinTime);
				yield return this.Wait(SpinTime - 1);
				this.Vanish(false);
				yield break;
			}
		}
	}


	public class GrenadeChuck : Script
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("880bbe4ce1014740ba6b4e2ea521e49d").bulletBank.GetBullet("grenade"));
			}
			base.PostWwiseEvent("Play_BOSS_lasthuman_volley_01", null);
			float airTime = base.BulletBank.GetBullet("grenade").BulletObject.GetComponent<ArcProjectile>().GetTimeInFlight();
			Vector2 vector = this.BulletManager.PlayerPosition();
			Bullet bullet2 = new Bullet("grenade", false, false, false);
			float direction2 = (vector - base.Position).ToAngle();
			base.Fire(new Direction(direction2, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet2);
			(bullet2.Projectile as ArcProjectile).AdjustSpeedToHit(vector);
			bullet2.Projectile.ImmuneToSustainedBlanks = true;
			//yield return base.Wait(150);
			for (int a = 0; a < 2; a++)
			{
				base.PostWwiseEvent("Play_BOSS_lasthuman_volley_01", null);
				for (int i = 0; i < 4; i++)
				{
					for (int h = 0; h < 2; h++)
					{
						base.Fire(new Direction(UnityEngine.Random.Range(-75f, 75f), DirectionType.Aim, -1f), new Speed(7.5f + h, SpeedType.Absolute), new WallBullet());
						yield return base.Wait(1);
					}
					yield return base.Wait(12);
					Vector2 targetVelocity = this.BulletManager.PlayerVelocity();
					float startAngle;
					float dist;
					if (targetVelocity != Vector2.zero && targetVelocity.magnitude > 0.5f)
					{
						startAngle = targetVelocity.ToAngle();
						dist = targetVelocity.magnitude * airTime;
					}
					else
					{
						startAngle = base.RandomAngle();
						dist = (7f * a) * airTime;
					}
					float angle = base.SubdivideCircle(startAngle, 4, i, 1f, false);
					Vector2 targetPoint = this.BulletManager.PlayerPosition() + BraveMathCollege.DegreesToVector(angle, dist);
					float direction = (targetPoint - base.Position).ToAngle();
					if (i > 0)
					{
						direction += UnityEngine.Random.Range(-12.5f, 12.5f);
					}
					Bullet bullet = new Bullet("grenade", false, false, false);
					base.Fire(new Direction(direction, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet);
					(bullet.Projectile as ArcProjectile).AdjustSpeedToHit(targetPoint);
					bullet.Projectile.ImmuneToSustainedBlanks = true;
				}
			}

			yield break;
		}
	}

	public class SpawnSkeletonBullet : Bullet
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x000085A7 File Offset: 0x000067A7
		public SpawnSkeletonBullet() : base("skull", false, false, false)
		{
		}
		/*
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			var list = new List<string> {
				//"shellet",
				"336190e29e8a4f75ab7486595b700d4a"
			};
			string guid = BraveUtility.RandomElement<string>(list);
			var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
			AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
		}
		*/
	}


	public class BrrapScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
			}
			for (int u = 0; u < 4; u++)
			{
				int scatter = UnityEngine.Random.Range(4, 7);
				AkSoundEngine.PostEvent("Play_WPN_golddoublebarrelshotgun_shot_01", this.BulletBank.aiActor.gameObject);
				for (int k = 0; k < scatter; k++)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(-25f, 25f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(9f, 13f), SpeedType.Absolute), new WallBullet());
				}
				yield return Wait(scatter - 1);
				base.Fire(new Direction(UnityEngine.Random.Range(-60f, 60f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(5f, 7f), SpeedType.Absolute), new SpeedUpBullet());
				base.Fire(new Direction(UnityEngine.Random.Range(-60f, 60f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(6f, 8f), SpeedType.Absolute), new SpeedUpBullet());
				yield return Wait(4);
			}
			yield break;
		}

	}
	public class SniperScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
			}
			int shots = UnityEngine.Random.Range(2, 6);
			for (int e = 0; e < shots; e++)
			{
				base.PostWwiseEvent("Play_WPN_sniperrifle_shot_01", null);
				for (int u = -2; u < 1; u++)
				{
					for (int h = 0; h < 2; h++)
					{
						base.Fire(new Direction((25 * u), DirectionType.Aim, -1f), new Speed(11.5f + h, SpeedType.Absolute), new WallBullet());

					}

				}
				for (int p = 0; p < 4; p++)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(-60f, 60f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(8f, 10f), SpeedType.Absolute), new WallBullet());
				}
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12.5f - (shots / 3), SpeedType.Absolute), new WallBullet());
				base.PostWwiseEvent("Play_WPN_sniperrifle_shot_01", null);
				yield return Wait(15 + shots);
				for (int u = 0; u < 1; u++)
				{
					for (int h = 0; h < 2; h++)
					{
						base.Fire(new Direction(25f, DirectionType.Aim, -1f), new Speed(11.5f + h, SpeedType.Absolute), new WallBullet());
						base.Fire(new Direction(-25f, DirectionType.Aim, -1f), new Speed(11.5f + h, SpeedType.Absolute), new WallBullet());
					}
					for (int p = 0; p < 3; p++)
					{
						base.Fire(new Direction(UnityEngine.Random.Range(-60f, 60f), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(8f, 10f), SpeedType.Absolute), new WallBullet());
					}

				}
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12.5f - (shots / 3), SpeedType.Absolute), new WallBullet());
				yield return Wait(15 + shots);
			}
			base.PostWwiseEvent("Play_ENM_hammer_target_01", null);
			yield return Wait(30);
			base.PostWwiseEvent("Play_BOSS_RatMech_Stomp_01", null);
			for (int u = 0; u < 4; u++)
			{
				for (int h = 0; h < 20; h++)
				{
					base.Fire(new Direction(18 * h + (9 * u), DirectionType.Aim, -1f), new Speed(11 + u, SpeedType.Absolute), new WallBullet());

				}
			}

			yield break;
		}

	}
	public class CannonScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("da797878d215453abba824ff902e21b4").bulletBank.GetBullet("snakeBullet"));
			}
			AkSoundEngine.PostEvent("Play_BOSS_RatMech_Cannon_01", base.BulletBank.gameObject);
			this.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(1f, SpeedType.Absolute), new CannonScript.SplitBall());
			this.Fire(new Direction(120f, DirectionType.Aim, -1f), new Speed(1f, SpeedType.Absolute), new CannonScript.SplitBall());
			this.Fire(new Direction(240f, DirectionType.Aim, -1f), new Speed(1f, SpeedType.Absolute), new CannonScript.SplitBall());

			yield break;
			//yield return base.Wait(20);
		}
		public class SplitBall : Bullet
		{
			// Token: 0x06000007 RID: 7 RVA: 0x000021A2 File Offset: 0x000003A2
			public SplitBall() : base("bigBullet", false, false, false)
			{
			}
			protected override IEnumerator Top()
			{
				base.ChangeSpeed(new Speed(30f, SpeedType.Absolute), 120);
				yield break;
			}
			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
				}
				if (!preventSpawningProjectiles)
				{
					float num = base.RandomAngle();
					float Amount = 16;
					float Angle = 360 / Amount;
					for (int i = 0; i < Amount; i++)
					{
						base.Fire(new Direction(num + Angle * (float)i, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new WallBullet());
					}
					for (int i = -1; i < 2; i++)
					{
						base.Fire(new Direction((10 * i), DirectionType.Aim, -1f), new Speed(1f, SpeedType.Absolute), new SpeedUpBullet());
					}
				}
			}
		}
		// Token: 0x04000091 RID: 145		
	}


	public class BurstBullet : Bullet
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x000085A7 File Offset: 0x000067A7
		public BurstBullet() : base("reversible", false, false, false)
		{
		}

		protected override IEnumerator Top()
		{
			this.Projectile.spriteAnimator.Play();
			yield break;
		}
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				base.Fire(new Direction((float)(i * 45), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new WallBullet());
			}
		}
	}

	public class SpeedUpBullet : Bullet
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
		public SpeedUpBullet() : base("reversible", false, false, false)
		{
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00030B48 File Offset: 0x0002ED48
		protected override IEnumerator Top()
		{
			float speed = this.Speed;
			yield return this.Wait(100);
			this.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 20);
			yield break;
		}
	}

	public class WallBullet : Bullet
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x00030B38 File Offset: 0x0002ED38
		public WallBullet() : base("default", false, false, false)
		{
		}

	}
	public class ChuckScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("880bbe4ce1014740ba6b4e2ea521e49d").bulletBank.GetBullet("grenade"));
			}
			float airTime = base.BulletBank.GetBullet("grenade").BulletObject.GetComponent<ArcProjectile>().GetTimeInFlight();
			Vector2 vector = this.BulletManager.PlayerPosition();
			Bullet bullet2 = new Bullet("grenade", false, false, false);
			float direction2 = (vector - base.Position).ToAngle();
			base.Fire(new Direction(direction2, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet2);
			(bullet2.Projectile as ArcProjectile).AdjustSpeedToHit(vector);
			bullet2.Projectile.ImmuneToSustainedBlanks = true;
			yield break;
		}

	}


	public class PleaseJustFuckingGlow : EnemyBehavior
	{
		public void Start()
		{

		}
	}


	public class EnemyBehavior : BraveBehaviour
	{




		private void Start()
		{
			Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
			mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 50);
			aiActor.sprite.renderer.material = mat;
			//base.aiActor.HasBeenEngaged = true;
			base.aiActor.healthHaver.OnPreDeath += (obj) =>
			{
				//AkSoundEngine.PostEvent("Play_ENM_beholster_death_01", base.aiActor.gameObject);
				//Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(spawnspot)rg;
				//chest2.IsLocked = false;

			};
			base.healthHaver.healthHaver.OnDeath += (obj) =>
			{


			}; ;
			this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

		}


	}
}