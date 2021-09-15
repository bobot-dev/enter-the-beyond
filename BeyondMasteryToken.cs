using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using BotsMod;

namespace BotsMod
{
    public class BeyondMasteryToken : BasicStatPickup
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Master Round";
            string resourceName = "BotsMod/sprites/master_token_castle_001";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<BeyondMasteryToken>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "??? Chamber";
            string longDesc = "This corrupted artifact indicates mastery of a strange chamber. \n\n The legendary hero felled the beast at the heart of the Gungeon with five rounds. According to the myth, the sixth remains unfired.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.SPECIAL;

            //EncounterDatabase.GetEntry(item.encounterTrackable.EncounterGuid).usesPurpleNotifications = true;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);


            BotsItemIds.BeyondMasteryToken = item.PickupObjectId;

            //item.ItemRespectsHeartMagnificence = true;

            item.minimapIcon = BotsModule.WarCrime;

            item.IsMasteryToken = true;

            item.PlaceItemInAmmonomiconAfterItemById(467);


            //Tools.BeyondItems.Add(item.PickupObjectId);
            //item.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            //item.sprite.usesOverrideMaterial = true;

        }
    }
        
}