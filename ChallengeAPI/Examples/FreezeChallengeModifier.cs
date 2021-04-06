using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;

namespace ChallengeAPI
{
    /// <summary>
    /// A basic example challenge.
    /// </summary>
    public class FreezeChallengeModifiereModifier : ChallengeModifier
    {
        PlayerController player;
        public void Start()
        {
            //If you want something to happen when the challenge starts, this is the place.

            ETGModConsole.Log("Example Challenge Started!");
        }

        

        public void Update()
        {
            player = GameManager.Instance.PrimaryPlayer;

            var goop = PickupObjectDatabase.GetById(411).GetComponent<DirectionalAttackActiveItem>().goopDefinition;
            //if (player.CurrentRoom != null && player.CurrentGoop != goop)
            //{

            goop.CanBeFrozen = true;
            goop.CanBeIgnited = false;
            goop.CanBeElectrified = false;
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
            goopManagerForGoopType.AddGoopCircle(player.specRigidbody.UnitBottomCenter, 2);
            goopManagerForGoopType.FreezeGoopCircle(player.specRigidbody.UnitBottomCenter, 2);
        }

        public void OnDestroy()
        {
            //If you want something to happen when the challenge ends, this is the place.




           // }
            ETGModConsole.Log("Example Challenge Ended!");
        }

        public override bool IsValid(RoomHandler room)
        {
            //This method checks if the room is valid for this challenge. If you return true it means it's valid, if you return false it means it's not valid.
            return true;
        }
    }
}
