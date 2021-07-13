using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SparkBolt : SpellPickupObject
    {
        public static void Init()
        {
            string itemName = "Spark Bolt";
            string resourceName = "BotsMod/sprites/Spells/ShittySpellTempSprite";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<SparkBolt>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "A Simple Test Spell";
            string longDesc = "this shouldnt be seen in game";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.COMMON;

            item.spell = StaticSpellReferences.sparkBolt;
        }
    }
}
