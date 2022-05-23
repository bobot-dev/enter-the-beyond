using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BotsMod
{
    class BeamGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Chain Of Idk Ill Add Another Word", "beyond_beam");
			Game.Items.Rename("outdated_gun_mods:chain_of_idk_ill_add_another_word", "bot:chain_of_idk_ill_add_another_word");
			gun.gameObject.AddComponent<BeamGun>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "beyond_beam_idle_001", 8);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.barrelOffset.localPosition = new Vector3(1.75f, 0.5625f, 0f);
			foreach (var module in gun.Volley.projectiles)			
            {
				module.ammoCost = 1;
				module.shootStyle = ProjectileModule.ShootStyle.Beam;
				module.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
				module.cooldownTime = 0.5f;
				module.numberOfShotsInClip = 100;
				module.angleVariance = 0;

				module.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
				module.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Chain Beam", "BotsMod/sprites/CustomGunAmmoTypes/chain_beam_clip_001", "BotsMod/sprites/CustomGunAmmoTypes/chain_beam_clip_002");

			}

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.DefaultModule.cooldownTime = 0.5f;
			gun.DefaultModule.numberOfShotsInClip = 100;
			gun.DefaultModule.angleVariance = 0;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Chain Beam", "BotsMod/sprites/CustomGunAmmoTypes/chain_beam_clip_001", "BotsMod/sprites/CustomGunAmmoTypes/chain_beam_clip_002");


			gun.reloadTime = 0f;

			
			gun.InfiniteAmmo = false;
			

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.TwoHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");
			
			Projectile projectile = Tools.SetupProjectile(15);
			


			List<string> BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_001",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_002",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_003",

			};

			List<string> ImpactAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_001",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_002",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_003",
				"BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_impact_004",
			};

			BasicBeamController beamComp = projectile.GenerateBeamPrefab("BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_001",
				new Vector2(32, 5),
				new Vector2(0, 2),
				BeamAnimPaths, 8,
				ImpactAnimPaths, 13,
				new Vector2(0, 0),
				new Vector2(0, 0),
				glows: true
			);
			projectile.baseData.damage = 10f;
			projectile.baseData.range = 100;
			projectile.baseData.speed = 200;

			beamComp.ContinueBeamArtToWall = false;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.endType = BasicBeamController.BeamEndType.Vanish;
			beamComp.chargeDelay = 0.3f;
			beamComp.usesChargeDelay = true;

			var projectile2 = Tools.SetupProjectileAndObject(projectile);
			var projectile3 = Tools.SetupProjectileAndObject(projectile);


			BeamAnimPaths = new List<string>()
			{
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_001",
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_002",
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_003",
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_004",
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_005",
				"BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_006",

			};

			projectile3.gameObject.GetComponent<BeamController>().chargeDelay = 0.35f;

			var projectile4 = Tools.SetupProjectileAndObject(projectile3);



			BasicBeamController beamComp2 = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/ChainBeam/chain_middle_beam_middle_001",
				new Vector2(32, 7),
				new Vector2(0, 0),
				BeamAnimPaths, 8,
				ImpactAnimPaths, 13,
				new Vector2(0, 0),
				new Vector2(0, 0),
				glows: true
			);

			beamComp2.ContinueBeamArtToWall = false;
			beamComp2.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp2.endType = BasicBeamController.BeamEndType.Vanish;
			beamComp2.chargeDelay = 0.25f;
			beamComp2.usesChargeDelay = true;


			var projectile5 = Tools.SetupProjectileAndObject(projectile4);





			var emission = projectile.gameObject.GetComponent<EmmisiveBeams>();
			emission.EmissivePower *= 0.25f;

			var emission2 = projectile2.gameObject.GetComponent<EmmisiveBeams>();
			emission2.EmissivePower *= 0.25f;

			var emission3 = projectile3.gameObject.GetComponent<EmmisiveBeams>();
			emission3.EmissivePower *= 0.5f;

			var emission4 = projectile4.gameObject.GetComponent<EmmisiveBeams>();
			emission4.EmissivePower = 42;
			emission4.EmissiveColorPower = 7;

			var emission5 = projectile5.gameObject.GetComponent<EmmisiveBeams>();
			emission5.EmissivePower = 42;
			emission5.EmissiveColorPower = 7;

			var hBeam = projectile.gameObject.AddComponent<HelixBeam>();
			hBeam.invert = false;
			hBeam.helixAmplitude = 0.5f;
			hBeam.helixWavelength = 1.5f;
			hBeam.helixBeamOffsetPerSecond = 4;
			var hBeam2 = projectile2.gameObject.AddComponent<HelixBeam>();
			hBeam2.invert = true;
			hBeam2.helixAmplitude = 0.5f;
			hBeam2.helixWavelength = 1.5f;
			hBeam2.helixBeamOffsetPerSecond = 4;

			var hBeam3 = projectile3.gameObject.AddComponent<HelixBeam>();
			hBeam3.invert = true;
			hBeam3.helixAmplitude = 1f;
			hBeam3.helixWavelength = 1.5f;
			hBeam3.helixBeamOffsetPerSecond = 6;

			var cBeam = projectile4.gameObject.AddComponent<CurvedBeam>();
			cBeam.curveIntensity = 3.25f;
			cBeam.baseCurve = 1.5f;
			cBeam.invert = false;

			var cBeam2 = projectile5.gameObject.AddComponent<CurvedBeam>();
			cBeam2.curveIntensity = 3.25f;
			cBeam2.baseCurve = 1.5f;
			cBeam2.invert = true;

			gun.DefaultModule.projectiles[0] = projectile;
			gun.Volley.projectiles[1].projectiles[0] = projectile2;
			gun.Volley.projectiles[2].projectiles[0] = projectile3;
			gun.Volley.projectiles[3].projectiles[0] = projectile4;
			gun.Volley.projectiles[4].projectiles[0] = projectile5;

			//gun.Volley.projectiles[1].angleVariance = 0;
			id = gun.PickupObjectId;

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

		static int id;

		public void Start()
		{
			m_gun = GetComponent<Gun>();
			//m_gun.postp += HandleLightningStrike;
			m_gun.OnInitializedWithOwner += InitializedWithOwner;
			m_gun.OnDropped += OnDropped;
			if (m_gun.CurrentOwner != null)
			{
				InitializedWithOwner(m_gun.CurrentOwner);
			}
		}

		public void InitializedWithOwner(GameActor owner)
		{
			if (owner is PlayerController)
			{
				m_playerOwner = owner as PlayerController;
                m_playerOwner.GunChanged += GunChanged;
				m_playerOwner.PostProcessBeam += DoPostProcessBeam;
			}
		}

		void DoPostProcessBeam(BeamController beam)
        {
			if (m_playerOwner != null && m_playerOwner.CurrentGun?.PickupObjectId == id && beam.gameObject.GetComponent<HelixBeam>() && beam.projectile)
            {
				beam.projectile.OverrideMotionModule = new HelixProjectileMotionModule
				{
					helixAmplitude = beam.gameObject.GetComponent<HelixBeam>().helixAmplitude,
					helixWavelength = beam.gameObject.GetComponent<HelixBeam>().helixWavelength,
					helixBeamOffsetPerSecond = beam.gameObject.GetComponent<HelixBeam>().helixBeamOffsetPerSecond,
					ForceInvert = beam.gameObject.GetComponent<HelixBeam>().invert,

				};
			}
			else if (m_playerOwner != null && m_playerOwner.CurrentGun?.PickupObjectId == id && beam.gameObject.GetComponent<CurvedBeam>() && beam.projectile)
			{
				beam.projectile.OverrideMotionModule = new CurvedBeamMotionModule
				{
					baseCurve = beam.gameObject.GetComponent<CurvedBeam>().baseCurve,
					curveIntensity = beam.gameObject.GetComponent<CurvedBeam>().curveIntensity,
					invert = beam.gameObject.GetComponent<CurvedBeam>().invert,
				};
			}
		}


		private void GunChanged(Gun arg1, Gun arg2, bool arg3)
        {
            //throw new NotImplementedException();
        }

        protected void OnDestroy()
		{			
			OnDropped();
		}

		public void OnDropped()
		{
			if (m_playerOwner != null)
			{
				m_playerOwner.GunChanged -= GunChanged;
				m_playerOwner.PostProcessBeam -= DoPostProcessBeam;
				m_playerOwner = null;
			}
		}

		private Gun m_gun;
		private PlayerController m_playerOwner;

	}
}
