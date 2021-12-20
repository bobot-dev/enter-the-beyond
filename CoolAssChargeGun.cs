using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CoolAssChargeGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("idk", "cool_beyond_gun");
			Game.Items.Rename("outdated_gun_mods:idk", "bot:idk");
			gun.gameObject.AddComponent<CoolAssChargeGun>();
			gun.SetShortDescription("");
			gun.SetLongDescription("");
			GunExt.SetupSprite(gun, null, "cool_beyond_gun_idle_001", 8);
			gun.SetAnimationFPS(gun.chargeAnimation, 25);
			gun.SetAnimationFPS(gun.shootAnimation, 15);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(370) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1.7f;

			gun.barrelOffset.transform.localPosition -= new Vector3(0f, 0.1875f, 0f);

			//SpriteBuilder.SpriteFromResource("BotsMod/sprites/Debug/c", gun.barrelOffset.transform.gameObject);

			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 30;

			gun.carryPixelOffset = new IntVector2(0, -1);

			gun.DefaultModule.cooldownTime = 0.11f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 1;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.S;

			gun.gunSwitchGroup = "Railgun";
			gun.gunClass = GunClass.CHARGE;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			var railgun = PickupObjectDatabase.GetById(370) as Gun;

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(railgun.DefaultModule.chargeProjectiles[1].Projectile);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			ETGModConsole.Log($"CoolAssChargeGun 1");
			
			var vfx = FakePrefab.Clone(railgun.DefaultModule.chargeProjectiles[0].VfxPool.effects[0].effects[0].effect);

			var railgunController = vfx.GetComponent<RailgunChargeEffectController>();

			railgunController.Width = 2f;
			railgunController.NewLineFrequency /= 3f;
			railgunController.lineMode = RailgunChargeEffectController.LineChargeMode.PYRAMIDAL_CONVERGE;
			ETGModConsole.Log($"CoolAssChargeGun 12");
			vfx.SetActive(false);
			//var vfx2 = FakePrefab.Clone(railgun.DefaultModule.chargeProjectiles[0].VfxPool.effects[0].effects[1].effect);

			//var railgunController2 = vfx2.GetComponent<RailgunChargeEffectController>();
			//if (railgunController2 == null)
            //{
			//	ETGModConsole.Log($"CoolAssChargeGun railgunController2 is null :|");
			//}

			ETGModConsole.Log($"CoolAssChargeGun 13");
			//railgunController2.Width = 0.5f;
			//railgunController2.NewLineFrequency = 0.2515f;

			//vfx2.SetActive(false);

			//vfx.AddComponent<BotsMod.Debugger>();
			//vfx2.AddComponent<BotsMod.Debugger>();

			ETGModConsole.Log($"CoolAssChargeGun 2");

			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					UsedProperties = ProjectileModule.ChargeProjectileProperties.delayedVFXClear | ProjectileModule.ChargeProjectileProperties.vfx,
					ChargeTime = 0,
					LightIntensity = 0,
					AdditionalWwiseEvent = "",
					AmmoCost = 0,
					OverrideShootAnimation = "",
					MegaReflection = false,
					OverrideMuzzleFlashVfxPool = null,
					Projectile = null,
					ScreenShake = new ScreenShakeSettings
					{
						direction = Vector2.zero,
						falloff = 0,
						magnitude = 0,
						simpleVibrationStrength = Vibration.Strength.Medium,
						simpleVibrationTime = Vibration.Time.Normal,
						speed = 0,
						time = 0,
						vibrationType = ScreenShakeSettings.VibrationType.Auto,
					},
					VfxPool = new VFXPool
					{
						type = VFXPoolType.Single,
						effects = new VFXComplex[]
						{
							new VFXComplex
							{
								effects = new VFXObject[]
								{
									new VFXObject
									{
										effect = vfx,
										alignment = VFXAlignment.Fixed,
										attached = true,
										destructible = false,
										orphaned = false,
										persistsOnDeath = false,
										usesZHeight = false,
										 zHeight = 0,
									},
									
								},
							}
						}
					}
				},
				new ProjectileModule.ChargeProjectile
				{
					Projectile = projectile,
					AmmoCost = 1,
					ChargeTime = 1.3f,
					//UsedProperties = ProjectileModule.ChargeProjectileProperties.vfx,
					/*VfxPool = new VFXPool
					{
						type = VFXPoolType.Single,
						effects = new VFXComplex[]
						{
							new VFXComplex
							{
								effects = new VFXObject[]
								{
									new VFXObject
									{
										effect = vfx2,
										alignment = VFXAlignment.Fixed,
										attached = true,
										destructible = false,
										orphaned = false,
										persistsOnDeath = false,
										usesZHeight = false,
										 zHeight = 0,
									},
									new VFXObject
									{
										effect = vfx2,
										alignment = VFXAlignment.Fixed,
										attached = true,
										destructible = false,
										orphaned = false,
										persistsOnDeath = false,
										usesZHeight = false,
										 zHeight = 0,
									},
								},
							}
						}
					}*/
				},
			};
			projectile.baseData.damage = 7f;
			projectile.baseData.speed = 250f;
			projectile.baseData.range = 1000f;
			projectile.baseData.force = 9f;

			//projectile.angularVelocityVariance = 4f;
			//var orb = UnityEngine.Object.Instantiate<GameObject>(Tools.AHHH.LoadAsset<GameObject>("boucey"), projectile.transform);
		}
	}
}
