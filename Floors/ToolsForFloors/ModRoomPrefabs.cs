using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NpcApi;
using Dungeonator;
using UnityEngine;

namespace BotsMod
{
    class ModRoomPrefabs
    {
        public static PrototypeDungeonRoom Keep_Entrance_Room;
        public static void InitCustomRooms()
        {
            Mod_RoomList = new List<string>()
            {
                //the names of all of our rooms once we make them.
                //"Beyond_Entrance2.room"
            };
            //Mod_Entrance_Room = RoomFactory.BuildFromResource("Mod/Resources/ModRooms/floorEntrance.room");
            Mod_Exit_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Exit.room");

            Mod_Entrance_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Entrance2.room");

            

            Mod_Shop_Room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/Beyond_Shop2.room");
            ItsDaFuckinShopApi.RegisterShopRoom(BotsModule.shop.gameObject, Mod_Shop_Room, new UnityEngine.Vector2(5f, 12f));
            

            Mod_Entrance_Room.category = PrototypeDungeonRoom.RoomCategory.ENTRANCE;

            EnterTheBeyond = RoomFactory.BuildFromResource("BotsMod/rooms/RoomUsedToGetToBeyond.room");
            DungeonPrerequisite[] array = new DungeonPrerequisite[0];
            //Vector2 vector = new Vector2((float)(protoroom.Width / 2) + offset.x, (float)(protoroom.Height / 2) + offset.y);
            var vector = new Vector2(5, 6);
            EnterTheBeyond.placedObjectPositions.Add(vector);
            EnterTheBeyond.placedObjects.Add(new PrototypePlacedObjectData
            {
                contentsBasePosition = vector,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = array,
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = new DungeonPlaceable
                {
                    width = 2,
                    height = 2,
                    respectsEncounterableDifferentiator = true,
                    variantTiers = new List<DungeonPlaceableVariant>
                    {
                        new DungeonPlaceableVariant
                        {
                            percentChance = 1f,
                            nonDatabasePlaceable = BeyondPrefabs.beyondEnterance,
                            prerequisites = array,
                            materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
                        }
                    }
                }
            });
            InitRooms();

            Mod_Entrance_Room_Past = RoomFactory.BuildFromResource("BotsMod/rooms/Past/LostPastRoom1.room");
            Mod_Entrance_Room_Past.category = PrototypeDungeonRoom.RoomCategory.ENTRANCE;

            Mod_Entrance_Room_Past.placedObjectPositions.Add(vector);
            Mod_Entrance_Room_Past.placedObjects.Add(new PrototypePlacedObjectData
            {
                contentsBasePosition = vector,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = array,
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = new DungeonPlaceable
                {
                    width = 2,
                    height = 2,
                    respectsEncounterableDifferentiator = true,
                    variantTiers = new List<DungeonPlaceableVariant>
                    {
                        new DungeonPlaceableVariant
                        {
                            percentChance = 1f,
                            nonDatabasePlaceable = BeyondPrefabs.pastControllerObject,
                            prerequisites = array,
                            materialRequirements = new DungeonPlaceableRoomMaterialRequirement[0]
                        }
                    }
                }
            });

            List<PrototypeDungeonRoom> m_feyondRooms = new List<PrototypeDungeonRoom>();

            foreach (string name in Mod_RoomList)
            {
                PrototypeDungeonRoom m_room = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/" + name);
                m_feyondRooms.Add(m_room);
            }

            Mod_Rooms = m_feyondRooms.ToArray();

            foreach (PrototypeDungeonRoom room in Mod_Rooms)
            {
                BeyondPrefabs.BeyondRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, 1));
            }

            Mod_Boss = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/overseerbossroombutround.room");
            Mod_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Mod_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Mod_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Mod_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Mod_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Mod_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Mod_Boss.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Mod_Boss.usesProceduralLighting = false;
            Mod_Boss.usesProceduralDecoration = false;
            Mod_Boss.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            //Mod_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.BELLYGEON;

            Mod_Boss_Past = RoomFactory.BuildFromResource("BotsMod/rooms/Past/LostPastBossRoom.room");
            Mod_Boss_Past.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Mod_Boss_Past.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS;
            Mod_Boss_Past.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Mod_Boss_Past.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Mod_Boss_Past.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Mod_Boss_Past.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Mod_Boss_Past.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Mod_Boss_Past.usesProceduralLighting = false;
            Mod_Boss_Past.usesProceduralDecoration = false;
            Mod_Boss_Past.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            Mod_Boss_Past.overriddenTilesets = GlobalDungeonData.ValidTilesets.BELLYGEON;


