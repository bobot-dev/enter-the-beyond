using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Alternator : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Alternator", "alternator");
			Game.Items.Rename("outdated_gun_mods:alternator", "bot:alternator");
			gun.gameObject.AddComponent<Alternator>();
			gun.SetShortDescription("Twin Barrel SMG");
			gun.SetLongDescription("");
			GunExt.SetupSprite(gun, null, "alternator_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 25);
			gun.SetAnimationFPS(gun.reloadAnimation, 15);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0.8f;

			//gun.barrelOffset.transform.localPosition = new Vector3(0.625f, 1.125f, 0f);

			//SpriteBuilder.SpriteFromResource("BotsMod/sprites/Debug/c", gun.barrelOffset.transform.gameObject);

			//gun.carryPixelOffset = new IntVector2(8, 0);

			gun.DefaultModule.cooldownTime = 0.07f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 20;
			gun.DefaultModule.angleVariance = 6;

			gun.SetBaseMaxAmmo(500);
			//gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			gun.gunSwitchGroup = "ak47";
			gun.gunClass = GunClass.FULLAUTO;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 23f;
			projectile.baseData.range = 20f;
			projectile.baseData.force = 8f;
			//projectile.angularVelocityVariance = 6f;

			projectile.PenetratesInternalWalls = true;

			//var orb = UnityEngine.Object.Instantiate<GameObject>(Tools.AHHH.LoadAsset<GameObject>("boucey"), projectile.transform);
		}   
    }
}
