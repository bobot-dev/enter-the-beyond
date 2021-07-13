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
		public static void Init()
		{
			//The name of the item
			string itemName = "Otherwordly Fury";
			string resourceName = "BotsMod/sprites/wip";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<OtherwordlyFury>();

			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.SPECIAL;


			

			Tools.BeyondItems.Add(item.PickupObjectId);

		}

		protected override void DoEffect(PlayerController user)
		{
			foreach (var obj in orbitals)
			{
				StartCoroutine(dofunnybeam(obj.GetComponent<PlayerOrbital>()));
			}
			
			base.DoEffect(user);
		}

		public override void Pickup(PlayerController player)
		{

			
			base.Pickup(player);

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
							SpeculativeRigidbody speculativeRigidbody = obj.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(6, 8));
							speculativeRigidbody.CollideWithTileMap = false;
							speculativeRigidbody.CollideWithOthers = true;
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

						//var obj = PlayerOrbitalItem.CreateOrbital(base.LastOwner, (PickupObjectDatabase.GetById(260) as PlayerOrbitalItem).OrbitalPrefab.gameObject, false);
						//var obj2 = PlayerOrbitalItem.CreateOrbital(base.LastOwner, (PickupObjectDatabase.GetById(466) as PlayerOrbitalItem).OrbitalPrefab.gameObject, false);
						//var obj3 = PlayerOrbitalItem.CreateOrbital(base.LastOwner, (PickupObjectDatabase.GetById(262) as PlayerOrbitalItem).OrbitalPrefab.gameObject, false);


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
			Gun fuckYouDie = PickupObjectDatabase.GetById(763) as Gun;

			Projectile currentProjectile = fuckYouDie.DefaultModule.GetCurrentProjectile();

			var beam = currentProjectile.GetComponent<BeamController>();
			//beam.AdjustPlayerBeamTint(colorKeys[num], 10000);


			BotsModule.Log(obj.sprite.WorldCenter.ToString());
			BotsModule.Log(obj.transform.localRotation.z.ToString());
			StartCoroutine(this.HandleFireShortBeam(currentProjectile, this.LastOwner, 5, obj.sprite.WorldCenter, obj, obj.transform.localRotation.z));
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
			Vector2 vector = obj.transform.position;
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
