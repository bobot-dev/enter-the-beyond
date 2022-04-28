using Dungeonator;
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Reflection;

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
		public static GameObject beyondChestPrefab;
		public static GameObject laserSight;
		public static GameObject BeyondTableH;
		public static GameObject BeyondTableV;

		public static PlayerHandController basicBeyondHands;

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

		public static Shader BeyondJammedShader;

		public static tk2dSpriteCollectionData beyondCollection;
		public static GenericLootTable beyondLootTable;

		public static tk2dSpriteCollectionData itemCollection = PickupObjectDatabase.GetByEncounterName("singularity").sprite.Collection;
		public static tk2dSpriteCollectionData ammonomiconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
		public static void InitCustomPrefabs()
		{


			AHHH = BotsModule.LoadAssetBundleFromLiterallyAnywhere("coolshader");
			fucktilesets = BotsModule.LoadAssetBundleFromLiterallyAnywhere("fucktilesets");
			EtbAssetBundle = BotsModule.LoadAssetBundleFromLiterallyAnywhere("enterthebeyond");
			BotsAssetBundle = BotsModule.LoadAssetBundleFromLiterallyAnywhere("botsassetbundle");



			var castlePrefab = DungeonDatabase.GetOrLoadByName("base_castle");

			Material[] materials = fucktilesets.LoadAllAssets<Material>();
			foreach (Material m in materials)
			{
				var shaderName = m.shader.name;
				var newShader = Shader.Find(shaderName);
				if (newShader != null)
				{
					m.shader = newShader;
				}
				else
				{
					Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
				}
			}

			ModAssets = AssetBundleLoader.LoadAssetBundleFromLiterallyAnywhere("modassets");

			//ENV_Tileset_Beyond = ModAssets.LoadAsset<Texture2D>("ENV_Tileset_Beyond");


			BeyondJammedShader = fucktilesets.LoadAsset<Shader>("BeyondJammedNoSF");

			//BeyondJammedShader.


			ENV_Tileset_Beyond = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/ENV_Tileset_Beyond.png");

			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
			shared_auto_001 = assetBundle;
			shared_auto_002 = assetBundle2;
			braveResources = ResourceManager.LoadAssetBundle("brave_resources_001");


			laserSight = (braveResources.LoadAsset("assets/resourcesbundle/global vfx/vfx_lasersight.prefab") as GameObject);

			if (ModAssets is null)
			{
				ETGModConsole.Log("ModAssets is null!");
			}

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
			

			doublebeholsterroom01 = BeyondDungeonFlows.LoadOfficialFlow("Secret_DoubleBeholster_Flow").AllNodes[2].overrideExactRoom;
			
			lostFigurePlaceable = FakePrefab.Clone(shared_auto_002.LoadAsset<GameObject>("RatFigure_Bullet"));
			lostFigurePlaceable.SetActive(false);
			var tksprite = lostFigurePlaceable.GetComponent<tk2dSprite>();
			tksprite.SetSprite(ItemAPI.SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/loststatuethatlooksawful", lostFigurePlaceable.GetComponent<tk2dSprite>().Collection));
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


			basicBeyondHands = ItemAPI.SpriteBuilder.SpriteFromResource("BotsMod/sprites/Enemies/hand.png", new GameObject("BeyondHand")).AddComponent<PlayerHandController>();
			FakePrefab.MarkAsFakePrefab(basicBeyondHands.gameObject);
			basicBeyondHands.ForceRenderersOff = false;
			var handSprite = basicBeyondHands.gameObject.GetComponent<tk2dSprite>();
			handSprite.Collection.spriteDefinitions[handSprite.spriteId].position0 = new Vector3(-0.125f, -0.125f, 0);
			handSprite.Collection.spriteDefinitions[handSprite.spriteId].position1 = new Vector3(0.125f, -0.125f, 0);
			handSprite.Collection.spriteDefinitions[handSprite.spriteId].position2 = new Vector3(-0.125f, 0.125f, 0);
			handSprite.Collection.spriteDefinitions[handSprite.spriteId].position3 = new Vector3(0.125f, 0.125f, 0);


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

			//ETGModConsole.Log($"{.GetComponent<UvStorer>().uvArray.Length}");
			//

			var marinePastPrefab = DungeonDatabase.GetOrLoadByName("Finalscenario_Soldier");


			var beyondCollectionObject = FakePrefab.Clone(marinePastPrefab.tileIndices.dungeonCollection.gameObject);
			beyondCollectionObject.name = "BeyondCollection";
			beyondCollection = beyondCollectionObject.GetComponent<tk2dSpriteCollectionData>();

			var baseObj = fucktilesets.LoadAsset<GameObject>("BeyondSpriteCollection");
			ETGModConsole.Log("obj done");
			foreach (Component component in baseObj.GetComponents<Component>())
			{
				ETGModConsole.Log(component.ToString());
				if (component.GetType().ToString().ToLower().Contains("tk2dspritecollectiondata"))
				{
					ETGModConsole.Log("comp done");
					tk2dSpriteDefinition[] uvArray = (tk2dSpriteDefinition[])ReflectionHelper.GetValue(component.GetType().GetField("spriteDefinitions"), component);

					ETGModConsole.Log(uvArray.Length.ToString());
					//var b = uvArray.ToList();
					//b.Sort();
					//uvArray = b.ToArray();

					var material = fucktilesets.LoadAsset<Material>("assets/tilesets/beyond/beyondspritecollection data/beyondtilesetmat.mat");
					ETGModConsole.Log("mat loaded");
					ETGModConsole.Log(beyondCollection.materials.Length.ToString());

					var mat1 = new Material(beyondCollection.materials[0]);
					var mat2 = new Material(beyondCollection.materials[1]);
					var mat3 = new Material(beyondCollection.materials[2]);

					beyondCollection.material = mat1;

					beyondCollection.materials = new Material[] { mat1, mat2, mat3 };
					ETGModConsole.Log("mat done");
					var tex = material.GetTexture("_MainTex");
					tex.filterMode = FilterMode.Point;
					mat1.SetTexture("_MainTex", tex);
					mat2.SetTexture("_MainTex", tex);
					mat3.SetTexture("_MainTex", tex);

					ETGModConsole.Log("tex loaded");
					beyondCollection.textures = new Texture[] { tex };
					ETGModConsole.Log("tex done");


					foreach (var def in uvArray)
					{
						bool isWall = (int.Parse(def.name) >= 22 && int.Parse(def.name) <= 28) || (int.Parse(def.name) >= 44 && int.Parse(def.name) <= 50) || (int.Parse(def.name) >= 44 && int.Parse(def.name) <= 50);
						beyondCollection.spriteDefinitions[int.Parse(def.name)].uvs = def.uvs.ToArray();
						beyondCollection.spriteDefinitions[int.Parse(def.name)].SetupTilesetSpriteDef(isWall, (int.Parse(def.name) >= 44 && int.Parse(def.name) <= 50));

						//beyondCollection.spriteDefinitions[] = def;
					}

					var backupDefs = beyondCollection.spriteDefinitions;

					beyondCollection.spriteDefinitions = new tk2dSpriteDefinition[704];

					foreach (var def in backupDefs)
                    {
						def.name = def.name.Replace("Final_Scenario_Tileset_Pilot/", "");
						//ETGModConsole.Log(def.name);
						beyondCollection.spriteDefinitions[int.Parse(def.name)] = def;
						

						def.name = "ENV_Beyond/" + def.name;
					}
					ETGModConsole.Log("reorder done");
					for (int i = 0; i < 704; i++)
					{
						beyondCollection.spriteDefinitions[i].material = beyondCollection.materials[0];
						beyondCollection.spriteDefinitions[i].materialId = 0;
						//ETGModConsole.Log($"[{i}] {beyondCollection.materials[beyondCollection.spriteDefinitions[i].materialId].name}");

					}

					beyondCollection.SetMaterial(0, 1);
					beyondCollection.SetMaterial(1, 1);
					beyondCollection.SetMaterial(2, 1);
					beyondCollection.SetMaterial(3, 1);
					beyondCollection.SetMaterial(4, 1);
					beyondCollection.SetMaterial(5, 1);
					beyondCollection.SetMaterial(6, 1);
					beyondCollection.SetMaterial(7, 1);
					beyondCollection.SetMaterial(8, 1);
					beyondCollection.SetMaterial(9, 1);
					beyondCollection.SetMaterial(10, 1);
					beyondCollection.SetMaterial(11, 1);
					beyondCollection.SetMaterial(12, 1);
					beyondCollection.SetMaterial(13, 1);
					beyondCollection.SetMaterial(14, 1);
					beyondCollection.SetMaterial(15, 1);
					beyondCollection.SetMaterial(16, 1);
					beyondCollection.SetMaterial(17, 1);
					beyondCollection.SetMaterial(18, 1);
					beyondCollection.SetMaterial(19, 1);
					beyondCollection.SetMaterial(20, 1);

					beyondCollection.SetMaterial(42, 1);
					beyondCollection.SetMaterial(43, 1);

					beyondCollection.SetMaterial(64, 1);
					beyondCollection.SetMaterial(65, 1);

					beyondCollection.SetMaterial(86, 1);
					beyondCollection.SetMaterial(87, 1);

					beyondCollection.SetMaterial(99, 1);
					beyondCollection.SetMaterial(100, 1);
					beyondCollection.SetMaterial(121, 1);
					beyondCollection.SetMaterial(122, 1);

					beyondCollection.SetMaterial(242, 2);
					beyondCollection.SetMaterial(243, 2);
					beyondCollection.SetMaterial(264, 2);
					beyondCollection.SetMaterial(265, 2);
					beyondCollection.SetMaterial(286, 2);
					beyondCollection.SetMaterial(308, 2);
					beyondCollection.SetMaterial(330, 2);
					beyondCollection.SetMaterial(352, 2);
					beyondCollection.SetMaterial(374, 2);
					beyondCollection.SetMaterial(396, 2);
					beyondCollection.SetMaterial(418, 2);
					beyondCollection.SetMaterial(528, 2);
					beyondCollection.SetMaterial(529, 2);
					beyondCollection.SetMaterial(530, 2);
					beyondCollection.SetMaterial(550, 2);



					beyondCollection.spriteDefinitions[25].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_LEFTEDGE, 1, 1);
					beyondCollection.spriteDefinitions[26].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, 1, 1);
					beyondCollection.spriteDefinitions[27].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, 0.05f, 1);
					beyondCollection.spriteDefinitions[28].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER_RIGHTEDGE, 1, 1);

					beyondCollection.spriteDefinitions[29].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, 0, 1);
					beyondCollection.spriteDefinitions[30].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, 0, 1);
					beyondCollection.spriteDefinitions[31].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER, 0, 1);

					beyondCollection.spriteDefinitions[27].metadata.usesAnimSequence = true;

					SimpleTilesetAnimationSequence wallEye = new SimpleTilesetAnimationSequence();

					wallEye.playstyle = SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.DELAYED_LOOP;
					wallEye.loopDelayMin = 6;
					wallEye.loopDelayMax = 25;
					wallEye.loopceptionTarget = -1;
					wallEye.loopceptionMin = 1;
					wallEye.loopceptionMax = 3;
					wallEye.coreceptionMin = 1;
					wallEye.coreceptionMax = 1;
					wallEye.randomStartFrame = true;
					wallEye.entries = new List<SimpleTilesetAnimationSequenceEntry>
					{
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 29,
							frameTime = 5f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 30,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 31,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 27,
							frameTime = 0.6f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 31,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 29,
							frameTime = 0.6f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 31,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 27,
							frameTime = 0.6f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 31,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 30,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 29,
							frameTime = 0.1f
						},
					};

					beyondCollection.SpriteIDsWithAnimationSequences.Add(27);
					beyondCollection.SpriteDefinedAnimationSequences.Add(wallEye);



					beyondCollection.spriteDefinitions[47].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_LEFTEDGE, 1, 1);
					beyondCollection.spriteDefinitions[48].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, 1, 1);
					beyondCollection.spriteDefinitions[49].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER, 1, 1);
					beyondCollection.spriteDefinitions[50].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER_RIGHTEDGE, 1, 1);

					beyondCollection.spriteDefinitions[58].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[59].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[60].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[61].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[80].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					//beyondCollection.spriteDefinitions[80].metadata.usesAnimSequence = true;
					beyondCollection.spriteDefinitions[81].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[82].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[83].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);

					beyondCollection.spriteDefinitions[99].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[100].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[121].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[122].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);

					beyondCollection.spriteDefinitions[102].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[103].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[104].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[105].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);

					beyondCollection.spriteDefinitions[124].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[125].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[126].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[127].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);

					//beyondCollection.spriteDefinitions[58].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[111].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE, 1, 1);

					beyondCollection.spriteDefinitions[112].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE, 0.01f, 1);

					beyondCollection.spriteDefinitions[112].metadata.usesAnimSequence = true;

					SimpleTilesetAnimationSequence floorEye = new SimpleTilesetAnimationSequence();

					floorEye.playstyle = SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.DELAYED_LOOP;
					floorEye.loopDelayMin = 6;
					floorEye.loopDelayMax = 25;
					floorEye.loopceptionTarget = -1;
					floorEye.loopceptionMin = 1;
					floorEye.loopceptionMax = 3;
					floorEye.coreceptionMin = 1;
					floorEye.coreceptionMax = 1;
					floorEye.randomStartFrame = true;
					floorEye.entries = new List<SimpleTilesetAnimationSequenceEntry>
					{
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 112,
							frameTime = 5f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 113,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 114,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 115,
							frameTime = 0.6f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 113,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 115,
							frameTime = 0.6f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 113,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 115,
							frameTime = 0.6f
						},

						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 114,
							frameTime = 0.1f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 113,
							frameTime = 0.2f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 112,
							frameTime = 0.1f
						},
					};

					beyondCollection.SpriteIDsWithAnimationSequences.Add(112);
					beyondCollection.SpriteDefinedAnimationSequences.Add(floorEye);

					beyondCollection.spriteDefinitions[113].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE, 0, 1);
					beyondCollection.spriteDefinitions[114].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE, 0, 1);
					beyondCollection.spriteDefinitions[115].metadata.SetupTileMetaData(TilesetIndexMetadata.TilesetFlagType.FLOOR_TILE, 0, 1);
					


					beyondCollection.spriteDefinitions[286].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[308].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[330].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);
					beyondCollection.spriteDefinitions[352].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 0);


					beyondCollection.spriteDefinitions[291].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[292].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[313].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[314].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[335].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[336].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[357].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[358].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[379].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[380].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[381].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[382].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[401].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[402].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[403].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[404].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[423].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[424].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[425].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[426].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[445].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[446].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[447].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[448].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[467].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[469].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[470].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[471].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[489].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[490].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[491].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[492].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[493].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[511].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[512].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[513].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[514].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[515].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					beyondCollection.spriteDefinitions[533].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[555].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[577].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);
					beyondCollection.spriteDefinitions[599].metadata.SetupTileMetaData((TilesetIndexMetadata.TilesetFlagType)0, 1, 1);

					var waterMat = new Material(castlePrefab.tileIndices.dungeonCollection.materials[5]);
					waterMat.SetColor("_CausticColor", new Color(0.4f, 0.11f, 0.41f, 0.672f));
					waterMat.SetTexture("_MainTex", tex);
					waterMat.SetTexture("_MaskTex", fucktilesets.LoadAsset<Texture2D>("atlasrefl0"));

					beyondCollection.spriteDefinitions[80].material = waterMat;
					beyondCollection.spriteDefinitions[80].materialId = 3;

					beyondCollection.spriteDefinitions[124].material = waterMat;
					beyondCollection.spriteDefinitions[124].materialId = 3;

					var ihatearrays = beyondCollection.materials.ToList();
					ihatearrays.Add(waterMat);
					beyondCollection.materials = ihatearrays.ToArray();

					SimpleTilesetAnimationSequence waterAnim = new SimpleTilesetAnimationSequence();
					
					waterAnim.playstyle = SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.SIMPLE_LOOP;
					waterAnim.loopDelayMin = 1;
					waterAnim.loopDelayMax = 3;
					waterAnim.loopceptionTarget = -1;
					waterAnim.loopceptionMin = 1;
					waterAnim.loopceptionMax = 3;
					waterAnim.coreceptionMin = 1;
					waterAnim.coreceptionMax = 1;
					waterAnim.randomStartFrame = false;
					waterAnim.entries = new List<SimpleTilesetAnimationSequenceEntry>
					{
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 80,
							frameTime = 0.3f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 81,
							frameTime = 0.3f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 82,
							frameTime = 0.3f
						},
						new SimpleTilesetAnimationSequenceEntry
						{
							entryIndex = 83,
							frameTime = 0.3f
						},
					};

					//beyondCollection.SpriteIDsWithAnimationSequences.Add(80);
					//beyondCollection.SpriteDefinedAnimationSequences.Add(waterAnim);




					break;
				}
			}

			beyondLootTable = LootTableAPI.LootTableTools.CreateLootTable();
			foreach (var item in Tools.BeyondItems)
			{
				if (item == BotsItemIds.SpinDownDice)
				{
					beyondLootTable.AddItemToPool(item, 0.5f);
				}
				else
				{
					beyondLootTable.AddItemToPool(item);
				}

			}


			


			string[] idleTableSpritesH = new string[] { "BotsMod/sprites/table/beyond/beyond_table_horizontal_idle_001.png" };
			string[] tableOutlinesH = new string[] { "BotsMod/sprites/table/beyond/beyond_table_horizontal_outline_top.png", "BotsMod/sprites/table/beyond/beyond_table_horizontal_outline_left.png", "BotsMod/sprites/table/beyond/beyond_table_horizontal_outline_right.png", "BotsMod/sprites/table/beyond/beyond_table_horizontal_outline_bottom.png" };


			//BreakAbleAPI.BreakableAPIToolbox.GenerateTable
			//(
			//	"Beyond",
			//	idleTableSpritesH,
			//	tableOutlinesH,


			//);
			marinePastPrefab = null;
			castlePrefab = null;

		}
	}	
}
