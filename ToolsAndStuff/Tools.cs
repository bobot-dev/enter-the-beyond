﻿using Dungeonator;
using Gungeon;
using ItemAPI;
using Pathfinding;
//using Pathfinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using tk2dRuntime.TileMap;
using UnityEngine;

namespace BotsMod
{
	public static class Tools //1
	{
		public static GameObject Mines_Cave_In;
		public static AssetBundle AHHH;
		// Token: 0x06000172 RID: 370 RVA: 0x00013060 File Offset: 0x00011260
		public static void Init()
		{


			AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("shared_auto_001");
			AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
			shared_auto_001 = assetBundle3;
			shared_auto_002 = assetBundle2;

			Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");

			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			string text = "assets/data/goops/water goop.asset";
			GoopDefinition goopDefinition;
			try
			{
				GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
				goopDefinition = gameObject.GetComponent<GoopDefinition>();
			}
			catch
			{
				goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
			}
			goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
			Tools.DefaultWaterGoop = goopDefinition;
			text = "assets/data/goops/poison goop.asset";
			GoopDefinition goopDefinition2;
			try
			{
				GameObject gameObject2 = assetBundle.LoadAsset(text) as GameObject;
				goopDefinition2 = gameObject2.GetComponent<GoopDefinition>();
			}
			catch
			{
				goopDefinition2 = (assetBundle.LoadAsset(text) as GoopDefinition);
			}
			goopDefinition2.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
			Tools.DefaultPoisonGoop = goopDefinition2;
			text = "assets/data/goops/napalmgoopquickignite.asset";
			GoopDefinition goopDefinition3;
			try
			{
				GameObject gameObject3 = assetBundle.LoadAsset(text) as GameObject;
				goopDefinition3 = gameObject3.GetComponent<GoopDefinition>();
			}
			catch
			{
				goopDefinition3 = (assetBundle.LoadAsset(text) as GoopDefinition);
			}
			goopDefinition3.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
			Tools.DefaultFireGoop = goopDefinition3;
			PickupObject byId = PickupObjectDatabase.GetById(310);
			bool flag = byId == null;
			GoopDefinition defaultCharmGoop;
			if (flag)
			{
				defaultCharmGoop = null;
			}
			else
			{
				WingsItem component = byId.GetComponent<WingsItem>();
				defaultCharmGoop = ((component != null) ? component.RollGoop : null);
			}
			Tools.DefaultCharmGoop = defaultCharmGoop;
			Tools.DefaultCheeseGoop = (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0].cheeseEffect.CheeseGoop;
			Tools.DefaultBlobulonGoop = EnemyDatabase.GetOrLoadByGuid("0239c0680f9f467dbe5c4aab7dd1eca6").GetComponent<GoopDoer>().goopDefinition;
			Tools.DefaultPoopulonGoop = EnemyDatabase.GetOrLoadByGuid("116d09c26e624bca8cca09fc69c714b3").GetComponent<GoopDoer>().goopDefinition;
		}
		public static GameObject ProcessGameObject(this GameObject obj)
		{
			obj.SetActive(false);
			FakePrefab.MarkAsFakePrefab(obj);
			UnityEngine.Object.DontDestroyOnLoad(obj);
			return obj;
		}

		public static void AddItemToSynergy(this PickupObject obj, string nameKey)
		{
			AddItemToSynergy(nameKey, obj.PickupObjectId);
		}

		public static void AddItemToSynergy(string nameKey, int id)
		{
			foreach (AdvancedSynergyEntry entry in GameManager.Instance.SynergyManager.synergies)
			{
				if (entry.NameKey == nameKey)
				{
					if (PickupObjectDatabase.GetById(id) != null)
					{
						PickupObject obj = PickupObjectDatabase.GetById(id);
						if (obj is Gun)
						{
							if (entry.OptionalGunIDs != null)
							{
								entry.OptionalGunIDs.Add(id);
							}
							else
							{
								entry.OptionalGunIDs = new List<int> { id };
							}
						}
						else
						{
							if (entry.OptionalItemIDs != null)
							{
								entry.OptionalItemIDs.Add(id);
							}
							else
							{
								entry.OptionalItemIDs = new List<int> { id };
							}
						}
					}
				}
			}
		}


		public static void CreateAmmoType(string resourcePath, string name)
		{
			dfAtlas.ItemInfo newItem = new dfAtlas.ItemInfo();
			dfAtlas.ItemInfo newItem2 = new dfAtlas.ItemInfo();
			newItem.name = name + " tex";
			newItem2.name = name + " tex2";
			newItem.texture = ResourceExtractor.GetTextureFromResource(resourcePath+".png");
			newItem.sizeInPixels = new Vector2(newItem.texture.width, newItem.texture.height);
			newItem2.sizeInPixels = newItem.sizeInPixels;
			Texture2D atlas = GameUIRoot.Instance.heartControllers[0].armorSpritePrefab.Atlas.Texture;
			Rect region = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Items[49].region;

			for (int x = 0; x < newItem.texture.width; x++)
			{
				for (int y = 0; y < newItem.texture.height; y++)
				{
					atlas.SetPixel(x + (int)(region.xMin * 2048), y + (int)(region.yMin * 2048), newItem.texture.GetPixel(x, y));
				}
			}

			for (int x = 0; x < newItem.texture.width; x++)
			{
				for (int y = 0; y < newItem.texture.height; y++)
				{
					atlas.SetPixel(x + (int)(region.xMin * 2048) + 1 + newItem.texture.width, y + (int)(region.yMin * 2048) + 1 + newItem.texture.height, Color.gray);
				}
			}
			atlas.Apply(false, false);
			newItem.region = new Rect(region.xMin, region.yMin, (float)newItem.texture.width / 2048f, (float)newItem.texture.height / 2048f);
			newItem2.region = new Rect(region.xMin + (float)(1 + newItem.texture.width) / 2048f, region.yMin + (float)(1 + newItem.texture.height) / 2048f, (float)newItem.texture.width / 2048f, (float)newItem.texture.height / 2048f);
			GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.AddItem(newItem);
			GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.AddItem(newItem2);
			Array.Resize(ref GameUIRoot.Instance.ammoControllers[0].ammoTypes, GameUIRoot.Instance.ammoControllers[0].ammoTypes.Length + 1);
			int last_memeber = GameUIRoot.Instance.ammoControllers[0].ammoTypes.Length - 1;
			GameUIAmmoType type = new GameUIAmmoType
			{
				ammoType = GameUIAmmoType.AmmoType.CUSTOM,
				customAmmoType = name + "UIClip"
			};
			GameUIRoot.Instance.ammoControllers[0].ammoTypes[last_memeber] = type;
			GameObject ExampleBG = new GameObject(name + "BG");
			GameObject ExampleFG = new GameObject(name + "FG");
			ExampleBG.AddComponent<dfTiledSprite>();
			ExampleFG.AddComponent<dfTiledSprite>();
			type.ammoBarBG = ExampleBG.GetComponent<dfTiledSprite>();
			type.ammoBarFG = ExampleFG.GetComponent<dfTiledSprite>();
			type.ammoBarBG.SpriteName = name + " tex2";
			type.ammoBarFG.SpriteName = name + " tex";


		}

		public static void ReplaceSpriteInAtlas(string name, Texture2D tex, Texture2D BaseAtlas, Rect BaseRegion, dfAtlas dfAtlas)
		{
			dfAtlas.ItemInfo pain = new dfAtlas.ItemInfo();
			pain.name = name;
			pain.texture = tex;
			pain.sizeInPixels = new Vector2(15f, 13f);
			Texture2D atlas = BaseAtlas;
			Rect region = BaseRegion;

			for (int x = 0; x < pain.texture.width; x++)
			{
				for (int y = 0; y < pain.texture.height; y++)
				{
					atlas.SetPixel(x + (int)(region.xMin * 2048), y + (int)(region.yMin * 2048), pain.texture.GetPixel(x, y));
				}
			}
			atlas.Apply(false, false);
			pain.region = new Rect(region.xMin, region.yMin, (float)pain.texture.width / 2048f, (float)pain.texture.height / 2048f);
			dfAtlas.AddItem(pain);
		}

		public static void AddGlow(PlayerController player, float EmissivePower, float EmissiveColorPower, Color EmissiveColor)
		{
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			material.SetTexture("_MainTexture", player.sprite.renderer.material.GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", EmissiveColor);
			material.SetFloat("_EmissiveColorPower", EmissiveColorPower);
			material.SetFloat("_EmissivePower", EmissivePower);
			player.sprite.renderer.material = material;
			//result = material.shader.name;

		}


		


		public static GameObject GetPlayerPrefab(PlayableCharacters character)
		{
			string resourceName;

			if (character == PlayableCharacters.Soldier)
				resourceName = "marine";
			else if (character == PlayableCharacters.Pilot)
				resourceName = "rogue";
			else
				resourceName = character.ToString().ToLower();

			return (GameObject)BraveResources.Load("player" + resourceName);

		}
		public static string modID = "Bot";

		public static bool verbose = false;



		public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
		{
			if (verbose || force)
			{
				string[] lines = obj.ToString().Split('\n');
				foreach (var line in lines)
					LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
			}

			//Log(obj.ToString());
		}

		public static void PrintError<T>(T obj, string color = "FF0000")
		{
			string[] lines = obj.ToString().Split('\n');
			foreach (var line in lines)
				LogToConsole($"<color=#{color}>[{modID}] {line}</color>");

			//Log(obj.ToString());
		}

		private static Dictionary<string, float> timers = new Dictionary<string, float>();

		public static void StartTimer(string name)
		{
			string key = name.ToLower();
			if (timers.ContainsKey(key))
			{
				PrintError($"Timer {name} already exists.");
				return;
			}
			timers.Add(key, Time.realtimeSinceStartup);
		}

		public static void StopTimerAndReport(string name)
		{
			string key = name.ToLower();
			if (!timers.ContainsKey(key))
			{
				Tools.PrintError($"Could not stop timer {name}, no such timer exists");
				return;
			}
			float timerStart = timers[key];
			int elapsed = (int)((Time.realtimeSinceStartup - timerStart) * 1000);
			timers.Remove(key);
			Tools.Print($"{name} finished in " + elapsed + "ms");
		}

		public static void PrintException(Exception e, string color = "FF0000")
		{
			string message = e.Message + "\n" + e.StackTrace;
			{
				string[] lines = message.Split('\n');
				foreach (var line in lines)
					LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
			}

			//Log(e.Message);
			//Log("\t" + e.StackTrace);
		}

		public static void LogToConsole(string message)
		{
			message.Replace("\t", "    ");
			ETGModConsole.Log(message);
		}

		public static List<Texture2D> GetTexturesFromDirectory(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				//Tools.PrintError(directoryPath + " not found.");
				return null;
			}

			List<Texture2D> textures = new List<Texture2D>();
			foreach (string filePath in Directory.GetFiles(directoryPath))
			{
				if (!filePath.EndsWith(".png")) continue;

				Texture2D texture = BytesToTexture(File.ReadAllBytes(filePath), System.IO.Path.GetFileName(filePath).Replace(".png", ""));
				textures.Add(texture);
			}
			return textures;
		}

		public static Texture2D BytesToTexture(byte[] bytes, string resourceName)
		{
			Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			ImageConversion.LoadImage(texture, bytes);
			texture.filterMode = FilterMode.Point;
			texture.name = resourceName;
			return texture;
		}

