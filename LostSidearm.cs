using System;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace BotsMod
{

	public class LostSidearm : GunBehaviour
	{
		private bool HasReloaded;


		private static Gun lostSidearm;

		public static void Add()
		{

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Lost Sidearm", "lost_revolver");
			Game.Items.Rename("outdated_gun_mods:lost_sidearm", "bot:lost_sidearm");
			gun.gameObject.AddComponent<LostSidearm>();
			gun.SetShortDescription("Decay");
			gun.SetLongDescription("Add text here before releasing");
			
			gun.SetupSprite(null, "lost_revolver_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			
			
			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			//gun.barrelOffset.transform.localPosition += new Vector3(10f, 0f, 0f);

			gun.DefaultModule.ammoCost = 1;
			
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.StarterGunForAchievement = true;
			
			
			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;


			//gun.DefaultModule.usesOptionalFinalProjectile = true;
			//gun.DefaultModule.numberOfFinalProjectiles = 1;

			//Projectile replacementProjectile = gun2.DefaultModule.projectiles[0];

			

			//gun.DefaultModule.finalProjectile = replacementProjectile;
			//gun.DefaultModule.finalCustomAmmoType = gun2.DefaultModule.customAmmoType;
			//gun.DefaultModule.finalAmmoType = gun2.DefaultModule.ammoType;

			//gun.DefaultModule.finalProjectile.statusEffectsToApply.Add(Debuffs.decayEffect);
			//gun.DefaultModule.finalProjectile.ChanceToTransmogrify = 0;

			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.SPECIAL;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.SHITTY;
			gun.CanBeDropped = true;
			gun.PreventStartingOwnerFromDropping = true;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			
			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 5f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;





			ETGMod.Databases.Items.Add(gun, null, "ANY");

			lostSidearm = gun;
			BotsItemIds.LostSidearm = gun.PickupObjectId;

			//Tools.BeyondItems.Add(gun.PickupObjectId);
		}

		static TrailRenderer tr;

		public override void PostProcessProjectile(Projectile projectile)
		{
			base.PostProcessProjectile(projectile);


			if (setup)
			{
				setup = true;

				

				projectile.sprite.renderer.enabled = true;

				var tro = projectile.gameObject.AddChild("trail object");
				tro.transform.position = projectile.transform.position;
				tro.transform.localPosition = new Vector3(1f, 0.2f, 0f);

				tr = tro.AddComponent<TrailRenderer>();
				tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				tr.receiveShadows = false;
				var mat = new Material(Shader.Find("Sprites/Default"));
				mat.mainTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/wip.png");
				mat.SetColor("_Color", new Color32(107, 0, 173, 255));
				tr.material = mat;
				tr.time = 0.2f;
				tr.minVertexDistance = 0.1f;
				tr.startWidth = 1f;
				tr.endWidth = 0f;
				tr.startColor = Color.white;
				tr.endColor = new Color(1f, 1f, 1f, 0f);
			}

			

			//projectile.OverrideMotionModule = new LostProjectile(ItemAPI.ResourceExtractor.GetTextureFromResource("ExampleMod/Resources/Other/squaregrad.png"));

			//projectile.OnHitEnemy += ApplyDecay;
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

		


		public static void ApplyDecay(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
		{
			if (hitRigidbody.aiActor != null)
			{
				hitRigidbody.aiActor.ApplyEffect(Debuffs.decayEffect);

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
				AkSoundEngine.PostEvent("Play_OBJ_blackhole_close_01", base.gameObject);
			}
		}



		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}

		public class ThisThingIsAsUselessAsMe : MonoBehaviour
		{
		}

		public class LostProjectile : ProjectileMotionModule
		{
			Texture2D _gradTexture;

			public LostProjectile(Texture2D gradTexture)
			{
				_gradTexture = gradTexture;
			}

			public override void UpdateDataOnBounce(float angleDiff)
			{
				//throw new NotImplementedException();
			}

			bool setup = false;
			float initialRot = 0f;
			Vector2 targetPosition;
			Vector2 startPosition;

			float startOpacity = 0f;
			float additionalMagnitude = 0f;
			float additionalMagnitude2 = 0f;
			int direction = 1;

			TrailRenderer tr;

			public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
			{
				
			}

			Color GetColorFromSprite(int ind)
			{
				switch (ind)
				{
					case 2:
						return new Color32(242, 244, 245, 255);
					case 3:
						return new Color32(211, 212, 184, 255);
					case 4:
						return new Color32(255, 140, 140, 255);
					case 5:
						return new Color32(135, 135, 135, 255);
					case 6:
						return new Color32(255, 254, 219, 255);
					case 7:
						return new Color32(255, 181, 102, 255);
					default:
						return new Color32(137, 215, 232, 255);
				}
			}

			void ResetTargetPosition(Projectile source)
			{
				if (source.Owner is PlayerController player)
				{
					targetPosition = player.unadjustedAimPoint.XY();
				}
				else if (source.Owner is AIActor ai)
				{
					targetPosition = ai.PlayerTarget.CenterPosition;
				}
				else
				{
					targetPosition = new Vector2(10f, 0f).Rotate(initialRot);
				}
			}
		}

	}
}
