using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;

using static BotsMod.PirmalShotgrub;
using System.Net.NetworkInformation;

namespace BotsMod
{
	public class Roomba : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "bot:roomba"; //give your companion some unique guid
		float damageBuff = -1;

		public static void Init()
		{
			string itemName = "Roomba";
			string resourceName = "BotsMod/sprites/wip";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<Roomba>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "wip";
			string longDesc = "no";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.D;
			item.CompanionGuid = guid; //this will be used by the item later to pull your companion from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
			//item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 5f);

			item.CompanionGuid = Roomba.guid;
			item.CanBeDropped = false;
			BuildPrefab();
			item.PlaceItemInAmmonomiconAfterItemById(664);

		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			//prefab = CompanionBuilder.BuildPrefab("mistake", guid, "BotsMod/sprites/Pets/mistake/Idle_front/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
			prefab = CompanionBuilder.BuildPrefab("roomba", guid, "BotsMod/sprites/Roomba/roomba_idle_N_001", new IntVector2(1, 0), new IntVector2(9, 9));

			//prefab.AddComponent<HitEffectHandler>();

			//Add a companion component to the prefab (could be a custom class)
			var companion = prefab.AddComponent<RoombaBehaviors>();
			companion.aiActor.MovementSpeed = 6f;
			//companion.aiActor.PathableTiles = CellTypes.WALL;

			

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			//prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle", "BotsMod/sprites/Roomba/roomba_idle_N_001", fps: 5, AnimationType.Idle, DirectionType.Single);
			prefab.AddAnimation("move_back", "BotsMod/sprites/Roomba/roomba_idle_N_001", fps: 5, AnimationType.Idle, DirectionType.FourWayCardinal);
			prefab.AddAnimation("move_right", "BotsMod/sprites/Roomba/roomba_idle_E_001", fps: 5, AnimationType.Idle, DirectionType.FourWayCardinal);
			prefab.AddAnimation("move_front", "BotsMod/sprites/Roomba/roomba_idle_S_001", fps: 5, AnimationType.Idle, DirectionType.FourWayCardinal);
			prefab.AddAnimation("move_left", "BotsMod/sprites/Roomba/roomba_idle_W_001", fps: 5, AnimationType.Idle, DirectionType.FourWayCardinal);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that

			AIActor aIActor = EnemyDatabase.Instance.InternalGetByGuid("3a077fa5872d462196bb9a3cb1af02a3");


			var bs = prefab.GetComponent<BehaviorSpeculator>();

			//bs.MovementBehaviors.Add(

			bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				/*new BotsCompanionFollowPlayerBehavior() {

					PathInterval = 0.25f,
					DisableInCombat = true,
					IdealRadius = 3,
					CatchUpRadius = 8,
					CatchUpAccelTime = 5,
					CatchUpSpeed = 5,
					CatchUpMaxSpeed = 10,
					CatchUpAnimation = "idle",
					CatchUpOutAnimation = "idle",
					RollAnimation = "idle",
					CanRollOverPits = false,
					TemporarilyDisabled = false,

					IdleAnimations = new string[] { "idle" }

				},*/


				new BotsPingPongAroundBehavior()
				{
					motionType = BotsPingPongAroundBehavior.MotionType.Diagonals,
					startingAngles = new float[] { 45.0f, 135.0f, 225.0f, 315.0f },	

				},
				/*
				new MoveErraticallyBehavior()
				{
					AvoidTarget = true,
					InitialDelay = 0,
					PathInterval = 0.25f,
					PointReachedPauseTime = 0.1f,
					PreventFiringWhileMoving = true,
					StayOnScreen = false,
					UseTargetsRoom = true,

				},
				
				new SeekTargetBehavior()
				{
					StopWhenInRange = false,
					CustomRange = -1,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.25f,
					ExternalCooldownSource = false,
					SpecifyRange = true,
					MaxActiveRange = 14,
					MinActiveRange = 0

				},*/
			};


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





