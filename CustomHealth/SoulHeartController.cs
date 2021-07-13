using Dungeonator;
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    static class SoulHeartController
    {
        public static float soulHeartCount = 0;
        
        public static float currentSoulHeartCount;
        public static List<dfSprite> extantSoulHearts = new List<dfSprite>();

		public static string fullSoulHeartName = "heart_full_soul_001";

		public static void Init()
		{
			var atlas = GameUIRoot.Instance.ConversationBar.portraitSprite.Atlas;
			atlas.AddNewItemToAtlas(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/heart_full_purple_001.png"), fullSoulHeartName);
		}

		public static void OnSoulHeartLost(PlayerController player)
		{
			AkSoundEngine.PostEvent("Play_Bot_Hammer", player.gameObject);
		}

		public static void AddSoulHeart(GameUIHeartController heartController)
		{

			FieldInfo _panel = typeof(GameUIHeartController).GetField("m_panel", BindingFlags.NonPublic | BindingFlags.Instance);

			Vector3 position = heartController.transform.position;

			var soulHeartPrefab = FakePrefab.Clone(heartController.heartSpritePrefab.gameObject);
			var soulHeartSprite = soulHeartPrefab.GetComponent<dfSprite>();
			soulHeartSprite.SpriteName = fullSoulHeartName;


			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(soulHeartPrefab, position, Quaternion.identity);
			gameObject.transform.parent = heartController.transform.parent;
			gameObject.layer = heartController.gameObject.layer;
			dfSprite component = gameObject.GetComponent<dfSprite>();
			if (heartController.IsRightAligned)
			{
				component.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Right);
			}
			Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
			component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			component.IsInteractive = false;
			if (!heartController.IsRightAligned)
			{
				float num = (heartController.extantHearts.Count <= 0) ? 0f : ((heartController.extantHearts[0].Width + Pixelator.Instance.CurrentTileScale) * (float)heartController.extantHearts.Count);
				float num2 = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)extantSoulHearts.Count;
				component.RelativePosition = (_panel.GetValue(heartController) as dfPanel).RelativePosition + new Vector3(num + num2, 0f, 0f);
			}
			else
			{
				component.RelativePosition = (_panel.GetValue(heartController) as dfPanel).RelativePosition - new Vector3(component.Width, 0f, 0f);
				for (int i = 0; i < extantSoulHearts.Count; i++)
				{
					dfSprite dfSprite = extantSoulHearts[i];
					if (dfSprite)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, true, false, true);
						dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
				for (int j = 0; j < heartController.extantHearts.Count; j++)
				{
					dfSprite dfSprite2 = heartController.extantHearts[j];
					if (dfSprite2)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
						dfSprite2.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
			extantSoulHearts.Add(component);
			GameUIRoot.Instance.AddControlToMotionGroups(component, (!heartController.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
		}

		public static void RemoveSoulHeart(GameUIHeartController self)
		{
			FieldInfo _panel = typeof(GameUIHeartController).GetField("m_panel", BindingFlags.NonPublic | BindingFlags.Instance);
			if (extantSoulHearts.Count > 0)
			{
				dfSprite dfSprite = self.damagedArmorAnimationPrefab;
				dfSprite dfSprite2 = extantSoulHearts[extantSoulHearts.Count - 1];
				if (dfSprite2)
				{
					if (dfSprite2.SpriteName == self.crestSpritePrefab.SpriteName)
					{
						dfSprite = self.damagedCrestAnimationPrefab;
					}
					if (dfSprite != null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(dfSprite.gameObject);
						gameObject.transform.parent = self.transform.parent;
						gameObject.layer = self.gameObject.layer;
						dfSprite component = gameObject.GetComponent<dfSprite>();
						component.BringToFront();
						dfSprite2.Parent.AddControl(component);
						dfSprite2.Parent.BringToFront();
						component.ZOrder = dfSprite2.ZOrder - 1;
						component.RelativePosition = dfSprite2.RelativePosition + self.damagedArmorPrefabOffset;
						(_panel.GetValue(self) as dfPanel).AddControl(component);
					}
				}
				float width = extantSoulHearts[0].Width;
				if (dfSprite2)
				{
					GameUIRoot.Instance.RemoveControlFromMotionGroups(dfSprite2);
					UnityEngine.Object.Destroy(extantSoulHearts[extantSoulHearts.Count - 1]);
				}
				extantSoulHearts.RemoveAt(extantSoulHearts.Count - 1);
				if (self.IsRightAligned)
				{
					for (int i = 0; i < extantSoulHearts.Count; i++)
					{
						dfSprite dfSprite3 = extantSoulHearts[i];
						if (dfSprite3)
						{
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, true, false, true);
							dfSprite3.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
							GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite3);
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
						}
					}
					for (int j = 0; j < self.extantHearts.Count; j++)
					{
						dfSprite dfSprite4 = self.extantHearts[j];
						if (dfSprite4)
						{
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite4, true, false, true);
							dfSprite4.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
							GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite4);
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite4, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
						}
					}
				}
			}
		}
	}
}
