using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static PickupObject;

namespace BotsMod
{
    static class StaticSpellReferences
    {

        public static Projectile blankProjectile;



        public static GenericLootTable spellLootTable;
        public static Dictionary<SpellTypes, Spell> validSpells = new Dictionary<SpellTypes, Spell>();
        public static Dictionary<string, int> JUSTFUCKINGWORK = new Dictionary<string, int>();




        public static void Init()
        {


            Projectile bouncingBulletProj = Tools.SetupProjectile(99);

            bouncingBulletProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 5,
                force = 2,
                speed = 50,
                range = 300,
            };
            bouncingBulletProj.sprite.renderer.enabled = false;
            var boucy = bouncingBulletProj.gameObject.AddComponent<BounceProjModifier>();

            
            boucy.numberOfBounces = 10;

            var ok = UnityEngine.Object.Instantiate(BeyondPrefabs.AHHH.LoadAsset<GameObject>("VFX_Bouncing_Bolt"), bouncingBulletProj.transform);
            ok.transform.localPosition = Vector3.zero;
            Projectile sparkBoltProj = Tools.SetupProjectile(61);

            sparkBoltProj.CanTransmogrify = false;
            sparkBoltProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 6,
                force = 5,
                speed = 20,
                range = 25,
            };

            Projectile arrowProj = Tools.SetupProjectile(12);

            arrowProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 10,
                force = 20,
                speed = 20,
                range = 50,
            };

            spellLootTable = LootTableAPI.LootTableTools.CreateLootTable();

            GameObject homing1 = PrefabAPI.PrefabBuilder.BuildObject("homingmoduledummy1");

            var homingMod1 = homing1.AddComponent<HomingModifier>();

            homingMod1.HomingRadius = 20;
            homingMod1.AngularVelocity = 180;

            Projectile fireballProj = Tools.SetupProjectile(125);

            fireballProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 5,
                force = 20,
                speed = 28,
                range = 75,
            };

            var boom = fireballProj.gameObject.AddComponent<ExplosiveModifier>();
            UnityEngine.Object.Destroy(fireballProj.gameObject.GetComponent<PierceProjModifier>());
            boom.doDistortionWave = false;
            boom.doExplosion = true;
            boom.IgnoreQueues = true;
            boom.explosionData = new ExplosionData
            {
                breakSecretWalls = false,
                comprehensiveDelay = 0,
                damage = 15,
                damageRadius = 4.5f,
                damageToPlayer = 0.5f,
                debrisForce = 50,
                doDamage = true,
                doDestroyProjectiles = false,
                forcePreventSecretWallDamage = true,
                doForce = true,
                doScreenShake = false,
                doExplosionRing = true,
                doStickyFriction = false,
                effect = null,
                explosionDelay = 0,
                force = 100,
                forceUseThisRadius = false,
                freezeEffect = null,
                freezeRadius = 0,
                ignoreList = new List<SpeculativeRigidbody>(),
                IsChandelierExplosion = false,
                isFreezeExplosion = false,
                overrideRangeIndicatorEffect = null,
                playDefaultSFX = true,
                preventPlayerForce = false,
                pushRadius = 6,
                rotateEffectToNormal = false,
                secretWallsRadius = 0,
                ss = null,
                useDefaultExplosion = false,
                usesComprehensiveDelay = false,
            };

            validSpells.Add(SpellTypes.emptySlot, new Spell { name = "None", isEmptySlot = true });
            validSpells.Add(SpellTypes.sparkBolt, new Spell { name = "Spark Bolt", isCombatSpell = true, spellProj = sparkBoltProj });
            validSpells.Add(SpellTypes.arrow, new Spell { name = "Arrow", isCombatSpell = true, spellProj = arrowProj });
            validSpells.Add(SpellTypes.bouncingBullet, new Spell { name = "Bouncing Bullet", impactOnFireRate = -0.1f, isCombatSpell = true, spellProj = bouncingBulletProj, weight = 80f });

            validSpells.Add(SpellTypes.flamethrower, new Spell { name = "Fireball", isCombatSpell = true, spellProj = fireballProj, weight = 70f });

            validSpells.Add(SpellTypes.bounce, new Spell { name = "Bouncy", impactOnPiercing = 2, isCombatSpell = false });;
            validSpells.Add(SpellTypes.homing, new Spell { name = "Homing", addsComponents = true, isCombatSpell = false, homingModifier = homingMod1, weight = 70f });

            validSpells.Add(SpellTypes.lessCoolDown, new Spell { name = "Reduced Cooldown", impactOnFireRate = 0.3f, impactOnReload = -0.7f, isCombatSpell = false });
            validSpells.Add(SpellTypes.reducdedSpread, new Spell { name = "Reduced Spread", impactOnSpread = -20, isCombatSpell = false });
            validSpells.Add(SpellTypes.increaseRange, new Spell { name = "More Range", impactOnRange = 10, isCombatSpell = false });
            validSpells.Add(SpellTypes.increaseSpeed, new Spell { name = "Faster Bullets", impactOnSpeed = 2, isCombatSpell = false });

            validSpells.Add(SpellTypes.piercing, new Spell { name = "Piercing", impactOnPiercing = 2, isCombatSpell = false, weight = 70f });

            //validSpells.Add(SpellTypes.chainLightning, new Spell { name = "Chain Lightning", isChainLightning = true, isCombatSpell = false, weight = 0.5f });
            //validSpells.Add(SpellTypes.arrow, new Spell { name = "Arrow", isCombatSpell = false });

            foreach (var spell in validSpells)
            {
                if (spell.Key == SpellTypes.emptySlot)
                {
                    continue;
                }

                spell.Value.type = spell.Key;
                spellLootTable.AddItemToPool(SetupSpellItem(spell.Value), spell.Value.weight);
            }




        }

        public static SpellPickupObject SetupSpellItem(Spell spell)
        {
            string resourceName = $"BotsMod/sprites/Spells/new/spellicon{(int)spell.type}.png";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<SpellPickupObject>();
            ItemBuilder.AddSpriteToObject(spell.name, resourceName, obj);

            ItemBuilder.SetupItem(item, spell.type.ToString(), "page is a major wip proper info will be here soonTM", "bot_spell");
            item.quality = ItemQuality.EXCLUDED;
            

            item.spellToGive = spell;
            item.type = spell.type;
            item.minimapIcon = SpriteBuilder.SpriteFromResource("BotsMod/sprites/SpellMiniMapIcon");

            UnityEngine.Object.DontDestroyOnLoad(item.minimapIcon);
            FakePrefab.MarkAsFakePrefab(item.minimapIcon);

            Tools.Spells.Add(item.PickupObjectId);
            return item;
        }


        public static Spell GetSpellByType(SpellTypes type)
        {
            return validSpells[type];
        }

    }
    public enum SpellTypes
    {
        emptySlot,
        sparkBolt,
        arrow,
        bouncingBullet,
        flamethrower,
        bounce,
        lessCoolDown,
        homing,
        reducdedSpread,
        increaseRange,
        increaseSpeed,
        piercing,
        chainLightning,
    }
}
