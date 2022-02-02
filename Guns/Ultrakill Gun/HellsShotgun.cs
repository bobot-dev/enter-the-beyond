using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace BotsMod
{
	class HellsShotgun : GunBehaviour
	{
		private bool HasReloaded;

		public static GameObject coinPrefab;

		public static void Add()
		{
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(383) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Hell's Shotgun", "hells_shotgun");
			Game.Items.Rename("outdated_gun_mods:hell's_shotgun", "bot:hells_shotgun");
			var meNeedMakeCoin = gun.gameObject.AddComponent<HellsRevolver>();
			gun.SetShortDescription("fuck you flesh prison");
			gun.SetLongDescription("MANKIND IS DEAD \nBLOOD IS FUEL \nHELL IS FULL");

			gun.SetupSprite(null, "hells_shotgun_idle_001", 8);

			Gun other = PickupObjectDatabase.GetById(99) as Gun;



			gun.AddProjectileModuleFrom(other, true, false);

			gun.DefaultModule.cooldownTime = 0.66f;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.numberOfShotsInClip = 4;
			//gun.DefaultModule.angleFromAim = -12 + (6 * i);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BLUE_SHOTGUN;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;
			gun.DefaultModule.angleVariance = 16;
			gun.DefaultModule.cooldownTime = 0.3f;
			gun.DefaultModule.positionOffset = new Vector3(0, 0, 0);
			gun.DefaultModule.numberOfShotsInClip = 5;

			Projectile projectile = Tools.SetupProjectile(other.DefaultModule.projectiles[0]);

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 6f;
			projectile.baseData.force = 15f;
			projectile.baseData.range = 15f;

			projectile.collidesWithPlayer = true;

			//projectile.Ramp(10, 0.3f);
			projectile.gameObject.AddComponent<ProjBoost>();
			projectile.gameObject.AddComponent<Bleed>();
			gun.DefaultModule.projectiles[0] = projectile;


			for (int i = 1; i < 5; i++)
			{
				gun.AddProjectileModuleFrom(other, true, false);

				gun.Volley.projectiles[i].cooldownTime = 0.66f;
				gun.Volley.projectiles[i].angleVariance = 0f;
				gun.Volley.projectiles[i].numberOfShotsInClip = 4;
				//gun.Volley.projectiles[i].angleFromAim = -12 + (6 * i);

				gun.Volley.projectiles[i].ammoCost = 1;

				gun.Volley.projectiles[i].shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				gun.Volley.projectiles[i].ammoType = GameUIAmmoType.AmmoType.BLUE_SHOTGUN;
				gun.Volley.projectiles[i].customAmmoType = other.DefaultModule.customAmmoType;
				gun.Volley.projectiles[i].angleVariance = 16;
				gun.Volley.projectiles[i].cooldownTime = 0.3f;
				gun.Volley.projectiles[i].positionOffset = new Vector3(0, 0, 0);
				gun.Volley.projectiles[i].numberOfShotsInClip = 5;
				

				//projectile.transform.parent = gun.barrelOffset;
				projectile.baseData.damage = 4f;
				projectile.baseData.speed = 6f;
				projectile.baseData.force = 15f;
				projectile.baseData.range = 15f;

				projectile.collidesWithPlayer = true;

				//projectile.Ramp(10, 0.3f);
				projectile.gameObject.AddComponent<ProjBoost>();
				projectile.gameObject.AddComponent<Bleed>();
				gun.Volley.projectiles[i].projectiles[0] = projectile;

				
				bool flag = gun.Volley.projectiles[i] != gun.DefaultModule;
				if (flag)
				{
					gun.Volley.projectiles[i].ammoCost = 0;
				}
				
				
			}



			gun.Volley.UsesShotgunStyleVelocityRandomizer = true;


			gun.SetBaseMaxAmmo(500);
			gun.InfiniteAmmo = false;

			
			gun.StarterGunForAchievement = false;

			//gun.barrelOffset = gun4,

			//gun.damageModifier = 1;
			gun.reloadTime = 0.7f;

			

			

			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.SHOTGUN;
			

			
			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;









			

			MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{


				if (sharedMaterials[i].shader == EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material.shader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", new Color32(176, 225, 225, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 50);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			ETGMod.Databases.Items.Add(gun, null, "ANY");


		}


	}


	
}
