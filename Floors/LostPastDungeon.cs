using Dungeonator;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class LostPastDungeon
    {
        public static GameLevelDefinition LostPastDefinition;
        public static GameObject GameManagerObject;
        public static tk2dSpriteCollectionData gofuckyourself;

        public static Hook getOrLoadByName_Hook;
        public static void InitCustomDungeon()
        {
            getOrLoadByName_Hook = new Hook(
                typeof(DungeonDatabase).GetMethod("GetOrLoadByName", BindingFlags.Static | BindingFlags.Public),
                typeof(FloorHooks).GetMethod("GetOrLoadByNameHook", BindingFlags.Static | BindingFlags.Public)
            );


            AssetBundle braveResources = ResourceManager.LoadAssetBundle("brave_resources_001");
            GameManagerObject = braveResources.LoadAsset<GameObject>("_GameManager");

            LostPastDefinition = new GameLevelDefinition()
            {
                dungeonSceneName = "botfs_lost", //this is the name we will use whenever we want to load our dungeons scene
                dungeonPrefabPath = "Base_LostPast", //this is what we will use when we want to acess our dungeon prefab
                priceMultiplier = 1.5f, //multiplies how much things cost in the shop
                secretDoorHealthMultiplier = 1, //multiplies how much health secret room doors have, aka how many shots you will need to expose them
                enemyHealthMultiplier = 1, //multiplies how much health enemies have
                damageCap = 300, // damage cap for regular enemies
                bossDpsCap = 78, // damage cap for bosses
                flowEntries = new List<DungeonFlowLevelEntry>(0),
                predefinedSeeds = new List<int>(0)
            };

            // sets the level definition of the GameLevelDefinition in GameManager.Instance.customFloors if it exists
            foreach (GameLevelDefinition levelDefinition in GameManager.Instance.customFloors)
            {
                if (levelDefinition.dungeonSceneName == "botfs_lost") { LostPastDefinition = levelDefinition; }
            }

            GameManager.Instance.customFloors.Add(LostPastDefinition);
            GameManagerObject.GetComponent<GameManager>().customFloors.Add(LostPastDefinition);
        }

        public static Dungeon LostPastGeon(Dungeon dungeon)
        {
            Dungeon MinesDungeonPrefab = FloorHooks.GetOrLoadByName_Orig("Base_Mines");
            Dungeon CatacombsPrefab = FloorHooks.GetOrLoadByName_Orig("Base_Catacombs");
            Dungeon MarinePastPrefab = FloorHooks.GetOrLoadByName_Orig("Finalscenario_Soldier");
            Dungeon RatDungeonPrefab = FloorHooks.GetOrLoadByName_Orig("Base_ResourcefulRat");


            if (gofuckyourself == null)
            {
                gofuckyourself = MarinePastPrefab.tileIndices.dungeonCollection;
            }

            //DungeonMaterial FinalScenario_MainMaterial = UnityEngine.Object.Instantiate(RatDungeonPrefab.roomMaterialDefinitions[0]);
            DungeonMaterial FinalScenario_MainMaterial = UnityEngine.Object.Instantiate(MinesDungeonPrefab.roomMaterialDefinitions[0]);
            FinalScenario_MainMaterial.supportsPits = true;
            FinalScenario_MainMaterial.doPitAO = false;
            // FinalScenario_MainMaterial.pitsAreOneDeep = true;
            FinalScenario_MainMaterial.useLighting = true;
            // FinalScenario_MainMaterial.supportsLavaOrLavalikeSquares = true;
            FinalScenario_MainMaterial.lightPrefabs.elements[0].rawGameObject = MinesDungeonPrefab.roomMaterialDefinitions[0].lightPrefabs.elements[0].rawGameObject;
            FinalScenario_MainMaterial.roomFloorBorderGrid = MinesDungeonPrefab.roomMaterialDefinitions[0].roomFloorBorderGrid;
            FinalScenario_MainMaterial.pitLayoutGrid = MinesDungeonPrefab.roomMaterialDefinitions[0].pitLayoutGrid;
            FinalScenario_MainMaterial.pitBorderFlatGrid = MinesDungeonPrefab.roomMaterialDefinitions[0].pitBorderFlatGrid;

            DungeonTileStampData m_FloorNameStampData = ScriptableObject.CreateInstance<DungeonTileStampData>();
            m_FloorNameStampData.name = "ENV_LostPast_STAMP_DATA";
            m_FloorNameStampData.tileStampWeight = 0;
            m_FloorNameStampData.spriteStampWeight = 0;
            m_FloorNameStampData.objectStampWeight = 1;
            m_FloorNameStampData.stamps = new TileStampData[0];
            m_FloorNameStampData.spriteStamps = new SpriteStampData[0];
            m_FloorNameStampData.objectStamps = RatDungeonPrefab.stampData.objectStamps;
            m_FloorNameStampData.SymmetricFrameChance = 0.25f;
            m_FloorNameStampData.SymmetricCompleteChance = 0.6f;

            dungeon.gameObject.name = "Base_LostPast";
            dungeon.contentSource = ContentSource.CONTENT_UPDATE_03;
            dungeon.DungeonSeed = 0;
            dungeon.DungeonFloorName = "The LostPast."; // what shows up At the top when floor is loaded
            dungeon.DungeonShortName = "LostPast."; // no clue lol, just make it the same
            dungeon.DungeonFloorLevelTextOverride = "Chamber ???"; // what shows up below the floorname
            dungeon.LevelOverrideType = GameManager.LevelOverrideState.CHARACTER_PAST;
            dungeon.debugSettings = new DebugDungeonSettings()
            {
                RAPID_DEBUG_DUNGEON_ITERATION_SEEKER = false,
                RAPID_DEBUG_DUNGEON_ITERATION = false,
                RAPID_DEBUG_DUNGEON_COUNT = 50,
                GENERATION_VIEWER_MODE = true,
                FULL_MINIMAP_VISIBILITY = true,
                COOP_TEST = true,
                DISABLE_ENEMIES = true,
                DISABLE_LOOPS = false,
                DISABLE_SECRET_ROOM_COVERS = true,
                DISABLE_OUTLINES = false,
                WALLS_ARE_PITS = true
            };
            dungeon.ForceRegenerationOfCharacters = false;
            dungeon.ActuallyGenerateTilemap = true;

            if (BeyondPrefabs.ENV_Tileset_Beyond == null)
            {
                BeyondPrefabs.ENV_Tileset_Beyond = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/ENV_Tileset_Beyond.png");
            }

            dungeon.tileIndices = new TileIndices()
            {
                tilesetId = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND, //sets it to our floors CustomValidTileset

                //since the tileset im using here is a copy of the Rat dungeon tileset, the first variable in ReplaceDungeonCollection is RatDungeonPrefab.tileIndices.dungeonCollection,
                //otherwise we will use a different dungeon prefab
                dungeonCollection = gofuckyourself,//Tools.ReplaceDungeonCollection(RatDungeonPrefab.tileIndices.dungeonCollection, BeyondPrefabs.ENV_Tileset_Beyond),
                dungeonCollectionSupportsDiagonalWalls = false,
                aoTileIndices = RatDungeonPrefab.tileIndices.aoTileIndices,
                placeBorders = true,
                placePits = false,
                chestHighWallIndices = new List<TileIndexVariant>() {
                    new TileIndexVariant() {
                        index = 41,
                        likelihood = 0.5f,
                        overrideLayerIndex = 0,
                        overrideIndex = 0
                    }
                },
                decalIndexGrid = null,
                patternIndexGrid = RatDungeonPrefab.tileIndices.patternIndexGrid,
                globalSecondBorderTiles = new List<int>(0),
                edgeDecorationTiles = null
            };
            dungeon.tileIndices.dungeonCollection.name = "ENV_LostPast_Collection";
            dungeon.roomMaterialDefinitions = new DungeonMaterial[] {
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial,
                FinalScenario_MainMaterial
            };
            dungeon.dungeonWingDefinitions = new DungeonWingDefinition[0];

            //This section can be used to take parts from other floors and use them as our own.
            //we can make the running dust from one floor our own, the tables from another our own, 
            //we can use all of the stuff from the same floor, or if you want, you can make your own.
            dungeon.pathGridDefinitions = new List<TileIndexGrid>() { MinesDungeonPrefab.pathGridDefinitions[0] };
            dungeon.dungeonDustups = new DustUpVFX()
            {
                runDustup = MinesDungeonPrefab.dungeonDustups.runDustup,
                waterDustup = MinesDungeonPrefab.dungeonDustups.waterDustup,
                additionalWaterDustup = MinesDungeonPrefab.dungeonDustups.additionalWaterDustup,
                rollNorthDustup = MinesDungeonPrefab.dungeonDustups.rollNorthDustup,
                rollNorthEastDustup = MinesDungeonPrefab.dungeonDustups.rollNorthEastDustup,
                rollEastDustup = MinesDungeonPrefab.dungeonDustups.rollEastDustup,
                rollSouthEastDustup = MinesDungeonPrefab.dungeonDustups.rollSouthEastDustup,
                rollSouthDustup = MinesDungeonPrefab.dungeonDustups.rollSouthDustup,
                rollSouthWestDustup = MinesDungeonPrefab.dungeonDustups.rollSouthWestDustup,
                rollWestDustup = MinesDungeonPrefab.dungeonDustups.rollWestDustup,
                rollNorthWestDustup = MinesDungeonPrefab.dungeonDustups.rollNorthWestDustup,
                rollLandDustup = MinesDungeonPrefab.dungeonDustups.rollLandDustup
            };
            dungeon.PatternSettings = new SemioticDungeonGenSettings()
            {
                flows = new List<DungeonFlow>()
                {
                    //this will contain our dungeon flows after we make them
                    //LostPastDungeonFlows.F1b_LostPast_flow_01(),
                    BeyondDungeonFlows.F1b_LostPast_flow_01(),
                },
                mandatoryExtraRooms = new List<ExtraIncludedRoomData>(0),
                optionalExtraRooms = new List<ExtraIncludedRoomData>(0),
                MAX_GENERATION_ATTEMPTS = 250,
                DEBUG_RENDER_CANVASES_SEPARATELY = false
            };

            dungeon.damageTypeEffectMatrix = MinesDungeonPrefab.damageTypeEffectMatrix;
            dungeon.stampData = m_FloorNameStampData;
            dungeon.UsesCustomFloorIdea = false;
            dungeon.FloorIdea = new RobotDaveIdea()
            {
                ValidEasyEnemyPlaceables = new DungeonPlaceable[0],
                ValidHardEnemyPlaceables = new DungeonPlaceable[0],
                UseWallSawblades = false,
                UseRollingLogsVertical = true,
                UseRollingLogsHorizontal = true,
                UseFloorPitTraps = false,
                UseFloorFlameTraps = true,
                UseFloorSpikeTraps = true,
                UseFloorConveyorBelts = true,
                UseCaveIns = true,
                UseAlarmMushrooms = false,
                UseChandeliers = true,
                UseMineCarts = false,
                CanIncludePits = false
            };

            dungeon.decoSettings = new TilemapDecoSettings
            {

                decalExpansion = RatDungeonPrefab.decoSettings.decalExpansion,
                decalLayerStyle = RatDungeonPrefab.decoSettings.decalLayerStyle,
                decalSize = RatDungeonPrefab.decoSettings.decalSize,
                decoPatchFrequency = RatDungeonPrefab.decoSettings.decoPatchFrequency,
                decalSpacing = RatDungeonPrefab.decoSettings.decalSpacing,
                patternExpansion = RatDungeonPrefab.decoSettings.patternExpansion,
                patternLayerStyle = RatDungeonPrefab.decoSettings.patternLayerStyle,
                patternSize = RatDungeonPrefab.decoSettings.patternSize,
                patternSpacing = RatDungeonPrefab.decoSettings.patternSpacing,
                standardRoomVisualSubtypes = RatDungeonPrefab.decoSettings.standardRoomVisualSubtypes,


                ambientLightColor = new Color32(170, 150, 180, 255),
                ambientLightColorTwo = new Color32(169, 134, 180, 255),
                lowQualityAmbientLightColor = new Color32(255, 255, 255, 255),
                lowQualityAmbientLightColorTwo = new Color32(255, 255, 255, 255),
                lowQualityCheapLightVector = new Vector4(1, 0, -1, 0),

                UsesAlienFXFloorColor = true,
                AlienFXFloorColor = new Color32(228, 160, 250, 255),

                generateLights = true,
                lightCullingPercentage = 0.2f,
                lightOverlapRadius = 8,
                nearestAllowedLight = 12,
                minLightExpanseWidth = 2,
                lightHeight = -2,


                lightCookies = new Texture2D[0],
                debug_view = true,


            };

            //more variable we can copy from other floors, or make our own
            dungeon.PlaceDoors = true;
            dungeon.doorObjects = CatacombsPrefab.doorObjects;
            dungeon.oneWayDoorObjects = MinesDungeonPrefab.oneWayDoorObjects;
            dungeon.oneWayDoorPressurePlate = MinesDungeonPrefab.oneWayDoorPressurePlate;
            dungeon.phantomBlockerDoorObjects = MinesDungeonPrefab.phantomBlockerDoorObjects;
            dungeon.UsesWallWarpWingDoors = false;
            dungeon.baseChestContents = CatacombsPrefab.baseChestContents;
            dungeon.SecretRoomSimpleTriggersFacewall = new List<GameObject>() { CatacombsPrefab.SecretRoomSimpleTriggersFacewall[0] };
            dungeon.SecretRoomSimpleTriggersSidewall = new List<GameObject>() { CatacombsPrefab.SecretRoomSimpleTriggersSidewall[0] };
            dungeon.SecretRoomComplexTriggers = new List<ComplexSecretRoomTrigger>(0);
            dungeon.SecretRoomDoorSparkVFX = CatacombsPrefab.SecretRoomDoorSparkVFX;
            dungeon.SecretRoomHorizontalPoofVFX = CatacombsPrefab.SecretRoomHorizontalPoofVFX;
            dungeon.SecretRoomVerticalPoofVFX = CatacombsPrefab.SecretRoomVerticalPoofVFX;
            dungeon.sharedSettingsPrefab = CatacombsPrefab.sharedSettingsPrefab;
            dungeon.NormalRatGUID = string.Empty;
            dungeon.BossMasteryTokenItemId = -1;
            dungeon.UsesOverrideTertiaryBossSets = false;
            dungeon.OverrideTertiaryRewardSets = new List<TertiaryBossRewardSet>(0);
            dungeon.defaultPlayerPrefab = MinesDungeonPrefab.defaultPlayerPrefab;
            dungeon.StripPlayerOnArrival = true;
            dungeon.SuppressEmergencyCrates = false;
            dungeon.SetTutorialFlag = false;
            dungeon.PlayerIsLight = true;
            dungeon.PlayerLightColor = CatacombsPrefab.PlayerLightColor;
            dungeon.PlayerLightIntensity = 4;
            dungeon.PlayerLightRadius = 4;
            dungeon.PrefabsToAutoSpawn = new GameObject[0];


            //include this for custom floor audio
            //dungeon.musicEventName = "play_sound"; 


            CatacombsPrefab = null;
            RatDungeonPrefab = null;
            MinesDungeonPrefab = null;

            return dungeon;
        }



    }
}
