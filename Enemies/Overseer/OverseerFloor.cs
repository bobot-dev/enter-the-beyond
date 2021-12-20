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


	class OverseerFloor : AIActor
	{
		public static GameObject OverseerPrefab;
		public static readonly string guid = "bot:Overseer";
		public static GameObject shootpoint;
		private static Texture2D BossCardTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\overseer_floor\\overseerf_Bosscard_001.png");
		public static string TargetVFX;
		public static AIActor overseerAiActor;
		public static GameObject Eye;
		public static GameObject RightArmBand;
		public static GameObject LeftArmBand;
		//make teleport ring turn around and go back the other way after like a second






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

		public static void Init()
		{
			
			OverseerFloor.BuildPrefab();

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
			bool flag = OverseerPrefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				
				OverseerPrefab = BossBuilder.BuildPrefab("Overseer", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);

				OverseerPrefab.layer = 28;

				var enemy = OverseerPrefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.knockbackDoer.weight = 200;
				enemy.aiActor.MovementSpeed = 3.33f;
				enemy.aiActor.healthHaver.PreventAllDamage = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.HasShadow = true;
				enemy.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;
				
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.aiAnimator.HitReactChance = 0.05f;
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.PreventFallingInPitsEver = true;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(1500f);
				enemy.aiActor.healthHaver.SetHealthMaximum(1500f);
				enemy.aiActor.CollisionKnockbackStrength = 2f;
				enemy.aiActor.procedurallyOutlined = true;
				enemy.aiActor.CanTargetPlayers = true;

				//enemy.aiActor.MovementSpeed = 0f;

				BotsModule.Strings.Enemies.Set("#THE_OVERSEER", "Overseer");
				BotsModule.Strings.Enemies.Set("#BOT_????", "???");
				BotsModule.Strings.Enemies.Set("#BOT_SUBTITLE", "All Seeing Eye");
				BotsModule.Strings.Enemies.Set("#BOT_QUOTE", "ahhhh");
				enemy.aiActor.healthHaver.overrideBossName = "#THE_OVERSEER";
				enemy.aiActor.OverrideDisplayName = "#THE_OVERSEER";
				enemy.aiActor.ActorName = "#THE_OVERSEER";
				enemy.aiActor.name = "#THE_OVERSEER";
				OverseerPrefab.name = enemy.aiActor.OverrideDisplayName;
				

				GenericIntroDoer miniBossIntroDoer = OverseerPrefab.AddComponent<GenericIntroDoer>();
				OverseerPrefab.AddComponent<LostPastBossIntro>();
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
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				}
				else
				{
					miniBossIntroDoer.SkipBossCard = true;
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				}
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
				//BehaviorSpeculator aIActor = EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").behaviorSpeculator;
				//Tools.DebugInformation(aIActor);

				/////

				SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/OverseerIcon", SpriteBuilder.ammonomiconCollection);
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "bot:Overseer";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = "";
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "BotsMod/sprites/OverseerIcon";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\overseer_floor\\ammonomicon_enemy_overseer_001.png");

				BotsModule.Strings.Enemies.Set("#BOT_OVERSEER_SHORTDESC", "All Seeing Eye");
				//This fast and ruthless  is known to disorient all who challenge them.
				BotsModule.Strings.Enemies.Set("#BOT_OVERSEER_LONGDESC", "A powerful cult leader from another realm. \n\nVery little is know about this opponent.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#THE_OVERSEER";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#BOT_OVERSEER_SHORTDESC";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#BOT_OVERSEER_LONGDESC";
				Tools.AddEnemyToDatabase2(enemy.gameObject, "bot:Overseer", true, true);
				EnemyDatabase.GetEntry("bot:Overseer").ForcedPositionInAmmonomicon = 10000;
				EnemyDatabase.GetEntry("bot:Overseer").isInBossTab = true;
				EnemyDatabase.GetEntry("bot:Overseer").isNormalEnemy = true;



				var afterImage = enemy.gameObject.AddComponent<AfterImageTrailController>();

				afterImage.dashColor = new Color(180, 32, 42);
				afterImage.spawnShadows = false;
				afterImage.shadowTimeDelay = 0.1f;
				afterImage.shadowLifetime = 0.6f;
				afterImage.minTranslation = 0.2f;
				afterImage.maxEmission = 800;
				afterImage.minEmission = 100;
				afterImage.targetHeight = -2;
				afterImage.dashColor = new Color(1, 0, 0);

				//GameObject EnemyPrefab = companion.gameObject;

				//if (!string.IsNullOrEmpty(EnemyPrefab.GetComponent<AIActor>().ActorName))
				//{
				//	string EnemyName = "bot:" + EnemyPrefab.GetComponent<AIActor>().ActorName.Replace(" ", "_").Replace("(", "_").Replace(")", string.Empty).ToLower();
				//	if (!Game.Enemies.ContainsID(EnemyName)) { Game.Enemies.Add(EnemyName, EnemyPrefab.GetComponent<AIActor>()); }
				//}




				//enemy.aiActor.healthHaver.SetHealthMaximum(800, null, false);
				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 52,
					ManualOffsetY = 5,
					ManualWidth = 23,
					ManualHeight = 79,
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
					ManualOffsetX = 52,
					ManualOffsetY = 5,
					ManualWidth = 23,
					ManualHeight = 79,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});


				Eye = new GameObject("OverseersEye");
				Eye.transform.parent = enemy.transform;
				Eye.transform.position = new Vector2(4f, 4.5f);//64, 72

				RightArmBand = new GameObject("OverseersRightArmBand");
				RightArmBand.transform.parent = enemy.transform;
				RightArmBand.transform.position = new Vector2(5f, 3.25f);//80, 52

				LeftArmBand = new GameObject("OverseersLeftArmBand");
				LeftArmBand.transform.parent = enemy.transform;
				LeftArmBand.transform.position = new Vector2(3f, 3.25f);//48, 52


				GameObject OverseersEye = enemy.transform.Find("OverseersEye").gameObject;
				GameObject OverseersRightArmBand = enemy.transform.Find("OverseersRightArmBand").gameObject;
				GameObject OverseersLeftArmBand = enemy.transform.Find("OverseersLeftArmBand").gameObject;


				var beams = Tools.ExtremeLaziness(OverseerPrefab, Eye.transform, 5, new List<string> { "fuckyoulaser1", "fuckyoulaser2", "fuckyoulaser3", "fuckyoulaser4", "fuckyoulaser5", "fuckyoulaser6", });



				AIBeamShooter2 bholsterbeam1 = enemy.gameObject.AddComponent<AIBeamShooter2>();
				AIActor actor2 = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b");
				BeholsterController beholsterbeam = actor2.GetComponent<BeholsterController>();
				bholsterbeam1.beamTransform = Eye.transform;
				bholsterbeam1.beamModule = beholsterbeam.beamModule;
				//bholsterbeam1.beamProjectile = beholsterbeam.beamModule.GetCurrentProjectile();
				bholsterbeam1.beamProjectile = beholsterbeam.projectile;
				
				//

				bholsterbeam1.firingEllipseCenter = Eye.transform.position;
				bholsterbeam1.name = "OverseerEyeBeamButBiger";
				bholsterbeam1.northAngleTolerance = 8;

				var trail = OverseersEye.gameObject.AddComponent<TrailRenderer>();
				trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				trail.receiveShadows = false;
				var mat = new Material(Shader.Find("Sprites/Default"));
				mat.SetColor("_Color", Color.magenta);
				trail.material = mat;
				trail.time = 0.7f;
				trail.minVertexDistance = 0.1f;
				trail.startWidth = 1f;
				trail.endWidth = 0f;
				trail.startColor = Color.white;
				trail.endColor = new Color(1f, 1f, 1f, 0f);
				trail.enabled = false;
				trail.sortingLayerID = LayerMask.NameToLayer("Unpixelated");

				var trail2 = OverseersRightArmBand.gameObject.AddComponent<TrailRenderer>();
				trail2.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				trail2.receiveShadows = false;
				var mat2 = new Material(Shader.Find("Sprites/Default"));
				mat2.SetColor("_Color", Color.magenta);
				trail2.material = mat2;
				trail2.time = 0.5f;
				trail2.minVertexDistance = 0.1f;
				trail2.startWidth = 0.3f;
				trail2.endWidth = 0f;
				trail2.startColor = Color.white;
				trail2.endColor = new Color(1f, 1f, 1f, 0f);
				trail2.enabled = false;
				trail2.sortingLayerID = LayerMask.NameToLayer("Unpixelated");

				var trail3 = OverseersLeftArmBand.gameObject.AddComponent<TrailRenderer>();
				trail3.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				trail3.receiveShadows = false;
				var mat3 = new Material(Shader.Find("Sprites/Default"));
				mat3.SetColor("_Color", Color.magenta);
				trail3.material = mat3;
				trail3.time = 0.5f;
				trail3.minVertexDistance = 0.1f;
				trail3.startWidth = 0.3f;
				trail3.endWidth = 0f;
				trail3.startColor = Color.white;
				trail3.endColor = new Color(1f, 1f, 1f, 0f);
				trail3.enabled = false;
				trail3.sortingLayerID = LayerMask.NameToLayer("Unpixelated");

				OverseerPrefab.GetComponent<tk2dSprite>().renderer.sortingLayerID = actor2.sprite.renderer.sortingLayerID;



				enemy.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				enemy.aiActor.PreventBlackPhantom = false;
				OverseerPrefab.GetOrAddComponent<AIAnimator>().OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();

				OverseerPrefab.GetOrAddComponent<AIAnimator>().OtherVFX = EnemyDatabase.GetOrLoadByGuid("19b420dec96d4e9ea4aebc3398c0ba7a").gameObject.GetComponent<AIAnimator>().OtherVFX;


				OverseerPrefab.AddAnimation("idle", "BotsMod/sprites/overseer_floor/idle", 3, AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

				OverseerPrefab.AddAnimation("intro", "BotsMod/sprites/overseer_floor/intro", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				OverseerPrefab.AddAnimation("death", "BotsMod/sprites/overseer_floor/death", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;

				OverseerPrefab.AddAnimation("blank", "BotsMod/sprites/overseer_floor/blank", 4, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;

				OverseerPrefab.AddAnimation("beam_tell", "BotsMod/sprites/overseer_floor/TelBeam", 16, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;

				OverseerPrefab.AddAnimation("beam", "BotsMod/sprites/overseer_floor/beam", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

				OverseerPrefab.AddAnimation("gaurd", "BotsMod/sprites/overseer_floor/gaurd", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
				
				OverseerPrefab.AddAnimation("remote_end", "BotsMod/sprites/overseer_floor/remoteEnd", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Single;


				var beamTel = OverseerPrefab.AddAnimation("remote_tell", "BotsMod/sprites/overseer_floor/remoteTell", 8, AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
				beamTel.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
				beamTel.loopStart = 6;

				OverseerPrefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("blank").frames[2].eventInfo = "blank";

				var bs = OverseerPrefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = Eye.transform.position;
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
										/*new AttackBehaviorGroup.AttackGroupItem()
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

									teleportOutBulletScript = new CustomBulletScriptSelector(typeof(OverseerTeleportStartLinesScript)),
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
								new TwoBeamsBehavior() {
									//ShootPoint = m_CachedGunAttachPoint,
									initialAimOffset = -15f,
									InitialCooldown = 2f,
									firingTime = 14f,
									AttackCooldown = 2f,
									RequiresLineOfSight = true,
									BulletScript =  new CustomBulletScriptSelector(typeof(SixBeamScript2)),
									//BulletScript =  new CustomBulletScriptSelector(typeof(SixBeamScript)),
									chargeTime = 2,
									UsesBaseSounds = true,
									LaserFiringSound = "Play_ENM_deathray_shot_01",
									StopLaserFiringSound = "Stop_ENM_deathray_loop_01",
									ChargeAnimation = "beam_tell",
									FireAnimation = "beam",
									PostFireAnimation = "",
									beamSelection = ShootBeamBehavior.BeamSelection.All,
									//initialAimType = CustomShootBeamBehavior.InitialAimType.Aim,
									maxTurnRate = 32f,
									turnRateAcceleration = 24,
									turnRateMultiplier = 1.5f,
									LockInPlaceWhileAttacking = true,
									ShootPoint = m_CachedGunAttachPoint.transform,
									//BulletScript = new CustomBulletScriptSelector(typeof(Wailer.Wail))
									MinWallDistance = 5,
								},
							},
							OverrideCooldowns = new List<float>
							{
								0
							},
							RunInClass = false,

						},
						NickName = "Beam Teleport Magic bs"

					},*/

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

									teleportOutBulletScript = new CustomBulletScriptSelector(typeof(OverseerTeleportStartLinesScript)),
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
								new FireLotsOfLasersBehaviour() {
									//ShootPoint = m_CachedGunAttachPoint,
									initialAimOffset = -15f,
									InitialCooldown = 2f,
									firingTime = 14f,
									AttackCooldown = 2f,
									RequiresLineOfSight = true,
									BulletScript =  new CustomBulletScriptSelector(typeof(SixBeamScript2)),
									//BulletScript =  new CustomBulletScriptSelector(typeof(SixBeamScript)),
									chargeTime = 2,
									UsesBaseSounds = true,
									LaserFiringSound = "Play_ENM_deathray_shot_01",
									StopLaserFiringSound = "Stop_ENM_deathray_loop_01",
									ChargeAnimation = "beam_tell",
									FireAnimation = "beam",
									PostFireAnimation = "",
									beamSelection = ShootBeamBehavior.BeamSelection.All,
									//initialAimType = CustomShootBeamBehavior.InitialAimType.Aim,
									maxTurnRate = 32f,
									turnRateAcceleration = 24,
									turnRateMultiplier = 1.5f,
									LockInPlaceWhileAttacking = true,
									ShootPoint = m_CachedGunAttachPoint.transform,
									//BulletScript = new CustomBulletScriptSelector(typeof(Wailer.Wail))
									MinWallDistance = 5,
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

					//secondBulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines2)),
					new AttackBehaviorGroup.AttackGroupItem()
					{
						//Probability = 0.7f,
						Probability = 1f,

						Behavior = new SequentialAttackBehaviorGroup()
						{
							

							AttackBehaviors = new List<AttackBehaviorBase>
							{
								//5
								#region dash1
								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.TowardTarget,
									quantizeDirection = 0,
									dashDistance = 15,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),									
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion//5
								//8
								#region dash2


								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.Random,
									quantizeDirection = 0,
									dashDistance = 8,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//5
								#region dash1
								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.TowardTarget,
									quantizeDirection = 0,
									dashDistance = 5,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//8
								#region dash2


								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.Random,
									quantizeDirection = 0,
									dashDistance = 8,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//5
								#region dash1
								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.TowardTarget,
									quantizeDirection = 0,
									dashDistance = 5,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//8
								#region dash2


								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.Random,
									quantizeDirection = 0,
									dashDistance = 8,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//5
								#region dash1
								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.TowardTarget,
									quantizeDirection = 0,
									dashDistance = 5,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
								//8
								#region dash2


								new DashButGoodBehavior
								{
									dashDirection = DashButGoodBehavior.DashDirection.Random,
									quantizeDirection = 0,
									dashDistance = 8,
									dashTime = 0.4f,
									postDashSpeed = 0,
									doubleDashChance = 0,
									avoidTarget = true,
									ShootPoint = m_CachedGunAttachPoint,
									bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines2)),
									fireAtDashStart = true,
									stopOnCollision = false,
									warpDashAnimLength = false,
									hideGun = false,
									hideShadow = false,
									toggleTrailRenderer = true,
									enableShadowTrail = true,
									Cooldown = 1,
									CooldownVariance = 2,
									GlobalCooldown = 0.5f,
									InitialCooldown = 2,
									InitialCooldownVariance = 0,
									GroupCooldown = 0,
									GroupName = "",
									Range = 0,
									MinRange = 0,
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
									AttackCooldown = 0,
									doDodgeDustUp = false,
									chargeAnim = "",
									dashAnim = "idle",
								},
								#endregion
							},
							OverrideCooldowns = new List<float>{ 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.01f, 0.5f },
							RunInClass = false,
						},
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						//Probability = 0.7f,
						Probability = 0.5f,

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

									teleportOutBulletScript = new CustomBulletScriptSelector(typeof(OverseerTeleportStartDoubleLinesScript)),
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
								new ShootBehavior{
									ShootPoint = m_CachedGunAttachPoint,
									BulletScript = new CustomBulletScriptSelector(typeof(OverseerTwinBeams1)),
									LeadAmount = 0f,
									AttackCooldown = 1.2f,
									TellAnimation = "",
									FireAnimation = "",
									RequiresLineOfSight = false,
									Cooldown = 6f,
									MinWallDistance = 0,
									StopDuring = ShootBehavior.StopType.Attack,
									Uninterruptible = true
								},
							},
							OverrideCooldowns = new List<float>
							{
								0,
								0
							},
							RunInClass = false,

						},
						NickName = "EYE Teleport Magic bs"

					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 4f,
						Behavior = new DashButGoodBehavior
						{
							dashDirection = DashButGoodBehavior.DashDirection.PerpendicularToTarget,
							quantizeDirection = 0,
							dashDistance = 16,
							dashTime = 0.2f,
							postDashSpeed = 0,
							doubleDashChance = -1f,
							avoidTarget = true,
							ShootPoint = m_CachedGunAttachPoint,
							bulletScript = new CustomBulletScriptSelector(typeof(OverseerDashAtPlayerScript)),
							fireAtDashStart = true,
							stopOnCollision = false,
							warpDashAnimLength = false,
							hideGun = false,
							hideShadow = false,
							toggleTrailRenderer = true,
							enableShadowTrail = true,
							Cooldown = 1,
							CooldownVariance = 1,
							GlobalCooldown = 0.5f,
							InitialCooldown = 1,
							InitialCooldownVariance = 0,
							GroupCooldown = 0,
							GroupName = "",
							Range = 0,
							MinRange = 0,
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
							AttackCooldown = 0,
							doDodgeDustUp = false,
							chargeAnim = "",
							dashAnim = "idle",
						},
						NickName = "dash at player"
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 2f,
						Behavior = new DashButGoodBehavior
						{
							dashDirection = DashButGoodBehavior.DashDirection.Random,
							quantizeDirection = 0,
							dashDistance = 8,
							dashTime = 0.4f,
							postDashSpeed = 0,
							doubleDashChance = 0.6f,
							avoidTarget = true,
							ShootPoint = m_CachedGunAttachPoint,
							bulletScript = new CustomBulletScriptSelector(typeof(OverseerDashStartScript)),
							fireAtDashStart = true,
							stopOnCollision = false,
							warpDashAnimLength = false,
							hideGun = false,
							hideShadow = false,
							toggleTrailRenderer = true,
							enableShadowTrail = true,
							Cooldown = 1,
							CooldownVariance = 2,
							GlobalCooldown = 0.5f,
							InitialCooldown = 2,
							InitialCooldownVariance = 0,
							GroupCooldown = 0,
							GroupName = "",
							Range = 0,
							MinRange = 0,
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
							AttackCooldown = 0,
							doDodgeDustUp = false,
							chargeAnim = "",
							dashAnim = "idle",
						},
						NickName = "looks cool"
					},

					/*new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 2f,
						Behavior = new DashAndShootBehavior
						{
							dashDirection = DashAndShootBehavior.DashDirection.Random,
							quantizeDirection = 0,
							dashDistance = 8,
							dashTime = 0.4f,
							postDashSpeed = 0,
							dashCount = 8,
							avoidTarget = true,
							ShootPoint = m_CachedGunAttachPoint,
							bulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
							secondBulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines2)),
							fireAtDashStart = true,
							stopOnCollision = false,
							warpDashAnimLength = false,
							hideGun = false,
							hideShadow = false,
							toggleTrailRenderer = true,
							enableShadowTrail = true,
							Cooldown = 1,
							CooldownVariance = 2,
							GlobalCooldown = 0.5f,
							InitialCooldown = 2,
							InitialCooldownVariance = 0,
							GroupCooldown = 0,
							GroupName = "",
							Range = 0,
							MinRange = 0,
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
							AttackCooldown = 0,
							doDodgeDustUp = false,
							chargeAnim = "",
							dashAnim = "idle",
						},
						NickName = "dash lots and shoot lines ok? cool"
					},*/

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1f,
						Behavior = new RemoteShootBehavior
						{
							DefineRadius = true,
							MinRadius = 4,
							MaxRadius = 12,
							RemoteFootprint = new IntVector2(1,1),
							TellTime = 0.8f,
							remoteBulletScript = new CustomBulletScriptSelector(typeof(OverseerRemoteBullets)),
							FireTime = 3.5f,
							Multifire = true,
							MinShots = 2,
							MaxShots = 5,
							MidShotTime = 0.7f,
							ShootVfx = "magic",
							PostFireAnim = "remote_end",
							TellAnim = "remote_tell",
							StopDuringAnimation = true,
							

						},
						NickName = "remote pew pew"
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 1f,
						//Behavior = new DestroyBulletsBehavior
						Behavior = new EnemyBlankBehavior
						{
							BlankAnimation = "gaurd",
							TellAnimation = "",
							BlankTime = 3,
							BlankVfx = "screech",
							OverrideHitVfx = null,
							Radius = 6,
							SkippableCooldown = 1,
							SkippableRadius = 5,
							ShootPoint = m_CachedGunAttachPoint,
							/*RadiusCurve = UnityEngine.JsonUtility.FromJson("{\"keys\":[{\"time\":0.0,\"value\":0.0,\"tangentMode\":1,\"inTangent\":1.99555587768555,\"outTangent\":3.72456574440002},{\"time\":0.5008305311203,\"value\":0.999435305595398,\"tangentMode\":17,\"inTangent\":1.39006745815277,\"outTangent\"" +
							":-0.00450428389012814},{\"time\":0.860195815563202,\"value\":0.997816622257233,\"tangentMode\":5,\"inTangent\":-0.00450428389012814,\"outTangent\":-1.26312339305878},{\"time\":0.999110162258148,\"value\":0.0,\"tangentMode\":10,\"inTangent\":-7.1829628944397,\"outTangent\":-7.1829628944397}]}",
							typeof(AnimationCurve)) as AnimationCurve,*/
							
							Cooldown = 16,
							CooldownVariance = 0,
							GlobalCooldown = 0.5f,
							InitialCooldown = 10,
							InitialCooldownVariance = 0,
							GroupCooldown = 0,
							GroupName = "",
							Range = 0,
							MinRange = 0,
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
							AttackCooldown = 0,

						},
						NickName = "sheild 2"
					},

					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0.01f,
						//Behavior = new DestroyBulletsBehavior
						Behavior = new SPIIIIINBehavior
						{
							SpinAnimation = "",
							Speed = 10,
							TellAnimation = "",
							SpinTime = 10,
							startingAngles = new float[]
							{
								45f,
								135f,
								225f,
								315f
							},
							ShootPoint = m_CachedGunAttachPoint,
							bulletScript = new CustomBulletScriptSelector(typeof(SPIIIIIIIIIIIIIN)),

							Cooldown = 7,
							CooldownVariance = 0,
							GlobalCooldown = 0.5f,
							InitialCooldown = 10,
							InitialCooldownVariance = 0,
							GroupCooldown = 0,
							GroupName = "",
							Range = 0,
							MinRange = 0,
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
							AttackCooldown = 0,

						},
						NickName = "poor choices"
					},
					/*new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 2f,
						Behavior = new ShootBehavior
						{
							ShootPoint = m_CachedGunAttachPoint,
							BulletScript = new CustomBulletScriptSelector(typeof(OverseerRapidFireLines)),
							LeadAmount = 0f,
							AttackCooldown = 1.2f,
							TellAnimation = "",
							FireAnimation = "",
							RequiresLineOfSight = false,
							Cooldown = 3f,

							StopDuring = ShootBehavior.StopType.Attack,
							Uninterruptible = true
						},
						NickName = "lines"
					},

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
					},*/
					
					new AttackBehaviorGroup.AttackGroupItem()
					{
						Probability = 0f,
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SheildScript)),
						LeadAmount = 0f,
						AttackCooldown = 1.2f,
						TellAnimation = "",
						FireAnimation = "",
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
						Behavior = new ShootBehavior{
						ShootPoint = m_CachedGunAttachPoint,
						BulletScript = new CustomBulletScriptSelector(typeof(SixBeamScript2)),
						LeadAmount = 0f,
						AttackCooldown = 1.2f,
						TellAnimation = "",
						FireAnimation = "",
						RequiresLineOfSight = false,
						Cooldown = 3f,

						StopDuring = ShootBehavior.StopType.Attack,
						Uninterruptible = true
						},
						NickName = "test 1"
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
							teleportInAnim = "",
							teleportOutAnim = "",
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

				overseerAiActor = enemy.aiActor;

				enemy.gameObject.AddComponent<OverseerGlow>();
				//enemy.gameObject.AddComponent<BossFinalLostDeathController>();

				OverseerShield.newOwner = enemy.gameActor;



				Game.Enemies.Add("bot:the_overseer", enemy.aiActor);

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
					LostPastController convictPastController = UnityEngine.Object.FindObjectOfType<LostPastController>();
					convictPastController.OnBossKilled(base.transform);

				};
				base.healthHaver.healthHaver.OnDeath += (obj) =>
				{


				}; ;
				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

			}


		}

	}
}