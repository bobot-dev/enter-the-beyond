using Dungeonator;
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace BotsMod
{
    class BeyondPrefabs
    {
		public static AssetBundle shared_auto_002;
		public static AssetBundle shared_auto_001;
		public static AssetBundle ModAssets;
		public static AssetBundle braveResources;

		public static GameObject lostFigurePlaceable;

		private static Dungeon TutorialDungeonPrefab;
		private static Dungeon SewerDungeonPrefab;
		private static Dungeon MinesDungeonPrefab;
		private static Dungeon ratDungeon;
		private static Dungeon CathedralDungeonPrefab;
		private static Dungeon BulletHellDungeonPrefab;
		private static Dungeon ForgeDungeonPrefab;
		private static Dungeon CatacombsDungeonPrefab;
		private static Dungeon NakatomiDungeonPrefab;

		public static PrototypeDungeonRoom reward_room;
		public static PrototypeDungeonRoom gungeon_rewardroom_1;
		public static PrototypeDungeonRoom shop02;
		public static PrototypeDungeonRoom doublebeholsterroom01;


		public static GenericRoomTable shop_room_table;
		public static GenericRoomTable boss_foyertable;
		public static GenericRoomTable BeyondRoomTable;
		public static GenericRoomTable SecretRoomTable;

		public static GenericRoomTable CastleRoomTable;
		public static GenericRoomTable Gungeon_RoomTable;
		public static GenericRoomTable SewersRoomTable;
		public static GenericRoomTable AbbeyRoomTable;
		public static GenericRoomTable MinesRoomTable;
		public static GenericRoomTable CatacombsRoomTable;
		public static GenericRoomTable ForgeRoomTable;
		public static GenericRoomTable BulletHellRoomTable;

		public static Texture2D ENV_Tileset_Beyond;

		public static void InitCustomPrefabs()
		{
			ModAssets = AssetBundleLoader.LoadAssetBundleFromLiterallyAnywhere("modassets");

			//ENV_Tileset_Beyond = ModAssets.LoadAsset<Texture2D>("ENV_Tileset_Beyond");
			ENV_Tileset_Beyond = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/ENV_Tileset_Beyond.png");

			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
			shared_auto_001 = assetBundle;
			shared_auto_002 = assetBundle2;
			braveResources = ResourceManager.LoadAssetBundle("brave_resources_001");
			if (ModAssets is null)
			{
				ETGModConsole.Log("ModAssets is null!");
			}
			TutorialDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Tutorial");
			SewerDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");
			MinesDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Mines");
			ratDungeon = DungeonDatabase.GetOrLoadByName("base_resourcefulrat");
			CathedralDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Cathedral");
			BulletHellDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_BulletHell");
			ForgeDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
			CatacombsDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Catacombs");
			NakatomiDungeonPrefab = DungeonDatabase.GetOrLoadByName("base_nakatomi");

			reward_room = shared_auto_002.LoadAsset<PrototypeDungeonRoom>("reward room");
			gungeon_rewardroom_1 = shared_auto_002.LoadAsset<PrototypeDungeonRoom>("gungeon_rewardroom_1");
			shop_room_table = shared_auto_002.LoadAsset<GenericRoomTable>("Shop Room Table");
			shop02 = shared_auto_002.LoadAsset<PrototypeDungeonRoom>("shop02");
			boss_foyertable = shared_auto_002.LoadAsset<GenericRoomTable>("Boss Foyers");

			BeyondRoomTable = ScriptableObject.CreateInstance<GenericRoomTable>();
			BeyondRoomTable.includedRooms = new WeightedRoomCollection();
			BeyondRoomTable.includedRooms.elements = new List<WeightedRoom>();
			BeyondRoomTable.includedRoomTables = new List<GenericRoomTable>(0);

			SecretRoomTable = shared_auto_002.LoadAsset<GenericRoomTable>("secret_room_table_01");

			CastleRoomTable = shared_auto_002.LoadAsset<GenericRoomTable>("Castle_RoomTable");
			Gungeon_RoomTable = shared_auto_002.LoadAsset<GenericRoomTable>("Gungeon_RoomTable");
			SewersRoomTable = SewerDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
			AbbeyRoomTable = CathedralDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
			MinesRoomTable = MinesDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
			CatacombsRoomTable = CatacombsDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
			ForgeRoomTable = ForgeDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;
			BulletHellRoomTable = BulletHellDungeonPrefab.PatternSettings.flows[0].fallbackRoomTable;

			doublebeholsterroom01 = BeyondDungeonFlows.LoadOfficialFlow("Secret_DoubleBeholster_Flow").AllNodes[2].overrideExactRoom;


			lostFigurePlaceable = ItemAPI.SpriteBuilder.SpriteFromResource("BotsMod/sprites/loststatuethatlooksawful");

			FakePrefab.MarkAsFakePrefab(lostFigurePlaceable);
			lostFigurePlaceable.SetActive(false);

			SpeculativeRigidbody rigidbody = NpcApi.ItsDaFuckinShopApi.GenerateOrAddToRigidBody(lostFigurePlaceable, CollisionLayer.HighObstacle, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(17, 27), new IntVector2(0, 0)) ;


			PlacedWallDecorator placedWallDecorator = lostFigurePlaceable.AddComponent<PlacedWallDecorator>();
			

			placedWallDecorator.ignoresBorders = false;
			placedWallDecorator.ignoreWallDrawing = false;
			placedWallDecorator.wallClearanceHeight = 3;
			placedWallDecorator.wallClearanceWidth = 1;
			placedWallDecorator.wallClearanceXStart = 0;
			placedWallDecorator.wallClearanceXStart = 0;

			var comp = shared_auto_002.LoadAsset<GameObject>("RatFigure_Bullet").GetComponent<MinorBreakable>();

			lostFigurePlaceable.AddComponent(comp);


			DungeonPlaceableBehaviour dungeonPlaceableBehaviour = lostFigurePlaceable.AddComponent<DungeonPlaceableBehaviour>();
			dungeonPlaceableBehaviour.placeableWidth = 1;
			dungeonPlaceableBehaviour.placeableHeight = 1;
			dungeonPlaceableBehaviour.difficulty = 0;
			dungeonPlaceableBehaviour.isPassable = false;

			shared_auto_002.LoadAsset<DungeonPlaceable>("Rat Figure Random").variantTiers.Add(new DungeonPlaceableVariant
			{
				percentChance = 1,
				unitOffset = Vector2.zero,
				nonDatabasePlaceable = lostFigurePlaceable,
				enemyPlaceableGuid = "",
				pickupObjectPlaceableId = -1,
				forceBlackPhantom = false,
				addDebrisObject = false,	
				prerequisites = new DungeonPrerequisite[0],
				materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
				
			});


		}
	}
}
