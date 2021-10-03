using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class ThrowableTest : SpawnObjectPlayerItem
    {
        public static void Init()
        {
            string itemName = "Beyond Bomb";

            string resourceName = "BotsMod/sprites/bomb/beyond_bomb_armed_001";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ThrowableTest>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "throw thing yes";
            string longDesc = "";


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
            item.consumable = false;

            item.objectToSpawn = BuildPrefab();

            item.tossForce = 4;
            item.canBounce = true;
            
            item.IsCigarettes = false;
            item.RequireEnemiesInRoom = false;

            item.SpawnRadialCopies = true;
            item.RadialCopiesToSpawn = 8;

            item.AudioEvent = "Play_OBJ_bomb_fuse_01";
            item.IsKageBunshinItem = false;

            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }


        public static GameObject BuildPrefab()
        {

            var bomb = SpriteBuilder.SpriteFromResource("BotsMod/sprites/bomb/beyond_bomb_armed_001.png", new GameObject("BeyondBomb"));
            bomb.SetActive(false);
            FakePrefab.MarkAsFakePrefab(bomb);

            var animator = bomb.AddComponent<tk2dSpriteAnimator>();
            var collection = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection;

            var deployAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {

                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_spawn_001.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_spawn_002.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_spawn_003.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_spawn_004.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_spawn_005.png", collection),

            }, "beyond_bomb_deploy", tk2dSpriteAnimationClip.WrapMode.Once);
            deployAnimation.fps = 12;
            foreach(var frame in deployAnimation.frames)
            {
                frame.eventLerpEmissiveTime = 0.5f;
                frame.eventLerpEmissivePower = 30f;
            }

            var explodeAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {

                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/bomb/beyond_bomb_explode_001.png", collection),

            }, "beyond_bomb_explode", tk2dSpriteAnimationClip.WrapMode.Once);
            explodeAnimation.fps = 30;
            foreach (var frame in explodeAnimation.frames)
            {
                frame.eventLerpEmissiveTime = 0.5f;
                frame.eventLerpEmissivePower = 30f;
            }

            var armedAnimation = SpriteBuilder.AddAnimation(animator, collection, new List<int>
            {

                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_arm_001.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_arm_002.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_arm_003.png", collection),
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/robotmine/roboticmine_arm_004.png", collection),

            }, "beyond_bomb_armed", tk2dSpriteAnimationClip.WrapMode.Once);
            armedAnimation.fps = 10.0f;
            armedAnimation.loopStart = 2;
            foreach (var frame in armedAnimation.frames)
            {
                frame.eventLerpEmissiveTime = 0.5f;
                frame.eventLerpEmissivePower = 30f;
            }

            var audioListener = bomb.AddComponent<AudioAnimatorListener>();
            audioListener.animationAudioEvents = new ActorAudioEvent[]
            {
                new ActorAudioEvent
                {
                    eventName = "Play_OBJ_mine_beep_01",
                    eventTag = "beep"
                }
            };

            ProximityMine proximityMine = new ProximityMine
            {
                explosionData = new ExplosionData
                {
                    useDefaultExplosion = false,
                    doDamage = true,
                    forceUseThisRadius = false,
                    damageRadius = 3.5f,
                    damageToPlayer = 0,
                    damage = 15,
                    breakSecretWalls = true,
                    secretWallsRadius = 3.5f,
                    forcePreventSecretWallDamage = false,
                    doDestroyProjectiles = true,
                    doForce = true,
                    pushRadius = 6,
                    force = 25,
                    debrisForce = 12.5f,
                    preventPlayerForce = false,
                    explosionDelay = 0.1f,
                    usesComprehensiveDelay = false,
                    comprehensiveDelay = 0,
                    playDefaultSFX = false,

                    doScreenShake = true,
                    ss = new ScreenShakeSettings
                    {
                        magnitude = 2,
                        speed = 6.5f,
                        time = 0.22f,
                        falloff = 0,
                        direction = new Vector2(0, 0),
                        vibrationType = ScreenShakeSettings.VibrationType.Auto,
                        simpleVibrationStrength = Vibration.Strength.Medium,
                        simpleVibrationTime = Vibration.Time.Normal
                    },
                    doStickyFriction = true,
                    doExplosionRing = true,
                    isFreezeExplosion = false,
                    freezeRadius = 5,
                    IsChandelierExplosion = false,
                    rotateEffectToNormal = false,
                    ignoreList = new List<SpeculativeRigidbody>(),
                    overrideRangeIndicatorEffect = null,
                    effect = (PickupObjectDatabase.GetById(108) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<ProximityMine>().explosionData.effect,
                    freezeEffect = null,
                },
                explosionStyle = ProximityMine.ExplosiveStyle.TIMED,
                detectRadius = 3.5f,
                explosionDelay = 0.5f,
                usesCustomExplosionDelay = false,
                customExplosionDelay = 0.1f,
                deployAnimName = "beyond_bomb_deploy",
                explodeAnimName = "",
                idleAnimName = "beyond_bomb_armed",

                

                MovesTowardEnemies = true,
                HomingTriggeredOnSynergy = false,
                HomingDelay = 3.25f,
                HomingRadius = 10,
                HomingSpeed = 4,

            };

            var boom = bomb.AddComponent<ProximityMine>(proximityMine);


            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = boom.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
            mat.SetFloat("_EmissiveColorPower", 1.55f);
            mat.SetFloat("_EmissivePower", 100);
            boom.GetComponent<MeshRenderer>().material = mat;
            //overseerShield.GetComponent<tk2dSprite

            //SpriteOutlineManager.AddOutlineToSprite(boom.GetComponent<tk2dSprite>(), Color.black);

            return bomb;
        }
    }
}