			//companion.CanInterceptBullets = true;
			//companion.CanInterceptBullets = true;
			companion.aiActor.healthHaver.PreventAllDamage = true;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = true;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(1f);
			companion.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
			//companion.aiActor.specRigidbody.PixelColliders.Clear();
			/*companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				//CollisionLayer = CollisionLayer.PlayerCollider,
				CollisionLayer = CollisionLayer.TileBlocker,
				IsTrigger = true,
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
				ManualRightY = 0
			});*/
			//companion.aiActor.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox));
			//companion.aiActor.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox));
			companion.aiActor.specRigidbody.OnCollision += Anger;
			//aIActor.specRigidbody.up

		}

		private static void Anger(CollisionData collision)
		{
			BotsModule.Log("AHHHH");
			var m_aiActor = collision.MyRigidbody.GetComponent<AIActor>();
			//var m_aiActor = collision.MyRigidbody.GetComponent<AIActor>();

			if (collision.OtherRigidbody.projectile)
			{
				return;
			}
			if (collision.CollidedX || collision.CollidedY)
			{
				Vector2 vector = collision.MyRigidbody.Velocity;
				if (collision.CollidedX)
				{
					vector.x *= -1f;
				}
				if (collision.CollidedY)
				{
					vector.y *= -1f;
				}
				vector = vector.normalized * m_aiActor.MovementSpeed;
				PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
				m_aiActor.BehaviorVelocity = vector;


				Vector2 BottomOffset = m_aiActor.sprite.WorldTopCenter;
				Vector2 TopOffset = m_aiActor.sprite.WorldTopCenter + new Vector2(1, 1);
				GlobalSparksDoer.DoRandomParticleBurst(10, BottomOffset, TopOffset, new Vector3(-1, 1), 70f, 0.5f, null, new float?(0.75f), new Color(0, 0, 0), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
				AkSoundEngine.PostEvent("Play_Fuck", m_aiActor.gameObject);

			}



		}

		

	}

	public class RoombaBehaviors : CompanionController
	{

		private void Start()
		{

			Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
			mat.SetColor("_EmissiveColor", new Color32(2, 212, 9, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 50);
			aiActor.sprite.renderer.material = mat;
			

			
		}





		public override void Update()
		{
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(base.aiActor.specRigidbody.UnitCenter, 0.7f);


			for (int i = 0; i < StaticReferenceManager.AllCorpses.Count; i++)
			{

				
				GameObject gameObject = StaticReferenceManager.AllCorpses[i];
				if (gameObject && gameObject.GetComponent<tk2dBaseSprite>() && gameObject.transform.position.GetAbsoluteRoom() == base.m_owner.CurrentRoom)
				{
					if (Vector2.Distance(base.aiActor.CenterPosition, gameObject.GetComponent<tk2dBaseSprite>().WorldCenter) <= 1)
					{

						Instantiate<GameObject>(PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject, gameObject.GetComponent<tk2dBaseSprite>().sprite.WorldCenter, Quaternion.identity);

						//Vector2 BottomOffset = gameObject.transform.position;
						//Vector2 TopOffset = gameObject.transform.position + new Vector3(1, 1);
						//var TargetColor = new Color(0.5f, 0.1f, 0.1f);
						
						UnityEngine.Object.Destroy(gameObject.gameObject);
					}
					
					
				}
			}

			for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
			{


				DebrisObject gameObject = StaticReferenceManager.AllDebris[i];
				if (gameObject && gameObject.GetComponent<tk2dBaseSprite>() && gameObject.transform.position.GetAbsoluteRoom() == base.m_owner.CurrentRoom)
				{
					if (true)//gameObject.minorBreakable != null || gameObject.majorBreakable != null)
					{
						if (Vector2.Distance(base.aiActor.CenterPosition, gameObject.GetComponent<tk2dBaseSprite>().WorldCenter) <= 0.7f)
						{

							Vector2 BottomOffset = base.aiActor.CenterPosition;
							Vector2 TopOffset = base.aiActor.CenterPosition + new Vector2(1, 1);
							GlobalSparksDoer.DoRandomParticleBurst(3, BottomOffset, TopOffset, new Vector3(-1, 1), 70f, 0.5f, null, new float?(0.75f), new Color(0, 0, 0), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
							UnityEngine.Object.Destroy(gameObject.gameObject);
						}
					}
				}
			}

			base.Update();
		}
	}
}







