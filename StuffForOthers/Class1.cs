using LootTableAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NpcApi;
using BotsMod;

namespace Items
{
    class fuck : MonoBehaviour
    {
        private void Awake()
        {
            m_gun = GetComponent<Gun>();
            Gun gun = m_gun;
            gun.OnReloadPressed += Reload;
        }
        private void Reload(PlayerController player, Gun gun, bool manual)
        {
            ETGModConsole.Log("ran 1");
            if (gun.IsReloading && !hasFired && !string.IsNullOrEmpty(synergyToCheck) && player.PlayerHasActiveSynergy(synergyToCheck))
            {
                ETGModConsole.Log("ran 2");
                for (int i = 0; i < numToFire; i++)
                {
                    Vector2 gunbarrel = gun.barrelOffset.position;
                    float angle = GetAccuracyAngled(gun.CurrentAngle, angleVariance, player);
                    GameObject gameObject = SpawnManager.SpawnProjectile(projToFire.gameObject, gunbarrel, Quaternion.Euler(0f, 0f, angle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = player;
                        component.Shooter = player.specRigidbody;
                        component.PossibleSourceGun = gun;
                        component.baseData.damage *= player.stats.GetStatValue(PlayerStats.StatType.Damage);
                        component.baseData.speed *= player.stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
                        component.baseData.force *= player.stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
                        component.baseData.range *= player.stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
                        component.BossDamageMultiplier *= player.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
                        component.AdditionalScaleMultiplier *= player.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale);
                        component.UpdateSpeed();
                        player.DoPostProcessProjectile(component);
                    }
                }
                hasFired = true;
            }
        }
        private void Update()
        {
            if (m_gun && !m_gun.IsReloading && hasFired) hasFired = false;
        }

        public static float GetAccuracyAngled(float startFloat, float variance, PlayerController playerToScaleAccuracyOff = null)
        {
            if (playerToScaleAccuracyOff != null) variance *= playerToScaleAccuracyOff.stats.GetStatValue(PlayerStats.StatType.Accuracy);
            float positiveVariance = variance * 0.5f;
            float negativeVariance = positiveVariance * -1f;
            float finalVariance = UnityEngine.Random.Range(negativeVariance, positiveVariance);
            return startFloat + finalVariance;
        }

        public bool hasFired;
        Gun m_gun;
        public int numToFire;
        [SerializeField]
        public string synergyToCheck;
        public Projectile projToFire;

        float angleVariance = 16;
    }
}