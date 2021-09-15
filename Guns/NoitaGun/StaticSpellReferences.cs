using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    static class StaticSpellReferences
    {

        public static Projectile blankProjectile;



        public static GenericLootTable spellLootTable;
        public static Dictionary<string, Spell> validSpells = new Dictionary<string, Spell>();
        public static Dictionary<string, int> JUSTFUCKINGWORK = new Dictionary<string, int>();




        public static void Init()
        {

            spellLootTable = LootTableAPI.LootTableTools.CreateLootTable();

            emptySlot = new Spell
            {
                name = "None",
                isEmptySlot = true,

            };

            sparkBolt = new Spell
            {
                name = "Spark Bolt",
                isCombatSpell = true,
            };

            arrow = new Spell
            {
                name = "Arrow",
                isCombatSpell = true,
            };



            bouncingBullet = new Spell
            {
                name = "Bouncing Bullet",
                impactOnFireRate = -0.1f,
                isCombatSpell = true,
            };


            lessCoolDown = new Spell
            {
                name = "Reduced Cooldown",
                impactOnFireRate = 0.3f,
                impactOnReload = -0.7f,
                isCombatSpell = false,
            };
            homing = new Spell
            {
                name = "Homing",
                isCombatSpell = false,
                addsComponents = true,
                componentsToAdd = new List<MonoBehaviour>
                {
                    new HomingModifier
                    {
                        HomingRadius = 20,
                        AngularVelocity = 180,
                        
                    }
                }
            };
            reducdedSpread = new Spell
            {
                name = "Reduced Spread",
                impactOnSpread = -20,
                isCombatSpell = false,
            };
            increaseRange = new Spell
            {
                name = "More Range",
                impactOnRange = 10,
                isCombatSpell = false,
            };
            increaseSpeed = new Spell
            {
                name = "Faster Bullets",
                impactOnSpeed = 2,
                isCombatSpell = false,
            };


            Projectile bouncingBulletProj = Tools.SetupProjectile(345);

            bouncingBulletProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 4,
                force = 2,
                speed = 50,
                range = 300,
            };

            var boucy = bouncingBulletProj.gameObject.AddComponent<BounceProjModifier>();
            boucy.numberOfBounces = 10;

            bouncingBullet.spellProj = bouncingBulletProj;


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

            sparkBolt.spellProj = sparkBoltProj;


            Projectile arrowProj = Tools.SetupProjectile(12);

            arrowProj.baseData = new ProjectileData
            {
                UsesCustomAccelerationCurve = false,
                damage = 10,
                force = 20,
                speed = 20,
                range = 50,
            };

            arrow.spellProj = arrowProj;



        }

        public static Spell emptySlot;

        public static Spell sparkBolt;

        public static Spell arrow;



        public static Spell bouncingBullet;


        public static Spell lessCoolDown;
        public static Spell homing;
        public static Spell reducdedSpread;
        public static Spell increaseRange;
        public static Spell increaseSpeed;


    }
}
