using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class LootTables
    {

        public static GenericLootTable testShopLable = new GenericLootTable();

        public static void Init()
        {
            AddItemToLootTable(testShopLable, 346);
            AddItemToLootTable(testShopLable, 8);
            AddItemToLootTable(testShopLable, 762);
            AddItemToLootTable(testShopLable, 241);
            AddItemToLootTable(testShopLable, 568);
            AddItemToLootTable(testShopLable, 636);
            AddItemToLootTable(testShopLable, 436);
        }

        public static void AddItemToLootTable(GenericLootTable lootTable, int itemId, float weight = 1)
        {
            PickupObject pickupObject = PickupObjectDatabase.GetById(itemId);
            lootTable.defaultItemDrops.Add(new WeightedGameObject()
            {
                pickupId = pickupObject.PickupObjectId,
                weight = weight,
                rawGameObject = pickupObject.gameObject,
                forceDuplicatesPossible = false,
                additionalPrerequisites = new DungeonPrerequisite[0]
            });
        }

    }
}
