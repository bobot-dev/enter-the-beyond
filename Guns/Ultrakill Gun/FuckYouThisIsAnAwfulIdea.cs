using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace BotsMod
{
    public class FuckYouThisIsAnAwfulIdea : MonoBehaviour, ILevelLoadedListener
	{
		public dfPanel Panel
		{
			get
			{
				return this.m_panel;
			}
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x00051030 File Offset: 0x0004F230
		private void Awake()
		{
			this.m_panel = base.GetComponent<dfPanel>();
			this.extantCoins = new List<dfSprite>();
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x003A17D8 File Offset: 0x0039F9D8
		private void Start()
		{
			this.m_panel.IsInteractive = false;
			var pos = this.m_panel.RelativePosition;
			pos.y -= 32;
			Collider[] components = base.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x00051049 File Offset: 0x0004F249
		public void BraveOnLevelWasLoaded()
		{
			if (this.extantCoins != null)
			{
				this.extantCoins.Clear();
			}
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x003A1814 File Offset: 0x0039FA14
		public void UpdateScale()
		{
			for (int i = 0; i < this.extantCoins.Count; i++)
			{
				dfSprite dfSprite = this.extantCoins[i];
				if (dfSprite)
				{
					Vector2 sizeInPixels = dfSprite.SpriteInfo.sizeInPixels;
					dfSprite.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
				}
			}
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x003A187C File Offset: 0x0039FA7C
		public dfSprite AddCoin()
		{
			BotsModule.Log("0");
			if (gameObject.transform.position != null)
            {
				BotsModule.Log(gameObject.transform.position.ToString());
			}

			Vector3 position = gameObject.transform.position;
			BotsModule.Log("0.5");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.CoinSpritePrefab.gameObject, position, Quaternion.identity);
			BotsModule.Log("1");
			gameObject2.transform.parent = transform.parent;
			gameObject2.layer = gameObject.layer;
			dfSprite component = gameObject2.GetComponent<dfSprite>();
			if (this.IsRightAligned)
			{
				component.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Right);
			}
			BotsModule.Log("2");
			Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
			component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			if (!this.IsRightAligned)
			{
				float x = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)this.extantCoins.Count;
				component.RelativePosition = this.m_panel.RelativePosition + new Vector3(x, 0f, 0f);
			}
			else
			{
				component.RelativePosition = this.m_panel.RelativePosition - new Vector3(component.Width, 0f, 0f);
				for (int i = 0; i < this.extantCoins.Count; i++)
				{
					dfSprite dfSprite = this.extantCoins[i];
					if (dfSprite)
					{
						dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
					}
				}
			}
			BotsModule.Log("3");
			component.IsInteractive = false;
			this.extantCoins.Add(component);
			GameUIRoot.Instance.AddControlToMotionGroups(component, (!this.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
			return component;
		}

		// Token: 0x06008C4E RID: 35918 RVA: 0x003A1A4C File Offset: 0x0039FC4C
		public void RemoveCoin()
		{
			if (this.extantCoins.Count > 0)
			{
				float width = this.extantCoins[0].Width;
				dfSprite dfSprite = this.extantCoins[this.extantCoins.Count - 1];
				if (dfSprite)
				{
					GameUIRoot.Instance.RemoveControlFromMotionGroups(dfSprite);
					UnityEngine.Object.Destroy(dfSprite);
				}
				this.extantCoins.RemoveAt(this.extantCoins.Count - 1);
				if (this.IsRightAligned)
				{
					for (int i = 0; i < this.extantCoins.Count; i++)
					{
						dfSprite dfSprite2 = this.extantCoins[i];
						if (dfSprite2)
						{
							dfSprite2.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
							GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
						}
					}
				}
			}
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x003A1B44 File Offset: 0x0039FD44
		public void UpdateCoins(int numCoins)
		{
			if (GameManager.Instance.IsLoadingLevel || Time.timeSinceLevelLoad < 0.01f)
			{
				return;
			}
			if (this.extantCoins.Count < numCoins)
			{
				for (int i = this.extantCoins.Count; i < numCoins; i++)
				{
					this.AddCoin();
				}
			}
			else if (this.extantCoins.Count > numCoins)
			{
				while (this.extantCoins.Count > numCoins)
				{
					this.RemoveCoin();
				}
			}
		}

		// Token: 0x04009376 RID: 37750
		public dfSprite CoinSpritePrefab;

		// Token: 0x04009377 RID: 37751
		public List<dfSprite> extantCoins;

		// Token: 0x04009378 RID: 37752
		public bool IsRightAligned;

		// Token: 0x04009379 RID: 37753
		private dfPanel m_panel;
	}

}
