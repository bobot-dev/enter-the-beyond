using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;

namespace BotsMod
{
    class BeyondDungeonFlows
    {
        public static DungeonFlow LoadCustomFlow(Func<string, DungeonFlow> orig, string target)
        {
            string flowName = target;
            if (flowName.Contains("/")) { flowName = target.Substring(target.LastIndexOf("/") + 1); }
            if (KnownFlows != null && KnownFlows.Count > 0)
            {
                foreach (DungeonFlow flow in KnownFlows)
                {
                    if (flow.name != null && flow.name != string.Empty)
                    {
                        if (flowName.ToLower() == flow.name.ToLower())
                        {
                            DebugTime.RecordStartTime();
                            DebugTime.Log("AssetBundle.LoadAsset<DungeonFlow>({0})", new object[] { flowName });
                            return flow;
                        }
                    }
                }
            }
            return orig(target);
        }

        public static DungeonFlow LoadOfficialFlow(string target)
        {
            string flowName = target;
            if (flowName.Contains("/")) { flowName = target.Substring(target.LastIndexOf("/") + 1); }
            AssetBundle m_assetBundle_orig = ResourceManager.LoadAssetBundle("flows_base_001");
            DebugTime.RecordStartTime();
            DungeonFlow result = m_assetBundle_orig.LoadAsset<DungeonFlow>(flowName);
            DebugTime.Log("AssetBundle.LoadAsset<DungeonFlow>({0})", new object[] { flowName });
            if (result == null)
            {
                Debug.Log("ERROR: Requested DungeonFlow not found!\nCheck that you provided correct DungeonFlow name and that it actually exists!");
                m_assetBundle_orig = null;
                return null;
            }
            else
            {
                m_assetBundle_orig = null;
                return result;
            }
        }

        public static List<DungeonFlow> KnownFlows;

        public static DungeonFlow Foyer_Flow;

        // Default stuff to use with custom Flows
        public static SharedInjectionData BaseSharedInjectionData;
        public static SharedInjectionData GungeonInjectionData;
        public static SharedInjectionData SewersInjectionData;
        public static SharedInjectionData HollowsInjectionData;
        public static SharedInjectionData CastleInjectionData;

        public static ProceduralFlowModifierData SecretBeyondEntranceInjector;

        public static DungeonFlowSubtypeRestriction BaseSubTypeRestrictions = new DungeonFlowSubtypeRestriction()
        {
            baseCategoryRestriction = PrototypeDungeonRoom.RoomCategory.NORMAL,
            normalSubcategoryRestriction = PrototypeDungeonRoom.RoomNormalSubCategory.TRAP,
            bossSubcategoryRestriction = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS,
            specialSubcategoryRestriction = PrototypeDungeonRoom.RoomSpecialSubCategory.UNSPECIFIED_SPECIAL,
            secretSubcategoryRestriction = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET,
            maximumRoomsOfSubtype = 1
        };

        // Custom Room Table for Keep Shared Injection Data 
        public static GenericRoomTable m_KeepEntranceRooms;

        // Generate a DungeonFlowNode with a default configuration
        public static DungeonFlowNode GenerateDefaultNode(DungeonFlow targetflow, PrototypeDungeonRoom.RoomCategory roomType, PrototypeDungeonRoom overrideRoom = null, GenericRoomTable overrideTable = null, bool oneWayLoopTarget = false, bool isWarpWingNode = false, string nodeGUID = null, DungeonFlowNode.NodePriority priority = DungeonFlowNode.NodePriority.MANDATORY, float percentChance = 1, bool handlesOwnWarping = true)
        {
            try
            {
                if (string.IsNullOrEmpty(nodeGUID)) { nodeGUID = Guid.NewGuid().ToString(); }

                DungeonFlowNode m_CachedNode = new DungeonFlowNode(targetflow)
                {
                    isSubchainStandin = false,
                    nodeType = DungeonFlowNode.ControlNodeType.ROOM,
                    roomCategory = roomType,
                    percentChance = percentChance,
                    priority = priority,
                    overrideExactRoom = overrideRoom,
                    overrideRoomTable = overrideTable,
                    capSubchain = false,
                    subchainIdentifier = string.Empty,
                    limitedCopiesOfSubchain = false,
                    maxCopiesOfSubchain = 1,
                    subchainIdentifiers = new List<string>(0),
                    receivesCaps = false,
                    isWarpWingEntrance = isWarpWingNode,
                    handlesOwnWarping = handlesOwnWarping,
                    forcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                    loopForcedDoorType = DungeonFlowNode.ForcedDoorType.NONE,
                    nodeExpands = false,
                    initialChainPrototype = "n",
                    chainRules = new List<ChainRule>(0),
                    minChainLength = 3,
                    maxChainLength = 8,
                    minChildrenToBuild = 1,
                    maxChildrenToBuild = 1,
                    canBuildDuplicateChildren = false,
                    guidAsString = nodeGUID,
                    parentNodeGuid = string.Empty,
                    childNodeGuids = new List<string>(0),
                    loopTargetNodeGuid = string.Empty,
                    loopTargetIsOneWay = oneWayLoopTarget,
                    flow = targetflow
                };



                return m_CachedNode;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }
        }


