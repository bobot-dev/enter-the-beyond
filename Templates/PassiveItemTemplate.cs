using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class PassiveItemTemplate : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Item Name";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PassiveItemTemplate>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            item.quality = PickupObject.ItemQuality.C;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}
