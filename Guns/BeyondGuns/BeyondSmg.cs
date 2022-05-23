using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class BeyondSmg : OverheatGunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Smg", "beyond_smg");
			Game.Items.Rename("outdated_gun_mods:beyond_smg", "bot:beyond_smg");
			var overheat = gun.gameObject.AddComponent<BeyondSmg>();
			gun.SetShortDescription("Hot Hot Hot");
			gun.SetLongDescription("This gun uses a powerful energy core to generate bullets completly removing the need to reload but be careful firing for to long will cause the gun to overheat and burn the user \\n\\nA weapon given to only the most skilled fighters in the Master's army.");
			GunExt.SetupSprite(gun, null, "beyond_smg_idle_001", 8);
			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_smg_fire");
			float[] offsetsX = new float[] { -0.0625f, 0.0000f };
			float[] offsetsY = new float[] { 0.0000f, 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) 
			{ 
				int id = animationclip.frames[i].spriteId;
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i];
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; 
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i];
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; 
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i];
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; 
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i];
				animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; 
			}



			//0.0625
			gun.barrelOffset.localPosition = new Vector3(0.775f, 0.0625f, 0f);
			//gun.muzzleOffset.localPosition = new Vector3(0.775f, 0.125f, 0f);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;

			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(685) as Gun).gunSwitchGroup;

			//gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(357) as Gun).muzzleFlashEffects;
			gun.muzzleFlashEffects = null;
			gun.DefaultModule.cooldownTime = 0.07f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = -1;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = PickupObject.ItemQuality.C;

			overheat.burnOnOverHeat = true;

			overheat.maxWaitTime = 1.75f;

			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			projectile.hitEffects = (PickupObjectDatabase.GetById(57) as Gun).DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 2.5f;
			projectile.baseData.speed = 23f;
			projectile.baseData.force = 6f;
			projectile.baseData.range = 16f;
			projectile.shouldRotate = true;
			projectile.SetProjectileSpriteRight("beyond_smg_projectile_001", 8, 5, false, tk2dBaseSprite.Anchor.LowerLeft);
			gun.DefaultModule.angleVariance = 7;

			BotsItemIds.BeyondSmg = gun.PickupObjectId;

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

        public override void PostProcessProjectile(Projectile projectile)
        {
			
			projectile.baseData.damage *= (1 + (((this.heatLevel / this.maxHeat))/2));

			if (BotsModule.debugMode) ETGModConsole.Log($"{projectile.baseData.damage}");

			base.PostProcessProjectile(projectile);
        }

    }
}
