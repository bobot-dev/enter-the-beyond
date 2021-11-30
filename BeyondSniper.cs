using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
	class BeyondSniper : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("beyond sniper", "beyond_sniper");
			Game.Items.Rename("outdated_gun_mods:beyond_sniper", "bot:beyond_sniper");
			gun.gameObject.AddComponent<BeyondSniper>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "beyond_sniper_idle_001", 8);
			gun.SetAnimationFPS(gun.reloadAnimation, 8);
			gun.SetAnimationFPS(gun.introAnimation, 10);
			gun.SetAnimationFPS(gun.idleAnimation, 10);			


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(5) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1f;

			gun.DefaultModule.cooldownTime = 1.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 10;

			gun.SetBaseMaxAmmo(50);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.B;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 30;
			//projectile.baseData.speed

			BotsItemIds.BeyondSniper = gun.PickupObjectId;

		}
	}
}
