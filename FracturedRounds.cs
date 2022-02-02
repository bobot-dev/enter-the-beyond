using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class FracturedRounds : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Fractured Rounds";
            string resourceName = "BotsMod/sprites/fractured_rounds";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<FracturedRounds>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Two Parts Of A Whole";
            string longDesc = "These bullets where forged in the depths of the beyond their original function is unkown but they react violently with bullets from the Gungeon causing them to rip in two when making contact with any living thing this reaction comes with a downside as it greatly weakens the bullets.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.D;
            item.AddPassiveStatModifier(PlayerStats.StatType.Damage, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            Tools.BeyondItems.Add(item.PickupObjectId);

            item.sprite.usesOverrideMaterial = true;
            Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

            material.SetTexture("_MainTex", item.sprite.renderer.material.mainTexture);
            material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
            material.SetFloat("_EmissiveColorPower", 1.55f);
            material.SetFloat("_EmissivePower", 55);
            item.sprite.renderer.material = material;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            FracturedRounds.IncrementFlag(player, typeof(FracturedRounds));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            FracturedRounds.DecrementFlag(player, typeof(FracturedRounds));
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            FracturedRounds.DecrementFlag(GameManager.Instance.PrimaryPlayer, typeof(FracturedRounds));
            base.OnDestroy();
        }

    }
}
