using Gungeon;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class Judgment : GunBehaviour
	{

		static Projectile projectile;
		public static void Add()
		{

			//it'll be easy she said a simple melee weapon she said but nooooooooo zatherz fucking bull shit had to ruin it :(


			
			Gun gun = ETGMod.Databases.Items.NewGun("Judgment", "judgment");
			Game.Items.Rename("outdated_gun_mods:judgment", "bot:judgment");
			gun.gameObject.AddComponent<Judgment>();
			gun.SetShortDescription("Judge Jury and Executioner");
			gun.SetLongDescription("A powerful sword that can launch its user forward every third swing.\n\nA weapon once used by executioners in the beyond but it seems after the Overseer's rise to power this tradition has died out.");
			GunExt.SetupSprite(gun, null, "judgment_idle_001", 8);
			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_fire");
			float[] offsetsX = new float[] { 0.4375f, -0.3125f, -0.3125f, -0.5000f, 1.0000f, 0.5625f, 0.5625f };
			float[] offsetsY = new float[] { 1.1250f, 0.9375f, 0.9375f, 0.9375f, 0.4375f, -2.0000f, -2.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 20;

			gun.UpdateAnimation("fire2", null, true);
			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_fire2");

			if (animationclip == null)
            {
				BotsModule.Log("!WARNING!: \"Judgment\" could not be setup this is likely due to your version of ggb being out of date or not there at all please install gungeon go brrr version 1.1.2 or higher", BotsModule.LOCKED_CHARACTOR_COLOR, false);
				return;
            }

			offsetsX = new float[] { 0.3125f, 0.3125f, 1.0625f, 1.2500f, 1.3125f };
			offsetsY = new float[] { -2.3125f, -2.3125f, -2.2500f, -1.2500f, 0.5000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 20;
			gun.UpdateAnimation("3fire", null, true);
			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_3fire");
			offsetsX = new float[] { 1.0625f, -0.1875f, 0.6875f, 0.8125f, 0.8125f };
			offsetsY = new float[] { 0.1875f, -0.3125f, -0.3750f, -0.5000f, -0.5000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 12;

			animationclip.frames.First().eventInfo = "startDash";
			animationclip.frames.First().triggerEvent = true;
			animationclip.frames.Last().eventInfo = "endDash";
			animationclip.frames.Last().triggerEvent = true;

			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_idle");
			offsetsX = new float[] { 0.3750f };
			offsetsY = new float[] { 0.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }

			gun.UpdateAnimation("2idle", null, false);
			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_2idle");
			offsetsX = new float[] { 0.5625f };
			offsetsY = new float[] { -2.0000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 12;
			gun.UpdateAnimation("3idle", null, false);
			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_3idle");
			offsetsX = new float[] { 1.3125f };
			offsetsY = new float[] { 0.5000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 12;

			animationclip = gun.sprite.spriteAnimator.GetClipByName("judgment_reload");
			offsetsX = new float[] { 0.7500f, 0.3750f, -0.9375f, 0.8750f, 0.3125f, 0.3750f };
			offsetsY = new float[] { -0.0625f, 0.2500f, -1.0000f, -2.0000f, -1.0000f, -0.2500f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) { int id = animationclip.frames[i].spriteId; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position0.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position1.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position2.y += offsetsY[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.x += offsetsX[i]; animationclip.frames[i].spriteCollection.spriteDefinitions[id].position3.y += offsetsY[i]; }
			animationclip.fps = 12;
			//gun.carryPixelOffset = new IntVector2(12, 0);

			//var particle = UnityEngine.Object.Instantiate<GameObject>(BeyondPrefabs.AHHH.LoadAsset<GameObject>("Sword"), gun.gameObject.transform);
			//particle.transform.localPosition = new Vector3(0.6875f, 0.375f, 1);
			//particle.layer = 23;


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);


			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0.6f;

			gun.DefaultModule.cooldownTime = 0.55f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 3;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.S;


			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Beyond Sword Slash", "BotsMod/sprites/CustomGunAmmoTypes/judgment_custom_clip_001", "BotsMod/sprites/CustomGunAmmoTypes/judgment_custom_clip_empty_001");

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			gun.muzzleFlashEffects = null;
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			projectile.baseData.damage = 45f;
			projectile.baseData.force = 10f;

			ProjectileSlashingBehaviour slashing = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
			//slashing.DestroyBaseAfterFirstSlash = true;
			slashing.SlashDamageUsesBaseProjectileDamage = true;
			slashing.UsesAngleVariance = false;
			slashing.DoSound = true;
			slashing.SlashRange = 1f;
			slashing.SlashDimensions = 90f;
			slashing.delayBeforeSlash = 0.2f;
			slashing.InteractMode = SlashDoer.ProjInteractMode.DESTROY;
			slashing.playerKnockback = 5;
			slashing.playerKnockbackImmutable = true;

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

		new void Start()
        {
			base.Start();
			if (gun.spriteAnimator.AnimationEventTriggered == null)
            {
				gun.spriteAnimator.AnimationEventTriggered = this.HandleAnimationEvent;
			}
			else
            {
				gun.spriteAnimator.AnimationEventTriggered += this.HandleAnimationEvent;
			}

			//gun.spriteAnimator.AnimationEventTriggered += HandleAnimationEvent;

		}

        public override void OnDropped()
        {
			attackStage = 1;
			this.gun.shootAnimation = $"judgment_fire";
			this.gun.idleAnimation = $"judgment_idle";
			gun.spriteAnimator.AnimationEventTriggered -= HandleAnimationEvent;
			base.OnDropped();
        }

        void OnDestroy()
        {


			gun.spriteAnimator.AnimationEventTriggered -= HandleAnimationEvent;
		}

		bool isAlreadyEthereal;
		void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
		{
			tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
			if (BotsModule.debugMode) ETGModConsole.Log(frame.eventInfo);
			var owner = gun.CurrentOwner as PlayerController;
			if (owner != null)
            {

				if (frame?.eventInfo == "startDash")
				{
					//isAlreadyEthereal = !owner.ReceivesTouchDamage;

					//if (!isAlreadyEthereal)
					//{
					owner.healthHaver.IsVulnerable = false;
						
					//}
					

					if (owner.gameObject.GetComponent<ImprovedAfterImage>() == null)
					{
						afterImage = owner.gameObject.AddComponent<ImprovedAfterImage>();
					}
					afterImage.dashColor = new Color32(255, 0, 247, 255);
					afterImage.spawnShadows = true;
					afterImage.shadowTimeDelay = 0.025f;
					owner.SetInputOverride("THY PUNISHMENT IS DEATH");
					ETGMod.StartGlobalCoroutine(DelayEthereal(owner));
				}
				else if (frame?.eventInfo == "endDash")
				{
					
				}
			}

		}
		List<AIActor> ignore = new List<AIActor>();
		IEnumerator DelayEthereal(PlayerController owner)
        {
			yield return null;

			//owner.knockbackDoer.ApplyKnockback(angle, 260, 0.25f, false);
			
			yield return new WaitForSeconds(0.5f);
			//if (!isAlreadyEthereal)
			//{
				owner.healthHaver.IsVulnerable = true;
			//}
			owner.ClearInputOverride("THY PUNISHMENT IS DEATH");
			if (owner.gameObject.GetComponent<ImprovedAfterImage>() != null)
			{
				afterImage.spawnShadows = false;
			}

		}

		ImprovedAfterImage afterImage = new ImprovedAfterImage()
		{
			dashColor = new Color32(255, 0, 247, 255),
			spawnShadows = false,
		};


		public int attackStage = 1;

		void PrepNextAttack()
        {
			attackStage++;
			if (attackStage >= 4)
            {
				attackStage = 1;

			}

			if (attackStage == 3)
			{
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().playerKnockback = 260;
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().delayBeforeSlash = 0f;

			}
			else if (attackStage == 2)
            {
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().playerKnockback = 5;
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().delayBeforeSlash = 0f;
			}
			else
			{
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().playerKnockback = 5;
				projectile.gameObject.GetComponent<ProjectileSlashingBehaviour>().delayBeforeSlash = 0;
			}

			switch(attackStage)
            {
				case 1:
					this.gun.shootAnimation = $"judgment_fire";
					this.gun.idleAnimation = $"judgment_idle";
					break;
				case 2:
					this.gun.shootAnimation = $"judgment_fire2";
					this.gun.idleAnimation = $"judgment_2idle";
					break;
				case 3:
					this.gun.shootAnimation = $"judgment_3fire";
					this.gun.idleAnimation = $"judgment_3idle";
					break;
			}

			
			//
        }

        //public override void OnInitializedWithOwner(GameActor actor)
        //{
        //    base.OnInitializedWithOwner(actor);
        //}

        protected void Update()
		{

			//if (gun.CurrentOwner != null)
            //{
			//	OnInitializedWithOwner(gun.CurrentOwner);

			//}

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

			if(this.gun?.CurrentOwner?.GetAbsoluteParentRoom() != null && afterImage && afterImage.spawnShadows)
            {


				//var angle = (this.gun.CurrentOwner as PlayerController).unadjustedAimPoint.XY() - this.gun.CurrentOwner.CenterPosition;

				//-1f * BraveMathCollege.DegreesToVector(this.gunAngle, 1f)
				// 0.5f

				//this.gun.CurrentOwner.specRigidbody.Velocity += new Vector2(Mathf.Lerp(this.gun.CurrentOwner.specRigidbody.Velocity.x, angle.normalized.x * 26, 0.1f), Mathf.Lerp(this.gun.CurrentOwner.specRigidbody.Velocity.y, angle.normalized.y * 26, 0.1f));
				this.gun?.CurrentOwner?.GetAbsoluteParentRoom().ApplyActionToNearbyEnemies(this.gun.CurrentOwner.sprite.WorldCenter, 1.3f, delegate (AIActor enemy, float dist)
				{
					if (enemy && enemy.healthHaver)
					{
						enemy.healthHaver.ApplyDamage(30, (this.gun.CurrentOwner as PlayerController).unadjustedAimPoint.XY() - (this.gun.CurrentOwner as PlayerController).CenterPosition, "JUDGMENT!", CoreDamageTypes.None, DamageCategory.Normal);
						ignore.Add(enemy);
					}
				});
			}
			else if (afterImage && !afterImage.spawnShadows && ignore.Count > 0)
            {
				ignore.Clear();
            }
		}


		IEnumerator DoDashVelocity(float time, Vector2 direction, float force)
        {
			float elTime = 0;

			while (time >= elTime)
            {
				elTime += Time.deltaTime;
				yield return null;
				//direction.normalized * (force / 10f);
			}

		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{


			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				attackStage = 0;
				PrepNextAttack();
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
			}
		}



		public override void OnPostFired(PlayerController player, Gun gun)
		{
			PrepNextAttack();
			gun.PreventNormalFireAudio = true;
			//AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}

		private bool HasReloaded;

	}
}
