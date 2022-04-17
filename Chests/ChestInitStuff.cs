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
using PrefabAPI;

namespace BotsMod
{
    class ChestInitStuff
    {
        public static void Init()
        {
            //InitKeyChest();
            //CreateKeyMimicPrefab();
        }
        static Texture2D texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\paradox_test.png");
        public static GameObject InitChest(string name, string id, string idleSpritePath, List<string> openSpritePaths, int openFps, List<string> appearSpritePaths, int appearFps, List<string> breakSpritePaths, int breakFps, string minimapIconSpritePath, List<string> lockIdlePaths, int lockIdleFps,
            List<string> lockOpenPaths, int lockOpenFps, List<string> lockBrokenPaths, int lockBrokenFps, List<string> lockNoKeyPaths, int lockNoKeyFps, List<int> forcedIds, LootData lootData, LootData brokenLootData, tk2dSprite shadowSprite, int overrideJunkId, GameObject preSpawnVfx,
            GameObject groundHitVfx, float groundHitDelay)
        {

            var spriteAnimator = Tools.shared_auto_001.LoadAsset<GameObject>("Chest_Animation").GetComponent<tk2dSpriteAnimation>(); ;


            var collection = spriteAnimator.clips[0].frames[0].spriteCollection;

            var idList = new List<int>();

            BotsModule.Log("c0");
            //var chestObj = PrefabBuilder.BuildObject($"{id}:{name}_chest");
            var chestObj = FakePrefab.Clone(GameManager.Instance.RewardManager.C_Chest.gameObject);
            chestObj.SetActive(false);
            chestObj.name = $"{id}:{name}_chest";
            var idleSpriteId = SpriteBuilder.AddSpriteToCollection(idleSpritePath, collection, $"{id}:{name}_chest_idle_001");
            chestObj.GetComponent<tk2dSprite>().SetSprite(collection, idleSpriteId);

            //SpriteBuilder.SpriteFromResource(idleSpritePath, chestObj);


            var chestAnimator = chestObj.GetComponent<tk2dSpriteAnimator>();
            chestAnimator.Library = spriteAnimator;

            /*foreach (var sprite in appearSpritePaths)
            {
                idList.Add(idleSpriteId);
            }

            SpriteHandler.AddAnimation(chestAnimator, collection, idList, $"{id}_{name}_chest_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 15);
            chestAnimator.DefaultClipId = chestAnimator.GetClipIdByName($"{id}_{name}_chest_idle");
            
            idList.Clear();*/
            //chestAnimator.playAutomatically = true;



            foreach (var sprite in appearSpritePaths)
            {
                idList.Add(SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection));
            }
            var a = SpriteHandler.AddAnimation(chestAnimator, collection, idList, $"{id}_{name}_chest_appear", tk2dSpriteAnimationClip.WrapMode.Once, appearFps);
            a.AddAudioEventByFrame(0, "Play_OBJ_smallchest_spawn_01");


