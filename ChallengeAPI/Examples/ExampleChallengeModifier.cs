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
    public class ExampleChallengeModifier : ChallengeModifier
    {
        PlayerController player;
        public void Start()
        {
            //If you want something to happen when the challenge starts, this is the place.
            player = GameManager.Instance.PrimaryPlayer;
            player.healthHaver.OnDamaged += HealthHaver_OnDamaged;
            ETGModConsole.Log("Example Challenge Started!");
        }

        private void HealthHaver_OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, UnityEngine.Vector2 damageDirection)
        {
            if (UnityEngine.Random.Range(1, 11) > 5)
            {
                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        var passiveItem = player.passiveItems[UnityEngine.Random.Range(0, player.passiveItems.Count)];
                        if (passiveItem.CanActuallyBeDropped(player))
                        {
                            player.DropPassiveItem(passiveItem);
                        }
                       
                        break;


                    case 2:
                        var activeItem = player.activeItems[UnityEngine.Random.Range(0, player.passiveItems.Count)];
                        if (activeItem.CanActuallyBeDropped(player))
                        {
                            player.DropActiveItem(activeItem);
                        }
                        break;


                    default:
                        var gun = player.inventory.AllGuns[UnityEngine.Random.Range(0, player.passiveItems.Count)];
                        if (gun.CanActuallyBeDropped(player))
                        {
                            player.ForceDropGun(gun);
                        }
                        break;
                }
            }
            
        }

        public void Update()
        {

        }

        public void OnDestroy()
        {
            //If you want something to happen when the challenge ends, this is the place.
            player.healthHaver.OnDamaged -= HealthHaver_OnDamaged;
            ETGModConsole.Log("Example Challenge Ended!");
        }

        public override bool IsValid(RoomHandler room)
        {
            //This method checks if the room is valid for this challenge. If you return true it means it's valid, if you return false it means it's not valid.
            return true;
        }
    }
}
