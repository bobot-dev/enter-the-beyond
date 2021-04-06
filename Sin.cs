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
    public class Sin : PassiveItem
    {

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "help!";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<Sin>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "no one can help you now";
            string longDesc = "hahahahhahahahahahahahahhahahahahahahahahahahahahahahahahahahaha";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.EXCLUDED;

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.PreventStartingOwnerFromDropping = true;

        }

        List<string> enemies = new List<string>();

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += OnEnteredCombat;
        }

        private void OnEnteredCombat()
        {

            if (enemies.Count > 0)
            {
                foreach (string guid in enemies)
                {
                    AIActor fuckyou = Tools.SummonAtRandomPosition(guid, base.Owner);
                    fuckyou.RegisterOverrideColor(new Color32(255, 0, 0, 255), "god help you");
                    fuckyou.gameObject.AddComponent<PlsIgnoreMe>();
                }
            }


            List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].EnemyGuid != "22fc2c2c45fb47cf9fb5f7b043a70122" && activeEnemies[i].EnemyGuid != "699cd24270af4cd183d671090d8323a1" && activeEnemies[i].EnemyGuid != "a446c626b56d4166915a4e29869737fd" && activeEnemies[i].gameObject.GetComponent<PlsIgnoreMe>() == null)
                    {
                        enemies.Add(activeEnemies[i].EnemyGuid);
                    }
                }
            }
        }
    }

    public class PlsIgnoreMe : MonoBehaviour
    {
    }
}