        // Retrieve sharedInjectionData from a specific floor if one is available
        public static List<SharedInjectionData> RetrieveSharedInjectionDataListFromCurrentFloor()
        {
            Dungeon dungeon = GameManager.Instance.CurrentlyGeneratingDungeonPrefab;

            if (dungeon == null)
            {
                dungeon = GameManager.Instance.Dungeon;
                if (dungeon == null) { return new List<SharedInjectionData>(0); }

            }

            if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FINALGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON |
                dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                return new List<SharedInjectionData>(0);
            }

            List<SharedInjectionData> m_CachedInjectionDataList = new List<SharedInjectionData>(0);

            if (dungeon.PatternSettings != null && dungeon.PatternSettings.flows != null && dungeon.PatternSettings.flows.Count > 0)
            {
                if (dungeon.PatternSettings.flows[0].sharedInjectionData != null && dungeon.PatternSettings.flows[0].sharedInjectionData.Count > 0)
                {
                    m_CachedInjectionDataList = dungeon.PatternSettings.flows[0].sharedInjectionData;
                }
            }

            return m_CachedInjectionDataList;
        }

        public static ProceduralFlowModifierData RickRollSecretRoomInjector;

        public static SharedInjectionData CustomSecretFloorSharedInjectionData;


        // Initialize KnownFlows array with custom + official flows.
        public static void InitDungeonFlows(bool refreshFlows = false)
        {

            Dungeon TutorialPrefab = DungeonDatabase.GetOrLoadByName("Base_Tutorial");
            Dungeon CastlePrefab = DungeonDatabase.GetOrLoadByName("Base_Castle");
            Dungeon SewerPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");
            Dungeon GungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_Gungeon");
            Dungeon CathedralPrefab = DungeonDatabase.GetOrLoadByName("Base_Cathedral");
            Dungeon MinesPrefab = DungeonDatabase.GetOrLoadByName("Base_Mines");
            Dungeon ResourcefulRatPrefab = DungeonDatabase.GetOrLoadByName("Base_ResourcefulRat");
            Dungeon CatacombsPrefab = DungeonDatabase.GetOrLoadByName("Base_Catacombs");
            Dungeon NakatomiPrefab = DungeonDatabase.GetOrLoadByName("Base_Nakatomi");
            Dungeon ForgePrefab = DungeonDatabase.GetOrLoadByName("Base_Forge");
            Dungeon BulletHellPrefab = DungeonDatabase.GetOrLoadByName("Base_BulletHell");

            BaseSharedInjectionData = BeyondPrefabs.shared_auto_002.LoadAsset<SharedInjectionData>("Base Shared Injection Data");
            GungeonInjectionData = GungeonPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            SewersInjectionData = SewerPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            HollowsInjectionData = CatacombsPrefab.PatternSettings.flows[0].sharedInjectionData[1];
            CastleInjectionData = CastlePrefab.PatternSettings.flows[0].sharedInjectionData[0];


            m_KeepEntranceRooms = ScriptableObject.CreateInstance<GenericRoomTable>();
            m_KeepEntranceRooms.includedRoomTables = new List<GenericRoomTable>(0);
            m_KeepEntranceRooms.includedRooms = new WeightedRoomCollection()
            {
                elements = new List<WeightedRoom>()
                {
                    //ModRoomPrefabs.GenerateWeightedRoom(ModRoomPrefabs.Mod_Entrance_Room, Weight: 1f)
                }
            };
            //Boss_Cathedral_StainedGlass_Lights

            SecretBeyondEntranceInjector = new ProceduralFlowModifierData()
            {
                annotation = "Secret Floor Entrance Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
                    ProceduralFlowModifierData.FlowModifierPlacementType.RANDOM_NODE_CHILD
                },
                roomTable = m_KeepEntranceRooms,
                // exactRoom = SewersInjectionData.InjectionData[0].exactRoom,
                exactRoom = null,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[0],
                CanBeForcedSecret = true,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            };

           

            CastleInjectionData.InjectionData.Add(SecretBeyondEntranceInjector);

            // Don't build/add flows until injection data is created!
            Foyer_Flow = FlowHelpers.DuplicateDungeonFlow(BeyondPrefabs.shared_auto_002.LoadAsset<DungeonFlow>("Foyer Flow"));

            // List<DungeonFlow> m_knownFlows = new List<DungeonFlow>();
            KnownFlows = new List<DungeonFlow>();

