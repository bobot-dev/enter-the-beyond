using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class RoomPrefabs
    {
        public static void Init()
        {
            ExpandObjectDatabase objectDatabase = new ExpandObjectDatabase();

            Expand_Beyond_Exit = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Entrance.room", true);
            Expand_Beyond_Exit.associatedMinimapIcon = BeyondPrefabs.exit_room_basic.associatedMinimapIcon;
           
            RoomFactory.AddObjectToRoom(Expand_Beyond_Exit, new Vector2(2, 5), objectDatabase.GodRays);

            Expand_Beyond_Entrance = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastEntrace.room", true);
            RoomFactory.AddObjectToRoom(Expand_Beyond_Entrance, new Vector2(4, 5), objectDatabase.GodRays);
            Expand_Beyond_Entrance.associatedMinimapIcon = BeyondPrefabs.elevator_entrance.associatedMinimapIcon;


            Expand_Beyond_Boss = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastBossRoom.room", false);
            Expand_Beyond_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Expand_Beyond_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Expand_Beyond_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Expand_Beyond_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Expand_Beyond_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Expand_Beyond_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Expand_Beyond_Boss.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Expand_Beyond_Boss.usesProceduralLighting = false;
            Expand_Beyond_Boss.usesProceduralDecoration = false;
            Expand_Beyond_Boss.rewardChestSpawnPosition = new IntVector2(25, 20);
            Expand_Beyond_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.JUNGLEGEON;
            foreach (PrototypeRoomExit exit in Expand_Beyond_Boss.exitData.exits) { exit.exitType = PrototypeRoomExit.ExitType.ENTRANCE_ONLY; }

        }

        public static PrototypeDungeonRoom Expand_Beyond_Exit;
        public static PrototypeDungeonRoom Expand_Beyond_Entrance;
        public static PrototypeDungeonRoom Expand_Beyond_Boss;
    }
}
