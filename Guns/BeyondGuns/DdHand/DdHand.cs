using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class DdHand : GunBehaviour
    {

        public static void Add()
        {
            var gun1 = ItemBuilder.BuildGun<DdHand>("DD Hand", "bot", "dd_hand_tier1", "Tier 1", "wow", 8, 8, 8, PickupObject.ItemQuality.EXCLUDED, 377, ProjectileModule.ShootStyle.Automatic,
                ItemBuilder.BuildProjectile(377, 1.5f, 22, 6, 1000), 0f, 0.07f, 8, -1, 1000, new Vector2(0.25f, 0.125f), GunClass.FULLAUTO, GunHandedness.HiddenOneHanded);
            gun1.gunSwitchGroup = (PickupObjectDatabase.GetById(29) as Gun).gunSwitchGroup;
            gun1.muzzleFlashEffects = null;


            var gun2 = ItemBuilder.BuildGun<DdHand>("DD Hand 2", "bot", "dd_hand_tier2", "Tier 2", "wow", 8, 8, 8, PickupObject.ItemQuality.EXCLUDED, 377, ProjectileModule.ShootStyle.Automatic,
                ItemBuilder.BuildProjectile(377, 1.75f, 22, 6, 1000), 0f, 0.05f, 8, -1, 1000, new Vector2(0.25f, 0.125f), GunClass.FULLAUTO, GunHandedness.HiddenOneHanded);
            gun2.gunSwitchGroup = (PickupObjectDatabase.GetById(29) as Gun).gunSwitchGroup;
            gun2.muzzleFlashEffects = null;

            var gun3 = ItemBuilder.BuildGun<DdHand>("DD Hand 3", "bot", "dd_hand_tier3", "Tier 3", "wow", 8, 8, 8, PickupObject.ItemQuality.EXCLUDED, 377, ProjectileModule.ShootStyle.Automatic,
                ItemBuilder.BuildProjectile(377, 2, 22, 6, 1000), 0f, 0.03f, 8, -1, 1000, new Vector2(0.25f, 0.125f), GunClass.FULLAUTO, GunHandedness.HiddenOneHanded);
            gun3.gunSwitchGroup = (PickupObjectDatabase.GetById(29) as Gun).gunSwitchGroup;
            gun3.muzzleFlashEffects = null;


            gun1.GetComponent<DdHand>().TierThresholds = new int[] { 5, 10 };
            gun1.GetComponent<DdHand>().GunTierIds = new int[] { gun2.PickupObjectId, gun3.PickupObjectId };
            gun1.GetComponent<DdHand>().MaxTier = 3;
            gun1.GetComponent<DdHand>().homingChance = 0.1f;
            gun1.GetComponent<DdHand>().radialSlowInterface = new RadialSlowInterface
            {
                audioEvent = "",
                DoesCirclePass = false,
                DoesSepia = false,
                EffectRadius = 100,
                RadialSlowHoldTime = 5,
                RadialSlowInTime = 0.05f,
                RadialSlowOutTime = 0.05f,
                RadialSlowTimeModifier = 0.1f,
                UpdatesForNewEnemies = true
            };

        }

        private void Awake()
        {
            this.m_gun = base.GetComponent<Gun>();
            Gun gun = this.m_gun;
            gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun.OnInitializedWithOwner, new Action<GameActor>(this.OnGunInitialized));
            Gun gun2 = this.m_gun;
            gun2.OnDropped = (Action)Delegate.Combine(gun2.OnDropped, new Action(this.OnGunDroppedOrDestroyed));
            if (this.m_gun.CurrentOwner != null)
            {
                this.OnGunInitialized(this.m_gun.CurrentOwner);
            }
        }


        public override void PostProcessProjectile(Projectile projectile)
        {
            UnityEngine.Object.Destroy(projectile.gameObject.GetComponent<PierceProjModifier>());
            if (this.CurrentStrengthTier == this.MaxTier - 1)
            {
                if (UnityEngine.Random.value < homingChance)
                {
                    var piercing = projectile.gameObject.AddComponent<PierceProjModifier>();
                    piercing.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
                    piercing.penetratesBreakables = true;
                    piercing.penetration = 4;
                    piercing.UsesMaxBossImpacts = false;

                    var homing = projectile.gameObject.AddComponent<HomingModifier>();
                    homing.AngularVelocity = 360.0f;
                    homing.HomingRadius = 12f;

                    if (UnityEngine.Random.value <= 0.5f) projectile.AdjustPlayerProjectileTint(new Color32(251, 0, 255, 255), 0, 0);
                    if (UnityEngine.Random.value >= 0.5f) projectile.AdjustPlayerProjectileTint(new Color32(0, 242, 255, 255), 0, 0);
                }
                else
                {
                    projectile.AdjustPlayerProjectileTint(new Color32(255, 30, 0, 255), 0, 0);
                }
            }
            else
            {
                projectile.AdjustPlayerProjectileTint(new Color32(255, 30, 0, 255), 0, 0);
            }

            base.PostProcessProjectile(projectile);
        }


        private void OnGunInitialized(GameActor obj)
        {
            if (this.m_playerOwner != null)
            {
                this.OnGunDroppedOrDestroyed();
            }
            if (obj == null)
            {
                return;
            }
            if (obj is PlayerController)
            {
                this.m_playerOwner = (obj as PlayerController);
                this.m_playerOwner.OnKilledEnemy += this.OnEnemyKilled;
            }
        }

        private void OnGunDroppedOrDestroyed()
        {
            if (this.m_playerOwner != null)
            {
                this.m_playerOwner.OnKilledEnemy -= this.OnEnemyKilled;
                this.m_playerOwner = null;
            }
        }

        private void OnEnemyKilled(PlayerController obj)
        {
            if (obj.CurrentGun == this.m_gun)
            {
                this.m_kills++;
                if (this.m_kills > this.GetKillsToNextTier())
                {
                    this.CurrentStrengthTier = Mathf.Clamp(this.CurrentStrengthTier + 1, 0, this.MaxTier - 1);
                    this.m_gun.TransformToTargetGun((PickupObjectDatabase.GetById(GunTierIds[this.CurrentStrengthTier - 1]) as Gun));



                    AkSoundEngine.PostEvent("State_Bullet_Time_on", this.gameObject);
                    obj.StartCoroutine(this.HandleDuration(obj));

                }
            }
        }

        private IEnumerator HandleDuration(PlayerController user)
        {
            AkSoundEngine.PostEvent("State_Bullet_Time_on", this.gameObject);
            RenderSettings.ambientLight = new Color32(255, 0, 174, 255);
            this.m_activeElapsed = 0f;
            this.m_activeDuration = radialSlowInterface.RadialSlowHoldTime;            
            this.radialSlowInterface.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
            float ela = 0f;
            while (ela < this.m_activeDuration)
            {

                RenderSettings.ambientLight = Color32.Lerp(new Color32(255, 0, 174, 255), new Color32(255, 255, 255, 255), ela / radialSlowInterface.RadialSlowHoldTime);

                ela += GameManager.INVARIANT_DELTA_TIME;
                this.m_activeElapsed = ela;
                yield return null;
            }
            if (this)
            {
                AkSoundEngine.PostEvent("State_Bullet_Time_off", this.gameObject);
            }
            yield break;
        }



        public void MidGameSerialize(List<object> data, int dataIndex)
        {
            data.Add(this.m_kills);
        }

        public void MidGameDeserialize(List<object> data, ref int dataIndex)
        {
            this.m_kills = (int)data[dataIndex];
            if (this.m_gun == null)
            {
                this.m_gun = base.GetComponent<Gun>();
            }
            while (this.m_kills > this.GetKillsToNextTier())
            {
                this.CurrentStrengthTier = Mathf.Clamp(this.m_gun.CurrentStrengthTier + 1, 0, this.MaxTier - 1);
            }
            dataIndex++;
        }

        private int GetKillsToNextTier()
        {
            int currentStrengthTier = this.CurrentStrengthTier;
            if (currentStrengthTier >= this.TierThresholds.Length)
            {
                return int.MaxValue;
            }
            return this.TierThresholds[currentStrengthTier];
        }

        public RadialSlowInterface radialSlowInterface;
        public int[] TierThresholds;
        public int MaxTier;
        public float homingChance;
        public int CurrentStrengthTier;
        public int[] GunTierIds;
        private Gun m_gun;
        private PlayerController m_playerOwner;
        private int m_kills;
        float m_activeElapsed;
        float m_activeDuration;


    }
}
