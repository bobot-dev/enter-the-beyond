using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class StairWayWarpComp : BraveBehaviour, IPlayerInteractable
	{

		RoomHandler curRoom;
		// Token: 0x060068CF RID: 26831 RVA: 0x0003E8FF File Offset: 0x0003CAFF
		private void Start()
		{
			foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
			{
				if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.ENTRANCE)
				{
					curRoom = roomHandler2;
					break;
				}
			}

			//curRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			if (curRoom == null) ETGModConsole.Log("FUUUCK");
			curRoom.RegisterInteractable(this);

			
			curRoom.Exited += StairWayWarpComp_Exited;

		}

        private void StairWayWarpComp_Exited()
        {
			UnityEngine.Object.Destroy(this.gameObject);
		}

        protected override void OnDestroy()
        {
			curRoom.Exited -= StairWayWarpComp_Exited;
			base.OnDestroy();
        }

        // Token: 0x060068D0 RID: 26832 RVA: 0x0003E92C File Offset: 0x0003CB2C
        public float GetDistanceToPoint(Vector2 point)
		{
			return Vector2.Distance(point, base.sprite.WorldBottomCenter);
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x0003E93F File Offset: 0x0003CB3F
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
			this.m_justWarped = false;
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x0003E963 File Offset: 0x0003CB63
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x002A0870 File Offset: 0x0029EA70
		public void Interact(PlayerController interactor)
		{
			if (this.m_justWarped)
			{
				return;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleWarpCooldown(interactor));
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x002A08C0 File Offset: 0x0029EAC0
		private IEnumerator HandleWarpCooldown(PlayerController player)
		{
			this.m_justWarped = true;
			Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
			yield return new WaitForSeconds(0.1f);
			player.SetInputOverride("arbitrary warp");
			RoomHandler roomHandler = null;

			foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
			{
				if (roomHandler2.area.PrototypeRoomName == targetRoomName)
				{
					roomHandler = roomHandler2;
					break;
				}
			}

			roomHandler.ForcePitfallForFliers = true;
			roomHandler.TargetPitfallRoom = curRoom;

			Vector2 targetPoint2 = roomHandler.area.basePosition.ToVector2() + new Vector2((float)roomHandler.area.dimensions.x / 2f, 8f);
			player.WarpToPoint(targetPoint2, false, false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
				if (otherPlayer && otherPlayer.healthHaver.IsAlive)
				{
					otherPlayer.ReuniteWithOtherPlayer(player, false);
				}
			}
			GameManager.Instance.MainCameraController.ForceToPlayerPosition(player);
			Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
			player.ClearInputOverride("arbitrary warp");
			yield return new WaitForSeconds(1f);
			this.m_justWarped = false;
			
			yield break;
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x00032CEF File Offset: 0x00030EEF
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x00005FAD File Offset: 0x000041AD
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		// Token: 0x04006513 RID: 25875

		public string targetRoomName;

		private bool m_justWarped;
	}

}
