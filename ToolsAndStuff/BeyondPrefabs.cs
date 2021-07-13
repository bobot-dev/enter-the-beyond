using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class BeyondPrefabs
    {
        //almost all of this is just stolen from apache :P
        public static void Init()
        {

            
            sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");

            shop02 = sharedAssets2.LoadAsset<PrototypeDungeonRoom>("shop02");





            gungeon_rewardroom_1 = sharedAssets2.LoadAsset<PrototypeDungeonRoom>("gungeon_rewardroom_1");
            exit_room_basic = sharedAssets2.LoadAsset<PrototypeDungeonRoom>("exit_room_basic");
            elevator_entrance = sharedAssets2.LoadAsset<PrototypeDungeonRoom>("elevator entrance");
            doublebeholsterroom01 = Tools.LoadOfficialFlow("Secret_DoubleBeholster_Flow").AllNodes[2].overrideExactRoom;



            boss_foyertable = sharedAssets2.LoadAsset<GenericRoomTable>("Boss Foyers");
            shop_room_table = sharedAssets2.LoadAsset<GenericRoomTable>("Shop Room Table");

            CastleRoomTable = sharedAssets2.LoadAsset<GenericRoomTable>("Castle_RoomTable");
            SewerRoomTable = sharedAssets2.LoadAsset<GenericRoomTable>("Sewers_RoomTable");
            Gungeon_RoomTable = sharedAssets2.LoadAsset<GenericRoomTable>("Gungeon_RoomTable");
            SecretRoomTable = sharedAssets2.LoadAsset<GenericRoomTable>("secret_room_table_01");

            TutorialDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Tutorial");
            SewerDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");
            MinesDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Mines");
            ratDungeon = DungeonDatabase.GetOrLoadByName("base_resourcefulrat");
            CathedralDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Cathedral");
            BulletHellDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_BulletHell");
            ForgeDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
            CatacombsDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Catacombs");
            NakatomiDungeonPrefab = DungeonDatabase.GetOrLoadByName("base_nakatomi");


            SewersRoomTable = SewerDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
            AbbeyRoomTable = CathedralDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
            MinesRoomTable = MinesDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
            CatacombsRoomTable = CatacombsDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
            ForgeRoomTable = ForgeDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
            BulletHellRoomTable = BulletHellDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;



            BeyondRoomTable = ScriptableObject.CreateInstance<GenericRoomTable>();
            BeyondRoomTable.includedRooms = new WeightedRoomCollection();
            BeyondRoomTable.includedRooms.elements = new List<WeightedRoom>();
            BeyondRoomTable.includedRoomTables = new List<GenericRoomTable>(0);
        }

        public static AssetBundle sharedAssets2;

        //rooms
        public static PrototypeDungeonRoom shop02;
        public static PrototypeDungeonRoom gungeon_rewardroom_1;
        public static PrototypeDungeonRoom exit_room_basic;
        public static PrototypeDungeonRoom elevator_entrance;
        public static PrototypeDungeonRoom doublebeholsterroom01;

        //room tables
        public static GenericRoomTable boss_foyertable;
        public static GenericRoomTable shop_room_table;
        public static GenericRoomTable CastleRoomTable;
        public static GenericRoomTable SewerRoomTable;
        public static GenericRoomTable Gungeon_RoomTable;
        public static GenericRoomTable SecretRoomTable;

        //custom tables
        public static GenericRoomTable BeyondRoomTable;

        // Dungeon Specific Room Tables (from Dungeon AssetBundles)
        public static GenericRoomTable SewersRoomTable;
        public static GenericRoomTable AbbeyRoomTable;
        public static GenericRoomTable MinesRoomTable;
        public static GenericRoomTable CatacombsRoomTable;
        public static GenericRoomTable ForgeRoomTable;
        public static GenericRoomTable BulletHellRoomTable;

        //floors
        private static Dungeon TutorialDungeonPrefab;
        private static Dungeon SewerDungeonPrefab;
        private static Dungeon MinesDungeonPrefab;
        private static Dungeon ratDungeon;
        private static Dungeon CathedralDungeonPrefab;
        private static Dungeon BulletHellDungeonPrefab;
        private static Dungeon ForgeDungeonPrefab;
        private static Dungeon CatacombsDungeonPrefab;
        private static Dungeon NakatomiDungeonPrefab;

    }
}
