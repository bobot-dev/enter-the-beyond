using System;
using System.Collections;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000D RID: 13
	public class SimpleShrine : SimpleInteractable, IPlayerInteractable
	{
		
		private void Start()
		{
			this.talkPoint = base.transform.Find("talkpoint");
			this.m_isToggled = false;
		}

		
		public void Interact(PlayerController interactor)
		{
			bool flag = TextBoxManager.HasTextBox(this.talkPoint);
			if (!flag)
			{
				Tools.Print<string>("Can use: " + (this.CanUse == null).ToString(), "FFFFFF", false);
				this.m_canUse = ((this.CanUse != null) ? this.CanUse(interactor, base.gameObject) : this.m_canUse);
				base.StartCoroutine(this.HandleConversation(interactor));
			}
		}

		
		private IEnumerator HandleConversation(PlayerController interactor)
		{
			TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, this.text, true, false);
			int selectedResponse = -1;
			interactor.SetInputOverride("shrineConversation");
			yield return null;
			bool flag = !this.m_canUse;
			if (flag)
			{
				GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.declineText, string.Empty);
			}
			else
			{
				bool isToggle = this.isToggle;
				if (isToggle)
				{
					bool isToggled = this.m_isToggled;
					if (isToggled)
					{
						GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.declineText, string.Empty);
					}
					else
					{
						GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.acceptText, string.Empty);
					}
				}
				else
				{
					GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.acceptText, this.declineText);
				}
			}
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			interactor.ClearInputOverride("shrineConversation");
			TextBoxManager.ClearTextBox(this.talkPoint);
			bool flag2 = !this.m_canUse;
			if (flag2)
			{
				yield break;
			}
			bool flag3 = selectedResponse == 0 && this.isToggle;
			if (flag3)
			{
				Action<PlayerController, GameObject> action = this.m_isToggled ? this.OnDecline : this.OnAccept;
				if (action != null)
				{
					action(interactor, base.gameObject);
				}
				this.m_isToggled = !this.m_isToggled;
				yield break;
			}
			bool flag4 = selectedResponse == 0;
			if (flag4)
			{
				Action<PlayerController, GameObject> onAccept = this.OnAccept;
				if (onAccept != null)
				{
					onAccept(interactor, base.gameObject);
				}
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				if (onDecline != null)
				{
					onDecline(interactor, base.gameObject);
				}
			}
			yield break;
		}

		
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		}

		
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		
		public float GetDistanceToPoint(Vector2 point)
		{
			bool flag = base.sprite == null;
			float result;
			if (flag)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}
	}
}
