using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace BotsMod
{
	public class Bob : PassiveItem
	{
		public static void Init()
		{
			//The name of the item
			string itemName = "Bob";

			//Refers to an embedded png in the project. Make sure to embed your sprites! Google it
			string resourceName = "BotsMod/sprites/shades";

			//Create new GameObject
			GameObject obj = new GameObject();

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<Bob>();

			//Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "Deal With It";
			string longDesc = "So Cool \n\n This is only here coz it was my first item hope you like the ms paint sprite ^_^";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

			//Adds the actual passive effect to the item
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);

			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.EXCLUDED;
		}
	}
}
