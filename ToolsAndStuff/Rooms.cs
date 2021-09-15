using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Rooms
    {
        public static void Init()
        {
            SecretLostUnlockRoom = RoomFactory.BuildFromResource("BotsMod/rooms/Beyond/LostUnlockRoom.room");
            SecretLostUnlockRoom.roomEvents = new List<RoomEventDefinition>() {
                new RoomEventDefinition(RoomEventTriggerCondition.NPC_TRIGGER_A, RoomEventTriggerAction.SEAL_ROOM),
                new RoomEventDefinition(RoomEventTriggerCondition.NPC_TRIGGER_C, RoomEventTriggerAction.UNSEAL_ROOM),
            };
            SecretLostUnlockRoom.usesProceduralLighting = false;
            SecretLostUnlockRoom.allowFloorDecoration = false;
            SecretLostUnlockRoom.allowWallDecoration = false;
            SecretLostUnlockRoom.usesProceduralDecoration = false;
            SecretLostUnlockRoom.precludeAllTilemapDrawing = true;

        }

        public static PrototypeDungeonRoom SecretLostUnlockRoom;
    }
}
