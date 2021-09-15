using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using Pathfinding;
using UnityEngine.SceneManagement;
using GungeonAPI;
using BotsMod;
using SaveAPI;

namespace BotsMod
{
    class TheBeyond : SpecialDungeon
    {
        public override Dungeon BuildDungeon(Dungeon dungeon)
        {



            dungeon.gameObject.name = "TheBeyond";
            dungeon.LevelOverrideType = GameManager.LevelOverrideState.NONE;
            dungeon.contentSource = ContentSource.BASE;
            dungeon.DungeonShortName = "Beyond";
            dungeon.DungeonFloorName = "The Beyond";
            dungeon.DungeonFloorLevelTextOverride = "Chamber ???";

            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_TheBeyond", "\"This seems wrong\"");
            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_TheBeyond_FLOOR_TEXT", "TheBeyond");
            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_TheBeyond_SHORT", "TheBeyond");
            dungeon.PatternSettings = new SemioticDungeonGenSettings
            {
                DEBUG_RENDER_CANVASES_SEPARATELY = dungeon.PatternSettings.DEBUG_RENDER_CANVASES_SEPARATELY,
                flows = new List<DungeonFlow> { TheBeyondFlow.BuildFlow() },
                //flows = new List<DungeonFlow> { OfficialFlows.GetDungeonPrefab("Base_Sewer").PatternSettings.flows[0] },
                mandatoryExtraRooms = dungeon.PatternSettings.mandatoryExtraRooms,
                MAX_GENERATION_ATTEMPTS = dungeon.PatternSettings.MAX_GENERATION_ATTEMPTS,
                optionalExtraRooms = dungeon.PatternSettings.optionalExtraRooms

            };
            //dungeon.tileIndices.tilesetId = GlobalDungeonData.ValidTilesets.HELLGEON;
            //dungeon.IsEndTimes = true;
            dungeon.BossMasteryTokenItemId = BotsItemIds.BeyondMasteryToken;
            //dungeon.DungeonFloorLevelTextOverride = "The Past (1602 Years Prior)";
            //dungeon.override
            ENV_Tileset_Beyond_Texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\ENV_Tileset_Beyond.png");
            dungeon.tileIndices = new TileIndices
            {
                tilesetId = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND,



                dungeonCollection = Tools.ReplaceDungeonCollection(orLoadByName_Orig5.tileIndices.dungeonCollection, ENV_Tileset_Beyond_Texture, null),
                dungeonCollectionSupportsDiagonalWalls = false,
                aoTileIndices = orLoadByName_Orig5.tileIndices.aoTileIndices,
                placeBorders = true,
                placePits = false,
                chestHighWallIndices = new List<TileIndexVariant>
                {
                    new TileIndexVariant
                    {
                        index = 41,
                        likelihood = 0.5f,
                        overrideLayerIndex = 0,
                        overrideIndex = 0
                    }
                },
                decalIndexGrid = null,
                patternIndexGrid = orLoadByName_Orig5.tileIndices.patternIndexGrid,
                globalSecondBorderTiles = new List<int>(0),
                edgeDecorationTiles = null
            };

            dungeon.debugSettings = new DebugDungeonSettings()
            {
                RAPID_DEBUG_DUNGEON_ITERATION_SEEKER = false,
                RAPID_DEBUG_DUNGEON_ITERATION = false,
                RAPID_DEBUG_DUNGEON_COUNT = 50,
                GENERATION_VIEWER_MODE = true,
                FULL_MINIMAP_VISIBILITY = true,
                COOP_TEST = false,
                DISABLE_ENEMIES = true,
                DISABLE_LOOPS = false,
                DISABLE_SECRET_ROOM_COVERS = true,
                DISABLE_OUTLINES = true,
                WALLS_ARE_PITS = true
            };

            dungeon.OverrideAmbientColor = Color.magenta;
            dungeon.OverrideAmbientLight = false;

            dungeon.decoSettings = orLoadByName_Orig5.decoSettings;

