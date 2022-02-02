using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using static BotsMod.UltraKillGun;

namespace BotsMod
{
	class NailGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(383) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Nail Minigun", "nail_vulcan");
			Game.Items.Rename("outdated_gun_mods:nail_minigun", "bot:nail_minigun");
			var meNeedMakeCoin = gun.gameObject.AddComponent<NailGun>();
			gun.SetShortDescription("MANKIND IS DEAD");
			gun.SetLongDescription("A heavily modified version of the Vulcan that now fires nails and magnets");

			gun.SetupSprite(null, "nail_vulcan_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			//gun.gameObject.AddComponent<UltraKillGun>();

			Gun other = PickupObjectDatabase.GetById(26) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(1000);
			gun.InfiniteAmmo = false;

			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(84) as Gun).gunSwitchGroup;
			gun.alternateSwitchGroup = (PickupObjectDatabase.GetById(84) as Gun).gunSwitchGroup;

			ProjectileModule altProjModual = ProjectileModule.CreateClone(other.DefaultModule, false, -1);
			altProjModual.projectiles = new List<Projectile>(other.DefaultModule.projectiles.Capacity);

			//var altProjModual = gun.AddProjectileModuleFrom(other, true, false);


			gun.alternateVolley = new ProjectileVolleyData
			{
				projectiles = new List<ProjectileModule>
				{
					altProjModual,
				},
				UsesShotgunStyleVelocityRandomizer = false,
				ModulesAreTiers = false,
				BeamRotationDegreesPerSecond = 30.0f,
				DecreaseFinalSpeedPercentMin = -5.0f,
				IncreaseFinalSpeedPercentMax = 5.0f,
				UsesBeamRotationLimiter = false,
			};

			gun.reloadTime = 1.3f;

			gun.gunHandedness = GunHandedness.TwoHanded;

			Gun gun5 = PickupObjectDatabase.GetById(84) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;
			gun.IsTrickGun = true;

			gun.alternateVolley.projectiles[0].ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.alternateVolley.projectiles[0].customAmmoType = "Rifle";
			gun.alternateVolley.projectiles[0].ammoCost = 1;
			gun.alternateVolley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.alternateVolley.projectiles[0].cooldownTime = 0.3f;
			gun.alternateVolley.projectiles[0].numberOfShotsInClip = 1;
			gun.alternateVolley.projectiles[0].angleVariance = 0;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.cooldownTime = 0.05f;
			gun.alternateVolley.projectiles[0].angleVariance = 20;


			gun.DefaultModule.numberOfShotsInClip = 100;

			gun.quality = PickupObject.ItemQuality.S;
			gun.gunClass = GunClass.FULLAUTO;

			Projectile projectile = Tools.SetupProjectile(other.DefaultModule.projectiles[0]);
			Projectile projectileMagnet = Tools.SetupProjectile(15);

			gun.DefaultModule.projectiles[0] = projectile;
			altProjModual.projectiles.Add(projectileMagnet);

			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 15f;
			projectile.baseData.range = 1000f;

			
			projectileMagnet.baseData.damage = 0f;
			projectileMagnet.baseData.speed = 16f;
			projectileMagnet.baseData.force = 15f;
			projectileMagnet.baseData.range = 30f;
			projectileMagnet.shouldRotate = true;
			projectileMagnet.pierceMinorBreakables = true;
			projectileMagnet.SetProjectileSpriteRight("nailmagnet_001", 17, 4, false, tk2dBaseSprite.Anchor.LowerLeft);

			projectileMagnet.gameObject.AddComponent<NailMagnet>();

			projectile.gameObject.AddComponent<Bleed>();
			projectile.gameObject.AddComponent((PickupObjectDatabase.GetById(692) as Gun).DefaultModule.projectiles[0].GetComponent<StrafeBleedBuff>());
			//projectile.gameObject.AddComponent<NailBleedBuff>();
			var homing = projectile.gameObject.AddComponent<NailHomingModifier>();

			homing.AngularVelocity = 3600;
			homing.HomingRadius = 15;


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
			material.SetColor("_EmissiveColor", new Color32(244, 124, 46, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 150);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			ETGMod.Databases.Items.Add(gun, null, "ANY");


		}


        public override void PostProcessProjectile(Projectile projectile)
        {
			if(gun.CurrentOwner is PlayerController && (gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy("Overheat"))
            {
				projectile.FireApplyChance = 0.3f;
				projectile.AppliesFire = true;
				projectile.fireEffect = (PickupObjectDatabase.GetById(295) as BulletStatusEffectItem).FireModifierEffect;
				projectile.AdjustPlayerProjectileTint((PickupObjectDatabase.GetById(295) as BulletStatusEffectItem).TintColor, 1);

			}
            base.PostProcessProjectile(projectile);
        }
    }
}
