using CustomCharacters;
using Dungeonator;
using FerryMansOar;
using Gungeon;
using ItemAPI;
using Pathfinding;
//using Pathfinding;
using System;
using System.Collections;
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
		public static GameObject Foyer_ElevatorChamber;
		public static List<int> BeyondItems = new List<int>();
		public static List<int> Spells = new List<int>();




		public static void Init()
		{


			AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("shared_auto_001");
			AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");

			ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<Texture2D>("nebula_reducednoise");

			shared_auto_001 = assetBundle3;
			shared_auto_002 = assetBundle2;
			brave = ResourceManager.LoadAssetBundle("brave_resources_001");

			Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");
			Foyer_ElevatorChamber = assetBundle2.LoadAsset<GameObject>("Foyer_ElevatorChamber");

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

		public static void AddAudioEventByFrame(this tk2dSpriteAnimationClip clip, int frame, string audio)
		{
			clip.frames[frame].eventAudio = audio;
			clip.frames[frame].triggerEvent = true;
		}

		public static AIBeamShooter2 AddAIBeamShooter(AIActor enemy, Transform transform, string name, Projectile beamProjectile, ProjectileModule beamModule = null, float angle = 0f)
		{
			AIBeamShooter2 aibeamShooter = enemy.gameObject.AddComponent<AIBeamShooter2>();
			aibeamShooter.beamTransform = transform;
			aibeamShooter.beamModule = beamModule;
			aibeamShooter.beamProjectile = beamProjectile.projectile;
			aibeamShooter.firingEllipseCenter = transform.position;
			aibeamShooter.name = name;
			aibeamShooter.northAngleTolerance = angle;
			return aibeamShooter;
		}

		public static void SetupTileMetaData(this TilesetIndexMetadata metadata, TilesetIndexMetadata.TilesetFlagType type, float weight = 1, int dungeonRoomSubType = 0, int dungeonRoomSubType2 = -1, int dungeonRoomSubType3 = -1, bool animated = false, bool preventStamps = true)
		{
			metadata.type = type;
			metadata.weight = weight;
			metadata.dungeonRoomSubType = dungeonRoomSubType;
			metadata.secondRoomSubType = dungeonRoomSubType2;
			metadata.thirdRoomSubType = dungeonRoomSubType3;
			metadata.usesAnimSequence = animated;
			metadata.usesNeighborDependencies = false;
			metadata.preventWallStamping = preventStamps;

			metadata.usesPerTileVFX = false;
			metadata.tileVFXPlaystyle = TilesetIndexMetadata.VFXPlaystyle.CONTINUOUS;
			metadata.tileVFXChance = 0;
			metadata.tileVFXPrefab = null;
			metadata.tileVFXOffset = Vector2.zero;
			metadata.tileVFXDelayTime = 1;
			metadata.tileVFXDelayVariance = 0;
			metadata.tileVFXAnimFrame = 0;
		}

		public static Texture2D AddBorderToAtlasSprites(tk2dSpriteCollectionData collection, TileIndexList indexList, string subtypeName, Color color)
		{

			tk2dSpriteDefinition def;
			string defName;
			Material material;
			Texture2D texture, output;
			int width, height, minX, minY, maxX, maxY, w, h;
			Vector2[] uvs;

			def = collection.spriteDefinitions[0];
			material = def.material == null ? def.materialInst : def.material;

			texture = (Texture2D)material.mainTexture.GetReadable();
			width = texture.width;
			height = texture.height;
			output = new Texture2D(width, height);
			

			foreach (var id in indexList.indices)
			{
				//ETGModConsole.Log($"{id}/{collection.spriteDefinitions.Length - 1}");
				if (id < 0) continue;
				def = collection.spriteDefinitions[id];
				if (def == null) continue;


				defName = def.name;

				uvs = def.uvs;
				if (def.uvs == null || def.uvs.Length < 4)
				{
					ToolsCharApi.PrintError($"Failed to dump {defName} in {subtypeName}: Invalid UV's");
					continue;
				}

				minX = Mathf.RoundToInt(uvs[0].x * width);
				minY = Mathf.RoundToInt(uvs[0].y * height);
				maxX = Mathf.RoundToInt(uvs[3].x * width);
				maxY = Mathf.RoundToInt(uvs[3].y * height);

				w = maxX - minX;
				h = maxY - minY;



				for (int y = minY; y < maxY; y++)
				{
					if (y == minY || y == maxY - 1)
					{
						for (int x = minX; x < maxX; x++)
						{
							output.SetPixel(x, y, color);
						}
					}
					else
					{
						output.SetPixel(minX, y, color);
						output.SetPixel(maxX, y, color);
					}

				}
				output.Apply();

			}

			output.name = subtypeName + UnityEngine.Random.Range(0, 100000);


			return output;
			
			
		}

		public static void SetMaterial(this tk2dSpriteCollectionData collection, int spriteId, int matNum)
        {
			collection.spriteDefinitions[spriteId].material = collection.materials[matNum];
			collection.spriteDefinitions[spriteId].materialId = matNum;

		}

		public static void SetupTilesetSpriteDef(this tk2dSpriteDefinition def, bool wall = false, bool lower = false)
        {
			def.boundsDataCenter = new Vector3(0.5f, 0.5f, 0);
			def.boundsDataExtents = new Vector3(1, 1f, 0);
			def.untrimmedBoundsDataCenter = new Vector3(0.5f, 0.5f, 0);
			def.untrimmedBoundsDataExtents = new Vector3(1, 1, 0);
			def.texelSize = new Vector2(0.625f, 0.625f);
			//def.colliderType = tk2dSpriteDefinition.ColliderType.None'
			def.position0 = new Vector3(0, 0f, 0);
			def.position1 = new Vector3(1, 0f, 0);
			def.position2 = new Vector3(0, 1, 0);
			def.position3 = new Vector3(1, 1, 0);
			def.regionH = 16;
			def.regionW = 16;
			if (wall)
            {
				def.colliderType = tk2dSpriteDefinition.ColliderType.Box;
				def.collisionLayer = lower ? CollisionLayer.LowObstacle : CollisionLayer.HighObstacle;
				def.colliderVertices = new Vector3[]
				{
					new Vector3(0, 1, -1),
					new Vector3(0, 1, 1),
					new Vector3(0, 0, -1),
					new Vector3(0, 0, 1),
					new Vector3(1, 0, -1),
					new Vector3(1, 0, 1),
					new Vector3(1, 1, -1),
					new Vector3(1, 1, 1),
				};
			}
		}

		public static TileIndexGrid CreateBlankIndexGrid()
        {
			var indexGrid = ScriptableObject.CreateInstance<TileIndexGrid>();
			var yes = new TileIndexList { indexWeights = new List<float> { 0.1f }, indices = new List<int> { -1 } };

			indexGrid.topLeftIndices = yes;
			indexGrid.topIndices = yes;
			indexGrid.topRightIndices = yes;
			indexGrid.leftIndices = yes;
			indexGrid.centerIndices = yes;
			indexGrid.rightIndices = yes;
			indexGrid.bottomLeftIndices = yes;
			indexGrid.bottomIndices = yes;
			indexGrid.bottomRightIndices = yes;
			indexGrid.horizontalIndices = yes;
			indexGrid.verticalIndices = yes;
			indexGrid.topCapIndices = yes;
			indexGrid.rightCapIndices = yes;
			indexGrid.bottomCapIndices = yes;
			indexGrid.leftCapIndices = yes;
			indexGrid.allSidesIndices = yes;
			indexGrid.topLeftNubIndices = yes;
			indexGrid.topRightNubIndices = yes;
			indexGrid.bottomLeftNubIndices = yes;
			indexGrid.bottomRightNubIndices = yes;

			indexGrid.extendedSet = false;

			indexGrid.topCenterLeftIndices = yes;
			indexGrid.topCenterIndices = yes;
			indexGrid.topCenterRightIndices = yes;
			indexGrid.thirdTopRowLeftIndices = yes;
			indexGrid.thirdTopRowCenterIndices = yes;
			indexGrid.thirdTopRowRightIndices = yes;
			indexGrid.internalBottomLeftCenterIndices = yes;
			indexGrid.internalBottomCenterIndices = yes;
			indexGrid.internalBottomRightCenterIndices = yes;

			indexGrid.borderTopNubLeftIndices = yes;
			indexGrid.borderTopNubRightIndices = yes;
			indexGrid.borderTopNubBothIndices = yes;
			indexGrid.borderRightNubTopIndices = yes;
			indexGrid.borderRightNubBottomIndices = yes;
			indexGrid.borderRightNubBothIndices = yes;
			indexGrid.borderBottomNubLeftIndices = yes;
			indexGrid.borderBottomNubRightIndices = yes;
			indexGrid.borderBottomNubBothIndices = yes;
			indexGrid.borderLeftNubTopIndices = yes;
			indexGrid.borderLeftNubBottomIndices = yes;
			indexGrid.borderLeftNubBothIndices = yes;
			indexGrid.diagonalNubsTopLeftBottomRight = yes;
			indexGrid.diagonalNubsTopRightBottomLeft = yes;
			indexGrid.doubleNubsTop = yes;
			indexGrid.doubleNubsRight = yes;
			indexGrid.doubleNubsBottom = yes;
			indexGrid.doubleNubsLeft = yes;
			indexGrid.quadNubs = yes;
			indexGrid.topRightWithNub = yes;
			indexGrid.topLeftWithNub = yes;
			indexGrid.bottomRightWithNub = yes;
			indexGrid.bottomLeftWithNub = yes;

			indexGrid.diagonalBorderNE = yes;
			indexGrid.diagonalBorderSE = yes;
			indexGrid.diagonalBorderSW = yes;
			indexGrid.diagonalBorderNW = yes;
			indexGrid.diagonalCeilingNE = yes;
			indexGrid.diagonalCeilingSE = yes;
			indexGrid.diagonalCeilingSW = yes;
			indexGrid.diagonalCeilingNW = yes;

			indexGrid.CenterCheckerboard = false;
			indexGrid.CheckerboardDimension = 1;
			indexGrid.CenterIndicesAreStrata = false;

			indexGrid.PitInternalSquareGrids = new List<TileIndexGrid>();

			indexGrid.PitInternalSquareOptions = new PitSquarePlacementOptions { CanBeFlushBottom = false, CanBeFlushLeft = false, CanBeFlushRight = false, PitSquareChance = -1 };

			indexGrid.PitBorderIsInternal = false;

			indexGrid.PitBorderOverridesFloorTile = false;

			indexGrid.CeilingBorderUsesDistancedCenters = false;

			indexGrid.UsesRatChunkBorders = false;
			indexGrid.RatChunkNormalSet = yes;
			indexGrid.RatChunkBottomSet = yes;

			indexGrid.PathFacewallStamp = null;
			indexGrid.PathSidewallStamp = null;

			indexGrid.PathPitPosts = yes;
			indexGrid.PathPitPostsBL = yes;
			indexGrid.PathPitPostsBR = yes;

			indexGrid.PathStubNorth = null;
			indexGrid.PathStubEast = null;
			indexGrid.PathStubSouth = null;
			indexGrid.PathStubWest = null;


			return indexGrid;
		}


		public static void SetupBeyondRoomMaterial(ref DungeonMaterial material)
        {
			material.facewallLightStamps = new List<LightStampData>
			{
				new LightStampData
				{
					width = 1,
					height = 2,
					relativeWeight = 1,
					placementRule = DungeonTileStampData.StampPlacementRule.ON_LOWER_FACEWALL,
					occupySpace = DungeonTileStampData.StampSpace.WALL_SPACE,
					stampCategory = DungeonTileStampData.StampCategory.MUNDANE,
					preferredIntermediaryStamps = 0,
					intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY,
					requiresForcedMatchingStyle = false,
					opulence = Opulence.FINE,
					roomTypeData = new List<StampPerRoomPlacementSettings>(),
					indexOfSymmetricPartner = -1,
					preventRoomRepeats = false,
					objectReference = BeyondPrefabs.shared_auto_001.LoadAsset<GameObject>("DefaultTorchPurple"),
					CanBeCenterLight = true,
					CanBeTopWallLight = true,
					FallbackIndex = 0,
				}
			};

			material.sidewallLightStamps = new List<LightStampData>
			{
				new LightStampData
				{
					width = 1,
					height = 2,
					relativeWeight = 1,
					placementRule = DungeonTileStampData.StampPlacementRule.ON_LOWER_FACEWALL,
					occupySpace = DungeonTileStampData.StampSpace.WALL_SPACE,
					stampCategory = DungeonTileStampData.StampCategory.MUNDANE,
					preferredIntermediaryStamps = 0,
					intermediaryMatchingStyle = DungeonTileStampData.IntermediaryMatchingStyle.ANY,
					requiresForcedMatchingStyle = false,
					opulence = Opulence.FINE,
					roomTypeData = new List<StampPerRoomPlacementSettings>(),
					indexOfSymmetricPartner = -1,
					preventRoomRepeats = false,
					objectReference = BeyondPrefabs.shared_auto_001.LoadAsset<GameObject>("DefaultTorchSidePurple"),
					CanBeCenterLight = true,
					CanBeTopWallLight = true,
					FallbackIndex = 0,
				}
			};

			material.lightPrefabs.elements = new List<WeightedGameObject>
			{
				new WeightedGameObject
				{
					additionalPrerequisites = new DungeonPrerequisite[0],
					forceDuplicatesPossible = false,
					pickupId = -1,
					rawGameObject = BeyondPrefabs.shared_auto_001.LoadAsset<GameObject>("Gungeon Light (Purple)"),
					weight = 1,
				}
			};
		}

		public static AIActor GetNearestEnemy(this RoomHandler room, Vector2 position, out float nearestDistance, List<AIActor> excludedActors, bool includeBosses = true, bool excludeDying = false)
		{
			AIActor result = null;
			nearestDistance = float.MaxValue;

			var activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

			if (activeEnemies == null)
			{
				return null;
			}
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (includeBosses || !activeEnemies[i].healthHaver.IsBoss)
				{
					if (!excludeDying || !activeEnemies[i].healthHaver.IsDead)
					{
						float num = Vector2.Distance(position, activeEnemies[i].CenterPosition);
						if (num < nearestDistance && !excludedActors.Contains(activeEnemies[i]))
						{
							nearestDistance = num;
							result = activeEnemies[i];
						}
					}
				}
			}
			return result;
		}


		public static WeightedGameObjectCollection GetCompiledCollectionButCooler(this GenericLootTable lootTable)
		{
			int num = 0;

			if (lootTable.includedLootTables.Count == 0 && num == 0)
			{
				return lootTable.defaultItemDrops;
			}
			WeightedGameObjectCollection weightedGameObjectCollection = new WeightedGameObjectCollection();
			for (int i = 0; i < lootTable.defaultItemDrops.elements.Count; i++)
			{
				weightedGameObjectCollection.Add(lootTable.defaultItemDrops.elements[i]);
			}
			int j = 0;
			while (j < lootTable.includedLootTables.Count)
			{
				if (lootTable.includedLootTables[j].tablePrerequisites.Length <= 0)
				{
					goto IL_136;
				}
				bool flag = false;
				for (int k = 0; k < lootTable.includedLootTables[j].tablePrerequisites.Length; k++)
				{
					if (!lootTable.includedLootTables[j].tablePrerequisites[k].CheckConditionsFulfilled())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					goto IL_136;
				}
				IL_17F:
				j++;
				continue;
				IL_136:
				WeightedGameObjectCollection compiledCollection = lootTable.includedLootTables[j].GetCompiledCollectionButCooler();
				for (int l = 0; l < compiledCollection.elements.Count; l++)
				{
					weightedGameObjectCollection.Add(compiledCollection.elements[l]);
				}
				goto IL_17F;
			}
			return weightedGameObjectCollection;
		}

		public static GameObject SelectByWeightButGood(this WeightedGameObjectCollection lootTable, out int outIndex, bool useSeedRandom = false)
		{
			outIndex = -1;
			List<WeightedGameObject> list = new List<WeightedGameObject>();
			float num = 0f;
			for (int i = 0; i < lootTable.elements.Count; i++)
			{
				WeightedGameObject weightedGameObject = lootTable.elements[i];
				bool flag = true;
				if (weightedGameObject.additionalPrerequisites != null)
				{
					for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
					{
						if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
						{
							flag = false;
							break;
						}
					}
				}
				if (weightedGameObject.gameObject != null)
				{
					PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
					if (component != null)
					{
						flag = false;
					}
				}
				if (flag)
				{
					list.Add(weightedGameObject);
					num += weightedGameObject.weight;
				}
			}
			float num2 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
			float num3 = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				num3 += list[k].weight;
				if (num3 > num2)
				{
					outIndex = lootTable.elements.IndexOf(list[k]);
					return list[k].gameObject;
				}
			}
			outIndex = lootTable.elements.IndexOf(list[list.Count - 1]);
			return list[list.Count - 1].gameObject;
		}


		public static GameObject SpawnCustomBowlerNote(GameObject note, Vector2 position, RoomHandler parentRoom, string customText, bool doPoof = false)
		{
			GameObject noteObject = UnityEngine.Object.Instantiate(note, position.ToVector3ZisY(0f), Quaternion.identity);
			if (noteObject)
			{
				NoteDoer BowlerNote = noteObject.GetComponentInChildren<NoteDoer>();
				if (BowlerNote)
				{
					if (BowlerNote)
					{
						BowlerNote.alreadyLocalized = true;
						BowlerNote.stringKey = customText;
						BowlerNote.RegenerateCache();
					}
				}
				IPlayerInteractable[] interfacesInChildren = noteObject.GetInterfacesInChildren<IPlayerInteractable>();
				for (int i = 0; i < interfacesInChildren.Length; i++) { parentRoom.RegisterInteractable(interfacesInChildren[i]); }
			}
			if (doPoof)
			{
				GameObject vfxObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
				tk2dBaseSprite component = vfxObject.GetComponent<tk2dBaseSprite>();
				component.PlaceAtPositionByAnchor(position.ToVector3ZUp(0f) + new Vector3(0.5f, 0.75f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
				component.HeightOffGround = 5f;
				component.UpdateZDepth();
			}
			return noteObject;
		}

		public static PickupObject GetPickupObjectFromAnywhere(this GameObject obj)
		{
			
			var po1 = obj.GetComponent<PickupObject>();
			var po2 = obj.GetComponentInChildren<PickupObject>();
			var po3 = obj.GetComponentInParent<PickupObject>();
			if (po1 != null)
			{
				return po1;
			}
			else if(po2 != null)
			{
				return po2;
			}
			else if (po3 != null)
			{
				return po3;
			}
			return null;
		}

		public static Texture2D GenerateTexture2DFromRenderTexture(RenderTexture rTex)
		{
			Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
			var old_rt = RenderTexture.active;
			RenderTexture.active = rTex;
			tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
			tex.Apply();
			RenderTexture.active = old_rt;
			return tex;
		}

		public static Projectile SetupProjectile(int id)
        {
			Projectile proj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(id) as Gun).DefaultModule.projectiles[0]);
			proj.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(proj.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(proj);

			return proj;
		}

		public static Projectile SetupProjectile(Projectile projToCopy)
		{
			Projectile proj = UnityEngine.Object.Instantiate<Projectile>(projToCopy);
			proj.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(proj.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(proj);

			return proj;
		}

		public static Projectile SetupProjectileAndObject(Projectile projToCopy)
		{
			Projectile proj = UnityEngine.Object.Instantiate(projToCopy.gameObject).GetComponent<Projectile>();
			proj.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(proj.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(proj);

			return proj;
		}

		public static List<Transform> GetChildren(this Transform transform)
		{
			List<Transform> children = new List<Transform>();
			foreach (Transform c in transform)
			{
				children.Add(c);
			}
			return children;
		}

		public static List<GameObject> GetChildObjects(this Transform transform)
		{
			List<GameObject> children = new List<GameObject>();
			foreach (Transform c in transform)
			{
				children.Add(c.gameObject);
			}
			return children;
		}
		public static bool HasModdedItem(this PlayerController player, string id)
		{

			var item = ETGMod.Databases.Items.GetModItemByName(id);//ETGMod.Databases.Items[id];// 
			if (item != null)
			{
				return (player.HasPickupID(item.PickupObjectId));
			}
			return false;
		}

		public static void UpdateLink(DebrisObject targetPos, tk2dTiledSprite m_extantLink, DebrisObject landedPoint)
		{

			Vector2 unitCenter = landedPoint.sprite.WorldCenter;
			Vector2 unitCenter2 = targetPos.sprite.WorldCenter;
			m_extantLink.transform.position = unitCenter;
			Vector2 vector = unitCenter2 - unitCenter;
			float num = BraveMathCollege.Atan2Degrees(vector.normalized);
			int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			m_extantLink.dimensions = new Vector2((float)num2, m_extantLink.dimensions.y);
			m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, num);
			m_extantLink.UpdateZDepth();


		}
		public static List<FuckYouThisIsAnAwfulIdea> coinUIControllersList = new List<FuckYouThisIsAnAwfulIdea>();
		public static List<FuckYouThisIsAnAwfulIdea> coinUIControllers(this GameUIRoot hi)
		{
			Debug.Log(hi);
			return coinUIControllersList;
		}

		public static void coinUIControllersAdd(this GameUIRoot hi, FuckYouThisIsAnAwfulIdea help)
		{
			Debug.Log(hi);
			coinUIControllersList.Add(help);
		}



		public static class ReflectionHelpers
		{

			public static IList CreateDynamicList(Type type)
			{
				bool flag = type == null;
				if (flag) { throw new ArgumentNullException("type", "Argument cannot be null."); }
				ConstructorInfo[] constructors = typeof(List<>).MakeGenericType(new Type[] { type }).GetConstructors();
				foreach (ConstructorInfo constructorInfo in constructors)
				{
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					bool flag2 = parameters.Length != 0;
					if (!flag2) { return (IList)constructorInfo.Invoke(null, null); }
				}
				throw new ApplicationException("Could not create a new list with type <" + type.ToString() + ">.");
			}

			public static IDictionary CreateDynamicDictionary(Type typeKey, Type typeValue)
			{
				bool flag = typeKey == null;
				if (flag)
				{
					throw new ArgumentNullException("type_key", "Argument cannot be null.");
				}
				bool flag2 = typeValue == null;
				if (flag2) { throw new ArgumentNullException("type_value", "Argument cannot be null."); }
				ConstructorInfo[] constructors = typeof(Dictionary<,>).MakeGenericType(new Type[] { typeKey, typeValue }).GetConstructors();
				foreach (ConstructorInfo constructorInfo in constructors)
				{
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					bool flag3 = parameters.Length != 0;
					if (!flag3) { return (IDictionary)constructorInfo.Invoke(null, null); }
				}
				throw new ApplicationException(string.Concat(new string[] {
				"Could not create a new dictionary with types <",
				typeKey.ToString(),
				",",
				typeValue.ToString(),
				">."
			}));
			}

			public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
			{
				FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
				return (T)field.GetValue(o);
			}

			public static void ReflectSetField<T>(Type classType, string fieldName, T value, object o = null)
			{
				FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
				field.SetValue(o, value);
			}

			public static T ReflectGetProperty<T>(Type classType, string propName, object o = null, object[] indexes = null)
			{
				PropertyInfo property = classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
				return (T)property.GetValue(o, indexes);
			}

			public static void ReflectSetProperty<T>(Type classType, string propName, T value, object o = null, object[] indexes = null)
			{
				PropertyInfo property = classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
				property.SetValue(o, value, indexes);
			}

			public static MethodInfo ReflectGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
			{
				MethodInfo[] array = ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
				bool flag = array.Count() == 0;
				if (flag) { throw new MissingMethodException("Cannot reflect method, not found based on input parameters."); }
				bool flag2 = array.Count() > 1;
				if (flag2) { throw new InvalidOperationException("Cannot reflect method, more than one method matched based on input parameters."); }
				return array[0];
			}

			public static MethodInfo ReflectTryGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
			{
				MethodInfo[] array = ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
				bool flag = array.Count() == 0;
				MethodInfo result;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = array.Count() > 1;
					if (flag2) { result = null; } else { result = array[0]; }
				}
				return result;
			}

			public static MethodInfo[] ReflectTryGetMethods(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
			{
				BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
				bool flag = isStatic == null || isStatic.Value;
				if (flag) { bindingFlags |= BindingFlags.Static; }
				bool flag2 = isStatic == null || !isStatic.Value;
				if (flag2) { bindingFlags |= BindingFlags.Instance; }
				MethodInfo[] methods = classType.GetMethods(bindingFlags);
				List<MethodInfo> list = new List<MethodInfo>();
				for (int i = 0; i < methods.Length; i++)
				{
					// foreach (MethodInfo methodInfo in methods) {
					bool flag3 = methods[i].Name != methodName;
					if (!flag3)
					{
						bool isGenericMethodDefinition = methods[i].IsGenericMethodDefinition;
						if (isGenericMethodDefinition)
						{
							bool flag4 = genericMethodTypes == null || genericMethodTypes.Length == 0;
							if (flag4) { goto IL_14D; }
							Type[] genericArguments = methods[i].GetGenericArguments();
							bool flag5 = genericArguments.Length != genericMethodTypes.Length;
							if (flag5) { goto IL_14D; }
							methods[i] = methods[i].MakeGenericMethod(genericMethodTypes);
						}
						else
						{
							bool flag6 = genericMethodTypes != null && genericMethodTypes.Length != 0;
							if (flag6) { goto IL_14D; }
						}
						ParameterInfo[] parameters = methods[i].GetParameters();
						bool flag7 = methodArgumentTypes != null;
						if (!flag7) { goto IL_141; }
						bool flag8 = parameters.Length != methodArgumentTypes.Length;
						if (!flag8)
						{
							for (int j = 0; j < parameters.Length; j++)
							{
								ParameterInfo parameterInfo = parameters[j];
								bool flag9 = parameterInfo.ParameterType != methodArgumentTypes[j];
								if (flag9) { goto IL_14A; }
							}
							goto IL_141;
						}
					IL_14A:
						goto IL_14D;
					IL_141:
						list.Add(methods[i]);
					}
				IL_14D:;
				}
				return list.ToArray();
			}

			public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, T0 p0)
			{
				object[] parameters = new object[] { p0 };
				return methodInfo.Invoke(o, parameters);
			}

			public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, ref T0 p0)
			{
				object[] array = new object[] { p0 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				return result;
			}

			public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, T1 p1)
			{
				object[] parameters = new object[] { p0, p1 };
				return methodInfo.Invoke(o, parameters);
			}

			public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1)
			{
				object[] array = new object[] { p0, p1 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				return result;
			}

			public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1)
			{
				object[] array = new object[] { p0, p1 };
				object result = methodInfo.Invoke(o, array);
				p1 = (T1)array[1];
				return result;
			}

			public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1)
			{
				object[] array = new object[] { p0, p1 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				p1 = (T1)array[1];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, T2 p2)
			{
				object[] parameters = new object[] { p0, p1, p2 };
				return methodInfo.Invoke(o, parameters);
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p1 = (T1)array[1];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, ref T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p2 = (T2)array[2];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				p1 = (T1)array[1];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, ref T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				p2 = (T2)array[2];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, ref T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p1 = (T1)array[1];
				p2 = (T2)array[2];
				return result;
			}

			public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, ref T2 p2)
			{
				object[] array = new object[] { p0, p1, p2 };
				object result = methodInfo.Invoke(o, array);
				p0 = (T0)array[0];
				p1 = (T1)array[1];
				p2 = (T2)array[2];
				return result;
			}
		}


		public static DungeonPlaceable GenerateDungeonPlacable(GameObject ObjectPrefab = null, bool spawnsEnemy = false, bool useExternalPrefab = false, bool spawnsItem = false, string EnemyGUID = "479556d05c7c44f3b6abb3b2067fc778", int itemID = 307, Vector2? CustomOffset = null, bool itemHasDebrisObject = true, float spawnChance = 1f)
		{
			AssetBundle m_assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			AssetBundle m_assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
			AssetBundle m_resourceBundle = ResourceManager.LoadAssetBundle("brave_resources_001");

			// Used with custom DungeonPlacable        
			GameObject ChestBrownTwoItems = m_assetBundle.LoadAsset<GameObject>("Chest_Wood_Two_Items");
			GameObject Chest_Silver = m_assetBundle.LoadAsset<GameObject>("chest_silver");
			GameObject Chest_Green = m_assetBundle.LoadAsset<GameObject>("chest_green");
			GameObject Chest_Synergy = m_assetBundle.LoadAsset<GameObject>("chest_synergy");
			GameObject Chest_Red = m_assetBundle.LoadAsset<GameObject>("chest_red");
			GameObject Chest_Black = m_assetBundle.LoadAsset<GameObject>("Chest_Black");
			GameObject Chest_Rainbow = m_assetBundle.LoadAsset<GameObject>("Chest_Rainbow");
			// GameObject Chest_Rat = m_assetBundle.LoadAsset<GameObject>("Chest_Rat");

			m_assetBundle = null;
			m_assetBundle2 = null;
			m_resourceBundle = null;

			DungeonPlaceableVariant BlueChestVariant = new DungeonPlaceableVariant();
			BlueChestVariant.percentChance = 0.35f;
			BlueChestVariant.unitOffset = new Vector2(1, 0.8f);
			BlueChestVariant.enemyPlaceableGuid = string.Empty;
			BlueChestVariant.pickupObjectPlaceableId = -1;
			BlueChestVariant.forceBlackPhantom = false;
			BlueChestVariant.addDebrisObject = false;
			BlueChestVariant.prerequisites = null;
			BlueChestVariant.materialRequirements = null;
			BlueChestVariant.nonDatabasePlaceable = Chest_Silver;

			DungeonPlaceableVariant BrownChestVariant = new DungeonPlaceableVariant();
			BrownChestVariant.percentChance = 0.28f;
			BrownChestVariant.unitOffset = new Vector2(1, 0.8f);
			BrownChestVariant.enemyPlaceableGuid = string.Empty;
			BrownChestVariant.pickupObjectPlaceableId = -1;
			BrownChestVariant.forceBlackPhantom = false;
			BrownChestVariant.addDebrisObject = false;
			BrownChestVariant.prerequisites = null;
			BrownChestVariant.materialRequirements = null;
			BrownChestVariant.nonDatabasePlaceable = ChestBrownTwoItems;

			DungeonPlaceableVariant GreenChestVariant = new DungeonPlaceableVariant();
			GreenChestVariant.percentChance = 0.25f;
			GreenChestVariant.unitOffset = new Vector2(1, 0.8f);
			GreenChestVariant.enemyPlaceableGuid = string.Empty;
			GreenChestVariant.pickupObjectPlaceableId = -1;
			GreenChestVariant.forceBlackPhantom = false;
			GreenChestVariant.addDebrisObject = false;
			GreenChestVariant.prerequisites = null;
			GreenChestVariant.materialRequirements = null;
			GreenChestVariant.nonDatabasePlaceable = Chest_Green;

			DungeonPlaceableVariant SynergyChestVariant = new DungeonPlaceableVariant();
			SynergyChestVariant.percentChance = 0.2f;
			SynergyChestVariant.unitOffset = new Vector2(1, 0.8f);
			SynergyChestVariant.enemyPlaceableGuid = string.Empty;
			SynergyChestVariant.pickupObjectPlaceableId = -1;
			SynergyChestVariant.forceBlackPhantom = false;
			SynergyChestVariant.addDebrisObject = false;
			SynergyChestVariant.prerequisites = null;
			SynergyChestVariant.materialRequirements = null;
			SynergyChestVariant.nonDatabasePlaceable = Chest_Synergy;

			DungeonPlaceableVariant RedChestVariant = new DungeonPlaceableVariant();
			RedChestVariant.percentChance = 0.15f;
			RedChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
			RedChestVariant.enemyPlaceableGuid = string.Empty;
			RedChestVariant.pickupObjectPlaceableId = -1;
			RedChestVariant.forceBlackPhantom = false;
			RedChestVariant.addDebrisObject = false;
			RedChestVariant.prerequisites = null;
			RedChestVariant.materialRequirements = null;
			RedChestVariant.nonDatabasePlaceable = Chest_Red;

			DungeonPlaceableVariant BlackChestVariant = new DungeonPlaceableVariant();
			BlackChestVariant.percentChance = 0.1f;
			BlackChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
			BlackChestVariant.enemyPlaceableGuid = string.Empty;
			BlackChestVariant.pickupObjectPlaceableId = -1;
			BlackChestVariant.forceBlackPhantom = false;
			BlackChestVariant.addDebrisObject = false;
			BlackChestVariant.prerequisites = null;
			BlackChestVariant.materialRequirements = null;
			BlackChestVariant.nonDatabasePlaceable = Chest_Black;

			DungeonPlaceableVariant RainbowChestVariant = new DungeonPlaceableVariant();
			RainbowChestVariant.percentChance = 0.005f;
			RainbowChestVariant.unitOffset = new Vector2(0.5f, 0.5f);
			RainbowChestVariant.enemyPlaceableGuid = string.Empty;
			RainbowChestVariant.pickupObjectPlaceableId = -1;
			RainbowChestVariant.forceBlackPhantom = false;
			RainbowChestVariant.addDebrisObject = false;
			RainbowChestVariant.prerequisites = null;
			RainbowChestVariant.materialRequirements = null;
			RainbowChestVariant.nonDatabasePlaceable = Chest_Rainbow;

			DungeonPlaceableVariant ItemVariant = new DungeonPlaceableVariant();
			ItemVariant.percentChance = spawnChance;
			if (CustomOffset.HasValue)
			{
				ItemVariant.unitOffset = CustomOffset.Value;
			}
			else
			{
				ItemVariant.unitOffset = Vector2.zero;
			}
			// ItemVariant.unitOffset = new Vector2(0.5f, 0.8f);
			ItemVariant.enemyPlaceableGuid = string.Empty;
			ItemVariant.pickupObjectPlaceableId = itemID;
			ItemVariant.forceBlackPhantom = false;
			if (itemHasDebrisObject)
			{
				ItemVariant.addDebrisObject = true;
			}
			else
			{
				ItemVariant.addDebrisObject = false;
			}
			RainbowChestVariant.prerequisites = null;
			RainbowChestVariant.materialRequirements = null;

			List<DungeonPlaceableVariant> ChestTiers = new List<DungeonPlaceableVariant>();
			ChestTiers.Add(BrownChestVariant);
			ChestTiers.Add(BlueChestVariant);
			ChestTiers.Add(GreenChestVariant);
			ChestTiers.Add(SynergyChestVariant);
			ChestTiers.Add(RedChestVariant);
			ChestTiers.Add(BlackChestVariant);
			ChestTiers.Add(RainbowChestVariant);

			DungeonPlaceableVariant EnemyVariant = new DungeonPlaceableVariant();
			EnemyVariant.percentChance = spawnChance;
			EnemyVariant.unitOffset = Vector2.zero;
			EnemyVariant.enemyPlaceableGuid = EnemyGUID;
			EnemyVariant.pickupObjectPlaceableId = -1;
			EnemyVariant.forceBlackPhantom = false;
			EnemyVariant.addDebrisObject = false;
			EnemyVariant.prerequisites = null;
			EnemyVariant.materialRequirements = null;

			List<DungeonPlaceableVariant> EnemyTiers = new List<DungeonPlaceableVariant>();
			EnemyTiers.Add(EnemyVariant);

			List<DungeonPlaceableVariant> ItemTiers = new List<DungeonPlaceableVariant>();
			ItemTiers.Add(ItemVariant);

			DungeonPlaceable m_cachedCustomPlacable = ScriptableObject.CreateInstance<DungeonPlaceable>();
			m_cachedCustomPlacable.name = "CustomChestPlacable";
			if (spawnsEnemy | useExternalPrefab)
			{
				m_cachedCustomPlacable.width = 2;
				m_cachedCustomPlacable.height = 2;
			}
			else if (spawnsItem)
			{
				m_cachedCustomPlacable.width = 1;
				m_cachedCustomPlacable.height = 1;
			}
			else
			{
				m_cachedCustomPlacable.width = 4;
				m_cachedCustomPlacable.height = 1;
			}
			m_cachedCustomPlacable.roomSequential = false;
			m_cachedCustomPlacable.respectsEncounterableDifferentiator = true;
			m_cachedCustomPlacable.UsePrefabTransformOffset = false;
			m_cachedCustomPlacable.isPassable = true;
			if (spawnsItem)
			{
				m_cachedCustomPlacable.MarkSpawnedItemsAsRatIgnored = true;
			}
			else
			{
				m_cachedCustomPlacable.MarkSpawnedItemsAsRatIgnored = false;
			}

			m_cachedCustomPlacable.DebugThisPlaceable = false;
			if (useExternalPrefab && ObjectPrefab != null)
			{
				DungeonPlaceableVariant ExternalObjectVariant = new DungeonPlaceableVariant();
				ExternalObjectVariant.percentChance = spawnChance;
				if (CustomOffset.HasValue)
				{
					ExternalObjectVariant.unitOffset = CustomOffset.Value;
				}
				else
				{
					ExternalObjectVariant.unitOffset = Vector2.zero;
				}
				ExternalObjectVariant.enemyPlaceableGuid = string.Empty;
				ExternalObjectVariant.pickupObjectPlaceableId = -1;
				ExternalObjectVariant.forceBlackPhantom = false;
				ExternalObjectVariant.addDebrisObject = false;
				ExternalObjectVariant.nonDatabasePlaceable = ObjectPrefab;
				List<DungeonPlaceableVariant> ExternalObjectTiers = new List<DungeonPlaceableVariant>();
				ExternalObjectTiers.Add(ExternalObjectVariant);
				m_cachedCustomPlacable.variantTiers = ExternalObjectTiers;
			}
			else if (spawnsEnemy)
			{
				m_cachedCustomPlacable.variantTiers = EnemyTiers;
			}
			else if (spawnsItem)
			{
				m_cachedCustomPlacable.variantTiers = ItemTiers;
			}
			else
			{
				m_cachedCustomPlacable.variantTiers = ChestTiers;
			}
			return m_cachedCustomPlacable;
		}

		public static List<T> ConstructListOfSameValues<T>(T value, int length)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < length; i++)
			{
				list.Add(value);
			}
			return list;
		}

		public static void AnimateProjectile(this Projectile proj, List<string> names, int fps, tk2dSpriteAnimationClip.WrapMode wrapMode, int loopStart, List<IntVector2> pixelSizes, List<bool> lighteneds, List<tk2dBaseSprite.Anchor> anchors, List<bool> anchorsChangeColliders,
			List<bool> fixesScales, List<Vector3?> manualOffsets, List<IntVector2?> overrideColliderPixelSizes, List<IntVector2?> overrideColliderOffsets, List<Projectile> overrideProjectilesToCopyFrom)
		{
			tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
			clip.name = "idle";
			clip.fps = fps;
			List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
			for (int i = 0; i < names.Count; i++)
			{
				string name = names[i];
				IntVector2 pixelSize = pixelSizes[i];
				IntVector2? overrideColliderPixelSize = overrideColliderPixelSizes[i];
				IntVector2? overrideColliderOffset = overrideColliderOffsets[i];
				Vector3? manualOffset = manualOffsets[i];
				bool anchorChangesCollider = anchorsChangeColliders[i];
				bool fixesScale = fixesScales[i];
				if (!manualOffset.HasValue)
				{
					manualOffset = new Vector2?(Vector2.zero);
				}
				tk2dBaseSprite.Anchor anchor = anchors[i];
				bool lightened = lighteneds[i];
				Projectile overrideProjectileToCopyFrom = overrideProjectilesToCopyFrom[i];
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
				frame.spriteCollection = ETGMod.Databases.Items.ProjectileCollection;
				frames.Add(frame);
				int? overrideColliderPixelWidth = null;
				int? overrideColliderPixelHeight = null;
				if (overrideColliderPixelSize.HasValue)
				{
					overrideColliderPixelWidth = overrideColliderPixelSize.Value.x;
					overrideColliderPixelHeight = overrideColliderPixelSize.Value.y;
				}
				int? overrideColliderOffsetX = null;
				int? overrideColliderOffsetY = null;
				if (overrideColliderOffset.HasValue)
				{
					overrideColliderOffsetX = overrideColliderOffset.Value.x;
					overrideColliderOffsetY = overrideColliderOffset.Value.y;
				}
				tk2dSpriteDefinition def = SetupDefinitionForProjectileSprite(name, frame.spriteId, pixelSize.x, pixelSize.y, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX, overrideColliderOffsetY,
					overrideProjectileToCopyFrom);
				def.ConstructOffsetsFromAnchor(anchor, def.position3, fixesScale, anchorChangesCollider);
				def.position0 += manualOffset.Value;
				def.position1 += manualOffset.Value;
				def.position2 += manualOffset.Value;
				def.position3 += manualOffset.Value;
				if (i == 0)
				{
					proj.GetAnySprite().SetSprite(frame.spriteCollection, frame.spriteId);
				}
			}
			clip.wrapMode = wrapMode;
			clip.loopStart = loopStart;
			clip.frames = frames.ToArray();
			if (proj.sprite.spriteAnimator == null)
			{
				proj.sprite.spriteAnimator = proj.sprite.gameObject.AddComponent<tk2dSpriteAnimator>();
			}
			proj.sprite.spriteAnimator.playAutomatically = true;
			bool flag = proj.sprite.spriteAnimator.Library == null;
			if (flag)
			{
				proj.sprite.spriteAnimator.Library = proj.sprite.spriteAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
				proj.sprite.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
				proj.sprite.spriteAnimator.Library.enabled = true;
			}
			proj.sprite.spriteAnimator.Library.clips = proj.sprite.spriteAnimator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
			proj.sprite.spriteAnimator.DefaultClipId = proj.sprite.spriteAnimator.Library.GetClipIdByName("idle");
			proj.sprite.spriteAnimator.deferNextStartClip = false;
		}

		
		public static void AddFlashRayBeam(this Projectile projectile, List<string> animationPaths, Vector2 colliderDimensions, Vector2 colliderOffsets, int fps = 12, List<string> startAnimationPaths = null, int startFps = 12, GameObject trailObject = null)
		{
			try
			{
				if (trailObject == null)
                {
					trailObject = new GameObject("trail");
					FakePrefab.MarkAsFakePrefab(trailObject);
					trailObject.layer = 22;

				}

				trailObject.transform.parent = projectile.transform;
				//trailObject.transform.localPosition = new Vector3(-2.548388f, -0.06926762f, -0.06926762f);
				trailObject.SetActive(true);
				var split = animationPaths[0].Split('/');
				int spriteID = SpriteBuilder.AddSpriteToCollection(animationPaths[0], ETGMod.Databases.Items.ProjectileCollection, split.Last());
				tk2dTiledSprite tiledSprite = trailObject.GetOrAddComponent<tk2dTiledSprite>();
				TrailController trailController = trailObject.GetOrAddComponent<TrailController>();

				trailController.usesAnimation = true;
				trailController.usesStartAnimation = true;

				trailController.startAnimation = "trail_start";


				trailController.animation = "trail";

				trailController.usesCascadeTimer = false;
				trailController.usesSoftMaxLength = false;
				trailController.usesGlobalTimer = true;
				trailController.destroyOnEmpty = false;
				trailController.FlipUvsY = false;
				trailController.rampHeight = false;

				trailController.UsesDispersalParticles = false;

				trailController.cascadeTimer = 0.1f;
				trailController.softMaxLength = 0f;
				trailController.globalTimer = 0f;
				trailController.rampStartHeight = 2f;
				trailController.rampTime = 1f;
				trailController.DispersalDensity = 5f;
				trailController.DispersalMinCoherency = 0.4f;
				trailController.DispersalMaxCoherency = 0.9f;
				trailController.boneSpawnOffset = Vector2.zero;
				trailController.DispersalParticleSystemPrefab = null;

				tk2dSpriteAnimator animator = trailObject.GetOrAddComponent<tk2dSpriteAnimator>();

				tk2dSpriteAnimation animation = trailObject.GetOrAddComponent<tk2dSpriteAnimation>();

				float convertedColliderX = colliderDimensions.x / 16f;
				float convertedColliderY = colliderDimensions.y / 16f;
				float convertedOffsetX = colliderOffsets.x / 16f;
				float convertedOffsetY = colliderOffsets.y / 16f;


				tiledSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
				tiledSprite.automaticallyManagesDepth = true;

				tiledSprite.dimensions = new Vector2(0, 14);
				tk2dSpriteDefinition def = tiledSprite.GetCurrentSpriteDef();
				def.colliderVertices = new Vector3[]{
					new Vector3(convertedOffsetX, convertedOffsetY, 0f),
					new Vector3(convertedColliderX, convertedColliderY, 0f)
				};

				def.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);


				animation.clips = new tk2dSpriteAnimationClip[0];
				animator.Library = animation;
				
				if (animationPaths != null)
				{
					tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "trail", frames = new tk2dSpriteAnimationFrame[0], fps = fps, wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop };
					List<string> spritePaths = animationPaths;

					List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
					foreach (string path in spritePaths)
					{
						split = path.Split('/');
						tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
						int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection, split.Last());
						tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
						frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
						frameDef.colliderVertices = def.colliderVertices;
						frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
					}
					
					clip.frames = frames.ToArray();
					animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
				}

				if (startAnimationPaths != null)
				{
					tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "trail_start", frames = new tk2dSpriteAnimationFrame[0], fps = fps, wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop };
					List<string> spritePaths = startAnimationPaths;

					List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
					foreach (string path in spritePaths)
					{
						split = path.Split('/');
						tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
						int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection, split.Last());
						tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
						frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
						frameDef.colliderVertices = def.colliderVertices;
						frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
					}

					clip.frames = frames.ToArray();
					animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
				}
				else
                {
					tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "trail_start", frames = new tk2dSpriteAnimationFrame[0], fps = fps, wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop };
					List<string> spritePaths = animationPaths;

					List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
					foreach (string path in spritePaths)
					{
						split = path.Split('/');
						tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
						int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection, split.Last());
						tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
						frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
						frameDef.colliderVertices = def.colliderVertices;
						frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
					}

					clip.frames = frames.ToArray();
					animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
				}


				animator.DefaultClipId = animation.GetClipIdByName("trail");

			}
			catch (Exception e)
			{
				ETGModConsole.Log(e.ToString());
			}
		}

		public static bool IsStarterItem(this PlayerController player, int id)
        {
			return player.startingActiveItemIds.Contains(id) || player.startingAlternateGunIds.Contains(id) || player.startingGunIds.Contains(id) || player.startingPassiveItemIds.Contains(id);

		}

		public static GameObject MakeLine(string spritePath, Vector2 colliderDimensions, Vector2 colliderOffsets)
		{
			try
			{

				var line = new GameObject("line");

				float convertedColliderX = colliderDimensions.x / 16f;
				float convertedColliderY = colliderDimensions.y / 16f;
				float convertedOffsetX = colliderOffsets.x / 16f;
				float convertedOffsetY = colliderOffsets.y / 16f;

				int spriteID = SpriteBuilder.AddSpriteToCollection(spritePath, ETGMod.Databases.Items.ProjectileCollection);
				tk2dTiledSprite tiledSprite = line.GetOrAddComponent<tk2dTiledSprite>();

				tiledSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
				tk2dSpriteDefinition def = tiledSprite.GetCurrentSpriteDef();
				def.colliderVertices = new Vector3[]{
					new Vector3(convertedOffsetX, convertedOffsetY, 0f),
					new Vector3(convertedColliderX, convertedColliderY, 0f)
				};

				def.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);

				//tiledSprite.anchor = tk2dBaseSprite.Anchor.MiddleCenter;
				/*tk2dSpriteAnimator animator = line.GetOrAddComponent<tk2dSpriteAnimator>();
				tk2dSpriteAnimation animation = line.GetOrAddComponent<tk2dSpriteAnimation>();
				animation.clips = new tk2dSpriteAnimationClip[0];
				animator.Library = animation;
				
				if (beamAnimationPaths != null)
				{
					tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "line_idle_thing", frames = new tk2dSpriteAnimationFrame[0], fps = beamFPS };
					List<string> spritePaths = beamAnimationPaths;

					List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
					foreach (string path in spritePaths)
					{
						tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
						int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection);
						tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
						frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
						frameDef.colliderVertices = def.colliderVertices;
						frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
					}
					clip.frames = frames.ToArray();
					animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
				}*/
				
				return line;
			}
			catch (Exception e)
			{
				ETGModConsole.Log(e.ToString());
				return null;
			}
		}

		public static void SetupSpriteOffset(this tk2dSpriteDefinition def, Vector3 offset, bool useDodgeRollUnits = true)
        {
			if (useDodgeRollUnits)
            {
				offset = offset / 16;
			}
			def.position0 += offset;
			def.position1 += offset;
			def.position2 += offset;
			def.position3 += offset;
		}

		public static BasicBeamController GenerateBeamPrefab(this Projectile projectile, string spritePath, Vector2 colliderDimensions, Vector2 colliderOffsets, List<string> beamAnimationPaths = null, int beamFPS = -1, List<string> impactVFXAnimationPaths = null, int beamImpactFPS = -1, Vector2? impactVFXColliderDimensions = null, Vector2? impactVFXColliderOffsets = null, List<string> endVFXAnimationPaths = null, int beamEndFPS = -1, Vector2? endVFXColliderDimensions = null, Vector2? endVFXColliderOffsets = null, List<string> muzzleVFXAnimationPaths = null, int beamMuzzleFPS = -1, Vector2? muzzleVFXColliderDimensions = null, Vector2? muzzleVFXColliderOffsets = null, bool glows = false)
		{
			try
			{
				//BotsModule.Log("beam 0");
				if (projectile.specRigidbody != null)
                {
					projectile.specRigidbody.CollideWithOthers = false;
				}
				
				float convertedColliderX = colliderDimensions.x / 16f;
				float convertedColliderY = colliderDimensions.y / 16f;
				float convertedOffsetX = colliderOffsets.x / 16f;
				float convertedOffsetY = colliderOffsets.y / 16f;

				int spriteID = SpriteBuilder.AddSpriteToCollection(spritePath, ETGMod.Databases.Items.ProjectileCollection, spritePath.Split('/').Last());
				tk2dTiledSprite tiledSprite = projectile.gameObject.GetOrAddComponent<tk2dTiledSprite>();

				//BotsModule.Log("beam 1");

				tiledSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
				tk2dSpriteDefinition def = tiledSprite.GetCurrentSpriteDef();
				def.colliderVertices = new Vector3[]{
					new Vector3(convertedOffsetX, convertedOffsetY, 0f),
					new Vector3(convertedColliderX, convertedColliderY, 0f)
				};

				def.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);

				//tiledSprite.anchor = tk2dBaseSprite.Anchor.MiddleCenter;
				tk2dSpriteAnimator animator = projectile.gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
				tk2dSpriteAnimation animation = projectile.gameObject.GetOrAddComponent<tk2dSpriteAnimation>();
				animation.clips = new tk2dSpriteAnimationClip[0];
				animator.Library = animation;
				UnityEngine.Object.Destroy(projectile.GetComponentInChildren<tk2dSprite>());
				BasicBeamController beamController = projectile.gameObject.GetOrAddComponent<BasicBeamController>();
				projectile.sprite = tiledSprite;
				//BotsModule.Log("---------------- Sets up the animation for the main part of the beam");
				if (beamAnimationPaths != null)
				{
					tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "beam_idle", frames = new tk2dSpriteAnimationFrame[0], fps = beamFPS };
					List<string> spritePaths = beamAnimationPaths;

					List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
					foreach (string path in spritePaths)
					{
						tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
						int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection, path.Split('/').Last());
						tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
						frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
						frameDef.colliderVertices = def.colliderVertices;
						frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
					}
					clip.frames = frames.ToArray();
					animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
					beamController.beamAnimation = "beam_idle";
				}
				//BotsModule.Log("------------- Sets up the animation for the part of the beam that touches the wall");
				if (endVFXAnimationPaths != null && endVFXColliderDimensions != null && endVFXColliderOffsets != null)
				{
					SetupBeamPart(animation, endVFXAnimationPaths, "beam_end", beamEndFPS, (Vector2)endVFXColliderDimensions, (Vector2)endVFXColliderOffsets);
					beamController.beamEndAnimation = "beam_end";
				}
				else
				{
					SetupBeamPart(animation, beamAnimationPaths, "beam_end", beamFPS, null, null, def.colliderVertices);
					beamController.beamEndAnimation = "beam_end";
				}

				//BotsModule.Log("---------------Sets up the animaton for the VFX that plays over top of the end of the beam where it hits stuff");
				if (impactVFXAnimationPaths != null && impactVFXColliderDimensions != null && impactVFXColliderOffsets != null)
				{
					SetupBeamPart(animation, impactVFXAnimationPaths, "beam_impact", beamImpactFPS, (Vector2)impactVFXColliderDimensions, (Vector2)impactVFXColliderOffsets);
					beamController.impactAnimation = "beam_impact";
				}

				//BotsModule.Log("--------------Sets up the animation for the very start of the beam");
				if (muzzleVFXAnimationPaths != null && muzzleVFXColliderDimensions != null && muzzleVFXColliderOffsets != null)
				{
					SetupBeamPart(animation, muzzleVFXAnimationPaths, "beam_start", beamMuzzleFPS, (Vector2)muzzleVFXColliderDimensions, (Vector2)muzzleVFXColliderOffsets);
					beamController.beamStartAnimation = "beam_start";
				}
				else
				{
					SetupBeamPart(animation, beamAnimationPaths, "beam_start", beamFPS, null, null, def.colliderVertices);
					beamController.beamStartAnimation = "beam_start";
				}

				if (glows)
				{
					EmmisiveBeams emission = projectile.gameObject.GetOrAddComponent<EmmisiveBeams>();
					//emission

				}
				return beamController;
			}
			catch (Exception e)
			{
				ETGModConsole.Log(e.ToString());
				return null;
			}
		}

		private static void SetupBeamPart(tk2dSpriteAnimation beamAnimation, List<string> animSpritePaths, string animationName, int fps, Vector2? colliderDimensions = null, Vector2? colliderOffsets = null, Vector3[] overrideVertices = null)
		{
			tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = animationName, frames = new tk2dSpriteAnimationFrame[0], fps = fps };
			List<string> spritePaths = animSpritePaths;

			List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
			foreach (string path in spritePaths)
			{
				tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
				int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection, path.Split('/').Last());
				tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
				frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
				if (overrideVertices != null)
				{
					frameDef.colliderVertices = overrideVertices;
				}
				else
				{
					if (colliderDimensions == null || colliderOffsets == null)
					{
						ETGModConsole.Log("<size=100><color=#ff0000ff>BEAM ERROR: colliderDimensions or colliderOffsets was null with no override vertices!</color></size>", false);
					}
					else
					{
						Vector2 actualDimensions = (Vector2)colliderDimensions;
						Vector2 actualOffsets = (Vector2)colliderDimensions;
						frameDef.colliderVertices = new Vector3[]{
							new Vector3(actualOffsets.x / 16, actualOffsets.y / 16, 0f),
							new Vector3(actualDimensions.x / 16, actualDimensions.y / 16, 0f)
						};
					}
				}
				frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
			}
			clip.frames = frames.ToArray();
			beamAnimation.clips = beamAnimation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
		}

		/*
		public static void RerollShops(PlayerController user)
		{
			foreach (BaseShopController baseShopController in StaticReferenceManager.AllShops)
			{
				Console.WriteLine(string.Format("Searching shop <{0}>: {1} {2}", baseShopController.GetType().Name, baseShopController.baseShopType, baseShopController));
				List<ShopItemController> list = ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", baseShopController);
				for (int i = 0; i < list.Count; i++)
				{
					ShopItemController shopItemController = list[i];
					bool flag = shopItemController == null;
					if (!flag)
					{
						PickupObject item = shopItemController.item;
						bool flag2 = !this.ItemIsRerollable(item, null, data);
						if (!flag2)
						{
							GameObject gameObject = null;
							bool flag3 = item is PassiveItem;
							if (flag3)
							{
								gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(data.ItemsToAvoidPickingForPassives, false);
								bool flag4 = gameObject != null;
								if (flag4)
								{
									data.ItemsToAvoidPickingForPassives.Add(gameObject);
								}
							}
							else
							{
								bool flag5 = item is PlayerItem;
								if (flag5)
								{
									gameObject = GameManager.Instance.RewardManager.ItemsLootTable.SelectByWeightWithoutDuplicates(data.ItemsToAvoidPickingForActives, false);
									bool flag6 = gameObject != null;
									if (flag6)
									{
										data.ItemsToAvoidPickingForActives.Add(gameObject);
									}
								}
								else
								{
									bool flag7 = item is Gun;
									if (flag7)
									{
										gameObject = GameManager.Instance.RewardManager.GunsLootTable.SelectByWeightWithoutDuplicates(data.ItemsToAvoidPickingForGuns, false);
										bool flag8 = gameObject != null;
										if (flag8)
										{
											data.ItemsToAvoidPickingForGuns.Add(gameObject);
										}
									}
								}
							}
							bool flag9 = gameObject == null;
							if (flag9)
							{
								Console.WriteLine("Couldn't add an item! Giving junk instead.");
								PickupObject pickupObject = Game.Items.Get("gungeon:junk");
								bool flag10 = pickupObject == null;
								if (flag10)
								{
									Console.WriteLine("Cannot get 'gungeon:junk' item! Not changing shop!");
								}
								else
								{
									gameObject = pickupObject.gameObject;
								}
							}
							bool flag11 = gameObject != null;
							if (flag11)
							{
								PickupObject component = gameObject.GetComponent<PickupObject>();
								Console.WriteLine(string.Format("Attempting to change shop contents: {0}={1} to {2}={3}", new object[]
								{
									item.PickupObjectId,
									item.name,
									component.PickupObjectId,
									component.name
								}));
								ShopItemController x = ReplaceShopItem(baseShopController, shopItemController, gameObject);
								bool flag12 = true;
								bool flag13 = flag12 && x != null;
								if (flag13)
								{
									LootEngine.DoDefaultItemPoof(base.sprite.WorldCenter, false, false);
								}
							}
						}
					}
				}
			}
		}

		public static ShopItemController ReplaceShopItem(BaseShopController shopController, ShopItemController oldShopItemController, GameObject itemToAdd)
		{
			PickupObject component = itemToAdd.GetComponent<PickupObject>();
			List<ShopItemController> list = ReflectGetField<List<ShopItemController>>(typeof(BaseShopController), "m_itemControllers", shopController);
			List<GameObject> list2 = ReflectGetField<List<GameObject>>(typeof(BaseShopController), "m_shopItems", shopController);
			for (int i = 0; i < list2.Count; i++)
			{
				GameObject gameObject = list2[i];
				bool flag = gameObject == null;
				if (!flag)
				{
					PickupObject component2 = gameObject.GetComponent<PickupObject>();
					bool flag2 = component2 == null;
					if (!flag2)
					{
						bool flag3 = component2 != oldShopItemController.item;
						if (!flag3)
						{
							Transform parent = oldShopItemController.gameObject.transform.parent;
							bool flag4 = parent == null;
							if (!flag4)
							{
								list2[i] = itemToAdd;
								InitializeInternal(oldShopItemController, component);
								return oldShopItemController;
							}
							Console.WriteLine("null transform");
						}
					}
				}
			}
			Console.WriteLine("Did not replace shop item! Could not find old item in the shop's List<GameObject> shop items.");
			return null;
		}

		public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
		{
			FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
			return (T)((object)field.GetValue(o));
		}

		public static void InitializeInternal(ShopItemController sic, PickupObject i)
		{
			BaseShopController baseShopController = ReflectGetField<BaseShopController>(typeof(ShopItemController), "m_baseParentShop", sic);
			ShopController shopController = ReflectGetField<ShopController>(typeof(ShopItemController), "m_parentShop", sic);
			sic.item = i;
			bool flag = i is SpecialKeyItem && (i as SpecialKeyItem).keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR;
			if (flag)
			{
				sic.IsResourcefulRatKey = true;
			}
			bool flag2 = sic.item && sic.item.encounterTrackable;
			if (flag2)
			{
				GameStatsManager.Instance.SingleIncrementDifferentiator(sic.item.encounterTrackable);
			}
			sic.CurrentPrice = sic.item.PurchasePrice;
			bool flag3 = baseShopController != null && baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY;
			if (flag3)
			{
				sic.CurrentPrice = 1;
				bool flag4 = sic.item.quality == PickupObject.ItemQuality.A;
				if (flag4)
				{
					sic.CurrentPrice = 2;
				}
				bool flag5 = sic.item.quality == PickupObject.ItemQuality.S;
				if (flag5)
				{
					sic.CurrentPrice = 3;
				}
			}
			bool flag6 = baseShopController != null && baseShopController.baseShopType == BaseShopController.AdditionalShopType.NONE && (sic.item is BankMaskItem || sic.item is BankBagItem || sic.item is PaydayDrillItem);
			if (flag6)
			{
				EncounterTrackable encounterTrackable = sic.item.encounterTrackable;
				bool flag7 = encounterTrackable && !encounterTrackable.PrerequisitesMet();
				if (flag7)
				{
					bool flag8 = sic.item is BankMaskItem;
					if (flag8)
					{
						sic.SetsFlagOnSteal = true;
						sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK;
					}
					else
					{
						bool flag9 = sic.item is BankBagItem;
						if (flag9)
						{
							sic.SetsFlagOnSteal = true;
							sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG;
						}
						else
						{
							bool flag10 = sic.item is PaydayDrillItem;
							if (flag10)
							{
								sic.SetsFlagOnSteal = true;
								sic.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_DRILL;
							}
						}
					}
					sic.OverridePrice = new int?(9999);
				}
			}
			sic.gameObject.GetOrAddComponent<tk2dSprite>();
			tk2dSprite tk2dSprite = i.GetComponent<tk2dSprite>();
			bool flag11 = tk2dSprite == null;
			if (flag11)
			{
				tk2dSprite = i.GetComponentInChildren<tk2dSprite>();
			}
			sic.sprite.SetSprite(tk2dSprite.Collection, tk2dSprite.spriteId);
			sic.sprite.IsPerpendicular = true;
			bool useOmnidirectionalItemFacing = sic.UseOmnidirectionalItemFacing;
			if (useOmnidirectionalItemFacing)
			{
				sic.sprite.IsPerpendicular = false;
			}
			sic.sprite.HeightOffGround = 1f;
			bool flag12 = shopController != null;
			if (flag12)
			{
				bool flag13 = shopController is MetaShopController;
				if (flag13)
				{
					sic.UseOmnidirectionalItemFacing = true;
					sic.sprite.IsPerpendicular = false;
				}
				sic.sprite.HeightOffGround += shopController.ItemHeightOffGroundModifier;
			}
			else
			{
				bool flag14 = baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLACKSMITH;
				if (flag14)
				{
					sic.UseOmnidirectionalItemFacing = true;
				}
				else
				{
					bool flag15 = baseShopController.baseShopType == BaseShopController.AdditionalShopType.TRUCK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.GOOP || baseShopController.baseShopType == BaseShopController.AdditionalShopType.CURSE || baseShopController.baseShopType == BaseShopController.AdditionalShopType.BLANK || baseShopController.baseShopType == BaseShopController.AdditionalShopType.KEY || baseShopController.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT;
					if (flag15)
					{
						sic.UseOmnidirectionalItemFacing = true;
					}
				}
			}
			sic.sprite.PlaceAtPositionByAnchor(sic.transform.parent.position, tk2dBaseSprite.Anchor.MiddleCenter);
			sic.sprite.transform.position = sic.sprite.transform.position.Quantize(0.0625f);
			DepthLookupManager.ProcessRenderer(sic.sprite.renderer);
			tk2dSprite componentInParent = sic.transform.parent.gameObject.GetComponentInParent<tk2dSprite>();
			bool flag16 = componentInParent != null;
			if (flag16)
			{
				componentInParent.AttachRenderer(sic.sprite);
			}
			SpriteOutlineManager.AddOutlineToSprite(sic.sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
			GameObject gameObject = null;
			bool flag17 = shopController != null && shopController.shopItemShadowPrefab != null;
			if (flag17)
			{
				gameObject = shopController.shopItemShadowPrefab;
			}
			bool flag18 = baseShopController != null && baseShopController.shopItemShadowPrefab != null;
			if (flag18)
			{
				gameObject = baseShopController.shopItemShadowPrefab;
			}
			bool flag19 = gameObject != null;
			if (flag19)
			{
				GameObject gameObject2 = ReflectionHelpers.ReflectGetField<GameObject>(typeof(ShopItemController), "m_shadowObject", sic);
				bool flag20 = !gameObject2;
				if (flag20)
				{
					gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					ReflectionHelpers.ReflectSetField<GameObject>(typeof(ShopItemController), "m_shadowObject", gameObject2, sic);
				}
				tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
				component.PlaceAtPositionByAnchor(sic.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				component.transform.position = component.transform.position.Quantize(0.0625f);
				sic.sprite.AttachRenderer(component);
				component.transform.parent = sic.sprite.transform;
				component.HeightOffGround = -0.5f;
				bool flag21 = shopController is MetaShopController;
				if (flag21)
				{
					component.HeightOffGround = -0.0625f;
				}
			}
			sic.sprite.UpdateZDepth();
			SpeculativeRigidbody orAddComponent = sic.gameObject.GetOrAddComponent<SpeculativeRigidbody>();
			orAddComponent.PixelColliders = new List<PixelCollider>();
			PixelCollider pixelCollider = new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle,
				CollisionLayer = CollisionLayer.HighObstacle,
				ManualDiameter = 14
			};
			Vector2 vector = sic.sprite.WorldCenter - sic.transform.position.XY();
			pixelCollider.ManualOffsetX = PhysicsEngine.UnitToPixel(vector.x) - 7;
			pixelCollider.ManualOffsetY = PhysicsEngine.UnitToPixel(vector.y) - 7;
			orAddComponent.PixelColliders.Add(pixelCollider);
			orAddComponent.Initialize();
			orAddComponent.OnPreRigidbodyCollision = null;
			sic.RegenerateCache();
			bool flag22 = !GameManager.Instance.IsFoyer && sic.item is Gun && GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns;
			if (flag22)
			{
				sic.ForceOutOfStock();
			}
		}*/


		public static readonly Dictionary<TextAnchor, Vector2> AnchorMap = new Dictionary<TextAnchor, Vector2>
		{
			{
				TextAnchor.LowerLeft,
				new Vector2(0f, 0f)
			},
			{
				TextAnchor.LowerCenter,
				new Vector2(0.5f, 0f)
			},
			{
				TextAnchor.LowerRight,
				new Vector2(1f, 0f)
			},
			{
				TextAnchor.MiddleLeft,
				new Vector2(0f, 0.5f)
			},
			{
				TextAnchor.MiddleCenter,
				new Vector2(0.5f, 0.5f)
			},
			{
				TextAnchor.MiddleRight,
				new Vector2(1f, 0.5f)
			},
			{
				TextAnchor.UpperLeft,
				new Vector2(0f, 1f)
			},
			{
				TextAnchor.UpperCenter,
				new Vector2(0.5f, 1f)
			},
			{
				TextAnchor.UpperRight,
				new Vector2(1f, 1f)
			}
		};

		public static void LogPropertiesAndFields<T>(T obj, string header = "")
		{
			BotsModule.Log(header);
			BotsModule.Log("=======================");
			bool flag = obj == null;
			if (flag)
			{
				BotsModule.Log("LogPropertiesAndFields: Null object");
			}
			else
			{
				Type type = obj.GetType();
				BotsModule.Log(string.Format("Type: {0}", type));
				PropertyInfo[] properties = type.GetProperties();
				BotsModule.Log(string.Format("{0} Properties: ", typeof(T)));
				foreach (PropertyInfo propertyInfo in properties)
				{
					try
					{
						object value = propertyInfo.GetValue(obj, null);
						string text = value.ToString();
						bool flag2 = ((obj != null) ? obj.GetType().GetGenericTypeDefinition() : null) == typeof(List<>);
						bool flag3 = flag2;
						if (flag3)
						{
							List<object> list = value as List<object>;
							text = string.Format("List[{0}]", list.Count);
							foreach (object obj2 in list)
							{
								text = text + "\n\t\t" + obj2.ToString();
							}
						}
						BotsModule.Log("\t" + propertyInfo.Name + ": " + text);
					}
					catch
					{
					}
				}
				BotsModule.Log(string.Format("{0} Fields: ", typeof(T)));
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					BotsModule.Log(string.Format("\t{0}: {1}", fieldInfo.Name, fieldInfo.GetValue(obj)));
				}
			}
		}


		public static Texture2D SpriteToTexture(tk2dSprite sourceSprite)
		{

			Texture2D sourceTexture = (Texture2D)sourceSprite.GetCurrentSpriteDef().material.mainTexture;

			Vector2[] UVs = sourceSprite.GetCurrentSpriteDef().uvs;

			// Get the raw uv-pixel co-ordinates of the image.
			float fX = (sourceTexture.width) * UVs[0].x;
			float fY = (sourceTexture.height) * UVs[2].y;
			float fX2 = (sourceTexture.width) * UVs[1].x; ;
			float fY2 = (sourceTexture.height) * UVs[0].y;

			// Calculate the width and height of the sprite.
			float fWidth = fX2 - fX;
			float fHeight = -(fY2 - fY);

			// Round the values to pixel units.
			int x = Mathf.RoundToInt(fX);
			int y = Mathf.RoundToInt(fY - fHeight);
			int width = Mathf.RoundToInt(fWidth);
			int height = Mathf.RoundToInt(fHeight);

			// Get the pixels contained within the pixel units of the atlas.
			Color[] pixelRect = sourceTexture.GetPixels(x, y, width, height);

			// Generate a new texture and populate it with the pixel data.
			Texture2D newTexture = new Texture2D(width, height);
			newTexture.SetPixels(pixelRect);
			newTexture.Apply();

			return newTexture;
		}


		public static GameObject ProcessGameObject(this GameObject obj)
		{
			obj.SetActive(false);
			FakePrefab.MarkAsFakePrefab(obj);
			UnityEngine.Object.DontDestroyOnLoad(obj);
			return obj;
		}

		public static void AddItemToSynergy(this PickupObject obj, string nameKey, bool clearMandatory = false)
		{
			AddItemToSynergy(nameKey, obj.PickupObjectId, clearMandatory);
		}

		public static void AddItemToSynergy(string nameKey, int id, bool clearMandatory = false)
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
								if (entry.MandatoryGunIDs != null && clearMandatory)
								{
									foreach (var mId in entry.MandatoryGunIDs)
									{
										entry.OptionalItemIDs.Add(mId);
									}
									entry.MandatoryItemIDs.Clear();
								}
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
								if (entry.MandatoryItemIDs != null && clearMandatory)
								{
									foreach(var mId in entry.MandatoryItemIDs)
									{
										entry.OptionalItemIDs.Add(mId);
									}
									entry.MandatoryItemIDs.Clear();
								}
								
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

		public static void AddItemToPool(this GenericLootTable lootTable, PickupObject po, float weight = 1)
		{
			lootTable.defaultItemDrops.Add(new WeightedGameObject()
			{
				pickupId = po.PickupObjectId,
				weight = weight,
				rawGameObject = po.gameObject,
				forceDuplicatesPossible = false,
				additionalPrerequisites = new DungeonPrerequisite[0]
			});
		}

		public static void AddItemToPool(this GenericLootTable lootTable, int poID, float weight = 1)
		{

			var po = PickupObjectDatabase.GetById(poID);
			lootTable.defaultItemDrops.Add(new WeightedGameObject()
			{
				pickupId = po.PickupObjectId,
				weight = weight,
				rawGameObject = po.gameObject,
				forceDuplicatesPossible = false,
				additionalPrerequisites = new DungeonPrerequisite[0]
			});
		}

		/// <summary>
		/// Adds a trail to a GameObject
		/// </summary>
		/// <param name="obj">Object the trail will be added to</param> 
		/// <param name="color">Color of the trail</param>
		/// <param name="texture">Texture the trail will use</param>
		/// <param name="time">The time (in seconds) that the trail will last</param>
		/// <param name="minVertexDistance">"Set the minimum distance the trail can travel before a new vertex is added to it. Smaller values with give smoother trails, consisting of more vertices, but costing more performance." - unity docs</param>
		/// <param name="startWidth">Width at the start of the trail</param>
		/// <param name="endWidth">Width at the end of the trail</param>
		/// <param name="startColor">Color at the start of the trail</param>
		/// <param name="endColor">Color at the end of the trail</param>
		/// <returns></returns>
		public static void AddTrailToObject(this GameObject obj, Color color, Texture texture, float time, float minVertexDistance, float startWidth, float endWidth, Color startColor, Color endColor)
		{
			var tr = obj.AddComponent<TrailRenderer>();
			tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			tr.receiveShadows = false;
			var mat = new Material(Shader.Find("Sprites/Default"));
			mat.mainTexture = texture;
			mat.SetColor("_Color", color);
			tr.material = mat;
			tr.time = time;
			tr.minVertexDistance = minVertexDistance;
			tr.startWidth = startWidth;
			tr.endWidth = endWidth;
			tr.startColor = startColor;
			tr.endColor = endColor;
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

		public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
		{
			FieldInfo field = classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static));
			return (T)field.GetValue(o);
		}

		public static tk2dSpriteDefinition CopyDefinitionFrom(this tk2dSpriteDefinition other)
		{
			tk2dSpriteDefinition result = new tk2dSpriteDefinition
			{
				boundsDataCenter = new Vector3
				{
					x = other.boundsDataCenter.x,
					y = other.boundsDataCenter.y,
					z = other.boundsDataCenter.z
				},
				boundsDataExtents = new Vector3
				{
					x = other.boundsDataExtents.x,
					y = other.boundsDataExtents.y,
					z = other.boundsDataExtents.z
				},
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
				position0 = new Vector3
				{
					x = other.position0.x,
					y = other.position0.y,
					z = other.position0.z
				},
				position1 = new Vector3
				{
					x = other.position1.x,
					y = other.position1.y,
					z = other.position1.z
				},
				position2 = new Vector3
				{
					x = other.position2.x,
					y = other.position2.y,
					z = other.position2.z
				},
				position3 = new Vector3
				{
					x = other.position3.x,
					y = other.position3.y,
					z = other.position3.z
				},
				regionH = other.regionH,
				regionW = other.regionW,
				regionX = other.regionX,
				regionY = other.regionY,
				tangents = other.tangents,
				texelSize = new Vector2
				{
					x = other.texelSize.x,
					y = other.texelSize.y
				},
				untrimmedBoundsDataCenter = new Vector3
				{
					x = other.untrimmedBoundsDataCenter.x,
					y = other.untrimmedBoundsDataCenter.y,
					z = other.untrimmedBoundsDataCenter.z
				},
				untrimmedBoundsDataExtents = new Vector3
				{
					x = other.untrimmedBoundsDataExtents.x,
					y = other.untrimmedBoundsDataExtents.y,
					z = other.untrimmedBoundsDataExtents.z
				}
			};
			List<Vector2> uvs = new List<Vector2>();
			foreach (Vector2 vector in other.uvs)
			{
				uvs.Add(new Vector2
				{
					x = vector.x,
					y = vector.y
				});
			}
			result.uvs = uvs.ToArray();
			List<Vector3> colliderVertices = new List<Vector3>();
			foreach (Vector3 vector in other.colliderVertices)
			{
				colliderVertices.Add(new Vector3
				{
					x = vector.x,
					y = vector.y,
					z = vector.z
				});
			}
			result.colliderVertices = colliderVertices.ToArray();
			return result;
		}


		public static Gun GetGunById(this PickupObjectDatabase database, int id)
		{
			return PickupObjectDatabase.GetById(id) as Gun;
		}

		
		public static Gun GetGunById(int id)
		{
			return Tools.GetGunById((PickupObjectDatabase)null, id);
		}

		
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

		public static tk2dSpriteDefinition SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, bool lightened = true, tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.LowerLeft, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null, 
			bool anchorChangesCollider = true, bool fixesScale = false, int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
		{
			try
			{
				proj.GetAnySprite().Collection = ETGMod.Databases.Items.ProjectileCollection.inst;
				proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);

				tk2dSpriteDefinition def = SetupDefinitionForProjectileSprite(name, proj.GetAnySprite().spriteId, pixelWidth, pixelHeight, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX,
					overrideColliderOffsetY, overrideProjectileToCopyFrom);
				def.ConstructOffsetsFromAnchor(anchor, def.position3, fixesScale, anchorChangesCollider);
				proj.GetAnySprite().scale = new Vector3(1f, 1f, 1f);
				proj.transform.localScale = new Vector3(1f, 1f, 1f);
				proj.GetAnySprite().transform.localScale = new Vector3(1f, 1f, 1f);
				proj.AdditionalScaleMultiplier = 1f;
				return def;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:");
				ETGModConsole.Log(ex.ToString());
				return null;
			}
		}

		public static void SetProjectileSpriteRight2(this Projectile proj, string name, int pixelWidth, int pixelHeight, bool lightened = true, tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.LowerLeft, bool anchorChangesCollider = true, int? overrideColliderPixelWidth = null,
int? overrideColliderPixelHeight = null, int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
		{
			try
			{
				proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
				var sprite = proj.GetAnySprite();
				BotsModule.Log($"[{sprite.spriteId}]: {sprite.Collection.spriteDefinitions[sprite.spriteId].name}");

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

		
		public static GameActorCharmEffect CopyCharmFrom(this GameActorCharmEffect self, GameActorCharmEffect other)
		{
			bool flag = self == null;
			if (flag)
			{
				self = new GameActorCharmEffect();
			}
			return (GameActorCharmEffect)self.CopyEffectFrom(other);
		}

		
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

		public static void IgnoreCollisionsFor(this Projectile proj, float time)
		{
			proj.specRigidbody.CollideWithOthers = false;
			GameManager.Instance.StartCoroutine(collisionDelay(proj, time));
		}

		static IEnumerator collisionDelay(Projectile proj, float time)
        {
			yield return new WaitForSeconds(time);
			proj.specRigidbody.CollideWithOthers = true;
		}

		public static List<AIBeamShooter2> ExtremeLaziness(GameObject prefab, Transform parent, int amountOfBeams, List<string> names)
        {
			var beamList = new List<AIBeamShooter2>();
			for(int i = 0; i < amountOfBeams; i++)
            {
				/*AIBeamShooter2 aIBeamShooter = prefab.GetOrAddComponent<AIBeamShooter2>();
				AIActor actor = EnemyDatabase.GetOrLoadByGuid("21dd14e5ca2a4a388adab5b11b69a1e1");
				AIBeamShooter aIBeamShooter2 = actor.GetComponent<AIBeamShooter>();
				aIBeamShooter.beamTransform = parent;
				aIBeamShooter.beamModule = aIBeamShooter2.beamModule;
				aIBeamShooter.beamProjectile = aIBeamShooter2.beamProjectile;
				aIBeamShooter.name = names[i];
				aIBeamShooter.PreventBeamContinuation = true;*/


				AIBeamShooter2 bholsterbeam1 = prefab.AddComponent<AIBeamShooter2>();
				AIActor actor2 = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b");
				BeholsterController beholsterbeam = actor2.GetComponent<BeholsterController>();
				bholsterbeam1.beamTransform = parent;
				bholsterbeam1.beamModule = beholsterbeam.beamModule;
				bholsterbeam1.sprite.HeightOffGround = -20;
				//bholsterbeam1.beamProjectile = beholsterbeam.beamModule.GetCurrentProjectile();
				bholsterbeam1.beamProjectile = beholsterbeam.projectile;

				beamList.Add(bholsterbeam1);

			}
			

			return beamList;
        }

		
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
		public static AssetBundle brave;

		public static void AddComplex(this StringDBTable stringdb, string key, string value)
		{
			StringTableManager.ComplexStringCollection stringCollection = new StringTableManager.ComplexStringCollection();
			stringCollection.AddString(value, 1f);
			stringdb[key] = stringCollection;
		}

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


		public static DungeonFlowNode GenerateDefaultNode(this DungeonFlow targetflow, PrototypeDungeonRoom.RoomCategory roomType, PrototypeDungeonRoom overrideRoom = null, GenericRoomTable overrideTable = null, bool oneWayLoopTarget = false, bool isWarpWingNode = false, string nodeGUID = null, DungeonFlowNode.NodePriority priority = DungeonFlowNode.NodePriority.MANDATORY, float percentChance = 1, bool handlesOwnWarping = true)
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
				//return null;
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
