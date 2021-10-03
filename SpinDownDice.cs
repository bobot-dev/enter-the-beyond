using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SpinDownDice : PlayerItem
    {
		public static void Init()
		{
			//The name of the item
			string itemName = "Spin Down Dice";
			string resourceName = "BotsMod/sprites/SpinDownDice";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<SpinDownDice>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Hehe SPIIIIIIIIIIIIIIIIIIIIIIIINNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
			string longDesc = "drugs maybe have been involved with this item idfk";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.S;

			Tools.BeyondItems.Add(item.PickupObjectId);

		}

		public override void Update()
		{




			base.Update();
		}


		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_CHR_dice_voice_01", user.gameObject);

			RerollItemsOnGround();
		}

		List<int> excludedInputIds = new List<int>
		{
			73,
			74,
			85,
			68,
			67,
		};
		List<int> excludedOutputIds = new List<int>();

		private int SpinDownID(int id)
		{
			int newId = id - 1;


			while (true)
			{
				var num = 0;

				BotsModule.Log($"Id {num}: {newId}");


				num++;


				if (PickupObjectDatabase.GetById(newId) != null)
				{
					return newId;
				} 
				else
				{
					newId--;
				}

				
			}


		}

		public static DebrisObject SpawnItem(GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disableHeightBoost = false)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, spawnPosition, Quaternion.identity);
			GameObject spawnedItem = gameObject;
			return typeof(LootEngine).GetMethod("SpawnInternal", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { spawnedItem, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, false, disableHeightBoost }) as DebrisObject;
		}

		private void RerollItemsOnGround()
		{
			foreach (DebrisObject debrisObject in StaticReferenceManager.AllDebris.ToArray())
			{
				bool flag = debrisObject == null;
				if (!flag)
				{
					PickupObject pickupObject = debrisObject.GetComponent<PickupObject>();
					Gun componentInChildren = debrisObject.GetComponentInChildren<Gun>();
					bool flag2 = pickupObject == null && componentInChildren == null;
					if (!flag2)
					{
						bool flag3 = pickupObject == null && componentInChildren != null;
						if (flag3)
						{
							pickupObject = componentInChildren;
						}
						bool flag4 = pickupObject != null;
						if (flag4)
						{
							if (!excludedInputIds.Contains(pickupObject.PickupObjectId))
							{
								if (pickupObject != null)
								{
									var lable = GameUIRoot.Instance.RegisterDefaultLabel(pickupObject.gameObject.transform, new Vector3(0, 1, 0), pickupObject.PickupObjectId.ToString());

									lable.GetComponent<DefaultLabelController>().panel.IsVisible = true;


								}

								if (pickupObject.PickupObjectId <= 0)
                                {
									debrisObject.TriggerDestruction(true);
								} else
                                {
									var gameObject = PickupObjectDatabase.GetById(SpinDownID(pickupObject.PickupObjectId)).gameObject;
									bool flag12 = gameObject == null;
									if (flag12)
									{
										Console.WriteLine("Couldn't add an item! Giving junk instead.");
										PickupObject pickupObject2 = Game.Items.Get("junk");
										bool flag13 = pickupObject2 == null;
										if (flag13)
										{
											Console.WriteLine("Cannot get 'gungeon:junk' item! Not changing ground item!");
										}
										else
										{
											gameObject = pickupObject2.gameObject;
										}
									}
									bool flag14 = gameObject != null;
									if (flag14)
									{
										PickupObject component = gameObject.GetComponent<PickupObject>();
										Console.WriteLine(string.Format("Attempting to change ground item: {0}={1}={2} to {3}={4}={5}", new object[]
										{
											pickupObject.PickupObjectId,
											pickupObject.name,
											pickupObject.DisplayName,
											component.PickupObjectId,
											component.name,
											component.DisplayName
										}));
										DebrisObject debrisObject2 = SpawnItem(gameObject, debrisObject.UnadjustedDebrisPosition, Vector2.zero, 0f, true, false, true);
										LootEngine.DoDefaultItemPoof(debrisObject.UnadjustedDebrisPosition, false, false);
										debrisObject.TriggerDestruction(true);
									}
								}

								
							}

							
							
						}
					}
				}
			}
		}

	}
}
