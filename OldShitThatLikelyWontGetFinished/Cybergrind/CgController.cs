using Dungeonator;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CgController
    {
		public static float aaaa;
		public float DelayPreExpansion;
		public float DelayPostExpansionPreEnemies;
		private List<string> c_rewardRoomObjects = new List<string>();

		public IEnumerator HandleSeamlessTransitionToCombatRoom(RoomHandler sourceRoom, PrototypeDungeonRoom targetRoom)
		{
			yield return new WaitForSeconds(5);
			Dungeon d = GameManager.Instance.Dungeon;

			int tmapExpansion = 13;
			RoomHandler newRoom = d.RuntimeDuplicateChunk(sourceRoom.area.basePosition, sourceRoom.area.dimensions, tmapExpansion, sourceRoom, true);
			//RoomHandler newRoom = d.AddRuntimeRoom(targetRoom, null, DungeonData.LightGenerationStyle.FORCE_COLOR);
			newRoom.ClearReinforcementLayers();
			//newRoom.CompletelyPreventLeaving = true;
			List<Transform> movedObjects = new List<Transform>();


			foreach(Transform transform in sourceRoom.hierarchyParent)
            {
				if (transform)
				{
					movedObjects.Add(transform);
					this.MoveObjectBetweenRooms(transform, sourceRoom, newRoom);
				}
			}
			/*for (int i = 0; i < this.c_rewardRoomObjects.Count; i++)
			{
				Transform transform = sourceRoom.hierarchyParent.Find(this.c_rewardRoomObjects[i]);
				if (transform)
				{
					movedObjects.Add(transform);
					this.MoveObjectBetweenRooms(transform, sourceRoom, newRoom);
				}
			}*/
			//this.MoveObjectBetweenRooms(sourceChest.transform, sourceRoom, newRoom);



			Vector2 oldPlayerPosition = GameManager.Instance.BestActivePlayer.transform.position.XY();
			Vector2 playerOffset = oldPlayerPosition - sourceRoom.area.basePosition.ToVector2();
			Vector2 newPlayerPosition = newRoom.area.basePosition.ToVector2() + playerOffset;
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			Pathfinder.Instance.InitializeRegion(d.data, newRoom.area.basePosition, newRoom.area.dimensions);
			GameManager.Instance.BestActivePlayer.WarpToPoint(newPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}
			yield return null;
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				GameManager.Instance.AllPlayers[j].WarpFollowersToPlayer(false);
				GameManager.Instance.AllPlayers[j].WarpCompanionsToPlayer(false);
			}
			BotsModule.Log("HandleCombatRoomExpansion");
			yield return d.StartCoroutine(this.HandleCombatRoomExpansion(sourceRoom, newRoom));

			bool goodToGo = false;
			while (!goodToGo)
			{
				goodToGo = true;
				for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
				{
					//float num = Vector2.Distance(sourceChest.specRigidbody.UnitCenter, GameManager.Instance.AllPlayers[j].CenterPosition);
					if (aaaa > 3f)
					{
						goodToGo = false;
					}
				}
				yield return null;
			}

			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.BestActivePlayer.CenterPosition;



			for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
			{
				GameManager.Instance.AllPlayers[l].SetInputOverride("shrinkage");
			}
			BotsModule.Log("HandleCombatRoomShrinking");
			yield return d.StartCoroutine(this.HandleCombatRoomShrinking(newRoom));
			for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
			{
				GameManager.Instance.AllPlayers[m].ClearInputOverride("shrinkage");
			}
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			AkSoundEngine.PostEvent("Play_OBJ_paydaydrill_end_01", GameManager.Instance.gameObject);
			GameManager.Instance.MainCameraController.SetManualControl(false, false);
			GameManager.Instance.BestActivePlayer.WarpToPoint(oldPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			Pathfinder.Instance.InitializeRegion(d.data, newRoom.area.basePosition, newRoom.area.dimensions);
			GameManager.Instance.BestActivePlayer.WarpToPoint(newPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}
			yield return null;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].WarpFollowersToPlayer(false);
				GameManager.Instance.AllPlayers[i].WarpCompanionsToPlayer(false);
			}
			yield return new WaitForSeconds(this.DelayPostExpansionPreEnemies);
			BotsModule.Log("HandleCombatWaves");
			yield return d.StartCoroutine(this.HandleCombatWaves(d, newRoom, 1));

			AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", GameManager.Instance.gameObject);
			
			
			Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
			GameManager.Instance.BestActivePlayer.WarpToPoint(oldPlayerPosition, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
			}
			


			yield break;
		}

		protected IEnumerator HandleCombatWaves(Dungeon d, RoomHandler newRoom, int waveCount)
		{
			DrillWaveDefinition wavesToUse = new DrillWaveDefinition
			{
				MaxEnemies = (3 * waveCount) + 3,
				MinEnemies = (3 * waveCount)
			};
			int numEnemiesToSpawn = UnityEngine.Random.Range(wavesToUse.MinEnemies, wavesToUse.MaxEnemies + 1);
			for (int i = 0; i < numEnemiesToSpawn; i++)
			{
				newRoom.AddSpecificEnemyToRoomProcedurally(d.GetWeightedProceduralEnemy().enemyGuid, true, null);
			}
			yield return new WaitForSeconds(3f);
			while (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) > 0)
			{
				yield return new WaitForSeconds(1f);
			}
			if (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 0)
			{
				List<AIActor> activeEnemies = newRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				for (int j = 0; j < activeEnemies.Count; j++)
				{
					if (activeEnemies[j].IsNormalEnemy)
					{
						activeEnemies[j].EraseFromExistence(false);
					}
				}
			}
			yield break;
		}

		private IEnumerator HandleCombatRoomShrinking(RoomHandler targetRoom)
		{
			float elapsed = 5.5f;
			int numExpansionsDone = 6;
			while (elapsed > 0f)
			{
				elapsed -= BraveTime.DeltaTime * 9f;
				while (elapsed < (float)numExpansionsDone && numExpansionsDone > 0)
				{
					numExpansionsDone--;
					this.ShrinkRoom(targetRoom);
				}
				yield return null;
			}
			yield break;
		}

		private void MoveObjectBetweenRooms(Transform foundObject, RoomHandler fromRoom, RoomHandler toRoom)
		{
			Vector2 b = foundObject.position.XY() - fromRoom.area.basePosition.ToVector2();
			Vector2 v = toRoom.area.basePosition.ToVector2() + b;
			foundObject.transform.position = v;
			if (foundObject.parent == fromRoom.hierarchyParent)
			{
				foundObject.parent = toRoom.hierarchyParent;
			}
			SpeculativeRigidbody component = foundObject.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				component.Reinitialize();
			}
			tk2dBaseSprite component2 = foundObject.GetComponent<tk2dBaseSprite>();
			if (component2)
			{
				component2.UpdateZDepth();
			}
		}


		private IEnumerator HandleCombatRoomExpansion(RoomHandler sourceRoom, RoomHandler targetRoom)
		{
			yield return new WaitForSeconds(this.DelayPreExpansion);
			float duration = 5.5f;
			float elapsed = 0f;
			int numExpansionsDone = 0;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime * 9f;
				while (elapsed > (float)numExpansionsDone)
				{
					numExpansionsDone++;
					this.ExpandRoom(targetRoom);
					AkSoundEngine.PostEvent("Play_OBJ_rock_break_01", GameManager.Instance.gameObject);
				}
				yield return null;
			}
			Dungeon d = GameManager.Instance.Dungeon;
			Pathfinder.Instance.InitializeRegion(d.data, targetRoom.area.basePosition + new IntVector2(-5, -5), targetRoom.area.dimensions + new IntVector2(10, 10));

			yield break;
		}

		private void ShrinkRoom(RoomHandler r)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
			tk2dTileMap tk2dTileMap = null;
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int i = -5; i < r.area.dimensions.x + 5; i++)
			{
				for (int j = -5; j < r.area.dimensions.y + 5; j++)
				{
					IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
					CellData cellData = (!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector];
					if (cellData != null && cellData.type != CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.WALL))
					{
						hashSet.Add(cellData.position);
					}
				}
			}
			foreach (IntVector2 key in hashSet)
			{
				CellData cellData2 = dungeon.data[key];
				cellData2.breakable = true;
				cellData2.occlusionData.overrideOcclusion = true;
				cellData2.occlusionData.cellOcclusionDirty = true;
				tk2dTileMap = dungeon.ConstructWallAtPosition(key.x, key.y, true);
				r.Cells.Remove(cellData2.position);
				r.CellsWithoutExits.Remove(cellData2.position);
				r.RawCells.Remove(cellData2.position);
			}
			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
			if (tk2dTileMap)
			{
				dungeon.RebuildTilemap(tk2dTileMap);
			}
		}

		private void ExpandRoom(RoomHandler r)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
			tk2dTileMap tk2dTileMap = null;
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int i = -5; i < r.area.dimensions.x + 5; i++)
			{
				for (int j = -5; j < r.area.dimensions.y + 5; j++)
				{
					IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
					CellData cellData = (!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector];
					if (cellData != null && cellData.type == CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.FLOOR))
					{
						hashSet.Add(cellData.position);
					}
				}
			}
			foreach (IntVector2 key in hashSet)
			{
				CellData cellData2 = dungeon.data[key];
				cellData2.breakable = true;
				cellData2.occlusionData.overrideOcclusion = true;
				cellData2.occlusionData.cellOcclusionDirty = true;
				
				tk2dTileMap = dungeon.DestroyWallAtPosition(key.x, key.y, true);
				if (UnityEngine.Random.value < 0.25f)
				{
					//this.VFXDustPoof.SpawnAtPosition(key.ToCenterVector3((float)key.y), 0f, null, null, null, null, false, null, null, false);
				}
				r.Cells.Add(cellData2.position);
				r.CellsWithoutExits.Add(cellData2.position);
				r.RawCells.Add(cellData2.position);
			}
			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
			if (tk2dTileMap)
			{
				dungeon.RebuildTilemap(tk2dTileMap);
			}
		}

	}
}
