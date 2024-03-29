﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using UnityEngine;
using Dungeonator;
using Random = UnityEngine.Random;
using CustomShrineController = GungeonAPI.ShrineFactory.CustomShrineController;
using FloorType = Dungeonator.CellVisualData.CellFloorType;
using Ionic.Zip;
using BotsMod;

namespace GungeonAPI
{
    public static class RoomFactory
    {
        public static Dictionary<string, RoomData> rooms = new Dictionary<string, RoomData>();
        //public static string roomDirectory = Path.Combine(ETGMod.GameFolder, "CustomRoomData").Replace('/', Path.DirectorySeparatorChar);
        private static readonly string dataHeader = "***DATA***";
        private static FieldInfo m_cellData = typeof(PrototypeDungeonRoom).GetField("m_cellData", BindingFlags.Instance | BindingFlags.NonPublic);
        private static RoomEventDefinition sealOnEnterWithEnemies = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES, RoomEventTriggerAction.SEAL_ROOM);
        private static RoomEventDefinition unsealOnRoomClear = new RoomEventDefinition(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, RoomEventTriggerAction.UNSEAL_ROOM);

        public static string RoomDirectory()
        {
            return Path.GetFullPath(Path.Combine(ETGMod.GameFolder, "CustomRoomData")) + Path.DirectorySeparatorChar;
        }

        public static void LoadRoomsFromRoomDirectory()
        {
            string roomDirectory = RoomDirectory();
            Directory.CreateDirectory(roomDirectory);
            foreach (string f in Directory.GetFiles(roomDirectory, "*", SearchOption.AllDirectories))
            {
                if (f.EndsWith(".room", StringComparison.OrdinalIgnoreCase))
                {
                    string name = Path.GetFullPath(f).RemovePrefix(roomDirectory).RemoveSuffix(".room");
                    ToolsGAPI.Log($"Found room: \"{name}\"");
                    var roomData = BuildFromRoomFile(f);
                    DungeonHandler.Register(roomData);
                    rooms.Add(name, roomData);
                }
                else if (f.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ToolsGAPI.Log($"Found zipped rooms: \"{f}\"");
                    var roomsData = BuildFromZipFile(f);
                    foreach (var roomData in roomsData)
                    {
                        string name = roomData.room.name;
                        ToolsGAPI.Log($"Found room: \"{name}\"");
                        DungeonHandler.Register(roomData);
                        rooms.Add(roomData.room.name, roomData);
                    }
                }
            }
        }

        public static RoomData BuildFromRoomFile(string roomPath)
        {
            var texture = ResourceExtractor.GetTextureFromFile(roomPath, ".room");
            texture.name = Path.GetFileName(roomPath);
            RoomData roomData = ExtractRoomDataFromFile(roomPath);
            roomData.room = Build(texture, roomData);
            return roomData;
        }

        public static IEnumerable<RoomData> BuildFromZipFile(string zipFilePath)
        {
            if (!ZipFile.IsZipFile(zipFilePath))
            {
                ToolsGAPI.Log($"Not a valid zip file!");
                yield break;
            }

            using (var zipFile = ZipFile.Read(zipFilePath))
            {
                foreach (var entry in zipFile.Entries)
                {
                    var fileName = Path.GetFileNameWithoutExtension(entry.FileName);
                    var extension = Path.GetExtension(entry.FileName);
                    if (!string.Equals(extension, ".room", StringComparison.OrdinalIgnoreCase))
                        continue;

                    byte[] zipData;
                    int capacity = (int)entry.UncompressedSize;
                    if (capacity < 0)
                        continue;

                    using (var ms = new MemoryStream(capacity))
                    {
                        entry.Extract(ms);
                        zipData = ms.ToArray();
                    }

                    var texture = ResourceExtractor.BytesToTexture(zipData, fileName);
                    var roomData = ExtractRoomDataFromBytes(zipData);
                    roomData.room = Build(texture, roomData);

                    yield return roomData;
                }
            }
        }

