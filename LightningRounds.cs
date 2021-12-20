using Dungeonator;
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
            string longDesc = "bullets have a chance to chain lightning to nearby enemies";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.A;

			item.PlaceItemInAmmonomiconAfterItemById(298);
			CustomLightning.Init();


		}

		protected override void Update()
		{
			PlayerController player = this.Owner;
			if (player && this.extantLink == null && LinkVFXPrefab != null)
			{
				tk2dTiledSprite component = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				this.extantLink = component;
			}
			else if (player && this.extantLink != null)
			{
				var badGuys = player.CurrentRoom?.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All);
				if (badGuys != null);
				foreach (AIActor enemy in badGuys)
				{
					if (!enemy.healthHaver.IsDead && enemy.GetComponent<NotReallyADebuff>() != null && startpos != null && extantLink != null)
					{
						UpdateLink(enemy, this.extantLink, startpos);
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
			startpos = obj.specRigidbody.UnitCenter;
			
			//this.Owner.CurrentRoom.ApplyActionToNearbyEnemies(hitRigidbody.UnitCenter, 5f, delegate (AIActor enemy, float dist)
			int limit = 5;
			int range = 10;
			//if (this.Owner.CurrentRoom != null)
			//{
				if (this.Owner.PlayerHasActiveSynergy("Full Circuit"))
				{
					limit = 8;
					range = 15;
				}
				List<AIActor> ignoreList = new List<AIActor>();
				ChainLightningToTarget(obj.specRigidbody.UnitCenter, 4, range, ignoreList, limit);
				/*ApplyActionToNearbyEnemiesWithALimit(obj.specRigidbody.UnitCenter, range, limit, this.Owner.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
				{

					if (enemy && enemy.healthHaver && !ignoreList.Contains(enemy))
					{

						var linkVFXPrefab = CustomLightning.lightningVFX;
						//var linkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);

						tk2dTiledSprite component = SpawnManager.SpawnVFX(linkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
						this.extantLink = component;

						UpdateLink(enemy, component, obj.specRigidbody.UnitCenter);
						enemy.gameObject.AddComponent<NotReallyADebuff>();
						enemy.healthHaver.ApplyDamage(4, Vector2.zero, string.Empty, CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
						ignoreList.Add(enemy);
						ETGMod.StartGlobalCoroutine(doTimerMagic(component.gameObject));
						if (enemy.specRigidbody)
                        {
							ApplyActionToNearbyEnemiesWithALimit(enemy.specRigidbody.UnitCenter, range / 2, limit / 2, this.Owner.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy2, float dist2)
							{

								if (enemy2 && enemy2.healthHaver && !ignoreList.Contains(enemy2))
								{
									var linkVFXPrefab2 = CustomLightning.lightningVFX;
									//var linkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);

									tk2dTiledSprite component2 = SpawnManager.SpawnVFX(linkVFXPrefab2, false).GetComponent<tk2dTiledSprite>();

									UpdateLink(enemy2, component2, enemy.specRigidbody.UnitCenter);
									enemy2.gameObject.AddComponent<NotReallyADebuff>();
									enemy2.healthHaver.ApplyDamage(2, Vector2.zero, string.Empty, CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
									StartCoroutine(doTimerMagic(component2.gameObject));

								}


							});
						}
					}
				});*/
			//}
		}

		void ChainLightningToTarget(Vector2 pos, float damage, float range, List<AIActor> ignoreList, int limit)
        {
			var room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(pos.ToIntVector2());
			if (damage <= 0.5f || range <= 0 || room == null)
            {
				return;
            }
			
			float distance = 0;
			var enemy = room.GetNearestEnemy(pos, out distance, ignoreList, true, true);
			if (distance <= range && enemy != null)
            {
				var linkVFXPrefab = CustomLightning.lightningVFX;//Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX also works

				tk2dTiledSprite component = SpawnManager.SpawnVFX(linkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				this.extantLink = component;
				ignoreList.Add(enemy);
				UpdateLink(enemy, component, pos);
				enemy.gameObject.AddComponent<NotReallyADebuff>();
				enemy.healthHaver.ApplyDamage(damage, Vector2.zero, string.Empty, CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);		

				
				StartCoroutine(doTimerMagic(component.gameObject, delay));
				

				ChainLightningToTarget((enemy.sprite != null ? enemy.sprite.WorldCenter : (Vector2)enemy.transform.position), damage, range, ignoreList, limit - 1);

				//enemy.RegisterOverrideColor(new Color32(132, 3, 252, 255), "test");
				//ETGModConsole.Log(pos.ToString());
			}
        }


		private void UpdateLink(AIActor target, tk2dTiledSprite m_extantLink, Vector2 landedPoint)
		{
			//SpeculativeRigidbody specRigidbody = target.specRigidbody;
			//SpeculativeRigidbody speculativeRigidbody = specRigidbody;
			//Material material = m_extantLink.GetComponent<Renderer>().material;
			//material.SetFloat("_BlackBullet", 0.995f);
			//material.SetFloat("_EmissiveColorPower", 4.9f);

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


		private static IEnumerator doTimerMagic(GameObject lightning, float delay)
		{
			yield return new WaitForSeconds(delay);

			
			SpawnManager.Despawn(lightning);
			yield break;
		}
		public float delay = 0.25f;
		private tk2dTiledSprite extantLink;
		Vector2 startpos;
		Color lightningColour = new Color(1.066f, 0, 1.686f);
		GameObject LinkVFXPrefab;

	}
}
