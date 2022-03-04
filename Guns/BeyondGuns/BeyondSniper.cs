using BreakAbleAPI;
using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class BeyondSniper : GunBehaviour
	{
		static DebrisObject barrelTop;
		static DebrisObject barrelBottom;

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("beyond sniper", "beyond_sniper");
			Game.Items.Rename("outdated_gun_mods:beyond_sniper", "bot:beyond_sniper");
			gun.gameObject.AddComponent<BeyondSniper>();
			gun.SetShortDescription("Aim High");
			gun.SetLongDescription("The projectiles fired by this weapon seem to do more damage to the heads of enemies. \n\nA very powerful rifle used by the Deadeyes that roam the beyond.");
			GunExt.SetupSprite(gun, null, "beyond_sniper_idle_001", 8);
			gun.SetAnimationFPS(gun.reloadAnimation, 8);
			gun.SetAnimationFPS(gun.introAnimation, 10);
			gun.SetAnimationFPS(gun.idleAnimation, 10);


			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_sniper_fire");
			float[] offsetsX = new float[] { -0.9375f, -1.0625f, -0.9375f, -0.9375f };
			float[] offsetsY = new float[] { -0.2500f, -0.2500f, -0.2500f, -0.2500f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++) 
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]); 
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip.frames[2].eventInfo = "launch";
			animationclip.frames[2].triggerEvent = true;
			
			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_sniper_reload");
			offsetsX = new float[] { -1.1875f, -1.1250f, -0.9375f, -0.9375f, -0.9375f, -0.9375f, -0.9375f };
			offsetsY = new float[] { -0.6875f, -0.8750f, -0.8750f, -0.4375f, -0.3750f, -0.2500f, -0.2500f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_sniper_idle");
			offsetsX = new float[] { -0.9375f };
			offsetsY = new float[] { -0.2500f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			barrelTop = BreakableAPIToolbox.GenerateDebrisObject("BotsMod/sprites/VFX/Debris/broken_barrel_001.png", true, 1.5f, 3f, 390, 0, null, 0.4f, "", null, 1, false, null, 0);
			barrelTop.breaksOnFall = true;

			barrelBottom = BreakableAPIToolbox.GenerateDebrisObject("BotsMod/sprites/VFX/Debris/broken_barrel_002.png", true, 1.5f, 3f, 390, 0, null, 0.4f, "", null, 1, false, null, 0);
			barrelBottom.breaksOnFall = true;

			gun.barrelOffset.localPosition = new Vector3(1.4375f - 0.9375f, 0.3125f - 0.2500f, 0f);

			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(5) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1f;

			gun.DefaultModule.cooldownTime = 1.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 1;

			gun.SetBaseMaxAmmo(50);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.A;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			var sniperBullet = projectile.gameObject.AddComponent<SniperBullet>();
			sniperBullet.CopyFromNormalProjectile(projectile);
			Destroy(projectile);
			gun.DefaultModule.projectiles[0] = sniperBullet;
			//projectile.transform.parent = gun.barrelOffset;
			sniperBullet.baseData.damage = 30;
			//projectile.baseData.speed

			BotsItemIds.BeyondSniper = gun.PickupObjectId;

			Tools.BeyondItems.Add(gun.PickupObjectId);

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

		void OnDestroy()
		{
			gun.spriteAnimator.AnimationEventTriggered -= HandleAnimationEvent;
		}

		void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
		{
			tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
			if (BotsModule.debugMode) ETGModConsole.Log(frame.eventInfo);
			var owner = gun.CurrentOwner as PlayerController;
			if (owner != null)
			{

				if (frame?.eventInfo == "launch")
				{
					var dir = this.gun.CurrentAngle < 0 ? -this.gun.CurrentAngle : this.gun.CurrentAngle;

					Vector3 vector = Quaternion.Euler(0f, 0f, dir) * (this.transform.PositionVector2().normalized * 2).ToVector3ZUp(2);
					GameObject gameObject = SpawnManager.SpawnDebris(barrelBottom.gameObject, this.gun.barrelOffset.position.WithZ(-0.05f), Quaternion.Euler(0f, 0f, dir));
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (this.gun.sprite.attachParent != null && component != null)
					{
						component.attachParent = this.gun.sprite.attachParent;
						component.HeightOffGround = this.gun.sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					vector = Vector3.Scale(vector, Vector3.one) * 0.7f;
					component2.Trigger(vector, 0.5f, 1);
				}

				if (frame?.eventInfo == "launch")
				{
					var dir = this.gun.CurrentAngle < 0 ? -this.gun.CurrentAngle : this.gun.CurrentAngle;

					Vector3 vector = Quaternion.Euler(0f, 0f, dir) * (this.transform.PositionVector2().normalized * 2).ToVector3ZUp(2);
					GameObject gameObject = SpawnManager.SpawnDebris(barrelTop.gameObject, this.gun.barrelOffset.position.WithZ(-0.05f), Quaternion.Euler(0f, 0f, dir));
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (this.gun.sprite.attachParent != null && component != null)
					{
						component.attachParent = this.gun.sprite.attachParent;
						component.HeightOffGround = this.gun.sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					vector = Vector3.Scale(vector, Vector3.one) * 0.7f;
					component2.Trigger(-vector, 0.5f, 1);
				}
			}

		}

		/*public override void PostProcessProjectile(Projectile projectile)
        {
			projectile.OnHitEnemy += BOOMHEADSHOT;

			base.PostProcessProjectile(projectile);
        }

		void BOOMHEADSHOT(Projectile proj, SpeculativeRigidbody target, bool killeded)
        {

			//ETGModConsole.Log($"{proj.sprite.WorldCenter} -- {target.UnitTopRight}-{target.UnitTopLeft} -- {target.UnitCenterLeft}-{target.UnitCenterRight}");
			var headBottom = (target.sprite.WorldTopCenter - target.sprite.WorldCenter)/1.5f;
			if (proj.sprite.WorldCenter.y > (target.sprite.WorldCenter + headBottom).y)
            {
				target.aiActor.healthHaver.ApplyDamage(1984, Vector2.zero, "DIE");
				ETGModConsole.Log($"wow :O");


			}
		}*/

    }
}