            var Play_Test_Boss = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/overseerbossroombutround.room");
            Play_Test_Boss.category = PrototypeDungeonRoom.RoomCategory.BOSS;
            Play_Test_Boss.subCategoryBoss = PrototypeDungeonRoom.RoomBossSubCategory.MINI_BOSS;
            Play_Test_Boss.subCategoryNormal = PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT;
            Play_Test_Boss.subCategorySpecial = PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP;
            Play_Test_Boss.subCategorySecret = PrototypeDungeonRoom.RoomSecretSubCategory.UNSPECIFIED_SECRET;
            Play_Test_Boss.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            Play_Test_Boss.associatedMinimapIcon = BeyondPrefabs.doublebeholsterroom01.associatedMinimapIcon;
            Play_Test_Boss.usesProceduralLighting = false;
            Play_Test_Boss.usesProceduralDecoration = false;
            Play_Test_Boss.rewardChestSpawnPosition = new IntVector2(25, 20); //Where the reward pedestal spawns, should be changed based on room size
            Play_Test_Boss.overriddenTilesets = GlobalDungeonData.ValidTilesets.CASTLEGEON;

            StairWayRoom = RoomFactory.BuildFromResource("BotsMod/rooms/StairWayNpcRoom.room");
            ItsDaFuckinShopApi.RegisterShopRoom(BotsModule.shop2.gameObject, StairWayRoom, new UnityEngine.Vector2(4f, 17));

            //foreach (PrototypeRoomExit exit in Mod_Boss.exitData.exits) { exit.exitType = PrototypeRoomExit.ExitType.ENTRANCE_ONLY; }
            //    RoomBuilder.AddExitToRoom(Mod_Boss, new Vector2(26, 37), DungeonData.Direction.NORTH, PrototypeRoomExit.ExitType.EXIT_ONLY, PrototypeRoomExit.ExitGroup.B);



        }

        public static ProceduralFlowModifierData GenerateNewMrocData(PrototypeDungeonRoom RequiredRoom)
        {
            string name = RequiredRoom.name.ToString();
            if (RequiredRoom.name.ToString() == null)
            {
                name = "BeyondEffigyRoomThingPleaseWork";
            }
            ProceduralFlowModifierData PrayerRoomMines = new ProceduralFlowModifierData()
            {
                annotation = name,
                
                DEBUG_FORCE_SPAWN = true,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
                },
                roomTable = null,
                exactRoom = RequiredRoom,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 1,
                chanceToSpawn = 100,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[]
                {
                    new DungeonGenToolbox.AdvancedDungeonPrerequisite
                    {
                       prerequisiteType = DungeonPrerequisite.PrerequisiteType.NUMBER_PASTS_COMPLETED,
                       comparisonValue = 0,
                    }
                },
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,

            };
            return PrayerRoomMines;
        }

        public static void InitRooms()
        {
            SharedInjectionData injector = ScriptableObject.CreateInstance<SharedInjectionData>();
            injector.UseInvalidWeightAsNoInjection = true;
            
            injector.PreventInjectionOfFailedPrerequisites = false;
            injector.IsNPCCell = false;
            injector.IgnoreUnmetPrerequisiteEntries = false;
            injector.OnlyOne = false;
            injector.ChanceToSpawnOne = 1f;
            injector.AttachedInjectionData = new List<SharedInjectionData>();
            injector.InjectionData = new List<ProceduralFlowModifierData>
            {
                GenerateNewMrocData(ModRoomPrefabs.EnterTheBeyond),
            };
            injector.name = "beyond entrace stuff";

            SharedInjectionData BaseInjection = FloorHooks.GetOrLoadByName_Orig("base_catacombs").PatternSettings.flows[0].sharedInjectionData[1];
            if (BaseInjection.AttachedInjectionData == null)
            {
                BaseInjection.AttachedInjectionData = new List<SharedInjectionData>();
            }
            BaseInjection.AttachedInjectionData.Add(injector);

        }

        public static WeightedRoom GenerateWeightedRoom(PrototypeDungeonRoom Room, float Weight = 1, bool LimitedCopies = true, int MaxCopies = 1, DungeonPrerequisite[] AdditionalPrerequisites = null)
        {
            if (Room == null) { return null; }
            if (AdditionalPrerequisites == null) { AdditionalPrerequisites = new DungeonPrerequisite[0]; }
            return new WeightedRoom() { room = Room, weight = Weight, limitedCopies = LimitedCopies, maxCopies = MaxCopies, additionalPrerequisites = AdditionalPrerequisites };
        }

        public static PrototypeDungeonRoom Mod_Entrance_Room;
        public static PrototypeDungeonRoom Mod_Entrance_Room_Past;
        public static PrototypeDungeonRoom Mod_Exit_Room;

        public static PrototypeDungeonRoom Mod_Shop_Room;

        public static PrototypeDungeonRoom EnterTheBeyond;

        public static PrototypeDungeonRoom[] Mod_Rooms;
        public static PrototypeDungeonRoom Mod_Boss;
        public static PrototypeDungeonRoom Mod_Boss_Past;

        public static PrototypeDungeonRoom StairWayRoom;

        public static List<string> Mod_RoomList; // this will contain all of our mods rooms.
    }
}
