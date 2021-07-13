using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod.NPCs
{
    class NpcInitShit
    {


        public static void Init()
        {
            try
            {
                List<string> testNpcIdleSprites = new List<string>
                {
                    "test_npc_idle_001.png",
                    "test_npc_idle_002.png",
                    "test_npc_idle_003.png",
                    "test_npc_idle_004.png"
                };

                List<string> testNpcTalkSprites = new List<string>
                {
                    "test_npc_talk_001.png",
                    "test_npc_talk_002.png",
                };

                BotsModule.Log("anim lists done");

                var npcObj = SpriteBuilder.SpriteFromResource("BotsMod/sprites/Npcs/Test/test_npc_idle_001.png", new GameObject("Bot:Test_Npc"));

                var collection = npcObj.GetComponent<tk2dSprite>().Collection;

                var idleIdsList = new List<int>();
                var talkIdsList = new List<int>();

                foreach (string sprite in testNpcIdleSprites)
                {
                    idleIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Test/" + sprite, collection));
                }

                foreach (string sprite in testNpcTalkSprites)
                {
                    talkIdsList.Add(SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/Npcs/Test/" + sprite, collection));
                }


                BotsModule.Log("anim lists done fr this time");

                tk2dSpriteAnimator spriteAnimator = npcObj.AddComponent<tk2dSpriteAnimator>();

                SpriteBuilder.AddAnimation(spriteAnimator, collection, idleIdsList, "test_npc_idle", tk2dSpriteAnimationClip.WrapMode.Loop);
                SpriteBuilder.AddAnimation(spriteAnimator, collection, talkIdsList, "test_npc_talk", tk2dSpriteAnimationClip.WrapMode.Loop);

                SpeculativeRigidbody rigidbody = Tools.GenerateOrAddToRigidBody(npcObj, CollisionLayer.BulletBlocker, PixelCollider.PixelColliderGeneration.Manual, true, true, true, false, false, false, false, true, new IntVector2(20, 18), new IntVector2(5, 0));

                BotsModule.Log("rigidbody done");

                var talkPoint = new GameObject("TalkPoint").GetComponent<Transform>();

                talkPoint.localPosition = new Vector3(0.6875f, 2.0625f, 0);
                talkPoint.parent = npcObj.transform;

                UnityEngine.Object.DontDestroyOnLoad(talkPoint);

                BotsModule.Log("talkPoint done");

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
                talkDoer.speakPoint = talkPoint;
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

                /*UltraFortunesFavor dreamLuck = npcObj.AddComponent<UltraFortunesFavor>();

                dreamLuck.goopRadius = 2;
                dreamLuck.beamRadius = 2;
                dreamLuck.bulletRadius = 2;
                dreamLuck.bulletSpeedModifier = 0.8f;

                dreamLuck.vfxOffset = 0.625f;
                dreamLuck.sparkOctantVFX = Tools.shared_auto_001.LoadAsset<GameObject>("FortuneFavor_VFX_Spark");
                */
                BotsModule.Log("UltraFortunesFavor done");


                AIAnimator aIAnimator = Tools.GenerateBlankAIAnimator(npcObj);
                aIAnimator.IdleAnimation = new DirectionalAnimation
                {
                    Type = DirectionalAnimation.DirectionType.Single,
                    Prefix = "test_npc_idle",
                    AnimNames = new string[]
                    {
                                ""
                    },
                    Flipped = new DirectionalAnimation.FlipType[]
                    {
                                DirectionalAnimation.FlipType.None
                    }

                };

                aIAnimator.OtherAnimations.Add(new AIAnimator.NamedDirectionalAnimation
                {
                    name = "talk",
                    anim = new DirectionalAnimation
                    {
                        Type = DirectionalAnimation.DirectionType.Single,
                        Prefix = "test_npc_talk",
                        AnimNames = new string[]
                        {
                                    ""
                        },
                        Flipped = new DirectionalAnimation.FlipType[]
                        {
                                    DirectionalAnimation.FlipType.None
                        }
                    }
                });

                PlayMakerFSM iHaveNoFuckingClueWhatThisIs = npcObj.AddComponent<PlayMakerFSM>();
                /*iHaveNoFuckingClueWhatThisIs.Fsm.Name = "Main";
                iHaveNoFuckingClueWhatThisIs.Fsm.StartState = "Idle";
                iHaveNoFuckingClueWhatThisIs.Fsm.DataVersion = 1;
                iHaveNoFuckingClueWhatThisIs.Fsm.States = new FsmState[]
                {
                    new FsmState(iHaveNoFuckingClueWhatThisIs.Fsm)
                    {
                        Name = "Idle",
                        Description = "",
                        ColorIndex = 1,
                        IsBreakpoint = false,
                        IsSequence = false,
                        HideUnused = false,
                        Transitions = new FsmTransition[]
                        {
                            new FsmTransition
                            {
                                FsmEvent = new FsmEvent("playerInteract")
                                {
                                    IsGlobal = true,
                                    IsSystemEvent = false,
                                },
                                ToState = "Bool Tests",
                                LinkStyle = FsmTransition.CustomLinkStyle.Default,
                                LinkConstraint = FsmTransition.CustomLinkConstraint.None,
                                ColorIndex = 0,

                            },

                        },


                    }
                };*/

                UnityEngine.JsonUtility.FromJsonOverwrite(File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/fuckingdie.txt"), iHaveNoFuckingClueWhatThisIs);
                //iHaveNoFuckingClueWhatThisIs.Fsm.Init(iHaveNoFuckingClueWhatThisIs);
                //   iHaveNoFuckingClueWhatThisIs
                GungeonAPI.FakePrefab.MarkAsFakePrefab(npcObj);
                UnityEngine.Object.DontDestroyOnLoad(npcObj);
                npcObj.SetActive(true);

                BotsModule.Log("npc set up now moving to shop but just like the loot table");

                ETGMod.Databases.Strings.Core.Set("#TEST_NPC_CRY", "*cry*");
                ETGMod.Databases.Strings.Core.Set("#TEST_NPC_BAD", "bad lol");
                ETGMod.Databases.Strings.Core.Set("#TEST_NPC_FUCK_YOU_RESPONCE", "FUCK YOU >:(");
                ETGMod.Databases.Strings.Core.Set("#TEST_NPC_YES", "yes...");
                ETGMod.Databases.Strings.Core.Set("#TEST_NPC_GET_GOOD", "get good kid");



                ETGMod.Databases.Strings.Core.Set("#TESTNPC_IDFK", "Hello there mode begin 1 has triggered... i have no fucking clue what that means.");
                ETGMod.Databases.Strings.Core.Set("#TESTNPC_IDFK2", "Hello there mode begin 2 has triggered... i have no fucking clue what that means.");

                ETGMod.Databases.Strings.Core.Set("#TESTNPC_CAUGHT_STEALING", "THIS TEXT SHOULD NEVER BE DISPLAYED AS STEALING FROM THIS SHOP SHOULD NOT WORK!!");
                ETGMod.Databases.Strings.Core.Set("#TESTNPC_OUCH", "Ouch");

                ETGMod.Databases.Strings.Core.Set("#TESTNPC_SELL_SOUL", "Good choice indeed...");
                ETGMod.Databases.Strings.Core.Set("#TESTNPC_FAIL_TO_SELL_SOUL", "Nope!");

                ETGMod.Databases.Strings.Core.Set("#TESTNPC_TELL_CULTIST_TO_FUCK_OFF", "You're not old enough to sell your soul.");

                ETGMod.Databases.Strings.Core.Set("#TESTNPC_Greet", "Welcome, please sell me your soul.");

                var devilLootTable = ScriptableObject.CreateInstance<GenericLootTable>();
                devilLootTable.defaultItemDrops = new WeightedGameObjectCollection()
                {
                    elements = new List<WeightedGameObject>()
                    {
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 60,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 125,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 434,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 271,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 571,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 33,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 17,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 347,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 90,
                          rawGameObject = null,
                          weight = 1                       
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 336,
                          rawGameObject = null,
                          weight = 1
                        },
                        new WeightedGameObject()
                        {
                          additionalPrerequisites = new DungeonPrerequisite[0],
                          forceDuplicatesPossible = true,
                          pickupId = 285,
                          rawGameObject = null,
                          weight = 1
                        },
                    }
                };
                devilLootTable.includedLootTables = new List<GenericLootTable>();
                devilLootTable.tablePrerequisites = new DungeonPrerequisite[0];


                //var devilLootTable = UnityEngine.Object.Instantiate(Tools.shared_auto_001.LoadAsset<GenericLootTable>("Shop_Key_Items_01"));
               


                //var npc = UnityEngine.Object.Instantiate(npcObj, GameManager.Instance.PrimaryPlayer.gameObject.transform.position, Quaternion.identity);
                //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(npc.GetComponent<TalkDoerLite>());

                BotsModule.Log("npc set up now moving to shop");

                var shopObj = new GameObject("Test_Npc_Shop").AddComponent<BaseShopController>();
                GungeonAPI.FakePrefab.MarkAsFakePrefab(shopObj.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(shopObj.gameObject);

                shopObj.gameObject.SetActive(false);

                shopObj.placeableHeight = 5;
                shopObj.placeableWidth = 5;
                shopObj.difficulty = 0;
                shopObj.isPassable = true;
                shopObj.baseShopType = BaseShopController.AdditionalShopType.NONE;
                shopObj.FoyerMetaShopForcedTiers = false;
                shopObj.IsBeetleMerchant = false;
                shopObj.ExampleBlueprintPrefab = null;
                shopObj.shopItems = devilLootTable;
                shopObj.spawnPositions = Tools.shared_auto_001.LoadAsset<GameObject>("Merchant_Key").GetComponent<BaseShopController>().spawnPositions;

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
                shopObj.OptionalMinimapIcon = null;
                shopObj.ShopCostModifier = 1;
                shopObj.FlagToSetOnEncounter = GungeonFlags.NONE;

                npcObj.transform.parent = shopObj.gameObject.transform;


                BotsModule.shop = shopObj;
                BotsModule.NPC = npcObj;
                //DungeonPlaceableUtility.InstantiateDungeonPlaceable(shopObj.gameObject, GameManager.Instance.PrimaryPlayer.CurrentRoom, new IntVector2((int)GameManager.Instance.PrimaryPlayer.gameObject.transform.position.x, (int)GameManager.Instance.PrimaryPlayer.gameObject.transform.position.y), false);
                //UnityEngine.Object.Instantiate(shopObj, , Quaternion.identity).GetComponent<BaseShopController>().ConfigureOnPlacement();
                //shopObj.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;
            }
            catch (Exception message)
            {
                BotsModule.Log(message.ToString());
            }
        }
    }
}
