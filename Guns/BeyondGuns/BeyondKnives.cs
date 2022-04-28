using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class BeyondKnives : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Gun Template", "beyond_knives");
			Game.Items.Rename("outdated_gun_mods:gun_template", "bot:beyond_knives");
			gun.gameObject.AddComponent<BeyondKnives>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "beyond_knives_idle_001", 8);




			gun.UpdateAnimation("fire2", null, true);
			gun.UpdateAnimation("idle2", null, true);
			var animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_knives_fire2");

			gun.carryPixelOffset = new IntVector2(15, -4);
			gun.barrelOffset.localPosition = new Vector3(0.75f, 0.625f, 0f);

			if (animationclip == null)
			{
				BotsModule.Log($"!WARNING!: \"{gun.EncounterNameOrDisplayName}\" could not be setup this is likely due to your version of ggb being out of date or not there at all please install gungeon go brrr version 1.1.2 or higher", BotsModule.LOCKED_CHARACTOR_COLOR, false);
				return;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_knives_fire2");
			float[] offsetsX = new float[] { 0.0000f, 0.0000f, -0.7500f, -0.7500f, -0.7500f, -0.6875f };
			float[] offsetsY = new float[] { 0.0000f, -0.8750f, -0.9375f, 0.0000f, 0.0000f, 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }


			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_knives_fire");
			offsetsX = new float[] { 0.0000f, -0.6250f, 0.3125f, 1.0625f, 1.0625f, 0.0000f };
			offsetsY = new float[] { 0.0000f, -0.8750f, -0.9375f, 0.0000f, 0.0000f, 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }


			gun.SetAnimationFPS(gun.shootAnimation, 18);
			gun.SetAnimationFPS("beyond_knives_fire2", 18);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;

			gun.DefaultModule.cooldownTime = 0.3f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = -1;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);


			projectile.baseData.damage = 10;

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			

			ProjectileSlashingBehaviour slashing = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
			//slashing.DestroyBaseAfterFirstSlash = true;
			slashing.SlashDamageUsesBaseProjectileDamage = true;
			slashing.UsesAngleVariance = false;
			slashing.DoSound = true;
			slashing.SlashRange = 1.8f;
			slashing.SlashDimensions = 90f;
			slashing.delayBeforeSlash = 0f;
			slashing.InteractMode = SlashDoer.ProjInteractMode.DESTROY;
			slashing.playerKnockback = 0;
			slashing.playerKnockbackImmutable = true;

			gun.currentGunStatModifiers = new StatModifier[]
			{
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.MovementSpeed,
					amount = 1.25f,
					modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
					isMeatBunBuff = false
				},
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

		public int attackStage = 1;


		public override void OnPostFired(PlayerController player, Gun gun)
        {
			ChangeAnims();
			gun.PreventNormalFireAudio = true;
		}

		protected void Update()
		{
			if (this.gun.CurrentOwner)
			{
				if (!this.gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!this.gun.IsReloading && !this.HasReloaded)
				{
					this.HasReloaded = true;
				}
			}

		}

		void ChangeAnims()
        {
			attackStage += 1;

			if (attackStage > 2)
			{
				attackStage = 1;
			}

			switch (attackStage)
			{
				case 1:
					this.gun.shootAnimation = $"beyond_knives_fire";
					this.gun.idleAnimation = $"beyond_knives_idle";
					break;
				case 2:
					this.gun.shootAnimation = $"beyond_knives_fire2";
					this.gun.idleAnimation = $"beyond_knives_idle2";
					break;
			}
		}


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{


			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				//AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
			}
		}

		


		private bool HasReloaded;

	}
}
