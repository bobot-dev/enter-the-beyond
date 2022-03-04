using Dungeonator;
using GungeonAPI;
using ItemAPI;
using MultiplayerBasicExample;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class RedKey : PlayerItem
	{
		public static void Init()
		{
			//The name of the item
			string itemName = "Red Key";
			string resourceName = "BotsMod/sprites/red_key";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<RedKey>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Explore the other side";
			string longDesc = "text goes here i think";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1);
			item.consumable = false;
			item.quality = ItemQuality.EXCLUDED;

		}
		GenericRoomTable CastleRoomTable;
		AssetBundle sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");
		public override void Pickup(PlayerController player)
		{
			CastleRoomTable = sharedAssets2.LoadAsset<GenericRoomTable>("Castle_RoomTable");
			base.Pickup(player);
		}

		protected override void DoEffect(PlayerController user)
		{
			var room = user.CurrentRoom;
			List<RoomHandler> RoomSequence = new List<RoomHandler>();
			for (int i = 1; i < room.injectionFrameData.Count; i++)
			{
				RoomSequence.Add(room.injectionFrameData[i]);
				BotsModule.Log(room.GetRoomName());
			}


	

			GameObject effect = (PickupObjectDatabase.GetById(353) as RagePassiveItem).OverheadVFX;


			for (int i = 1; i < RoomSequence.Count; i++)
			{
				var exist = room.GetExitConnectedToRoom(RoomSequence[i]);
				GameObject gameObject = SpawnManager.SpawnVFX(effect, false);
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();

				Vector3 a = new Vector3(exist.GetExitOrigin(0).x, exist.GetExitOrigin(0).y, 0);
				component.PlaceAtPositionByAnchor(a, tk2dBaseSprite.Anchor.MiddleCenter);
			}

			return;
			/*var room = RoomFactory.CreateRoomFromTexture(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/cursething.png"));
			RoomFactory.AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
			RoomFactory.AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
			RoomFactory.AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
			RoomFactory.AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
			room.usesCustomAmbientLight = true;
			room.customAmbientLight = Color.red;

			room.UseCustomMusic = true;*/

			//room.music

			//room.ti

			//room.overriddenTilesets = GlobalDungeonData.ValidTilesets.


			//RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);
			
			

			//creepyRoom.OverrideTilemap = 

			//Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



			//user.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

			/*
			if (user.CurrentRoom != null)
			{
				RoomHandler room = user.CurrentRoom;
				RoomHandler absoluteRoom = user.transform.position.GetAbsoluteRoom();
				var i = 0;
				BotsModule.Log("room found", BotsModule.LOST_COLOR);

				//room.area.prototypeRoom
				//room.pos

				//absoluteRoom.

				if (absoluteRoom.connectedDoors != null)
				{
					BotsModule.Log("connectedDoors found");
					foreach (var thing in absoluteRoom.connectedDoors)
					{
						thing.ForceBecomeLockedDoor();
						BotsModule.Log("door locked");
					}

				}
				else
				{
					BotsModule.Log("connectedDoors nulled", BotsModule.LOCKED_CHARACTOR_COLOR);
				}


				if (room.area.prototypeRoom.exitData != null)
				{
					BotsModule.Log("exitData found");

				}
				else
				{
					BotsModule.Log("exitData nulled", BotsModule.LOCKED_CHARACTOR_COLOR);
				}
				//foreach (PrototypeRoomExit door in room.area.prototypeRoom.exitData.exits)
				//{
				//	if (door != null)
				//	{
				//		BotsModule.Log("door number " + i + " is facing " + door.exitDirection);

				//	}
				//	else
				//	{
				//		BotsModule.Log("door nulled", BotsModule.LOCKED_CHARACTOR_COLOR);
				//	}
				//	i++;
				//}
			}
			else
			{
				BotsModule.Log("room nulled", BotsModule.LOCKED_CHARACTOR_COLOR);
			}
			*/
			

		}
	}
}
