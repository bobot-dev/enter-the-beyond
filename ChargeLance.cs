using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
	public class ChargeLance : GunBehaviour
	{
		private bool HasReloaded;

		public static void Add()
		{

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Charge Lance", "charge_lance");
			Game.Items.Rename("outdated_gun_mods:charge_lance", "bot:charge_lance");
			gun.gameObject.AddComponent<LostSidearm>();
			gun.SetShortDescription("Head: Destroyed");
			gun.SetLongDescription("Add text here before releasing");

			gun.SetupSprite(null, "charge_lance_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);

			gun.spriteAnimator.Library.GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.spriteAnimator.Library.GetClipByName(gun.chargeAnimation).loopStart = 11;
			//gun.spriteAnimator.Library.GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;

			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(100);
			gun.InfiniteAmmo = false;


			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			

			gun.reloadTime = 0.8f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Charge Lance Charge", "BotsMod/sprites/CustomGunAmmoTypes/charge_lance_custom_clip_001", "BotsMod/sprites/CustomGunAmmoTypes/charge_lance_custom_clip_002");


			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.quality = PickupObject.ItemQuality.B;

			gun.gunClass = GunClass.CHARGE;
			gun.CanBeDropped = true;

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;


			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					AmmoCost = 1,
					Projectile = projectile,
					ChargeTime = 0,
				},
				
			};




			ETGMod.Databases.Items.Add(gun, null, "ANY");


			//Tools.BeyondItems.Add(gun.PickupObjectId);
		}
	}
}