        public static RoomData BuildFromResource(string roomPath)
        {
            var texture = ResourceExtractor.GetTextureFromResource(roomPath);
            RoomData roomData = ExtractRoomDataFromResource(roomPath);
            roomData.room = Build(texture, roomData);
            return roomData;
        }

        public static PrototypeDungeonRoom Build(Texture2D texture, RoomData roomData)
        {
            try
            {
                var room = CreateRoomFromTexture(texture);
                ApplyRoomData(room, roomData);
                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                ToolsGAPI.PrintError("Failed to build room!");
                ToolsGAPI.PrintException(e);
            }

            return CreateEmptyRoom(12, 12);
        }

        public static void ApplyRoomData(PrototypeDungeonRoom room, RoomData roomData)
        {
            if (roomData.exitPositions != null)
            {
                for (int i = 0; i < roomData.exitPositions.Length; i++)
                {
                    DungeonData.Direction dir = (DungeonData.Direction)Enum.Parse(typeof(DungeonData.Direction), roomData.exitDirections[i].ToUpper());
                    AddExit(room, roomData.exitPositions[i], dir);
                }
            }
            else
            {
                AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
            }

            if (roomData.enemyPositions != null)
            {
                for (int i = 0; i < roomData.enemyPositions.Length; i++)
                {
                    AddEnemyToRoom(room, roomData.enemyPositions[i], roomData.enemyGUIDs[i], roomData.enemyReinforcementLayers[i], roomData.randomizeEnemyPositions);
                }
            }

            if (roomData.placeablePositions != null)
            {
                for (int i = 0; i < roomData.placeablePositions.Length; i++)
                {
                    AddPlaceableToRoom(room, roomData.placeablePositions[i], roomData.placeableGUIDs[i]);
                }
            }

            if (roomData.floors != null)
            {
                foreach (var floor in roomData.floors)
                {
                    room.prerequisites.Add(new DungeonPrerequisite()
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
                        requiredTileset = ToolsGAPI.GetEnumValue<GlobalDungeonData.ValidTilesets>(floor)
                    });
                }
            }
            //Set categories
            if (!string.IsNullOrEmpty(roomData.category))
                room.category = ToolsGAPI.GetEnumValue<PrototypeDungeonRoom.RoomCategory>(roomData.category);
            if (!string.IsNullOrEmpty(roomData.normalSubCategory))
                room.subCategoryNormal = ToolsGAPI.GetEnumValue<PrototypeDungeonRoom.RoomNormalSubCategory>(roomData.normalSubCategory);
            if (!string.IsNullOrEmpty(roomData.bossSubCategory))
                room.subCategoryBoss = ToolsGAPI.GetEnumValue<PrototypeDungeonRoom.RoomBossSubCategory>(roomData.bossSubCategory);
            if (!string.IsNullOrEmpty(roomData.specialSubCategory))
                room.subCategorySpecial = ToolsGAPI.GetEnumValue<PrototypeDungeonRoom.RoomSpecialSubCategory>(roomData.specialSubCategory);

