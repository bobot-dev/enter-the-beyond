using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;
//demonetization
namespace BotsMod
{
    class CosmicSludge : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {
            //The name of the item
            string itemName = "Cosmic Sludge";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "BotsMod/sprites/cosmic_sludge";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CosmicSludge>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "What the...";
            string longDesc = "Randomizes some stats after rolling.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
            item.PlaceItemInAmmonomiconAfterItemById(190);
            item.SetupUnlockOnFlag(GungeonFlags.FLAG_EEVEE_UNLOCKED, true);
        }

        private void HandleRollStarted(PlayerController player, Vector2 arg2)
        {
            this.RandomizeStats(player);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnKilledEnemy += this.OnKill;
            base.Pickup(player);
            player.OnRollStarted += this.HandleRollStarted;
            this.ClearOverrideShader();
            //player.IsTemporaryEeveeForUnlock = true;
        }

        public void ClearOverrideShader()
        {

        }
        private void RandomizeStats(PlayerController player) 
        {
            if (Randomized == true)
            {
                float curDamage2 = player.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
                float curAccuracy2 = player.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy);
                float curSpeed2 = player.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
                float curFireRate2 = player.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
                float curReload2 = player.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
                float curSize2 = player.stats.GetBaseStatValue(PlayerStats.StatType.PlayerBulletScale);

                float newDamage2 = curDamage2 - damageRandom;
                float newAccuracy2 = curAccuracy2 - accuracyRandom;
                float newSpeed2 = curSpeed2 - speedRandom;
                float newFireRate2 = curFireRate2 - fireRateRandom;
                float newReload2 = curReload2 - reloadRandom;
                float newSize2 = curSize2 - sizeRandom;

                player.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage2, player);
                player.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, newAccuracy2, player);
                player.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed2, player);
                player.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate2, player);
                player.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newReload2, player);
                player.stats.SetBaseStatValue(PlayerStats.StatType.PlayerBulletScale, newSize2, player);
            }
            float damageRange = UnityEngine.Random.Range(-1.8f, 0.2f);
            float accuracyRange = UnityEngine.Random.Range(-1.5f, 0.5f);
            float speedRange = UnityEngine.Random.Range(-1.8f, 0.2f);
            float fireRateRange = UnityEngine.Random.Range(-1.5f, 0.5f);
            float reloadRange = UnityEngine.Random.Range(-1.5f, 0.5f);
            float sizeRange = UnityEngine.Random.Range(-1.5f, 0.5f);

            float curDamage = player.stats.GetBaseStatValue(PlayerStats.StatType.Damage);
            float curAccuracy = player.stats.GetBaseStatValue(PlayerStats.StatType.Accuracy);
            float curSpeed = player.stats.GetBaseStatValue(PlayerStats.StatType.MovementSpeed);
            float curFireRate = player.stats.GetBaseStatValue(PlayerStats.StatType.RateOfFire);
            float curReload = player.stats.GetBaseStatValue(PlayerStats.StatType.ReloadSpeed);
            float curSize = player.stats.GetBaseStatValue(PlayerStats.StatType.PlayerBulletScale);

            float newDamage = curDamage + damageRange;
            float newAccuracy = curAccuracy + accuracyRange;
            float newSpeed = curSpeed + speedRange;
            float newFireRate = curFireRate + fireRateRange;
            float newReload = curReload + reloadRange;
            float newSize = curSize + sizeRange;

            player.stats.SetBaseStatValue(PlayerStats.StatType.Damage, newDamage, player);
            player.stats.SetBaseStatValue(PlayerStats.StatType.Accuracy, newAccuracy, player);
            player.stats.SetBaseStatValue(PlayerStats.StatType.MovementSpeed, newSpeed, player);
            player.stats.SetBaseStatValue(PlayerStats.StatType.RateOfFire, newFireRate, player);
            player.stats.SetBaseStatValue(PlayerStats.StatType.ReloadSpeed, newAccuracy, player);
            player.stats.SetBaseStatValue(PlayerStats.StatType.PlayerBulletScale, newSize, player);

            damageRandom = newDamage - curDamage;
            accuracyRandom = newAccuracy - curAccuracy;
            speedRandom = newSpeed - curSpeed;
            fireRateRandom = newFireRate - curFireRate;
            reloadRandom = newReload - curReload;
            sizeRandom = newSize - curSize;
            Randomized = true;
        }


        private void OnKill(PlayerController player)
        {
           //EnableShader(this.Owner);
           //this.m_glintShader = Shader.Find("Brave/PlayerShaderEevee");
           ////if (player.CurrentGun)
           //{
           //     this.ProcessGunShader(player.CurrentGun);
          // }
        }
        /*
        private void ProcessGunShader(Gun g)
        {
            MeshRenderer component = g.GetComponent<MeshRenderer>();
            if (!component)
            {
                return;
            }
            Material[] sharedMaterials = component.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].shader == this.m_glintShader)
                {
                    return;
                }
            }
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
            Material material = new Material(this.m_glintShader);
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
        }
        protected void EnableShader(PlayerController user)
        {
            if (!user)
            {
                return;
            }
            Material[] array = user.SetOverrideShader(ShaderCache.Acquire("Brave/PlayerShaderEevee"));
            for (int i = 0; i < array.Length; i++)
            {
                if (!(array[i] == null))
                {
                    array[i].SetFloat("_AllColorsToggle", 1f);
                }
            }
        }
       // public tk2dSpriteAnimation RatAnimationLibrary;
        //private PlayerController m_lastPlayer;
        private Shader m_glintShader;
        */
        float damageRandom = -1;
        float accuracyRandom = -1;
        float speedRandom = -1;
        float fireRateRandom = -1;
        float reloadRandom = -1;
        float sizeRandom = -1;

        bool Randomized = false;
    }
}
