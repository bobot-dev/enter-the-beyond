using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace BotsMod
{
    class HellsRevolver : GunBehaviour
	{
		private bool HasReloaded;

		public static GameObject coinPrefab;

		public static void Add()
		{
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(383) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Hell's Revolver", "hells_revolver");
			Game.Items.Rename("outdated_gun_mods:hell's_revolver", "bot:hells_revolver");
			var meNeedMakeCoin = gun.gameObject.AddComponent<HellsRevolver>();
			gun.SetShortDescription("Blodd is Fuel");
			gun.SetLongDescription("This weapon uses blood of fallen foes to genrate ammo.\n\nA powerful revolver once used by a war robot fighting it's way through bullet hell.");

			gun.SetupSprite(null, "hells_revolver_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);
			gun.gameObject.AddComponent<UltraKillGun>();

			Gun other = PickupObjectDatabase.GetById(99) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(50);
			gun.InfiniteAmmo = false;

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;

			//gun.barrelOffset = gun4,

			//gun.damageModifier = 1;
			gun.reloadTime = 0.7f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;

			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.14f;
			gun.DefaultModule.positionOffset = new Vector3(0,0,0);
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.quality = PickupObject.ItemQuality.B;
			gun.gunClass = GunClass.CHARGE;
			//Projectile projectile = Tools.SetupProjectile(other.DefaultModule.projectiles[0]);
			Projectile projectile = Tools.SetupProjectile(17);
			Projectile projectileBeam = Tools.SetupProjectile(383);

			List<string> BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_001",
				"BotsMod/sprites/beam/LostFriend/lost_friend_laser_middle_002",

			};
			List<string> StartAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/LostFriend/lost_friend_laser_flare_001",
				"BotsMod/sprites/beam/LostFriend/lost_friend_laser_flare_002",
			};

			projectile.AddFlashRayBeam(BeamAnimPaths, new Vector2(16, 4), new Vector2(0, 0), 12, StartAnimPaths);

			gun.DefaultModule.projectiles[0] = projectile;
			gun.DefaultModule.projectiles.Add(projectileBeam);

			projectileBeam.transform.localPosition = new Vector3(0, 0, 0);

            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
            {
                new ProjectileModule.ChargeProjectile
                {
                    AmmoCost = 1,
                    Projectile = projectile,
                    ChargeTime = 0,
                },
                new ProjectileModule.ChargeProjectile
                {
                    AmmoCost = 10,
                    Projectile = projectileBeam,
                    ChargeTime = 0.7f,
                    UsedProperties = ProjectileModule.ChargeProjectileProperties.depleteAmmo  | ProjectileModule.ChargeProjectileProperties.ammo | ProjectileModule.ChargeProjectileProperties.shootAnim,
                    OverrideShootAnimation = "hells_revolver_critical_fire",
					

                }
            };
			// i forgot why these ids are here. ill leave them here coz i can B)
            //7319972676379809622 
            //5272075203329454387
            gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 9f;
			projectile.baseData.speed = 200;
			projectile.baseData.force = 15f;
			projectile.baseData.range = 1600000f;

			//projectileBeam.transform.parent = gun.barrelOffset;
			projectileBeam.baseData.damage = 30f;
			projectileBeam.baseData.speed = 300f;
			projectileBeam.baseData.force = 0f;
			projectileBeam.baseData.range = 10000f;
			projectileBeam.AppliesKnockbackToPlayer = true;
			projectileBeam.PlayerKnockbackForce = 32;

			projectileBeam.shouldFlipVertically = true;
			projectileBeam.shouldRotate = true;
			projectileBeam.shouldFlipHorizontally = false;
			

			projectileBeam.gameObject.AddComponent<Bleed>();
			projectile.gameObject.AddComponent<Bleed>();

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
			material.SetColor("_EmissiveColor", new Color32(176, 225, 225, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 50);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			/*var foreSyn = gun.gameObject.AddComponent<GunFormeSynergyProcessor>();
			foreSyn.Formes = new GunFormeData[]
			{

				new GunFormeData
				{
					FormeID = gun.PickupObjectId,
					RequiresSynergy = false,
				},
				new CustomGunFormeData
				{
					FormeID = Game.Items["bot:hells_revolver+the_marksman"].PickupObjectId,
					RequiredSynergyName = "The Marksman",
					RequiresSynergy = true,
				}
			};
			*/

			GameObject coin = SpriteBuilder.SpriteFromResource("BotsMod/sprites/TempCoinSprite-export", new GameObject("HellsCoin"));
			coin.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(coin);
			UnityEngine.Object.DontDestroyOnLoad(coin);

			coin.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(10, 10)).PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;//31

			SpriteOutlineManager.AddOutlineToSprite(coin.GetComponent<tk2dSprite>(), Color.black);

			coin.AddComponent<CoinController>();



			coinPrefab = coin;

			gun.associatedItemChanceMods = new LootModData[]
			{
				new LootModData
				{
					AssociatedPickupId = 1,
					DropRateMultiplier = 10
				}
			};
		}


	}
}
