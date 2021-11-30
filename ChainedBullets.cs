using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using static GameActorEffect;

namespace BotsMod
{
    class ChainedBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Chained Bullets";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "BotsMod/sprites/chainbullets";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<ChainedBullets>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Divide and Conqueror";
            string longDesc = "Bullets have a chance to mark enemies, mark enemies chain to other nearby marked enemies, damage is split between chained enemies.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MoneyMultiplierFromEnemies, 1f, StatModifier.ModifyMethod.MULTIPLICATIVE);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.SPECIAL;
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Player_PostProcessProjectile;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= Player_PostProcessProjectile;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                Owner.PostProcessProjectile -= Player_PostProcessProjectile;
            }
            base.OnDestroy();
        }

        private void Player_PostProcessProjectile(Projectile proj, float arg2)
        {
            proj.OnHitEnemy += OnHitEnemy;
        }

        private void OnHitEnemy(Projectile proj, SpeculativeRigidbody target, bool fatal)
        {
            if (target.aiActor)
            {
                target.aiActor.ApplyEffect(new Marked
                {
                    effectIdentifier = "MarkedForChain",
                    AffectsEnemies = true,
                    AffectsPlayers = false,
                    stackMode = EffectStackingMode.Ignore,
                    maxStackedDuration = 10,
                    //OverheadVFX = overheadVFX,
                    AppliesTint = true,
                    TintColor = new Color32(94, 94, 94, 255),
                    AppliesDeathTint = true,
                    DeathTintColor = new Color32(48, 48, 48, 255),
                    duration = 10,
                    PlaysVFXOnActor = true,
                    AppliesOutlineTint = false,
                    chainedTo = new List<AIActor>(),
                    OutlineTintColor = Color.white,
                    resistanceType = EffectResistanceType.None
                });
            }
        }

       
    }
}
