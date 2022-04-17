using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BotsMod
{
    class BeyondHammer : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Hammer", "beyond_hammer");
			Game.Items.Rename("outdated_gun_mods:beyond_hammer", "bot:beyond_hammer");
			gun.gameObject.AddComponent<BeyondHammer>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "beyond_hammer_idle_001", 8);

			gun.SetAnimationFPS(gun.shootAnimation, 16);
			gun.SetAnimationFPS(gun.chargeAnimation, 8);

			gun.carryPixelOffset = new IntVector2(13, 10);

			gun.barrelOffset.localPosition = new Vector3(3.1875f, 0f, 0f);

			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_hammer_charge");
			float[] offsetsX = new float[] { 0.0000f, -0.0625f, -0.1875f, -1.6250f, -1.3125f, -1.3125f, -1.3125f, -1.3125f };
			float[] offsetsY = new float[] { 0.0000f, 0.0625f, 0.1250f, 0.4375f, 0.4375f, 0.4375f, 0.4375f, 0.4375f };

			animationclip.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			animationclip.loopStart = 7;
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_hammer_fire");
			offsetsX = new float[] { -0.6875f, -0.6875f, 1.6250f, 1.5000f, 1.4375f, 0.9375f, -1.7500f, -2.0000f, 0.0000f, 0.0000f };
			offsetsY = new float[] { 0.3750f, -0.5625f, -0.5625f, -1.8750f, -2.2500f, -2.4375f, -2.5000f, -0.8125f, 0.1250f, 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip.AddAudioEventByFrame(1, "Play_WPN_blasphemy_shot_01");
			animationclip.AddAudioEventByFrame(2, "Play_WPN_beyond_hammer_shot_01");
			animationclip.AddAudioEventByFrame(6, "Play_WPN_blasphemy_shot_01");

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(541) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.muzzleFlashEffects = null;

			gun.DefaultModule.cooldownTime = 0.8f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 100;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(541) as Gun).DefaultModule.chargeProjectiles[0].Projectile);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			var boom = projectile.gameObject.AddComponent<ExplosiveModifier>();

			boom.doDistortionWave = true;
			boom.distortionDuration = 0.3f;
			boom.distortionIntensity = 0.1f;
			boom.distortionRadius = 0.8f;
			boom.doExplosion = true;
			boom.IgnoreQueues = true;
			boom.maxDistortionRadius = 1;
			boom.explosionData = new ExplosionData
			{
				breakSecretWalls = false,
				comprehensiveDelay = 0,
				damage = 50,
				damageRadius = 2.5f,
				damageToPlayer = 0f,
				debrisForce = 500,
				doDamage = true,
				doDestroyProjectiles = false,
				forcePreventSecretWallDamage = true,
				doForce = true,
				doScreenShake = false,
				doExplosionRing = true,
				doStickyFriction = false,
				effect = null,
				explosionDelay = 0f,
				force = 300,
				forceUseThisRadius = false,
				freezeEffect = null,
				freezeRadius = 0,
				ignoreList = new List<SpeculativeRigidbody>(),
				IsChandelierExplosion = false,
				isFreezeExplosion = false,
				overrideRangeIndicatorEffect = null,
				playDefaultSFX = true,
				preventPlayerForce = false,
				pushRadius = 3,
				rotateEffectToNormal = false,
				secretWallsRadius = 0,
				ss = null,
				useDefaultExplosion = false,
				usesComprehensiveDelay = false,
			};

			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					ChargeTime = 0.8f,
					AmmoCost = 1,
					Projectile = projectile,
				}
			};

			Tools.BeyondItems.Add(gun.PickupObjectId);
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

		}
    }
}
