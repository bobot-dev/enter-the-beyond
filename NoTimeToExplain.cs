using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Brave.BulletScript;
using Dungeonator;
using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace BotsMod
{
	// Token: 0x02000020 RID: 32
	public class NoTimeToExplain : GunBehaviour
	{



		// Token: 0x060000A0 RID: 160 RVA: 0x000063DC File Offset: 0x000045DC
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("No Time to Explain", "syntest");
			Game.Items.Rename("outdated_gun_mods:no_time_to_explain", "bot:no_time_to_explain");
			gun.gameObject.AddComponent<NoTimeToExplain>();
			gun.SetShortDescription("A single word etched onto the inside of the weapon's casing: Now.");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "syntest_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 24);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
			//gun.DefaultModule.customAmmoType = "hammer";

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.2f;

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = true;
			gun.DefaultModule.numberOfShotsInClip = 24;
			gun.DefaultModule.burstShotCount = 3;
			gun.DefaultModule.burstCooldownTime = 0.05f;

			gun.SetBaseMaxAmmo(496);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			Gun gun2 = PickupObjectDatabase.GetById(145) as Gun;

			//gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;

			//gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			//gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SKULL;
			//gun.DefaultModule.customAmmoType = 

			gun.DefaultModule.angleVariance = 0f;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;



			//gun.DefaultModule.customAmmoType = "locrtfsf_idle_001";
			//Gun gun3 = PickupObjectDatabase.GetById(504) as Gun;

			//gun.DefaultModule.customAmmoType = gun3.CustomAmmoType;
			Guid.NewGuid().ToString();
			//gun.encounterTrackable.EncounterGuid = "why wont you work please work im going mad";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			//projectile.baseData.damage *= 1.2f;
			//projectile.baseData.speed *= 0.7f;
			///	projectile.SetProjectileSpriteRight("locrtfsf_projectile_001", 7, 7, null, null);
			///	
			//gun.UsesRechargeLikeActiveItem = true;
			

			gunId = gun.PickupObjectId;


			ntte = gun;

			gun.PlaceItemInAmmonomiconAfterItemById(88);
			//PortalThing.Init();

		}
		public static Gun ntte;

		//public static GunBehaviour lGun = gun;
		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OnHitEnemy += this.HandleHitEnemy;
			base.PostProcessProjectile(projectile);
		}

		int timeChasm = 0;

		private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
		{		

			if (hitRigidbody.aiActor != null)
			{
				if (hitRigidbody.aiActor.FreezeAmount >= 1)
				{

					if (timeChasm < 10)
					{
						timeChasm = 1 + timeChasm;
						BotsModule.Log($"Time Chasm is at: {timeChasm}", BotsModule.TEXT_COLOR);
					}
					else if (timeChasm == 10)
					{
						PlayerController playerController = this.gun.CurrentOwner as PlayerController;
						PlayerOrbitalItem portal = PickupObjectDatabase.GetById(263).GetComponent<PlayerOrbitalItem>();
						//portal.OrbitalPrefab.shootCooldown -= 2;
						

						//PortalThing.CreateOrbital();
						CreatePortal(playerController);
						//PlayerOrbitalItem.CreateOrbital(playerController, PortalThing.orbitalPrefab.gameObject, false, null);
						timeChasm = 0;
					}
					
				}
			}


		}

		private void CreatePortal(PlayerController owner)
		{
			PortalThing portal = new PortalThing();
			PortalThing.BuildPrefab();
			var speeeeeeeeeeeeeeen = PlayerOrbitalItem.CreateOrbital(owner, portal.OrbitalPrefab.gameObject, false, portal);
		}

		private void SpawnPortal()
		{

		}


		// Token: 0x060000A1 RID: 161 RVA: 0x00006510 File Offset: 0x00004710
		protected void Update()
		{

			if (gun.CurrentOwner)
			{

				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}


		// Token: 0x060000A3 RID: 163 RVA: 0x00006629 File Offset: 0x00004829
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			if (gun.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam)
			{
				AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
			}

		}

		public override void OnDropped()
		{
			base.OnDropped();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006644 File Offset: 0x00004844
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{

				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
			}
		}

		public static int gunId;

		private bool HasReloaded;



	}
	public class PortalThing : PlayerOrbitalItem
	{
		public static void Init()
		{
			string name = "portal";
			string resourcePath = "BotsMod/sprites/wip";
			GameObject gameObject = new GameObject(name);
			var item = gameObject.AddComponent<PortalThing>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "you shouldnt see this thing ever";
			string longDesc = "if you have this something broke or you cheated :)";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			PortalThing.BuildPrefab();
			item.OrbitalPrefab = PortalThing.orbitalPrefab;
			//ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1.5f, StatModifier.ModifyMethod.ADDITIVE);
			item.PlaceItemInAmmonomiconAfterItemById(270);

			
		}

		public static void BuildPrefab()
		{
			bool flag = PortalThing.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			if (flag3)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("BotsMod/sprites/wip", null);
				gameObject.name = "Portal Thing";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(8, 8));
				PortalThing.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				PortalThing.orbitalPrefab.shouldRotate = false;
				PortalThing.orbitalPrefab.orbitRadius = 2.5f;
				PortalThing.orbitalPrefab.orbitDegreesPerSecond = 90f;
				PortalThing.orbitalPrefab.orbitDegreesPerSecond = 120f;
				PortalThing.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this.m_extantOrbital != null)
			{
				this.cooldown -= BraveTime.DeltaTime;
				if (this.m_extantOrbital.transform.position.GetAbsoluteRoom() != null && this.m_extantOrbital.transform.position.GetAbsoluteRoom().HasActiveEnemies(RoomHandler.ActiveEnemyType.All) && cooldown <= 0f)
				{
					AIActor aiactor = null;
					float nearestDistance = float.MaxValue;
					List<AIActor> activeEnemies = this.m_extantOrbital.transform.position.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					AIActor result;
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i]; ;
						bool flag3 = !aiactor2.healthHaver.IsDead;
						if (flag3)
						{
							float num = Vector2.Distance(this.m_extantOrbital.transform.position, aiactor2.CenterPosition);
							bool flag5 = num < nearestDistance;
							if (flag5)
							{
								nearestDistance = num;
								aiactor = aiactor2;
							}
						}
					}
					result = aiactor;
					if (result != null)
					{
						GameObject obj = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0].gameObject, this.m_extantOrbital.GetComponent<tk2dSprite>().WorldCenter, Quaternion.Euler(0, 0, BraveMathCollege.Atan2Degrees(result.sprite.WorldCenter - this.m_extantOrbital.GetComponent<tk2dSprite>().WorldCenter)));
						Projectile proj = obj.GetComponent<Projectile>();
						if (proj != null)
						{
							if (this.m_owner != null)
							{
								proj.Owner = this.m_owner;
								proj.Shooter = this.m_owner.specRigidbody;
							}
							proj.baseData.damage *= (1);
						}
						this.cooldown = 0.2f;
					}
				}
			}
		}

		public static PlayerOrbital orbitalPrefab;
		public static PlayerOrbital upgradeOrbitalPrefab;
		private float cooldown = 0;
	}
}
