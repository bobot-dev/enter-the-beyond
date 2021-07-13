using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{ 
    class SpellPickupObject : PickupObject, IPlayerInteractable
    {
		public Spell spell;
		private bool m_pickedUp;


		public override void Pickup(PlayerController player)
        {
			//add check here later to make sure the player's inv isnt full or maybe put it in Interact ¯\_(ツ)_/¯

			//GameUIRoot.Instance.InformNeedsReload(player, new Vector3(player.specRigidbody.UnitCenter.x - player.transform.position.x, 1.25f, 0f), 1f, "Obtained Spell: " + spell.name);
			if (this.spell == null)
			{
				Wand.GainNewSpell(StaticSpellReferences.sparkBolt);
			}
			else
			{
				Wand.GainNewSpell(this.spell);
			}	
			


			//this.m_pickedUp = true;
			//this.m_isBeingEyedByRat = false;
			

			UnityEngine.Object.Destroy(base.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);

		}



		public void Interact(PlayerController interactor)
		{
			if (!this)
			{
				return;
			}
			if (!interactor.HasGun(BotsItemIds.Wand))
			{
				GameUIRoot.Instance.InformNeedsReload(interactor, new Vector3(interactor.specRigidbody.UnitCenter.x - interactor.transform.position.x, 1.25f, 0f), 1f, "You require a wand to pick this up");
				return;
			}
			if (RoomHandler.unassignedInteractableObjects.Contains(this))
			{
				RoomHandler.unassignedInteractableObjects.Remove(this);
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			this.Pickup(interactor);
		}

		public float GetDistanceToPoint(Vector2 point)
		{
			if (!base.sprite)
			{
				return 1000f;
			}
			Bounds bounds = base.sprite.GetBounds();
			bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
			float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
			float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
			return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2)) / 1.5f;
		}

		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		
		public void OnEnteredRange(PlayerController interactor)
		{
			if (!this)
			{
				return;
			}
			if (!interactor.CurrentRoom.IsRegistered(this) && !RoomHandler.unassignedInteractableObjects.Contains(this))
			{
				return;
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		public void OnExitRange(PlayerController interactor)
		{
			if (!this)
			{
				return;
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

	}
}
