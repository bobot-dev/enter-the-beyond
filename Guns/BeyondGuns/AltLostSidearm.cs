using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace BotsMod
{

	public class AltLostSidearm : GunBehaviour
	{
		private bool HasReloaded;

		public static void Add()
		{
			
			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Lost Sidearm", "lost_revolver_b");
			Game.Items.Rename("outdated_gun_mods:lost_sidearm", "bot:lost_sidearm_alt");
			gun.gameObject.AddComponent<AltLostSidearm>();
			gun.SetShortDescription("Decay");
			gun.SetLongDescription("Add text here before releasing");
			
			gun.SetupSprite(null, "lost_revolver_b_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 12);


			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("lost_revolver_b_reload");
			float[] offsetsX = new float[] { 0.1250f, 0.0000f, -1.1250f, -1.3750f, -0.1875f, 0.0000f, 0.6875f, 0.1875f, 0.0000f, -0.1250f, 0.0000f, 0.0625f };
			float[] offsetsY = new float[] { -0.3750f, -0.1250f, 0.9375f, -0.0625f, 0.5625f, 0.0000f, 1.5000f, 1.2500f, 0.8750f, 0.5625f, -1.0625f, -0.7500f };



			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}
			//lost_revolver_b_fire_001

			animationclip = gun.sprite.spriteAnimator.GetClipByName(gun.shootAnimation);
			offsetsX = new float[] { 0.0000f, -0.1875f, -0.0625f, 0.0000f };
			offsetsY = new float[] { -0.0625f, 0.1250f, -0.0625f, 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			//gun.barrelOffset.transform.localPosition += new Vector3(10f, 0f, 0f);

			gun.DefaultModule.ammoCost = 1;
			
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = true;
			
			
			//gun.damageModifier = 1;
			gun.reloadTime = 1f;


			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Lost Sidearm Alt", "BotsMod/sprites/CustomGunAmmoTypes/lost_sidearm_clip_alt_001", "BotsMod/sprites/CustomGunAmmoTypes/lost_sidearm_clip_alt_002");

			//gun.DefaultModule.usesOptionalFinalProjectile = true;
			//gun.DefaultModule.numberOfFinalProjectiles = 1;

			//Projectile replacementProjectile = gun2.DefaultModule.projectiles[0];



			//gun.DefaultModule.finalProjectile = replacementProjectile;
			//gun.DefaultModule.finalCustomAmmoType = gun2.DefaultModule.customAmmoType;
			//gun.DefaultModule.finalAmmoType = gun2.DefaultModule.ammoType;

			//gun.DefaultModule.finalProjectile.statusEffectsToApply.Add(Debuffs.decayEffect);
			//gun.DefaultModule.finalProjectile.ChanceToTransmogrify = 0;

			Gun gun5 = PickupObjectDatabase.GetById(128) as Gun;
			
			gun.muzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.17f;
			gun.DefaultModule.numberOfShotsInClip = 7;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.SHITTY;
			gun.CanBeDropped = true;
			gun.PreventStartingOwnerFromDropping = true;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			gun.DefaultModule.angleVariance = 7;

			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;


			projectile.SetProjectileSpriteRight("lost_sidearm_projectile_alt_001", 8, 8, true, tk2dBaseSprite.Anchor.MiddleCenter, 6, 2, false, false, 0, 0);


			//projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 5f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;
			projectile.shouldRotate = true;


			//gun.additionalHandState = AdditionalHandState.HideBoth;
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
			material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 30);
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.AltLostSidearm = gun.PickupObjectId;

			//gun.SetTag("beyond");
		}

		protected void Update()
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				bool flag2 = !this.gun.PreventNormalFireAudio;
				if (flag2)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag3)
				{
					this.HasReloaded = true;
				}
			}
		}

		bool setup;		


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{

			 
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_OBJ_blackhole_close_01", base.gameObject);
			}
		}



		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}
	}
}
