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

            AIActor chickenAiActor = EnemyDatabase.GetOrLoadByGuid("76bc43539fc24648bff4568c75c686d1");

            

            CompanionController chickenFriend = chicken.gameObject.GetOrAddComponent<CompanionController>();



            chickenFriend.gameObject.AddAnimation("pet", "BotsMod/sprites/pet", fps: 5, AnimationType.Idle, DirectionType.None);
            chickenFriend.CanBePet = true;
        }
    }
}
