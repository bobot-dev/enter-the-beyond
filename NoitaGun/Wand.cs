using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using LiveRecolor;
using MultiplayerBasicExample;
using UnityEngine;


namespace BotsMod
{
    class Wand : GunBehaviour
    {
		public static List<Spell> avalableSpells = new List<Spell>();
		public static List<Spell> spells = new List<Spell>();
		public int spellSlots = 3;


		public static void Add()
		{

			Gun gun2 = PickupObjectDatabase.GetById(221) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(99) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(57) as Gun;
			Gun gun = ETGMod.Databases.Items.NewGun("Wand", "beyond_wand");
			Game.Items.Rename("outdated_gun_mods:wand", "bot:wand");
			var wand = gun.gameObject.AddComponent<Wand>();
			gun.SetShortDescription("Woooo magic!");
			gun.SetLongDescription("A very dangerus artifact from some world far from here");

			gun.SetupSprite(null, "beyond_wand_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);


			Gun other = PickupObjectDatabase.GetById(810) as Gun;
			gun.AddProjectileModuleFrom(other, true, false);
			gun.SetBaseMaxAmmo(27616);
			gun.InfiniteAmmo = true;

			

			//gun.barrelOffset.transform.localPosition += new Vector3(10f, 0f, 0f);

			gun.DefaultModule.ammoCost = 1;

			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;


			gun.currentGunStatModifiers = new StatModifier[]
			{
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.Accuracy,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
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
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.ProjectileSpeed,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.RangeMultiplier,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
			};

			//gun.damageModifier = 1;
			gun.reloadTime = 1.3f;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);

			Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
			projectile2.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile2);

			

			int i = 0;
			foreach(var id in new List<int> { 512, 86, 394, 670, 198, 153, 376, 129 })
			{
				continue;
				Projectile projectile3 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(id) as Gun).DefaultModule.projectiles[0]);
				projectile3.gameObject.SetActive(false);
				ItemAPI.FakePrefab.MarkAsFakePrefab(projectile3.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile3);

				avalableSpells.Add(new Spell
				{
					name = projectile3.name,
					spellProj = projectile3
				});
				i++;
			}

			gun.DefaultModule.projectiles = new List<Projectile>
			{
				projectile,
			};

		

			gun.DefaultModule.burstShotCount = 3;
			gun.DefaultModule.burstCooldownTime = 0.3f;


			Gun gun5 = PickupObjectDatabase.GetById(37) as Gun;
			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = 0.15f;


			gun.DefaultModule.numberOfShotsInClip = -1;


			gun.quality = PickupObject.ItemQuality.S;
			Guid.NewGuid().ToString();
			gun.gunClass = GunClass.FULLAUTO;
			gun.CanBeDropped = true;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;

			//gun.DefaultModule.projectiles[0] = projectile;

			projectile.transform.parent = gun.barrelOffset;
			

			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;



			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.Wand = gun.PickupObjectId;

			BotsModule.Log($"{BotsItemIds.Wand}, {gun.PickupObjectId}");

			StaticSpellReferences.Init();

			GainNewSpell(StaticSpellReferences.arrow);
			GainNewSpell(StaticSpellReferences.bouncingBullet);
			var sbnum = GainNewSpell(StaticSpellReferences.sparkBolt);
			var hnym = GainNewSpell(StaticSpellReferences.homing);
			GainNewSpell(StaticSpellReferences.increaseRange);
			GainNewSpell(StaticSpellReferences.increaseSpeed);
			var lcdnum = GainNewSpell(StaticSpellReferences.lessCoolDown);
			GainNewSpell(StaticSpellReferences.reducdedSpread);

			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[lcdnum]);
			wand.AddSpellToWand(gun, avalableSpells[lcdnum]);

			wand.ChangeWandProperties(gun);
		}

		public override void OnInitializedWithOwner(GameActor actor)
		{


			//this.gameObject.GetComponent<GUIhandler>().m_player = actor as PlayerController;
			base.OnInitializedWithOwner(actor);
		}

		public void ChangeWandProperties(Gun wand)
		{
			float Spread = 0;
			float FireRate = 0;
			float Reload = 0;
			float Speed = 0;
			float Range = 0;

			wand.DefaultModule.projectiles.Clear();
			spellSlots = Wand.spells.Count;

			int i = 0;
			int u = 0;

			ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();

			projectileVolleyData.InitializeFrom(wand.Volley);

			foreach (var spell in Wand.spells)
			{
				//BotsModule.Log($"{spell.name}: {u} {i}");
				if (spell.spellProj != null)
				{
					i++;
					projectileVolleyData.projectiles[0].projectiles.Add(spell.spellProj);
				}
				Spread += spell.impactOnSpread;
				FireRate += spell.impactOnFireRate;
				Reload += spell.impactOnReload;
				Speed += spell.impactOnSpeed;
				Range += spell.impactOnRange;
				u++;

			}
			//wand.modifiedVolley 

			wand.Volley = projectileVolleyData;

			wand.DefaultModule.numberOfShotsInClip = i;
			wand.DefaultModule.burstShotCount = i;

			wand.currentGunStatModifiers[0].amount = Spread;
			wand.currentGunStatModifiers[1].amount = FireRate;
			wand.currentGunStatModifiers[2].amount = Reload;
			wand.currentGunStatModifiers[3].amount = Speed;
			wand.currentGunStatModifiers[4].amount = Range;
			if (wand.CurrentOwner != null && wand.CurrentOwner is PlayerController)
			{
				(wand.CurrentOwner as PlayerController).stats.RecalculateStats(wand.CurrentOwner as PlayerController);
			}

		}

		public void AddSpellToWand (Gun wand, Spell spell, bool updateWand = true)
		{
			if (wand.gameObject.GetComponent<Wand>() == null)
			{
				BotsModule.Log($"[AddSpellToWand]: \"Wand\" component has nulled");
			}

			Wand.spells.Add(spell);
			
			if (updateWand)
			{
				ChangeWandProperties(wand);
			}
			
		}

		public static int GainNewSpell(Spell spell)
		{
			var spellToAdd = spell.Clone();
			spellToAdd.index = Wand.avalableSpells.Count;
			avalableSpells.Add(spellToAdd);
			return avalableSpells.Count - 1;
		}

		public void AddSpellSlots(int slots)
		{
			for(int i = 0; i < slots; i++)
			{
				this.spellSlots++;
			}
		}

		ProjectileVolleyData volly;

	}


	class Spell
	{
		public Projectile spellProj;

		public string name;

		public int index;

		public bool isCombatSpell;
		public bool isEmptySlot;
		public bool isLockedSlot;

		public float impactOnFireRate;
		public float impactOnReload;
		public float impactOnSpread;
		public float impactOnSpeed;
		public float impactOnRange;


		public Spell Clone()
		{
			var newSpell = new Spell
			{
				impactOnFireRate = this.impactOnFireRate,
				impactOnRange = this.impactOnRange,
				impactOnReload = this.impactOnReload,
				impactOnSpeed = this.impactOnSpeed,
				impactOnSpread = this.impactOnSpread,
				isCombatSpell = this.isCombatSpell,
				isEmptySlot = this.isEmptySlot,
				isLockedSlot = this.isLockedSlot,
				name = this.name,
				spellProj = this.spellProj,
			};



			return newSpell;
		}

	}
}
