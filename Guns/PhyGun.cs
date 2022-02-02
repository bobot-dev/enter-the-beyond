using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class PhyGun : GunBehaviour
	{
		public static void Add()
		{

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Physics Gun", "phy_gun");
			Game.Items.Rename("outdated_gun_mods:physics_gun", "bot:physics_gun");
			gun.gameObject.AddComponent<PhyGun>();
			gun.SetShortDescription("");
			gun.SetLongDescription("Add text here before releasing");

			gun.SetupSprite(null, "phy_gun_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			//gun.SetAnimationFPS(gun.reloadAnimation, 10);


			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			gun.gunSwitchGroup = "EnergyCannon";

			//gun.barrelOffset.transform.localPosition += new Vector3(10f, 0f, 0f);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;

			gun.reloadTime = 0f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;

			gun.DefaultModule.cooldownTime = 0.15f;
			gun.DefaultModule.numberOfShotsInClip = 1000;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.gunClass = GunClass.BEAM;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;


			List<string> BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/PhyGun/phy_gun_laser_middle_001",
				"BotsMod/sprites/beam/PhyGun/phy_gun_laser_middle_002",

			};

			//Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
			//moonraker bloom material
			BasicBeamController beamComp = projectile.GenerateBeamPrefab("BotsMod/sprites/beam/PhyGun/phy_gun_laser_middle_001", new Vector2(16, 4), new Vector2(0, 0), BeamAnimPaths, 8, glows: true);
			projectile.gameObject.SetActive(false);
			projectile.gameObject.AddComponent<PhyGunBeam>();
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);



			beamComp.ContinueBeamArtToWall = true;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.endType = BasicBeamController.BeamEndType.Vanish;

			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissiveColorPower = 7;
			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissivePower = 42;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 0.1f;
			projectile.baseData.speed = 500f;
			projectile.baseData.force = 0f;
			projectile.baseData.range = 1000f;





			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}
		/*
		 private IEnumerator LerpToSize(AIActor target, Vector2 targetScale)
        {
            float elapsed = 0f;
            Vector2 startScale = target.EnemyScale;
            int cachedLayer = target.gameObject.layer;
            int cachedOutlineLayer = cachedLayer;
            target.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
            cachedOutlineLayer = SpriteOutlineManager.ChangeOutlineLayer(target.sprite, LayerMask.NameToLayer("Unpixelated"));
            

            
        }
		*/


	}

	class PhyGunBeam : MonoBehaviour
	{
		Projectile proj;
		BasicBeamController beam;
		float coolDown = 3;
		float coolDownRemaining = 0;
		void Start()
        {
			proj = this.GetComponent<Projectile>();
			beam = this.GetComponent<BasicBeamController>();
			if (beam.Owner is PlayerController)
            {
				proj.OnHitEnemy += PhyGunBeam_PostProcessBeam;
            }
		}

        private void PhyGunBeam_PostProcessBeam(Projectile proj, SpeculativeRigidbody target, bool fatal)
        {
			if (target.aiActor != null && coolDownRemaining <= 0)
            {
				if (Input.GetKey(KeyCode.Plus))
				{
					target.aiActor.EnemyScale += Vector2.one / 10;
					coolDownRemaining = coolDown;
				}
				if (Input.GetKey(KeyCode.Minus))
				{
					target.aiActor.EnemyScale -= Vector2.one / 10;
					coolDownRemaining = coolDown;
				}
			}

		}


		void Update()
        {
			if (coolDownRemaining >= 0)
            {
				coolDownRemaining -= Time.deltaTime;
			}
		}
	}
}
