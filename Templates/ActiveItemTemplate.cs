using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod.Templates
{
    class ActiveItemTemplate : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Item Name";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ActiveItemTemplate>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);

            item.quality = PickupObject.ItemQuality.C;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
        }
    }   
}

