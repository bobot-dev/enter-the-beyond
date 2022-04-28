using Brave.BulletScript;
using EnemyAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class EtgRandomizerController
    {

        public static void Init()
        {
            RandomizeEnemies();
        }

        static List<AIActor> idiots = new List<AIActor>();
        static List<AttackBehaviorBase> idiotsBehaviors = new List<AttackBehaviorBase>();


        private static void RandomizeItems()
        {

            foreach (PickupObject item in PickupObjectDatabase.Instance.Objects)
            {
                if (item != null)
                {
                    int id = item.PickupObjectId;

                    if (PickupObjectDatabase.GetById(id).quality > PickupObject.ItemQuality.COMMON)
                    {
                        PickupObject.ItemQuality oldQuality = PickupObjectDatabase.GetById(id).quality;
                        PickupObjectDatabase.GetById(id).quality = (PickupObject.ItemQuality)UnityEngine.Random.Range(1, 5);

                        //ETGModConsole.Log(PickupObjectDatabase.GetById(id).name + " " + PickupObjectDatabase.GetById(id).quality + " used to be " + oldQuality);
                    }
                }
            }
        }

        private static void RandomizeEnemies()
        {
            foreach (EnemyDatabaseEntry entry in EnemyDatabase.Instance.Entries)
            {
                if (entry == null)
                { 
                    BotsModule.Log("SHIT!!", BotsModule.LOCKED_CHARACTOR_COLOR);
                }

                BotsModule.Log("foreach started", BotsModule.LOCKED_CHARACTOR_COLOR);
                

                if (entry != null)
                {
                    BotsModule.Log(entry.name, BotsModule.LOST_COLOR);

                    AIActor victum = EnemyDatabase.GetOrLoadByGuid(entry.myGuid);
                    if (victum != null && victum.IsNormalEnemy && !victum.InBossAmmonomiconTab)
                    {
                        if (victum.behaviorSpeculator != null && victum.behaviorSpeculator.AttackBehaviors != null && victum.behaviorSpeculator.AttackBehaviors.Count >= 1)
                        {

                            BotsModule.Log(entry.name + ": " + entry.myGuid, BotsModule.TEXT_COLOR);

                            //BotsModule.Log("attack list start", BotsModule.TEXT_COLOR);
                            idiots.Add(victum);
                            //BotsModule.Log("added to list", BotsModule.TEXT_COLOR);


                            var i = 0;
                            foreach (AttackBehaviorBase attack in victum.behaviorSpeculator.AttackBehaviors)
                            {
                                //BotsModule.Log("for each started", BotsModule.TEXT_COLOR);
                                idiotsBehaviors.Add(attack);
                                i++;
                            }

                            BotsModule.Log(victum.name + ": " + i, BotsModule.TEXT_COLOR);
                            if (victum.name == "BulletManVest")
                            {
                                break;
                            }
                        }
                    }

                }
               // BotsModule.Log("done");
            }
            BotsModule.Log("attack list made", BotsModule.TEXT_COLOR);
            foreach (AIActor targetIdiot in idiots)
            {
                BotsModule.Log("attack list foreach started", BotsModule.TEXT_COLOR);
                var idiotToBeGungeonModdered = idiots[UnityEngine.Random.Range(0, idiots.Count -1)];

                BotsModule.Log("target selected. target: " + idiotToBeGungeonModdered.name, BotsModule.TEXT_COLOR);


                if (idiotToBeGungeonModdered.CurrentGun != null)
                {

                    var m_CachedGunAttachPoint = targetIdiot.transform.Find("GunAttachPoint").gameObject;


                    EnemyBuilder.DuplicateAIShooterAndAIBulletBank(targetIdiot.gameObject, idiotToBeGungeonModdered.aiShooter, idiotToBeGungeonModdered.GetComponent<AIBulletBank>(), 151, m_CachedGunAttachPoint.transform, null, null);
                }
                else
                {
                    foreach (AIBulletBank.Entry bullet in idiotToBeGungeonModdered.bulletBank.Bullets)
                    {
                        targetIdiot.bulletBank.Bullets.Add(bullet);
                    }
                }


                BotsModule.Log("bullet bank updated", BotsModule.TEXT_COLOR);

                targetIdiot.behaviorSpeculator.AttackBehaviors = idiotToBeGungeonModdered.behaviorSpeculator.AttackBehaviors;

                targetIdiot.behaviorSpeculator.OverrideBehaviors = idiotToBeGungeonModdered.behaviorSpeculator.OverrideBehaviors;
                targetIdiot.behaviorSpeculator.OtherBehaviors = idiotToBeGungeonModdered.behaviorSpeculator.OtherBehaviors;


                //targetIdiot.aiShooter = idiotToBeGungeonModdered.aiShooter;

                BotsModule.Log("non attack behaviors set up", BotsModule.TEXT_COLOR);

                //foreach (BasicAttackBehavior stopTabbingOutToStartWatchingAnotherVideoEvery5MinutesYouFuckingIdiot in idiotToBeGungeonModdered.behaviorSpeculator.AttackBehaviors)
                //{
                //   targetIdiot.behaviorSpeculator.AttackBehaviors.Add(stopTabbingOutToStartWatchingAnotherVideoEvery5MinutesYouFuckingIdiot);
                //}

                BotsModule.Log("attack behaviors set up", BotsModule.TEXT_COLOR);

                BotsModule.Log(targetIdiot.name + " is now a modder thanks to " + idiotToBeGungeonModdered.name, BotsModule.TEXT_COLOR);
            }
        }
    }
}
