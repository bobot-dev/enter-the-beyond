using CustomCharacters;
using Gungeon;
using ItemAPI;
using PrefabAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class BeyondRifle : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Rifle", "beyond_rifle");
			Game.Items.Rename("outdated_gun_mods:beyond_rifle", "bot:beyond_rifle");
			gun.gameObject.AddComponent<BeyondRifle>();
			gun.SetShortDescription("");
			gun.SetLongDescription("");
			GunExt.SetupSprite(gun, null, "beyond_rifle_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 15);
			gun.SetAnimationFPS(gun.reloadAnimation, 12);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.carryPixelOffset = new IntVector2(7,-3);

			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_rifle_idle");
			float[] offsetsX = new float[] { 0.0000f };
			float[] offsetsY = new float[] { 0.0000f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_rifle_fire");
			offsetsX = new float[] { 0.0000f, -0.0625f, 0.0000f, -0.0625f, 0.0000f, -0.0625f };
			offsetsY = new float[] { 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f };


			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_rifle_reload");
			offsetsX = new float[] { 0.0000f, 0.0000f, 0.0000f, -0.0625f, 0.0000f, 0.0000f, 0.0000f, -0.5625f, -0.5000f, 0.0000f, -0.0625f, -0.0625f, -0.0625f, -0.0625f, 0.0000f, 0.0000f };
			offsetsY = new float[] { 0.0000f, 0.0000f, 0.0000f, -0.6875f, -0.6250f, -0.6250f, -0.6250f, -0.6250f, -0.7500f, -0.7500f, -0.6250f, -0.6250f, -0.5625f, 0.0000f, 0.0000f, 0.0000f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			gun.barrelOffset.localPosition = new Vector3(1.8125f, 0.375f, 0f);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.3f;

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 70;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.B;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			projectile2.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile2);

			gun.DefaultModule.projectiles[0] = projectile;


			//projectile.SetProjectileSpriteRight("beyond_rifle_bullet_001", 12, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 2, false, false, 0, 0);

			projectile.hitEffects = (PickupObjectDatabase.GetById(57) as Gun).DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 5f;
			projectile.baseData.speed = 23f;
			projectile.baseData.force = 9f;
			projectile.baseData.range = 1000;
			projectile.shouldRotate = true;
			projectile.SetProjectileSpriteRight("beyond_smg_projectile_001", 8, 5, false, tk2dBaseSprite.Anchor.MiddleCenter);


			projectile2.hitEffects = (PickupObjectDatabase.GetById(57) as Gun).DefaultModule.projectiles[0].hitEffects;
			projectile2.baseData.damage = 2f;
			projectile2.baseData.speed = 13f;
			projectile2.baseData.force = 3f;
			projectile2.baseData.range = 1000;
			projectile2.shouldRotate = true;
			projectile2.SetProjectileSpriteRight("beyond_debuff_rifle_bullet_001", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter);

			var trail = Instantiate(BeyondPrefabs.AHHH.LoadAsset<GameObject>("RifleDebuffTrail"), projectile2.gameObject.transform);
			trail.transform.position = Vector3.zero;
			trail.transform.localPosition = Vector3.zero;

			var homing = projectile2.gameObject.AddComponent<HomingModifier>();
			
			homing.AngularVelocity = 270;
			homing.HomingRadius = 8;

			gun.DefaultModule.projectiles.Add(projectile);
			gun.DefaultModule.projectiles.Add(projectile);
			gun.DefaultModule.projectiles.Add(projectile);
			gun.DefaultModule.projectiles.Add(projectile);
			gun.DefaultModule.projectiles.Add(projectile2);

			gun.DefaultModule.angleVariance = 4;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = "recharge";



			var spikeVfx = PrefabBuilder.BuildObject("VFX_BeyondRifleDebuff_001");


			//to any modders looking at this code do not use it please its fucking awful


			var baseSpriteID = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName("beyond_debuff_rifle_bullet_001");

			var spikeSprite = spikeVfx.AddComponent<tk2dSprite>();
			spikeSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection.inst, baseSpriteID);
			spikeSprite.SortingOrder = 0;

			spikeSprite.IsPerpendicular = true;

			spikeVfx.GetComponent<BraveBehaviour>().sprite = spikeSprite;

			FieldInfo _animationStyle = typeof(BuffVFXAnimator).GetField("animationStyle", BindingFlags.NonPublic | BindingFlags.Instance);

			var vfxAnimator = spikeVfx.AddComponent<BuffVFXAnimator>();
			vfxAnimator.motionPeriod = 1;
			vfxAnimator.ChanceOfApplication = 1;
			vfxAnimator.persistsOnDeath = true;
			vfxAnimator.AdditionalPierceDepth = 0;
			vfxAnimator.UsesVFXToSpawnOnDeath = false;
			vfxAnimator.VFXToSpawnOnDeath = new VFXPool { effects = new VFXComplex[0], type = VFXPoolType.None };
			vfxAnimator.NonPoolVFX = null;
			vfxAnimator.DoesSparks = false;
			_animationStyle.SetValue(vfxAnimator, 0);

			var trail2 = Instantiate(BeyondPrefabs.AHHH.LoadAsset<GameObject>("RifleDebuffTrail"), spikeVfx.gameObject.transform);
			trail2.transform.position = Vector3.zero;
			trail2.transform.localPosition = Vector3.zero;

			var debuff = projectile2.gameObject.AddComponent<OrbitalProjDebuff>();
			debuff.vfx = spikeVfx;


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

		}
	}
}
