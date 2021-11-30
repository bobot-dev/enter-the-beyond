using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Kr82m : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("KR-82M", "kr82m");
			Game.Items.Rename("outdated_gun_mods:kr-82m", "bot:kr82m");
			gun.gameObject.AddComponent<Kr82m>();
			gun.SetShortDescription("");
			gun.SetLongDescription("");
			GunExt.SetupSprite(gun, null, "kr82m_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 25);
			gun.SetAnimationFPS(gun.reloadAnimation, 15);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0.8f;

			gun.barrelOffset.transform.localPosition = new Vector3(2f, 0.6875f, 0f);

			//SpriteBuilder.SpriteFromResource("BotsMod/sprites/Debug/c", gun.barrelOffset.transform.gameObject);

			gun.carryPixelOffset = new IntVector2(6, 0);

			gun.DefaultModule.cooldownTime = 0.11f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 30;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			gun.gunSwitchGroup = "ak47";
			gun.gunClass = GunClass.FULLAUTO;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			projectile.baseData.damage = 7f;
			projectile.baseData.speed = 23f;
			projectile.baseData.range = 1000f;
			projectile.baseData.force = 9f;
			projectile.angularVelocity = 4f;

			projectile.PenetratesInternalWalls = true;

			//var orb = UnityEngine.Object.Instantiate<GameObject>(Tools.AHHH.LoadAsset<GameObject>("boucey"), projectile.transform);
		}
	}
}
