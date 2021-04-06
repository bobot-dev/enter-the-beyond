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

namespace BotsMod
{
    class LostsPast : SpecialDungeon
    {
        public override Dungeon BuildDungeon(Dungeon dungeon)
        {

            

            dungeon.gameObject.name = "LostsPast";
            dungeon.LevelOverrideType = GameManager.LevelOverrideState.CHARACTER_PAST;
            dungeon.contentSource = ContentSource.BASE;
            dungeon.DungeonShortName = "The Past";
            dungeon.DungeonFloorName = "???";
            dungeon.DungeonFloorLevelTextOverride = "The Past (1602 Years Prior)";
            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_LostsPast", "\"This seems wrong\"");
            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_LostsPast_FLOOR_TEXT", "LostsPast");
            //BotsModMod.Strings.Core.Set("#BOT_DUNGEON_LostsPast_SHORT", "LostsPast");
            dungeon.PatternSettings = new SemioticDungeonGenSettings
            {
                DEBUG_RENDER_CANVASES_SEPARATELY = dungeon.PatternSettings.DEBUG_RENDER_CANVASES_SEPARATELY,
                flows = new List<DungeonFlow> { LostsPastFlow.BuildFlow() },
                mandatoryExtraRooms = dungeon.PatternSettings.mandatoryExtraRooms,
                MAX_GENERATION_ATTEMPTS = dungeon.PatternSettings.MAX_GENERATION_ATTEMPTS,
                optionalExtraRooms = dungeon.PatternSettings.optionalExtraRooms
                
            };
            //dungeon.tileIndices.tilesetId = GlobalDungeonData.ValidTilesets.HELLGEON;
            //dungeon.IsEndTimes = true;
            dungeon.OverrideAmbientColor = Color.magenta;
            dungeon.OverrideAmbientLight = false;
            //dungeon.DungeonFloorLevelTextOverride = "The Past (1602 Years Prior)";
            //dungeon.override
            ENV_Tileset_Beyond_Texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\ENV_Tileset_Sewer.png");
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

            dungeon.musicEventName = "Play_MUS_funnysong";

            return dungeon;

        }

        public override float BossDPSCap => -1f;
        public override float DamageCap => -1f;
        public override float EnemyHealthMultiplier => 1f;
        public override List<DungeonFlowLevelEntry> FlowEntries => base.FlowEntries;
        public override string PrefabPath => "LostsPast";
        public override float PriceMultiplier => 1f;
        public override string SceneName => "botfs_lost";
        public override float SecretDoorHealthMultiplier => 1f;

        public static Texture2D ENV_Tileset_Beyond_Texture;

        Dungeon orLoadByName_Orig3 = GetOrLoadByNameOrig("FinalScenario_Pilot");
       
        Dungeon orLoadByName_Orig4 = GetOrLoadByNameOrig("FinalScenario_Bullet");

        Dungeon orLoadByName_Orig5 = GetOrLoadByNameOrig("base_sewer");
    }
   
    

    class LostsPastFlow
    {
        public static DungeonFlow BuildFlow()
        {
            DungeonFlow flow = ScriptableObject.CreateInstance<DungeonFlow>();
            flow.name = "LostsPastFlow";
            flow.fallbackRoomTable = null;
            flow.phantomRoomTable = null;
            flow.subtypeRestrictions = new List<DungeonFlowSubtypeRestriction>(0);
            flow.flowInjectionData = new List<ProceduralFlowModifierData>(0);
            flow.sharedInjectionData = new List<SharedInjectionData>() { Tools.shared_auto_002.LoadAsset<SharedInjectionData>("Base Shared Injection Data") };

            var bossRoom = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastBossRoom2.room").room;
            
            //bossRoom.

            DungeonFlowNode node = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.ENTRANCE, RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostPastEntrace.room").room);
            DungeonFlowNode node2 = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.BOSS, bossRoom);

            //DungeonFlowNode node3 = Tools.GenerateFlowNode(flow, PrototypeDungeonRoom.RoomCategory.EXIT, RoomFactory.BuildFromResource("BotsMod/rooms/Swordtress/SwordtressExit.room").room);
            flow.Initialize();
            flow.AddNodeToFlow(node, null);
            flow.AddNodeToFlow(node2, node);
            //flow.AddNodeToFlow(node3, node2);
            flow.FirstNode = node;
            return flow;
        }
    }

}
