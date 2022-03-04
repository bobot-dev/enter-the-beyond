using Gungeon;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class GunParasite : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Gun Parasite", "gun_parasite");
			Game.Items.Rename("outdated_gun_mods:gun_parasite", "bot:gun_parasite");
			var a = gun.gameObject.AddComponent<GunParasite>();
			gun.SetShortDescription("");
			gun.SetLongDescription("");
			GunExt.SetupSprite(gun, null, "gun_parasite_idle_001", 8);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 100;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			targetDamage = projectile.baseData.damage;
			targetSpeed = projectile.baseData.speed;
			targetRange = projectile.baseData.range;

		}

		enum StatToSteal
        {
			reload = 0,
			damage = 1,
			rateOfFire = 2,
			spread = 3,
			range = 4,
			projSpeed = 5,
			magSize = 6,
        }

		void StealGunStat(Gun targetGun, StatToSteal statToSteal)
        {
			switch(statToSteal)
            {
				case StatToSteal.rateOfFire:
					this.gun.DefaultModule.cooldownTime = targetGun.DefaultModule.cooldownTime;
					this.gun.DefaultModule.shootStyle = targetGun.DefaultModule.shootStyle;
					break;
				case StatToSteal.spread:
					this.gun.DefaultModule.angleVariance = targetGun.DefaultModule.angleVariance;
					break;
				case StatToSteal.magSize:
					this.gun.DefaultModule.numberOfShotsInClip = targetGun.DefaultModule.numberOfShotsInClip;
					this.gun.DefaultModule.customAmmoType = targetGun.DefaultModule.customAmmoType;
					this.gun.DefaultModule.ammoType = targetGun.DefaultModule.ammoType;
					break;
				case StatToSteal.reload:
					this.gun.reloadTime = targetGun.reloadTime;
					break;
				case StatToSteal.damage:
					targetDamage = targetGun.DefaultModule.projectiles[0].baseData.damage;
					break;
				case StatToSteal.projSpeed:
					targetSpeed = targetGun.DefaultModule.projectiles[0].baseData.speed;
					break;
				case StatToSteal.range:
					targetRange = targetGun.DefaultModule.projectiles[0].baseData.range;
					break;
			}
			GameUIRoot.Instance.notificationController.DoCustomNotification("Gun Consumed", $"{statToSteal} has been absorbed", gun.sprite.Collection, gun.sprite.spriteId, UINotificationController.NotificationColor.PURPLE);

		}

		static float targetDamage;
		static float targetSpeed;
		static float targetRange;

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);


			projectile.baseData.damage = targetDamage;
			projectile.baseData.speed = targetSpeed;
			projectile.baseData.range = targetRange;
			projectile.UpdateSpeed();
		}

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
			float distance = 0f;
			var target = GetNearestGun(player.sprite.WorldCenter, out distance);
			if (distance <= 4)
            {
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleGunSuck(target));
				StealGunStat(target, (StatToSteal)UnityEngine.Random.Range(0, 7));
				//StealGunStat(target, StatToSteal.rateOfFire);
				UnityEngine.Object.Destroy(target.transform.parent.gameObject);
				AkSoundEngine.PostEvent("Play_CHR_muncher_eat_01", this.gameObject);
			}

			base.OnReloadPressed(player, gun, bSOMETHING);
        }

		private IEnumerator HandleGunSuck(Gun target)
		{
			Transform copySprite = this.CreateEmptySprite(target);
			Vector3 startPosition = copySprite.transform.position;
			float elapsed = 0f;
			float duration = 0.5f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				if (this.gun && copySprite)
				{
					Vector3 position = this.gun.barrelOffset.position;
					float t = elapsed / duration * (elapsed / duration);
					copySprite.position = Vector3.Lerp(startPosition, position, t);
					copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
					copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
				}
				yield return null;
			}
			if (copySprite)
			{
				UnityEngine.Object.Destroy(copySprite.gameObject);
			}
			yield break;
		}

		public Gun GetNearestGun(Vector2 position, out float nearestDistance)
		{
			DebrisObject result = null;
			nearestDistance = float.MaxValue;
			if (StaticReferenceManager.AllDebris == null)
			{
				return null;
			}
			foreach (var debris in StaticReferenceManager.AllDebris)
			{
				if (debris && debris.GetComponentInChildren<Gun>())
                {
					float num = Vector2.Distance(position, debris.sprite.WorldCenter);
					if (num < nearestDistance)
					{
						nearestDistance = num;
						result = debris;
					}
				}
			}
			return result.GetComponentInChildren<Gun>();
		}

		private Transform CreateEmptySprite(Gun target)
		{
			GameObject gameObject = new GameObject("suck image");
			gameObject.layer = target.gameObject.layer;
			tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
			gameObject.transform.parent = SpawnManager.Instance.VFX;
			tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
			tk2dSprite.transform.position = target.sprite.transform.position;
			GameObject gameObject2 = new GameObject("image parent");
			gameObject2.transform.position = tk2dSprite.WorldCenter;
			tk2dSprite.transform.parent = gameObject2.transform;
			return gameObject2.transform;
		}
	}
}
