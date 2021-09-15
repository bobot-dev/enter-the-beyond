using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NpcApi;

namespace BotsMod
{
    class ModRoomPrefabs
    {
        public static PrototypeDungeonRoom Keep_Entrance_Room;
        public static void InitCustomRooms()
        {
            Mod_RoomList = new List<string>()
            {
                //the names of all of our rooms once we make them.
                //"Beyond_Entrance2.room"
            };
            // Mod_Entrance_Room = RoomFactory.BuildFromResource("Mod/Resources/ModRooms/floorEntrance.room");
            // Mod_Exit_Room = RoomFactory.BuildFromResource("Mod/Resources/ModRooms/floorExit.room");

            Mod_Entrance_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Entrance2.room");
           
            Mod_Exit_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Exit.room");

            Mod_Shop_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Shop.room");
            ItsDaFuckinShopApi.RegisterShopRoom(BotsModule.shop.gameObject, Mod_Shop_Room, new UnityEngine.Vector2(-2.5f, -3));
            

            Mod_Entrance_Room.category = PrototypeDungeonRoom.RoomCategory.ENTRANCE;



            Mod_Entrance_Room_Past = RoomFactory.BuildFromResource("BotsMod/rooms/Past/LostPastRoom1.room");
            Mod_Entrance_Room_Past.category = PrototypeDungeonRoom.RoomCategory.ENTRANCE;


            List<PrototypeDungeonRoom> m_feyondRooms = new List<PrototypeDungeonRoom>();

            foreach (string name in Mod_RoomList)
            {
                PrototypeDungeonRoom m_room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/" + name);
                m_feyondRooms.Add(m_room);
            }

            Mod_Rooms = m_feyondRooms.ToArray();

            foreach (PrototypeDungeonRoom room in Mod_Rooms)
            {
                BeyondPrefabs.BeyondRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, 1));
            }

            Mod_Boss = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/overseerbossroombutround.room");
            Mod_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Mod_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Mod_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Mod_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Mod_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Mod_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Mod_Boss.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Mod_Boss.usesProceduralLighting = false;
            Mod_Boss.usesProceduralDecoration = false;
            Mod_Boss.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            Mod_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.BELLYGEON;

            Mod_Boss_Past = RoomFactory.BuildFromResource("BotsMod/rooms/Past/LostPastBossRoom.room");
            Mod_Boss_Past.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Mod_Boss_Past.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Mod_Boss_Past.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Mod_Boss_Past.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Mod_Boss_Past.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Mod_Boss_Past.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Mod_Boss_Past.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Mod_Boss_Past.usesProceduralLighting = false;
            Mod_Boss_Past.usesProceduralDecoration = false;
            Mod_Boss_Past.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            Mod_Boss_Past.overriddenTilesets = GlobalDungeonData.ValidTilesets.BELLYGEON;


            //foreach (PrototypeRoomExit exit in Mod_Boss.exitData.exits) { exit.exitType = PrototypeRoomExit.ExitType.ENTRANCE_ONLY; }
            //    RoomBuilder.AddExitToRoom(Mod_Boss, new Vector2(26, 37), DungeonData.Direction.NORTH, PrototypeRoomExit.ExitType.EXIT_ONLY, PrototypeRoomExit.ExitGroup.B);
        }

        public static WeightedRoom GenerateWeightedRoom(PrototypeDungeonRoom Room, float Weight = 1, bool LimitedCopies = true, int MaxCopies = 1, DungeonPrerequisite[] AdditionalPrerequisites = null)
        {
            if (Room == null) { return null; }
            if (AdditionalPrerequisites == null) { AdditionalPrerequisites = new DungeonPrerequisite[0]; }
            return new WeightedRoom() { room = Room, weight = Weight, limitedCopies = LimitedCopies, maxCopies = MaxCopies, additionalPrerequisites = AdditionalPrerequisites };
        }

        public static PrototypeDungeonRoom Mod_Entrance_Room;
        public static PrototypeDungeonRoom Mod_Entrance_Room_Past;
        public static PrototypeDungeonRoom Mod_Exit_Room;
        public static PrototypeDungeonRoom Mod_Shop_Room;
        public static PrototypeDungeonRoom[] Mod_Rooms;
        public static PrototypeDungeonRoom Mod_Boss;
        public static PrototypeDungeonRoom Mod_Boss_Past;
        public static List<string> Mod_RoomList; // this will contain all of our mods rooms.
    }
}
