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
	public class Lost : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "bot:lost";
		private static tk2dSpriteCollectionData LostCollection;


		public static void Init()
		{

				Lost.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e");
				prefab = EnemyBuilder.BuildPrefab("Lost", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), true);
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


				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.None,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle",
					}
				};
				/*
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
				};*/
				
				bool flag3 = LostCollection == null;
				if (flag3)
				{
					LostCollection = SpriteBuilder.ConstructCollection(prefab, "lost_Collection");
					UnityEngine.Object.DontDestroyOnLoad(LostCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], LostCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, LostCollection, new List<int>
					{

						0,
						1,
						2,
						3

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
				}

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e").behaviorSpeculator;

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
				EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, aIActor.aiShooter, aIActor.GetComponent<AIBulletBank>(), 145, m_CachedGunAttachPoint.transform);
			
				Game.Enemies.Add("bot:lost", companion.aiActor);


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
			"BotsMod/sprites/lost/guide_idle_front_001",
			"BotsMod/sprites/lost/guide_idle_front_002",//1
			"BotsMod/sprites/lost/guide_idle_front_003",//1
			"BotsMod/sprites/lost/guide_idle_front_004",//1

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

	





	

