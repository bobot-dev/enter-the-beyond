using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod;


namespace BotsMod
{ 
    class RockFallBehavior : BasicAttackBehavior
	{

        public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
        {
            base.Init(gameObject, aiActor, aiShooter);
        }

        public override BehaviorResult Update()
        {
            BehaviorResult behaviorResult = base.Update();
            if (behaviorResult != BehaviorResult.Continue)
            {
                return behaviorResult;
            }
            if (!this.IsReady())
            {
                return BehaviorResult.Continue;
            }
            SpawnRockslides(this.m_aiActor);

            return behaviorResult;
        }

        public void SpawnRockslides(AIActor user)
        {




            Vector2 SelectedEnemyPosition = user.TargetRigidbody.sprite.WorldCenter;



            GameManager.Instance.StartCoroutine(HandleTriggerRockSlide(user, Tools.Mines_Cave_In, SelectedEnemyPosition));

        }


        private IEnumerator HandleTriggerRockSlide(AIActor user, GameObject RockSlidePrefab, Vector2 targetPosition)
        {
            RoomHandler CurrentRoom = user.ParentRoom;
            GameObject NewRockSlide = UnityEngine.Object.Instantiate(RockSlidePrefab, targetPosition, Quaternion.identity);
            for (int i = 0; i < rocksToSpawn; i++)
            {
                
                HangingObjectController RockSlideController = NewRockSlide.GetComponent<HangingObjectController>();
                // If you don't null the trigger object, it will attempt to place the plunger somewhere. (but we don't want a plunger to appear)
                // But the room won't have the event trigger so an excpetion will happen in Start()!
                RockSlideController.triggerObjectPrefab = null;
                GameObject[] m_additionalDestroyObjects = new GameObject[] { RockSlideController.additionalDestroyObjects[1] };
                RockSlideController.additionalDestroyObjects = m_additionalDestroyObjects;
                UnityEngine.Object.Destroy(NewRockSlide.transform.Find("Sign").gameObject);
                RockSlideController.ConfigureOnPlacement(CurrentRoom);
                // If we attempt to immedietely trigger it, the cave in animation bugs and it happens too quickly. (there is no fall animation/delay to impact)
                // Delay just a tiny bit before activating.
                yield return new WaitForSeconds(0.01f);
                RockSlideController.Interact(GameManager.Instance.PrimaryPlayer);
                //m_Ready = false;
                yield break;
            }
           

        }

        public int rocksToSpawn;
	}
}
