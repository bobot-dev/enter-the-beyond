using Gungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	//The bearer of bad news
	public class TheMessanger : GunBehaviour
	{
		private bool HasReloaded;

		public FirstSlotPerks firstPerk;
		public SecondSlotPerks secondPerk;
		

		public static void Add()
		{

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("The Messenger", "messanger");
			Game.Items.Rename("outdated_gun_mods:the_messenger", "bot:the_messenger");
			var pewpewgun =  gun.gameObject.AddComponent<TheMessanger>();
			gun.SetShortDescription("The bearer of bad news");
			gun.SetLongDescription("");

			gun.SetupSprite(null, "messanger_idle_001", 8);

			//gun.SetAnimationFPS(gun.shootAnimation, 12);
			//gun.SetAnimationFPS(gun.reloadAnimation, 10);

			pewpewgun.firstPerk = FirstSlotPerks.Outlaw;
			pewpewgun.secondPerk = SecondSlotPerks.Desperado;

			Gun other = PickupObjectDatabase.GetById(15) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(300);

			

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.burstCooldownTime = 0.09f;
			gun.DefaultModule.burstShotCount = 3;
			gun.StarterGunForAchievement = true;


			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;


			

			gun.finalMuzzleFlashEffects = other.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.4f;
			gun.DefaultModule.numberOfShotsInClip = 30;
			gun.quality = PickupObject.ItemQuality.B;
			gun.gunClass = GunClass.RIFLE;
			gun.CanBeDropped = true;

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);

			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			gun.DefaultModule.projectiles[0] = projectile;

			gun.shellsToLaunchOnReload = gun.DefaultModule.numberOfShotsInClip;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			//projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;
			projectile.baseData.damage = 6f;
			projectile.baseData.speed = 23f;
			projectile.baseData.force = 15f;
			projectile.baseData.range = 20f;




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
					//return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", new Color32(238, 158, 0, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 0);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			gun.currentGunStatModifiers = new StatModifier[]
			{
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.Damage,
					amount = 1,
					modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.RateOfFire,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.ReloadSpeed,
					amount = 1,
					modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.MovementSpeed,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
			};

			

			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}
		void OnHit(Projectile proj, SpeculativeRigidbody target, bool fatal)
        {
			if (fatal)
            {
				if (firstPerk == FirstSlotPerks.Outlaw)
                {
					SetBuff("reload");

				}
				if (secondPerk == SecondSlotPerks.Desperado)
				{
					SetBuff("firerate");
				}
				if (secondPerk == SecondSlotPerks.KillClip)
				{
					SetBuff("damage");

				}
				
			}
			
        }

		IEnumerator SetBuff(string type, int time = 3)
		{

			if (type == "reload")
            {
				needsReloadBuff = true;
				yield return new WaitForSeconds(time);
				needsReloadBuff = false;
			}

			if (type == "firerate")
			{
				AkSoundEngine.PostEvent("Play_ENM_hammer_target_01", base.gameObject);
				needsFirerateBuff = true;
				yield return new WaitForSeconds(time);
				needsFirerateBuff = false;
			}

			if (type == "damage")
			{
				needsDamageBuff = true;
				yield return new WaitForSeconds(time);
				needsDamageBuff = false;
			}

			


			yield return null;
		}

		IEnumerator ClearBuff(int time, StatModifier buff, float amount)
        {

			buff.amount += amount;
			(gun.CurrentOwner as PlayerController).stats.RecalculateStats(gun.CurrentOwner as PlayerController);

			yield return new WaitForSeconds(time);

			buff.amount -= amount;
			(gun.CurrentOwner as PlayerController).stats.RecalculateStats(gun.CurrentOwner as PlayerController);

			yield return null;
        }

		bool needsReloadBuff;
		bool needsFirerateBuff;
		bool needsDamageBuff;
	

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{




			bool flag = !gun.IsReloading;// && this.HasReloaded;
			if (flag)
			{
				//this.HasReloaded = false;
				//AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				if (needsReloadBuff)
				{
					StartCoroutine(ClearBuff(5, gun.currentGunStatModifiers[2], -0.5f));
					needsReloadBuff = false;
				}

				if (needsFirerateBuff)
				{
					AkSoundEngine.PostEvent("Play_ENM_hammer_target_01", base.gameObject);
					StartCoroutine(ClearBuff(5, gun.currentGunStatModifiers[1], 1.2f));
					needsFirerateBuff = false;
				}

				if (needsDamageBuff)
				{
					StartCoroutine(ClearBuff(5, gun.currentGunStatModifiers[0], 1.6f));
					needsDamageBuff = false;
				}
				
				//AkSoundEngine.PostEvent("Play_OBJ_blackhole_close_01", base.gameObject);

				

			}
			base.OnReloadPressed(player, gun, bSOMETHING);
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OnHitEnemy += OnHit;
			base.PostProcessProjectile(projectile);

		}

		private void ProcessGunShader(Gun g, int num)
		{
			var m_glintShader = EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material;
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].GetFloat("_EmissivePower") == num * 20)
				{
					//return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(m_glintShader);

			material.SetColor("_EmissiveColor", new Color32(238, 158, 0, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", num * 20);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
		}
		int tokenCount = 0;
		protected void Update()
		{
			if (firstPerk == FirstSlotPerks.MovingTarget)
			{
				if (gun.currentGunStatModifiers[3].amount != 2)
				{
					gun.currentGunStatModifiers[3].amount = 2;
					(gun.CurrentOwner as PlayerController).stats.RecalculateStats(gun.CurrentOwner as PlayerController);
				}
			}
			


			int tokens = 0;
			foreach (var item in (gun.CurrentOwner as PlayerController).passiveItems)
            {
				
				if (item is BasicStatPickup && (item as BasicStatPickup).IsMasteryToken)
                {
					tokens++;
				}
			}
			if (tokenCount != tokens)
			{
				tokenCount = tokens;
				ProcessGunShader(gun, tokenCount);
			}


			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				bool flag2 = !this.gun.PreventNormalFireAudio;
				if (flag2)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag3)
				{
					this.HasReloaded = true;
				}
			}


			/*MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;

			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", new Color32(238, 158, 0, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", (gun.CurrentOwner as PlayerController).MasteryTokensCollectedThisRun * 10);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;*/


		}
		




		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}


		public enum FirstSlotPerks
        {
			MovingTarget,
			Outlaw,
        }

		public enum SecondSlotPerks
        {
			OneForAll,
			KillClip,
			Desperado,
			Frenzy
        }

	}
}