            collection.SetAttachPoints(a.frames[0].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(100, 2f) } });
            collection.SetAttachPoints(a.frames[1].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(100, 2f) } });
            collection.SetAttachPoints(a.frames[2].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(100, 2f) } });
            collection.SetAttachPoints(a.frames[3].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(100f, 2f) } });
            collection.SetAttachPoints(a.frames[4].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 2f) } });

            collection.SetAttachPoints(a.frames[5].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 2f) } });
            collection.SetAttachPoints(a.frames[6].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 2f) } });
            collection.SetAttachPoints(a.frames[7].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 2f) } });
            collection.SetAttachPoints(a.frames[8].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 2f) } });


            collection.SetAttachPoints(a.frames[9].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 0.25f) } });
            collection.SetAttachPoints(a.frames[10].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 0.5f) } });
            collection.SetAttachPoints(a.frames[11].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 0.25f) } });
            collection.SetAttachPoints(a.frames[12].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 0.3125f) } });
            collection.SetAttachPoints(a.frames[13].spriteId, new tk2dSpriteDefinition.AttachPoint[] { new tk2dSpriteDefinition.AttachPoint { angle = 0, name = "lock", position = new Vector3(0.8125f, 0.25f) } });

            idList.Clear();

            foreach (var sprite in openSpritePaths)
            {
                idList.Add(SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection));
            }
            a = SpriteHandler.AddAnimation(chestAnimator, collection, idList, $"{id}_{name}_chest_open", tk2dSpriteAnimationClip.WrapMode.Once, openFps);
            a.frames[19].triggerEvent = true;
            a.frames[19].eventLerpEmissive = true;
            a.frames[19].eventLerpEmissivePower = 0;
            a.frames[19].eventLerpEmissiveTime = 0.083f;
            idList.Clear();

            
           foreach (var sprite in breakSpritePaths)
           {
                var sid = SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection);
                collection.spriteDefinitions[sid].position0 += new Vector3(-0.125f, -0.25f, 0);
                collection.spriteDefinitions[sid].position1 += new Vector3(-0.125f, -0.25f, 0);
                collection.spriteDefinitions[sid].position2 += new Vector3(-0.125f, -0.25f, 0);
                collection.spriteDefinitions[sid].position3 += new Vector3(-0.125f, -0.25f, 0);
                idList.Add(sid);
            }
           SpriteHandler.AddAnimation(chestAnimator, collection, idList, $"{id}_{name}_chest_break", tk2dSpriteAnimationClip.WrapMode.Once, breakFps);
           idList.Clear();
           



            BotsModule.Log("c1");

           var attachPoint = chestObj.GetComponent<tk2dSpriteAttachPoint>();
           attachPoint.attachPoints = new List<Transform> { chestObj.transform.Find("lock") };
           /*
           attachPoint.deactivateUnusedAttachPoints = false;
           attachPoint.disableEmissionOnUnusedParticleSystems = false;
           attachPoint.ignorePosition = false;
           attachPoint.ignoreRotation = false;
           attachPoint.ignoreScale = false;
           attachPoint.centerUnusedAttachPoints = false;
           var a = new GameObject("lockAttachPoint").transform;
           a.parent = chestObj.transform;
           attachPoint.attachPoints = new List<Transform>
           {
               a
           };

           var majorBreakable = chestObj.AddComponent<MajorBreakable>();

           majorBreakable.HitPoints = 40;
           majorBreakable.DamageReduction = 0;
           majorBreakable.MinHits = 0;
           majorBreakable.EnemyDamageOverride = -1;
           majorBreakable.ImmuneToBeastMode = false;
           majorBreakable.ScaleWithEnemyHealth = false;
           majorBreakable.OnlyExplosions = false;
           majorBreakable.IgnoreExplosions = false;
           majorBreakable.GameActorMotionBreaks = false;
           majorBreakable.spawnShards = true;
           majorBreakable.distributeShards = false;
           majorBreakable.shardClusters = new ShardCluster[0];
           majorBreakable.minShardPercentSpeed = 0.05f;
           majorBreakable.maxShardPercentSpeed = 0.3f;
           majorBreakable.shardBreakStyle = MinorBreakable.BreakStyle.CONE;
           majorBreakable.usesTemporaryZeroHitPointsState = true;
           majorBreakable.spriteNameToUseAtZeroHP = $"{id}_{name}_chest_break_001";
           majorBreakable.destroyedOnBreak = false;
           majorBreakable.childrenToDestroy = new List<GameObject>();
           majorBreakable.playsAnimationOnNotBroken = false;
           majorBreakable.notBreakAnimation = "";
           majorBreakable.handlesOwnBreakAnimation = false;
           majorBreakable.breakAnimation = "";
           majorBreakable.handlesOwnPrebreakFrames = false;
           majorBreakable.prebreakFrames = new BreakFrame[0];
           majorBreakable.damageVfx = new VFXPool { effects = new VFXComplex[0], type = VFXPoolType.None };
           majorBreakable.damageVfxMinTimeBetween = 0.2f;
           majorBreakable.breakVfx = new VFXPool { effects = new VFXComplex[0], type = VFXPoolType.None };
           majorBreakable.delayDamageVfx = false;
           majorBreakable.SpawnItemOnBreak = false;
           majorBreakable.ItemIdToSpawnOnBreak = -1;
           majorBreakable.HandlePathBlocking = false;






           var rigidbody = chestObj.AddComponent<SpeculativeRigidbody>();

           rigidbody.PixelColliders = GameManager.Instance.RewardManager.C_Chest.specRigidbody.PixelColliders;
           */
            var chest = chestObj.GetComponent<Chest>();

            //chest.placeableHeight = 1;
            //chest.placeableWidth = 3;
            //chest.difficulty = 0;
            //chest.isPassable = true;
            //chest.ChestIdentifier = Chest.SpecialChestIdentifier.NORMAL;
            if (forcedIds != null) chest.forceContentIds = forcedIds;
            /*if (chest != null && chest.sprite != null && chest.sprite.renderer != null && chest.sprite.renderer.material != null)
			{

				chest.sprite.usesOverrideMaterial = true;

				chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
				chest.sprite.renderer.material.SetTexture("_EeveeTex", texture);

				chest.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
				chest.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");



			}*/
            BotsModule.Log("c2");
            chest.lootTable = lootData;
            BotsModule.Log("c2.5");
            chest.breakertronLootTable = brokenLootData;
            BotsModule.Log("c3");
            chest.ShadowSprite = shadowSprite;

            chest.breakAnimName = $"{id}_{name}_chest_break";
            chest.openAnimName = $"{id}_{name}_chest_open";
            chest.spawnAnimName = $"{id}_{name}_chest_appear";

            chest.gameObject.GetComponent<MajorBreakable>().spriteNameToUseAtZeroHP = collection.spriteDefinitions[chestAnimator.GetClipByName(chest.breakAnimName).frames[0].spriteId].name;

            BotsModule.Log("c4");
            chest.overrideJunkId = overrideJunkId;
            chest.VFX_PreSpawn = preSpawnVfx;
            chest.VFX_GroundHit = groundHitVfx;
            chest.groundHitDelay = groundHitDelay;

            chest.MinimapIconPrefab = SpriteBuilder.SpriteFromResource(minimapIconSpritePath);
            UnityEngine.Object.DontDestroyOnLoad(chest.MinimapIconPrefab);
            FakePrefab.MarkAsFakePrefab(chest.MinimapIconPrefab);

            BotsModule.Log("c5");

            //chest.LockAnimator = GameManager.Instance.RewardManager.C_Chest.LockAnimator;
            collection = chest.LockAnimator.Library.clips[0].frames[0].spriteCollection;
            foreach (var sprite in lockIdlePaths)
            {
                var sid = SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection);
                idList.Add(sid);
            }
            SpriteHandler.AddAnimation(chest.LockAnimator, collection, idList, $"{id}_{name}_lock_idle", tk2dSpriteAnimationClip.WrapMode.Loop, lockIdleFps);
            chest.LockAnimator.playAutomatically = true;
            chest.LockAnimator.DefaultClipId = chest.LockAnimator.GetClipIdByName($"{id}_{name}_lock_idle");
            idList.Clear();

            foreach (var sprite in lockBrokenPaths)
            {
                var sid = SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection);
                collection.spriteDefinitions[sid].position0 += new Vector3(-0.1875f, -0.1875f);
                collection.spriteDefinitions[sid].position1 += new Vector3(-0.1875f, -0.1875f);
                collection.spriteDefinitions[sid].position2 += new Vector3(-0.1875f, -0.1875f);
                collection.spriteDefinitions[sid].position3 += new Vector3(-0.1875f, -0.1875f);
                idList.Add(sid);
            }
            SpriteHandler.AddAnimation(chest.LockAnimator, collection, idList, $"{id}_{name}_lock_broken", tk2dSpriteAnimationClip.WrapMode.Once, lockBrokenFps);
            idList.Clear();

            //theres some very unfunny joke here with the funny blue man with a big head raising his eyebrow and saying no keys? isnt there
            foreach (var sprite in lockNoKeyPaths)
            {
                var sid = SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection);
                idList.Add(sid);
            }
            SpriteHandler.AddAnimation(chest.LockAnimator, collection, idList, $"{id}_{name}_lock_no_key", tk2dSpriteAnimationClip.WrapMode.Once, lockNoKeyFps);
            idList.Clear();

            foreach (var sprite in lockOpenPaths)
            {
                var sid = SpriteHandler.AddSpriteToCollection(ItemAPI.ResourceExtractor.GetTextureFromResource(sprite), collection);
                idList.Add(sid);
            }
            SpriteHandler.AddAnimation(chest.LockAnimator, collection, idList, $"{id}_{name}_lock_open", tk2dSpriteAnimationClip.WrapMode.Once, lockOpenFps);
            idList.Clear();

            chest.LockBreakAnim = $"{id}_{name}_lock_broken";
            chest.LockNoKeyAnim = $"{id}_{name}_lock_no_key";
            chest.LockOpenAnim = $"{id}_{name}_lock_open";

            chest.sprite = chestObj.GetComponent<tk2dSprite>();
            chest.spriteAnimator = chestAnimator;

            BotsModule.Log("c6");

            chest.overrideMimicChance = 0;

            
            Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            

            material.SetTexture("_MainTex", chest.sprite.renderer.material.GetTexture("_MainTex"));
            material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
            material.SetFloat("_EmissiveColorPower", 1.55f);
            material.SetFloat("_EmissivePower", 55);

            chest.sprite.renderer.material = material;
            chest.sprite.usesOverrideMaterial = true;

            Material material2 = new Material(material);

            material2.SetTexture("_MainTex", chest.LockAnimator.renderer.material.GetTexture("_MainTex"));
            material2.SetFloat("_EmissiveColorPower", 0.725f);

            chest.LockAnimator.renderer.material = material;
            chest.LockAnimator.sprite.usesOverrideMaterial = true;

            return chest.gameObject;
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
