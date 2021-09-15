using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{

	//item breaks when dropped fix that or else...
    class OtherwordlyFury : PlayerItem
    {

		static Projectile beam;

		public static void Init()
		{
			//The name of the item
			string itemName = "Otherwordly Fury";
			string resourceName = "BotsMod/sprites/otherworldly_fury";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<OtherwordlyFury>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.B;

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

			Projectile projectile4 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
			//moonraker bloom material
			BasicBeamController beamComp = projectile4.GenerateBeamPrefab("BotsMod/sprites/beam/OtherWorldlyFury/otherworldly_fury_beam_middle_001",
				new Vector2(32, 5),
				new Vector2(0, 2),
				BeamAnimPaths, 8,
				ImpactAnimPaths, 13,
				new Vector2(0, 0),
				new Vector2(0, 0),
				glows: true
				);
			projectile4.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile4.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile4);
			projectile4.baseData.damage = 20f;
			projectile4.baseData.range = 100;
			projectile4.baseData.speed = 200;
			projectile4.PenetratesInternalWalls = true;

			beamComp.ContinueBeamArtToWall = false;
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
			beamComp.endType = BasicBeamController.BeamEndType.Vanish;




			beamComp.ProjectileAndBeamMotionModule = new HelixProjectileMotionModule();
			beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;

			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissiveColorPower = 7;
			beamComp.gameObject.GetOrAddComponent<EmmisiveBeams>().EmissivePower = 42;


			beam = projectile4;

			Tools.BeyondItems.Add(item.PickupObjectId);

		}

		protected override void DoEffect(PlayerController user)
		{
			foreach (var obj in orbitals)
			{
				StartCoroutine(this.HandleFireShortBeam(beam, this.LastOwner, 5, obj.GetComponent<PlayerOrbital>().sprite.WorldCenter, obj.GetComponent<PlayerOrbital>(), obj.GetComponent<PlayerOrbital>().transform.localRotation.z));
			}
			
			base.DoEffect(user);
		}

		public override void Pickup(PlayerController player)
		{

			player.OnNewFloorLoaded += FloorLoaded;
			base.Pickup(player);

		}


		public void FloorLoaded(PlayerController player)
		{
			foreach (var obj in orbitals)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			orbitals.Clear();
		}

        protected override void OnDestroy()
        {
			foreach (var obj in orbitals)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			orbitals.Clear();
			this.LastOwner.OnNewFloorLoaded -= FloorLoaded;
			base.OnDestroy();
        }

        protected override void OnPreDrop(PlayerController user)
        {
			foreach (var obj in orbitals)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			orbitals.Clear();
			user.OnNewFloorLoaded -= FloorLoaded;
			base.OnPreDrop(user);
        }

        public override void Update()
		{
			// && base.LastOwner.CurrentItem != null
			if (base.LastOwner != null)
			{
				if (base.LastOwner.CurrentItem == this)
				{
					if (orbitals.Count <= 0)
					{
						for (int i = 0; i < 3; i++)
						{

							var obj = SpriteBuilder.SpriteFromResource("BotsMod/sprites/otherworldlyfuryorbital");
							obj.name = "Otherwordly Fury Orbital";
							obj.layer = 22;
							SpeculativeRigidbody speculativeRigidbody = obj.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(6, 8));
							speculativeRigidbody.CollideWithTileMap = false;
							speculativeRigidbody.CollideWithOthers = false;
							speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
							var orb = obj.AddComponent<PlayerOrbital>();
							orb.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
							orb.shouldRotate = false;
							orb.orbitDegreesPerSecond = 120;
							orb.orbitRadius = 3;
							orb.SetOrbitalTier(0);
							UnityEngine.Object.DontDestroyOnLoad(obj);
							FakePrefab.MarkAsFakePrefab(obj);
							obj.SetActive(false);


							var orbObj = PlayerOrbitalItem.CreateOrbital(base.LastOwner, obj, false);

							orbitals.Add(orbObj);
						}
					}
				} 
				else
				{
					foreach (var obj in orbitals)
					{
						UnityEngine.Object.DestroyImmediate(obj);
					}
					orbitals.Clear();
				}
			}

		}


		public IEnumerator dofunnybeam(PlayerOrbital obj)
		{
			
			yield break;
		}

		private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration, Vector3 pos, PlayerOrbital obj, float angle = 0)
		{
			float elapsed = 0f;
			BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, source.CurrentGun.CurrentAngle, pos, obj);
			yield return null;
			while (elapsed < duration && obj != null)
			{
				elapsed += BraveTime.DeltaTime;
				this.ContinueFiringBeam(beam, source, angle, pos, obj);
				yield return null;
			}
			this.CeaseBeam(beam);
			yield break;
		}

		private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, PlayerOrbital obj)
		{
			Vector2 vector = obj.transform.position;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
			Projectile component = gameObject.GetComponent<Projectile>();
			component.Owner = source;
			BeamController component2 = gameObject.GetComponent<BeamController>();
			component2.Owner = source;
			component2.HitsPlayers = false;
			component2.HitsEnemies = true;
			Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
			component2.Direction = v;
			component2.Origin = vector;
			return component2;
		}

		private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, PlayerOrbital obj)
		{
			Vector2 vector = obj.sprite.WorldCenter;
			beam.Direction = BraveMathCollege.DegreesToVector(source.CurrentGun.CurrentAngle, 1f);
			beam.Origin = vector;
			beam.LateUpdatePosition(vector);
		}

		private void CeaseBeam(BeamController beam)
		{
			beam.CeaseAttack();
		}

		GameObject orbital;

		List<GameObject> orbitals = new List<GameObject>();
	}
}
