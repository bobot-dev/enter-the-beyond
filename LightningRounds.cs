using Gungeon;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class LightningRounds : PassiveItem
	{
        public static void Init()
        {

            string itemName = "Lightning Rounds";
            string resourceName = "BotsMod/sprites/lightning_bullet_001.png";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<LightningRounds>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Truly Shocking";
            string longDesc = "this item is purly for testing";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.B;

			item.PlaceItemInAmmonomiconAfterItemById(298);

			
		}

		protected override void Update()
		{
			PlayerController player = GameManager.Instance.PrimaryPlayer;
			if (player && this.extantLink == null && LinkVFXPrefab != null)
			{
				tk2dTiledSprite component = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				this.extantLink = component;
			}
			else if (player && this.extantLink != null)
			{
				foreach(AIActor enemy in player.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All))
				{
					if (enemy.GetComponent<NotReallyADebuff>() != null && badThingSpapiWillProbablyYellAtMeFor != null)
					{
						UpdateLink(enemy, this.extantLink, badThingSpapiWillProbablyYellAtMeFor);
					}
				}
				
			}
			else if (extantLink != null)// || actor == null)
			{
				SpawnManager.Despawn(extantLink.gameObject);
				extantLink = null;
			}

			base.Update();
		}

		

		public override void Pickup(PlayerController player)
		{
			player.PostProcessProjectile += PostProcessProjectile;
			base.Pickup(player);
		}

		protected override void OnDestroy()
		{
			Owner.PostProcessProjectile -= PostProcessProjectile;
			base.OnDestroy();
		}

		public override DebrisObject Drop(PlayerController player)
		{
			player.PostProcessProjectile -= PostProcessProjectile;
			return base.Drop(player);
		}

		private void PostProcessProjectile(Projectile bullet, float effectChanceScalar)
		{
			Gun gun = (!Owner) ? null : Owner.CurrentGun;
			float fireRateChance = 1f / gun.DefaultModule.cooldownTime;

			float shotgunNerf = gun.Volley.projectiles.Count * 5;
			var value = fireRateChance + shotgunNerf;
			float arg = 1f;

			if (gun && gun.DefaultModule != null)
			{
				float num = 0f;
				if (gun.Volley != null)
				{
					List<ProjectileModule> projectiles = gun.Volley.projectiles;
					for (int i = 0; i < projectiles.Count; i++)
					{
						num += projectiles[i].GetEstimatedShotsPerSecond(gun.reloadTime);
					}

					num /= projectiles.Count;
				}
				else if (gun.DefaultModule != null)
				{
					num += gun.DefaultModule.GetEstimatedShotsPerSecond(gun.reloadTime);
				}
				if (num > 0f)
				{
					arg = 3.5f / num;
				}

			}

			float num2 = this.chanceOfActivating;
			if (this.chanceOfActivating < 1f)
			{
				num2 = this.chanceOfActivating * effectChanceScalar;
			}
			var rng = UnityEngine.Random.value;
			//BotsModule.Log("dum dum rng: " + num2);
			if (rng < num2)
			{
				
				bullet.OnDestruction += Bullet_OnDestruction;
				//bullet.OnHitEnemy += ZapZap;
				bullet.AdjustPlayerProjectileTint(lightningColour, 10);
			}
			
		}

		float chanceOfActivating = 0.3f;

		//5/10 = rr(0, 10) < 4
		private void Bullet_OnDestruction(Projectile obj)
		{
			badThingSpapiWillProbablyYellAtMeFor = obj.specRigidbody.UnitCenter;
			
			//this.Owner.CurrentRoom.ApplyActionToNearbyEnemies(hitRigidbody.UnitCenter, 5f, delegate (AIActor enemy, float dist)
			int limit = 5;
			int range = 10;
			if (this.Owner.CurrentRoom != null)
			{
				if (this.Owner.PlayerHasActiveSynergy("Full Circuit"))
				{
					limit *= 2;
					range = 15;
				}
				ApplyActionToNearbyEnemiesWithALimit(obj.specRigidbody.UnitCenter, range, limit, this.Owner.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
				{

					if (enemy && enemy.healthHaver)
					{

						var linkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);

						tk2dTiledSprite component = SpawnManager.SpawnVFX(linkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
						this.extantLink = component;

						UpdateLink(enemy, component, obj.specRigidbody.UnitCenter);
						enemy.gameObject.AddComponent<NotReallyADebuff>();
						enemy.healthHaver.ApplyDamage(4, Vector2.zero, string.Empty, CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
						StartCoroutine(doTimerMagic(extantLink.gameObject));

					}


				});
			}
		}


		private void UpdateLink(AIActor target, tk2dTiledSprite m_extantLink, Vector2 landedPoint)
		{
			//SpeculativeRigidbody specRigidbody = target.specRigidbody;
			//SpeculativeRigidbody speculativeRigidbody = specRigidbody;
			Material material = m_extantLink.GetComponent<Renderer>().material;
			material.SetFloat("_BlackBullet", 0.995f);
			material.SetFloat("_EmissiveColorPower", 4.9f);

			Vector2 unitCenter = landedPoint;
			Vector2 unitCenter2 = target.specRigidbody.HitboxPixelCollider.UnitCenter;
			m_extantLink.transform.position = unitCenter;
			Vector2 vector = unitCenter2 - unitCenter;
			float num = BraveMathCollege.Atan2Degrees(vector.normalized);
			int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			m_extantLink.dimensions = new Vector2((float)num2, m_extantLink.dimensions.y);
			m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, num);
			m_extantLink.UpdateZDepth();


		}

		public static void ApplyActionToNearbyEnemiesWithALimit(Vector2 position, float radius, float limit,List<AIActor> activeEnemies, Action<AIActor, float> lambda)
		{
			float num = radius * radius;
			
			if (activeEnemies != null)
			{
				var l = 0;
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i])
					{
						bool flag = radius < 0f;
						Vector2 vector = activeEnemies[i].CenterPosition - position;
						if (!flag)
						{
							flag = (vector.sqrMagnitude < num);
						}
						if (flag)
						{
							if (l <= limit)
							lambda(activeEnemies[i], vector.magnitude);
							l++;
						}
					}
				}
			}
		}


		private static IEnumerator doTimerMagic(GameObject lightning)
		{
			yield return new WaitForSeconds(0.25f);
			SpawnManager.Despawn(lightning);
			yield break;
		}

		private tk2dTiledSprite extantLink;
		Vector2 badThingSpapiWillProbablyYellAtMeFor;
		Color lightningColour = new Color(1.066f, 0, 1.686f);
		GameObject LinkVFXPrefab;

	}
}
