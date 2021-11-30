using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod.Templates
{
    class GunTemplate : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Gun Template", "gun_template");
			Game.Items.Rename("outdated_gun_mods:gun_template", "bot:gun_template");
			gun.gameObject.AddComponent<GunTemplate>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "gun_template_idle_001", 8);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 100;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

		}
    }
}
