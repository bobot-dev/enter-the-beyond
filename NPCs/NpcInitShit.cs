using Dungeonator;
using HutongGames.PlayMaker;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod.NPCs
{
    class NpcInitShit
    {

        #region shopnpc
        public static void Init()
        {
            try
            {
                List<string> testNpcIdleSprites = new List<string>
                {
                    "shopkeep_001.png",
                    "shopkeep_002.png",
                    "shopkeep_003.png",
                    "shopkeep_004.png",
                    "shopkeep_005.png",
                    "shopkeep_006.png",
                    "shopkeep_007.png",
                    "shopkeep_008.png",
                    "shopkeep_009.png",
                };

                List<string> testNpcTalkSprites = new List<string>
                {
                    "shopkeep_talk_001.png",
                    "shopkeep_talk_002.png",
                    "shopkeep_talk_003.png",
                    "shopkeep_talk_004.png",
                    "shopkeep_talk_005.png",
                };

                //BotsModule.Log("anim lists done");

                var SpeechPoint = new GameObject("SpeechPoint");
                SpeechPoint.transform.position = new Vector3(0.8125f, 2.1875f, -1.31f);



                var npcObj = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Npcs/Beyond/shopkeep_001.png", new GameObject("Bot:Test_Npc"));

                npcObj.layer = 22;

                var collection = npcObj.GetComponent<tk2dSprite>().Collection;
                SpeechPoint.transform.parent = npcObj.transform;

                FakePrefab.MarkAsFakePrefab(SpeechPoint);
                UnityEngine.Object.DontDestroyOnLoad(SpeechPoint);
                SpeechPoint.SetActive(true);

                //-2729308948368026681
                //-2729308948368026681

                var idleIdsList = new List<int>();
                var talkIdsList = new List<int>();

                foreach (string sprite in testNpcIdleSprites)
                {
                    idleIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Beyond/" + sprite, collection));
                }

                foreach (string sprite in testNpcTalkSprites)
                {
                    talkIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Beyond/" + sprite, collection));
                }


                //BotsModule.Log("anim lists done fr this time");

                tk2dSpriteAnimator spriteAnimator = npcObj.AddComponent<tk2dSpriteAnimator>();

                SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "beyond_npc_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 6);
                SpriteBuilder.AddAnimation(spriteAnimator, collection, talkIdsList, "beyond_npc_talk", tk2dSpriteAnimationClip.WrapMode.LoopSection, 8).loopStart = 3;

                SpeculativeRigidbody rigidbody = Tools.GenerateOrAddToRigidBody(npcObj, CollisionLayer.BulletBlocker, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(20, 18), new IntVector2(5, 0));

                //BotsModule.Log("rigidbody done");

               

                //BotsModule.Log("talkPoint done");

                TalkDoerLite talkDoer = npcObj.AddComponent<TalkDoerLite>();

                talkDoer.placeableWidth = 4;
                talkDoer.placeableHeight = 3;
                talkDoer.difficulty = 0;
                talkDoer.isPassable = true;
                talkDoer.usesOverrideInteractionRegion = false;
                talkDoer.overrideRegionOffset = Vector2.zero;
                talkDoer.overrideRegionDimensions = Vector2.zero;
                talkDoer.overrideInteractionRadius = -1;
                talkDoer.PreventInteraction = false;
                talkDoer.AllowPlayerToPassEventually = true;
                talkDoer.speakPoint = SpeechPoint.transform;
                talkDoer.SpeaksGleepGlorpenese = false;
                talkDoer.audioCharacterSpeechTag = "oldman";
                talkDoer.playerApproachRadius = 5;
                talkDoer.conversationBreakRadius = 5;
                talkDoer.echo1 = null;
                talkDoer.echo2 = null;
                talkDoer.PreventCoopInteraction = false;
                talkDoer.IsPaletteSwapped = false;
                talkDoer.PaletteTexture = null;
                talkDoer.OutlineDepth = 0.5f;
                talkDoer.OutlineLuminanceCutoff = 0.05f;
                talkDoer.MovementSpeed = 3;
                talkDoer.PathableTiles = CellTypes.FLOOR;

                //BotsModule.Log("TalkDoerLite done");

                UltraFortunesFavor dreamLuck = npcObj.AddComponent<UltraFortunesFavor>();

                dreamLuck.goopRadius = 2;
                dreamLuck.beamRadius = 2;
                dreamLuck.bulletRadius = 2;
                dreamLuck.bulletSpeedModifier = 0.8f;

                dreamLuck.vfxOffset = 0.625f;
                dreamLuck.sparkOctantVFX = Tools.shared_auto_001.LoadAsset<GameObject>("FortuneFavor_VFX_Spark");
                
                //BotsModule.Log("UltraFortunesFavor done");


                AIAnimator aIAnimator = Tools.GenerateBlankAIAnimator(npcObj);
                
                aIAnimator.IdleAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "beyond_npc_idle",
                    AnimNames = new string[]
                    {
                        ""
                    },
                    Flipped = new DirectionalAnimation.FlipType[]
                    {
                        DirectionalAnimation.FlipType.None
                    }
                    
                };

                aIAnimator.TalkAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "beyond_npc_talk",
                    AnimNames = new string[]
                    {
                        ""
                    },
                    Flipped = new DirectionalAnimation.FlipType[]
                    {
                        DirectionalAnimation.FlipType.None
                    }
                };

                //PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();


                var basenpc = ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<GameObject>("Merchant_Key").transform.Find("NPC_Key").gameObject;
                if (basenpc == null)
                {
                    //BotsModule.Log("fuck shit fuck fuck shit");
                }

                PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();

                UnityEngine.JsonUtility.FromJsonOverwrite(UnityEngine.JsonUtility.ToJson(basenpc.GetComponent<PlayMakerFSM>()), iHaveNoFuckingClueWhatThisIs);


                FieldInfo fsmStringParams = typeof(ActionData).GetField("fsmStringParams", BindingFlags.NonPublic | BindingFlags.Instance);

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[0].Value = "#BEYOND_RUNBASEDMULTILINE_GENERIC";
                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[1].Value = "#BEYOND_RUNBASEDMULTILINE_STOPPER";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[4].ActionData) as List<FsmString>)[0].Value = "#BEYOND_SHOP_PURCHASED";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[5].ActionData) as List<FsmString>)[0].Value = "#BEYOND_PURCHASE_FAILED";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[7].ActionData) as List<FsmString>)[0].Value = "#BEYOND_INTRO";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[8].ActionData) as List<FsmString>)[0].Value = "#BEYOND_TAKEPLAYERDAMAGE";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[9].ActionData) as List<FsmString>)[0].Value = "#SUBSHOP_GENERIC_CAUGHT_STEALING";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[10].ActionData) as List<FsmString>)[0].Value = "#SHOP_GENERIC_NO_SALE_LABEL";

                (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[12].ActionData) as List<FsmString>)[0].Value = "#COOP_REBUKE";


                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "You are not one of us free of the masters control...");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "more words");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "even more words");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "to many words");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_RUNBASEDMULTILINE_STOPPER", "Enough talk");

                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Yes yes good good");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Enjoy this one");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_PURCHASE_FAILED", "To weak come back when you're in better condition");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_INTRO", "Welcome...");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_TAKEPLAYERDAMAGE", "The master's fury will not be kind to you!");



                npcObj.name = "Bot:Test_Npc";
                
                FakePrefab.MarkAsFakePrefab(npcObj);
                UnityEngine.Object.DontDestroyOnLoad(npcObj);
                npcObj.SetActive(true);                

                var devilLootTable = Alexandria.Helpers.Misc.LootUtility.CreateLootTable();
                foreach(var item in AlexandriaTags.GetAllItemsIdsWithTag("beyond"))
                {
                    devilLootTable.AddItemToPool(item);
                }
                
               

                //var devilLootTable = UnityEngine.Object.Instantiate(Tools.shared_auto_001.LoadAsset<GenericLootTable>("Shop_Key_Items_01"));

                var ItemPoint1 = new GameObject("ItemPoint1");
                ItemPoint1.transform.position = new Vector3(1.125f, 2.125f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint1);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint1);
                ItemPoint1.SetActive(true);
                var ItemPoint2 = new GameObject("ItemPoint2");
                ItemPoint2.transform.position = new Vector3(2.625f, 1f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint2);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint2);
                ItemPoint2.SetActive(true);
                var ItemPoint3 = new GameObject("ItemPoint3");
                ItemPoint3.transform.position = new Vector3(4.125f, 2.125f, 1);
                FakePrefab.MarkAsFakePrefab(ItemPoint3);
                UnityEngine.Object.DontDestroyOnLoad(ItemPoint3);
                ItemPoint3.SetActive(true);
                //var npc = UnityEngine.Object.Instantiate(npcObj, GameManager.Instance.PrimaryPlayer.gameObject.transform.position, Quaternion.identity);
                //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(npc.GetComponent<TalkDoerLite>());

                //BotsModule.Log("npc set up now moving to shop");

                var shopObj = new GameObject("Test_Npc_Shop").AddComponent<BaseShopController>();
                FakePrefab.MarkAsFakePrefab(shopObj.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(shopObj.gameObject);

                shopObj.gameObject.SetActive(false);

                shopObj.placeableHeight = 5;
                shopObj.placeableWidth = 5;
                shopObj.difficulty = 0;
                shopObj.isPassable = true;
                shopObj.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;
                shopObj.FoyerMetaShopForcedTiers = false;
                shopObj.IsBeetleMerchant = false;
                shopObj.ExampleBlueprintPrefab = null;
                shopObj.shopItems = devilLootTable;
                shopObj.spawnPositions = new Transform[] { ItemPoint1.transform, ItemPoint2.transform, ItemPoint3.transform };

                foreach (var pos in shopObj.spawnPositions) 
                {
                    pos.parent = shopObj.gameObject.transform;
                }

                shopObj.shopItemsGroup2 = null;
                shopObj.spawnPositionsGroup2 = null;
                shopObj.spawnGroupTwoItem1Chance = 0.5f;
                shopObj.spawnGroupTwoItem2Chance = 0.5f;
                shopObj.spawnGroupTwoItem3Chance = 0.5f;
                shopObj.shopkeepFSM = npcObj.GetComponent<PlayMakerFSM>();
                shopObj.shopItemShadowPrefab = Tools.shared_auto_001.LoadAsset<GameObject>("Merchant_Key").GetComponent<BaseShopController>().shopItemShadowPrefab;
                shopObj.cat = null;
                shopObj.OptionalMinimapIcon = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Npcs/mapiconshop");
                shopObj.ShopCostModifier = 1;
                shopObj.FlagToSetOnEncounter = GungeonFlags.NONE;

                shopObj.gameObject.AddComponent<DevilDealShopHelper>();


                

                npcObj.transform.parent = shopObj.gameObject.transform;
                npcObj.transform.position = new Vector3(1.9375f, 3.4375f, 5.9375f);

                var carpetObj = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Npcs/beyond_merch_carpet_001.png", new GameObject("Bot:Test_Npc_Carpet"));
                carpetObj.GetComponent<tk2dSprite>().SortingOrder = 2;
                FakePrefab.MarkAsFakePrefab(carpetObj);
                UnityEngine.Object.DontDestroyOnLoad(carpetObj);
                carpetObj.SetActive(true);

                carpetObj.transform.parent = shopObj.gameObject.transform;
                carpetObj.layer = 20;


                BotsModule.shop = shopObj;
                BotsModule.NPC = npcObj;
                //DungeonPlaceableUtility.InstantiateDungeonPlaceable(shopObj.gameObject, GameManager.Instance.PrimaryPlayer.CurrentRoom, new IntVector2((int)GameManager.Instance.PrimaryPlayer.gameObject.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.gameObject.transform.position.y), false);
                //UnityEngine.Object.Instantiate(shopObj, , Quaternion.identity).GetComponent<BaseShopController>().ConfigureOnPlacement();
                //shopObj.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;
                //BotsModule.Log("all done :D");
            }
            catch (Exception message)
            {
                BotsModule.Log(message.ToString());
            }
        }
        #endregion
    /*
    #region jailnpc
    public static void Init2()
    {
        try
        {
            List<string> testNpcIdleSprites = new List<string>
            {
                "shopkeep_001.png",
                "shopkeep_002.png",
                "shopkeep_003.png",
                "shopkeep_004.png",
                "shopkeep_005.png",
                "shopkeep_006.png",
                "shopkeep_007.png",
                "shopkeep_008.png",
                "shopkeep_009.png",
            };

            List<string> testNpcTalkSprites = new List<string>
            {
                "shopkeep_talk_001.png",
                "shopkeep_talk_002.png",
                "shopkeep_talk_003.png",
                "shopkeep_talk_004.png",
                "shopkeep_talk_005.png",
            };

            BotsModule.Log("anim lists done");

            var SpeechPoint = new GameObject("SpeechPoint");
            SpeechPoint.transform.position = new Vector3(0.8125f, 2.1875f, -1.31f);



            var npcObj = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Npcs/Beyond/shopkeep_001.png", new GameObject("Bot:Test_Npc_Jail"));

            npcObj.layer = 22;

            var collection = npcObj.GetComponent<tk2dSprite>().Collection;
            SpeechPoint.transform.parent = npcObj.transform;

            FakePrefab.MarkAsFakePrefab(SpeechPoint);
            SpeechPoint.SetActive(true);

            //-2729308948368026681
            //-2729308948368026681

            var idleIdsList = new List<int>();
            var talkIdsList = new List<int>();

            foreach (string sprite in testNpcIdleSprites)
            {
                idleIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Beyond/" + sprite, collection));
            }

            foreach (string sprite in testNpcTalkSprites)
            {
                talkIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Beyond/" + sprite, collection));
            }


            BotsModule.Log("anim lists done fr this time");

            tk2dSpriteAnimator spriteAnimator = npcObj.AddComponent<tk2dSpriteAnimator>();

            SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "beyond_npc_idle", tk2dSpriteAnimationClip.WrapMode.Loop, 6);
            SpriteBuilder.AddAnimation(spriteAnimator, collection, talkIdsList, "beyond_npc_talk", tk2dSpriteAnimationClip.WrapMode.LoopSection, 8).loopStart = 3;

            SpeculativeRigidbody rigidbody = Tools.GenerateOrAddToRigidBody(npcObj, CollisionLayer.BulletBlocker, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(20, 18), new IntVector2(5, 0));

            BotsModule.Log("rigidbody done");





            TalkDoerLite talkDoer = npcObj.AddComponent<TalkDoerLite>();

            talkDoer.placeableWidth = 4;
            talkDoer.placeableHeight = 3;
            talkDoer.difficulty = 0;
            talkDoer.isPassable = true;
            talkDoer.usesOverrideInteractionRegion = false;
            talkDoer.overrideRegionOffset = Vector2.zero;
            talkDoer.overrideRegionDimensions = Vector2.zero;
            talkDoer.overrideInteractionRadius = -1;
            talkDoer.PreventInteraction = false;
            talkDoer.AllowPlayerToPassEventually = true;
            talkDoer.speakPoint = SpeechPoint.transform;
            talkDoer.SpeaksGleepGlorpenese = false;
            talkDoer.audioCharacterSpeechTag = "oldman";
            talkDoer.playerApproachRadius = 5;
            talkDoer.conversationBreakRadius = 5;
            talkDoer.echo1 = null;
            talkDoer.echo2 = null;
            talkDoer.PreventCoopInteraction = false;
            talkDoer.IsPaletteSwapped = false;
            talkDoer.PaletteTexture = null;
            talkDoer.OutlineDepth = 0.5f;
            talkDoer.OutlineLuminanceCutoff = 0.05f;
            talkDoer.MovementSpeed = 3;
            talkDoer.PathableTiles = CellTypes.FLOOR;

            BotsModule.Log("TalkDoerLite done");

            UltraFortunesFavor dreamLuck = npcObj.AddComponent<UltraFortunesFavor>();

            dreamLuck.goopRadius = 2;
            dreamLuck.beamRadius = 2;
            dreamLuck.bulletRadius = 2;
            dreamLuck.bulletSpeedModifier = 0.8f;

            dreamLuck.vfxOffset = 0.625f;
            dreamLuck.sparkOctantVFX = Tools.shared_auto_001.LoadAsset<GameObject>("FortuneFavor_VFX_Spark");

            BotsModule.Log("UltraFortunesFavor done");


            AIAnimator aIAnimator = Tools.GenerateBlankAIAnimator(npcObj);

            aIAnimator.IdleAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.Single,
                Prefix = "beyond_npc_idle",
                AnimNames = new string[]
                {
                    ""
                },
                Flipped = new DirectionalAnimation.FlipType[]
                {
                    DirectionalAnimation.FlipType.None
                }

            };

            aIAnimator.TalkAnimation = new DirectionalAnimation
            {
                Type = DirectionalAnimation.DirectionType.Single,
                Prefix = "beyond_npc_talk",
                AnimNames = new string[]
                {
                    ""
                },
                Flipped = new DirectionalAnimation.FlipType[]
                {
                    DirectionalAnimation.FlipType.None
                }
            };

            //PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();


            var basenpc = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("NPC_Key_Jailed");
            if (basenpc == null)
            {
                BotsModule.Log("fuck shit fuck fuck shit");
            }

            PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();

            UnityEngine.JsonUtility.FromJsonOverwrite(UnityEngine.JsonUtility.ToJson(basenpc.GetComponent<PlayMakerFSM>()), iHaveNoFuckingClueWhatThisIs);


            FieldInfo fsmStringParams = typeof(ActionData).GetField("fsmStringParams", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo actionNames = typeof(ActionData).GetField("actionNames", BindingFlags.NonPublic | BindingFlags.Instance);



            (actionNames.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)


            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[0].Value = "#BEYOND_RUNBASEDMULTILINE_GENERIC";
            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[1].ActionData) as List<FsmString>)[1].Value = "#BEYOND_RUNBASEDMULTILINE_STOPPER";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[4].ActionData) as List<FsmString>)[0].Value = "#BEYOND_SHOP_PURCHASED";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[5].ActionData) as List<FsmString>)[0].Value = "#BEYOND_PURCHASE_FAILED";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[7].ActionData) as List<FsmString>)[0].Value = "#BEYOND_INTRO";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[8].ActionData) as List<FsmString>)[0].Value = "#BEYOND_TAKEPLAYERDAMAGE";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[9].ActionData) as List<FsmString>)[0].Value = "#SUBSHOP_GENERIC_CAUGHT_STEALING";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[10].ActionData) as List<FsmString>)[0].Value = "#SHOP_GENERIC_NO_SALE_LABEL";

            (fsmStringParams.GetValue(iHaveNoFuckingClueWhatThisIs.FsmStates[12].ActionData) as List<FsmString>)[0].Value = "#COOP_REBUKE";


            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "You are not one of us free of the masters control...");
            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "more words");
            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "even more words");
            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "to many words");

            ETGMod.Databases.Strings.Core.Set("#BEYOND_RUNBASEDMULTILINE_STOPPER", "Enough talk");

            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Yes yes good good");
            ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Enjoy this one");

            ETGMod.Databases.Strings.Core.Set("#BEYOND_PURCHASE_FAILED", "To weak come back when you're in better condition");

            ETGMod.Databases.Strings.Core.Set("#BEYOND_INTRO", "Welcome...");

            ETGMod.Databases.Strings.Core.Set("#BEYOND_TAKEPLAYERDAMAGE", "The master's fury will not be kind to you!");



            npcObj.name = "Bot:Test_Npc_Jail";



            BotsModule.NPC = npcObj;
            BotsModule.Log("all done :D");
        }
        catch (Exception message)
        {
            BotsModule.Log(message.ToString());
        }
    }
    #endregion
    */
    }
}
