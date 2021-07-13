using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace TurboItems
{
    public class FinalCountdownUnused : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Final Countdown Unused", "final_countdown");
            Game.Items.Rename("outdated_gun_mods:final_countdown_unused", "turbo:final_countdown_unused");
            gun.gameObject.AddComponent<FinalCountdownUnused>();
            gun.SetShortDescription("*sick guitar riff*");
            gun.SetLongDescription("Fires many final projectiles from whatever guns it feels like in no particular order.\n\nIT'S THE FI-NAL COUNT-DOWN");
            gun.SetupSprite(null, "final_countdown_idle_001", 24);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.quality = PickupObject.ItemQuality.S;
            gun.InfiniteAmmo = true;
            //gun.SetBaseMaxAmmo(350);
            gun.encounterTrackable.EncounterGuid = "*sick guitar riff* IT'S THE FINAL COUNTDOWN";

            //ice breaker
            Projectile IceBreakerFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(225) as Gun).RawSourceVolley.projectiles[0].finalProjectile);
            IceBreakerFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(IceBreakerFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(IceBreakerFinalProjectile);
            IceBreakerFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("Ice Breaker Proj Inited");

            //zorgun
            Projectile ZorgunFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(6) as Gun).DefaultModule.finalProjectile);
            ZorgunFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(ZorgunFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(ZorgunFinalProjectile);
            ZorgunFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("zorgun Proj Inited");

            //judge
            foreach (var projModual in (PickupObjectDatabase.GetById(184) as Gun).DefaultModule.finalVolley.projectiles)
            {
                foreach (var proj in projModual.projectiles)
                {
                    Projectile JudgeFinalProjectile = UnityEngine.Object.Instantiate<Projectile>(proj);
                    JudgeFinalProjectile.gameObject.SetActive(false);
                    FakePrefab.MarkAsFakePrefab(JudgeFinalProjectile.gameObject);
                    UnityEngine.Object.DontDestroyOnLoad(JudgeFinalProjectile);
                    JudgeFinalProjectile.transform.parent = gun.barrelOffset;


                    gun.DefaultModule.projectiles.Add(JudgeFinalProjectile);
                }
            }
            ETGModConsole.Log("judge Proj Inited");

            //teapot
            Projectile TeapotFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(596) as Gun).DefaultModule.finalProjectile);
            TeapotFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(TeapotFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(TeapotFinalProjectile);
            TeapotFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("teapot Proj Inited");

            //luxin cannon
            Projectile LuxinCannonFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(199) as Gun).DefaultModule.finalProjectile);
            LuxinCannonFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(LuxinCannonFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(LuxinCannonFinalProjectile);
            LuxinCannonFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("luxin cannon Proj Inited");

            //barrel (like shooting fish's synergy form)
            Projectile BarrelFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(709) as Gun).DefaultModule.finalProjectile);
            BarrelFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BarrelFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BarrelFinalProjectile);
            BarrelFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("barrel Proj Inited");

            //eye of the beholster
            Projectile BeholsterFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(90) as Gun).DefaultModule.finalProjectile);
            BeholsterFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BeholsterFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BeholsterFinalProjectile);
            BeholsterFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("eye of the beholster Proj Inited");

            //finished gun
            Projectile FinishedFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(762) as Gun).DefaultModule.finalProjectile);
            FinishedFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(FinishedFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(FinishedFinalProjectile);
            FinishedFinalProjectile.transform.parent = gun.barrelOffset;

            ETGModConsole.Log("finished gun Proj Inited");

            //ice breaker

            gun.DefaultModule.projectiles[0] = IceBreakerFinalProjectile;

            //zorgun

            gun.DefaultModule.projectiles.Add(ZorgunFinalProjectile);

            //judge



            //teapot

            gun.DefaultModule.projectiles.Add(TeapotFinalProjectile);

            //luxin cannon

            gun.DefaultModule.projectiles.Add(LuxinCannonFinalProjectile);

            //barrel

            gun.DefaultModule.projectiles.Add(BarrelFinalProjectile);

            //eye of the beholster

            gun.DefaultModule.projectiles.Add(BeholsterFinalProjectile);

            //finished gun: 1% chance to fire
            gun.DefaultModule.projectiles.Add(FinishedFinalProjectile);


            ETGMod.Databases.Items.Add(gun, null, "ANY");
            FinalCountdownID = gun.PickupObjectId;
        }
        public static int FinalCountdownID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
        }
        private bool HasReloaded;
        protected void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }
    }
}