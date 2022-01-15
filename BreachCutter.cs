using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;


namespace BotsMod
{
    class BreachCutter : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Breach Cutter", "lost_revolver");
			Game.Items.Rename("outdated_gun_mods:breach_cutter", "bot:breach_cutter");
			gun.gameObject.AddComponent<BreachCutter>();
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



			gun.StarterGunForAchievement = true;

			gun.gunSwitchGroup = "EnergyCannon";



			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;


			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;






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

			var laser = projectile.gameObject.AddComponent<St4keProj>();

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

			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}
	}
}
