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
			string shortDesc = "You Spin Me Right Round";
			string longDesc = "Rerolls all items in the room to by lowering the item's id by 1";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 650);
			item.consumable = false;
			item.quality = ItemQuality.S;
			
			item.SetTag("beyond");

			item.sprite.usesOverrideMaterial = true;
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", item.sprite.renderer.material.mainTexture);
			material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 55);
			item.sprite.renderer.material = material;
			BotsItemIds.SpinDownDice = item.PickupObjectId;

		}
		CustomHologramDoer hologramDoer;
		public override void Pickup(PlayerController player)
        {
			hologramDoer = player.gameObject.GetOrAddComponent<CustomHologramDoer>();

			base.Pickup(player);
        }

		public override void Update()
        {
			if (LastOwner != null && LastOwner.CurrentItem == this)
            {
				foreach (DebrisObject debrisObject in StaticReferenceManager.AllDebris.ToArray())
				{
					if (debrisObject.gameObject != gameObject)
                    {
						PickupObject pickupObject = debrisObject.GetComponent<PickupObject>();

						Gun componentInChildren = debrisObject.GetComponentInChildren<Gun>();

						bool flag2 = pickupObject == null && componentInChildren == null;
						if (!flag2)
						{
							if (pickupObject == null && componentInChildren != null)
							{
								pickupObject = componentInChildren;
							}
							if (!excludedInputIds.Contains(pickupObject.PickupObjectId)) hologramDoer.ShowSpinDownHologram(pickupObject.PickupObjectId, pickupObject.gameObject);

							
						}
					}
					
				}

				foreach (var item in LastOwner.passiveItems)
				{

					if (item != null)
					{
						if (item.gameObject.transform.Find("spindown hologram") != null)
						{
							Destroy(item.gameObject.transform.Find("spindown hologram").gameObject);
						}
					}
				}
				foreach (var item in LastOwner.inventory.AllGuns)
				{

					if (item != null)
					{
						if (item.gameObject.transform.Find("spindown hologram") != null)
						{
							Destroy(item.gameObject.transform.Find("spindown hologram").gameObject);
						}
					}
				}
				foreach (var item in LastOwner.activeItems)
				{

					if (item != null)
					{
						if (item.gameObject.transform.Find("spindown hologram") != null)
						{
							Destroy(item.gameObject.transform.Find("spindown hologram").gameObject);
						}
					}
				}

			}
			else
            {
				foreach (DebrisObject debrisObject in StaticReferenceManager.AllDebris.ToArray())
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
						if(pickupObject.gameObject.transform.Find("spindown hologram") != null)
                        {
							Destroy(pickupObject.gameObject.transform.Find("spindown hologram").gameObject);
                        }


					}


				}
			}
            base.Update();
        }

        protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_CHR_dice_voice_01", user.gameObject);

			RerollItemsOnGround();
		}

		public static List<int> excludedInputIds = new List<int>
		{
			73,
			74,
			78,
			85,
			68,
			67,
			120,
			127,
			137,
			224,
			735,
			297,
			600,
		};
		public static List<int> excludedOutputIds = new List<int>
		{

			127,

			296,
			418,
			429,
			497,

			752,
			753,
			754,
			486,
			735,

			501,
			502,
		};

		public static int SpinDownID(int id)
		{



			int newId = id - 1;


			while (true)
			{
				var num = 0;

				//BotsModule.Log($"Id {num}: {newId}");


				num++;

				bool baseCheck = PickupObjectDatabase.GetById(newId) != null;
				bool settingsCheck = BeyondSettings.HasInstance && BeyondSettings.Instance.allowSpindownInsanity;
				bool antifunCheck = baseCheck && (PickupObjectDatabase.GetById(newId).PrerequisitesMet() && !excludedOutputIds.Contains(newId) && PickupObjectDatabase.GetById(newId).quality != ItemQuality.EXCLUDED && PickupObjectDatabase.GetById(newId).quality != ItemQuality.SPECIAL &&
					PickupObjectDatabase.GetById(newId).quality != ItemQuality.COMMON && (PickupObjectDatabase.GetById(newId) is Gun || PickupObjectDatabase.GetById(newId) is PlayerItem || PickupObjectDatabase.GetById(newId) is PassiveItem));

				if (baseCheck && settingsCheck)
				{
					//ETGModConsole.Log("setting");
					return newId;
				}
				else if(baseCheck && antifunCheck)
				{
					//ETGModConsole.Log("fun bad");
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
