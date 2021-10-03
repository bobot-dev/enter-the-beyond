using UnityEngine;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using SaveAPI;

namespace BotsMod
{
	class BlessedOrb : PlayerItem
    {
		public static void Init()
		{
			//The name of the item
			string itemName = "Blessed Orb";
			string resourceName = "BotsMod/sprites/blessed_orb";
			GameObject obj = new GameObject();
			var item = obj.AddComponent <BlessedOrb>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "The Orb of RNG";
			string longDesc = "WARNING USING THIS ITEM WILL MAKE YOU LOSE ALL GUNS THIS CANNOT BE UNDONE \n\n An old orb made by the Sorceress.Grant the user some of her great power.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 1);
			item.consumable = false;
			item.quality = ItemQuality.D;
			item.PlaceItemInAmmonomiconAfterItemById(634);
			item.SetupUnlockOnFlag(GungeonFlags.SORCERESS_BLESSED_MODE_COMPLETE, true);
		}

		public override void Pickup(PlayerController player)
		{
			player.CurrentGun.OnReloadPressed += this.OnReload;
			player.PostProcessThrownGun += this.PostProcessThrownGun;
			player.GunChanged += this.OnGunChanged;
			//player.DoPostProcessThrownGun += this.PostProcessThrownGun;
			//PlayerItem item = LootEngine.GetItemOfTypeAndQuality<PlayerItem>(ItemQuality.D, GameManager.Instance.RewardManager.ItemsLootTable, true);
			
			base.Pickup(player);

		}

		public void OnGunChanged(Gun old, Gun current, bool newGun)
		{
			current.OnReloadPressed += this.OnReload;
			OnReload(LastOwner, current, newGun);
		}

		protected override void OnPreDrop(PlayerController player)
		{
			player.CurrentGun.OnReloadPressed -= this.OnReload;
			player.PostProcessThrownGun -= this.PostProcessThrownGun;
			player.GunChanged -= this.OnGunChanged;
			base.OnPreDrop(player);
		}



		public void PostProcessThrownGun(Projectile p)
		{
			p.DestroyMode = Projectile.ProjectileDestroyMode.Destroy;
		}



		public void OnReload(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (LastOwner.CharacterUsesRandomGuns == true)
			{
				if (gun.ClipShotsRemaining == 0)
				{
					bool flag = player.CurrentGun != null;
					if (flag)
					{
						player.CurrentGun.PrepGunForThrow();
						typeof(Gun).GetField("m_prepThrowTime", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(player.CurrentGun, 999);
						player.CurrentGun.CeaseAttack(true, null);
					}
				}

			}
		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_NPC_magic_blessing_01", base.gameObject);
			if (user.CharacterUsesRandomGuns == true)
			{
				user.ChangeToRandomGun();
				
			} else
			{
				user.CharacterUsesRandomGuns = true;
			}

			
		}
	}
}