            room.usesProceduralDecoration = true;
            room.allowFloorDecoration = roomData.doFloorDecoration;
            room.allowWallDecoration = roomData.doWallDecoration;
            room.usesProceduralLighting = roomData.doLighting;
        }


        public static void AddObjectToRoom(PrototypeDungeonRoom room, Vector2 position, DungeonPlaceable PlacableContents = null, DungeonPlaceableBehaviour NonEnemyBehaviour = null, string EnemyBehaviourGuid = null, float SpawnChance = 1f, int xOffset = 0, int yOffset = 0, int layer = 0)
        {
            if (room == null) { return; }
            if (room.placedObjects == null) { room.placedObjects = new List<PrototypePlacedObjectData>(); }
            if (room.placedObjectPositions == null) { room.placedObjectPositions = new List<Vector2>(); }

            PrototypePlacedObjectData m_NewObjectData = new PrototypePlacedObjectData()
            {
                placeableContents = null,
                nonenemyBehaviour = null,
                spawnChance = SpawnChance,
                unspecifiedContents = null,
                enemyBehaviourGuid = string.Empty,
                contentsBasePosition = position,
                layer = layer,
                xMPxOffset = xOffset,
                yMPxOffset = yOffset,
                fieldData = new List<PrototypePlacedObjectFieldData>(0),
                instancePrerequisites = new DungeonPrerequisite[0],
                linkedTriggerAreaIDs = new List<int>(0),
                assignedPathIDx = -1,
                assignedPathStartNode = 0
            };

            if (PlacableContents != null)
            {
                m_NewObjectData.placeableContents = PlacableContents;
            }
            else if (NonEnemyBehaviour != null)
            {
                m_NewObjectData.nonenemyBehaviour = NonEnemyBehaviour;
            }
            else if (EnemyBehaviourGuid != null)
            {
                m_NewObjectData.enemyBehaviourGuid = EnemyBehaviourGuid;
            }
            else
            {
                // All possible object fields were left null? Do nothing and return if this is the case.
                return;
            }

            room.placedObjects.Add(m_NewObjectData);
            room.placedObjectPositions.Add(position);
            return;
        }

        public static PrototypeDungeonRoom BuildFromResource(string roomPath, bool setRoomCategory = false, bool autoAssignToFloor = false, bool defineFullPath = false, bool assignDecorationSettings = false)
        {
            string RoomPath = (roomPath);
            if (defineFullPath) { RoomPath = roomPath; }
            Texture2D texture = ResourceExtractor.GetTextureFromResource(RoomPath);
            RoomData roomData = ExtractRoomDataFromResource(RoomPath);
            return Build(texture, roomData, setRoomCategory, autoAssignToFloor, assignDecorationSettings, roomData.weight);
        }

        public static PrototypeDungeonRoom Build(Texture2D texture, RoomData roomData, bool SetRoomCategory, bool AutoAssignToFloor, bool AssignDecorationProperties, float? Weight)
        {
            try
            {
                PrototypeDungeonRoom room = CreateRoomFromTexture(texture);
                ApplyRoomData(room, roomData, SetRoomCategory, AutoAssignToFloor, AssignDecorationProperties, Weight);
                room.OnBeforeSerialize();
                room.OnAfterDeserialize();
                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                Tools.PrintError("Failed to build room!");
                Tools.PrintException(e);
            }
            return CreateEmptyRoom(12, 12);
        }

        public static void ApplyRoomData(PrototypeDungeonRoom room, RoomData roomData, bool setRoomCategory, bool autoAssignToFloor, bool assignDecorationProperties, float? Weight)
        {
            // Tools.Print("Building Exits...");
            if (roomData.exitPositions != null)
            {
                for (int i = 0; i < roomData.exitPositions.Length; i++)
                {
                    DungeonData.Direction dir = (DungeonData.Direction)Enum.Parse(typeof(DungeonData.Direction), roomData.exitDirections[i].ToUpper());
                    AddExit(room, roomData.exitPositions[i], dir);
                }
            }
            else
            {
                AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
            }

            // Tools.Print("Adding Enemies...");
            if (roomData.enemyPositions != null)
            {
                for (int i = 0; i < roomData.enemyPositions.Length; i++)
                {
                    AddEnemyToRoom(room, roomData.enemyPositions[i], roomData.enemyGUIDs[i], roomData.enemyReinforcementLayers[i], roomData.randomizeEnemyPositions);
                }
            }

            // Tools.Print("Adding Objects...");
            if (roomData.placeablePositions != null)
            {
                for (int i = 0; i < roomData.placeablePositions.Length; i++)
                {
                    AddPlaceableToRoom(room, roomData.placeablePositions[i], roomData.placeableGUIDs[i]);
                }
            }

            if (setRoomCategory | autoAssignToFloor)
            {
                // Set categories
                if (!string.IsNullOrEmpty(roomData.category)) { room.category = GetRoomCategory(roomData.category); }
                if (!string.IsNullOrEmpty(roomData.normalSubCategory)) { room.subCategoryNormal = GetRoomNormalSubCategory(roomData.normalSubCategory); }
                if (!string.IsNullOrEmpty(roomData.bossSubCategory)) { room.subCategoryBoss = GetRoomBossSubCategory(roomData.bossSubCategory); }
                if (!string.IsNullOrEmpty(roomData.specialSubCategory)) { room.subCategorySpecial = GetRoomSpecialSubCategory(roomData.specialSubCategory); }
            }
            if (autoAssignToFloor && roomData.floors != null)
            {
                if (!Weight.HasValue)
                {
                    if (room.category == PrototypeDungeonRoom.RoomCategory.SECRET)
                    {
                        Weight = 15; // Normal secret rooms have weight of 15.
                    }
                    else
                    {
                        Weight = 1;
                    }
                }
                if (room.category == PrototypeDungeonRoom.RoomCategory.SECRET)
                {
                    // Secret rooms are generally shared across all floors via a specific room table.
                    // Room Editor doesn't currently set this for secret rooms
                    room.OverrideMusicState = DungeonFloorMusicController.DungeonMusicState.CALM;
                    BeyondPrefabs.SecretRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, Weight.Value));
                }
                else
                {
                    foreach (string floor in roomData.floors) { AssignRoomToFloorRoomTable(room, GetTileSet(floor), Weight); }
                    /*BeyondPrefabs.CustomRoomTableSecretGlitchFloor.includedRooms.elements.Add(GenerateWeightedRoom(room, Weight.Value));
                    BeyondPrefabs.CustomRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(room, Weight.Value));
                    BeyondPrefabs.CustomRoomTable2.includedRooms.elements.Add(GenerateWeightedRoom(room, Weight.Value));*/
                }
            }

            if (assignDecorationProperties)
            {
                room.allowFloorDecoration = roomData.doFloorDecoration;
                room.allowWallDecoration = roomData.doWallDecoration;
                room.usesProceduralLighting = roomData.doLighting;
            }
        }

        public static WeightedRoom GenerateWeightedRoom(PrototypeDungeonRoom Room, float Weight = 1, bool LimitedCopies = true, int MaxCopies = 1, DungeonPrerequisite[] AdditionalPrerequisites = null)
        {
            if (Room == null) { return null; }
            if (AdditionalPrerequisites == null) { AdditionalPrerequisites = new DungeonPrerequisite[0]; }
            return new WeightedRoom() { room = Room, weight = Weight, limitedCopies = LimitedCopies, maxCopies = MaxCopies, additionalPrerequisites = AdditionalPrerequisites };
        }

        public static GlobalDungeonData.ValidTilesets GetTileSet(string val)
        {
            return (GlobalDungeonData.ValidTilesets)Enum.Parse(typeof(GlobalDungeonData.ValidTilesets), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomCategory GetRoomCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomNormalSubCategory GetRoomNormalSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomNormalSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomNormalSubCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomBossSubCategory GetRoomBossSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomBossSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomBossSubCategory), val.ToUpper());
        }
        public static PrototypeDungeonRoom.RoomSpecialSubCategory GetRoomSpecialSubCategory(string val)
        {
            return (PrototypeDungeonRoom.RoomSpecialSubCategory)Enum.Parse(typeof(PrototypeDungeonRoom.RoomSpecialSubCategory), val.ToUpper());
        }


        public static void AssignRoomToFloorRoomTable(PrototypeDungeonRoom Room, GlobalDungeonData.ValidTilesets targetTileSet, float? Weight)
        {
            if (targetTileSet == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                BeyondPrefabs.CastleRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.SEWERGEON)
            {
                BeyondPrefabs.SewersRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                BeyondPrefabs.Gungeon_RoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
            {
                BeyondPrefabs.AbbeyRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                BeyondPrefabs.MinesRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
            {
                BeyondPrefabs.CatacombsRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.OFFICEGEON)
            {
                // R&G floor doesn't use room tables yet. If a room has this set, I'll assign to all floors instead.
                return;
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                BeyondPrefabs.ForgeRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            else if (targetTileSet == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                BeyondPrefabs.BulletHellRoomTable.includedRooms.elements.Add(GenerateWeightedRoom(Room, Weight.Value));
            }
            return;
        }

        public static RoomData ExtractRoomDataFromBytes(byte[] data)
        {
            string stringData = ResourceExtractor.BytesToString(data);
            return ExtractRoomData(stringData);
        }

        public static RoomData ExtractRoomDataFromFile(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return ExtractRoomDataFromBytes(data);
        }

        public static RoomData ExtractRoomDataFromResource(string path)
        {
            byte[] data = ResourceExtractor.ExtractEmbeddedResource(path);
            return ExtractRoomDataFromBytes(data);
        }

        public static RoomData ExtractRoomData(string data)
        {
            int end = data.Length - dataHeader.Length - 1;
            for (int i = end; i > 0; i--)
            {
                string sub = data.Substring(i, dataHeader.Length);
                if (sub.Equals(dataHeader))
                    return JsonUtility.FromJson<RoomData>(data.Substring(i + dataHeader.Length));
            }
            ToolsGAPI.Log($"No room data found at {data}");
            return new RoomData();
        }

        public static PrototypeDungeonRoom CreateRoomFromTexture(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            PrototypeDungeonRoom room = GetNewPrototypeDungeonRoom(width, height);
            PrototypeDungeonRoomCellData[] cellData = new PrototypeDungeonRoomCellData[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cellData[x + y * width] = CellDataFromColor(texture.GetPixel(x, y));
                }
            }
            room.FullCellData = cellData;
            room.name = texture.name;
            return room;
        }

        public static PrototypeDungeonRoomCellData CellDataFromColor(Color32 color)
        {
            if (color.Equals(Color.magenta)) return null;

            var data = new PrototypeDungeonRoomCellData();
            data.state = TypeFromColor(color);
            data.diagonalWallType = DiagonalWallTypeFromColor(color);
            data.appearance = new PrototypeDungeonRoomCellAppearance()
            {
                OverrideFloorType = FloorType.Stone
            };
            return data;
        }

        public static CellType TypeFromColor(Color color)
        {
            if (color == Color.black)
                return CellType.PIT;
            else if (color == Color.white)
                return CellType.FLOOR;
            else
                return CellType.WALL;
        }

        public static DiagonalWallType DiagonalWallTypeFromColor(Color color)
        {
            if (color == Color.red)
                return DiagonalWallType.NORTHEAST;
            else if (color == Color.green)
                return DiagonalWallType.SOUTHEAST;
            else if (color == Color.blue)
                return DiagonalWallType.SOUTHWEST;
            else if (color == Color.yellow)
                return DiagonalWallType.NORTHWEST;
            else
                return DiagonalWallType.NONE;
        }

        public static RoomData CreateEmptyRoomData(int width = 12, int height = 12)
        {
            RoomData data = new RoomData()
            {
                room = CreateEmptyRoom(width, height),
                category = "NORMAL",
                weight = DungeonHandler.GlobalRoomWeight
            };

            return data;
        }

        public static PrototypeDungeonRoom CreateEmptyRoom(int width = 12, int height = 12)
        {
            try
            {
                PrototypeDungeonRoom room = GetNewPrototypeDungeonRoom(width, height);
                
                AddExit(room, new Vector2(width / 2, height), DungeonData.Direction.NORTH);
                AddExit(room, new Vector2(width / 2, 0), DungeonData.Direction.SOUTH);
                AddExit(room, new Vector2(width, height / 2), DungeonData.Direction.EAST);
                AddExit(room, new Vector2(0, height / 2), DungeonData.Direction.WEST);

                PrototypeDungeonRoomCellData[] cellData = m_cellData.GetValue(room) as PrototypeDungeonRoomCellData[];
                cellData = new PrototypeDungeonRoomCellData[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        cellData[x + y * width] = new PrototypeDungeonRoomCellData()
                        {
                            state = CellType.FLOOR,
                            appearance = new PrototypeDungeonRoomCellAppearance()
                            {
                                OverrideFloorType = CellVisualData.CellFloorType.Stone,
                            },
                        };
                    }
                }
                m_cellData.SetValue(room, cellData);

                room.UpdatePrecalculatedData();
                return room;
            }
            catch (Exception e)
            {
                ToolsGAPI.PrintException(e);
                return null;
            }
        }

        public static int GetStyleValue(string dungeonName, string shrineID)
        {
            if (ShrineFactory.builtShrines != null && ShrineFactory.builtShrines.ContainsKey(shrineID))
            {
                var shrineData = ShrineFactory.builtShrines[shrineID]?.GetComponent<CustomShrineController>();
                if (shrineData != null && shrineData.roomStyles != null && shrineData.roomStyles.ContainsKey(dungeonName))
                    return shrineData.roomStyles[dungeonName];
            }
            return -1;
        }

        public static void AddPlaceableToRoom(PrototypeDungeonRoom room, Vector2 location, string assetPath)
        {
            try
            {
                GameObject asset = GetGameObjectFromBundles(assetPath);
                if (asset)
                {
                    DungeonPrerequisite[] emptyReqs = new DungeonPrerequisite[0];
                    room.placedObjectPositions.Add(location);

                    var placeableContents = ScriptableObject.CreateInstance<DungeonPlaceable>();
                    placeableContents.width = 2;
                    placeableContents.height = 2;
                    placeableContents.respectsEncounterableDifferentiator = true;
                    placeableContents.variantTiers = new List<DungeonPlaceableVariant>()
                    {
                        new DungeonPlaceableVariant()
                            {
                                percentChance = 1,
                                nonDatabasePlaceable = asset,
                                prerequisites = emptyReqs,
                                materialRequirements= new DungeonPlaceableRoomMaterialRequirement[0]
                            }
                    };

                    room.placedObjects.Add(new PrototypePlacedObjectData()
                    {
                        contentsBasePosition = location,
                        fieldData = new List<PrototypePlacedObjectFieldData>(),
                        instancePrerequisites = emptyReqs,
                        linkedTriggerAreaIDs = new List<int>(),
                        placeableContents = placeableContents
                    });
                    //ToolsGAPI.Print($"Added {asset.name} to room.");
                    return;
                }
                DungeonPlaceable placeable = GetPlaceableFromBundles(assetPath);
                if (placeable)
                {
                    DungeonPrerequisite[] emptyReqs = new DungeonPrerequisite[0];
                    room.placedObjectPositions.Add(location);
                    room.placedObjects.Add(new PrototypePlacedObjectData()
                    {
                        contentsBasePosition = location,
                        fieldData = new List<PrototypePlacedObjectFieldData>(),
                        instancePrerequisites = emptyReqs,
                        linkedTriggerAreaIDs = new List<int>(),
                        placeableContents = placeable
                    });
                    return;
                }

                ToolsGAPI.PrintError($"Unable to find asset in asset bundles: {assetPath}");

            }
            catch (Exception e)
            {
                ToolsGAPI.PrintException(e);
            }
        }

        public static DungeonPlaceable GetPlaceableFromBundles(string assetPath)
        {
            DungeonPlaceable placeable = null;
            foreach (var bundle in StaticReferences.AssetBundles.Values)
            {
                placeable = bundle.LoadAsset<DungeonPlaceable>(assetPath);
                if (placeable)
                    break;
            }
            return placeable;
        }

        public static GameObject GetGameObjectFromBundles(string assetPath)
        {
            GameObject asset = null;
            foreach (var bundle in StaticReferences.AssetBundles.Values)
            {
                asset = bundle.LoadAsset<GameObject>(assetPath);
                if (asset)
                    break;
            }
            if (asset == null)
            {
                foreach (var fakeAsset in FakePrefab.ExistingFakePrefabs)
                {
                    BotsModule.Log(assetPath + ": " + fakeAsset.name);
                    if (fakeAsset.name.ToLower().Contains(assetPath.ToLower()))
                    {
                        fakeAsset.SetActive(true);
                        asset = fakeAsset;
                        break;

                    }

                    
                }
            }


            return asset;
        }

        public static void AddEnemyToRoom(PrototypeDungeonRoom room, Vector2 location, string guid, int layer, bool shuffle)
        {
            DungeonPrerequisite[] emptyReqs = new DungeonPrerequisite[0];

            var placeableContents = ScriptableObject.CreateInstance<DungeonPlaceable>();
            placeableContents.width = 1;
            placeableContents.height = 1;
            placeableContents.respectsEncounterableDifferentiator = true;
            placeableContents.variantTiers = new List<DungeonPlaceableVariant>()
            {
                new DungeonPlaceableVariant()
                {
                    percentChance = 1,
                    prerequisites = emptyReqs,
                    enemyPlaceableGuid = guid,
                    materialRequirements= new DungeonPlaceableRoomMaterialRequirement[0],
                }
            };

            var objectData = new PrototypePlacedObjectData()
            {
                contentsBasePosition = location,
                fieldData = new List<PrototypePlacedObjectFieldData>(),
                instancePrerequisites = emptyReqs,
                linkedTriggerAreaIDs = new List<int>(),
                placeableContents = placeableContents,
            };


            if (layer > 0)
                AddObjectDataToReinforcementLayer(room, objectData, layer - 1, location, shuffle);
            else
            {
                room.placedObjects.Add(objectData);
                room.placedObjectPositions.Add(location);
            }

            if (!room.roomEvents.Contains(sealOnEnterWithEnemies))
                room.roomEvents.Add(sealOnEnterWithEnemies);
            if (!room.roomEvents.Contains(unsealOnRoomClear))
                room.roomEvents.Add(unsealOnRoomClear);
        }

        public static void AddObjectDataToReinforcementLayer(PrototypeDungeonRoom room, PrototypePlacedObjectData objectData, int layer, Vector2 location, bool shuffle)
        {
            if (room.additionalObjectLayers.Count <= layer)
            {
                for (int i = room.additionalObjectLayers.Count; i <= layer; i++)
                {
                    var newLayer = new PrototypeRoomObjectLayer()
                    {
                        layerIsReinforcementLayer = true,
                        placedObjects = new List<PrototypePlacedObjectData>(),
                        placedObjectBasePositions = new List<Vector2>(),
                        shuffle = shuffle,
                    };
                    room.additionalObjectLayers.Add(newLayer);

                }
            }
            room.additionalObjectLayers[layer].placedObjects.Add(objectData);
            room.additionalObjectLayers[layer].placedObjectBasePositions.Add(location);
        }

        public static void AddExit(PrototypeDungeonRoom room, Vector2 location, DungeonData.Direction direction)
        {
            if (room.exitData == null)
                room.exitData = new PrototypeRoomExitData();
            if (room.exitData.exits == null)
                room.exitData.exits = new List<PrototypeRoomExit>();

            PrototypeRoomExit exit = new PrototypeRoomExit(direction, location);
            exit.exitType = PrototypeRoomExit.ExitType.NO_RESTRICTION;
            Vector2 margin = (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST) ? new Vector2(0, 1) : new Vector2(1, 0);
            exit.containedCells.Add(location + margin);
            room.exitData.exits.Add(exit);
        }

        public static PrototypeDungeonRoom GetNewPrototypeDungeonRoom(int width = 12, int height = 12)
        {
            PrototypeDungeonRoom room = ScriptableObject.CreateInstance<PrototypeDungeonRoom>();
            room.injectionFlags = new RuntimeInjectionFlags();
            room.RoomId = UnityEngine.Random.Range(10000, 1000000);
            room.pits = new List<PrototypeRoomPitEntry>();
            room.placedObjects = new List<PrototypePlacedObjectData>();
            room.placedObjectPositions = new List<Vector2>();
            room.additionalObjectLayers = new List<PrototypeRoomObjectLayer>();
            room.eventTriggerAreas = new List<PrototypeEventTriggerArea>();
            room.roomEvents = new List<RoomEventDefinition>();
            room.paths = new List<SerializedPath>();
            room.prerequisites = new List<DungeonPrerequisite>();
            room.excludedOtherRooms = new List<PrototypeDungeonRoom>();
            room.rectangularFeatures = new List<PrototypeRectangularFeature>();
            room.exitData = new PrototypeRoomExitData();
            room.exitData.exits = new List<PrototypeRoomExit>();
            room.allowWallDecoration = false;
            room.allowFloorDecoration = false;
            room.Width = width;
            room.Height = height;
            return room;
        }

        public static void LogExampleRoomData()
        {
            Vector2[] vectorArray = new Vector2[]
            {
                new Vector2(4, 4),
                new Vector2(4, 14),
                new Vector2(14, 4),
                new Vector2(14, 14),
            };
            string[] guids = new string[]
            {
                "01972dee89fc4404a5c408d50007dad5",
                "7b0b1b6d9ce7405b86b75ce648025dd6",
                "ffdc8680bdaa487f8f31995539f74265",
                "01972dee89fc4404a5c408d50007dad5",
            };

            Vector2[] exits = new Vector2[]
            {
                new Vector2(0, 9),
                new Vector2(9, 0),
                new Vector2(20, 9),
                new Vector2(9, 20),
            };

            string[] dirs = new string[]
            {
                "EAST", "SOUTH", "WEST",  "NORTH"
            };

            RoomData rd = new RoomData()
            {
                enemyPositions = vectorArray,
                enemyGUIDs = guids,
                exitPositions = exits,
                exitDirections = dirs,

            };
            ToolsGAPI.Print("Data to JSON: " + JsonUtility.ToJson(rd));
        }

        public static void StraightLine()
        {
            try
            {
                Vector2[] enemyPositions = new Vector2[100];
                string[] enemyGuids = new string[100];
                int[] enemyLayers = new int[100];
                for (int i = 0; i < enemyGuids.Length; i++)
                {
                    var db = EnemyDatabase.Instance.Entries;
                    int r = Random.Range(0, db.Count);
                    enemyGuids[i] = db[r].encounterGuid;
                    enemyPositions[i] = new Vector2(i * 2, 10);
                    enemyLayers[i] = 0;
                }

                Vector2[] exits = new Vector2[]
                {
                new Vector2(0, 9),
                new Vector2(200, 9),
                };

                string[] dirs = new string[]
                {
                    "WEST", "EAST"
                };

                RoomData data = new RoomData()
                {
                    enemyPositions = enemyPositions,
                    enemyGUIDs = enemyGuids,
                    enemyReinforcementLayers = enemyLayers,
                    exitPositions = exits,
                    exitDirections = dirs,
                };
                ToolsGAPI.Log("Data to JSON: " + JsonUtility.ToJson(data));
            }
            catch (Exception e)
            {
                ToolsGAPI.PrintException(e);
            }
        }

        public struct RoomData
        {
            public string
                category,
                normalSubCategory,
                specialSubCategory,
                bossSubCategory;
            public Vector2[] enemyPositions;
            public string[] enemyGUIDs;
            public Vector2[] placeablePositions;
            public string[] placeableGUIDs;
            public int[] enemyReinforcementLayers;
            public Vector2[] exitPositions;
            public string[] exitDirections;
            public string[] floors;
            public float weight;
            public bool isSpecialRoom;
            public bool randomizeEnemyPositions, doFloorDecoration, doWallDecoration, doLighting;
            [NonSerialized]
            public PrototypeDungeonRoom room;
        }
    }
}
