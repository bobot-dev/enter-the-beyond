using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using FrostAndGunfireItems;
using System.Collections;

namespace BotsMod
{
	public class LostFriend : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "bot:lost_friend"; //give your chanceKinBehavioar some unique guid

		static List<string> BeamAnimPaths = new List<string>()
		{
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_001",
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_002",

		};
		static List<string> ImpactAnimPaths = new List<string>()
		{
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_impact_001",
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_impact_002",
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_impact_003",
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_impact_004",
		};

		static List<string> StartAnimPaths = new List<string>()
		{
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_flare_001",
			"BotsMod/sprites/beam/LostFriend/lost_friend_laser_flare_002",
		};


		public static void Init()
		{
			string itemName = "IDFK what to call this thing";
			string resourceName = "BotsMod/sprites/LostFriend/idle/lostfriend1";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<LostFriend>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "A Lost Friend";
			string longDesc = "no";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.C;
			item.CompanionGuid = guid; //this will be used by the item later to pull your chanceKinBehavioar from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
															   //item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 5f);

			item.CompanionGuid = LostFriend.guid;
			item.CanBeDropped = true;
			BuildPrefab();
			item.PlaceItemInAmmonomiconAfterItemById(664);

			Tools.BeyondItems.Add(item.PickupObjectId);
		}
		
		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			//prefab = CompanionBuilder.BuildPrefab("mistake", guid, "BotsMod/sprites/Pets/mistake/Idle_front/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
			prefab = CompanionBuilder.BuildPrefab("lostfriend", guid, "BotsMod/sprites/LostFriend/idle/lostfriend1", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a chanceKinBehavioar component to the prefab (could be a custom class)
			//var chanceKinBehavioar = prefab.AddComponent<LostFriendBehavior>();
			LostFriend.LostFriendBehavior chanceKinBehavioar = prefab.AddComponent<LostFriend.LostFriendBehavior>();
			chanceKinBehavioar.aiActor.MovementSpeed = 5f;

			chanceKinBehavioar.CanBePet = true;

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			//prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle", "BotsMod/sprites/LostFriend/idle", 8, AnimationType.Idle, DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			prefab.AddAnimation("pet", "BotsMod/sprites/LostFriend/pet", 5, AnimationType.Other, DirectionType.Single, DirectionalAnimation.FlipType.None).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;





			Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
			//moonraker bloom material
			BasicBeamController beamComp = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_001", new Vector2(32, 4),new Vector2(0, 0),BeamAnimPaths, 8,ImpactAnimPaths, 13,new Vector2(0, 0),new Vector2(0, 0),
				muzzleVFXAnimationPaths: StartAnimPaths, muzzleVFXColliderDimensions: new Vector2(32, 4), muzzleVFXColliderOffsets: new Vector2(0, 0), glows: true);
			//projectile4.gameObject.SetActive(false);
			projectile4.collidesWithPlayer = false;
			FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile4);
			projectile4.baseData.damage = 20f;
			projectile4.baseData.range = 100;
			projectile4.baseData.speed = 200;
			projectile4.PenetratesInternalWalls = false;

			beamComp.ContinueBeamArtToWall = false;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.endType = BasicBeamController.BeamEndType.Vanish;




			
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;

			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissiveColorPower = 7;
			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissivePower = 42;

			var beamCon = prefab.AddComponent<AIBeamShooter>();
			

			var shootpoint = new GameObject("attach");
			shootpoint.transform.parent = prefab.transform;
			shootpoint.transform.position = new Vector2(0.34375f, 0.34375f);

			beamCon.beamProjectile = projectile4;
			beamCon.beamTransform = shootpoint.transform;

			if (EnemyDatabase.GetOrLoadByGuid("3f40178e10dc4094a1565cd4fdc4af56").GetComponent<AIBeamShooter>().chargeVfx == null)
            {
				BotsModule.Log("charge effect is null >:(");
            }

			if (EnemyDatabase.GetOrLoadByGuid("3f40178e10dc4094a1565cd4fdc4af56").GetComponent<AIBeamShooter>().beamModule == null)
			{
				BotsModule.Log("beamModule is null now stop being a lazy bitch and write it by hand :)");
			}

			beamCon.chargeVfx = EnemyDatabase.GetOrLoadByGuid("3f40178e10dc4094a1565cd4fdc4af56").GetComponent<AIBeamShooter>().chargeVfx;
			beamCon.beamModule = EnemyDatabase.GetOrLoadByGuid("3f40178e10dc4094a1565cd4fdc4af56").GetComponent<AIBeamShooter>().beamModule;
			beamCon.TurnDuringDissipation = true;
			beamCon.PreventBeamContinuation = true;
			beamCon.heightOffset = 1.9f;
			beamCon.northAngleTolerance = 90f;

			beamCon.northRampHeight = 0f;
			beamCon.otherRampHeight = 0f;

			beamCon.firingEllipseA = 0.15f;
			beamCon.firingEllipseB = 0.15f;
			beamCon.eyeballFudgeAngle = 45f;
			beamCon.firingEllipseCenter = new Vector2(0, 0);

			GameObject m_CachedGunAttachPoint = prefab.transform.Find("attach").gameObject;


			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that

			var bs = prefab.GetComponent<BehaviorSpeculator>();
			//bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });

			bs.MovementBehaviors = new List<MovementBehaviorBase>() {

				new CompanionFollowPlayerBehavior() 
				{
					PathInterval = 0.25f,
					DisableInCombat = true,
					IdealRadius = 3,
					CatchUpRadius = 8,
					CatchUpAccelTime = 5,
					CatchUpSpeed = 5,
					CatchUpMaxSpeed = 10,
					CatchUpAnimation = "",
					CatchUpOutAnimation = "",
					IdleAnimations = new string[] { "idle" },
					CanRollOverPits = false,
					RollAnimation = "roll",
					
				},

				new SeekTargetBehavior() 
				{
					StopWhenInRange = true,
					CustomRange = 10,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.25f,
					SpecifyRange = false,
					MinActiveRange = 0,
					MaxActiveRange = 0						
				},

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
			bs.AttackBehaviors = new List<AttackBehaviorBase>()
			{
				new ShootBeamBehavior
				{
					beamSelection = ShootBeamBehavior.BeamSelection.All,
					specificBeamShooter = null,
					firingTime = 3,
					stopWhileFiring = true,
					initialAimType = ShootBeamBehavior.InitialAimType.FacingDirection,
					initialAimOffset = 5,
					randomInitialAimOffsetSign = true,
					restrictBeamLengthToAim = false,
					beamLengthOFfset = 0,
					beamLengthSinMagnitude = 0,
					beamLengthSinPeriod = 0,
					trackingType = ShootBeamBehavior.TrackingType.Follow,
					maxUnitTurnRate = 10,
					unitTurnRateAcceleration = 24,
					minUnitRadius = 7,
					useDegreeCatchUp = true,
					minDegreesForCatchUp = 15,
					degreeCatchUpSpeed = 180,
					useUnitCatchUp = true,
					minUnitForCatchUp = 2,
					maxUnitForCatchUp = 10,
					unitCatchUpSpeed = 8,
					useUnitOvershoot = true,
					minUnitForOvershoot = 1,
					unitOvershootTime = 0.200000002980232f, //stole this from dodge roll i have no fucking clue why 0.2 wasnt good enough
					unitOvershootSpeed = 3,
					maxDegTurnRate = 0,
					degTurnRateAcceleration = 0,
					TellAnimation = "",
					FireAnimation = "",
					PostFireAnimation = "",
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
				/*new ShootBehavior() {


					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new BulletScriptSelector{ scriptTypeName = nameof(ShelletonBasicAttack1) },
					BulletName = "",
					LeadAmount = 0,
					StopDuring = ShootBehavior.StopType.None,
					ImmobileDuringStop = false,
					MoveSpeedModifier = 1,
					LockFacingDirection = false,
					ContinueAimingDuringTell = false,
					ReaimOnFire = false,
					MultipleFireEvents = false,
					RequiresTarget = true,
					PreventTargetSwitching = false,
					Uninterruptible = false,
					ClearGoop = false,
					ClearGoopRadius = 2,
					ShouldOverrideFireDirection = false,
					OverrideFireDirection = 0,
					SpecifyAiAnimator = null,

					ChargeAnimation = "",
					ChargeTime = 0,

					TellAnimation = "",
					FireAnimation = "",
					PostFireAnimation = "",

					HideGun = true,
					OverrideBaseAnims = false,
					OverrideIdleAnim = "",
					OverrideMoveAnim = "",
					UseVfx = false,
					ChargeVfx = null,
					TellVfx = null,
					FireVfx = null,
					Vfx = null,
					EnabledDuringAttack = new GameObject[0],
					Cooldown = 2,
					CooldownVariance = 0,
					AttackCooldown = 1,
					GlobalCooldown = 0,
					InitialCooldown = 0,
					InitialCooldownVariance = 0,
					GroupName = "",
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
					RequiresLineOfSight = true,
					MaxUsages = 0,

				}*/
			};
			AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("3f40178e10dc4094a1565cd4fdc4af56");
			EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, null, aIActor.GetComponent<AIBulletBank>(), 0, m_CachedGunAttachPoint.transform);

			chanceKinBehavioar.CanInterceptBullets = false;
			//chanceKinBehavioar.CanInterceptBullets = true;
			
			chanceKinBehavioar.aiActor.CanTargetPlayers = false;
			chanceKinBehavioar.aiActor.healthHaver.PreventAllDamage = true;
			chanceKinBehavioar.aiActor.specRigidbody.CollideWithOthers = true;
			chanceKinBehavioar.aiActor.specRigidbody.CollideWithTileMap = false;
			chanceKinBehavioar.aiActor.healthHaver.ForceSetCurrentHealth(1f);
			chanceKinBehavioar.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
			chanceKinBehavioar.aiActor.specRigidbody.PixelColliders.Clear();
			chanceKinBehavioar.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				//CollisionLayer = CollisionLayer.PlayerCollider,
				CollisionLayer = CollisionLayer.EnemyBulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 12,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});

		}
		protected override void Update()
		{
			base.Update();
		}
		public override void Pickup(PlayerController player)
		{
			//this.CreateNewCompanion(player);
			base.Pickup(player);
		}



		public class LostFriendBehavior : CompanionController
		{
			public void Start()
			{
				Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
				mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
				mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
				mat.SetFloat("_EmissiveColorPower", 1.55f);
				mat.SetFloat("_EmissivePower", 50);
				aiActor.sprite.renderer.material = mat;
				this.Owner = this.m_owner;
				 
				if (this.GetComponent<AIBeamShooter>() != null && this.GetComponent<AIBeamShooter>().beamProjectile == null)
                {
					Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
					//moonraker bloom material
					BasicBeamController beamComp = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_001", new Vector2(32, 4), new Vector2(0, 0), BeamAnimPaths, 8, ImpactAnimPaths, 13, new Vector2(0, 0), new Vector2(0, 0),
						muzzleVFXAnimationPaths: StartAnimPaths, muzzleVFXColliderDimensions: new Vector2(32, 4), muzzleVFXColliderOffsets: new Vector2(0, 0), glows: true);
					//projectile4.gameObject.SetActive(false);
					FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
					UnityEngine.Object.DontDestroyOnLoad(projectile4);
					projectile4.baseData.damage = 20f;
					projectile4.baseData.range = 100;
					projectile4.baseData.speed = 200;
					projectile4.PenetratesInternalWalls = true;

					beamComp.ContinueBeamArtToWall = false;
					beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
					beamComp.endType = BasicBeamController.BeamEndType.Vanish;





					beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;

					beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissiveColorPower = 7;
					beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissivePower = 42;

					this.GetComponent<AIBeamShooter>().beamProjectile = projectile4;
				}
			}
			public PlayerController Owner;
		}

	}
}







