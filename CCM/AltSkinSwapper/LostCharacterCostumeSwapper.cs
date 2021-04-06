using CustomCharacters;
using ItemAPI;
using System;
using System.IO;
using UnityEngine;

namespace BotsMod
{
	public class LostCharacterCostumeSwapper : MonoBehaviour, IPlayerInteractable
	{
		// Token: 0x060060D5 RID: 24789 RVA: 0x00247F40 File Offset: 0x00246140
		private void Start()
		{

			var characterCostumeSwapperJsonText = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/CharacterCostumeSwapper #54230.json");

			CharacterCostumeSwapper characterCostumeSwapper = new CharacterCostumeSwapper();



			JsonUtility.FromJsonOverwrite(characterCostumeSwapperJsonText, characterCostumeSwapper);

			BotsModule.Log("started");
			bool flag = GameStatsManager.Instance.GetCharacterSpecificFlag(this.TargetCharacter, CharacterSpecificGungeonFlags.KILLED_PAST);
			//this.CostumeSprite = this.gameObject.GetComponent<tk2dSprite>();

			//this.CostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/lostfriend", this.gameObject).GetComponent<tk2dSprite>();
			this.CostumeSprite = characterCostumeSwapper.CostumeSprite;
			//this.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cloak", this.gameObject).GetComponent<tk2dSprite>();
			this.AlternateCostumeSprite = characterCostumeSwapper.AlternateCostumeSprite;


			this.TargetLibrary = characterCostumeSwapper.TargetLibrary;

			if (flag)
			{
				this.m_active = true;
				//if (this.TargetCharacter == PlayableCharacters.Guide)
				//{
				//	this.CostumeSprite.HeightOffGround = 0.25f;
				//	this.AlternateCostumeSprite.HeightOffGround = 0.25f;
				//	this.CostumeSprite.UpdateZDepth();
				//	this.AlternateCostumeSprite.UpdateZDepth();
				//}
				AlternateCostumeSprite.renderer.enabled = true;
				CostumeSprite.renderer.enabled = false;


			}
			else
			{
				this.m_active = false;
				this.AlternateCostumeSprite.renderer.enabled = false;
				this.CostumeSprite.renderer.enabled = false;
				BotsModule.Log("nope");

			}

		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x00248058 File Offset: 0x00246258
		private void Update()
		{
			if (this.m_active)
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
				if (this.TargetCharacter != PlayableCharacters.CoopCultist && GameManager.Instance.PrimaryPlayer.characterIdentity != this.TargetCharacter)
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
					SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
					this.AlternateCostumeSprite.renderer.enabled = true;
					this.CostumeSprite.renderer.enabled = false;
				}
			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x00248114 File Offset: 0x00246314
		public float GetDistanceToPoint(Vector2 point)
		{
			if (!this.m_active)
			{
				return 1000f;
			}
			if (this.AlternateCostumeSprite.renderer.enabled)
			{
				return Vector2.Distance(point, this.AlternateCostumeSprite.WorldCenter);
			}
			return Vector2.Distance(point, this.CostumeSprite.WorldCenter);
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x0024816C File Offset: 0x0024636C
		public void OnEnteredRange(PlayerController interactor)
		{
			if (interactor.characterIdentity != this.TargetCharacter)
			{
				return;
			}
			if (this.AlternateCostumeSprite.renderer.enabled)
			{
				SpriteOutlineManager.AddOutlineToSprite(this.AlternateCostumeSprite, Color.white);
			}
			else
			{
				SpriteOutlineManager.AddOutlineToSprite(this.CostumeSprite, Color.white);
			}
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x002481C8 File Offset: 0x002463C8
		public void OnExitRange(PlayerController interactor)
		{
			if (interactor.characterIdentity != this.TargetCharacter)
			{
				return;
			}
			if (this.AlternateCostumeSprite.renderer.enabled)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
			}
			else
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
			}
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x0024821C File Offset: 0x0024641C
		public void Interact(PlayerController interactor)
		{
			if (interactor.characterIdentity != this.TargetCharacter)
			{
				return;
			}
			if (!this.m_active)
			{
				return;
			}
			if (interactor.IsUsingAlternateCostume)
			{
				interactor.SwapToAlternateCostume(null);
			}
			else
			{
				if (this.TargetLibrary)
				{
					interactor.AlternateCostumeLibrary = this.TargetLibrary;
				}
				interactor.SwapToAlternateCostume(null);
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternateCostumeSprite, false);
			SpriteOutlineManager.RemoveOutlineFromSprite(this.CostumeSprite, false);
			this.AlternateCostumeSprite.renderer.enabled = !this.AlternateCostumeSprite.renderer.enabled;
			this.CostumeSprite.renderer.enabled = !this.CostumeSprite.renderer.enabled;
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

		// Token: 0x04005B70 RID: 23408
		public PlayableCharacters TargetCharacter = (PlayableCharacters)CustomPlayableCharacters.Lost;

		// Token: 0x04005B71 RID: 23409
		public tk2dSprite CostumeSprite;

		// Token: 0x04005B72 RID: 23410
		public tk2dSprite AlternateCostumeSprite;

		// Token: 0x04005B73 RID: 23411
		public tk2dSpriteAnimation TargetLibrary;

		// Token: 0x04005B74 RID: 23412
		public bool HasCustomTrigger = false;

		// Token: 0x04005B75 RID: 23413
		public bool CustomTriggerIsFlag = false;

		// Token: 0x04005B76 RID: 23414
		public GungeonFlags TriggerFlag = GungeonFlags.NONE;

		// Token: 0x04005B77 RID: 23415
		public bool CustomTriggerIsSpecialReserve = false;

		// Token: 0x04005B78 RID: 23416
		private bool m_active;
	}
}

