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

using System.Runtime.CompilerServices;
using BotsMod;
using EnemyAPI;
//using EnemyAPI;

namespace BotsMod
{
    class Enforcer : AIActor
	{
		public static GameObject EnforcerPrefab;
		public static readonly string guid = "bot:enforcer";
		public static GameObject shootpoint;
		public static string TargetVFX;
		public static AIActor enforcerAiActor;

		//make teleport ring turn around and go back the other way after like a second

		public static void Init()
		{
			Enforcer.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = EnforcerPrefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{

				EnforcerPrefab = BossBuilder.BuildPrefab("Enforcer", guid, "BotsMod/sprites/Enemies/MiniBosses/Enforcer/Intro/enforcer_intro_025.png", new IntVector2(134, 46), new IntVector2(33, 57), false, true);

				EnforcerPrefab.layer = 28;

				var enemy = EnforcerPrefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.knockbackDoer.weight = 600;
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
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(550);
				enemy.aiActor.healthHaver.SetHealthMaximum(550);
				enemy.aiActor.CollisionKnockbackStrength = 2f;
				enemy.aiActor.procedurallyOutlined = true;
				enemy.aiActor.CanTargetPlayers = true;

				//enemy.aiActor.MovementSpeed = 0f;

				BotsModule.Strings.Enemies.Set("#THE_ENFORCER", "Enforcer");
				BotsModule.Strings.Enemies.Set("#BOT_ENFORCER_SUBTITLE", "All Seeing Eye");
				BotsModule.Strings.Enemies.Set("#BOT_ENFORCERT_QUOTE", "");
				enemy.aiActor.healthHaver.overrideBossName = "#THE_ENFORCER";
				enemy.aiActor.OverrideDisplayName = "#THE_ENFORCER";
				enemy.aiActor.ActorName = "#THE_ENFORCER";
				enemy.aiActor.name = "#THE_ENFORCER";
				EnforcerPrefab.name = enemy.aiActor.OverrideDisplayName;
				ETGModConsole.Log("1");

				GenericIntroDoer miniBossIntroDoer = EnforcerPrefab.AddComponent<GenericIntroDoer>();
				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Ending_Pilot_01";
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
					bossNameString = "#THE_ENFORCER",
					bossSubtitleString = "#BOT_ENFORCER_SUBTITLE",
					bossQuoteString = "#BOT_ENFORCER_QUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = new Color32(255, 0, 247, 255)
				};
				miniBossIntroDoer.SkipBossCard = true;
				enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
				ETGModConsole.Log("2");
				SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Enemies/MiniBosses/Enforcer/EnforcerIcon", BeyondPrefabs.ammonomiconCollection, "EnforcerIcon");
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "bot:enforcer";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = "";
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "EnforcerIcon";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = null;

				BotsModule.Strings.Enemies.Set("#BOT_ENFORCER_SHORTDESC", "All Seeing Eye");
				//This fast and ruthless  is known to disorient all who challenge them.
				BotsModule.Strings.Enemies.Set("#BOT_ENFORCER_LONGDESC", "An extremely skilled swordsman and the leader of The Beyond's army.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#THE_ENFORCER";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#BOT_ENFORCER_SHORTDESC";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#BOT_ENFORCER_LONGDESC";
				Tools.AddEnemyToDatabase2(enemy.gameObject, "bot:enforcer", true, true);
				EnemyDatabase.GetEntry("bot:enforcer").ForcedPositionInAmmonomicon = 10000;
				EnemyDatabase.GetEntry("bot:enforcer").isInBossTab = true;
				EnemyDatabase.GetEntry("bot:enforcer").isNormalEnemy = true;
				ETGModConsole.Log("3");
				enemy.aiActor.specRigidbody.PixelColliders.Clear();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 134,
					ManualOffsetY = 46,
					ManualWidth = 33,
					ManualHeight = 57,
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
					ManualOffsetX = 134,
					ManualOffsetY = 46,
					ManualWidth = 33,
					ManualHeight = 57,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,


				});
				ETGModConsole.Log("4");
				AIActor actor2 = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b");
				
				EnforcerPrefab.GetComponent<tk2dSprite>().renderer.sortingLayerID = actor2.sprite.renderer.sortingLayerID;



				enemy.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				enemy.aiActor.PreventBlackPhantom = false;
				EnforcerPrefab.GetOrAddComponent<AIAnimator>().OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>();

				EnforcerPrefab.GetOrAddComponent<AIAnimator>().OtherVFX = EnemyDatabase.GetOrLoadByGuid("19b420dec96d4e9ea4aebc3398c0ba7a").gameObject.GetComponent<AIAnimator>().OtherVFX;


				EnforcerPrefab.AddAnimation("idle", "BotsMod/sprites/Enemies/MiniBosses/Enforcer/Idle", 3, EnemyBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

				var introAnim = EnforcerPrefab.AddAnimation("intro", "BotsMod/sprites/Enemies/MiniBosses/Enforcer/Intro", 8, EnemyBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);


				introAnim.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				ETGModConsole.Log("5");

				var bs = EnforcerPrefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = enemy.transform;
				GameObject m_CachedGunAttachPoint = enemy.transform.Find("attach").gameObject;

				ETGModConsole.Log("6");
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
					


				};

				//companion.bulletBank
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;

				enforcerAiActor = enemy.aiActor;

				Game.Enemies.Add("bot:enforcer", enemy.aiActor);

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
					//LostPastController convictPastController = UnityEngine.Object.FindObjectOfType<LostPastController>();
					//convictPastController.OnBossKilled(base.transform);

				};
				base.healthHaver.healthHaver.OnDeath += (obj) =>
				{


				}; ;
				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

			}


		}

	}    
}
