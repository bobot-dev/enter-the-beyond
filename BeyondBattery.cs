using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class BeyondBattery : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Beyond Battery";
            string resourceName = "BotsMod/sprites/beyond_battery";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BeyondBattery>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Random Recharge";
            string longDesc = "This strange power source sometimes lets out a strong burst of energy, while there is no documented cases of injury as a result of said discharges it dose seem they can negate the need to recharge some object found within the gungeon.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            item.quality = PickupObject.ItemQuality.C;

            BeyondBattery.negationChance = 0.1f;

            item.SetTag("beyond");
            BotsItemIds.BeyondBattery = item.PickupObjectId;
        }

         
        public static bool ShouldNegateCooldown()
        {
            var val = UnityEngine.Random.value;
            ETGModConsole.Log(val + "");
            return val <= negationChance;
        }

        static float negationChance = 0;
    }
}
