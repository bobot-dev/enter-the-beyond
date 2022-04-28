using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class BeyondDungeon
    {
        public static GameLevelDefinition BeyondDefinition;
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

            BeyondDefinition = new GameLevelDefinition()
            {
                dungeonSceneName = "tt_beyond", //this is the name we will use whenever we want to load our dungeons scene
                dungeonPrefabPath = "Base_Beyond", //this is what we will use when we want to acess our dungeon prefab
                priceMultiplier = 1.5f, //multiplies how much things cost in the shop
                secretDoorHealthMultiplier = 1, //multiplies how much health secret room doors have, aka how many shots you will need to expose them
                enemyHealthMultiplier = 2, //multiplies how much health enemies have
                damageCap = 300, // damage cap for regular enemies
                bossDpsCap = 78, // damage cap for bosses
                flowEntries = new List<DungeonFlowLevelEntry>(0),
                predefinedSeeds = new List<int>(0)
            };

            // sets the level definition of the GameLevelDefinition in GameManager.Instance.customFloors if it exists
            foreach (GameLevelDefinition levelDefinition in GameManager.Instance.customFloors)
            {
                if (levelDefinition.dungeonSceneName == "tt_beyond") { BeyondDefinition = levelDefinition; }
            }

            GameManager.Instance.customFloors.Add(BeyondDefinition);
            GameManagerObject.GetComponent<GameManager>().customFloors.Add(BeyondDefinition);
        }

        public static Dungeon BeyondGeon(Dungeon dungeon)
        {
            Debug.Log("beyond setup 1");
            Dungeon CatacombsPrefab = DungeonDatabase.GetOrLoadByName("Base_Catacombs");
            Dungeon MarinePastPrefab = DungeonDatabase.GetOrLoadByName("Finalscenario_Soldier");
            Dungeon RatDungeonPrefab = DungeonDatabase.GetOrLoadByName("Base_ResourcefulRat");

            if (gofuckyourself == null)
            {
                gofuckyourself = MarinePastPrefab.tileIndices.dungeonCollection;
            }

            
            //DungeonMaterial FinalScenario_MainMaterial = UnityEngine.Object.Instantiate(RatDungeonPrefab.roomMaterialDefinitions[0]);
            DungeonMaterial FinalScenario_MainMaterial = UnityEngine.Object.Instantiate(MarinePastPrefab.roomMaterialDefinitions[0]);
            FinalScenario_MainMaterial.supportsPits = true;
            //FinalScenario_MainMaterial.doPitAO = true;
            // FinalScenario_MainMaterial.pitsAreOneDeep = true;
            FinalScenario_MainMaterial.useLighting = true;

            FinalScenario_MainMaterial.supportsDiagonalWalls = true;

            var pitGridCave = Tools.CreateBlankIndexGrid();
            pitGridCave.topLeftIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.topIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.topRightIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };

            pitGridCave.leftIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.centerIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.rightIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.bottomLeftIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.bottomIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.bottomRightIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };


            pitGridCave.horizontalIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.verticalIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.topCapIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.rightCapIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.leftCapIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };
            pitGridCave.allSidesIndices = new TileIndexList { indices = new List<int> { 58 }, indexWeights = new List<float> { 1f } };

            pitGridCave.extendedSet = true;
            pitGridCave.topCenterLeftIndices = new TileIndexList { indices = new List<int> { 80 }, indexWeights = new List<float> { 1f } };
            pitGridCave.topCenterIndices = new TileIndexList { indices = new List<int> { 80 }, indexWeights = new List<float> { 1f } };
            pitGridCave.topCenterRightIndices = new TileIndexList { indices = new List<int> { 80 }, indexWeights = new List<float> { 1f } };

            pitGridCave.thirdTopRowLeftIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.thirdTopRowCenterIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.thirdTopRowRightIndices = new TileIndexList { indices = new List<int> { 124 }, indexWeights = new List<float> { 1f } };
            pitGridCave.CheckerboardDimension = 1;
            pitGridCave.PitInternalSquareOptions = new PitSquarePlacementOptions { CanBeFlushBottom = false, CanBeFlushLeft = false, CanBeFlushRight = false, PitSquareChance = 0.1f };




            FinalScenario_MainMaterial.pitLayoutGrid = pitGridCave;

            var pitBorderGridCave = Tools.CreateBlankIndexGrid();

            pitBorderGridCave.topLeftIndices = new TileIndexList { indices = new List<int> { 94 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.topIndices = new TileIndexList { indices = new List<int> { 95 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.topRightIndices = new TileIndexList { indices = new List<int> { 96 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.leftIndices = new TileIndexList { indices = new List<int> { 116 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.rightIndices = new TileIndexList { indices = new List<int> { 118 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.bottomLeftIndices = new TileIndexList { indices = new List<int> { 138 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.bottomIndices = new TileIndexList { indices = new List<int> { 139 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.bottomRightIndices = new TileIndexList { indices = new List<int> { 140 }, indexWeights = new List<float> { 1f } };

            pitBorderGridCave.topLeftNubIndices = new TileIndexList { indices = new List<int> { 120 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.topRightNubIndices = new TileIndexList { indices = new List<int> { 119 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.bottomLeftNubIndices = new TileIndexList { indices = new List<int> { 98 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.bottomRightNubIndices = new TileIndexList { indices = new List<int> { 97 }, indexWeights = new List<float> { 1f } };

            pitBorderGridCave.diagonalNubsTopLeftBottomRight = new TileIndexList { indices = new List<int> { 142 }, indexWeights = new List<float> { 1f } };
            pitBorderGridCave.diagonalNubsTopRightBottomLeft = new TileIndexList { indices = new List<int> { 141 }, indexWeights = new List<float> { 1f } };

            pitBorderGridCave.PitBorderIsInternal = false;
            pitBorderGridCave.extendedSet = false;
            pitBorderGridCave.PitBorderOverridesFloorTile = false;
            FinalScenario_MainMaterial.pitBorderFlatGrid = pitBorderGridCave;
            FinalScenario_MainMaterial.supportsUpholstery = true;


            Tools.SetupBeyondRoomMaterial(ref FinalScenario_MainMaterial);

            DungeonMaterial beyondBrickMaterial = ScriptableObject.CreateInstance<DungeonMaterial>();

            //Tools.SetupBeyondRoomMaterial(ref beyondBrickMaterial);
            beyondBrickMaterial.lightPrefabs = FinalScenario_MainMaterial.lightPrefabs;
            beyondBrickMaterial.facewallLightStamps = FinalScenario_MainMaterial.facewallLightStamps;
            beyondBrickMaterial.sidewallLightStamps = FinalScenario_MainMaterial.sidewallLightStamps;
            beyondBrickMaterial.wallShards = FinalScenario_MainMaterial.wallShards;
            beyondBrickMaterial.bigWallShards = FinalScenario_MainMaterial.bigWallShards;



            beyondBrickMaterial.bigWallShardDamageThreshold = 10;
            beyondBrickMaterial.fallbackVerticalTileMapEffects = FinalScenario_MainMaterial.fallbackVerticalTileMapEffects;
            beyondBrickMaterial.fallbackHorizontalTileMapEffects = FinalScenario_MainMaterial.fallbackHorizontalTileMapEffects;
            beyondBrickMaterial.pitfallVFXPrefab = FinalScenario_MainMaterial.pitfallVFXPrefab;
            beyondBrickMaterial.UsePitAmbientVFX = false;
            beyondBrickMaterial.AmbientPitVFX = new List<GameObject>();
            beyondBrickMaterial.PitVFXMinCooldown = 5;
            beyondBrickMaterial.PitVFXMaxCooldown = 30;
            beyondBrickMaterial.ChanceToSpawnPitVFXOnCooldown = 1;
            beyondBrickMaterial.UseChannelAmbientVFX = false;
            beyondBrickMaterial.ChannelVFXMinCooldown = 1;
            beyondBrickMaterial.ChannelVFXMaxCooldown = 15;
            beyondBrickMaterial.AmbientChannelVFX = new List<GameObject>();
            beyondBrickMaterial.stampFailChance = 0.2f;
            beyondBrickMaterial.overrideTableTable = null;
            beyondBrickMaterial.supportsPits = true;
            beyondBrickMaterial.doPitAO = true;
            beyondBrickMaterial.useLighting = true;
            beyondBrickMaterial.pitsAreOneDeep = false;
            beyondBrickMaterial.supportsDiagonalWalls = false;
            beyondBrickMaterial.supportsUpholstery = true;
            beyondBrickMaterial.carpetIsMainFloor = false;
            var carpetGridBrick = Tools.CreateBlankIndexGrid();

            carpetGridBrick.topLeftIndices = new TileIndexList { indices = new List<int> { 278 }, indexWeights = new List<float> { 1f } };//
            carpetGridBrick.topIndices = new TileIndexList { indices = new List<int> { 279 }, indexWeights = new List<float> { 1f } };//
            carpetGridBrick.topRightIndices = new TileIndexList { indices = new List<int> { 280 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.leftIndices = new TileIndexList { indices = new List<int> { 300 }, indexWeights = new List<float> { 1f } };//
            carpetGridBrick.centerIndices = new TileIndexList { indices = new List<int> { 325, 325, 327, 328, 326 }, indexWeights = new List<float> { 1f, 1, 1, 1, 1 } };
            carpetGridBrick.rightIndices = new TileIndexList { indices = new List<int> { 302 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.bottomLeftIndices = new TileIndexList { indices = new List<int> { 322 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.bottomIndices = new TileIndexList { indices = new List<int> { 323 }, indexWeights = new List<float> { 1f } };//
            carpetGridBrick.bottomRightIndices = new TileIndexList { indices = new List<int> { 324 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.topLeftNubIndices = new TileIndexList { indices = new List<int> { 304 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.topRightNubIndices = new TileIndexList { indices = new List<int> { 303 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.bottomLeftNubIndices = new TileIndexList { indices = new List<int> { 282 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.bottomRightNubIndices = new TileIndexList { indices = new List<int> { 281 }, indexWeights = new List<float> { 1f } };
            carpetGridBrick.extendedSet = true;
            carpetGridBrick.CenterIndicesAreStrata = true;

            beyondBrickMaterial.carpetGrids = new TileIndexGrid[]
            {
                carpetGridBrick,
            };
            //beyondBrickMaterial.carpetGrids = new TileIndexGrid[0];
            beyondBrickMaterial.supportsChannels = false;
            beyondBrickMaterial.minChannelPools = 0;
            beyondBrickMaterial.maxChannelPools = 3;
            beyondBrickMaterial.channelTenacity = 0.75f;
            beyondBrickMaterial.channelGrids = new TileIndexGrid[0];        
            beyondBrickMaterial.supportsLavaOrLavalikeSquares = false;
            beyondBrickMaterial.lavaGrids = new TileIndexGrid[0];
            beyondBrickMaterial.supportsIceSquares = false;
            beyondBrickMaterial.iceGrids = new TileIndexGrid[0];

            var ceilingBorderGrid = Tools.CreateBlankIndexGrid();
            ceilingBorderGrid.topLeftIndices = new TileIndexList { indices = new List<int> { 291 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topIndices = new TileIndexList { indices = new List<int> { 379 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topRightIndices = new TileIndexList { indices = new List<int> { 313 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.leftIndices = new TileIndexList { indices = new List<int> { 401 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.centerIndices = new TileIndexList { indices = new List<int> { 468 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.rightIndices = new TileIndexList { indices = new List<int> { 423 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomLeftIndices = new TileIndexList { indices = new List<int> { 335 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomIndices = new TileIndexList { indices = new List<int> { 445 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomRightIndices = new TileIndexList { indices = new List<int> { 357 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.horizontalIndices = new TileIndexList { indices = new List<int> { 555 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.verticalIndices = new TileIndexList { indices = new List<int> { 577 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topCapIndices = new TileIndexList { indices = new List<int> { 467 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.rightCapIndices = new TileIndexList { indices = new List<int> { 511 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomCapIndices = new TileIndexList { indices = new List<int> { 533 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.leftCapIndices = new TileIndexList { indices = new List<int> { 489 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.allSidesIndices = new TileIndexList { indices = new List<int> { 599 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topLeftNubIndices = new TileIndexList { indices = new List<int> { 493 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topRightNubIndices = new TileIndexList { indices = new List<int> { 492 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomLeftNubIndices = new TileIndexList { indices = new List<int> { 471 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomRightNubIndices = new TileIndexList { indices = new List<int> { 470 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderTopNubBothIndices = new TileIndexList { indices = new List<int> { 382 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderRightNubTopIndices = new TileIndexList { indices = new List<int> { 425 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderRightNubBottomIndices = new TileIndexList { indices = new List<int> { 424 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderRightNubBothIndices = new TileIndexList { indices = new List<int> { 426 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderBottomNubLeftIndices = new TileIndexList { indices = new List<int> { 446 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderBottomNubRightIndices = new TileIndexList { indices = new List<int> { 447 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderBottomNubBothIndices = new TileIndexList { indices = new List<int> { 448 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderLeftNubTopIndices = new TileIndexList { indices = new List<int> { 403 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderLeftNubBottomIndices = new TileIndexList { indices = new List<int> { 402 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.borderLeftNubBothIndices = new TileIndexList { indices = new List<int> { 404 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.diagonalNubsTopLeftBottomRight = new TileIndexList { indices = new List<int> { 469 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.diagonalNubsTopRightBottomLeft = new TileIndexList { indices = new List<int> { 491 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.doubleNubsTop = new TileIndexList { indices = new List<int> { 513 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.doubleNubsRight = new TileIndexList { indices = new List<int> { 514 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.doubleNubsBottom = new TileIndexList { indices = new List<int> { 512 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.doubleNubsLeft = new TileIndexList { indices = new List<int> { 515 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.quadNubs = new TileIndexList { indices = new List<int> { 490 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topRightWithNub = new TileIndexList { indices = new List<int> { 314 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.topLeftWithNub = new TileIndexList { indices = new List<int> { 292 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomRightWithNub = new TileIndexList { indices = new List<int> { 358 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.bottomLeftWithNub = new TileIndexList { indices = new List<int> { 336 }, indexWeights = new List<float> { 1f } };
            ceilingBorderGrid.CheckerboardDimension = 1;


            beyondBrickMaterial.roomCeilingBorderGrid = ceilingBorderGrid;

            var pitBorderGridBrick = Tools.CreateBlankIndexGrid();

            pitBorderGridBrick.topLeftIndices = new TileIndexList { indices = new List<int> { 209 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.topIndices = new TileIndexList { indices = new List<int> { 210 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.topRightIndices = new TileIndexList { indices = new List<int> { 211 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.leftIndices = new TileIndexList { indices = new List<int> { 231 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.rightIndices = new TileIndexList { indices = new List<int> { 233 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.bottomLeftIndices = new TileIndexList { indices = new List<int> { 253 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.bottomIndices = new TileIndexList { indices = new List<int> { 254 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.bottomRightIndices = new TileIndexList { indices = new List<int> { 255 }, indexWeights = new List<float> { 1f } };
            
            pitBorderGridBrick.topLeftNubIndices = new TileIndexList { indices = new List<int> { 235 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.topRightNubIndices = new TileIndexList { indices = new List<int> { 234 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.bottomLeftNubIndices = new TileIndexList { indices = new List<int> { 213 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.bottomRightNubIndices = new TileIndexList { indices = new List<int> { 212 }, indexWeights = new List<float> { 1f } };

            pitBorderGridBrick.diagonalNubsTopLeftBottomRight = new TileIndexList { indices = new List<int> { 257 }, indexWeights = new List<float> { 1f } };
            pitBorderGridBrick.diagonalNubsTopRightBottomLeft = new TileIndexList { indices = new List<int> { 256 }, indexWeights = new List<float> { 1f } };

            pitBorderGridBrick.PitBorderIsInternal = false;
            pitBorderGridBrick.extendedSet = false;
            pitBorderGridBrick.PitBorderOverridesFloorTile = false;

            beyondBrickMaterial.pitLayoutGrid = null;
            beyondBrickMaterial.pitBorderFlatGrid = pitBorderGridBrick;
            beyondBrickMaterial.pitBorderRaisedGrid = null;
            beyondBrickMaterial.additionalPitBorderFlatGrid = null;

            beyondBrickMaterial.roomFloorBorderGrid = pitBorderGridBrick;


            var floorSquares = Tools.CreateBlankIndexGrid();

            floorSquares.topLeftIndices = new TileIndexList { indices = new List<int> { 99 }, indexWeights = new List<float> { 1f } };
            floorSquares.topRightIndices = new TileIndexList { indices = new List<int> { 100 }, indexWeights = new List<float> { 1f } };
            floorSquares.bottomLeftIndices = new TileIndexList { indices = new List<int> { 121 }, indexWeights = new List<float> { 1f } };
            floorSquares.bottomRightIndices = new TileIndexList { indices = new List<int> { 122 }, indexWeights = new List<float> { 1f } };


            beyondBrickMaterial.floorSquares = new TileIndexGrid[] { floorSquares };
            beyondBrickMaterial.floorSquareDensity = 0.5f;


            var brickFaceWallGrid = Tools.CreateBlankIndexGrid();


            brickFaceWallGrid.roomTypeRestriction = -1;
            brickFaceWallGrid.topLeftIndices = new TileIndexList { indices = new List<int> { 25 }, indexWeights = new List<float> { 1f } };
            brickFaceWallGrid.topIndices = new TileIndexList { indices = new List<int> { 26, 27 }, indexWeights = new List<float> { 1f, 0.005f } };
            brickFaceWallGrid.topRightIndices = new TileIndexList { indices = new List<int> { 28 }, indexWeights = new List<float> { 1f } };
            brickFaceWallGrid.bottomLeftIndices = new TileIndexList { indices = new List<int> { 47 }, indexWeights = new List<float> { 1f } };
            brickFaceWallGrid.bottomIndices = new TileIndexList { indices = new List<int> { 48, 49 }, indexWeights = new List<float> { 1f } };
            brickFaceWallGrid.bottomRightIndices = new TileIndexList { indices = new List<int> { 50 }, indexWeights = new List<float> { 1f } };

            beyondBrickMaterial.facewallGrids = new FacewallIndexGridDefinition[]
            {
                new FacewallIndexGridDefinition
                {
                    minWidth = 3,
                    maxWidth = 6,
                    hasIntermediaries = false,
                    minIntermediaryBuffer = 0,
                    maxIntermediaryBuffer = 20,
                    maxIntermediaryLength = 1,
                    minIntermediaryLength = 1,
                    topsMatchBottoms = true,
                    middleSectionSequential = false,
                    canExistInCorners = false,
                    forceEdgesInCorners = false,
                    canAcceptWallDecoration = false,
                    canAcceptFloorDecoration = true,
                    forcedStampMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY,
                    canBePlacedInExits = false,
                    chanceToPlaceIfPossible = 0.025f,
                    perTileFailureRate = 0.5f,
                    grid = brickFaceWallGrid,
                }
            };
            beyondBrickMaterial.usesFacewallGrids = true;
            beyondBrickMaterial.usesProceduralMaterialTransitions = false;

            beyondBrickMaterial.internalMaterialTransitions = new RoomInternalMaterialTransition[0];
            beyondBrickMaterial.secretRoomWallShardCollections = new List<GameObject>();
            beyondBrickMaterial.overrideStoneFloorType = false;
            beyondBrickMaterial.overrideFloorType = CellVisualData.CellFloorType.Stone;
            beyondBrickMaterial.useLighting = true;
            beyondBrickMaterial.usesDecalLayer = false;
            beyondBrickMaterial.decalIndexGrid = null;
            beyondBrickMaterial.decalLayerStyle = TilemapDecoSettings.DecoStyle.GROW_FROM_WALLS;
            beyondBrickMaterial.decalSize = 1;
            beyondBrickMaterial.decalSpacing = 1;
            beyondBrickMaterial.usesPatternLayer = false;
            beyondBrickMaterial.patternIndexGrid = null;

            beyondBrickMaterial.patternLayerStyle = TilemapDecoSettings.DecoStyle.GROW_FROM_WALLS;
            beyondBrickMaterial.patternSize = 1;
            beyondBrickMaterial.patternSpacing = 1;
            beyondBrickMaterial.forceEdgesDiagonal = false;
            beyondBrickMaterial.exteriorFacadeBorderGrid = null;
            beyondBrickMaterial.facadeTopGrid = null;
            beyondBrickMaterial.bridgeGrid = null;


            Debug.Log("beyond setup 2");

            DungeonTileStampData m_FloorNameStampData = ScriptableObject.CreateInstance<DungeonTileStampData>();
            m_FloorNameStampData.name = "ENV_Beyond_STAMP_DATA";
            m_FloorNameStampData.tileStampWeight = 0;
            m_FloorNameStampData.spriteStampWeight = 0;
            m_FloorNameStampData.objectStampWeight = 1;
            m_FloorNameStampData.stamps = new TileStampData[0];
            m_FloorNameStampData.spriteStamps = new SpriteStampData[0];
            m_FloorNameStampData.objectStamps = MarinePastPrefab.stampData.objectStamps;
            m_FloorNameStampData.SymmetricFrameChance = 0.25f;
            m_FloorNameStampData.SymmetricCompleteChance = 0.6f;
            
            Debug.Log("beyond setup 2.5");

            dungeon.gameObject.name = "Base_Beyond";
            dungeon.contentSource = ContentSource.CONTENT_UPDATE_03;
            dungeon.DungeonSeed = 0;
            dungeon.DungeonFloorName = "The Beyond."; // what shows up At the top when floor is loaded
            dungeon.DungeonShortName = "Beyond."; // no clue lol, just make it the same
            dungeon.DungeonFloorLevelTextOverride = "Chamber ???"; // what shows up below the floorname
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
            dungeon.ForceRegenerationOfCharacters = false;
            dungeon.ActuallyGenerateTilemap = true;
            Debug.Log("beyond setup 3");
            //tk2dSpriteCollectionData mycollecion = PrefabAPI.PrefabBuilder.BuildObject("BeyondCollection").AddComponent<tk2dSpriteCollectionData>();



            dungeon.tileIndices = new TileIndices()
            {
                tilesetId = (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND, //sets it to our floors CustomValidTileset

                dungeonCollection = BeyondSettings.Instance.debug ? BeyondPrefabs.beyondCollection : gofuckyourself,



                dungeonCollectionSupportsDiagonalWalls = false,

                //aoTileIndices = MarinePastPrefab.tileIndices.aoTileIndices,
                aoTileIndices = new AOTileIndices
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
                    AOFloorDiagonalWallNortheast = 42,
                    AOFloorDiagonalWallNortheastLower = 64,
                    AOFloorDiagonalWallNortheastLowerJoint = 86,
                    AOFloorDiagonalWallNorthwest = 43,
                    AOFloorDiagonalWallNorthwestLower = 65,
                    AOFloorDiagonalWallNorthwestLowerJoint = 87,
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
                //patternIndexGrid = RatDungeonPrefab.tileIndices.patternIndexGrid,
                patternIndexGrid = null,
                globalSecondBorderTiles = new List<int>(0),
                edgeDecorationTiles = null,

            };
            Debug.Log("beyond setup 3.5");

            if (dungeon.tileIndices == null)
            {
                Debug.LogError("beyond setup dungeon.tileIndices nulled");
            }

            if (dungeon.tileIndices.dungeonCollection == null)
            {
                Debug.LogError("beyond setup dungeon.tileIndices.dungeonCollection nulled");
            }
            if (BeyondSettings.Instance.debug) dungeon.tileIndices.dungeonCollection.name = "ENV_Beyond_Collection";
            Debug.Log("beyond setup 3.6");




            if (BeyondSettings.Instance.debug)
            {
                dungeon.roomMaterialDefinitions = new DungeonMaterial[] {
                    FinalScenario_MainMaterial,
                    beyondBrickMaterial,
                };
            }
            else
            {

                dungeon.roomMaterialDefinitions = new DungeonMaterial[] {
                    UnityEngine.Object.Instantiate(MarinePastPrefab.roomMaterialDefinitions[0]),
                    UnityEngine.Object.Instantiate(MarinePastPrefab.roomMaterialDefinitions[0]),
                };

            }
            dungeon.dungeonWingDefinitions = new DungeonWingDefinition[0];
            Debug.Log("beyond setup 4");
            //This section can be used to take parts from other floors and use them as our own.
            //we can make the running dust from one floor our own, the tables from another our own, 
            //we can use all of the stuff from the same floor, or if you want, you can make your own.

            //
            //MarinePastPrefab.pathGridDefinitions[0]
            dungeon.pathGridDefinitions = new List<TileIndexGrid>() {  };
            //dungeon.pathGridDefinitions = new List<TileIndexGrid>() { BeyondTileGrids.floorBorderGrid };
            dungeon.dungeonDustups = new DustUpVFX()
            {
                runDustup = MarinePastPrefab.dungeonDustups.runDustup,
                waterDustup = MarinePastPrefab.dungeonDustups.waterDustup,
                additionalWaterDustup = MarinePastPrefab.dungeonDustups.additionalWaterDustup,
                rollNorthDustup = MarinePastPrefab.dungeonDustups.rollNorthDustup,
                rollNorthEastDustup = MarinePastPrefab.dungeonDustups.rollNorthEastDustup,
                rollEastDustup = MarinePastPrefab.dungeonDustups.rollEastDustup,
                rollSouthEastDustup = MarinePastPrefab.dungeonDustups.rollSouthEastDustup,
                rollSouthDustup = MarinePastPrefab.dungeonDustups.rollSouthDustup,
                rollSouthWestDustup = MarinePastPrefab.dungeonDustups.rollSouthWestDustup,
                rollWestDustup = MarinePastPrefab.dungeonDustups.rollWestDustup,
                rollNorthWestDustup = MarinePastPrefab.dungeonDustups.rollNorthWestDustup,
                rollLandDustup = MarinePastPrefab.dungeonDustups.rollLandDustup
            };
            dungeon.PatternSettings = new SemioticDungeonGenSettings()
            {
                flows = new List<DungeonFlow>()
                {
                    //this will contain our dungeon flows after we make them
                    //BeyondDungeonFlows.F1b_Beyond_flow_01(),
                    BeyondDungeonFlows.F1b_Beyond_flow_Overseer_Test_01(),
                },
                mandatoryExtraRooms = new List<ExtraIncludedRoomData>(0),
                optionalExtraRooms = new List<ExtraIncludedRoomData>(0),
                MAX_GENERATION_ATTEMPTS = 250,
                DEBUG_RENDER_CANVASES_SEPARATELY = false
            };
            Debug.Log("beyond setup 5");
            dungeon.damageTypeEffectMatrix = MarinePastPrefab.damageTypeEffectMatrix;
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
            Debug.Log("beyond setup 6");
            dungeon.decoSettings = new TilemapDecoSettings
            {

                decalExpansion = MarinePastPrefab.decoSettings.decalExpansion,
                decalLayerStyle = MarinePastPrefab.decoSettings.decalLayerStyle,
                decalSize = MarinePastPrefab.decoSettings.decalSize,
                decoPatchFrequency = MarinePastPrefab.decoSettings.decoPatchFrequency,
                decalSpacing = MarinePastPrefab.decoSettings.decalSpacing,
                patternExpansion = MarinePastPrefab.decoSettings.patternExpansion,
                patternLayerStyle = RatDungeonPrefab.decoSettings.patternLayerStyle,
                patternSize = MarinePastPrefab.decoSettings.patternSize,
                patternSpacing = MarinePastPrefab.decoSettings.patternSpacing,
                standardRoomVisualSubtypes = new WeightedIntCollection
                {
                    elements = new WeightedInt[]
                    {
                        new WeightedInt
                        {
                            annotation = "cave",
                            value = 0,
                            weight = 0.7f,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        },
                        new WeightedInt
                        {
                            annotation = "bricks",
                            value = 1,
                            weight = 1f,
                            additionalPrerequisites = new DungeonPrerequisite[0]
                        }
                    },
                },//RatDungeonPrefab.decoSettings.standardRoomVisualSubtypes,


                ambientLightColor = new Color32(70, 62, 74, 255),
                ambientLightColorTwo = new Color32(90, 68, 97, 255),
                lowQualityAmbientLightColor = new Color32(54, 54, 54, 255),
                lowQualityAmbientLightColorTwo = new Color32(54, 54, 54, 255),
                lowQualityCheapLightVector = new Vector4(1, 0, -1, 0),

                UsesAlienFXFloorColor = true,
                AlienFXFloorColor = new Color32(42, 28, 46, 255),

                generateLights = true,
                lightCullingPercentage = 0.2f,
                lightOverlapRadius = 8,
                nearestAllowedLight = 12,
                minLightExpanseWidth = 2,
                lightHeight = -2,
               

                lightCookies = new Texture2D[0],
                debug_view = false,


            };
            Debug.Log("beyond setup 7");
            //more variable we can copy from other floors, or make our own
            dungeon.PlaceDoors = true;
            dungeon.doorObjects = MarinePastPrefab.doorObjects;
            dungeon.oneWayDoorObjects = MarinePastPrefab.oneWayDoorObjects;
            dungeon.oneWayDoorPressurePlate = MarinePastPrefab.oneWayDoorPressurePlate;
            dungeon.phantomBlockerDoorObjects = MarinePastPrefab.phantomBlockerDoorObjects;
            dungeon.UsesWallWarpWingDoors = false;
            dungeon.baseChestContents = MarinePastPrefab.baseChestContents;
            dungeon.SecretRoomSimpleTriggersFacewall = new List<GameObject>() { CatacombsPrefab.SecretRoomSimpleTriggersFacewall[0] };
            dungeon.SecretRoomSimpleTriggersSidewall = new List<GameObject>() { CatacombsPrefab.SecretRoomSimpleTriggersSidewall[0] };
            dungeon.SecretRoomComplexTriggers = new List<ComplexSecretRoomTrigger>(0);
            dungeon.SecretRoomDoorSparkVFX = CatacombsPrefab.SecretRoomDoorSparkVFX;
            dungeon.SecretRoomHorizontalPoofVFX = CatacombsPrefab.SecretRoomHorizontalPoofVFX;
            dungeon.SecretRoomVerticalPoofVFX = CatacombsPrefab.SecretRoomVerticalPoofVFX;
            dungeon.sharedSettingsPrefab = CatacombsPrefab.sharedSettingsPrefab;
            dungeon.NormalRatGUID = string.Empty;
            dungeon.BossMasteryTokenItemId = BotsItemIds.BeyondMasteryToken;
            dungeon.UsesOverrideTertiaryBossSets = false;
            dungeon.OverrideTertiaryRewardSets = new List<TertiaryBossRewardSet>(0);
            dungeon.defaultPlayerPrefab = MarinePastPrefab.defaultPlayerPrefab;
            dungeon.StripPlayerOnArrival = false;
            dungeon.SuppressEmergencyCrates = false;
            dungeon.SetTutorialFlag = false;
            dungeon.PlayerIsLight = true;
            dungeon.PlayerLightColor = CatacombsPrefab.PlayerLightColor;
            dungeon.PlayerLightIntensity = 3;
            dungeon.PlayerLightRadius = 5;
            dungeon.PrefabsToAutoSpawn = new GameObject[0];

            Debug.Log("beyond setup 8");
            //include this for custom floor audio
            dungeon.musicEventName = "Play_MUS_Space_Intro_01";

            CatacombsPrefab = null;
            MarinePastPrefab = null;
            RatDungeonPrefab = null;

            return dungeon;
        }




    }
}
