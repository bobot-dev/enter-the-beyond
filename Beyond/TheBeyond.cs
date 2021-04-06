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
                //flows = new List<DungeonFlow> { TheBeyondFlow.BuildFlow() },
                flows = new List<DungeonFlow> { OfficialFlows.GetDungeonPrefab("Base_Sewer").PatternSettings.flows[0] },
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

            //TheBeyondFlow.SetUpFlow(dungeon.PatternSettings.flows);
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
        public static void SetUpFlow(List<DungeonFlow> flows)
        {
            var SecretRoomLostUnlockInjector = new ProceduralFlowModifierData()
            {
                annotation = "Secret Room Lost Unlock Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
                    ProceduralFlowModifierData.FlowModifierPlacementType.NO_LINKS
                },
                roomTable = null,
                // exactRoom = SewersInjectionData.InjectionData[0].exactRoom,
                //exactRoom = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostUnlockRoom.room").room,
                exactRoom = Rooms.SecretLostUnlockRoom,
                IsWarpWing = true,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 100000,
                chanceToSpawn = 100,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[] {
                    new DungeonPrerequisite() {
                        prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                        requiredTileset = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND,
                        requireTileset = false,
                        comparisonValue = 1,
                        encounteredObjectGuid = string.Empty,
                        maxToCheck = TrackedMaximums.MOST_KEYS_HELD,
                        requireDemoMode = false,
                        requireCharacter = false,
                        requiredCharacter = PlayableCharacters.Pilot,
                        requireFlag = true,
                        useSessionStatValue = false,
                        encounteredRoom = null,
                        requiredNumberOfEncounters = -1,
                        saveFlagToCheck = (GungeonFlags)CustomDungeonFlags.BOT_HAS_ROBE,
                        statToCheck = TrackedStats.GUNBERS_MUNCHED
                    }
                },
                CanBeForcedSecret = true,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            };
            flows[0].sharedInjectionData[1].InjectionData.Add(SecretRoomLostUnlockInjector);
            //flows[1].sharedInjectionData[1].InjectionData.Add(SecretRoomLostUnlockInjector);
            //flows[2].sharedInjectionData[1].InjectionData.Add(SecretRoomLostUnlockInjector);
        }

        public static DungeonFlow BuildFlow()
        {


            Dungeon SewerPrefab = DungeonDatabase.GetOrLoadByName("Base_Sewer");

            var SewersInjectionData = SewerPrefab.PatternSettings.flows[0].sharedInjectionData;


           // DungeonFlow flow = ScriptableObject.CreateInstance<DungeonFlow>();
            DungeonFlow flow = SewerPrefab.PatternSettings.flows[0];

            //flow.name = "TheBeyondFlow";
            //flow.fallbackRoomTable = null;
            //flow.phantomRoomTable = null;
            //flow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
            //flow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
            //flow.sharedInjectionData = new List<SharedInjectionData>() { Tools.shared_auto_002.LoadAsset<SharedInjectionData>("Base Shared Injection Data") };
            //flow.sharedInjectionData = SewersInjectionData;
            flow.Initialize();
            return flow;
            var SecretRoomLostUnlockInjector = new ProceduralFlowModifierData()
            {
                annotation = "Secret Room Lost Unlock Room",
                DEBUG_FORCE_SPAWN = false,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
                    ProceduralFlowModifierData.FlowModifierPlacementType.NO_LINKS
                },
                roomTable = null,
                // exactRoom = SewersInjectionData.InjectionData[0].exactRoom,
                exactRoom = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostUnlockRoom.room").room,
                IsWarpWing = true,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 100,
                chanceToSpawn = 100,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[] {
                    new DungeonPrerequisite() {
                        prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                        requiredTileset = GlobalDungeonData.ValidTilesets.GUNGEON,
                        requireTileset = true,
                        comparisonValue = 1,
                        encounteredObjectGuid = string.Empty,
                        maxToCheck = TrackedMaximums.MOST_KEYS_HELD,
                        requireDemoMode = false,
                        requireCharacter = false,
                        requiredCharacter = PlayableCharacters.Pilot,
                        requireFlag = true,
                        useSessionStatValue = false,
                        encounteredRoom = null,
                        requiredNumberOfEncounters = -1,
                        saveFlagToCheck = (GungeonFlags)CustomDungeonFlags.BOT_HAS_ROBE,
                        statToCheck = TrackedStats.GUNBERS_MUNCHED
                    }
                },
                CanBeForcedSecret = true,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,
            };
            //flow.sharedInjectionData[1].InjectionData.Add(SecretRoomLostUnlockInjector);
            
            /*
            DungeonFlowNode node = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastEntrace.room").room);
            DungeonFlowNode node2 = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.BOSS, RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastBossRoom2.room").room);
            DungeonFlowNode secretRoomLost = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.SECRET, RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostUnlockRoom.room").room);
            //DungeonFlowNode node3 = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.EXIT, RoomFactory.BuildFromResource("BotsMod/rooms/Swordtress/SwordtressExit.room").room);
            
            flow.AddNodeToFlow(node, null);
            flow.AddNodeToFlow(node2, node);
            flow.AddNodeToFlow(secretRoomLost, null);
            flow.FirstNode = node;*/
            
        }
    }

}
