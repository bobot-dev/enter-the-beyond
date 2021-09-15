using Brave.BulletScript;
using CustomCharacters;
using FrostAndGunfireItems;
using Gungeon;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class ChestInitStuff
    {
        public static void Init()
        {
            InitKeyChest();
            CreateKeyMimicPrefab();
        }
        static Texture2D texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\paradox_test.png");
        public static void InitKeyChest()
        {

            GameObject chestAnimator = Tools.shared_auto_001.LoadAsset<GameObject>("Chest_Animation");


            var spriteAnimator = chestAnimator.GetComponent<tk2dSpriteAnimation>();

            var animations = spriteAnimator.clips.ToList();

            var collection = animations[0].frames[0].spriteCollection;

            var idList = new List<int>();

            foreach (var frame in spriteAnimator.GetClipByName("green_chest_appear").frames)
            {
                var spriteDef = frame.spriteCollection.spriteDefinitions[frame.spriteId];
                var copy = spriteDef.Copy();
                copy.ReplaceTexture(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/chest/" + spriteDef.name.Replace("green", "beyond") + ".png"));
                copy.name = spriteDef.name.Replace("green", "beyond");
                idList.Add(SpriteHandler.AddSpriteToCollection(copy, collection));
                BotsModule.Log(copy.name);
            }



            animations.Add(new tk2dSpriteAnimationClip
            {
                fps = 11,
                loopStart = 0,
                maxFidgetDuration = 2,
                minFidgetDuration = 1,
                wrapMode = tk2dSpriteAnimationClip.WrapMode.Once,
                name = "beyond_chest_appear",

                frames = new tk2dSpriteAnimationFrame[]
                {
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "Play_OBJ_smallchest_spawn_01",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[0],
                       spriteCollection = collection

                   },

                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[1],
                       spriteCollection = collection

                   },

                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[2],
                       spriteCollection = collection

                   },

                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[3],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[4],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[5],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[6],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[7],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[8],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[9],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[10],
                       spriteCollection = collection

                   },
                   new tk2dSpriteAnimationFrame
                   {
                       invulnerableFrame = false,
                       groundedFrame = true,
                       requiresOffscreenUpdate = false,
                       eventAudio = "",
                       eventVfx = null,
                       eventStopVfx = null,
                       eventLerpEmissive = false,
                       eventLerpEmissivePower = 30.0f,
                       eventLerpEmissiveTime = 0.5f,
                       forceMaterialUpdate = false,
                       finishedSpawning = false,
                       triggerEvent = true,
                       eventInfo = "",
                       eventInt = 0,
                       eventFloat = 0,
                       eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified,
                       spriteId = idList[11],
                       spriteCollection = collection

                   },
                }

            });


            var chestObj = FakePrefab.Clone(GameManager.Instance.RewardManager.B_Chest.gameObject);
            chestObj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(chestObj);
            UnityEngine.Object.DontDestroyOnLoad(chestObj);

            var chest = chestObj.GetComponent<Chest>();

            chest.spawnAnimName = "beyond_chest_appear";


            /*if (chest != null && chest.sprite != null && chest.sprite.renderer != null && chest.sprite.renderer.material != null)
			{

				chest.sprite.usesOverrideMaterial = true;

				chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
				chest.sprite.renderer.material.SetTexture("_EeveeTex", texture);

				chest.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
				chest.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");



			}*/

            chest.lootTable.overrideItemLootTables.Clear();
			chest.lootTable.lootTable = ItemBuilder.LoadShopTable("Shop_Key_Items_01");
			chest.ChestIdentifier = (Chest.SpecialChestIdentifier)CustomEnums.CustomSpecialChestIdentifier.TEST;
			chest.ChestType = Chest.GeneralChestType.UNSPECIFIED;

			chest.lootTable.S_Chance = 0.1f;
			chest.lootTable.A_Chance = 0.1f;
			chest.lootTable.B_Chance = 0.2f;
			chest.lootTable.C_Chance = 0.4f;
			chest.lootTable.D_Chance = 0.5f;


			
            
            //chest.overrideMimicChance = 100f;
            chest.MimicGuid = "key_chest_mimic";

            BotsModule.KeyChest = chest;
        }
        public static void CreateKeyMimicPrefab()
        {
            AIActor aiactor = UnityEngine.Object.Instantiate<AIActor>(EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8"));
            aiactor.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(aiactor.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(aiactor);
            //aiactor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
            MeshRenderer component = aiactor.GetComponent<MeshRenderer>();
            Material[] sharedMaterials = component.sharedMaterials;
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);


            Material material = UnityEngine.Object.Instantiate<Material>(new Material(ShaderCache.Acquire("Brave/PlayerShaderEevee")));


            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            material.SetTexture("_EeveeTex", texture);

            material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");


            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;

            

            AIActor greenMimic = EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8");

            aiactor.behaviorSpeculator.AttackBehaviors.Clear();
            aiactor.behaviorSpeculator.AttackBehaviors.Add(new MimicAwakenBehavior
            {
                awakenAnim = "intro",
                ShootPoint = new GameObject(),
                BulletScript = new CustomBulletScriptSelector(typeof(KeyMimicIntroFire1)),
                Cooldown = 0,
                GlobalCooldown = 0,
                InitialCooldown = 0,
                GroupName = "",
                GroupCooldown = 0,
                MinRange = 0,
                Range = 0,
                MinWallDistance = 0,
                MaxEnemiesInRoom = 0,
                MinHealthThreshold = 0,
                MaxHealthThreshold = 0,
                AccumulateHealthThresholds = true,
                targetAreaStyle = null,
                IsBlackPhantom = false,
                resetCooldownOnDamage = null,
                RequiresLineOfSight = false,
                MaxUsages = 1,
            });
            var hand = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/GunHandController.json");
            GunHandController newHand = new GunHandController();
            JsonUtility.FromJsonOverwrite(hand, newHand);

            var bulllet = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/Projectile.json");
            Projectile newBullet = new Projectile();
            JsonUtility.FromJsonOverwrite(bulllet, newBullet);
            var gunHandThing = new GunHandBasicShootBehavior
            {
                LineOfSight = true,
                FireAllGuns = true,
                GunHands = new List<GunHandController>
                {
                    new GunHandController
                    {
                        GunId = 95,
                        OverrideProjectile = newBullet,
                        UsesOverrideProjectileData = true,
                        OverrideProjectileData = new ProjectileData
                        {
                            damage = 10,
                            speed = 2,
                            range = 50,
                            force = 10,
                            damping = 0,
                            UsesCustomAccelerationCurve = false,
                            CustomAccelerationCurveDuration = 1,
                        },

                        GunFlipMaster = newHand,
                        handObject = new PlayerHandController
                        {
                            handHeightFromGun = 0.05f,
                            ForceRenderersOff = false,                   
                        },
                        isEightWay = false,
                        gunBehindBody = new GunHandController.DirectionalAnimationBoolSixWay
                        {
                            Back = true,
                            BackRight = true,
                            ForwardRight = false,
                            Forward = false,
                            ForwardLeft = true,
                            BackLeft = true
                        },
                        gunBehindBodyEight = new GunHandController.DirectionalAnimationBoolEightWay
                        {
                            East = false,
                            North = false,
                            NorthEast = false,
                            NorthWest = false,
                            South = false,
                            SouthEast = false,
                            SouthWest = false,
                            West = false,
                        },
                        PreFireDelay = 0,
                        NumShots = 15,
                        ShotCooldown = 0.11f,
                        Cooldown = 3,
                        RampBullets = false,
                        RampStartHeight = 2,
                        RampTime = 1,

                    },
                    new GunHandController
                    {
                        GunId = 726,
                        OverrideProjectile = newBullet,
                        UsesOverrideProjectileData = true,
                        OverrideProjectileData = new ProjectileData
                        {
                            damage = 10,
                            speed = 2,
                            range = 50,
                            force = 10,
                            damping = 0,
                            UsesCustomAccelerationCurve = false,
                            CustomAccelerationCurveDuration = 1,
                        },
                        
                        GunFlipMaster = newHand,
                        handObject = new PlayerHandController
                        {
                            handHeightFromGun = 0.05f,
                            ForceRenderersOff = false,
                        },
                        isEightWay = false,
                        gunBehindBody = new GunHandController.DirectionalAnimationBoolSixWay
                        {
                            Back = true,
                            BackRight = true,
                            ForwardRight = false,
                            Forward = false,
                            ForwardLeft = true,
                            BackLeft = true
                        },
                        gunBehindBodyEight = new GunHandController.DirectionalAnimationBoolEightWay
                        {
                            East = false,
                            North = false,
                            NorthEast = false,
                            NorthWest = false,
                            South = false,
                            SouthEast = false,
                            SouthWest = false,
                            West = false,
                        },
                        PreFireDelay = 0,
                        NumShots = 15,
                        ShotCooldown = 0.11f,
                        Cooldown = 3,
                        RampBullets = false,
                        RampStartHeight = 2,
                        RampTime = 1,
                    }
                },
                Cooldown = 1,
                CooldownVariance = 0,
                AttackCooldown = 0,
                GlobalCooldown = 0,
                GroupCooldown = 0,
                InitialCooldown = 0,
                InitialCooldownVariance = 0,
                GroupName = "",
                MinRange = 0,
                Range = 0,
                MinWallDistance = 0,
                MaxEnemiesInRoom = 0,
                MinHealthThreshold = 0f,
                MaxHealthThreshold = 1f,
                HealthThresholds = new float[0],

                AccumulateHealthThresholds = true,


                RequiresLineOfSight = false,
            };
            
            aiactor.behaviorSpeculator.AttackBehaviors.Add(gunHandThing);

                BotsModule.Log(gunHandThing.IsReady() +"");
            


            aiactor.behaviorSpeculator.MovementBehaviors.Clear();
            aiactor.behaviorSpeculator.MovementBehaviors.Add(new MoveErraticallyBehavior
            {
                PathInterval = 0.2f,
                PointReachedPauseTime = 0.4f,
                PreventFiringWhileMoving = false,
                InitialDelay = 0,
                StayOnScreen = true,
                AvoidTarget = true,
                UseTargetsRoom = true,
            });
            aiactor.behaviorSpeculator.TargetBehaviors.Clear();
            aiactor.behaviorSpeculator.TargetBehaviors.Add(new TargetPlayerBehavior
            {
                Radius = 35,
                LineOfSight = true,
                ObjectPermanence = true,
                SearchInterval = 0.25f,
                PauseOnTargetSwitch = false,
                PauseTime = 0.25f
            });



            if (aiactor.healthHaver != null)
            {
                aiactor.healthHaver.SetHealthMaximum(150, null, true);
            }
            if (aiactor.bulletBank && aiactor.bulletBank.aiActor && aiactor.bulletBank.aiActor.TargetRigidbody)
            {
                //base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
                aiactor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8").bulletBank.GetBullet("bigBullet"));
                aiactor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8").bulletBank.GetBullet("8x8_enemy_projectile_001_dark"));
                aiactor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8").bulletBank.GetBullet("10x10_enemy_projectile_001_dark"));
            }

            aiactor.ActorName = "Key Chest Mimic";
            aiactor.EnemyGuid = "key_chest_mimic";
            Tools.AddEnemyToDatabase(aiactor.gameObject, "key_chest_mimic", false, true, true);

        }

        public class KeyMimicIntroFire1 : Script
        {

           
            
            protected override IEnumerator Top()
            {
                if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
                {
                    //base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
                    base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("abfb454340294a0992f4173d6e5898a8").bulletBank.GetBullet("bigBullet"));
                }
                bool isBlackPhantom = base.BulletBank && base.BulletBank.aiActor && base.BulletBank.aiActor.IsBlackPhantom;
                base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new MimicIntroFire1.BigBullet(isBlackPhantom));
                return null;
            }

            // Token: 0x0200028E RID: 654
            public class BigBullet : Bullet
            {
                
                public BigBullet(bool isBlackPhantom) : base("bigbullet", false, false, false)
                {
                    base.ForceBlackBullet = true;
                    this.m_isBlackPhantom = isBlackPhantom;
                }

                
                protected override IEnumerator Top()
                {
                    yield return this.Wait(80);
                    this.Vanish(false);
                    yield break;
                }

                
                public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
                {
                    if (preventSpawningProjectiles)
                    {
                        return;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        Bullet bullet = new SpeedChangingBullet(10f, 120, 600);
                        base.Fire(new Direction((float)(i * 45), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), bullet);
                        if (!this.m_isBlackPhantom)
                        {
                            bullet.ForceBlackBullet = false;
                            bullet.Projectile.ForceBlackBullet = false;
                            bullet.Projectile.ReturnFromBlackBullet();
                        }
                    }
                }

                // Token: 0x04000A4F RID: 2639
                private bool m_isBlackPhantom;
            }
        }

    }
}
