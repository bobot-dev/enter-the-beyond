using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    public class Marked : GameActorEffect
    {
        public Marked() 
        {
            overheadVFX = ItemAPI.SpriteBuilder.SpriteFromResource("BotsMod/sprites/chaineddebufftemp");
            defaultMarked = new Marked
            {
                effectIdentifier = "MarkedForChain",
                AffectsEnemies = true,
                AffectsPlayers = false,
                stackMode = EffectStackingMode.Refresh,
                maxStackedDuration = 10,
                //OverheadVFX = overheadVFX,
                AppliesTint = true,
                TintColor = new Color32(94, 94, 94, 255),
                AppliesDeathTint = true,
                DeathTintColor = new Color32(48, 48, 48, 255),
                duration = 10,
                PlaysVFXOnActor = true,
                AppliesOutlineTint = false,
                chainedTo = new List<AIActor>(),
                OutlineTintColor = Color.white,
                resistanceType = EffectResistanceType.None
            };
           
        }

        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {

            base.OnEffectApplied(actor, effectData, partialAmount);

            if (actor.healthHaver)
            {
                //actor.healthHaver.ModifyDamage += SplitDamage;
            }
            if (actor.specRigidbody)
            {
                /*GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(actor.specRigidbody.UnitCenter.ToIntVector2()).ApplyActionToNearbyEnemies(actor.specRigidbody.UnitCenter, 4, delegate (AIActor enemy, float dist)
                {
                    if (enemy != actor)
                    {
                        chainedTo.Add(enemy);
                        //if (enemy.healthHaver)
                        //{
                        //    //enemy.healthHaver.OnPreDeath += HealthHaver_OnPreDeath;
                        //}
                    }
                    
                });*/
            }
            

            
        }

        private void HealthHaver_OnPreDeath(Vector2 obj)
        {
            
        }

        void SplitDamage(HealthHaver hh, HealthHaver.ModifyDamageEventArgs args)
        {
            args.ModifiedDamage = args.InitialDamage / chainedTo.Count;
            //foreach(AIActor enemy in chainedTo)
            //{
            //    if (enemy.healthHaver && enemy.healthHaver != hh)
            //    {
            //        //enemy.healthHaver.ApplyDamage(args.ModifiedDamage, Vector2.zero, "chained");
             //   }
            //}
        }

        public List<AIActor> chainedTo = new List<AIActor>();
        public static GameObject overheadVFX;
        public static Marked defaultMarked;
    }
}
