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

		public static void Add()
		{

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
			
			var gunToCopy = PickupObjectDatabase.GetById(60) as Gun;

			gun.shootAnimation = gunToCopy.shootAnimation;


			Gun other = PickupObjectDatabase.GetById(60) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			gun.DefaultModule.ammoCost = 1;
			
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
			gun.StarterGunForAchievement = true;

			gun.gunSwitchGroup = "EnergyCannon";

			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
			gun.DefaultModule.customAmmoType = "yellow_beam";


			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.3f;
			gun.DefaultModule.numberOfShotsInClip = 1000;

			gun.DefaultModule.burstCooldownTime = 0.2f;
			gun.DefaultModule.burstShotCount = 3;

				

			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.BEAM;
			gun.CanBeDropped = true;
			gun.PreventStartingOwnerFromDropping = true;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			
			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			
			List<string> BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/beam_middle_001",
				"BotsMod/sprites/beam/beam_middle_002",
				"BotsMod/sprites/beam/beam_middle_003",
				"BotsMod/sprites/beam/beam_middle_004",
				"BotsMod/sprites/beam/beam_middle_005",
				"BotsMod/sprites/beam/beam_middle_006",
				"BotsMod/sprites/beam/beam_middle_007",
				"BotsMod/sprites/beam/beam_middle_008",

			};
			List<string> ImpactAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/beam_end_001",
				"BotsMod/sprites/beam/beam_end_002",
				"BotsMod/sprites/beam/beam_end_003",
				"BotsMod/sprites/beam/beam_end_004",
			};

			Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
			BasicBeamController beamComp = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/beam_middle_001", new Vector2(16, 9), new Vector2(0, 0), BeamAnimPaths, 8, ImpactAnimPaths, 13, new Vector2(0, 0), new Vector2(0, 0));
			projectile4.gameObject.SetActive(false);
			projectile4.baseData.damage *= 4;
			projectile4.baseData.range *= 2;
			projectile.baseData.speed *= 40;
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile4);
			beamComp.interpolateStretchedBones = false;
			beamComp.ContinueBeamArtToWall = true;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;

			beamComp.endType = BasicBeamController.BeamEndType.Persist;


			var funnyBeam = beamComp.gameObject.AddComponent<FireSubBeamSynergyProcessor>();

			funnyBeam.BeamAngle = 45;
			funnyBeam.FromProjectileDamageModifier = 3;
			funnyBeam.Mode = FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM;
			funnyBeam.NumberBeams = 10;
			funnyBeam.SubBeamProjectile = (PickupObjectDatabase.GetById(100) as Gun).DefaultModule.projectiles[0];
			funnyBeam.SynergyToCheck = CustomSynergyType.BLESSED_CURSED_BULLETS;


			var funnyBeam2 = beamComp.gameObject.AddComponent<FireSubBeamSynergyProcessor>();

			funnyBeam2.BeamAngle = 90;
			funnyBeam2.FromProjectileDamageModifier = 3;
			funnyBeam2.Mode = FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM;
			funnyBeam2.NumberBeams = 10;
			funnyBeam2.SubBeamProjectile = (PickupObjectDatabase.GetById(60) as Gun).DefaultModule.projectiles[0];
			funnyBeam2.SynergyToCheck = CustomSynergyType.BLESSED_CURSED_BULLETS;

			var funnyBeam3 = beamComp.gameObject.AddComponent<FireSubBeamSynergyProcessor>();

			funnyBeam3.BeamAngle = 315;
			funnyBeam3.FromProjectileDamageModifier = 3;
			funnyBeam3.Mode = FireSubBeamSynergyProcessor.SubBeamMode.FROM_BEAM;
			funnyBeam3.NumberBeams = 10;
			funnyBeam3.SubBeamProjectile = (PickupObjectDatabase.GetById(20) as Gun).DefaultModule.projectiles[0];
			funnyBeam3.SynergyToCheck = CustomSynergyType.BLESSED_CURSED_BULLETS;


			gun.DefaultModule.projectiles[0] = projectile4;

			projectile.baseData.damage = 50;

			var beam = projectile.gameObject.GetComponent<BasicBeamController>();

			beam.boneType = BasicBeamController.BeamBoneType.Projectile;

			beam.endType = BasicBeamController.BeamEndType.Persist;

			beam.beamEndAnimation = "";

			BotsModule.Log($"{projectile.gameObject.name}");
			//GetComponentInChildren
			foreach (var comp in projectile.gameObject.GetComponents<Component>())
			{
				BotsModule.Log($"=========[{comp.name}: {comp.GetType()}]=========");
			}


			ETGMod.Databases.Items.Add(gun, null, "ANY");
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
