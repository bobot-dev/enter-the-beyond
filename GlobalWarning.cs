using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class GlobalWarning : PassiveItem
	{
		public static void Init()
		{
			//The name of the item
			string itemName = "Global Warming";
			string resourceName = "BotsMod/sprites/globalwarming";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<GlobalWarning>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "AHHHHHHHHHHHHHHHHHHHHH";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = ItemQuality.EXCLUDED;

			BotsItemIds.GlobalWarming = item.PickupObjectId;
		}
	}

}