		public static AIActor SummonAtRandomPosition(string guid, PlayerController owner)
		{
			return AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(guid), new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value), true, AIActor.AwakenAnimationType.Awaken, true);
		}

		public static RoomHandler AddCustomRuntimeRoom(PrototypeDungeonRoom prototype, bool addRoomToMinimap = true, bool addTeleporter = true, bool isSecretRatExitRoom = false, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.STANDARD)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component3 = gameObject3.GetComponent<tk2dTileMap>();
			string str = UnityEngine.Random.Range(10000, 99999).ToString();
			gameObject3.name = "Breach_RuntimeTilemap_" + str;
			component3.renderData.name = "Breach_RuntimeTilemap_" + str + " Render Data";

			component3.Editor__SpriteCollection = dungeon.tileIndices.dungeonCollection;

			TK2DDungeonAssembler.RuntimeResizeTileMap(component3, 8, 8, component3.partitionSizeX, component3.partitionSizeY);

			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component2 = gameObject2.GetComponent<tk2dTileMap>();
			//creepyRoom.OverrideTilemap = component;
			tk2dTileMap component4 = GameObject.Find("TileMap").GetComponent<tk2dTileMap>();
			tk2dTileMap mainTilemap = component4;
			//tk2dTileMap mainTilemap = dungeon.MainTilemap;

			if (mainTilemap == null)
			{
				ETGModConsole.Log("ERROR: TileMap object is null! Something seriously went wrong!", false);
				Debug.Log("ERROR: TileMap object is null! Something seriously went wrong!");
				return null;
			}
			TK2DDungeonAssembler tk2DDungeonAssembler = new TK2DDungeonAssembler();
			tk2DDungeonAssembler.Initialize(dungeon.tileIndices);
			IntVector2 zero = IntVector2.Zero;
			IntVector2 intVector = new IntVector2(50, 50);
			int x = intVector.x;
			int y = intVector.y;
			IntVector2 intVector2 = new IntVector2(int.MaxValue, int.MaxValue);
			IntVector2 lhs = new IntVector2(int.MinValue, int.MinValue);
			intVector2 = IntVector2.Min(intVector2, zero);
			IntVector2 intVector3 = IntVector2.Max(lhs, zero + new IntVector2(prototype.Width, prototype.Height)) - intVector2;
			IntVector2 b = IntVector2.Min(IntVector2.Zero, -1 * intVector2);
			intVector3 += b;
			IntVector2 intVector4 = new IntVector2(dungeon.data.Width + x, x);
			int newWidth = dungeon.data.Width + x * 2 + intVector3.x;
			int newHeight = Mathf.Max(dungeon.data.Height, intVector3.y + x * 2);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(dungeon.data.cellData, dungeon.data.Width, dungeon.data.Height, newWidth, newHeight);
			dungeon.data.cellData = array;
			dungeon.data.ClearCachedCellData();
			IntVector2 intVector5 = new IntVector2(prototype.Width, prototype.Height);
			IntVector2 b2 = zero + b;
			IntVector2 intVector6 = intVector4 + b2;
			CellArea cellArea = new CellArea(intVector6, intVector5, 0);
			cellArea.prototypeRoom = prototype;
			RoomHandler roomHandler = new RoomHandler(cellArea);
			for (int i = -x; i < intVector5.x + x; i++)
			{
				for (int j = -x; j < intVector5.y + x; j++)
				{
					IntVector2 intVector7 = new IntVector2(i, j) + intVector6;
					if ((i >= 0 && j >= 0 && i < intVector5.x && j < intVector5.y) || array[intVector7.x][intVector7.y] == null)
					{
						CellData cellData = new CellData(intVector7, CellType.WALL);
						cellData.positionInTilemap = cellData.positionInTilemap - intVector4 + new IntVector2(y, y);
						cellData.parentArea = cellArea;
						cellData.parentRoom = roomHandler;
						cellData.nearestRoom = roomHandler;
						cellData.distanceFromNearestRoom = 0f;
						array[intVector7.x][intVector7.y] = cellData;
					}
				}
			}
			dungeon.data.rooms.Add(roomHandler);
			try
			{
				roomHandler.WriteRoomData(dungeon.data);
			}
			catch (Exception)
			{
				ETGModConsole.Log("WARNING: Exception caused during WriteRoomData step on room: " + roomHandler.GetRoomName(), false);
			}
			try
			{
				dungeon.data.GenerateLightsForRoom(dungeon.decoSettings, roomHandler, GameObject.Find("_Lights").transform, lightStyle);
			}
			catch (Exception)
			{
				ETGModConsole.Log("WARNING: Exception caused during GeernateLightsForRoom step on room: " + roomHandler.GetRoomName(), false);
			}
			if (postProcessCellData != null)
			{
				postProcessCellData(roomHandler);
			}
			if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				roomHandler.BuildSecretRoomCover();
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
			string str2 = UnityEngine.Random.Range(10000, 99999).ToString();
			gameObject.name = "Glitch_RuntimeTilemap_" + str;
			component.renderData.name = "Glitch_RuntimeTilemap_" + str + " Render Data";
			component.Editor__SpriteCollection = dungeon.tileIndices.dungeonCollection;
			try
			{
				TK2DDungeonAssembler.RuntimeResizeTileMap(component, intVector3.x + y * 2, intVector3.y + y * 2, mainTilemap.partitionSizeX, mainTilemap.partitionSizeY);
				IntVector2 intVector8 = new IntVector2(prototype.Width, prototype.Height);
				IntVector2 b3 = zero + b;
				IntVector2 intVector9 = intVector4 + b3;
				for (int k = -y; k < intVector8.x + y; k++)
				{
					for (int l = -y; l < intVector8.y + y + 2; l++)
					{
						tk2DDungeonAssembler.BuildTileIndicesForCell(dungeon, component, intVector9.x + k, intVector9.y + l);
					}
				}
				RenderMeshBuilder.CurrentCellXOffset = intVector4.x - y;
				RenderMeshBuilder.CurrentCellYOffset = intVector4.y - y;
				component.ForceBuild();
				RenderMeshBuilder.CurrentCellXOffset = 0;
				RenderMeshBuilder.CurrentCellYOffset = 0;
				component.renderData.transform.position = new Vector3((float)(intVector4.x - y), (float)(intVector4.y - y), (float)(intVector4.y - y));
			}
			catch (Exception exception)
			{
				ETGModConsole.Log("WARNING: Exception occured during RuntimeResizeTileMap / RenderMeshBuilder steps!", false);
				Debug.Log("WARNING: Exception occured during RuntimeResizeTileMap/RenderMeshBuilder steps!");
				Debug.LogException(exception);
			}
			roomHandler.OverrideTilemap = component;
			for (int m = 0; m < roomHandler.area.dimensions.x; m++)
			{
				for (int n = 0; n < roomHandler.area.dimensions.y + 2; n++)
				{
					IntVector2 intVector10 = roomHandler.area.basePosition + new IntVector2(m, n);
					if (dungeon.data.CheckInBoundsAndValid(intVector10))
					{
						CellData currentCell = dungeon.data[intVector10];
						TK2DInteriorDecorator.PlaceLightDecorationForCell(dungeon, component, currentCell, intVector10);
					}
				}
			}
			Pathfinder.Instance.InitializeRegion(dungeon.data, roomHandler.area.basePosition + new IntVector2(-3, -3), roomHandler.area.dimensions + new IntVector2(3, 3));
			if (prototype.usesProceduralDecoration && prototype.allowFloorDecoration)
			{
				new TK2DInteriorDecorator(tk2DDungeonAssembler).HandleRoomDecoration(roomHandler, dungeon, mainTilemap);
			}
			roomHandler.PostGenerationCleanup();
			if (addRoomToMinimap)
			{
				roomHandler.visibility = RoomHandler.VisibilityStatus.VISITED;
				MonoBehaviour mono = new MonoBehaviour();
				mono.StartCoroutine(Minimap.Instance.RevealMinimapRoomInternal(roomHandler, true, true, false));
				if (isSecretRatExitRoom)
				{
					roomHandler.visibility = RoomHandler.VisibilityStatus.OBSCURED;
				}
			}
			if (addTeleporter)
			{
				roomHandler.AddProceduralTeleporterToRoom();
			}
			if (addRoomToMinimap)
			{
				Minimap.Instance.InitializeMinimap(dungeon.data);
			}
			DeadlyDeadlyGoopManager.ReinitializeData();
			return roomHandler;
		}

		public static void AddEnemyToDatabase2(GameObject EnemyPrefab, string EnemyGUID, bool isInBossTab = false, bool IsNormalEnemy = true)
		{
			EnemyDatabaseEntry item = new EnemyDatabaseEntry
			{
				myGuid = EnemyGUID,
				placeableWidth = 2,
				placeableHeight = 2,
				isNormalEnemy = IsNormalEnemy,
				path = EnemyGUID,
				isInBossTab = isInBossTab,
				encounterGuid = EnemyGUID
			};
			EnemyDatabase.Instance.Entries.Add(item);
			//SpecialResources.resources.Add(EnemyGUID, EnemyPrefab);
			EncounterDatabaseEntry encounterDatabaseEntry = new EncounterDatabaseEntry(EnemyPrefab.GetComponent<AIActor>().encounterTrackable)
			{
				path = EnemyGUID,
				myGuid = EnemyPrefab.GetComponent<AIActor>().encounterTrackable.EncounterGuid
			};
			EncounterDatabase.Instance.Entries.Add(encounterDatabaseEntry);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00013268 File Offset: 0x00011468
		public static GameActorEffect CopyEffectFrom(this GameActorEffect self, GameActorEffect other)
		{
			bool flag = self == null || other == null;
			GameActorEffect result;
			if (flag)
			{
				result = null;
			}
			else
			{
				self.AffectsPlayers = other.AffectsPlayers;
				self.AffectsEnemies = other.AffectsEnemies;
				self.effectIdentifier = other.effectIdentifier;
				self.resistanceType = other.resistanceType;
				self.stackMode = other.stackMode;
				self.duration = other.duration;
				self.maxStackedDuration = other.maxStackedDuration;
				self.AppliesTint = other.AppliesTint;
				self.TintColor = new Color
				{
					r = other.TintColor.r,
					g = other.TintColor.g,
					b = other.TintColor.b
				};
				self.AppliesDeathTint = other.AppliesDeathTint;
				self.DeathTintColor = new Color
				{
					r = other.DeathTintColor.r,
					g = other.DeathTintColor.g,
					b = other.DeathTintColor.b
				};
				self.AppliesOutlineTint = other.AppliesOutlineTint;
				self.OutlineTintColor = new Color
				{
					r = other.OutlineTintColor.r,
					g = other.OutlineTintColor.g,
					b = other.OutlineTintColor.b
				};
				self.OverheadVFX = other.OverheadVFX;
				self.PlaysVFXOnActor = other.PlaysVFXOnActor;
				result = self;
			}
			return result;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000133F0 File Offset: 0x000115F0


		// Token: 0x06000175 RID: 373 RVA: 0x00013490 File Offset: 0x00011690
		public static bool PlayerHasCompletionItem(this PlayerController player)
		{
			bool result = false;
			bool flag = player != null && player.passiveItems != null;
			if (flag)
			{
				foreach (PassiveItem passiveItem in player.passiveItems)
				{
					bool flag2 = passiveItem is SynergyCompletionItem;
					if (flag2)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00013518 File Offset: 0x00011718
		public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
		{
			foreach (int num in player.ActiveExtraSynergies)
			{
				AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[num];
				bool flag = advancedSynergyEntry.NameKey == synergyNameToCheck;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00013598 File Offset: 0x00011798
		public static tk2dSpriteDefinition CopyDefinitionFrom(this tk2dSpriteDefinition other)
		{
			return new tk2dSpriteDefinition
			{
				boundsDataCenter = other.boundsDataCenter,
				boundsDataExtents = other.boundsDataExtents,
				colliderConvex = other.colliderConvex,
				colliderSmoothSphereCollisions = other.colliderSmoothSphereCollisions,
				colliderType = other.colliderType,
				colliderVertices = other.colliderVertices,
				collisionLayer = other.collisionLayer,
				complexGeometry = other.complexGeometry,
				extractRegion = other.extractRegion,
				flipped = other.flipped,
				indices = other.indices,
				material = new Material(other.material),
				materialId = other.materialId,
				materialInst = new Material(other.materialInst),
				metadata = other.metadata,
				name = other.name,
				normals = other.normals,
				physicsEngine = other.physicsEngine,
				position0 = other.position0,
				position1 = other.position1,
				position2 = other.position2,
				position3 = other.position3,
				regionH = other.regionH,
				regionW = other.regionW,
				regionX = other.regionX,
				regionY = other.regionY,
				tangents = other.tangents,
				texelSize = other.texelSize,
				untrimmedBoundsDataCenter = other.untrimmedBoundsDataCenter,
				untrimmedBoundsDataExtents = other.untrimmedBoundsDataExtents,
				uvs = other.uvs
			};
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00013730 File Offset: 0x00011930
		public static Gun GetGunById(this PickupObjectDatabase database, int id)
		{
			return PickupObjectDatabase.GetById(id) as Gun;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00013750 File Offset: 0x00011950
		public static Gun GetGunById(int id)
		{
			return Tools.GetGunById((PickupObjectDatabase)null, id);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001376C File Offset: 0x0001196C
		public static ExplosionData CopyExplosionData(this ExplosionData other)
		{
			return new ExplosionData
			{
				useDefaultExplosion = other.useDefaultExplosion,
				doDamage = other.doDamage,
				forceUseThisRadius = other.forceUseThisRadius,
				damageRadius = other.damageRadius,
				damageToPlayer = other.damageToPlayer,
				damage = other.damage,
				breakSecretWalls = other.breakSecretWalls,
				secretWallsRadius = other.secretWallsRadius,
				forcePreventSecretWallDamage = other.forcePreventSecretWallDamage,
				doDestroyProjectiles = other.doDestroyProjectiles,
				doForce = other.doForce,
				pushRadius = other.pushRadius,
				force = other.force,
				debrisForce = other.debrisForce,
				preventPlayerForce = other.preventPlayerForce,
				explosionDelay = other.explosionDelay,
				usesComprehensiveDelay = other.usesComprehensiveDelay,
				comprehensiveDelay = other.comprehensiveDelay,
				effect = other.effect,
				doScreenShake = other.doScreenShake,
				ss = new ScreenShakeSettings
				{
					magnitude = other.ss.magnitude,
					speed = other.ss.speed,
					time = other.ss.time,
					falloff = other.ss.falloff,
					direction = new Vector2
					{
						x = other.ss.direction.x,
						y = other.ss.direction.y
					},
					vibrationType = other.ss.vibrationType,
					simpleVibrationTime = other.ss.simpleVibrationTime,
					simpleVibrationStrength = other.ss.simpleVibrationStrength
				},
				doStickyFriction = other.doStickyFriction,
				doExplosionRing = other.doExplosionRing,
				isFreezeExplosion = other.isFreezeExplosion,
				freezeRadius = other.freezeRadius,
				freezeEffect = other.freezeEffect,
				playDefaultSFX = other.playDefaultSFX,
				IsChandelierExplosion = other.IsChandelierExplosion,
				rotateEffectToNormal = other.rotateEffectToNormal,
				ignoreList = other.ignoreList,
				overrideRangeIndicatorEffect = other.overrideRangeIndicatorEffect
			};
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000139AC File Offset: 0x00011BAC
		public static void SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, bool lightened = true, tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.LowerLeft, bool anchorChangesCollider = true, int? overrideColliderPixelWidth = null,
	 int? overrideColliderPixelHeight = null, int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
		{
			try
			{
				proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
				tk2dSpriteDefinition def = Tools.SetupDefinitionForProjectileSprite(name, proj.GetAnySprite().spriteId, pixelWidth, pixelHeight, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX,
					overrideColliderOffsetY, overrideProjectileToCopyFrom);
				def.ConstructOffsetsFromAnchor(anchor, def.position3);
				proj.GetAnySprite().scale = new Vector3(1f, 1f, 1f);
				proj.transform.localScale = new Vector3(1f, 1f, 1f);
				proj.GetAnySprite().transform.localScale = new Vector3(1f, 1f, 1f);
				proj.AdditionalScaleMultiplier = 1f;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:");
				ETGModConsole.Log(ex.ToString());
			}
		}

		public static tk2dSpriteDefinition SetupDefinitionForProjectileSprite(string name, int id, int pixelWidth, int pixelHeight, bool lightened = true, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null,
			int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
		{
			if (overrideColliderPixelWidth == null)
			{
				overrideColliderPixelWidth = pixelWidth;
			}
			if (overrideColliderPixelHeight == null)
			{
				overrideColliderPixelHeight = pixelHeight;
			}
			if (overrideColliderOffsetX == null)
			{
				overrideColliderOffsetX = 0;
			}
			if (overrideColliderOffsetY == null)
			{
				overrideColliderOffsetY = 0;
			}
			float thing = 16;
			float thing2 = 16;
			float trueWidth = (float)pixelWidth / thing;
			float trueHeight = (float)pixelHeight / thing;
			float colliderWidth = (float)overrideColliderPixelWidth.Value / thing2;
			float colliderHeight = (float)overrideColliderPixelHeight.Value / thing2;
			float colliderOffsetX = (float)overrideColliderOffsetX.Value / thing2;
			float colliderOffsetY = (float)overrideColliderOffsetY.Value / thing2;
			tk2dSpriteDefinition def = ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[(overrideProjectileToCopyFrom ??
					(PickupObjectDatabase.GetById(lightened ? 47 : 12) as Gun).DefaultModule.projectiles[0]).GetAnySprite().spriteId].CopyDefinitionFrom();
			def.boundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
			def.boundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
			def.untrimmedBoundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
			def.untrimmedBoundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
			def.texelSize = new Vector2(1 / 16f, 1 / 16f);
			def.position0 = new Vector3(0f, 0f, 0f);
			def.position1 = new Vector3(0f + trueWidth, 0f, 0f);
			def.position2 = new Vector3(0f, 0f + trueHeight, 0f);
			def.position3 = new Vector3(0f + trueWidth, 0f + trueHeight, 0f);
			def.colliderVertices[0].x = colliderOffsetX;
			def.colliderVertices[0].y = colliderOffsetY;
			def.colliderVertices[1].x = colliderWidth;
			def.colliderVertices[1].y = colliderHeight;
			def.name = name;
			ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[id] = def;
			return def;
		}

		public static void MakeOffset(this tk2dSpriteDefinition def, Vector2 offset, bool changesCollider = false)
		{
			float xOffset = offset.x;
			float yOffset = offset.y;
			def.position0 += new Vector3(xOffset, yOffset, 0);
			def.position1 += new Vector3(xOffset, yOffset, 0);
			def.position2 += new Vector3(xOffset, yOffset, 0);
			def.position3 += new Vector3(xOffset, yOffset, 0);
			def.boundsDataCenter += new Vector3(xOffset, yOffset, 0);
			def.boundsDataExtents += new Vector3(xOffset, yOffset, 0);
			def.untrimmedBoundsDataCenter += new Vector3(xOffset, yOffset, 0);
			def.untrimmedBoundsDataExtents += new Vector3(xOffset, yOffset, 0);
			if (def.colliderVertices != null && def.colliderVertices.Length > 0 && changesCollider)
			{
				def.colliderVertices[0] += new Vector3(xOffset, yOffset, 0);
			}
		}

		public static void ConstructOffsetsFromAnchor(this tk2dSpriteDefinition def, tk2dBaseSprite.Anchor anchor, Vector2 scale, bool changesCollider = true)
		{
			float xOffset = 0;
			if (anchor == tk2dBaseSprite.Anchor.LowerCenter || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.UpperCenter)
			{
				xOffset = -(scale.x / 2f);
			}
			else if (anchor == tk2dBaseSprite.Anchor.LowerRight || anchor == tk2dBaseSprite.Anchor.MiddleRight || anchor == tk2dBaseSprite.Anchor.UpperRight)
			{
				xOffset = -scale.x;
			}
			float yOffset = 0;
			if (anchor == tk2dBaseSprite.Anchor.MiddleLeft || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.MiddleLeft)
			{
				yOffset = -(scale.y / 2f);
			}
			else if (anchor == tk2dBaseSprite.Anchor.UpperLeft || anchor == tk2dBaseSprite.Anchor.UpperCenter || anchor == tk2dBaseSprite.Anchor.UpperRight)
			{
				yOffset = -scale.y;
			}
			def.MakeOffset(new Vector2(xOffset, yOffset), changesCollider);
		}


		// Token: 0x0600017C RID: 380 RVA: 0x00013BFC File Offset: 0x00011DFC
		public static StatModifier SetupStatModifier(PlayerStats.StatType statType, float modificationAmount, StatModifier.ModifyMethod modifyMethod = StatModifier.ModifyMethod.ADDITIVE, bool breaksOnDamage = false)
		{
			return new StatModifier
			{
				statToBoost = statType,
				amount = modificationAmount,
				modifyType = modifyMethod,
				isMeatBunBuff = breaksOnDamage
			};
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00013C30 File Offset: 0x00011E30
		public static GameActorCharmEffect CopyCharmFrom(this GameActorCharmEffect self, GameActorCharmEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorCharmEffect();
			}
			return (GameActorCharmEffect)self.CopyEffectFrom(other);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00013C60 File Offset: 0x00011E60
		public static GameActorFireEffect CopyFireFrom(this GameActorFireEffect self, GameActorFireEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorFireEffect();
			}
			bool flag2 = other == null;
			GameActorFireEffect result;
			if (flag2)
			{
				result = self;
			}
			else
			{
				self = (GameActorFireEffect)self.CopyEffectFrom(other);
				self.FlameVfx = new List<GameObject>();
				foreach (GameObject item in other.FlameVfx)
				{
					self.FlameVfx.Add(item);
				}
				self.flameNumPerSquareUnit = other.flameNumPerSquareUnit;
				self.flameBuffer = new Vector2
				{
					x = other.flameBuffer.x,
					y = other.flameBuffer.y
				};
				self.flameFpsVariation = other.flameFpsVariation;
				self.flameMoveChance = other.flameMoveChance;
				self.IsGreenFire = other.IsGreenFire;
				result = self;
			}
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00013D64 File Offset: 0x00011F64
		public static GameActorHealthEffect CopyPoisonFrom(this GameActorHealthEffect self, GameActorHealthEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorHealthEffect();
			}
			bool flag2 = other == null;
			GameActorHealthEffect result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				self.CopyEffectFrom(other);
				self.DamagePerSecondToEnemies = other.DamagePerSecondToEnemies;
				self.ignitesGoops = other.ignitesGoops;
				result = self;
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00013DB8 File Offset: 0x00011FB8
		public static GameActorSpeedEffect CopySpeedFrom(this GameActorSpeedEffect self, GameActorSpeedEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorSpeedEffect();
			}
			bool flag2 = other == null;
			GameActorSpeedEffect result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				self.CopyEffectFrom(other);
				self.SpeedMultiplier = other.SpeedMultiplier;
				self.CooldownMultiplier = other.CooldownMultiplier;
				self.OnlyAffectPlayerWhenGrounded = other.OnlyAffectPlayerWhenGrounded;
				result = self;
			}
			return result;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00013E18 File Offset: 0x00012018
		public static GameActorFreezeEffect CopyFreezeFrom(this GameActorFreezeEffect self, GameActorFreezeEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorFreezeEffect();
			}
			bool flag2 = other == null;
			GameActorFreezeEffect result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				self.CopyEffectFrom(other);
				self.FreezeAmount = other.FreezeAmount;
				self.UnfreezeDamagePercent = other.UnfreezeDamagePercent;
				self.FreezeCrystals = new List<GameObject>();
				foreach (GameObject item in other.FreezeCrystals)
				{
					self.FreezeCrystals.Add(item);
				}
				self.crystalNum = other.crystalNum;
				self.crystalRot = other.crystalRot;
				self.crystalVariation = new Vector2
				{
					x = other.crystalVariation.x,
					y = other.crystalVariation.y
				};
				self.debrisMinForce = other.debrisMinForce;
				self.debrisMaxForce = other.debrisMaxForce;
				self.debrisAngleVariance = other.debrisAngleVariance;
				self.vfxExplosion = other.vfxExplosion;
				result = self;
			}
			return result;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00013F48 File Offset: 0x00012148
		public static GameActorBleedEffect CopyBleedFrom(this GameActorBleedEffect self, GameActorBleedEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorBleedEffect();
			}
			bool flag2 = other == null;
			GameActorBleedEffect result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				self.CopyEffectFrom(other);
				self.ChargeAmount = other.ChargeAmount;
				self.ChargeDispelFactor = other.ChargeDispelFactor;
				self.vfxChargingReticle = other.vfxChargingReticle;
				self.vfxExplosion = other.vfxExplosion;
				result = self;
			}
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00013FB4 File Offset: 0x000121B4
		public static GameActorCheeseEffect CopyCheeseFrom(this GameActorCheeseEffect self, GameActorCheeseEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorCheeseEffect();
			}
			bool flag2 = other == null;
			GameActorCheeseEffect result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				self.CopyEffectFrom(other);
				self.CheeseAmount = other.CheeseAmount;
				self.CheeseGoop = other.CheeseGoop;
				self.CheeseGoopRadius = other.CheeseGoopRadius;
				self.CheeseCrystals = new List<GameObject>();
				foreach (GameObject item in other.CheeseCrystals)
				{
					self.CheeseCrystals.Add(item);
				}
				self.crystalNum = other.crystalNum;
				self.crystalRot = other.crystalRot;
				self.crystalVariation = new Vector2
				{
					x = other.crystalVariation.x,
					y = other.crystalVariation.y
				};
				self.debrisMinForce = other.debrisMinForce;
				self.debrisMaxForce = other.debrisMaxForce;
				self.debrisAngleVariance = other.debrisAngleVariance;
				self.vfxExplosion = other.vfxExplosion;
				result = self;
			}
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000140F0 File Offset: 0x000122F0
	

		// Token: 0x0600018A RID: 394 RVA: 0x00014158 File Offset: 0x00012358
		public static void SetupUnlockOnFlag(this PickupObject self, GungeonFlags flag, bool requiredFlagValue)
		{
			EncounterTrackable encounterTrackable = self.encounterTrackable;
			bool flag2 = encounterTrackable.prerequisites == null;
			if (flag2)
			{
				encounterTrackable.prerequisites = new DungeonPrerequisite[1];
				encounterTrackable.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
					requireFlag = requiredFlagValue,
					saveFlagToCheck = flag
				};
			}
			else
			{
				encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
						requireFlag = requiredFlagValue,
						saveFlagToCheck = flag
					}
				}).ToArray<DungeonPrerequisite>();
			}
			EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
			bool flag3 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
			if (flag3)
			{
				entry.ProxyEncounterGuid = "";
			}
			bool flag4 = entry.prerequisites == null;
			if (flag4)
			{
				entry.prerequisites = new DungeonPrerequisite[1];
				entry.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
					requireFlag = requiredFlagValue,
					saveFlagToCheck = flag
				};
			}
			else
			{
				entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
						requireFlag = requiredFlagValue,
						saveFlagToCheck = flag
					}
				}).ToArray<DungeonPrerequisite>();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00014290 File Offset: 0x00012490
		public static void SetupUnlockOnStat(this PickupObject self, TrackedStats stat, DungeonPrerequisite.PrerequisiteOperation operation, int comparisonValue)
		{
			EncounterTrackable encounterTrackable = self.encounterTrackable;
			bool flag = encounterTrackable.prerequisites == null;
			if (flag)
			{
				encounterTrackable.prerequisites = new DungeonPrerequisite[1];
				encounterTrackable.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
					prerequisiteOperation = operation,
					statToCheck = stat,
					comparisonValue = (float)comparisonValue
				};
			}
			else
			{
				encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
						prerequisiteOperation = operation,
						statToCheck = stat,
						comparisonValue = (float)comparisonValue
					}
				}).ToArray<DungeonPrerequisite>();
			}
			EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
			bool flag2 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
			if (flag2)
			{
				entry.ProxyEncounterGuid = "";
			}
			bool flag3 = entry.prerequisites == null;
			if (flag3)
			{
				entry.prerequisites = new DungeonPrerequisite[1];
				entry.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
					prerequisiteOperation = operation,
					statToCheck = stat,
					comparisonValue = (float)comparisonValue
				};
			}
			else
			{
				entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
						prerequisiteOperation = operation,
						statToCheck = stat,
						comparisonValue = (float)comparisonValue
					}
				}).ToArray<DungeonPrerequisite>();
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000143E8 File Offset: 0x000125E8
		public static Tools.ChestType GetChestType(this Chest chest)
		{
			bool flag = chest.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW;
			Tools.ChestType result;
			if (flag)
			{
				result = Tools.ChestType.SECRET_RAINBOW;
			}
			else
			{
				bool flag2 = chest.ChestIdentifier == Chest.SpecialChestIdentifier.RAT;
				if (flag2)
				{
					result = Tools.ChestType.RAT_REWARD;
				}
				else
				{
					bool isRainbowChest = chest.IsRainbowChest;
					if (isRainbowChest)
					{
						result = Tools.ChestType.RAINBOW;
					}
					else
					{
						bool isGlitched = chest.IsGlitched;
						if (isGlitched)
						{
							result = Tools.ChestType.GLITCHED;
						}
						else
						{
							bool flag3 = chest.breakAnimName.Contains("wood");
							if (flag3)
							{
								result = Tools.ChestType.BROWN;
							}
							else
							{
								bool flag4 = chest.breakAnimName.Contains("silver");
								if (flag4)
								{
									result = Tools.ChestType.BLUE;
								}
								else
								{
									bool flag5 = chest.breakAnimName.Contains("green");
									if (flag5)
									{
										result = Tools.ChestType.GREEN;
									}
									else
									{
										bool flag6 = chest.breakAnimName.Contains("redgold");
										if (flag6)
										{
											result = Tools.ChestType.RED;
										}
										else
										{
											bool flag7 = chest.breakAnimName.Contains("black");
											if (flag7)
											{
												result = Tools.ChestType.BLACK;
											}
											else
											{
												bool flag8 = chest.breakAnimName.Contains("synergy");
												if (flag8)
												{
													result = Tools.ChestType.SYNERGY;
												}
												else
												{
													result = Tools.ChestType.UNIDENTIFIED;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000144F5 File Offset: 0x000126F5
		public static void PlaceItemInAmmonomiconAfterItemById(this PickupObject item, int id)
		{
			item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(id).ForcedPositionInAmmonomicon;
		}

		// Token: 0x0400028B RID: 651
		public static GoopDefinition DefaultWaterGoop;

		// Token: 0x0400028C RID: 652
		public static GoopDefinition DefaultFireGoop;

		// Token: 0x0400028D RID: 653
		public static GoopDefinition DefaultPoisonGoop;

		// Token: 0x0400028E RID: 654
		public static GoopDefinition DefaultCharmGoop;

		// Token: 0x0400028F RID: 655
		public static GoopDefinition DefaultBlobulonGoop;

		// Token: 0x04000290 RID: 656
		public static GoopDefinition DefaultPoopulonGoop;

		// Token: 0x04000291 RID: 657
		public static GoopDefinition DefaultCheeseGoop;

		// Token: 0x02000093 RID: 147
		public enum ChestType
		{
			// Token: 0x040003C5 RID: 965
			BROWN = 1,
			// Token: 0x040003C6 RID: 966
			BLUE,
			// Token: 0x040003C7 RID: 967
			GREEN,
			// Token: 0x040003C8 RID: 968
			RED,
			// Token: 0x040003C9 RID: 969
			BLACK,
			// Token: 0x040003CA RID: 970
			SYNERGY = -1,
			// Token: 0x040003CB RID: 971
			RAINBOW = -2,
			// Token: 0x040003CC RID: 972
			SECRET_RAINBOW = -3,
			// Token: 0x040003CD RID: 973
			GLITCHED = -4,
			// Token: 0x040003CE RID: 974
			RAT_REWARD = -5,
			// Token: 0x040003CF RID: 975
			UNIDENTIFIED = -50
		}

		public static List<int> CastleWallIDs = new List<int>
		{
			22,
			23,
			24,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			62,
			73,
			74,
			75,
			76,
			80,
			81,
			82,
			83,
			95,
			96,
			97,
			98,
			176,
			332,
			333,
			334,
			354,
			355,
			356
		};


		///SpApi code
		///

		public static void AddEnemyToDatabase(GameObject EnemyPrefab, string EnemyGUID, bool isInBossTab = false, bool IsNormalEnemy = true, bool AddToMTGSpawnPool = true)
		{
			EnemyDatabaseEntry item = new EnemyDatabaseEntry
			{
				myGuid = EnemyGUID,
				placeableWidth = 2,
				placeableHeight = 2,
				isNormalEnemy = IsNormalEnemy,
				path = EnemyGUID,
				isInBossTab = isInBossTab,
				encounterGuid = EnemyGUID
			};
			EnemyDatabase.Instance.Entries.Add(item);
			
			EncounterDatabaseEntry encounterDatabaseEntry = new EncounterDatabaseEntry(EnemyPrefab.GetComponent<AIActor>().encounterTrackable)
			{
				path = EnemyGUID,
				myGuid = EnemyPrefab.GetComponent<AIActor>().encounterTrackable.EncounterGuid
			};
			EncounterDatabase.Instance.Entries.Add(encounterDatabaseEntry);
			if (AddToMTGSpawnPool && !string.IsNullOrEmpty(EnemyPrefab.GetComponent<AIActor>().ActorName))
			{
				string EnemyName = "bot:" + EnemyPrefab.GetComponent<AIActor>().ActorName.Replace(" ", "_").Replace("(", "_").Replace(")", string.Empty).ToLower();
				if (!Game.Enemies.ContainsID(EnemyName)) { Game.Enemies.Add(EnemyName, EnemyPrefab.GetComponent<AIActor>()); }
			}
		}

		public static void Add<T>(ref T[] array, T toAdd)
		{
			List<T> list = array.ToList();
			list.Add(toAdd);
			array = list.ToArray<T>();
		}

		public static AssetBundle shared_auto_002;
		public static AssetBundle shared_auto_001;

		public static DungeonFlowNode GenerateFlowNode(DungeonFlow flow, PrototypeDungeonRoom.RoomCategory category, PrototypeDungeonRoom overrideRoom = null, GenericRoomTable overrideRoomTable = null, bool loopTargetIsOneWay = false, bool isWarpWing = false,
		   bool handlesOwnWarping = true, float weight = 1f, DungeonFlowNode.NodePriority priority = DungeonFlowNode.NodePriority.MANDATORY, string guid = "")
		{
			if (string.IsNullOrEmpty(guid))
			{
				guid = Guid.NewGuid().ToString();
			}
			DungeonFlowNode node = new DungeonFlowNode(flow)
			{
				isSubchainStandin = false,
				nodeType = DungeonFlowNode.ControlNodeType.ROOM,
				roomCategory = category,
				percentChance = weight,
				priority = priority,
				overrideExactRoom = overrideRoom,
				overrideRoomTable = overrideRoomTable,
				capSubchain = false,
				subchainIdentifier = string.Empty,
				limitedCopiesOfSubchain = false,
				maxCopiesOfSubchain = 1,
				subchainIdentifiers = new List<string>(0),
				receivesCaps = false,
				isWarpWingEntrance = isWarpWing,
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
				guidAsString = guid,
				parentNodeGuid = string.Empty,
				childNodeGuids = new List<string>(0),
				loopTargetNodeGuid = string.Empty,
				loopTargetIsOneWay = loopTargetIsOneWay,
				flow = flow
			};
			return node;
		}

		public static GenericLootTable LoadShopTable(string assetName)
		{
			return Tools.shared_auto_001.LoadAsset<GenericLootTable>(assetName);
		}

		///scary apache code
		///
		public static Dungeon GetOrLoadByName_Orig(string name)
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("dungeons/" + name.ToLower());
			DebugTime.RecordStartTime();
			Dungeon component = assetBundle.LoadAsset<GameObject>(name).GetComponent<Dungeon>();
			DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
			return component;
		}

		public static TileIndexGrid DeserializeTileIndexGrid(string assetPath)
		{
			TileIndexGrid m_TileIndexGridData = ScriptableObject.CreateInstance<TileIndexGrid>();
			string serializedData = ResourceExtractor.BuildStringFromEmbeddedResource("SerializedData/" + assetPath);
			JsonUtility.FromJsonOverwrite(serializedData, m_TileIndexGridData);
			return m_TileIndexGridData;
		}

		public static void ApplyGlitchShader(tk2dBaseSprite sprite, bool usesOverrideMaterial = true, float GlitchInterval = 0.1f, float DispProbability = 0.4f, float DispIntensity = 0.01f, float ColorProbability = 0.4f, float ColorIntensity = 0.04f)
		{
			try
			{
				if (sprite == null) { return; }
				// Material m_cachedMaterial = new Material(ShaderCache.Acquire("Brave/Internal/Glitch"));
				
				//Material m_cachedMaterial = new Material(ResourceManager.LoadAssetBundle("ExpandSharedAuto").LoadAsset<Material>("ExpandGlitchBasicMaterial").shader);
				Material m_cachedMaterial = new Material(AHHH.LoadAsset<Shader>("assets/customglitchshader.shader"));
				//m_cachedMaterial.shader = AHHH.LoadAsset<Shader>("assets/customglitchshader.shader");
				m_cachedMaterial.name = "GlitchMaterial";
				m_cachedMaterial.SetFloat("_GlitchInterval", GlitchInterval);
				m_cachedMaterial.SetFloat("_DispProbability", DispProbability);
				m_cachedMaterial.SetFloat("_DispIntensity", DispIntensity);
				m_cachedMaterial.SetFloat("_ColorProbability", ColorProbability);
				m_cachedMaterial.SetFloat("_ColorIntensity", ColorIntensity);

				m_cachedMaterial.SetFloat("_WrapDispCoords", 0);

				MeshRenderer spriteComponent = sprite.GetComponent<MeshRenderer>();
				if (spriteComponent == null) { return; }

				Material[] sharedMaterials = spriteComponent.sharedMaterials;
				if (sharedMaterials == null) { return; }
				if (sharedMaterials.Length > 0)
				{
					foreach (Material material in sharedMaterials)
					{
						if (material.name.ToLower().StartsWith("glitchmaterial")) { return; }
						if (material.name.ToLower().StartsWith("hologrammaterial")) { return; }
						if (material.name.ToLower().StartsWith("galaxymaterial")) { return; }
						if (material.name.ToLower().StartsWith("spacematerial")) { return; }
						if (material.name.ToLower().StartsWith("paradoxmaterial")) { return; }
						if (material.name.ToLower().StartsWith("cosmichorrormaterial")) { return; }
						if (material.name.ToLower().StartsWith("rainbowmaterial")) { return; }
					}
				}
				Array.Resize(ref sharedMaterials, sharedMaterials.Length + 1);
				// Material CustomMaterial = Instantiate(m_cachedGlitchMaterial);
				if (sharedMaterials[0].GetTexture("_MainTex") == null) { return; }
				m_cachedMaterial.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
				sharedMaterials[sharedMaterials.Length - 1] = m_cachedMaterial;
				spriteComponent.sharedMaterials = sharedMaterials;
				sprite.usesOverrideMaterial = usesOverrideMaterial;
			}
			catch (Exception ex)
			{

				ETGModConsole.Log("[ExpandTheGungeon] Warning: Caught exception in ExpandShaders.ApplyGlitchShader!");
				ETGModConsole.Log("[ExpandTheGungeon] Warning: Caught exception in ExpandShaders.ApplyGlitchShader!");
				ETGModConsole.Log(ex.ToString());

				return;
			}
		}

		public static void GenerateSpriteAnimator(GameObject targetObject, tk2dSpriteAnimation library = null, int DefaultClipId = 0, float AdditionalCameraVisibilityRadius = 0f, bool AnimateDuringBossIntros = false, bool AlwaysIgnoreTimeScale = false, bool ignoreTimeScale = false, bool ForceSetEveryFrame = false, bool playAutomatically = false, bool IsFrameBlendedAnimation = false, float clipTime = 0f, float ClipFps = 15f, bool deferNextStartClip = false, bool alwaysUpdateOffscreen = false, bool maximumDeltaOneFrame = false)
		{
			if (targetObject.GetComponent<tk2dSpriteAnimator>())
			{
				UnityEngine.Object.Destroy(targetObject.GetComponent<tk2dSpriteAnimator>());
			}
			tk2dSpriteAnimator tk2dSpriteAnimator = targetObject.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimator.Library = library;
			tk2dSpriteAnimator.DefaultClipId = DefaultClipId;
			tk2dSpriteAnimator.AdditionalCameraVisibilityRadius = AdditionalCameraVisibilityRadius;
			tk2dSpriteAnimator.AnimateDuringBossIntros = AnimateDuringBossIntros;
			tk2dSpriteAnimator.AlwaysIgnoreTimeScale = AlwaysIgnoreTimeScale;
			tk2dSpriteAnimator.ignoreTimeScale = ignoreTimeScale;
			tk2dSpriteAnimator.ForceSetEveryFrame = ForceSetEveryFrame;
			tk2dSpriteAnimator.playAutomatically = playAutomatically;
			tk2dSpriteAnimator.IsFrameBlendedAnimation = IsFrameBlendedAnimation;
			tk2dSpriteAnimator.clipTime = clipTime;
			tk2dSpriteAnimator.ClipFps = ClipFps;
			tk2dSpriteAnimator.deferNextStartClip = deferNextStartClip;
			tk2dSpriteAnimator.alwaysUpdateOffscreen = alwaysUpdateOffscreen;
			tk2dSpriteAnimator.maximumDeltaOneFrame = maximumDeltaOneFrame;
		}




		public static tk2dSpriteAnimationClip AddAnimation(tk2dSpriteAnimator targetAnimator, tk2dSpriteCollectionData collection, List<string> spriteNameList, string clipName, tk2dSpriteAnimationClip.WrapMode wrapMode = tk2dSpriteAnimationClip.WrapMode.Once, int frameRate = 15, int loopStart = 0, float minFidgetDuration = 0.5f, float maxFidgetDuration = 1f)
		{
			try
			{

				if (!targetAnimator.Library)
				{
					targetAnimator.Library = targetAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
					targetAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
				}
				List<tk2dSpriteAnimationFrame> list = new List<tk2dSpriteAnimationFrame>();
				for (int i = 0; i < spriteNameList.Count; i++)
				{
					tk2dSpriteDefinition spriteDefinition = collection.GetSpriteDefinition(spriteNameList[i]);
					if (1 == 1)//spriteDefinition != null )//&& spriteDefinition.Valid)
					{
						list.Add(new tk2dSpriteAnimationFrame
						{
							spriteCollection = collection,
							spriteId = collection.GetSpriteIdByName(spriteNameList[i]),
							invulnerableFrame = false,
							groundedFrame = true,
							requiresOffscreenUpdate = false,
							eventAudio = string.Empty,
							eventVfx = string.Empty,
							eventStopVfx = string.Empty,
							eventLerpEmissive = false,
							eventLerpEmissiveTime = 0.5f,
							eventLerpEmissivePower = 30f,
							forceMaterialUpdate = false,
							finishedSpawning = false,
							triggerEvent = false,
							eventInfo = string.Empty,
							eventInt = 0,
							eventFloat = 0f,
							eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified
						});
					}
				}
				if (list.Count <= 0)
				{
					ETGModConsole.Log("[BotsMod] AddAnimation: ERROR! Animation list is empty! No valid sprites found in specified list!", false);
					foreach (string spriteName in spriteNameList)
					{
						ETGModConsole.Log(spriteName, false);
					}
					return null;
				}
				tk2dSpriteAnimationClip tk2dSpriteAnimationClip = new tk2dSpriteAnimationClip
				{
					name = clipName,
					frames = list.ToArray(),
					fps = 15,
					//fps = (float)frameRate,
					wrapMode = wrapMode,
					loopStart = loopStart,
					minFidgetDuration = minFidgetDuration,
					maxFidgetDuration = maxFidgetDuration
				};
				Array.Resize<tk2dSpriteAnimationClip>(ref targetAnimator.Library.clips, targetAnimator.Library.clips.Length + 1);
				targetAnimator.Library.clips[targetAnimator.Library.clips.Length - 1] = tk2dSpriteAnimationClip;
				return tk2dSpriteAnimationClip;
			}
			catch (Exception e)
			{

				BotsModule.Log(string.Format(("stupid animations Broke heres why:  " + e)));

				return null;
			}

		}

		public static void DuplicateAIShooterAndAIBulletBank(GameObject targetObject, AIShooter sourceShooter, AIBulletBank sourceBulletBank, int startingGunOverrideID = 0, Transform gunAttachPointOverride = null, Transform bulletScriptAttachPointOverride = null, PlayerHandController overrideHandObject = null)
		{
			if (targetObject.GetComponent<AIShooter>() && targetObject.GetComponent<AIBulletBank>())
			{
				return;
			}
			if (!targetObject.GetComponent<AIBulletBank>())
			{
				AIBulletBank aibulletBank = targetObject.AddComponent<AIBulletBank>();
				aibulletBank.Bullets = new List<AIBulletBank.Entry>(0);
				if (sourceBulletBank.Bullets.Count > 0)
				{
					foreach (AIBulletBank.Entry entry in sourceBulletBank.Bullets)
					{
						aibulletBank.Bullets.Add(new AIBulletBank.Entry
						{
							Name = entry.Name,
							BulletObject = entry.BulletObject,
							OverrideProjectile = entry.OverrideProjectile,
							ProjectileData = new ProjectileData
							{
								damage = entry.ProjectileData.damage,
								speed = entry.ProjectileData.speed,
								range = entry.ProjectileData.range,
								force = entry.ProjectileData.force,
								damping = entry.ProjectileData.damping,
								UsesCustomAccelerationCurve = entry.ProjectileData.UsesCustomAccelerationCurve,
								AccelerationCurve = entry.ProjectileData.AccelerationCurve,
								CustomAccelerationCurveDuration = entry.ProjectileData.CustomAccelerationCurveDuration,
								onDestroyBulletScript = entry.ProjectileData.onDestroyBulletScript,
								IgnoreAccelCurveTime = entry.ProjectileData.IgnoreAccelCurveTime
							},
							PlayAudio = entry.PlayAudio,
							AudioSwitch = entry.AudioSwitch,
							AudioEvent = entry.AudioEvent,
							AudioLimitOncePerFrame = entry.AudioLimitOncePerFrame,
							AudioLimitOncePerAttack = entry.AudioLimitOncePerAttack,
							MuzzleFlashEffects = new VFXPool
							{
								effects = entry.MuzzleFlashEffects.effects,
								type = entry.MuzzleFlashEffects.type
							},
							MuzzleLimitOncePerFrame = entry.MuzzleLimitOncePerFrame,
							MuzzleInheritsTransformDirection = entry.MuzzleInheritsTransformDirection,
							ShellTransform = entry.ShellTransform,
							ShellPrefab = entry.ShellPrefab,
							ShellForce = entry.ShellForce,
							ShellForceVariance = entry.ShellForceVariance,
							DontRotateShell = entry.DontRotateShell,
							ShellGroundOffset = entry.ShellGroundOffset,
							ShellsLimitOncePerFrame = entry.ShellsLimitOncePerFrame,
							rampBullets = entry.rampBullets,
							conditionalMinDegFromNorth = entry.conditionalMinDegFromNorth,
							forceCanHitEnemies = entry.forceCanHitEnemies,
							suppressHitEffectsIfOffscreen = entry.suppressHitEffectsIfOffscreen,
							preloadCount = entry.preloadCount
						});
					}
				}
				aibulletBank.useDefaultBulletIfMissing = true;
				aibulletBank.transforms = new List<Transform>();
				if (sourceBulletBank.transforms != null && sourceBulletBank.transforms.Count > 0)
				{
					foreach (Transform item in sourceBulletBank.transforms)
					{
						aibulletBank.transforms.Add(item);
					}
				}
				aibulletBank.RegenerateCache();
			}
			if (!targetObject.GetComponent<AIShooter>())
			{
				AIShooter aishooter = targetObject.AddComponent<AIShooter>();
				aishooter.volley = sourceShooter.volley;
				if (startingGunOverrideID != 0)
				{
					aishooter.equippedGunId = startingGunOverrideID;
				}
				else
				{
					aishooter.equippedGunId = sourceShooter.equippedGunId;
				}
				aishooter.shouldUseGunReload = true;
				aishooter.volleyShootPosition = sourceShooter.volleyShootPosition;
				aishooter.volleyShellCasing = sourceShooter.volleyShellCasing;
				aishooter.volleyShellTransform = sourceShooter.volleyShellTransform;
				aishooter.volleyShootVfx = sourceShooter.volleyShootVfx;
				aishooter.usesOctantShootVFX = sourceShooter.usesOctantShootVFX;
				aishooter.bulletName = sourceShooter.bulletName;
				aishooter.customShootCooldownPeriod = sourceShooter.customShootCooldownPeriod;
				aishooter.doesScreenShake = sourceShooter.doesScreenShake;
				aishooter.rampBullets = sourceShooter.rampBullets;
				aishooter.rampStartHeight = sourceShooter.rampStartHeight;
				aishooter.rampTime = sourceShooter.rampTime;
				if (gunAttachPointOverride)
				{
					aishooter.gunAttachPoint = gunAttachPointOverride;
				}
				else
				{
					aishooter.gunAttachPoint = sourceShooter.gunAttachPoint;
				}
				if (bulletScriptAttachPointOverride)
				{
					aishooter.bulletScriptAttachPoint = bulletScriptAttachPointOverride;
				}
				else
				{
					aishooter.bulletScriptAttachPoint = sourceShooter.bulletScriptAttachPoint;
				}
				aishooter.overallGunAttachOffset = sourceShooter.overallGunAttachOffset;
				aishooter.flippedGunAttachOffset = sourceShooter.flippedGunAttachOffset;
				if (overrideHandObject)
				{
					aishooter.handObject = overrideHandObject;
				}
				else
				{
					aishooter.handObject = sourceShooter.handObject;
				}
				aishooter.AllowTwoHands = sourceShooter.AllowTwoHands;
				aishooter.ForceGunOnTop = sourceShooter.ForceGunOnTop;
				aishooter.IsReallyBigBoy = sourceShooter.IsReallyBigBoy;
				aishooter.BackupAimInMoveDirection = sourceShooter.BackupAimInMoveDirection;
				aishooter.RegenerateCache();
			}
		}

		public static AIAnimator GenerateBlankAIAnimator(GameObject targetObject)
		{
			AIAnimator aianimator = targetObject.AddComponent<AIAnimator>();
			aianimator.facingType = AIAnimator.FacingType.Default;
			aianimator.faceSouthWhenStopped = false;
			aianimator.faceTargetWhenStopped = false;
			aianimator.AnimatedFacingDirection = -90f;
			aianimator.directionalType = AIAnimator.DirectionalType.Sprite;
			aianimator.RotationQuantizeTo = 0f;
			aianimator.RotationOffset = 0f;
			aianimator.ForceKillVfxOnPreDeath = false;
			aianimator.SuppressAnimatorFallback = false;
			aianimator.IsBodySprite = true;
			aianimator.IdleAnimation = new DirectionalAnimation
			{
				Type = DirectionalAnimation.DirectionType.None,
				Prefix = string.Empty,
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0]
			};
			aianimator.MoveAnimation = new DirectionalAnimation
			{
				Type = DirectionalAnimation.DirectionType.None,
				Prefix = string.Empty,
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0]
			};
			aianimator.FlightAnimation = new DirectionalAnimation
			{
				Type = DirectionalAnimation.DirectionType.None,
				Prefix = string.Empty,
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0]
			};
			aianimator.HitAnimation = new DirectionalAnimation
			{
				Type = DirectionalAnimation.DirectionType.None,
				Prefix = string.Empty,
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0]
			};
			aianimator.TalkAnimation = new DirectionalAnimation
			{
				Type = DirectionalAnimation.DirectionType.None,
				Prefix = string.Empty,
				AnimNames = new string[0],
				Flipped = new DirectionalAnimation.FlipType[0]
			};
			aianimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>(0);
			aianimator.OtherVFX = new List<AIAnimator.NamedVFXPool>(0);
			aianimator.OtherScreenShake = new List<AIAnimator.NamedScreenShake>(0);
			aianimator.IdleFidgetAnimations = new List<DirectionalAnimation>(0);
			aianimator.HitReactChance = 1f;
			aianimator.HitType = AIAnimator.HitStateType.Basic;
			return aianimator;
		}

		public static void GenerateAIActorTemplate(GameObject targetObject, out GameObject corpseObject, string EnemyName, string EnemyGUID, tk2dSprite spriteSource = null, GameObject gunAttachObjectOverride = null, Vector3? GunAttachOffset = null, int StartingGunID = 38, List<PixelCollider> customColliders = null, bool RigidBodyCollidesWithTileMap = true, bool RigidBodyCollidesWithOthers = true, bool RigidBodyCanBeCarried = true, bool RigidBodyCanBePushed = false, bool isFakePrefab = false, bool instantiateCorpseObject = true, GameObject ExternalCorpseObject = null, bool EnemyHasNoShooter = false, bool EnemyHasNoCorpse = false)
		{
			if (!targetObject)
			{
				targetObject = new GameObject(EnemyName)
				{
					layer = 28
				};
			}
			GameObject gameObject = null;
			corpseObject = null;
			if (instantiateCorpseObject && !EnemyHasNoCorpse)
			{
				corpseObject = UnityEngine.Object.Instantiate<GameObject>(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject);
				corpseObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(corpseObject);
			}
			else if (ExternalCorpseObject && !EnemyHasNoCorpse)
			{
				corpseObject = ExternalCorpseObject;
			}
			if (!targetObject.GetComponent<AIActor>() && !gunAttachObjectOverride && !EnemyHasNoShooter)
			{
				gameObject = new GameObject("GunAttachPoint")
				{
					layer = 0
				};
				gameObject.transform.position = targetObject.transform.position;
				if (GunAttachOffset != null)
				{
					gameObject.transform.position = GunAttachOffset.Value;
				}
				else
				{
					gameObject.transform.position = new Vector3(0.3125f, 0.25f, 0f);
				}
				gameObject.transform.parent = targetObject.transform;
			}
			else if (!targetObject.GetComponent<AIActor>() && gunAttachObjectOverride && !EnemyHasNoShooter)
			{
				gameObject = new GameObject("GunAttachPoint")
				{
					layer = 0
				};
				if (GunAttachOffset != null)
				{
					gameObject.transform.position = GunAttachOffset.Value;
				}
				else
				{
					gameObject.transform.position = new Vector3(0.3125f, 0.25f, 0f);
				}
				gameObject.transform.parent = targetObject.transform;
			}
			if (!targetObject.GetComponent<tk2dSprite>() && spriteSource)
			{
				Tools.DuplicateSprite(targetObject.AddComponent<tk2dSprite>(), spriteSource);
			}
			tk2dSprite component = targetObject.GetComponent<tk2dSprite>();
			if (!component)
			{
				return;
			}
			if (!targetObject.GetComponent<SpeculativeRigidbody>())
			{
				if (customColliders != null)
				{
					using (List<PixelCollider>.Enumerator enumerator = customColliders.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PixelCollider pixelCollider = enumerator.Current;
							int manualWidth = pixelCollider.ManualWidth;
							int manualHeight = pixelCollider.ManualHeight;
							int manualOffsetX = pixelCollider.ManualOffsetX;
							int manualOffsetY = pixelCollider.ManualOffsetY;
							Tools.GenerateOrAddToRigidBody(targetObject, pixelCollider.CollisionLayer, pixelCollider.ColliderGenerationMode, RigidBodyCollidesWithTileMap, RigidBodyCollidesWithOthers, RigidBodyCanBeCarried, RigidBodyCanBePushed, false, pixelCollider.IsTrigger, false, true, new IntVector2?(new IntVector2(manualWidth, manualHeight)), new IntVector2?(new IntVector2(manualOffsetX, manualOffsetY)));
						}
						goto IL_29D;
					}
				}
				Tools.GenerateOrAddToRigidBody(targetObject, CollisionLayer.EnemyCollider, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2?(new IntVector2(12, 4)), new IntVector2?(new IntVector2(5, 0)));
				Tools.GenerateOrAddToRigidBody(targetObject, CollisionLayer.EnemyHitBox, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2?(new IntVector2(12, 23)), new IntVector2?(new IntVector2(5, 0)));
			IL_29D:
				SpeculativeRigidbody component2 = targetObject.GetComponent<SpeculativeRigidbody>();
				component2.Reinitialize();
				if (customColliders == null)
				{
					component2.PixelColliders[1].Sprite = component;
				}
			}
			if (!targetObject.GetComponent<tk2dSpriteAnimator>())
			{
				Tools.GenerateSpriteAnimator(targetObject, null, 0, 0f, false, false, false, false, true, false, 0f, 0f, false, false, false);
			}
			if (!targetObject.GetComponent<HealthHaver>())
			{
				Tools.GenerateHealthHaver(targetObject, 15f, false, false, OnDeathBehavior.DeathType.Death, true, false, false, false, true, true);
			}
			if (!targetObject.GetComponent<HitEffectHandler>())
			{
				targetObject.AddComponent<HitEffectHandler>();
			}
			HitEffectHandler component3 = targetObject.GetComponent<HitEffectHandler>();
			component3.overrideHitEffect = new VFXComplex
			{
				effects = new VFXObject[0]
			};
			component3.overrideHitEffectPool = new VFXPool
			{
				effects = new VFXComplex[0]
			};
			component3.additionalHitEffects = new HitEffectHandler.AdditionalHitEffect[0];
			component3.SuppressAllHitEffects = false;
			if (!targetObject.GetComponent<KnockbackDoer>())
			{
				targetObject.AddComponent<KnockbackDoer>();
			}
			KnockbackDoer component4 = targetObject.GetComponent<KnockbackDoer>();
			component4.weight = 35f;
			component4.deathMultiplier = 2.5f;
			component4.knockbackWhileReflecting = false;
			component4.shouldBounce = true;
			component4.collisionDecay = 0.5f;
			if (!targetObject.GetComponent<AIAnimator>())
			{
				Tools.GenerateBlankAIAnimator(targetObject);
			}
			if (!targetObject.GetComponent<ObjectVisibilityManager>())
			{
				targetObject.AddComponent<ObjectVisibilityManager>();
			}
			targetObject.GetComponent<ObjectVisibilityManager>().SuppressPlayerEnteredRoom = false;
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
			if ((!targetObject.GetComponent<AIShooter>() | !targetObject.GetComponent<AIBulletBank>()) && !EnemyHasNoShooter)
			{
				Tools.DuplicateAIShooterAndAIBulletBank(targetObject, orLoadByGuid.gameObject.GetComponent<AIShooter>(), orLoadByGuid.gameObject.GetComponent<AIBulletBank>(), StartingGunID, gameObject.transform, null, null);
			}
			if (!targetObject.GetComponent<AIActor>())
			{
				targetObject.AddComponent<AIActor>();
			}
			AIActor component5 = targetObject.GetComponent<AIActor>();
			component5.placeableWidth = 1;
			component5.placeableHeight = 1;
			component5.difficulty = DungeonPlaceableBehaviour.PlaceableDifficulty.BASE;
			component5.isPassable = true;
			component5.ActorName = EnemyName;
			component5.OverrideDisplayName = EnemyName;
			component5.actorTypes = (CoreActorTypes)0;
			component5.HasShadow = true;
			component5.ShadowHeightOffGround = 0f;
			component5.ActorShadowOffset = Vector3.zero;
			component5.DoDustUps = false;
			component5.DustUpInterval = 0f;
			component5.FreezeDispelFactor = 20f;
			component5.ImmuneToAllEffects = false;
			component5.EffectResistances = new ActorEffectResistance[0];
			component5.OverrideColorOverridden = false;
			component5.EnemyId = UnityEngine.Random.Range(10000, 99999);
			component5.EnemyGuid = EnemyGUID;
			component5.ForcedPositionInAmmonomicon = -1;
			component5.SetsFlagOnDeath = false;
			component5.FlagToSetOnDeath = GungeonFlags.NONE;
			component5.SetsFlagOnActivation = false;
			component5.FlagToSetOnActivation = GungeonFlags.NONE;
			component5.SetsCharacterSpecificFlagOnDeath = false;
			component5.CharacterSpecificFlagToSetOnDeath = CharacterSpecificGungeonFlags.NONE;
			component5.IsNormalEnemy = true;
			component5.IsSignatureEnemy = false;
			component5.IsHarmlessEnemy = false;
			component5.CompanionSettings = new ActorCompanionSettings
			{
				WarpsToRandomPoint = false
			};
			component5.MovementSpeed = 8f;
			component5.PathableTiles = CellTypes.FLOOR;
			component5.DiesOnCollison = false;
			component5.CollisionDamage = 0.5f;
			component5.CollisionKnockbackStrength = 5f;
			component5.CollisionDamageTypes = CoreDamageTypes.None;
			component5.EnemyCollisionKnockbackStrengthOverride = -1f;
			component5.CollisionVFX = new VFXPool
			{
				effects = new VFXComplex[0]
			};
			component5.NonActorCollisionVFX = new VFXPool
			{
				effects = new VFXComplex[0]
			};
			component5.CollisionSetsPlayerOnFire = false;
			component5.TryDodgeBullets = false;
			component5.AvoidRadius = 4f;
			component5.ReflectsProjectilesWhileInvulnerable = false;
			component5.HitByEnemyBullets = false;
			component5.HasOverrideDodgeRollDeath = false;
			component5.OverrideDodgeRollDeath = string.Empty;
			component5.CanDropCurrency = true;
			component5.AdditionalSingleCoinDropChance = 0f;
			component5.CanDropItems = true;
			component5.CanDropDuplicateItems = false;
			component5.CustomLootTableMinDrops = 1;
			component5.CustomLootTableMaxDrops = 1;
			component5.ChanceToDropCustomChest = 0f;
			component5.IgnoreForRoomClear = false;
			component5.SpawnLootAtRewardChestPos = false;
			if (!EnemyHasNoCorpse && corpseObject && !ExternalCorpseObject)
			{
				component5.CorpseObject = corpseObject;
			}
			else if (!EnemyHasNoCorpse && ExternalCorpseObject)
			{
				component5.CorpseObject = ExternalCorpseObject;
			}
			else
			{
				component5.CorpseObject = null;
			}
			component5.CorpseShadow = true;
			component5.TransferShadowToCorpse = false;
			component5.shadowDeathType = AIActor.ShadowDeathType.Fade;
			component5.PreventDeathKnockback = false;
			component5.OnCorpseVFX = new VFXPool
			{
				effects = new VFXComplex[0]
			};
			component5.OnEngagedVFXAnchor = tk2dBaseSprite.Anchor.LowerLeft;
			component5.shadowHeightOffset = 0f;
			component5.invisibleUntilAwaken = false;
			component5.procedurallyOutlined = true;
			component5.forceUsesTrimmedBounds = true;
			component5.reinforceType = AIActor.ReinforceType.FullVfx;
			component5.UsesVaryingEmissiveShaderPropertyBlock = false;
			component5.EnemySwitchState = "Metal_Bullet_Man";
			component5.OverrideSpawnReticleAudio = string.Empty;
			component5.OverrideSpawnAppearAudio = string.Empty;
			component5.UseMovementAudio = false;
			component5.StartMovingEvent = string.Empty;
			component5.StopMovingEvent = string.Empty;
			component5.animationAudioEvents = new List<ActorAudioEvent>
			{
				new ActorAudioEvent
				{
					eventTag = "footstep",
					eventName = "Play_CHR_metalBullet_step_01"
				}
			};
			component5.HealthOverrides = new List<AIActor.HealthOverride>(0);
			component5.IdentifierForEffects = AIActor.EnemyTypeIdentifier.UNIDENTIFIED;
			component5.BehaviorOverridesVelocity = false;
			component5.BehaviorVelocity = Vector2.zero;
			component5.AlwaysShowOffscreenArrow = false;
			component5.BlackPhantomProperties = new BlackPhantomProperties
			{
				BonusHealthPercentIncrease = 2.2f,
				BonusHealthFlatIncrease = 0f,
				MaxTotalHealth = 175f,
				CooldownMultiplier = 0.66f,
				MovementSpeedMultiplier = 1.5f,
				LocalTimeScaleMultiplier = 1f,
				BulletSpeedMultiplier = 1f,
				GradientScale = 0.75f,
				ContrastPower = 1.3f
			};
			component5.ForceBlackPhantomParticles = false;
			component5.OverrideBlackPhantomParticlesCollider = false;
			component5.BlackPhantomParticlesCollider = 0;
			component5.PreventFallingInPitsEver = false;
			component5.RegenerateCache();
		}

		public static SpeculativeRigidbody GenerateOrAddToRigidBody(GameObject targetObject, CollisionLayer collisionLayer, PixelCollider.PixelColliderGeneration colliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon, bool collideWithTileMap = false, bool CollideWithOthers = true, bool CanBeCarried = true, bool CanBePushed = false, bool RecheckTriggers = false, bool IsTrigger = false, bool replaceExistingColliders = false, bool UsesPixelsAsUnitSize = false, IntVector2? dimensions = null, IntVector2? offset = null)
		{
			SpeculativeRigidbody orAddComponent = targetObject.GetOrAddComponent<SpeculativeRigidbody>();
			orAddComponent.CollideWithOthers = CollideWithOthers;
			orAddComponent.CollideWithTileMap = collideWithTileMap;
			orAddComponent.Velocity = Vector2.zero;
			orAddComponent.MaxVelocity = Vector2.zero;
			orAddComponent.ForceAlwaysUpdate = false;
			orAddComponent.CanPush = false;
			orAddComponent.CanBePushed = CanBePushed;
			orAddComponent.PushSpeedModifier = 1f;
			orAddComponent.CanCarry = false;
			orAddComponent.CanBeCarried = CanBeCarried;
			orAddComponent.PreventPiercing = false;
			orAddComponent.SkipEmptyColliders = false;
			orAddComponent.RecheckTriggers = RecheckTriggers;
			orAddComponent.UpdateCollidersOnRotation = false;
			orAddComponent.UpdateCollidersOnScale = false;
			IntVector2 intVector = IntVector2.Zero;
			IntVector2 intVector2 = IntVector2.Zero;
			if (colliderGenerationMode != PixelCollider.PixelColliderGeneration.Tk2dPolygon)
			{
				if (dimensions != null)
				{
					intVector2 = dimensions.Value;
					if (!UsesPixelsAsUnitSize)
					{
						intVector2 = new IntVector2(intVector2.x * 16, intVector2.y * 16);
					}
				}
				if (offset != null)
				{
					intVector = offset.Value;
					if (!UsesPixelsAsUnitSize)
					{
						intVector = new IntVector2(intVector.x * 16, intVector.y * 16);
					}
				}
			}
			PixelCollider item = new PixelCollider
			{
				ColliderGenerationMode = colliderGenerationMode,
				CollisionLayer = collisionLayer,
				IsTrigger = IsTrigger,
				BagleUseFirstFrameOnly = (colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon),
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = intVector.x,
				ManualOffsetY = intVector.y,
				ManualWidth = intVector2.x,
				ManualHeight = intVector2.y,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			};
			if (replaceExistingColliders | orAddComponent.PixelColliders == null)
			{
				orAddComponent.PixelColliders = new List<PixelCollider>
				{
					item
				};
			}
			else
			{
				orAddComponent.PixelColliders.Add(item);
			}
			if (orAddComponent.sprite && colliderGenerationMode == PixelCollider.PixelColliderGeneration.Tk2dPolygon)
			{
				Bounds bounds = orAddComponent.sprite.GetBounds();
				orAddComponent.sprite.GetTrueCurrentSpriteDef().colliderVertices = new Vector3[]
				{
					bounds.center - bounds.extents,
					bounds.center + bounds.extents
				};
			}
			return orAddComponent;
		}

		public static void GenerateHealthHaver(GameObject target, float maxHealth = 25f, bool disableAnimator = true, bool explodesOnDeath = true, OnDeathBehavior.DeathType explosionDeathType = OnDeathBehavior.DeathType.Death, bool flashesOnDamage = true, bool exploderSpawnsItem = false, bool isCorruptedObject = true, bool isRatNPC = false, bool skipAnimatorCheck = false, bool buildLists = false)
		{
			if (target.GetComponent<HealthHaver>() != null | target.GetComponentInChildren<HealthHaver>(true) != null | target.GetComponent<tk2dBaseSprite>() == null | target.GetComponentInChildren<SpeculativeRigidbody>() == null)
			{
				return;
			}
			if (!isRatNPC && !skipAnimatorCheck)
			{
				tk2dBaseSprite component = target.GetComponent<tk2dBaseSprite>();
				if (disableAnimator)
				{
					if (target.GetComponent<AIAnimator>())
					{
						UnityEngine.Object.Destroy(target.GetComponent<AIAnimator>());
					}
					if (target.GetComponent<tk2dSpriteAnimator>())
					{
						UnityEngine.Object.Destroy(target.GetComponent<tk2dSpriteAnimator>());
					}
					target.AddComponent<tk2dSpriteAnimator>();
					tk2dSpriteAnimator component2 = target.GetComponent<tk2dSpriteAnimator>();
					component2.Library = null;
					component2.DefaultClipId = 0;
					component2.AdditionalCameraVisibilityRadius = 0f;
					component2.AnimateDuringBossIntros = false;
					component2.AlwaysIgnoreTimeScale = true;
					component2.ForceSetEveryFrame = false;
					component2.playAutomatically = false;
					component2.IsFrameBlendedAnimation = false;
					component2.clipTime = 0f;
					component2.deferNextStartClip = false;
					AddAnimationOld (component2, component.Collection, new List<int>
					{
						component.spriteId
					}, "DummyFrame", tk2dSpriteAnimationClip.WrapMode.Once, 15, 0, 0.5f, 1f);
				}
				else if (!target.GetComponent<tk2dSpriteAnimator>() && component.Collection != null)
				{
					target.AddComponent<tk2dSpriteAnimator>();
					tk2dSpriteAnimator component3 = target.GetComponent<tk2dSpriteAnimator>();
					component3.Library = null;
					component3.DefaultClipId = 0;
					component3.AdditionalCameraVisibilityRadius = 0f;
					component3.AnimateDuringBossIntros = false;
					component3.AlwaysIgnoreTimeScale = true;
					component3.ForceSetEveryFrame = false;
					component3.playAutomatically = false;
					component3.IsFrameBlendedAnimation = false;
					component3.clipTime = 0f;
					component3.deferNextStartClip = false;
					AddAnimationOld(component3, component.Collection, new List<int>
					{
						component.spriteId
					}, "DummyFrame", tk2dSpriteAnimationClip.WrapMode.Once, 15, 0, 0.5f, 1f);
				}
				else if (target.GetComponent<tk2dSpriteAnimator>() && !disableAnimator)
				{
					tk2dSpriteAnimator component4 = target.GetComponent<tk2dSpriteAnimator>();
					if (!(component4.Library != null) || component4.Library.clips == null || component4.Library.clips.Length == 0)
					{
						return;
					}
					foreach (tk2dSpriteAnimationClip tk2dSpriteAnimationClip in component4.Library.clips)
					{
						if (tk2dSpriteAnimationClip.frames != null && tk2dSpriteAnimationClip.frames.Length != 0)
						{
							tk2dSpriteAnimationFrame[] frames = tk2dSpriteAnimationClip.frames;
							for (int j = 0; j < frames.Length; j++)
							{
								frames[j].invulnerableFrame = false;
							}
						}
					}
				}
				else if (!target.GetComponent<tk2dSpriteAnimator>() && !disableAnimator)
				{
					return;
				}
			}
			target.AddComponent<HealthHaver>();
			HealthHaver component5 = target.GetComponent<HealthHaver>();
			typeof(HealthHaver).GetField("isPlayerCharacter", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(component5, false);
			component5.IsVulnerable = true;
			component5.quantizeHealth = false;
			component5.quantizedIncrement = 0.5f;
			component5.flashesOnDamage = flashesOnDamage;
			component5.incorporealityOnDamage = false;
			component5.incorporealityTime = 0f;
			component5.PreventAllDamage = false;
			component5.persistsOnDeath = false;
			component5.SetHealthMaximum(maxHealth, null, false);
			component5.Armor = 0f;
			component5.CursedMaximum = maxHealth * 3f;
			component5.useFortunesFavorInvulnerability = false;
			component5.damagedAudioEvent = string.Empty;
			component5.overrideDeathAudioEvent = string.Empty;
			component5.overrideDeathAnimation = string.Empty;
			component5.shakesCameraOnDamage = false;
			component5.cameraShakeOnDamage = new ScreenShakeSettings
			{
				magnitude = 0.35f,
				speed = 6f,
				time = 0.06f,
				falloff = 0f,
				direction = Vector2.zero,
				vibrationType = ScreenShakeSettings.VibrationType.Auto,
				simpleVibrationTime = Vibration.Time.Normal,
				simpleVibrationStrength = Vibration.Strength.Medium
			};
			component5.damageTypeModifiers = new List<DamageTypeModifier>(0);
			component5.healthIsNumberOfHits = false;
			component5.OnlyAllowSpecialBossDamage = false;
			component5.overrideDeathAnimBulletScript = string.Empty;
			component5.noCorpseWhenBulletScriptDeath = false;
			component5.spawnBulletScript = false;
			component5.chanceToSpawnBulletScript = 0f;
			component5.bulletScriptType = HealthHaver.BulletScriptType.OnPreDeath;
			component5.bulletScript = new BulletScriptSelector
			{
				scriptTypeName = string.Empty
			};
			component5.bossHealthBar = HealthHaver.BossBarType.None;
			component5.overrideBossName = string.Empty;
			component5.forcePreventVictoryMusic = false;
			component5.GlobalPixelColliderDamageMultiplier = 1f;
			if (explodesOnDeath)
			{
				component5.gameObject.AddComponent<ExplodeOnDeath>();
				ExplodeOnDeath component6 = component5.gameObject.GetComponent<ExplodeOnDeath>();
				component6.deathType = explosionDeathType;
				//component6.spawnItemsOnExplosion = exploderSpawnsItem;
				//component6.isCorruptedObject = isCorruptedObject;
			}
			if (buildLists)
			{
				component5.DamageableColliders = new List<PixelCollider>();
				if (target.GetComponentsInChildren<SpeculativeRigidbody>(true) != null && target.GetComponentsInChildren<SpeculativeRigidbody>(true).Length != 0)
				{
					component5.bodyRigidbodies = new List<SpeculativeRigidbody>();
					foreach (SpeculativeRigidbody speculativeRigidbody in target.GetComponentsInChildren<SpeculativeRigidbody>(true))
					{
						component5.bodyRigidbodies.Add(speculativeRigidbody);
						if (speculativeRigidbody.PixelColliders != null && speculativeRigidbody.PixelColliders.Count > 0)
						{
							foreach (PixelCollider item in speculativeRigidbody.PixelColliders)
							{
								component5.DamageableColliders.Add(item);
							}
						}
					}
				}
				else
				{
					component5.bodyRigidbodies = new List<SpeculativeRigidbody>
					{
						target.GetComponent<SpeculativeRigidbody>()
					};
					if (target.GetComponent<SpeculativeRigidbody>().PixelColliders != null && target.GetComponent<SpeculativeRigidbody>().PixelColliders.Count > 0)
					{
						foreach (PixelCollider item2 in target.GetComponent<SpeculativeRigidbody>().PixelColliders)
						{
							component5.DamageableColliders.Add(item2);
						}
					}
				}
				if (target.GetComponentsInChildren<tk2dBaseSprite>(true) != null)
				{
					component5.bodySprites = new List<tk2dBaseSprite>();
					foreach (tk2dBaseSprite item3 in target.GetComponentsInChildren<tk2dBaseSprite>(true))
					{
						component5.bodySprites.Add(item3);
					}
				}
			}
			if (isCorruptedObject)
			{
				if (string.IsNullOrEmpty(target.name))
				{
					target.name = "Corrupted Object";
				}
				else if (!target.name.StartsWith("Corrupted"))
				{
					target.name = "Corrupted " + target.name;
				}
			}
			try
			{
				component5.RegenerateCache();
			}
			catch (Exception)
			{
			}
		}


		public static Material Copy(this Material orig, Texture2D textureOverride = null, Shader shaderOverride = null)
		{
			Material material = new Material(orig.shader)
			{
				name = orig.name,
				shaderKeywords = orig.shaderKeywords,
				globalIlluminationFlags = orig.globalIlluminationFlags,
				enableInstancing = orig.enableInstancing,
				doubleSidedGI = orig.doubleSidedGI,
				mainTextureOffset = orig.mainTextureOffset,
				mainTextureScale = orig.mainTextureScale,
				renderQueue = orig.renderQueue,
				color = orig.color,
				hideFlags = orig.hideFlags
			};
			if (textureOverride != null)
			{
				material.mainTexture = textureOverride;
			}
			else
			{
				material.mainTexture = orig.mainTexture;
			}
			if (shaderOverride != null)
			{
				material.shader = shaderOverride;
			}
			else
			{
				material.shader = orig.shader;
			}
			return material;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		public static tk2dSpriteDefinition Copy(this tk2dSpriteDefinition orig)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition = new tk2dSpriteDefinition();
			tk2dSpriteDefinition.boundsDataCenter = orig.boundsDataCenter;
			tk2dSpriteDefinition.boundsDataExtents = orig.boundsDataExtents;
			tk2dSpriteDefinition.colliderConvex = orig.colliderConvex;
			tk2dSpriteDefinition.colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions;
			tk2dSpriteDefinition.colliderType = orig.colliderType;
			tk2dSpriteDefinition.colliderVertices = orig.colliderVertices;
			tk2dSpriteDefinition.collisionLayer = orig.collisionLayer;
			tk2dSpriteDefinition.complexGeometry = orig.complexGeometry;
			tk2dSpriteDefinition.extractRegion = orig.extractRegion;
			tk2dSpriteDefinition.flipped = orig.flipped;
			tk2dSpriteDefinition.indices = orig.indices;
			if (orig.material != null)
			{
				tk2dSpriteDefinition.material = new Material(orig.material);
			}
			tk2dSpriteDefinition.materialId = orig.materialId;
			if (orig.materialInst != null)
			{
				tk2dSpriteDefinition.materialInst = new Material(orig.materialInst);
			}
			tk2dSpriteDefinition.metadata = orig.metadata;
			tk2dSpriteDefinition.name = orig.name;
			tk2dSpriteDefinition.normals = orig.normals;
			tk2dSpriteDefinition.physicsEngine = orig.physicsEngine;
			tk2dSpriteDefinition.position0 = orig.position0;
			tk2dSpriteDefinition.position1 = orig.position1;
			tk2dSpriteDefinition.position2 = orig.position2;
			tk2dSpriteDefinition.position3 = orig.position3;
			tk2dSpriteDefinition.regionH = orig.regionH;
			tk2dSpriteDefinition.regionW = orig.regionW;
			tk2dSpriteDefinition.regionX = orig.regionX;
			tk2dSpriteDefinition.regionY = orig.regionY;
			tk2dSpriteDefinition.tangents = orig.tangents;
			tk2dSpriteDefinition.texelSize = orig.texelSize;
			tk2dSpriteDefinition.untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter;
			tk2dSpriteDefinition.untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents;
			tk2dSpriteDefinition.uvs = orig.uvs;
			return tk2dSpriteDefinition;
		}
	

	public static tk2dSpriteCollectionData ReplaceDungeonCollection(tk2dSpriteCollectionData sourceCollection, Texture2D spriteSheet = null, List<string> spriteList = null)
		{
			if (sourceCollection == null)
			{
				return null;
			}
			tk2dSpriteCollectionData tk2dSpriteCollectionData = UnityEngine.Object.Instantiate<tk2dSpriteCollectionData>(sourceCollection);
			tk2dSpriteDefinition[] array = new tk2dSpriteDefinition[tk2dSpriteCollectionData.spriteDefinitions.Length];
			for (int i = 0; i < tk2dSpriteCollectionData.spriteDefinitions.Length; i++)
			{
				array[i] = tk2dSpriteCollectionData.spriteDefinitions[i].Copy();
			}
			tk2dSpriteCollectionData.spriteDefinitions = array;
			if (spriteSheet != null)
			{
				Material[] materials = sourceCollection.materials;
				Material[] array2 = new Material[materials.Length];
				if (materials != null)
				{
					for (int j = 0; j < materials.Length; j++)
					{
						array2[j] = materials[j].Copy(spriteSheet, null);
					}
					tk2dSpriteCollectionData.materials = array2;
					foreach (Material material in tk2dSpriteCollectionData.materials)
					{
						foreach (tk2dSpriteDefinition tk2dSpriteDefinition in tk2dSpriteCollectionData.spriteDefinitions)
						{
							if (material != null && tk2dSpriteDefinition.material.name.Equals(material.name))
							{
								tk2dSpriteDefinition.material = material;
								tk2dSpriteDefinition.materialInst = new Material(material);
							}
						}
					}
				}
			}
			else if (spriteList != null)
			{
				RuntimeAtlasPage runtimeAtlasPage = new RuntimeAtlasPage(0, 0, TextureFormat.RGBA32, 2);
				for (int m = 0; m < spriteList.Count; m++)
				{
					Texture2D textureFromResource = ResourceExtractor.GetTextureFromResource(spriteList[m]);
					if (!textureFromResource)
					{
						Debug.Log("[BuildDungeonCollection] Null Texture found at index: " + m);
					}
					else
					{
						float num = (float)textureFromResource.width / 16f;
						float num2 = (float)textureFromResource.height / 16f;
						tk2dSpriteDefinition tk2dSpriteDefinition2 = tk2dSpriteCollectionData.spriteDefinitions[m];
						if (tk2dSpriteDefinition2 != null)
						{
							if (tk2dSpriteDefinition2.boundsDataCenter != Vector3.zero)
							{
								try
								{
									RuntimeAtlasSegment runtimeAtlasSegment = runtimeAtlasPage.Pack(textureFromResource, false);
									tk2dSpriteDefinition2.materialInst.mainTexture = runtimeAtlasSegment.texture;
									tk2dSpriteDefinition2.uvs = runtimeAtlasSegment.uvs;
									tk2dSpriteDefinition2.extractRegion = true;
									tk2dSpriteDefinition2.position0 = Vector3.zero;
									tk2dSpriteDefinition2.position1 = new Vector3(num, 0f, 0f);
									tk2dSpriteDefinition2.position2 = new Vector3(0f, num2, 0f);
									tk2dSpriteDefinition2.position3 = new Vector3(num, num2, 0f);
									tk2dSpriteDefinition2.boundsDataCenter = new Vector2(num / 2f, num2 / 2f);
									tk2dSpriteDefinition2.untrimmedBoundsDataCenter = tk2dSpriteDefinition2.boundsDataCenter;
									tk2dSpriteDefinition2.boundsDataExtents = new Vector2(num, num2);
									tk2dSpriteDefinition2.untrimmedBoundsDataExtents = tk2dSpriteDefinition2.boundsDataExtents;
									goto IL_2E3;
								}
								catch (Exception)
								{
									Debug.Log("[BuildDungeonCollection] Exception caught at index: " + m);
									goto IL_2E3;
								}
							}
							try
							{
								tk2dSpriteDefinition2.ReplaceTexture(textureFromResource, true);
								goto IL_2E3;
							}
							catch (Exception)
							{
								Debug.Log("[BuildDungeonCollection] Exception caught at index: " + m);
								goto IL_2E3;
							}
						}
						Debug.Log("[BuildDungeonCollection] SpriteData is null at index: " + m);
					}
				IL_2E3:;
				}
				runtimeAtlasPage.Apply();
			}
			else
			{
				Debug.Log("[BuildDungeonCollection] SpriteList is null!");
			}
			return tk2dSpriteCollectionData;
		}


		public static tk2dSpriteAnimationClip AddAnimationOld(tk2dSpriteAnimator animator, tk2dSpriteCollectionData collection, List<int> spriteIDs, string clipName, tk2dSpriteAnimationClip.WrapMode wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop, int frameRate = 15, int loopStart = 0, float minFidgetDuration = 0.5f, float maxFidgetDuration = 1f)
		{
			if (animator.Library == null)
			{
				animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
				animator.Library.clips = new tk2dSpriteAnimationClip[0];
			}
			List<tk2dSpriteAnimationFrame> list = new List<tk2dSpriteAnimationFrame>();
			for (int i = 0; i < spriteIDs.Count; i++)
			{
				if (collection.spriteDefinitions[spriteIDs[i]].Valid)
				{
					list.Add(new tk2dSpriteAnimationFrame
					{
						spriteCollection = collection,
						spriteId = spriteIDs[i],
						invulnerableFrame = false
					});
				}
			}
			tk2dSpriteAnimationClip tk2dSpriteAnimationClip = new tk2dSpriteAnimationClip
			{
				name = clipName,
				frames = list.ToArray(),
				fps = (float)frameRate,
				wrapMode = wrapMode,
				loopStart = loopStart,
				minFidgetDuration = minFidgetDuration,
				maxFidgetDuration = maxFidgetDuration
			};
			Array.Resize<tk2dSpriteAnimationClip>(ref animator.Library.clips, animator.Library.clips.Length + 1);
			animator.Library.clips[animator.Library.clips.Length - 1] = tk2dSpriteAnimationClip;
			return tk2dSpriteAnimationClip;
		}

		public static void DuplicateSprite(tk2dSprite targetSprite, tk2dSprite sourceSprite)
		{
			targetSprite.automaticallyManagesDepth = sourceSprite.automaticallyManagesDepth;
			targetSprite.ignoresTiltworldDepth = sourceSprite.ignoresTiltworldDepth;
			targetSprite.depthUsesTrimmedBounds = sourceSprite.depthUsesTrimmedBounds;
			targetSprite.allowDefaultLayer = sourceSprite.allowDefaultLayer;
			targetSprite.attachParent = sourceSprite.attachParent;
			targetSprite.OverrideMaterialMode = sourceSprite.OverrideMaterialMode;
			targetSprite.independentOrientation = sourceSprite.independentOrientation;
			targetSprite.autodetectFootprint = sourceSprite.autodetectFootprint;
			targetSprite.customFootprintOrigin = sourceSprite.customFootprintOrigin;
			targetSprite.customFootprint = sourceSprite.customFootprint;
			targetSprite.hasOffScreenCachedUpdate = sourceSprite.hasOffScreenCachedUpdate;
			targetSprite.offScreenCachedCollection = sourceSprite.offScreenCachedCollection;
			targetSprite.offScreenCachedID = sourceSprite.offScreenCachedID;
			targetSprite.Collection = sourceSprite.Collection;
			targetSprite.color = sourceSprite.color;
			targetSprite.scale = sourceSprite.scale;
			targetSprite.spriteId = sourceSprite.spriteId;
			targetSprite.boxCollider2D = sourceSprite.boxCollider2D;
			targetSprite.boxCollider = sourceSprite.boxCollider;
			targetSprite.meshCollider = sourceSprite.meshCollider;
			targetSprite.meshColliderPositions = sourceSprite.meshColliderPositions;
			targetSprite.meshColliderMesh = sourceSprite.meshColliderMesh;
			targetSprite.CachedPerpState = sourceSprite.CachedPerpState;
			targetSprite.HeightOffGround = sourceSprite.HeightOffGround;
			targetSprite.SortingOrder = sourceSprite.SortingOrder;
			targetSprite.IsBraveOutlineSprite = sourceSprite.IsBraveOutlineSprite;
			targetSprite.IsZDepthDirty = sourceSprite.IsZDepthDirty;
			targetSprite.ApplyEmissivePropertyBlock = sourceSprite.ApplyEmissivePropertyBlock;
			targetSprite.GenerateUV2 = sourceSprite.GenerateUV2;
			targetSprite.LockUV2OnFrameOne = sourceSprite.LockUV2OnFrameOne;
			targetSprite.StaticPositions = sourceSprite.StaticPositions;
		}
	}
}
