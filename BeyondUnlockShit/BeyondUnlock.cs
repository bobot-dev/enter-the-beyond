using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class BeyondUnlock : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Broken Relic", "beyond_unlock_part_1");
			Game.Items.Rename("outdated_gun_mods:broken_relic", "bot:broken_relic");
			gun.gameObject.AddComponent<BeyondUnlock>();
			gun.SetShortDescription("???");
			gun.SetLongDescription("Broken");

			gun.SetupSprite(null, "beyond_unlock_part_1_idle_001", 8);

			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(0);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = false;

			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;

			
			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.NONE;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.Relic1 = gun.PickupObjectId;
			gun.SetTag("beyond");
		}

		public static void Add2()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Broken Relic", "beyond_unlock_part_2");
			Game.Items.Rename("outdated_gun_mods:broken_relic", "bot:broken_relic_2");
			gun.gameObject.AddComponent<BeyondUnlock>();
			gun.SetShortDescription("???");
			gun.SetLongDescription("Broken");

			gun.SetupSprite(null, "beyond_unlock_part_2_idle_001", 8);

			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(0);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = false;

			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;


			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.NONE;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.Relic2 = gun.PickupObjectId;
			gun.SetTag("beyond");
		}

		public static void Add3()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Relic", "beyond_unlock_whole");
			Game.Items.Rename("outdated_gun_mods:beyond_relic", "bot:beyond_relic");
			gun.gameObject.AddComponent<BeyondUnlock>();
			gun.SetShortDescription("???");
			gun.SetLongDescription("Whole once more...");

			gun.SetupSprite(null, "beyond_unlock_whole_idle_001", 8);

			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(0);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = false;

			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;


			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 0;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.NONE;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.Relic3 = gun.PickupObjectId;
			gun.SetTag("beyond");
		}
	}
}
