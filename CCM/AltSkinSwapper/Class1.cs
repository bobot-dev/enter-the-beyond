using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

using ItemAPI;

namespace BotsMod
{
	public class LostCharacterCostumeSwapper2 : MonoBehaviour, IPlayerInteractable
	{


		static public PlayableCharacters TargetCharacter = PlayableCharacters.Guide;

		public tk2dSprite CostumeSprite;

		// Token: 0x04005B72 RID: 23410
		public tk2dSprite AlternateCostumeSprite;

		// Token: 0x04005B73 RID: 23411
		public tk2dSpriteAnimation TargetLibrary;

		// Token: 0x04005B74 RID: 23412
		static public bool HasCustomTrigger = false;

		// Token: 0x04005B75 RID: 23413
		static public bool CustomTriggerIsFlag = false;

		// Token: 0x04005B76 RID: 23414
		static public GungeonFlags TriggerFlag = 0;

		// Token: 0x04005B77 RID: 23415
		static public bool CustomTriggerIsSpecialReserve = false;

		// Token: 0x04005B78 RID: 23416
		static private bool m_active = true;


		// Token: 0x060060D5 RID: 24789 RVA: 0x00247F40 File Offset: 0x00246140
		public void Start()
		{
			try
			{
				if (AlternateCostumeSprite == null)
				{
					BotsModule.Log("AlternateCostumeSprite (help ;-;)", "#eb1313");
				}

				if (CostumeSprite == null)
				{
					BotsModule.Log("CostumeSprite (help ;-;)", "#eb1313");
				}

				bool flag = GameStatsManager.Instance.GetCharacterSpecificFlag(TargetCharacter, CharacterSpecificGungeonFlags.KILLED_PAST);

				if (flag)
				{
					m_active = true;
					AlternateCostumeSprite.renderer.enabled = true;
					CostumeSprite.renderer.enabled = false;
					//LostItemsMod.Log("skin thing worked", LostItemsMod.TEXT_COLOR_GOOD);
				}
				else
				{
					m_active = false;
					AlternateCostumeSprite.renderer.enabled = false;
					CostumeSprite.renderer.enabled = false;
					//LostItemsMod.Log("skin thing not worked", LostItemsMod.TEXT_COLOR_BAD);
				}
				m_active = true;
			}
			catch (Exception e)
			{
				BotsModule.Log("shit shit shit shiiiiiiiiiiiiiiiiiiiiit (help ;-;)", "#eb1313");
				BotsModule.Log(string.Format(e + ""), "#eb1313");
			}


		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x00248058 File Offset: 0x00246258
		private void Update()
		{

			if (m_active)
			{


				if (GameManager.IsReturningToBreach)
				{
					return;
				}
				if (GameManager.Instance.IsSelectingCharacter)
				{
					return;
				}
				if (GameManager.Instance.IsLoadingLevel)
				{
					return;
				}
				if (GameManager.Instance.PrimaryPlayer == null)
				{
					return;
				}
				if (TargetCharacter != PlayableCharacters.CoopCultist && GameManager.Instance.PrimaryPlayer.name != "PlayerLost(Clone)")
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(AlternateCostumeSprite, false);
					SpriteOutlineManager.RemoveOutlineFromSprite(CostumeSprite, false);
					AlternateCostumeSprite.renderer.enabled = true;
					CostumeSprite.renderer.enabled = false;
				}


			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x00248114 File Offset: 0x00246314
		/*public float GetDistanceToPoint(Vector2 point)
		{

			LostItemsMod.Log("x " + point.x , LostItemsMod.TEXT_COLOR_GOOD);
			LostItemsMod.Log("y " + point.y , LostItemsMod.TEXT_COLOR_GOOD);
			if (!m_active)
			{
				
				return 1000f;
			}
			if (AlternateCostumeSprite.renderer.enabled)
			{
				LostItemsMod.Log("Distance " + Vector2.Distance(point, AlternateCostumeSprite.WorldCenter), LostItemsMod.TEXT_COLOR_GOOD);
				return Vector2.Distance(point, AlternateCostumeSprite.WorldCenter);
			}
			LostItemsMod.Log("Distance " + Vector2.Distance(point, CostumeSprite.WorldCenter), LostItemsMod.TEXT_COLOR_BAD);
			return Vector2.Distance(point, CostumeSprite.WorldCenter);
			
		}*/
		public float GetDistanceToPoint(Vector2 point)
		{

			float result;
			if (!m_active)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, this.GetComponent<tk2dSprite>().specRigidbody.UnitBottomLeft, this.GetComponent<tk2dSprite>().specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x0024816C File Offset: 0x0024636C
		public void OnEnteredRange(PlayerController interactor)
		{

			if (interactor.name != "PlayerLost(Clone)")
			{
				return;
			}

			BotsModule.Log("hi", BotsModule.TEXT_COLOR);

			if (AlternateCostumeSprite.renderer.enabled)
			{
				SpriteOutlineManager.AddOutlineToSprite(AlternateCostumeSprite, Color.white);
			}
			else
			{
				SpriteOutlineManager.AddOutlineToSprite(CostumeSprite, Color.white);
			}


		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x002481C8 File Offset: 0x002463C8
		public void OnExitRange(PlayerController interactor)
		{
			BotsModule.Log("bye", BotsModule.TEXT_COLOR);
			if (interactor.name != "PlayerLost(Clone)")
			{
				return;
			}

			//LostItemsMod.Log("bye", LostItemsMod.TEXT_COLOR_GOOD);

			if (AlternateCostumeSprite.renderer.enabled)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(AlternateCostumeSprite, false);
			}
			else
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(CostumeSprite, false);
			}
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x0024821C File Offset: 0x0024641C
		public void Interact(PlayerController interactor)
		{
			BotsModule.Log("yay", BotsModule.TEXT_COLOR);
			if (interactor.name != "PlayerLost(Clone)")
			{
				return;
			}
			if (interactor.IsUsingAlternateCostume)
			{
				interactor.SwapToAlternateCostume(null);
			}
			else
			{
				if (TargetLibrary)
				{
					interactor.AlternateCostumeLibrary = TargetLibrary;
				}
				interactor.SwapToAlternateCostume(null);
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(AlternateCostumeSprite, false);
			SpriteOutlineManager.RemoveOutlineFromSprite(CostumeSprite, false);
			AlternateCostumeSprite.renderer.enabled = !AlternateCostumeSprite.renderer.enabled;
			CostumeSprite.renderer.enabled = !CostumeSprite.renderer.enabled;
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x001A24CF File Offset: 0x001A06CF
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x0002A087 File Offset: 0x00028287
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}


	}
}

