using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BreakAbleAPI;

namespace BotsMod
{
    class CoolAssChargeGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Salvation", "cool_beyond_gun");
			Game.Items.Rename("outdated_gun_mods:salvation", "bot:salvation");
			gun.gameObject.AddComponent<CoolAssChargeGun>();
			gun.SetShortDescription("Bringer Of Change");
			gun.SetLongDescription("Generates an insanely powerful blast of energy.\n\nA weapon once used by highly trained soldiers during the Overseer's take over but later outlaw due to the Overseer's fear of revolt.");
			GunExt.SetupSprite(gun, null, "cool_beyond_gun_idle_001", 8);
			gun.SetAnimationFPS(gun.chargeAnimation, 25);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			gun.SetAnimationFPS(gun.reloadAnimation, 16);

			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("cool_beyond_gun_reload");
			//float[] offsetsX = new float[] { -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f };
			float[] offsetsX = new float[] { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f, 0.1250f };
			float[] offsetsY = new float[] { -0.4375f, -0.3750f, -0.3750f, -0.2500f, -0.1250f, -0.1250f, - 0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.5000f, -0.8125f, -0.1250f, -0.1250f, -0.8125f, -0.6875f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f, -0.1250f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }

			animationclip = gun.sprite.spriteAnimator.GetClipByName("cool_beyond_gun_fire");
			offsetsX = new float[] { 0.0625f, 0.0000f, 0.0000f, 0.0000f };
			offsetsY = new float[] { -0.1250f, -0.3750f, -0.3750f, -0.3750f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }

			gun.muzzleFlashEffects = null;

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(370) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
			gun.reloadTime = 1.5f;

			gun.barrelOffset.transform.localPosition -= new Vector3(0f, 0.1875f, 0f);

			//SpriteBuilder.SpriteFromResource("BotsMod/sprites/Debug/c", gun.barrelOffset.transform.gameObject);

			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 30;

			gun.carryPixelOffset = new IntVector2(3, -1);

			gun.DefaultModule.cooldownTime = 0.11f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 1;

			gun.SetBaseMaxAmmo(50);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.S;

			gun.gunSwitchGroup = "Railgun";
			gun.gunClass = GunClass.CHARGE;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Beyond Energy Cell", "BotsMod/sprites/CustomGunAmmoTypes/cool_beyond_charge_gun_custom_clip", "BotsMod/sprites/CustomGunAmmoTypes/cool_beyond_charge_gun_custom_clip_empty");

			var energyCell = BreakableAPIToolbox.GenerateDebrisObject("BotsMod/sprites/beyond_energy_cell_001.png", true, 1.5f, 3f, 90, 0, null, 0.4f, "", null, 0, false, null, 0);
			energyCell.breaksOnFall = true;
			gun.clipObject = energyCell.gameObject;
			gun.reloadClipLaunchFrame = 6;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			var railgun = PickupObjectDatabase.GetById(370) as Gun;

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(railgun.DefaultModule.chargeProjectiles[1].Projectile);
			projectile.gameObject.SetActive(false);




			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			var vfx = FakePrefab.Clone(railgun.DefaultModule.chargeProjectiles[0].VfxPool.effects[0].effects[0].effect);

			var railgunController = vfx.GetComponent<RailgunChargeEffectController>();

			railgunController.ColorGradient = new Gradient
			{
				mode = GradientMode.Blend,
				alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(0.25f, 0.5f), new GradientAlphaKey(1, 1) },
				colorKeys = new GradientColorKey[] { new GradientColorKey(new Color32(207, 21, 197, 255), 0), new GradientColorKey(new Color32(107, 7, 102, 255), 1) },
			};
			//var particles = FakePrefab.Clone(railgunController.ImpactParticles.gameObject).GetComponent<ParticleSystem>();			
			railgunController.Width = 2f;
			railgunController.NewLineFrequency /= 2f;
			railgunController.lineMode = RailgunChargeEffectController.LineChargeMode.PYRAMIDAL_CONVERGE;
			
			vfx.SetActive(false);

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
				},
			};
			projectile.baseData.damage = 100f;
			projectile.baseData.speed = 250f;
			projectile.baseData.range = 1000f;
			projectile.baseData.force = 9f;
			projectile.PlayerKnockbackForce = 26f;
			projectile.AppliesKnockbackToPlayer = true;

			projectile.gameObject.GetComponentInChildren<TrailController>().DispersalParticleSystemPrefab = BeyondPrefabs.AHHH.LoadAsset<GameObject>("VFX_Beyond_Dispersal");

			BotsItemIds.BeyondChargeGun = gun.PickupObjectId;

			gun.SetTag("beyond");
			MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 55);
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			//var orb = UnityEngine.Object.Instantiate<GameObject>(Tools.AHHH.LoadAsset<GameObject>("boucey"), projectile.transform);
		}
	}
}
