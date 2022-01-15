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
		public static GameObject beyondEnterance;
		public static GameObject pastControllerObject;
		public static GameObject laserCutterParticles;

		public static PlayerHandController basicBeyondHands;

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

		public static AssetBundle AHHH;
		public static AssetBundle fucktilesets;
		public static AssetBundle EtbAssetBundle;
		public static AssetBundle BotsAssetBundle;

		public static void InitCustomPrefabs()
		{


			AHHH = BotsModule.LoadAssetBundleFromLiterallyAnywhere("coolshader");
			fucktilesets = BotsModule.LoadAssetBundleFromLiterallyAnywhere("fucktilesets");
			EtbAssetBundle = BotsModule.LoadAssetBundleFromLiterallyAnywhere("enterthebeyond");
			BotsAssetBundle = BotsModule.LoadAssetBundleFromLiterallyAnywhere("botsassetbundle");


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
			
			lostFigurePlaceable = FakePrefab.Clone(shared_auto_002.LoadAsset<GameObject>("RatFigure_Bullet"));
			lostFigurePlaceable.SetActive(false);
			var tksprite = lostFigurePlaceable.GetComponent<tk2dSprite>();
			tksprite.SetSprite(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/loststatuethatlooksawful", lostFigurePlaceable.GetComponent<tk2dSprite>().Collection));
			var spriteDef = tksprite.Collection.spriteDefinitions[tksprite.spriteId];
			spriteDef.position0 = new Vector3(0.125f, 0.3125f, 0);
			spriteDef.position1 = new Vector3(1.125f, 0.3125f, 0);
			spriteDef.position2 = new Vector3(0.125f, 2.0625f, 0);
			spriteDef.position3 = new Vector3(1.125f, 2.0625f, 0);
			spriteDef.material = tksprite.Collection.spriteDefinitions[shared_auto_002.LoadAsset<GameObject>("RatFigure_Bullet").GetComponent<tk2dSprite>().spriteId].material;

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
			var idleIdsList = new List<int>();
			var unchargedIdleIdsList = new List<int>();
			var chargingIdsList = new List<int>();
			var collection = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection;
			List<string> idleSpritePaths = new List<string>
			{
				"BotsMod/sprites/effigy/effigy_of_the_beyond1.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond2.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond3.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond4.png",
			};

			List<string> unchargedIdleSpritePaths = new List<string>
			{
				"BotsMod/sprites/effigy/effigy_of_the_beyond_empty_001.png",
			};

			List<string> chargingSpritePaths = new List<string>
			{
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_001.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_002.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_003.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_004.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_005.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_006.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_007.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_008.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_009.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_010.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_011.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_012.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_013.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_014.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_015.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_016.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_017.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_018.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_019.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_020.png",
				"BotsMod/sprites/effigy/effigy_of_the_beyond_charge_021.png",
			};


			foreach (string sprite in idleSpritePaths)
			{
				idleIdsList.Add(SpriteBuilder.AddSpriteToCollection(sprite, collection));
			}

			foreach (string sprite in unchargedIdleSpritePaths)
			{
				unchargedIdleIdsList.Add(SpriteBuilder.AddSpriteToCollection(sprite, collection));
			}

			foreach (string sprite in chargingSpritePaths)
			{
				chargingIdsList.Add(SpriteBuilder.AddSpriteToCollection(sprite, collection));
			}

			pastControllerObject = new GameObject("PastControllerObject");
			FakePrefab.MarkAsFakePrefab(pastControllerObject);

			pastControllerObject.AddComponent<LostPastController>();


			basicBeyondHands = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Enemies/hand.png", new GameObject("BeyondHand")).AddComponent<PlayerHandController>();
			FakePrefab.MarkAsFakePrefab(basicBeyondHands.gameObject);
			basicBeyondHands.ForceRenderersOff = false;


			beyondEnterance = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], new GameObject("EffigyOfTheBeyond"));
			beyondEnterance.SetActive(false);
			FakePrefab.MarkAsFakePrefab(beyondEnterance);

			tk2dSpriteAnimator spriteAnimator = beyondEnterance.AddComponent<tk2dSpriteAnimator>();

			SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "idle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
			SpriteBuilder.AddAnimation(spriteAnimator, collection, unchargedIdleIdsList, "unchargedIdle", tk2dSpriteAnimationClip.WrapMode.Loop, 8);
			SpriteBuilder.AddAnimation(spriteAnimator, collection, chargingIdsList, "charging", tk2dSpriteAnimationClip.WrapMode.Once, 8);

			spriteAnimator.DefaultClipId = spriteAnimator.GetClipIdByName("idle");
			spriteAnimator.playAutomatically = true;

			var secretFloorInteractable = beyondEnterance.AddComponent<EffigyInteractableComp>();
			secretFloorInteractable.placeableWidth = 2;
			secretFloorInteractable.placeableHeight = 2;
			secretFloorInteractable.isPassable = true;
			secretFloorInteractable.targetLevelName = "tt_beyond";
			secretFloorInteractable.overridePitIndex = null;
			secretFloorInteractable.targetTileset = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND;
			secretFloorInteractable.worldLocks = null;

			laserCutterParticles = BeyondPrefabs.AHHH.LoadAsset<GameObject>("LaserCutterSystem");
			laserCutterParticles.AddComponent<PlayParticleSystemDuringBossIntro>();
		}
	}
}
