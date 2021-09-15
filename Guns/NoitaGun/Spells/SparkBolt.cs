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
            //ItemBuilder.AddAnimatedSpriteToObject(itemName, new List<string> { "BotsMod/sprites/Spells/ShittySpellTempSprite_001", "BotsMod/sprites/Spells/ShittySpellTempSprite_002", "BotsMod/sprites/Spells/ShittySpellTempSprite_003", "BotsMod/sprites/Spells/ShittySpellTempSprite_004", "BotsMod/sprites/Spells/ShittySpellTempSprite_005", "BotsMod/sprites/Spells/ShittySpellTempSprite_006", "BotsMod/sprites/Spells/ShittySpellTempSprite_007", "BotsMod/sprites/Spells/ShittySpellTempSprite_008", "BotsMod/sprites/Spells/ShittySpellTempSprite_009", "BotsMod/sprites/Spells/ShittySpellTempSprite_010", "BotsMod/sprites/Spells/ShittySpellTempSprite_011", "BotsMod/sprites/Spells/ShittySpellTempSprite_012", }, obj);


            string shortDesc = "A Simple Test Spell";
            string longDesc = "this shouldnt be seen in game";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.COMMON;

            //item.spell = StaticSpellReferences.sparkBolt;
            item.minimapIcon = SpriteBuilder.SpriteFromResource("BotsMod/sprites/SpellMiniMapIcon");

            UnityEngine.Object.DontDestroyOnLoad(item.minimapIcon);
            FakePrefab.MarkAsFakePrefab(item.minimapIcon);

            item.InitSpell(item, StaticSpellReferences.sparkBolt);
        }

      

    }
}
