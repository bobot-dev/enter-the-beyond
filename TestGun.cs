using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace BotsMod
{

	public class TestGun : GunBehaviour
	{
		private bool HasReloaded;

		private static Gun lostSidearm;


		//private async Task<Leaderboard?> FetchLeaderboard(int difficulty, bool precise)
		//{
			//Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync("Cyber Grind Wave Violent");
			//return leaderboard;
		//}

		public static void Add()
		{

			//SteamClient.Init(1229490, true);

			

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Bot's Test Gun", "lost_revolver");
			Game.Items.Rename("outdated_gun_mods:bot's_test_gun", "bot:test_gun");
			gun.gameObject.AddComponent<TestGun>();
			gun.SetShortDescription("this one breaks a lot");
			gun.SetLongDescription("this gun is used by bot to test random shit");
			
			gun.SetupSprite(null, "lost_revolver_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			
			
			Gun other = PickupObjectDatabase.GetById(86) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.AddProjectileModuleFrom(other, true, false);			
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			


			gun.StarterGunForAchievement = false;

			gun.gunSwitchGroup = "EnergyCannon";



			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;
			

			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;
			gun.DefaultModule.numberOfShotsInClip = 1000;
			gun.DefaultModule.cooldownTime = 0.05f;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;



			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.CHARGE;
			gun.CanBeDropped = true;
			gun.PreventStartingOwnerFromDropping = true;

			Projectile projectile = Tools.SetupProjectile(86);
			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 0f;
			projectile.baseData.speed = 15;
			projectile.baseData.force = 0f;
			projectile.baseData.range = 8;

			var pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();

			pierceProjModifier.penetration = int.MaxValue;
			pierceProjModifier.penetratesBreakables = true;
			pierceProjModifier.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;

			//var dieBirds = projectile.gameObject.AddComponent<AntiAirProjectile>();

			//dieBirds.AngularVelocity = 180;
			//dieBirds.HomingRadius = 2;

			/*var laser = projectile.gameObject.AddComponent<St4keProj>();

			//laser.BackLinkProjectile = null;
			//laser.damageCooldown = 1;
			//laser.damagePerHit = 9;
			//laser.damageTypes = CoreDamageTypes.None;
			laser.LinkVFXPrefab = FakePrefab.Clone((GameObject)BraveResources.Load("Global VFX/VFX_LaserSight", ".prefab"));//(PickupObjectDatabase.GetById(298) as ComplexProjectileModifier).ChainLightningVFX;
			laser.LinkVFXPrefab.SetActive(false);

			gun.DefaultModule.projectiles[0] = projectile;
			gun.DefaultModule.angleVariance = 0;
			gun.DefaultModule.positionOffset = new Vector3(0f, 1.4f, 0f);

			gun.Volley.projectiles[1].projectiles[0] = projectile;
			gun.Volley.projectiles[1].positionOffset = new Vector3(0f, -1.4f, 0f);
			gun.Volley.projectiles[1].angleVariance = 0;

			/*List<string> BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_001",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_002",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_003",

			};
			List<string> ImpactAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_001",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_002",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_003",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_004",
			};

			Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
			//moonraker bloom material
			BasicBeamController beamComp = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_001",
				new Vector2(32, 5),
				new Vector2(0, 2),
				BeamAnimPaths, 8,
				ImpactAnimPaths, 13,
				new Vector2(0, 0),
				new Vector2(0, 0),
				glows: true
				);
			projectile4.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile4);
			projectile4.baseData.damage = 10f;
			projectile4.baseData.range = 100;
			projectile4.baseData.speed = 200;

			beamComp.ContinueBeamArtToWall = false;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.endType = BasicBeamController.BeamEndType.Vanish;




			beamComp.ProjectileAndBeamMotionModule = new HelixProjectileMotionModule();
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.homingRadius = 10;
			beamComp.homingAngularVelocity = 1000;

			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissiveColorPower = 7;
			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissivePower = 42;*/

			/*foreach(var module in gun.Volley.projectiles)
            {
				module.ammoCost = 1;

				module.shootStyle = ProjectileModule.ShootStyle.Charged;
				module.ammoType = GameUIAmmoType.AmmoType.BEAM;
				module.customAmmoType = "yellow_beam";
				module.cooldownTime = 0.3f;
				module.numberOfShotsInClip = 1000;
				module.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
				{
					new ProjectileModule.ChargeProjectile
					{
						AmmoCost = 1,
						Projectile = projectile,
						ChargeTime = 0.7f,
						
					}
				};
			}

			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					AmmoCost = 1,
					Projectile = projectile,
					ChargeTime = 0,
				},
				new ProjectileModule.ChargeProjectile
				{
					AmmoCost = 1,
					Projectile = projectile,
					ChargeTime = 0.7f,
					


				}
			};*/



			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}

		public float heatLevel;
		public float maxHeat = 3;
		public bool overheated;

		float waitTime;
		float maxWaitTime = 0.8f;

		void Update()
        {
			if(this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController)
            {
				if (this.gun.IsFiring)
                {
					waitTime = 0;
					if (heatLevel >= maxHeat)
					{
						if (!overheated)
						{
							overheated = true;
							this.gun.CeaseAttack(false, null);
						}
					}
					else if (heatLevel < maxHeat)
					{
						heatLevel += Time.deltaTime;
					}
				} 
				else
                {
					if (waitTime >= maxWaitTime)
                    {
						if (heatLevel > 0)
						{
							heatLevel -= Time.deltaTime * 2;
							if (heatLevel <= 0)
							{
								overheated = false;
								heatLevel = 0;
							}
						}
					}
					else if (waitTime < maxWaitTime)
					{
						waitTime += Time.deltaTime;
					}
				}				
            }
        }

		public override void OnInitializedWithOwner(GameActor actor)
		{
			base.OnInitializedWithOwner(actor);

			if (actor is PlayerController)
			{
				(actor as PlayerController).OnKilledEnemyContext += TestGun_OnKilledEnemyContext;
				
			}
		}

		private void TestGun_OnKilledEnemyContext(PlayerController player, HealthHaver enemy)
		{
			if (enemy.aiActor != null)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetById(595).gameObject, enemy.aiActor.sprite.WorldCenter, Vector2.zero, 0);
			}
		}

	}
}
