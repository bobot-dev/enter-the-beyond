using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class HellsRevolver2 : GunBehaviour
	{
		private bool HasReloaded;

		public static void Add()
		{
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Hell's Revolver (Marksman)", "hells_revolver2");
			Game.Items.Rename("outdated_gun_mods:hell's_revolver_(marksman)", "bot:hells_revolver+the_marksman");
			var meNeedMakeCoin = gun.gameObject.AddComponent<HellsRevolver>();
			gun.SetShortDescription("fuck you minos");
			gun.SetLongDescription("\"MAXIMUM VOLUME YIELDS MAXIMUM RESULTS.\"");

			gun.SetupSprite(null, "hells_revolver2_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);


			Gun other = PickupObjectDatabase.GetById(99) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(500);
			gun.InfiniteAmmo = false;



			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = false;


			//gun.damageModifier = 1;
			gun.reloadTime = 0.7f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;

			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.IsMinusOneGun = false;

			gun.DefaultModule.cooldownTime = 0.14f;
			gun.DefaultModule.positionOffset = new Vector3(0, 0, 0);
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.PISTOL;
			Projectile projectile = Tools.SetupProjectile(other.DefaultModule.projectiles[0]);

			gun.DefaultModule.projectiles[0] = projectile;


			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 6f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 1600000f;

			projectile.gameObject.AddComponent<Bleed>();

			gun.gameObject.AddComponent<CoinGunModifier>();

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
			material.SetColor("_EmissiveColor", new Color32(0, 207, 3, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 50);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.TheMarksman = gun.PickupObjectId;


		}



	}
    
    
}
