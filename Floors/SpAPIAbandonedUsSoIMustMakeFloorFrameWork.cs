/*using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SpAPIAbandonedUsSoIMustMakeFloorFrameWork
    {
        public static Dungeon BellyDungeon(Dungeon dungeon)
        {
            AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            AssetBundle sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");
            Dungeon MinesDungeonPrefab = Tools.GetOrLoadByName_Orig("Base_Mines");
            Dungeon GungeonPrefab = Tools.GetOrLoadByName_Orig("Base_Gungeon");
            Dungeon SewersPrefab = Tools.GetOrLoadByName_Orig("Base_Sewer");
            Dungeon AbbeyPrefab = Tools.GetOrLoadByName_Orig("Base_Cathedral");

            DungeonMaterial BellyMaterial = ScriptableObject.CreateInstance<DungeonMaterial>();
            BellyMaterial.name = "Belly";
            BellyMaterial.wallShards = GungeonPrefab.roomMaterialDefinitions[0].wallShards;
            BellyMaterial.bigWallShards = GungeonPrefab.roomMaterialDefinitions[0].bigWallShards;
            BellyMaterial.bigWallShardDamageThreshold = 10;
            BellyMaterial.fallbackVerticalTileMapEffects = GungeonPrefab.roomMaterialDefinitions[0].fallbackVerticalTileMapEffects;
            BellyMaterial.fallbackHorizontalTileMapEffects = GungeonPrefab.roomMaterialDefinitions[0].fallbackHorizontalTileMapEffects;
            BellyMaterial.pitfallVFXPrefab = null;
            BellyMaterial.UsePitAmbientVFX = false;
            BellyMaterial.AmbientPitVFX = new List<GameObject>(0);
            BellyMaterial.PitVFXMinCooldown = 5;
            BellyMaterial.PitVFXMaxCooldown = 30;
            BellyMaterial.ChanceToSpawnPitVFXOnCooldown = 1;
            BellyMaterial.stampFailChance = 0.2f;
            BellyMaterial.overrideTableTable = null;
            BellyMaterial.supportsPits = true;
            BellyMaterial.doPitAO = false; // was True
            BellyMaterial.pitsAreOneDeep = false;
            BellyMaterial.supportsDiagonalWalls = false;
            BellyMaterial.supportsUpholstery = false;
            BellyMaterial.carpetIsMainFloor = false;
            BellyMaterial.supportsChannels = false;
            BellyMaterial.minChannelPools = 0;
            BellyMaterial.maxChannelPools = 3;
            BellyMaterial.channelTenacity = 0.75f;
            BellyMaterial.channelGrids = new TileIndexGrid[0];
            BellyMaterial.supportsLavaOrLavalikeSquares = false;
            BellyMaterial.carpetGrids = new TileIndexGrid[] { ExpandUtility.DeserializeTileIndexGrid("BellyAssets/carpetGrid1.txt") };
            BellyMaterial.lavaGrids = new TileIndexGrid[] { ExpandUtility.DeserializeTileIndexGrid("BellyAssets/lavaGrid.txt") };
            BellyMaterial.supportsIceSquares = false;
            BellyMaterial.iceGrids = new TileIndexGrid[0];
            BellyMaterial.roomFloorBorderGrid = ExpandUtility.DeserializeTileIndexGrid("BellyAssets/roomFloorBorderGrid.txt");
            BellyMaterial.roomCeilingBorderGrid = ExpandUtility.DeserializeTileIndexGrid("BellyAssets/roomCeilingBorderGrid.txt");
            BellyMaterial.pitLayoutGrid = ExpandUtility.DeserializeTileIndexGrid("BellyAssets/pitLayoutGrid.txt");
            BellyMaterial.pitBorderFlatGrid = ExpandUtility.DeserializeTileIndexGrid("BellyAssets/pitBorderFlatGrid.txt");
            BellyMaterial.pitBorderRaisedGrid = null;
            BellyMaterial.additionalPitBorderFlatGrid = null;
            BellyMaterial.outerCeilingBorderGrid = null;
            BellyMaterial.floorSquareDensity = 0.05f;
            BellyMaterial.floorSquares = new TileIndexGrid[0];
            BellyMaterial.usesFacewallGrids = false;
            BellyMaterial.facewallGrids = new FacewallIndexGridDefinition[] {
                new FacewallIndexGridDefinition() {
                    grid = ExpandUtility.DeserializeTileIndexGrid("BellyAssets/faceWallGrid1.txt"),
                    minWidth = 3,
                    maxWidth = 20,
                    hasIntermediaries = true,
                    minIntermediaryBuffer = 4,
                    maxIntermediaryBuffer = 6,
                    minIntermediaryLength = 1,
                    maxIntermediaryLength = 3,
                    topsMatchBottoms = true,
                    middleSectionSequential = false,
                    canExistInCorners = false,
                    forceEdgesInCorners = false,
                    canAcceptWallDecoration = false,
                    canAcceptFloorDecoration = true,
                    forcedStampMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY,
                    canBePlacedInExits = true,
                    chanceToPlaceIfPossible = 0.15f,
                    perTileFailureRate = 0.05f
                }
            };
            BellyMaterial.usesInternalMaterialTransitions = false;
            BellyMaterial.usesProceduralMaterialTransitions = false;
            BellyMaterial.internalMaterialTransitions = new RoomInternalMaterialTransition[0];
            BellyMaterial.secretRoomWallShardCollections = new List<GameObject>(0);
            BellyMaterial.overrideStoneFloorType = false;
            BellyMaterial.overrideFloorType = CellVisualData.CellFloorType.Stone;
            BellyMaterial.useLighting = true;
            BellyMaterial.lightPrefabs = new WeightedGameObjectCollection()
            {
                elements = new List<WeightedGameObject>() {
                   new WeightedGameObject() {
                       rawGameObject = ExpandPrefabs.BellyLight,
                       weight = 1,
                       forceDuplicatesPossible = false,
                       pickupId = -1,
                       additionalPrerequisites = new DungeonPrerequisite[0]
                   }
               }
            };
            BellyMaterial.facewallLightStamps = GungeonPrefab.roomMaterialDefinitions[0].facewallLightStamps;
            BellyMaterial.sidewallLightStamps = GungeonPrefab.roomMaterialDefinitions[0].sidewallLightStamps;
            BellyMaterial.usesDecalLayer = false;
            BellyMaterial.decalIndexGrid = null;
            BellyMaterial.decalLayerStyle = TilemapDecoSettings.DecoStyle.GROW_FROM_WALLS;
            BellyMaterial.decalSize = 1;
            BellyMaterial.decalSpacing = 1;
            BellyMaterial.patternLayerStyle = TilemapDecoSettings.DecoStyle.NONE;
            BellyMaterial.patternSpacing = 1;
            BellyMaterial.patternSize = 1;
            BellyMaterial.patternIndexGrid = null;
            BellyMaterial.forceEdgesDiagonal = false;
            BellyMaterial.exteriorFacadeBorderGrid = null;
            BellyMaterial.facadeTopGrid = null;
            BellyMaterial.bridgeGrid = null;


            DungeonTileStampData m_BellyStampData = ScriptableObject.CreateInstance<DungeonTileStampData>();
            m_BellyStampData.name = "ENV_BELLY_STAMP_DATA";
            m_BellyStampData.tileStampWeight = 1;
            m_BellyStampData.spriteStampWeight = 0;
            m_BellyStampData.objectStampWeight = 1;
            m_BellyStampData.stamps = new TileStampData[0];
            m_BellyStampData.spriteStamps = new SpriteStampData[0];
            m_BellyStampData.objectStamps = new ObjectStampData[] {
                new ObjectStampData() {
                    width = 1,
                    height = 1,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 2,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets.LoadAsset<GameObject>("Big_Skull_001")
                },
                new ObjectStampData() {
                    width = 1,
                    height = 1,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 2,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets2.LoadAsset<GameObject>("Big_Skull_002")
                },
                new ObjectStampData() {
                    width = 1,
                    height = 1,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 2,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets2.LoadAsset<GameObject>("Big_Skull_003")
                },
                new ObjectStampData() {
                    width = 2,
                    height = 1,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 2,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets.LoadAsset<GameObject>("Skull_Pile_001")
                },
                new ObjectStampData() {
                    width = 1,
                    height = 2,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 4,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets.LoadAsset<GameObject>("Skeleton_Left_Sit_Corner")
                },
                new ObjectStampData() {
                    width = 1,
                    height = 2,
                    relativeWeight = 1,
                    placementRule = DungeonTileStampData.StampPlacementRule.BELOW_LOWER_FACEWALL,
                    occupySpace = DungeonTileStampData.StampSpace.OBJECT_SPACE,
                    stampCategory = DungeonTileStampData.StampCategory.NATURAL,
                    preferredIntermediaryStamps = 4,
                    intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.SKELETON,
                    requiresForcedMatchingStyle = false,
                    opulence = Opulence.PLAIN,
                    roomTypeData = new List<StampPerRoomPlacementSettings>(0),
                    indexOfSymmetricPartner = -1,
                    preventRoomRepeats = false,
                    objectReference = sharedAssets2.LoadAsset<GameObject>("Skeleton_Right_Sit_Corner")
                },
            };

            // 
            m_BellyStampData.SymmetricFrameChance = 0.1f;
            m_BellyStampData.SymmetricCompleteChance = 0.1f;

            dungeon.gameObject.name = "Base_Belly";
            dungeon.contentSource = ContentSource.CONTENT_UPDATE_03;
            dungeon.DungeonSeed = 0;
            dungeon.DungeonFloorName = "Inside the Beast";
            dungeon.DungeonShortName = "Inside the Beast";
            dungeon.DungeonFloorLevelTextOverride = "A Disgusting Place...";
            dungeon.LevelOverrideType = GameManager.LevelOverrideState.NONE;
            dungeon.debugSettings = new DebugDungeonSettings()
            {
                RAPID_DEBUG_DUNGEON_ITERATION_SEEKER = false,
                RAPID_DEBUG_DUNGEON_ITERATION = false,
                RAPID_DEBUG_DUNGEON_COUNT = 50,
                GENERATION_VIEWER_MODE = false,
                FULL_MINIMAP_VISIBILITY = false,
                COOP_TEST = false,
                DISABLE_ENEMIES = false,
                DISABLE_LOOPS = false,
                DISABLE_SECRET_ROOM_COVERS = false,
                DISABLE_OUTLINES = false,
                WALLS_ARE_PITS = false
            };

            dungeon.PatternSettings = new SemioticDungeonGenSettings()
            {
                flows = new List<DungeonFlow>() { f2b_belly_flow_01.F2b_Belly_Flow_01() },
                mandatoryExtraRooms = new List<ExtraIncludedRoomData>(0),
                optionalExtraRooms = new List<ExtraIncludedRoomData>(0),
                MAX_GENERATION_ATTEMPTS = 250,
                DEBUG_RENDER_CANVASES_SEPARATELY = false
            };
            dungeon.ForceRegenerationOfCharacters = false;
            dungeon.ActuallyGenerateTilemap = true;
            dungeon.decoSettings = new TilemapDecoSettings
            {
                standardRoomVisualSubtypes = new WeightedIntCollection
                {
                    elements = new WeightedInt[] {
                        new WeightedInt() {
                            annotation = "belly",
                            value = 0,
                            weight = 1f,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        },
                        new WeightedInt() {
                            annotation = "unused",
                            value = 1,
                            weight = 0,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        },
                        new WeightedInt() {
                            annotation = "shop",
                            value = 2,
                            weight = 0,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        },
                        new WeightedInt() {
                            annotation = "unused",
                            value = 3,
                            weight = 0,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        },
                        new WeightedInt() {
                            annotation = "unused",
                            value = 4,
                            weight = 0,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        }
                    }
                },
                decalLayerStyle = TilemapDecoSettings.DecoStyle.NONE,
                decalSize = 3,
                decalSpacing = 1,
                decalExpansion = 0,
                patternLayerStyle = TilemapDecoSettings.DecoStyle.NONE,
                patternSize = 3,
                patternSpacing = 3,
                patternExpansion = 0,
                decoPatchFrequency = 0.01f,
                ambientLightColor = new Color(0.925355f, 1f, 0.661765f, 1),
                ambientLightColorTwo = new Color(0.92549f, 1f, 0.662745f, 1),
                lowQualityAmbientLightColor = new Color(1, 1, 1, 1),
                lowQualityAmbientLightColorTwo = new Color(1, 1, 1, 1),
                lowQualityCheapLightVector = new Vector4(1, 0, -1, 0),
                UsesAlienFXFloorColor = false,
                AlienFXFloorColor = new Color(0, 0, 0, 1),
                generateLights = true,
                lightCullingPercentage = 0.2f,
                lightOverlapRadius = 8,
                nearestAllowedLight = 12,
                minLightExpanseWidth = 2,
                lightHeight = -2,
                lightCookies = new Texture2D[0],
                debug_view = false
            };

            dungeon.tileIndices = new TileIndices()
            {
                tilesetId = GlobalDungeonData.ValidTilesets.BELLYGEON,
                // dungeonCollection = ExpandDungeonCollections.ENV_Tileset_Belly(dungeon.gameObject),
                dungeonCollection = ExpandPrefabs.ENV_Tileset_Belly.GetComponent<tk2dSpriteCollectionData>(),
                dungeonCollectionSupportsDiagonalWalls = false,
                aoTileIndices = new AOTileIndices()
                {
                    AOFloorTileIndex = 0,
                    AOBottomWallBaseTileIndex = 1,
                    AOBottomWallTileRightIndex = 2,
                    AOBottomWallTileLeftIndex = 3,
                    AOBottomWallTileBothIndex = 4,
                    AOTopFacewallRightIndex = 6,
                    AOTopFacewallLeftIndex = 5,
                    AOTopFacewallBothIndex = 7,
                    AOFloorWallLeft = 5,
                    AOFloorWallRight = 6,
                    AOFloorWallBoth = 7,
                    AOFloorPizzaSliceLeft = 8,
                    AOFloorPizzaSliceRight = 9,
                    AOFloorPizzaSliceBoth = 10,
                    AOFloorPizzaSliceLeftWallRight = 11,
                    AOFloorPizzaSliceRightWallLeft = 12,
                    AOFloorWallUpAndLeft = 13,
                    AOFloorWallUpAndRight = 14,
                    AOFloorWallUpAndBoth = 15,
                    AOFloorDiagonalWallNortheast = -1,
                    AOFloorDiagonalWallNortheastLower = -1,
                    AOFloorDiagonalWallNortheastLowerJoint = -1,
                    AOFloorDiagonalWallNorthwest = -1,
                    AOFloorDiagonalWallNorthwestLower = -1,
                    AOFloorDiagonalWallNorthwestLowerJoint = -1,
                    AOBottomWallDiagonalNortheast = -1,
                    AOBottomWallDiagonalNorthwest = -1
                },
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
                patternIndexGrid = null,
                globalSecondBorderTiles = new List<int>(0),
                edgeDecorationTiles = null
            };

            dungeon.roomMaterialDefinitions = new DungeonMaterial[] {
                BellyMaterial,
                BellyMaterial,
                BellyMaterial,
                BellyMaterial,
                sharedAssets2.LoadAsset<DungeonMaterial>("Boss_Cathedral_StainedGlass_Lights")
            };
            dungeon.dungeonWingDefinitions = new DungeonWingDefinition[0];
            dungeon.pathGridDefinitions = new List<TileIndexGrid>() { MinesDungeonPrefab.pathGridDefinitions[0] };
            dungeon.dungeonDustups = new DustUpVFX()
            {
                runDustup = GungeonPrefab.dungeonDustups.runDustup,
                waterDustup = GungeonPrefab.dungeonDustups.waterDustup,
                additionalWaterDustup = GungeonPrefab.dungeonDustups.additionalWaterDustup,
                rollNorthDustup = GungeonPrefab.dungeonDustups.rollNorthDustup,
                rollNorthEastDustup = GungeonPrefab.dungeonDustups.rollNorthEastDustup,
                rollEastDustup = GungeonPrefab.dungeonDustups.rollEastDustup,
                rollSouthEastDustup = GungeonPrefab.dungeonDustups.rollSouthEastDustup,
                rollSouthDustup = GungeonPrefab.dungeonDustups.rollSouthDustup,
                rollSouthWestDustup = GungeonPrefab.dungeonDustups.rollSouthWestDustup,
                rollWestDustup = GungeonPrefab.dungeonDustups.rollWestDustup,
                rollNorthWestDustup = GungeonPrefab.dungeonDustups.rollNorthWestDustup,
                rollLandDustup = GungeonPrefab.dungeonDustups.rollLandDustup
            };
            dungeon.damageTypeEffectMatrix = GungeonPrefab.damageTypeEffectMatrix;
            dungeon.stampData = m_BellyStampData;
            dungeon.UsesCustomFloorIdea = false;
            dungeon.FloorIdea = new RobotDaveIdea()
            {
                ValidEasyEnemyPlaceables = new DungeonPlaceable[0],
                ValidHardEnemyPlaceables = new DungeonPlaceable[0],
                UseWallSawblades = false,
                UseRollingLogsVertical = false,
                UseRollingLogsHorizontal = false,
                UseFloorPitTraps = false,
                UseFloorFlameTraps = false,
                UseFloorSpikeTraps = false,
                UseFloorConveyorBelts = false,
                UseCaveIns = false,
                UseAlarmMushrooms = false,
                UseChandeliers = false,
                UseMineCarts = false,
                CanIncludePits = true
            };
            dungeon.PlaceDoors = true;
            dungeon.doorObjects = ExpandPrefabs.Belly_Doors;
            dungeon.oneWayDoorObjects = AbbeyPrefab.oneWayDoorObjects;
            dungeon.oneWayDoorPressurePlate = AbbeyPrefab.oneWayDoorPressurePlate;
            dungeon.phantomBlockerDoorObjects = AbbeyPrefab.phantomBlockerDoorObjects;
            dungeon.UsesWallWarpWingDoors = false;
            dungeon.baseChestContents = AbbeyPrefab.baseChestContents;
            dungeon.SecretRoomSimpleTriggersFacewall = new List<GameObject>() { SewersPrefab.SecretRoomSimpleTriggersFacewall[0] };
            dungeon.SecretRoomSimpleTriggersSidewall = new List<GameObject>() { SewersPrefab.SecretRoomSimpleTriggersSidewall[0] };
            dungeon.SecretRoomComplexTriggers = new List<ComplexSecretRoomTrigger>(0);
            dungeon.SecretRoomDoorSparkVFX = GungeonPrefab.SecretRoomDoorSparkVFX;
            dungeon.SecretRoomHorizontalPoofVFX = GungeonPrefab.SecretRoomHorizontalPoofVFX;
            dungeon.SecretRoomVerticalPoofVFX = GungeonPrefab.SecretRoomVerticalPoofVFX;
            dungeon.sharedSettingsPrefab = AbbeyPrefab.sharedSettingsPrefab;
            dungeon.NormalRatGUID = string.Empty;
            dungeon.BossMasteryTokenItemId = -1;
            dungeon.UsesOverrideTertiaryBossSets = false;
            dungeon.OverrideTertiaryRewardSets = new List<TertiaryBossRewardSet>(0);
            dungeon.defaultPlayerPrefab = AbbeyPrefab.defaultPlayerPrefab;
            dungeon.StripPlayerOnArrival = false;
            dungeon.SuppressEmergencyCrates = false;
            dungeon.SetTutorialFlag = false;
            dungeon.PlayerIsLight = false;
            dungeon.PlayerLightColor = new Color(1, 1, 1, 1);
            dungeon.PlayerLightIntensity = 3;
            dungeon.PlayerLightRadius = 5;
            dungeon.PrefabsToAutoSpawn = new GameObject[0];
            dungeon.musicEventName = AbbeyPrefab.musicEventName;

            sharedAssets = null;
            sharedAssets2 = null;
            MinesDungeonPrefab = null;
            GungeonPrefab = null;
            AbbeyPrefab = null;

            Debug.Log("End Belly Construction...");

            return dungeon;
        }
    }
}
*/