            //we will add our custom flow here soon.
            KnownFlows.Add(F1b_Beyond_flow_01());
            KnownFlows.Add(F1b_Beyond_flow_Overseer_Test_01());
            KnownFlows.Add(F1b_LostPast_flow_01());


            // Fix issues with nodes so that things other then MainMenu can load Foyer flow
            Foyer_Flow.name = "Foyer_Flow";
            Foyer_Flow.AllNodes[1].handlesOwnWarping = true;
            Foyer_Flow.AllNodes[2].handlesOwnWarping = true;
            Foyer_Flow.AllNodes[3].handlesOwnWarping = true;

            KnownFlows.Add(Foyer_Flow);

            // Add official flows to list (flows found in Dungeon asset bundles after AG&D)
            for (int i = 0; i < TutorialPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(TutorialPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CastlePrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CastlePrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < SewerPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(SewerPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < GungeonPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(GungeonPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CathedralPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CathedralPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < MinesPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(MinesPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < ResourcefulRatPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(ResourcefulRatPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < CatacombsPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(CatacombsPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < NakatomiPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(NakatomiPrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < ForgePrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(ForgePrefab.PatternSettings.flows[i]));
            }
            for (int i = 0; i < BulletHellPrefab.PatternSettings.flows.Count; i++)
            {
                KnownFlows.Add(FlowHelpers.DuplicateDungeonFlow(BulletHellPrefab.PatternSettings.flows[i]));
            }


            TutorialPrefab = null;
            CastlePrefab = null;
            SewerPrefab = null;
            GungeonPrefab = null;
            CathedralPrefab = null;
            MinesPrefab = null;
            ResourcefulRatPrefab = null;
            CatacombsPrefab = null;
            NakatomiPrefab = null;
            ForgePrefab = null;
            BulletHellPrefab = null;
        }

        public static DungeonFlow F1b_Beyond_flow_01()
        {
            try
            {

                DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();

                DungeonFlowNode entranceNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, ModRoomPrefabs.Mod_Entrance_Room);
                DungeonFlowNode exitNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.EXIT, ModRoomPrefabs.Mod_Exit_Room);

                DungeonFlowNode bossfoyerNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.SPECIAL, overrideTable: BeyondPrefabs.boss_foyertable);
                DungeonFlowNode bossNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.BOSS, ModRoomPrefabs.Mod_Boss);

                DungeonFlowNode BeyondShopNode = GenerateDefaultNode(m_CachedFlow, BeyondPrefabs.shop02.category, overrideTable: BeyondPrefabs.shop_room_table);

                DungeonFlowNode BeyondRewardNode_01 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.REWARD, BeyondPrefabs.gungeon_rewardroom_1);
                DungeonFlowNode BeyondRewardNode_02 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.REWARD, BeyondPrefabs.gungeon_rewardroom_1);

                DungeonFlowNode BeyondHubNode_01 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB, oneWayLoopTarget: true);
                DungeonFlowNode BeyondHubNode_02 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB, oneWayLoopTarget: true);
                DungeonFlowNode BeyondHubNode_03 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB);
                DungeonFlowNode BeyondHubNode_04 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.HUB);

                DungeonFlowNode BeyondShopNode_01 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.SPECIAL, ModRoomPrefabs.Mod_Shop_Room);

                DungeonFlowNode BeyondRoomNode_01 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                //DungeonFlowNode BeyondRoomNode_02 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_03 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_04 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_05 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_06 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_07 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_08 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_09 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_10 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_11 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);               
                DungeonFlowNode BeyondRoomNode_12 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_13 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_14 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_15 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_16 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_17 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_18 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_19 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_20 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_21 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_22 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_23 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);
                DungeonFlowNode BeyondRoomNode_24 = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.NORMAL);

                m_CachedFlow.name = "F1b_Beyond_Flow_01";
                //m_CachedFlow.fallbackRoomTable = BeyondPrefabs.BeyondRoomTable;
                m_CachedFlow.fallbackRoomTable = BeyondPrefabs.CastleRoomTable;
                m_CachedFlow.phantomRoomTable = null;
                m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
                m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
                m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData };

                m_CachedFlow.Initialize();

                m_CachedFlow.AddNodeToFlow(entranceNode, null);

                
                
               



                //section 1

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_01, entranceNode);
                m_CachedFlow.AddNodeToFlow(BeyondHubNode_01, BeyondRoomNode_01);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_03, BeyondHubNode_01);


                m_CachedFlow.AddNodeToFlow(BeyondHubNode_02, BeyondRoomNode_03);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_04, BeyondHubNode_02);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_05, BeyondRoomNode_04);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_06, BeyondRoomNode_05);
               // 

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_07, BeyondHubNode_02);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_08, BeyondRoomNode_07);
                m_CachedFlow.AddNodeToFlow(BeyondRewardNode_01, BeyondRoomNode_08);

                

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_09, BeyondRoomNode_08);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_10, BeyondHubNode_02);

                m_CachedFlow.LoopConnectNodes(BeyondRewardNode_01, BeyondRoomNode_10);

                m_CachedFlow.ConnectNodes(BeyondRoomNode_10, BeyondRoomNode_09);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_11, BeyondRoomNode_10);



                //section 2

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_11, entranceNode);
                m_CachedFlow.ConnectNodes(BeyondRoomNode_11, entranceNode);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_12, BeyondRoomNode_11);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_13, BeyondRoomNode_12);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_14, BeyondRoomNode_13);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_15, BeyondRoomNode_14);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_16, BeyondRoomNode_15);
                m_CachedFlow.AddNodeToFlow(BeyondRewardNode_02, BeyondRoomNode_16);


                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_17, BeyondRoomNode_11);



                //section 3

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_18, entranceNode);
                m_CachedFlow.ConnectNodes(BeyondRoomNode_18, entranceNode);

                m_CachedFlow.ConnectNodes(BeyondRoomNode_17, BeyondRoomNode_18);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_19, BeyondRoomNode_18);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_19, BeyondRoomNode_18);
                m_CachedFlow.AddNodeToFlow(BeyondHubNode_03, BeyondRoomNode_19);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_20, BeyondHubNode_03);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_21, BeyondRoomNode_20);
                m_CachedFlow.AddNodeToFlow(BeyondShopNode_01, BeyondRoomNode_21);

                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_22, BeyondHubNode_03);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_23, BeyondRoomNode_22);
                m_CachedFlow.AddNodeToFlow(BeyondRoomNode_24, BeyondRoomNode_23);

                m_CachedFlow.AddNodeToFlow(bossfoyerNode, BeyondRoomNode_24);
                m_CachedFlow.AddNodeToFlow(bossNode, bossfoyerNode);
                m_CachedFlow.AddNodeToFlow(exitNode, bossNode);

                m_CachedFlow.FirstNode = entranceNode;

                return m_CachedFlow;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }



        }

        public static DungeonFlow F1b_LostPast_flow_01()
        {
            try
            {

                DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();

                DungeonFlowNode entranceNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, ModRoomPrefabs.Mod_Entrance_Room_Past);
                DungeonFlowNode bossNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.BOSS, ModRoomPrefabs.Mod_Boss_Past);

                m_CachedFlow.name = "F1b_LostPast_flow_01";
                //m_CachedFlow.fallbackRoomTable = BeyondPrefabs.BeyondRoomTable;
                m_CachedFlow.fallbackRoomTable = BeyondPrefabs.CastleRoomTable;
                m_CachedFlow.phantomRoomTable = null;
                m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
                m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
                m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData };

                m_CachedFlow.Initialize();

                m_CachedFlow.AddNodeToFlow(entranceNode, null);

                m_CachedFlow.AddNodeToFlow(bossNode, entranceNode);

                m_CachedFlow.FirstNode = entranceNode;

                return m_CachedFlow;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }



        }

        public static DungeonFlow F1b_Beyond_flow_Overseer_Test_01()
        {
            try
            {

                DungeonFlow m_CachedFlow = ScriptableObject.CreateInstance<DungeonFlow>();

                DungeonFlowNode entranceNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, ModRoomPrefabs.Mod_Entrance_Room);
                DungeonFlowNode exitNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.EXIT, ModRoomPrefabs.Mod_Exit_Room);

                DungeonFlowNode bossfoyerNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.SPECIAL, overrideTable: BeyondPrefabs.boss_foyertable);
                DungeonFlowNode bossNode = GenerateDefaultNode(m_CachedFlow, PrototypeDungeonRoom.RoomCategory.BOSS, ModRoomPrefabs.Mod_Boss);

                m_CachedFlow.name = "F1b_Beyond_flow_Overseer_Test_01";
                //m_CachedFlow.fallbackRoomTable = BeyondPrefabs.BeyondRoomTable;
                m_CachedFlow.fallbackRoomTable = BeyondPrefabs.CastleRoomTable;
                m_CachedFlow.phantomRoomTable = null;
                m_CachedFlow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
                m_CachedFlow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
                m_CachedFlow.sharedInjectionData = new List<SharedInjectionData>() { BaseSharedInjectionData };

                m_CachedFlow.Initialize();

                m_CachedFlow.AddNodeToFlow(entranceNode, null);

                m_CachedFlow.AddNodeToFlow(bossfoyerNode, entranceNode);
                m_CachedFlow.AddNodeToFlow(bossNode, bossfoyerNode);
                m_CachedFlow.AddNodeToFlow(exitNode, bossNode);

                m_CachedFlow.FirstNode = entranceNode;

                return m_CachedFlow;
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
                return null;
            }



        }
    }
}

