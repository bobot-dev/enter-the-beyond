using Dungeonator;
using GungeonAPI;
using ItemAPI;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BotsMod
{
    class OtherworldlyConnections : PassiveItem
    {
        public static bool gotHitThisFloor = false;
        public static void Init()
        {

            string itemName = "Otherworldly Connections";
            string resourceName = "BotsMod/sprites/otherworldlyconnections";
            GameObject obj = new GameObject();

            var item = obj.AddComponent<OtherworldlyConnections>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "We are watching make sure you impress";
            string longDesc = "";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.SPECIAL;
            item.CanBeDropped = false;

        }
        /*public override void Pickup(PlayerController player)
        {

            CreateText(player.gameObject.transform, new Vector2(0f, 0f), "FUCK", TextAnchor.MiddleLeft, 7);

            player.OnRoomClearEvent += Player_OnRoomClearEvent;
            player.OnNewFloorLoaded += OnNewFloorLoaded;
        }

        public static Text CreateText(Transform parent, Vector2 offset, string text, TextAnchor anchor = TextAnchor.MiddleCenter, int font_size = 20)
        {
            GameObject gameObject = new GameObject("Text");
            gameObject.transform.SetParent(parent);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.SetTextAnchor(anchor);
            rectTransform.anchoredPosition = offset;
            Tools.LogPropertiesAndFields<Font>(Tools.shared_auto_001.LoadAsset<Font>("04b_03__"), "");
            Text text2 = gameObject.AddComponent<Text>();
            text2.horizontalOverflow = HorizontalWrapMode.Overflow;
            text2.verticalOverflow = VerticalWrapMode.Overflow;
            text2.alignment = anchor;
            text2.text = text;
            text2.font = Tools.shared_auto_001.LoadAsset<Font>("04b_03__");
            text2.fontSize = font_size;
            text2.color = Color.white;
            return text2;
        }
        */



        private void OnNewFloorLoaded(PlayerController obj)
        {
            gotHitThisFloor = false;
        }


        private void Player_OnRoomClearEvent(PlayerController obj)
        {
            if (obj.CurrentRoom != null && obj.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && obj.CurrentRoom.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
            {
                var devilLootTable = UnityEngine.Object.Instantiate(Tools.shared_auto_001.LoadAsset<GenericLootTable>("Shop_Key_Items_01"));
                devilLootTable.defaultItemDrops.elements.Clear();

                devilLootTable.AddItemToPool(60);
                devilLootTable.AddItemToPool(125);
                devilLootTable.AddItemToPool(434);
                devilLootTable.AddItemToPool(271);
                devilLootTable.AddItemToPool(407);
                devilLootTable.AddItemToPool(571);
                devilLootTable.AddItemToPool(33);
                devilLootTable.AddItemToPool(17);
                devilLootTable.AddItemToPool(347);
                devilLootTable.AddItemToPool(90);
                devilLootTable.AddItemToPool(336);
                devilLootTable.AddItemToPool(285);

                var room = RoomFactory.BuildFromResource("BotsMod/rooms/npctest.room").room;


                RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);
                room.usesCustomAmbientLight = true;
                room.customAmbientLight = new Color32(78, 34, 79, 255);
                Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



                GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

                foreach (var shop in UnityEngine.Object.FindObjectsOfType<BaseShopController>())
                {
                    BotsModule.Log(shop.gameObject.name);
                    if (shop.gameObject.name.Contains("Merchant_Key"))
                    {
                        BotsModule.Log("found");
                        shop.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;

                        shop.shopItems = devilLootTable;

                    }

                }
            }
        }

    }
}