            dungeon.decoSettings.ambientLightColor = new Color32(118, 102, 163, 255);
            dungeon.decoSettings.ambientLightColorTwo = new Color32(118, 117, 119, 255);
            dungeon.decoSettings.lowQualityAmbientLightColor = new Color32(233, 196, 255, 255);
            dungeon.decoSettings.lowQualityAmbientLightColorTwo = new Color32(216, 194, 219, 255);
            dungeon.decoSettings.generateLights = false;



            dungeon.decoSettings.debug_view = true;
            return dungeon;

        }

        public override float BossDPSCap => 80.0f;
        public override float DamageCap => 300.0f;
        public override float EnemyHealthMultiplier => 2.1f;
        public override List<DungeonFlowLevelEntry> FlowEntries => base.FlowEntries;
        public override string PrefabPath => "TheBeyond";
        public override float PriceMultiplier => 2.0f;
        public override string SceneName => "beyond";
        public override float SecretDoorHealthMultiplier => 1f;

        public static Texture2D ENV_Tileset_Beyond_Texture;

        Dungeon orLoadByName_Orig5 = GetOrLoadByNameOrig("Base_ResourcefulRat");
        Dungeon orLoadByName_Orig6 = GetOrLoadByNameOrig("base_forge");
    }



    class TheBeyondFlow
    {

        public static SharedInjectionData BeyondInjectionData;

        public static DungeonFlow BuildFlow()
        {


            Dungeon SewerPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");


            var BaseSharedInjectionData = Tools.shared_auto_002.LoadAsset<SharedInjectionData>("Base Shared Injection Data");
            DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();

            //DungeonFlow flow = SewerPrefab.PatternSettings.flows[0];


            if (!BeyondInjectionData)
            {
                BeyondInjectionData = ScriptableObject.CreateInstance<SharedInjectionData>();
                BeyondInjectionData.name = "Beyond Common Injection Data";
                BeyondInjectionData.UseInvalidWeightAsNoInjection = true;
                BeyondInjectionData.PreventInjectionOfFailedPrerequisites = false;
                BeyondInjectionData.IsNPCCell = false;
                BeyondInjectionData.IgnoreUnmetPrerequisiteEntries = false;
                BeyondInjectionData.OnlyOne = false;
                BeyondInjectionData.ChanceToSpawnOne = 1f;
                BeyondInjectionData.AttachedInjectionData = new List<SharedInjectionData>(0);
                BeyondInjectionData.InjectionData = new List<ProceduralFlowModifierData>()
                {


                    new ProceduralFlowModifierData()
                    {
                        annotation = "Secret Room Lost Unlock Room",
                        DEBUG_FORCE_SPAWN = true,
                        OncePerRun = false,
                        placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                        {
                            ProceduralFlowModifierData.FlowModifierPlacementType.RANDOM_NODE_CHILD
                        },
                        roomTable = null,
                        // exactRoom = SewersInjectionData.InjectionData[0].exactRoom,
                        exactRoom = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostUnlockRoom.room"),
                        IsWarpWing = false,
                        RequiresMasteryToken = false,
                        chanceToLock = 0,
                        selectionWeight = 1,
                        chanceToSpawn = 1,
                        RequiredValidPlaceable = null,
                        CanBeForcedSecret = true,
                        RandomNodeChildMinDistanceFromEntrance = 0,
                        exactSecondaryRoom = null,
                        framedCombatNodes = 0,

                        prerequisites = new DungeonPrerequisite[]
                        {
                            new DungeonPrerequisite()
                            {
                                prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
                                statToCheck = TrackedStats.TIMES_REACHED_GUNGEON,
                                maxToCheck = 0,
                                comparisonValue = 0,
                                encounteredObjectGuid = string.Empty,
                                encounteredRoom = null,
                                requiredNumberOfEncounters = 0,
                                requiredCharacter = PlayableCharacters.Pilot,
                                requireCharacter = false,
                                requiredTileset = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND,
                                requireTileset = true,
                                saveFlagToCheck = 0,
                                requireFlag = false,
                                requireDemoMode = false
                            }
                        }
                    }
                };

            }

            DungeonFlowNode entranceNode = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.ENTRANCE, RoomPrefabs.Expand_Beyond_Entrance);
            DungeonFlowNode exitNode = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.EXIT, RoomPrefabs.Expand_Beyond_Exit);
            DungeonFlowNode bossfoyerNode = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.SPECIAL, overrideTable: BeyondPrefabs.boss_foyertable);
            DungeonFlowNode bossNode = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.BOSS, RoomPrefabs.Expand_Beyond_Boss);

            DungeonFlowNode BeyondShopNode = m_CachedFlow.GenerateDefaultNode(BeyondPrefabs.shop02.category, overrideTable: BeyondPrefabs.shop_room_table);
            DungeonFlowNode BeyondRewardNode_01 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.CONNECTOR, BeyondPrefabs.gungeon_rewardroom_1);
            DungeonFlowNode BeyondRewardNode_02 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.CONNECTOR, BeyondPrefabs.gungeon_rewardroom_1);


            DungeonFlowNode BeyondRoomNode_01 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.HUB, oneWayLoopTarget: true);
            DungeonFlowNode BeyondRoomNode_02 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_04 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_05 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_06 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.HUB, oneWayLoopTarget: true);
            DungeonFlowNode BeyondRoomNode_07 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_09 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_10 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_11 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.HUB);
            DungeonFlowNode BeyondRoomNode_12 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_13 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_14 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_16 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_17 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);
            DungeonFlowNode BeyondRoomNode_18 = m_CachedFlow.GenerateDefaultNode(PrototypeDungeonRoom.RoomCategory.NORMAL);

            m_CachedFlow.name = "F1b_Beyond_Flow_02";
            m_CachedFlow.fallbackRoomTable = BeyondPrefabs.SewerRoomTable;
            m_CachedFlow.phantomRoomTable = null;
            m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
            m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
            //m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData, BeyondInjectionData };
            m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData};
            m_CachedFlow.Initialize();

            m_CachedFlow.AddNodeToFlow(entranceNode, null);

            // First Looping branch
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_16, entranceNode);
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_01, BeyondRoomNode_16);
            // Dead End
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_05, BeyondRoomNode_01);
            // Start of Loop
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_02, BeyondRoomNode_01);
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_04, BeyondRoomNode_02);
            m_CachedFlow.AddNodeToFlow(BeyondRewardNode_01, BeyondRoomNode_04);
            // Connect End of Loop to first in chain
            m_CachedFlow.LoopConnectNodes(BeyondRewardNode_01, BeyondRoomNode_01);

            // Second Looping branch
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_17, entranceNode);
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_06, BeyondRoomNode_17);
            // Dead End
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_10, BeyondRoomNode_06);
            // Start of Loop
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_07, BeyondRoomNode_06);
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_09, BeyondRoomNode_07);
            m_CachedFlow.AddNodeToFlow(BeyondRewardNode_02, BeyondRoomNode_09);
            // Connect End of Loop to first in chain
            m_CachedFlow.LoopConnectNodes(BeyondRewardNode_02, BeyondRoomNode_06);

            // Splitting path to Shop or Boss
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_18, entranceNode);
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_11, BeyondRoomNode_18);
            // Path To Boss
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_12, BeyondRoomNode_11);
            // Path to Shop
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_13, BeyondRoomNode_11);
            m_CachedFlow.AddNodeToFlow(BeyondShopNode, BeyondRoomNode_13);
            // Dead End
            m_CachedFlow.AddNodeToFlow(BeyondRoomNode_14, BeyondRoomNode_11);


            m_CachedFlow.AddNodeToFlow(bossfoyerNode, BeyondRoomNode_12);
            m_CachedFlow.AddNodeToFlow(bossNode, bossfoyerNode);
            m_CachedFlow.AddNodeToFlow(exitNode, bossNode);

            m_CachedFlow.FirstNode = entranceNode;

            return m_CachedFlow;

        }
    }
}
