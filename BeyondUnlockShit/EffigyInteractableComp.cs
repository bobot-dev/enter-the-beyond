using Dungeonator;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class EffigyInteractableComp : DungeonPlaceableBehaviour, IPlayerInteractable
    {
		tk2dSpriteAnimator animator;

		public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
		{			
			return base.InstantiateObject(targetRoom, loc, deferConfiguration);
		}

		private void OnEnable()
		{
			RoomHandler absoluteParentRoom = base.GetAbsoluteParentRoom();
			for (int i = 0; i < this.worldLocks.Count; i++)
			{
				absoluteParentRoom.RegisterInteractable(this.worldLocks[i]);
			}

			animator = this.GetComponent<tk2dSpriteAnimator>();

			Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.mainTexture = animator.sprite.renderer.material.mainTexture;
			mat.SetColor("_EmissiveColor", new Color32(255, 69, 248, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 50);
			animator.sprite.renderer.material = mat;

			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);

			if (!BeyondSettings.Instance.debug || SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED))
            {
				state = EffigyState.POWERED;
				animator.DefaultClipId = spriteAnimator.GetClipIdByName("idle");
			} 
			else
            {
				state = EffigyState.NEEDS_RELIC;
				animator.DefaultClipId = spriteAnimator.GetClipIdByName("unchargedIdle");
			}
		}

		private bool IsValidForUse()
		{
			bool result = true;
			for (int i = 0; i < this.worldLocks.Count; i++)
			{
				if (this.worldLocks[i].IsLocked || this.worldLocks[i].spriteAnimator.IsPlaying(this.worldLocks[i].spriteAnimator.CurrentClip))
				{
					result = false;
				}
			}
			return result;
		}
		public void OnEnteredRange(PlayerController interactor)
		{
			if (!this)
			{
				return;
			}
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		public void OnExitRange(PlayerController interactor)
		{
			if (!this)
			{
				return;
			}
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}
		public float GetDistanceToPoint(Vector2 point)
		{		
			if (!this.IsValidForUse())
			{
				return 1000f;
			}
			Bounds bounds = base.sprite.GetBounds();
			bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
			float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
			float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
			return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
		}
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}
		public void Interact(PlayerController player)
		{
			if (state == EffigyState.POWERED)
			{
				if (this.IsValidForUse())
				{
					GameManager.Instance.LoadCustomLevel(this.targetLevelName);
				}
			} 
			else
            {
				this.m_canUse = (player.CurrentGun != null && player.CurrentGun.PickupObjectId == BotsItemIds.Relic3 && !SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED));
				base.StartCoroutine(HandleConversation(player));
			}
			
		}

		bool m_canUse = true;

		private IEnumerator HandleConversation(PlayerController interactor)
		{
			
			int selectedResponse = -1;
			interactor.SetInputOverride("shrineConversation");
			yield return null;
			bool flag = !this.m_canUse;
			if (flag)
			{
				GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, "Walk away", string.Empty);
			}
			else
			{
				GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, "Place the relic on the effigy", "Walk away");
			}
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			interactor.ClearInputOverride("shrineConversation");
			bool flag2 = !this.m_canUse;
			if (flag2)
			{
				yield break;
			}			
			bool flag4 = selectedResponse == 0;
			if (flag4)
			{
				if (m_canUse)
                {
					interactor.inventory.DestroyCurrentGun();
					SaveAPIManager.SetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED, true);					
					animator.Play("charging");
					animator.AnimationCompleted += OnAnimEnded;
				}
					
			}
			else
			{
				
			}
			yield break;
		}

		public void OnAnimEnded(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip)
        {
			if (clip.name == "charging")
            {
				animator.Play("idle");
				spriteAnimator.AnimationCompleted -= OnAnimEnded;
			}			
		}

		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
		public string targetLevelName;
		public List<InteractableLock> worldLocks;
		public GlobalDungeonData.ValidTilesets targetTileset;
		public TileIndexGrid overridePitIndex;
		public EffigyState state;
		private float m_timeHovering;
		private bool m_isLoading;

		public enum EffigyState
        {
			POWERED,
			NEEDS_RELIC
        }

	}
}
