using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod.NightmareNightmareNightmare
{
    class NoMoreFun
    {
        public static bool isEnabled = false;

        public static void Init()
        {
            var CurseOfTheBlind = new Hook(
                    typeof(DebrisObject).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(NoMoreFun).GetMethod("AwakeHook", BindingFlags.Static | BindingFlags.Public));
        }
        public delegate TResult Func<T, T2, T3, T4, T5, T6, T7, TResult>(T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);


        public static void AwakeHook(Action<DebrisObject> orig, DebrisObject self)
        {
            if (self.gameObject.GetPickupObjectFromAnywhere() != null && isEnabled)
            {
                var sprite = self.gameObject.GetPickupObjectFromAnywhere().sprite;
                if (sprite)
                {
                    if (sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite") == 0)
                    {
                        var id = ItemAPI.SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/curse_of_the_blind_sprite.png", sprite.Collection, "curse_of_the_blind_sprite");
                        sprite.SetSprite(id);
                    }
                    else
                    {
                        sprite.SetSprite(sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite"));
                    }
                }
            }

            orig(self);
        }

        

        public static DebrisObject SpawnItemHook(Func<GameObject, Vector3, Vector2, float, bool, bool, bool, DebrisObject> orig, GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disableHeightBoost = false)
        {
            if (isEnabled)
            {
                if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
                {
                    PickupObject component = item.GetComponent<PickupObject>();
                    if (component && component.PickupObjectId == GlobalItemIds.UnfinishedGun)
                    {
                        item = PickupObjectDatabase.GetById(GlobalItemIds.FinishedGun).gameObject;
                    }
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, spawnPosition, Quaternion.identity);

                var sprite = gameObject.GetComponent<tk2dSprite>();
                if (sprite)
                {
                    if (sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite") == 0)
                    {
                        var id = ItemAPI.SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/curse_of_the_blind_sprite.png", sprite.Collection, "curse_of_the_blind_sprite");
                        sprite.SetSprite(id);
                    }
                    else
                    {
                        sprite.SetSprite(sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite"));
                    }
                }

                gameObject.AddComponent<CurseOfTheBlind>();
                GameObject spawnedItem = gameObject;
                
                return (DebrisObject)typeof(LootEngine).GetMethod("SpawnInternal", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { spawnedItem, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, false, disableHeightBoost });

            }
            else
            {
                return orig(item, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, disableHeightBoost);
            }

            
        }

    }
    class CurseOfTheBlind : BraveBehaviour
    {
        void Start()
        {
            if (this.sprite)
            {
                if (this.sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite") == 0)
                {
                    var id = ItemAPI.SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/curse_of_the_blind_sprite.png", this.sprite.Collection, "curse_of_the_blind_sprite");
                    this.sprite.SetSprite(id);
                } 
                else
                {
                    this.sprite.SetSprite(this.sprite.Collection.GetSpriteIdByName("curse_of_the_blind_sprite"));
                }
            }
        }
    }
}
