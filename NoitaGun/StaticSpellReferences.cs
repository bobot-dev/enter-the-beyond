using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    static class StaticSpellReferences
    {

        public static Projectile blankProjectile;

        public static void Init()
        {
            Projectile bouncingBulletProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(345) as Gun).DefaultModule.projectiles[0]);
            bouncingBulletProj.gameObject.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(bouncingBulletProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(bouncingBulletProj);

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


            Projectile sparkBoltProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(61) as Gun).DefaultModule.projectiles[0]);
            sparkBoltProj.gameObject.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(sparkBoltProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(sparkBoltProj);

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


            Projectile arrowProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(12) as Gun).DefaultModule.projectiles[0]);
            arrowProj.gameObject.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(arrowProj.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(arrowProj);

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

        public static Spell emptySlot = new Spell
        {
            name = "None",
            isEmptySlot = true,
            
        };

        public static Spell sparkBolt = new Spell
        {
            name = "Spark Bolt",
            isCombatSpell = true,
        };

        public static Spell arrow = new Spell
        {
            name = "Arrow",
            isCombatSpell = true,
        };



        public static Spell bouncingBullet = new Spell
        {
            name = "Bouncing Bullet",            
            impactOnFireRate = -0.1f,
            isCombatSpell = true,
        };


        public static Spell lessCoolDown = new Spell
        {
            name = "Reduced Cooldown",
            impactOnFireRate = 0.3f,
            impactOnReload = -0.7f,
            isCombatSpell = false,
        };
        public static Spell homing = new Spell
        {
            name = "Homing",
            isCombatSpell = false,
        };
        public static Spell reducdedSpread = new Spell
        {
            name = "Reduced Spread",
            impactOnSpread = -20,
            isCombatSpell = false,
        };
        public static Spell increaseRange = new Spell
        {
            name = "More Range",
            impactOnRange = 10,
            isCombatSpell = false,
        };
        public static Spell increaseSpeed = new Spell
        {
            name = "Faster Bullets",
            impactOnSpeed = 2,
            isCombatSpell = false,
        };


    }
}
