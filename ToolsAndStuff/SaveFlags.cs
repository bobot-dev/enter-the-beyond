using SaveAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    static class SaveFlags
    {
        public static void Init()
        {
            BOT_EFFIGY_POWERED = SaveAPIManager.CreateNewDungeonFlag("bot", "BOT_EFFIGY_POWERED");
            BOT_LOST_UNLOCKED = SaveAPIManager.CreateNewDungeonFlag("bot", "BOT_LOST_UNLOCKED");
            BOT_BOSSKILLED_LOST_PAST = SaveAPIManager.CreateNewDungeonFlag("bot", "BOT_BOSSKILLED_LOST_PAST");

            MERCHANT_PURCHASES_HEART = SaveAPIManager.CreateNewTrackedStats("bot", "MERCHANT_PURCHASES_HEART");
            MONEY_SPENT_AT_HEART_SHOP = SaveAPIManager.CreateNewTrackedStats("bot", "MONEY_SPENT_AT_HEART_SHOP");
        }

        public static CustomDungeonFlags BOT_EFFIGY_POWERED;
        public static CustomDungeonFlags BOT_LOST_UNLOCKED;
        public static CustomDungeonFlags BOT_BOSSKILLED_LOST_PAST;

        public static CustomTrackedStats MERCHANT_PURCHASES_HEART;
        public static CustomTrackedStats MONEY_SPENT_AT_HEART_SHOP;
    }
}
