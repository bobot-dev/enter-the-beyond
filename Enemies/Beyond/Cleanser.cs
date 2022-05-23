using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using EnemyAPI;

namespace BotsMod
{
	class Cleanser : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:cleanser";
		private static tk2dSpriteCollectionData CleanserCollection;


		public static void Init()
		{
			//BotsMod.BotsModule.Log("hello???? can you run the code or not?");
			Cleanser.BuildPrefab();
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
					prefab = EnemyBuilder.BuildPrefab("Cleanser", guid, "BotsMod/sprites/Enemies/Cleanser/Idle/cleanser_idle_001.png", new IntVector2(52, 8), new IntVector2(20, 49), true);
					var companion = prefab.AddComponent<EnemyBehavior>();

					companion.aiActor.knockbackDoer.weight = 800;
					companion.aiActor.MovementSpeed = 3f;
					companion.aiActor.healthHaver.PreventAllDamage = false;
					companion.aiActor.CollisionDamage = 1f;
					companion.aiActor.HasShadow = true;
					companion.aiActor.IgnoreForRoomClear = false;
					companion.aiActor.aiAnimator.HitReactChance = 0f;
					companion.aiActor.specRigidbody.CollideWithOthers = true;
					companion.aiActor.specRigidbody.CollideWithTileMap = true;
					companion.aiActor.PreventFallingInPitsEver = false;
					companion.aiActor.CollisionKnockbackStrength = 5f;
					companion.aiActor.CanTargetPlayers = true;
					companion.aiActor.healthHaver.SetHealthMaximum(60, null, false);
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
						ManualOffsetX = 52,
						ManualOffsetY = 8,
						ManualWidth = 20,
						ManualHeight = 49,
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
						ManualOffsetX = 52,
						ManualOffsetY = 8,
						ManualWidth = 20,
						ManualHeight = 49,
						ManualDiameter = 0,
						ManualLeftX = 0,
						ManualLeftY = 0,
						ManualRightX = 0,
						ManualRightY = 0
					});
					companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").CorpseObject;
					companion.aiActor.PreventBlackPhantom = false;

					companion.gameObject.AddAnimation("idle", "BotsMod/sprites/Enemies/Cleanser/Idle", 6, EnemyBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
					var attackRing = companion.gameObject.AddAnimation("attack_ring", "BotsMod/sprites/Enemies/Cleanser/AttackLine", 5, EnemyBuilder.AnimationType.Other, DirectionalAnimation.DirectionType.Single, DirectionalAnimation.FlipType.None);
					attackRing.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
					attackRing.frames[1].triggerEvent = true;
					attackRing.frames[1].eventInfo = "spew";


					var rightArm = new GameObject("rightArm");
					rightArm.transform.parent = prefab.transform;
					rightArm.transform.position = new Vector2(3, 0.75f);//64, 72

					var leftArm = new GameObject("leftArm");
					leftArm.transform.parent = prefab.transform;
					leftArm.transform.position = new Vector2(5.0625f, 0.75f);//64, 72

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
					bs.AttackBehaviors = new List<AttackBehaviorBase>()
					{
						new SpawnGoopLinesBehavior
						{
							goopToUse = BeyondPrefabs.BeyondFireGoop,
							spewAnimation = "attack_ring",
							goopPointR = rightArm.transform,
							goopPointL = leftArm.transform,
							goopRadius = 0.5f,
							goopLineLength = 10,
							goopDuration = 0.3f,
							

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
					m_CachedGunAttachPoint.transform.localPosition = new Vector3(0.5625f, 0.875f, 0);

					EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, null, aIActor.GetComponent<AIBulletBank>(), -1, m_CachedGunAttachPoint.transform, overrideHandObject: BeyondPrefabs.basicBeyondHands);
					

					Game.Enemies.Add("bot:cleanser", companion.aiActor);
					//ETGModConsole.Log("cool");
				}
			}
			catch (Exception message)
			{
				ETGModConsole.Log("AAAAAAAAAAAA: " + message);

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
				mat.SetFloat("_EmissivePower", 35);
				aiActor.sprite.renderer.material = mat;
				base.aiActor.OverrideBlackPhantomShader = BeyondPrefabs.BeyondJammedShader;
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{

					DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(BeyondPrefabs.BeyondFireGoop).AddGoopCircle(base.aiActor.specRigidbody.UnitCenter, 3, -1, false, -1);


					AkSoundEngine.PostEvent("Play_OBJ_skeleton_collapse_01", base.aiActor.gameObject);
				};
			}


		}

	}
}
