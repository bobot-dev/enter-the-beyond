using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dungeonator;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using LiveRecolor;
using MonoMod.RuntimeDetour;
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
			gun.SetLongDescription("Hold [Interact] to toggle the menu and customize the wand.\n\nA very dangerus artifact from some world far from here");

			gun.SetupSprite(null, "beyond_wand_idle_001", 12);
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
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.AdditionalShotBounces,
					amount = 0,
					modifyType = StatModifier.ModifyMethod.ADDITIVE,
					isMeatBunBuff = false
				},
				new StatModifier
				{
					statToBoost = PlayerStats.StatType.AdditionalShotPiercing,
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
			/*foreach(var id in new List<int> { 512, 86, 394, 670, 198, 153, 376, 129 })
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
			}*/

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

			//projectile.transform.parent = gun.barrelOffset;
			

			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 16f;
			projectile.baseData.force = 10f;
			projectile.baseData.range = 16f;



			ETGMod.Databases.Items.Add(gun, null, "ANY");

			BotsItemIds.Wand = gun.PickupObjectId;

			//BotsModule.Log($"{BotsItemIds.Wand}, {gun.PickupObjectId}");

			

			//GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.arrow));
			//GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.bouncingBullet));
			var sbnum = GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.sparkBolt));
			var hnym = GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.emptySlot));
			//GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.increaseRange));
			//GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.increaseSpeed));
			//var lcdnum = GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.lessCoolDown));
			//GainNewSpell(StaticSpellReferences.GetSpellByType(SpellTypes.reducdedSpread));

			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[sbnum]);
			wand.AddSpellToWand(gun, avalableSpells[hnym]);
			wand.AddSpellToWand(gun, avalableSpells[hnym]);

			wand.ChangeWandProperties(gun);
			Tools.BeyondItems.Add(gun.PickupObjectId);

			Hook hook = new Hook(typeof(PlayerController).GetMethod("orig_Start", BindingFlags.Public | BindingFlags.Instance), typeof(Wand).GetMethod("SetupWandUi"));

		}
		public static void SetupWandUi(Action<PlayerController> action, PlayerController player)
		{
			action(player);
			GUIhandler handler = player.gameObject.AddComponent<GUIhandler>();
			handler.enabled = false;
		}

		void Update()
        {
			if (this.gun.CurrentOwner && !this.gun.CurrentOwner.gameObject.GetComponent<GUIhandler>().enabled)
			{
				this.gun.CurrentOwner.gameObject.GetComponent<GUIhandler>().m_player = this.gun.CurrentOwner as PlayerController;
				this.gun.CurrentOwner.gameObject.GetComponent<GUIhandler>().enabled = true;
			}

		}

		public override void OnInitializedWithOwner(GameActor actor)
		{
			ETGModConsole.Log("been grabbed");

			base.OnInitializedWithOwner(actor);
			
			actor.gameObject.GetComponent<GUIhandler>().m_player = actor as PlayerController;
			actor.gameObject.GetComponent<GUIhandler>().enabled = true;
			
		}

		private void PostProcessSpell(Projectile projectile)
		{

		}

        public override void OnPostFired(PlayerController player, Gun gun)
        {
			//foreach (var spell in Wand.spells)
			//{
			//	if (spell.useMaxUses && spell.maxUses > 0)
			//	{
			//		spell.maxUses--;
			//	}
			//	if (spell.useMaxUses && spell.maxUses < 0)
			//	{
			//		
			//		spells[spells.IndexOf(spell)] = spells.Where(sp => sp.type == SpellTypes.emptySlot).ToList()[0];
			//	}
			//	Wand.avalableSpells.Remove(spell);
			//	ChangeWandProperties(this.gun);
			//}
			base.OnPostFired(player, gun);
        }


        public override void PostProcessProjectile(Projectile projectile)
        {
			foreach (var spell in Wand.spells)
			{
				if (spell.addsComponents == true)
				{
					//BotsModule.Log($"[{spell.name}]: homing {spell.homingModifier == null} - bouncing {spell.bounceProjModifier == null} - piercing {spell.pierceProjModifier == null}");
					if (spell.homingModifier != null)
                    {

						HomingModifier homingModifier = projectile.gameObject.GetComponent<HomingModifier>();
						if (homingModifier == null)
						{
							homingModifier = projectile.gameObject.AddComponent<HomingModifier>();
							homingModifier.HomingRadius = 0f;
							homingModifier.AngularVelocity = 0f;
						}
						homingModifier.HomingRadius += spell.homingModifier.HomingRadius;
						homingModifier.AngularVelocity += spell.homingModifier.AngularVelocity;

						//projectile.gameObject.AddComponent<HomingModifier>(spell.homingModifier);
						//BotsModule.Log("added homing");
					}
					

					//if (spell.bounceProjModifier != null)
					//{
					//	projectile.gameObject.AddComponent<BounceProjModifier>(spell.bounceProjModifier);
					//	BotsModule.Log("added bouncing");
					//}

					//if (spell.pierceProjModifier != null)
					//{
					//	projectile.gameObject.AddComponent<PierceProjModifier>(spell.pierceProjModifier);
					//	BotsModule.Log("added piercing");
					//}


				}

			}
		}


		public void ChangeWandProperties(Gun wand)
		{
			float Spread = 0;
			float FireRate = 0;
			float Reload = 0;
			float Speed = 0;
			float Range = 0;
			float Bounce = 0;
			float Pierce = 0;

			wand.DefaultModule.projectiles.Clear();
			spellSlots = Wand.spells.Count;

			int i = 0;
			int u = 0;

			ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();

			projectileVolleyData.InitializeFrom(wand.Volley);

			foreach (var spell in Wand.spells)
			{
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
				Bounce += spell.impactOnBounces;
				Pierce += spell.impactOnPiercing;
				u++;
				
			}


			//wand.modifiedVolley 
			projectileVolleyData.projectiles[0].burstShotCount = projectileVolleyData.projectiles[0].projectiles.Count;
			wand.Volley = projectileVolleyData;

			
			projectileVolleyData.projectiles[0].numberOfShotsInClip = i;
			projectileVolleyData.projectiles[0].burstShotCount = i;

			wand.Volley.projectiles[0].ResetRuntimeData();

			wand.currentGunStatModifiers[0].amount = Spread;
			wand.currentGunStatModifiers[1].amount = FireRate;
			wand.currentGunStatModifiers[2].amount = Reload;
			wand.currentGunStatModifiers[3].amount = Speed;
			wand.currentGunStatModifiers[4].amount = Range;
			wand.currentGunStatModifiers[5].amount = Bounce;
			wand.currentGunStatModifiers[6].amount = Pierce;
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

	[Serializable]
	class Spell
	{
		public Projectile spellProj;

		public string name;

		public int index;
		public int itemId;

		public int maxUses;

		public bool isCombatSpell;
		public bool isEmptySlot;
		public bool isLockedSlot;
		public bool addsComponents;
		public bool isChainLightning;
		public bool useMaxUses;

		public float impactOnFireRate;
		public float impactOnReload;
		public float impactOnSpread;
		public float impactOnSpeed;
		public float impactOnRange;

		public float impactOnBounces;
		public float impactOnPiercing;

		public float weight = 100;

		//public List<MonoBehaviour> componentsToAdd;

		public SpellTypes type;

		[SerializeField]
		public BounceProjModifier bounceProjModifier;
		[SerializeField]
		public HomingModifier homingModifier;
		[SerializeField]
		public PierceProjModifier pierceProjModifier;

		public Spell Clone()
		{
			var newSpell = new Spell
			{
				impactOnFireRate = this.impactOnFireRate,
				impactOnRange = this.impactOnRange,
				impactOnReload = this.impactOnReload,
				impactOnSpeed = this.impactOnSpeed,
				impactOnSpread = this.impactOnSpread,
				impactOnBounces = this.impactOnBounces,
				impactOnPiercing = this.impactOnPiercing,
				isCombatSpell = this.isCombatSpell,
				isEmptySlot = this.isEmptySlot,
				isLockedSlot = this.isLockedSlot,
				name = this.name,
				spellProj = this.spellProj,
				addsComponents = this.addsComponents,
				type = this.type,
				weight = this.weight,
				homingModifier = this.homingModifier,
				pierceProjModifier = this.pierceProjModifier,
				bounceProjModifier = this.bounceProjModifier,
				maxUses = this.maxUses,
				useMaxUses = this.useMaxUses,
			};



			return newSpell;
		}

	}
}
