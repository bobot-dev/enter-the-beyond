using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using BotsMod;
using System.Runtime.CompilerServices;
using System.Reflection;
using static BotsMod.thingistolefromapache;
using System.IO;

namespace BotsMod
{
    public class LostsCloak : PassiveItem
    {

        
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Lost Robe";
            string resourceName = "BotsMod/sprites/cloak";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<LostsCloak>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "idk ill put something here";
            string longDesc = "*good description and lore here* atm this item just makes items from this mod more common it'll will be changed in the future but im unsure when that will be";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.SPECIAL;

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.PreventStartingOwnerFromDropping = true;
            m_item = item;
            BotsItemIds.LostCloak = item.PickupObjectId;

            item.PlaceItemInAmmonomiconAfterItemById(414);
            List<LootModData> itemList = new List<LootModData>();
            foreach(var po in PickupObjectDatabase.Instance.Objects)
            {
                if (po != null && po.PickupObjectId >= BotsItemIds.BeyondMasteryToken && po.PickupObjectId <= BotsItemIds.Stairway)
                {
                    itemList.Add(new LootModData { AssociatedPickupId = po.PickupObjectId, DropRateMultiplier = 10 });
                }
            }
            item.associatedItemChanceMods = itemList.ToArray();
            //Tools.BeyondItems.Add(item.PickupObjectId);

        }

        static PassiveItem m_item;

        private ImprovedAfterImage zoomy;

        public override void Pickup(PlayerController player)
        {
            //player.OnRollStarted += Player_OnRollStarted;


           
            base.Pickup(player);


            Tools.AddGlow(player, 55, 4.55f, new Color32(255, 0, 38, 255));


            if (m_pickedUp) { return; }



            zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
            zoomy.dashColor = new Color(180, 32, 42);
            zoomy.spawnShadows = false;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowLifetime = 0.3f;
            zoomy.minTranslation = 0.05f;
            zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");



            
        }


        






    }        
}

