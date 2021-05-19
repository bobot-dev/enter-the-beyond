using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;

namespace BotsMod
{
    class MakeThemAllPetable
    {
        public static void Init()
        {
            CompanionItem chicken = PickupObjectDatabase.GetById(572).GetComponent<CompanionItem>();

            //chicken.CompanionGuid

            AIActor chickenAiActor = EnemyDatabase.GetOrLoadByGuid("e456b66ed3664a4cb590eab3a8ff3814");

            CompanionController orAddComponent = chickenAiActor.gameObject.GetOrAddComponent<CompanionController>();

            //CompanionController chickenFriend = chicken.gameObject.GetOrAddComponent<CompanionController>();



            orAddComponent.gameObject.AddAnimation("pet", "BotsMod/sprites/pet", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
            orAddComponent.CanBePet = true;
        }
    }
}
