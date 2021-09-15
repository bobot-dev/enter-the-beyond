using FrostAndGunfireItems;
using GungeonAPI;
using ItemAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Steamworks;
using Gungeon;
using Pathfinding;
using System.Collections;
using Dungeonator;

using CustomCharacters;
using System.IO;
using Ionic.Zip;

using SaveAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using AmmonomiconAPI;
using HutongGames.PlayMaker;
using BotsMod.NPCs;
using BotsMod.ToolsAndStuff;
using LiveRecolor;
//using ChallengeAPI;


namespace BotsMod
{
    public class BotsModule : ETGModule
    {
        public static readonly string MOD_NAME = "Enter The Beyond";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#e01279";
        public static readonly string LOST_COLOR = "#7732a8";
        public static readonly string LOCKED_CHARACTOR_COLOR = "#ba0c00";

        public static AdvancedStringDB Strings;
        public static OverseerShield overseerShield;
        public static LostCharacterCostumeSwapper costumeSwapper;
        public static tk2dSpriteAnimation LostAltCostume;
        public static bool debugMode = true;
        public static BaseShopController shop;

        public static GameObject WarCrime;
        public static Chest KeyChest;
        public static PickupObject WarCrime2;
        public static GameObject Shop;
        public static GameObject NPC;

        public static string ZipFilePath;
        public static string FilePath;


        public static string characterFilePath;


        public static Gun ChamberGun;

        public static ETGModuleMetadata metadata;

        private int FontSize = 34;


        public static UnityEngine.UI.Text CatogreyText;

        public override void Start()
        {

            try
            {
                metadata = this.Metadata;
                characterFilePath = this.Metadata.Directory + "/characters";
                //gungeon api
                GungeonAP.Init();

                 ETGModConsole.Commands.AddUnit("botflow", delegate (string[] args)
                 {
                     DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
                     string str = DungeonHandler.debugFlow ? "enabled" : "disabled";
                     string color = DungeonHandler.debugFlow ? "#00FF00" : "FF0000";
                     BotsModule.Log("Debug flow " + str, color);
                 });
            }
            catch (Exception e)
            {
                BotsModule.Log("Flow Command Broke", "#eb1313");
                BotsModule.Log(string.Format(e + ""), "#eb1313");
            }

            try
            {
                //characterFilePath = this.Metadata.Directory + "/characters";

                CustomCharacters.Hooks.Init();

                FakePrefabHooks.Init();
                ToolsGAPI.Init();
                
                CharacterSwitcher.Init();
                

                ToolsGAPI.Print("Did Start()", "##00FF00");

            }
            catch (Exception e)
            {
                BotsModule.Log("Characters Broke (crying is a valid response)", "#eb1313");
                BotsModule.Log(string.Format(e + ""), "#eb1313");
            }

            try
            {

                ZipFilePath = this.Metadata.Archive;
                FilePath = this.Metadata.Directory;

                AudioResourceLoader.InitAudio();

                Strings = new AdvancedStringDB();
                //item api
                ItemBuilder.Init();
                ItemAPI.FakePrefabHooks.Init();

                //ChallengeInit.Init();


                //GungeonAP.Init();

                GameManager.Instance.gameObject.AddComponent<DiscordController>();
                /*
                var array = Tools.ReflectionHelpers.ReflectGetField<Type[]>(typeof(GameManager), "BraveLevelLoadedListeners", GameManager.Instance).ToList();
                array.Add(typeof(FuckYouThisIsAnAwfulIdea));
                Tools.ReflectionHelpers.ReflectSetField(typeof(GameManager), "BraveLevelLoadedListeners", array.ToArray(), GameManager.Instance);
                */
                //enemy api
                EnemyBuilder.Init();
                BossBuilder.Init();
                FrostAndGunfireItems.EnemyTools.Init();
                FrostAndGunfireItems.Hooks.Init();

                //funny book api
                AmmonomiconAPI.Tools.Init();

               

                //my stuff
                Tools.Init();
                Hooks.Init();
                Rooms.Init();
                //BeyondPrefabs.Init();
                //RoomPrefabs.Init();
                CustomFire.Init();



                Tools.AHHH = this.LoadAssetBundleFromLiterallyAnywhere("customglitchshader");

                Tools.BotsAssetBundle = this.LoadAssetBundleFromLiterallyAnywhere("botsassetbundle");

                SoulHeartController.Init();

                FloorHooks.Init();

                //StuffIStoleFromApacheForChallengeMode.Init();

                Ammonomicon.Init();

                InitGameObjects.Init();
                ChestInitStuff.Init();

                


                BeyondMasteryToken.Init();

                ScrapBook.Init();

                Bob.Init();
                CosmicSludge.Init();
                BlessedOrb.Init();
                BabyGoodMistake.Init();
                //LostsPast.Init();
                PirmalShotgrub.Init();
                BeyondKin.Init();
                LostPastBoss.Init();
                //OverseerDecoy.Init();
                TestActive.Init();
                //SpecialDungeon.Init();
                //SpecialDungeon2CozFuckYou.Init();
                RichPresenceItem.Register();

                BeyondScout.Init();

                TestGun.Add();

                Coin.Init();
                HellsRevolver2.Add();
                HellsRevolver.Add();
                HellsShotgun.Add();

                CompletlyRandomGun.Add();

                PossetionItem.Init();

                LostFriend.Init();

                PortalThing.Init();
                NoTimeToExplain.Add();

                TheMessanger.Add();

                LostsCloak.Init();

                //LootTables.Init();

                RedKey.Init();

                LostSidearm.Add();

                TrailBullets.Init();

                SpellInit.Init();
                Wand.Add();
               
                EnchantedEnemies.Init();

                TestPassive.Init();

                LightningRounds.Init();

                Sin.Init();

                Roomba.Init();

                SpinDownDice.Init();

                //OtherworldlyConnections.Init();
                OtherwordlyFury.Init();


                NpcInitShit.Init();
                //GameManager.Instance.PrimaryPlayer.star

                var stupidlist = Tools.ReflectionHelpers.ReflectGetField<Type[]>(typeof(GameManager), "BraveLevelLoadedListeners", GameManager.Instance).ToList();
                stupidlist.Add(typeof(FuckYouThisIsAnAwfulIdea));
                Tools.ReflectionHelpers.ReflectSetField<Type[]>(typeof(GameManager), "BraveLevelLoadedListeners", stupidlist.ToArray(), GameManager.Instance);
                Log("reflections done");

                //var coinPannel = FakePrefab.Clone(Tools.shared_auto_001.LoadAsset<GameObject>("BlankPanel"));
                var coinPannel = FakePrefab.Clone(GameUIRoot.Instance.blankControllers[0].gameObject);
                coinPannel.transform.parent = AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("UI Root").transform;
                Log("objs sorta done");
                var coinSpritePrefab = FakePrefab.Clone(coinPannel.GetComponent<GameUIBlankController>().blankSpritePrefab.gameObject).GetComponent<dfSprite>();
                coinSpritePrefab.gameObject.GetOrAddComponent<Transform>();


                

                coinSpritePrefab.SpriteName = "heart_shield_full_001";
                Log("objs done");

                Log("done");
                UnityEngine.Object.Destroy(coinPannel.GetComponent<GameUIBlankController>());
                Log("done done");
                GameUIRoot.Instance.coinUIControllersAdd(coinPannel.AddComponent<FuckYouThisIsAnAwfulIdea>());


                coinPannel.GetComponent<FuckYouThisIsAnAwfulIdea>().IsRightAligned = false;
                coinPannel.GetComponent<FuckYouThisIsAnAwfulIdea>().extantCoins = new List<dfSprite>();
                coinPannel.GetComponent<FuckYouThisIsAnAwfulIdea>().CoinSpritePrefab = coinSpritePrefab;

                FakePrefab.MarkAsFakePrefab(coinSpritePrefab.gameObject);
                FakePrefab.MarkAsFakePrefab(coinPannel);
                UnityEngine.Object.DontDestroyOnLoad(coinSpritePrefab.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(coinPannel);
                coinSpritePrefab.gameObject.SetActive(true);
                coinPannel.SetActive(true);

                Log("done done done");

                BeyondPrefabs.InitCustomPrefabs();
                ModRoomPrefabs.InitCustomRooms();
                BeyondDungeonFlows.InitDungeonFlows();
                BeyondDungeon.InitCustomDungeon();
                LostPastDungeon.InitCustomDungeon();
                Hook hook = new Hook(
                   typeof(GameManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                   typeof(BotsModule).GetMethod("GameManager_Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                   typeof(GameManager)
               );
                

                metadata = this.Metadata;



                GUI.Init();

                CatogreyText = GUI.CreateText(null, new Vector2(15f, 265), "", TextAnchor.MiddleLeft, font_size: FontSize);

                ETGModMainBehaviour.Instance.gameObject.AddComponent<CatogreyTeller>();

                InitSynergies.Init();

                Loader.BuildCharacter("Lost", true, true, true, true, new Color32(255, 69, 248, 255), 4.55f, 55, 2, true, "botfs_lost");
                Loader.BuildCharacter("Shade", false, true, true, false, new Color32(0, 0, 0, 0), 0, 0, 0, false, "");
                Loader.BuildCharacter("The Blind", false, true, true, false, new Color32(0, 0, 0, 0), 0, 0, 0, false, "");

                //ShrineFactory.RegisterShrineRoom(NPC, protoroom, "bot:test_npc_shrine", new Vector2(1, 1));

                //CollectionDumper.DumpCollection();


                //Ammonomicon.Init();

                ChamberGun = (PickupObjectDatabase.GetById(647) as Gun);
                if (ChamberGun.gameObject.GetComponent<ChamberGunProcessor>())
                {
                    UnityEngine.Object.Destroy(ChamberGun.gameObject.GetComponent<ChamberGunProcessor>());
                    ChamberGun.gameObject.AddComponent<BotChamberGunProcessor>();
                }

                MakeThemAllPetable.Init();
                //RichPresence.init();

                //GameStatsManager.Instance.SetStat(TrackedStats.NUMBER_DEATHS, 27615);
                /*
                if (LostPastBoss.BulletBank && LostPastBoss.BulletBank.aiActor && LostPastBoss.BulletBank.aiActor.TargetRigidbody)
                {
                    LostPastBoss.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b").bulletBank.GetBullet("eye"));


                }*/

                


                AlphabetSoupSynergyProcessor alphabetSoupSynergyProcessor = PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>();

                foreach (AlphabetSoupEntry entry in alphabetSoupSynergyProcessor.Entries)
                {
                    Log(entry.BaseProjectile.name, TEXT_COLOR);
                    
                };
                Log(((Gun)PickupObjectDatabase.GetById(340)).DefaultModule.projectiles[0].name, TEXT_COLOR);

                AlphabetSoupEntry TransRights = new AlphabetSoupEntry
                {
                    Words = new string[]
{
                        "transrights".ToUpper(),
},
                    BaseProjectile = alphabetSoupSynergyProcessor.Entries[0].BaseProjectile,
                    //BaseProjectile = 
                    RequiredSynergy = CustomSynergyType.ALPHABET_PLUS_ONE,//CustomEnums.CustomCustomSynergyType.LOWER_CASE_R_TEST,
                    AudioEvents = new string[]
{
                        "Play_WPN_rgun_bullet_01"
}
                };

                /*AlphabetSoupEntry iShouldntHaveBeenGivenThisPower6 = new AlphabetSoupEntry
                {
                    Words = new string[]
                    {
                        "FUCK"
                    },
                    BaseProjectile = alphabetSoupSynergyProcessor.Entries[0].BaseProjectile,
                    //BaseProjectile = 
                    RequiredSynergy = CustomSynergyType.ALPHABET_PLUS_ONE,//CustomEnums.CustomCustomSynergyType.LOWER_CASE_R_TEST,
                    AudioEvents = new string[]
                    {
                        "Play_Fuck"
                    }
                };
                Log("sc 0");
                if (Tools.Foyer_ElevatorChamber == null)
                {
                    Log("fuck!!!!!!!!");
                    Log("fuck!!!!!!!!");
                }

                ShortcutElevatorController shortcutElevatorController = (Resources.FindObjectsOfTypeAll(typeof(ShortcutElevatorController)) as ShortcutElevatorController[])[0];

                var shortCutController = Tools.Foyer_ElevatorChamber.GetComponent<ShortcutElevatorController>();
                Log("sc 0.5");
                if (shortCutController == null)
                {
                    Log("fuck2!!!!!!!!");
                }
                Log("sc 0.6");
                List<ShortcutDefinition> shortCuts = new List<ShortcutDefinition>();
                Log("sc 0.7");
                for (int i = 0; i < shortCutController.definedShortcuts.Length; i++)
                {
                    shortCuts.Add(shortCutController.definedShortcuts[i]);
                }

                
                Log("sc 1");
                SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/elevator_bottom_floor_beyond.png", shortCutController.elevatorFloorSprite.Collection, "elevator_bottom_floor_beyond");
                shortCuts.Add(new ShortcutDefinition
                {
                    elevatorFloorSpriteName = "elevator_bottom_floor_beyond",
                    IsBossRush = false,
                    IsSuperBossRush = false,
                    requiredFlag = GungeonFlags.NONE,
                    sherpaTextKey = "test",
                    targetLevelName = BeyondDungeon.BeyondDefinition.dungeonSceneName,
                });
                Log("sc 2");
                shortCutController.definedShortcuts = shortCuts.ToArray();
                Log("sc 3");*/
                var funnylist = new List<AlphabetSoupEntry> { TransRights };
               // var funnylist = alphabetSoupSynergyProcessor.Entries.ToList();
                //funnylist.Add(iShouldntHaveBeenGivenThisPower6);


                PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>().Entries = funnylist.ToArray();

                //alphabetSoupSynergyProcessor.Entries[];
                _SpawnAutocompletionSettings = new AutocompletionSettings(delegate (string input)
                {
                    List<string> list = new List<string>();
                    foreach (string text in Game.Enemies.IDs)
                    {
                        if (text.AutocompletionMatch(input.ToLower()))
                        {
                            list.Add(text.Replace("gungeon:", ""));
                        }
                    }
                    return list.ToArray();
                });

                _ItemIdAutocompletionSettings = new AutocompletionSettings(delegate (string input)
                {
                    List<string> list = new List<string>();
                    foreach (var obj in PickupObjectDatabase.Instance.Objects)
                    {
                        if (obj.PickupObjectId.ToString().AutocompletionMatch(obj.PickupObjectId.ToString().ToLower()))
                        {
                            list.Add(obj.PickupObjectId.ToString());
                        }
                    }
                    return list.ToArray();
                });

                _GiveAutocompletionSettings = new AutocompletionSettings(delegate (string input)
                {
                    List<string> list = new List<string>();
                    foreach (string text in Game.Items.IDs)
                    {
                        if (text.AutocompletionMatch(input.ToLower()))
                        {
                            Console.WriteLine(string.Format("INPUT {0} KEY {1} MATCH!", input, text));
                            list.Add(text.Replace("gungeon:", ""));
                        }
                        else
                        {
                            Console.WriteLine(string.Format("INPUT {0} KEY {1} NO MATCH!", input, text));
                        }
                    }
                    return list.ToArray();
                });

                ETGModConsole.Commands.AddGroup("bot");

                ETGModConsole.Commands.GetGroup("bot").AddUnit("randomize", delegate (string[] args)
                {
                    EtgRandomizerController.Init();
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("setUpUI", delegate (string[] args)
                {

                    GUIhandler handler = GameManager.Instance.PrimaryPlayer.gameObject.GetOrAddComponent<GUIhandler>();
                    handler.enabled = true;

                });
                ETGModConsole.Commands.GetGroup("bot").AddUnit("checkLootTable", delegate (string[] args)
                {
                    Log($"Number of items in loot table: {StaticSpellReferences.spellLootTable.defaultItemDrops.elements.Count}");
                    foreach (var item in StaticSpellReferences.spellLootTable.defaultItemDrops.elements)
                    {
                        Log($"[{item.pickupId}]: {item.rawGameObject.name}");
                    }

                });



                ETGModConsole.Commands.GetGroup("bot").AddUnit("addSpellSlots", delegate (string[] args)
                {

                    if (GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.GetComponent<Wand>() != null)
                    {
                        GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.GetComponent<Wand>().spellSlots += int.Parse(args[0]);
                        Log($"Added {int.Parse(args[0])} Slots to {GameManager.Instance.PrimaryPlayer.CurrentGun.EncounterNameOrDisplayName}.");
                    }
                    else
                    {
                        Log("Please Hold a Wand");
                    }

                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("addCoinToUi", delegate (string[] args)
                {
                    //foreach (var dumbfuck in GameUIRoot.Instance.coinUIControllers())
                    //{
                    //    Log("a");
                    //}

                    //GameUIRoot.Instance.coinUIControllers()[0].AddCoin();\
                    //UnityEngine.JsonUtility.ToJson();
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("dumpPlayerSprites", delegate (string[] args)
                {
                    CollectionDumper.DumpCollection(GameManager.Instance.PrimaryPlayer.sprite.Collection);
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("masterRound", delegate (string[] args)
                {
                    Log(GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun.ToString());
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("setPerks", delegate (string[] args)
                {

                    GameManager.Instance.PrimaryPlayer.CurrentGun.GetComponent<TheMessanger>().firstPerk = TheMessanger.FirstSlotPerks.MovingTarget;
                    GameManager.Instance.PrimaryPlayer.CurrentGun.GetComponent<TheMessanger>().secondPerk = TheMessanger.SecondSlotPerks.Desperado;
                });

                ETGModConsole.Commands.AddUnit("exportPlayerTexture", delegate (string[] args)
                {
                    File.WriteAllBytes(ETGMod.ResourcesDirectory + GameManager.Instance.PrimaryPlayer.characterIdentity.ToString() + "spritesheet.png", ((Texture2D)GameManager.Instance.PrimaryPlayer.sprite.renderer.material.GetTexture("_MainTex")).EncodeToPNG());
                });

                ETGModConsole.Commands.AddUnit("getAllInteractablesInRoom", delegate (string[] args)
                {
                    foreach(var obj in GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRoomInteractables())
                    {
                        Log(obj.ToString());
                        Log(obj.GetDistanceToPoint(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter).ToString());
                    }
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("npc", delegate (string[] args)
                {
                    Log("0");
                    if (GameManager.Instance.PrimaryPlayer.CurrentRoom == null)
                    {
                        Log("shop did a null");
                    }

                    var shopObj = DungeonPlaceableUtility.InstantiateDungeonPlaceable(shop.gameObject, GameManager.Instance.PrimaryPlayer.CurrentRoom, (GameManager.Instance.PrimaryPlayer.CurrentRoom.area.UnitCenter - GameManager.Instance.PrimaryPlayer.CurrentRoom.area.basePosition.ToVector2()).ToIntVector2(), false).transform.Find("Bot:Test_Npc").gameObject;
                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(shopObj.GetComponent<TalkDoerLite>());


                    //
                    File.WriteAllText("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Enter the Gungeon\\Resources\\EnterTheBeyond/fuckingdie.txt", UnityEngine.JsonUtility.ToJson(shopObj.GetComponent<PlayMakerFSM>(), true));

                    
                    Log("2");
                    return;
                    var room = RoomFactory.BuildFromResource("BotsMod/rooms/customnpctest.room");


                    RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.STANDARD);

                    Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions + new IntVector2(10, 10));



                    GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(2, 4)).ToVector2(), false, false);


                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("getItemInfo", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }



                    //var obj = Tools.LoadAssetFromAnywhere(args[0].Replace("_", " "));

                    var obj = Tools.brave.LoadAsset<GameObject>(args[0]);

                    if (obj != null)
                    {
                        foreach(var comp in (obj as GameObject).GetComponents<Component>())
                        {
                            Log($"{comp.GetType()}: {comp.name}");
                        }
                    } 
                    else
                    {
                        Log("no object found under \"" + args[0].Replace("_", " ") + "\"");
                    }

                });



                ETGModConsole.Commands.GetGroup("bot").AddUnit("past_kill", delegate (string[] args)
                {

                    GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST, true);
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("past_killnt", delegate (string[] args)
                {

                    GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST, false);
                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("past", delegate (string[] args)
                {
                    
                    GameManager.Instance.LoadCustomLevel("botfs_lost");
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("model", delegate (string[] args)
                {

                    var obj = UnityEngine.Object.Instantiate(Tools.BotsAssetBundle.LoadAsset<GameObject>("TestModel"), GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, new Quaternion());

                    obj.transform.localScale = new Vector3(3, 5, 5);

                    obj.AddComponent<RandomComps.MakeObjSpin>();
                });



                ETGModConsole.Commands.GetGroup("bot").AddUnit("AddPartsToEnemies", delegate (string[] args)
                {

                    LightningRounds.ApplyActionToNearbyEnemiesWithALimit(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter, 10, 30, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
                    {

                        if (enemy && enemy.healthHaver)
                        {
                            
                            var partSystem = UnityEngine.Object.Instantiate(Tools.BotsAssetBundle.LoadAsset<GameObject>("ParticleSystemObj"));
                            partSystem.transform.position = enemy.sprite.WorldCenter;
                            partSystem.transform.parent = enemy.transform;
                             partSystem.GetComponent<ParticleSystem>().gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
                            Log(enemy.name);

                        }


                    });
                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("burn", delegate (string[] args)
                {

                    LightningRounds.ApplyActionToNearbyEnemiesWithALimit(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter, 10, 30, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
                    {

                        if (enemy && enemy.healthHaver)
                        {
                            //-2510877616333010786
                            //-4066334418742256460
                            enemy.ApplyEffect(new CustomGameActorFireEffect
                            {
                                AffectsEnemies = true,
                                AffectsPlayers = false,
                                AppliesDeathTint = true,
                                AppliesOutlineTint = false,
                                AppliesTint = false,
                                PlaysVFXOnActor = false,
                                DamagePerSecondToEnemies = 10,
                                DeathTintColor = new Color32(128, 0, 128, 255),
                                TintColor = new Color(1, 0, 1, 1),
                                duration = 10,
                                effectIdentifier = "BeyondFire",
                                flameBuffer = new Vector2(0.0625f, 0.3f),
                                flameFpsVariation = 15,
                                flameMoveChance = 1,

                            }) ;

                        }


                    });
                });



                ETGModConsole.Commands.GetGroup("bot").AddUnit("breachRoom", delegate (string[] args)
                {
                    var orLoadByName_Orig5 = SpecialDungeon.GetOrLoadByNameOrig("Base_ResourcefulRat");
                    var room = RoomFactory.BuildFromResource("BotsMod/rooms/breachRoom.room");
                    room.overriddenTilesets = GlobalDungeonData.ValidTilesets.HELLGEON;
                    RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);

                    

                    Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);
                    GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(3, 4)).ToVector2(), false, false);

                    //UnityEngine.Object.Instantiate(SpriteBuilder.SpriteFromResource("BotsMod/sprites/TestBreachRoomTexture"), new Vector3(creepyRoom.area.basePosition.x, creepyRoom.area.basePosition.y), Quaternion.identity);
                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("beyond", delegate (string[] args)
                {

                    GameManager.Instance.LoadCustomLevel("beyond");
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("randomize_gun", delegate (string[] args)
                {

                    CompletlyRandomGun.DoRandomizeGun(CompletlyRandomGun.completlyRandomGun);
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("shield", delegate (string[] args)
                {

                    //var shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                    //var shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1,1,1), Quaternion.identity);
                    var shield3 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1, -1, -1), Quaternion.identity);
                    //shield2.gameObject.AddComponent<ShiledShader1>();
                    //shield3.gameObject.AddComponent<ShiledShader2>();

                    

                     
                    var stupidfuckingshopleasedie = shield3.gameObject.AddComponent<BaseShopController>();


                    stupidfuckingshopleasedie.baseShopType = BaseShopController.AdditionalShopType.NONE;
                    stupidfuckingshopleasedie.ShopCostModifier = 1000;
                    stupidfuckingshopleasedie.shopItemsGroup2 = Tools.LoadShopTable("Shop_Curse_Items_01");
                    stupidfuckingshopleasedie.shopItems = Tools.LoadShopTable("Shop_Blank_Items_01");
                    stupidfuckingshopleasedie.spawnGroupTwoItem2Chance = 1;

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("time", delegate (string[] args)
                {
                    Log(StringTableManager.EvaluateReplacementToken("%PLAYTIMEHOURS"));
                    
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("spawnitems", delegate (string[] args)
                {

                    var chest = Chest.Spawn(KeyChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));

                    var contents = new List<PickupObject>
                    {
                        PickupObjectDatabase.GetById(145),
                        PickupObjectDatabase.GetById(478),
                        PickupObjectDatabase.GetById(551),
                        PickupObjectDatabase.GetById(467),
                    };

                    var pos = new List<Vector3>
                    {
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(1,0),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(-1,0),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(0,1),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(0,-1),
                    };

                    List<DebrisObject> list = new List<DebrisObject>();

                    for (int i = 0; i < contents.Count; i++)
                    {
                        List<DebrisObject> list2 = LootEngine.SpewLoot(new List<GameObject> { contents[i].gameObject }, pos[i]);
                        list.AddRange(list2);
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (list2[j])
                            {
                                list2[j].PreventFallingInPits = true;
                            }
                            if (!(list2[j].GetComponent<Gun>() != null))
                            {
                                if (!(list2[j].GetComponent<CurrencyPickup>() != null))
                                {
                                    if (list2[j].specRigidbody != null)
                                    {
                                        list2[j].specRigidbody.CollideWithOthers = false;
                                        DebrisObject debrisObject = list2[j];
                                        debrisObject.OnTouchedGround += (Action<DebrisObject>)typeof(Chest).GetMethod("BecomeViableItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(chest, new object[] { debrisObject });
                                        //debrisObject.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnTouchedGround, new Action<DebrisObject>(this.BecomeViableItem));
                                    }
                                }
                            }
                        }
                    }

                });


                
                ETGModConsole.Commands.GetGroup("bot").AddUnit("findshops", delegate (string[] args)
                {
                    foreach (var shop in UnityEngine.Object.FindObjectsOfType<BaseShopController>())
                    {
                        Log(shop.gameObject.name + " has been found at " + shop.gameObject.transform.position);

                        foreach(Transform child in shop.gameObject.transform)
                        {
                            Log(child.name + ": " + child.gameObject.GetType() + " (" + child.gameObject.activeInHierarchy + ")");
                        }

                    }

                    Log("==========================");
                    foreach (var shop in StaticReferenceManager.AllShops)
                    {
                        Log(shop.name);
                    }

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("listlibraries", delegate (string[] args)
                {
                    if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee)
                    {
                        foreach(var libary in GameManager.Instance.PrimaryPlayer.GetComponent<CharacterAnimationRandomizer>().AnimationLibraries)
                        {
                            Log($"{libary}");
                        }
                    } 
                    else
                    {
                        Log("This command requires you are playing paradox");
                    }

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("flytest", delegate (string[] args)
                {
                    var room = RoomFactory.BuildFromResource("BotsMod/rooms/robotflytestroom.room");
                    RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);

                    Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



                    GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("FoyerSelectThing", delegate (string[] args)
                {
                    foreach (var foyerSelectFlag in UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>())
                    {
                        if (!foyerSelectFlag.name.ToLower().Contains("convict"))
                        {
                            continue;
                        }
                        Log($"============[{foyerSelectFlag.name}: Start]============");

                        foreach (var comp in foyerSelectFlag.GetComponents<Component>())
                        {
                            Log($"=========[{comp.name}: {comp.GetType()}]=========");
                            

                            PropertyInfo[] properties = comp.GetType().GetProperties();

                            foreach (PropertyInfo propertyInfo in properties)
                            {
                                string text = propertyInfo.DeclaringType.FullName + "::" + propertyInfo.Name;
                                if (propertyInfo.MemberType != MemberTypes.Method && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
                                {
                                    
                                    try
                                    {
                                        object value = ReflectionHelper.GetValue(propertyInfo, comp);
                                        Log(propertyInfo.Name + ": " + value.ToStringIfNoString());
                                    }
                                    catch (Exception message)
                                    {
                                        Debug.LogWarning("FoyerSelectThing: THIS LITTLE SHIT BROKE IT > " + text);
                                        Debug.LogWarning(message);
                                    }
                                }
                            }
                        }

                        Log($"============[{foyerSelectFlag.name}: End]============");

                    }
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("KillPillarsAreDumb", delegate (string[] args)
                {

                    foreach (var comp in EnemyDatabase.GetOrLoadByGuid("3f11bbbc439c4086a180eb0fb9990cb4").GetComponent<AIActorDummy>().realPrefab.GetComponentsInChildren<Component>())
                    {
                        Log($"=========[{comp.name}: {comp.GetType()}]=========");


                        /*PropertyInfo[] properties = comp.GetType().GetProperties();

                        foreach (PropertyInfo propertyInfo in properties)
                        {
                            string text = propertyInfo.DeclaringType.FullName + "::" + propertyInfo.Name;
                            if (propertyInfo.MemberType != MemberTypes.Method && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
                            {

                                try
                                {
                                    object value = ReflectionHelper.GetValue(propertyInfo, comp);
                                    Log(propertyInfo.Name + ": " + value.ToStringIfNoString());
                                }
                                catch (Exception message)
                                {
                                    Debug.LogWarning("KillPillarsAreDumb: THIS LITTLE SHIT BROKE IT > " + text);
                                    Debug.LogWarning(message);
                                }
                            }
                        }*/
                    }

                    

                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("shop", delegate (string[] args)
                {



                    var devilLootTable = UnityEngine.Object.Instantiate(Tools.shared_auto_001.LoadAsset<GenericLootTable>("Shop_Key_Items_01"));
                    devilLootTable.defaultItemDrops.elements.Clear();

                    devilLootTable.AddItemToPool(60);
                    devilLootTable.AddItemToPool(125);
                    devilLootTable.AddItemToPool(434);
                    devilLootTable.AddItemToPool(271);
                    devilLootTable.AddItemToPool(407);
                    devilLootTable.AddItemToPool(571);
                    devilLootTable.AddItemToPool(33);
                    devilLootTable.AddItemToPool(17);
                    devilLootTable.AddItemToPool(347);
                    devilLootTable.AddItemToPool(90);
                    devilLootTable.AddItemToPool(336);
                    devilLootTable.AddItemToPool(285);

                    var room = RoomFactory.BuildFromResource("BotsMod/rooms/npctest.room");


                    RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);

                    Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



                    GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

                    foreach (var shop in UnityEngine.Object.FindObjectsOfType<BaseShopController>())
                    {
                        Log(shop.gameObject.name);
                        if (shop.gameObject.name.Contains("Merchant_Key"))
                        {
                            Log("found");
                            shop.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;

                            shop.shopItems = devilLootTable;

                        }

                    }


                    return;
                    //GameObject asset = null;
                    //foreach (var bundle in StaticReferences.AssetBundles.Values)
                    //{
                    //    asset = bundle.LoadAsset<GameObject>("merchant_rat_placeable");
                    //    if (asset)
                    //       break;
                    //}

                    //var shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                    //var shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1,1,1), Quaternion.identity);
                    //var dumbFuckingRodent = BraveResources.Load<GameObject>("Merchant_Rat");

                    var sharedAssets1 = ResourceManager.LoadAssetBundle("shared_auto_001");
                    var asset = sharedAssets1.LoadAsset<GameObject>("Merchant_Key");



                 

                    asset.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(asset);
                    UnityEngine.Object.DontDestroyOnLoad(asset);

                    UnityEngine.Object.Instantiate(asset, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);
                    //ToolsGAPI.Print($"Added {asset.name} to room.");
                    

                    GameObject dumbFuckingRodent = UnityEngine.Object.Instantiate(asset);
                    dumbFuckingRodent.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodent);
                    UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodent);

                    UnityEngine.Object.Instantiate(dumbFuckingRodent, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);

                    return;
                    dumbFuckingRodent.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodent);
                    UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodent);

                    UnityEngine.Object.DestroyImmediate(dumbFuckingRodent.GetComponent<BaseShopController>());

                    var dumbFuckingRodentsDumbFuckingShop = dumbFuckingRodent.AddComponent<BaseShopController>();

                    var itemPos1 = new GameObject("EAST item pos");
                    itemPos1.transform.localPosition = new Vector3(1.125f, 2.375f, 1f);
                    itemPos1.transform.position = new Vector3(1.125f, 2.375f, 1f);
                    itemPos1.transform.localScale = new Vector3(1f, 1f, 1f);



                    var itemPos2 = new GameObject("WEST item pos");
                    itemPos2.transform.localPosition = new Vector3(2.625f, 1f, 1f);
                    itemPos2.transform.position = new Vector3(1.125f, 2.375f, 1f);
                    itemPos2.transform.localScale = new Vector3(1f, 1f, 1f);


                    var itemPos3 = new GameObject("NORTH item pos");
                    itemPos3.transform.localPosition = new Vector3(4.125f, 2.375f, 1f);
                    itemPos3.transform.position = new Vector3(1.125f, 2.375f, 1f);
                    itemPos3.transform.localScale = new Vector3(1f, 1f, 1f);

                    dumbFuckingRodentsDumbFuckingShop.placeableHeight = 5;
                    dumbFuckingRodentsDumbFuckingShop.placeableWidth = 5;
                    dumbFuckingRodentsDumbFuckingShop.difficulty = 0;
                    dumbFuckingRodentsDumbFuckingShop.isPassable = true;
                    dumbFuckingRodentsDumbFuckingShop.baseShopType = BaseShopController.AdditionalShopType.KEY;
                    dumbFuckingRodentsDumbFuckingShop.FoyerMetaShopForcedTiers = false;
                    dumbFuckingRodentsDumbFuckingShop.BeetleExhausted = false;
                    dumbFuckingRodentsDumbFuckingShop.ExampleBlueprintPrefab = null;
                    dumbFuckingRodentsDumbFuckingShop.shopItemsGroup2 = null;


                    //BotsMod/JSONShit/PlayMakerFSM #11743.json
                    var playMakerFSMTest = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/PlayMakerFSM #11743.json");

                    //BotsModule.Log(text, BotsModule.TEXT_COLOR);

                    PlayMakerFSM playMakerFSM = new PlayMakerFSM();

                    JsonUtility.FromJsonOverwrite(playMakerFSMTest, playMakerFSM);

                    dumbFuckingRodentsDumbFuckingShop.shopkeepFSM = playMakerFSM;

                    //dumbFuckingRodentsDumbFuckingShop.shopItems = LootTables.testShopLable;

                    dumbFuckingRodentsDumbFuckingShop.shopItems = Tools.LoadShopTable("Shop_Curse_Items_01");
                    dumbFuckingRodentsDumbFuckingShop.spawnPositions = new Transform[]
                    {
                        itemPos1.transform,
                        itemPos2.transform,
                        itemPos3.transform,

                    };
                    //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem1Chance = 0.5f;
                    //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem2Chance = 0.5f;
                    //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem3Chance = 0.5f;

                    dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cloak", new GameObject("StupidFuckingShadowCunt"));
                    dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab);
                    UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab);


                    dumbFuckingRodentsDumbFuckingShop.ShopCostModifier = 1000;

                    itemPos1.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos1);
                    UnityEngine.Object.DontDestroyOnLoad(itemPos1);

                    itemPos2.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos2);
                    UnityEngine.Object.DontDestroyOnLoad(itemPos2);

                    itemPos3.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos3);
                    UnityEngine.Object.DontDestroyOnLoad(itemPos3);

                    var swapper = UnityEngine.Object.Instantiate(dumbFuckingRodent, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity).GetComponent<BaseShopController>();
                    int i = 0;
                    foreach(Transform transform in swapper.spawnPositions)
                    {
                        if (transform == null)
                        {
                            Log(i + " : is null ;-;");
                        }
                        else if (transform != null)
                        {
                            Log(i + " : isn't null :)");
                        }
                        i++;
                    }

                    //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                    //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();
                    //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper);


                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("nomorebadguys", delegate (string[] args)
                {
                    var user = GameManager.Instance.PrimaryPlayer;
                    
                    if (user.CurrentRoom != null)
                    {
                        List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                        foreach (AIActor enemy in activeEnemies)
                        {
                            if (enemy != null)
                            {
                                enemy.CanTargetPlayers = false;
                            }

                        }
                    }



                });
                

                ETGModConsole.Commands.GetGroup("bot").AddUnit("spawn", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1, 2))
                    {
                        return;
                    }
                    if (!GameManager.Instance.PrimaryPlayer)
                    {
                        ETGModConsole.Log("Couldn't access Player Controller", false);
                        return;
                    }
                    string text = args[0];
                    if (!Game.Items.ContainsID(text))
                    {
                        ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
                        return;
                    }
                    ETGModConsole.Log(string.Concat(new object[]
                    {
                        "Attempting to spawn item ID ",
                        args[0],
                        " (numeric ",
                        text,
                        "), class ",
                        Game.Items.Get(text).GetType()
                    }), false);
                    if (args.Length == 2)
                    {
                        int num = int.Parse(args[1]);
                        for (int i = 0; i < num; i++)
                        {
                            IPlayerInteractable[] interfacesInChildren2 = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(LootEngine.SpawnItem(Game.Items[text].gameObject, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Vector2.zero, 0).gameObject);
                            for (int j = 0; j < interfacesInChildren2.Length; j++)
                            {
                                GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren2[j]);
                            }


                        }
                        return;
                    }
                    var gameObject2 = LootEngine.SpawnItem(Game.Items[text].gameObject, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Vector2.zero, 0);
                    IPlayerInteractable[] interfacesInChildren = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(gameObject2.gameObject);
                    for (int i = 0; i < interfacesInChildren.Length; i++)
                    {
                        GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren[i]);
                    }

                }, _GiveAutocompletionSettings);

                ETGModConsole.Commands.GetGroup("bot").AddUnit("lost_unlocks", delegate (string[] args)
                {

                    Log("Killed Past: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST), LOST_COLOR);
                    Log("Killed Past Alt: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME), LOST_COLOR);


                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("swapperActive", delegate (string[] args)
                {
                    try
                    {

                        foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<tk2dSprite>())
                        {
                            if (dumbpeiceofshit.gameObject.name.Contains("costume"))
                            {
                                BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.gameObject.transform.position}");
                                if (dumbpeiceofshit.gameObject.activeSelf == false)
                                {
                                    
                                    dumbpeiceofshit.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        BotsModule.Log("swapper dump broke", "#eb1313");
                        BotsModule.Log(string.Format(e + ""), "#eb1313");
                    }
                });
                /*
                 * 
			
                 * 
                 * 
                */

                ETGModConsole.Commands.GetGroup("bot").AddUnit("doReloadThing", delegate (string[] args)
                {

                    var reloadText = GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.AddComponent<CustomReloadText>();

                    reloadText.customReloadMessage = "deez nuts";

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("ihateunity", delegate (string[] args)
                {
                    int i = 0;
                    foreach(var comp in GameManager.Instance.PrimaryPlayer.gameObject.GetComponentsInChildren<MonoBehaviour>())
                    {
                        Log(comp.GetType().ToString());
                        try
                        {
                            var path = "C:\\Users\\noaht\\Documents\\unity\\stop crashing pls\\Assets\\fuck\\" + i + comp.GetType().ToString() + ".txt";

                            if (!File.Exists(path))
                            {
                                var f = File.Create(path);
                                f.Close();
                            }
                            File.WriteAllText(path, UnityEngine.JsonUtility.ToJson(comp));
                            i++;
                        }
                        catch(Exception e)
                        {
                            Log(e.ToString());
                        }
                        
                        
                        
                    }

                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("findgun", delegate (string[] args)
                {


                    foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<DebrisObject>())
                    {
                        if (dumbpeiceofshit.name.ToLower().Contains("gun"))
                        {
                            BotsModule.Log($"{dumbpeiceofshit.name}");
                            //GetComponentInChildren
                            foreach (var comp in dumbpeiceofshit.GetComponents<Component>())
                            {
                                Log($"=========[{comp.name}: {comp.GetType()}]=========");
                            }
                            BotsModule.Log($"children comps:");

                            
                            foreach (var comp in dumbpeiceofshit.GetComponentsInChildren <Component>())
                            {
                                Log($"=========[{comp.name}: {comp.GetType()}]=========");
                            }

                        }

                        
                    }

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("findfoyershit", delegate (string[] args)
                {


                    foreach (var a in UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>())
                    {
                        BotsModule.Log("From the foreach for array: " + a.name);
                    }

                });

                

                ETGModConsole.Commands.GetGroup("bot").AddUnit("swapper", delegate (string[] args)
                {
                    try
                    {

                        var baseSwapper = FakePrefab.Clone(ResourceManager.LoadAssetBundle("foyer_002").LoadAsset<GameObject>("costume_guide"));
                        var altSwapper = FakePrefab.Clone(ResourceManager.LoadAssetBundle("foyer_002").LoadAsset<GameObject>("costume_guide_alt"));

                        var sprite = baseSwapper.GetComponent<tk2dSprite>();
                        var altSprite = altSwapper.GetComponent<tk2dSprite>();

                        Log("objects set up");

                        sprite.SetSprite(sprite.Collection, sprite.spriteId);
                        altSprite.SetSprite(altSprite.Collection, altSprite.spriteId);

                        baseSwapper.transform.parent = altSwapper.transform;

                        altSwapper.name = "costume_lost_alt";

                        baseSwapper.name = "costume_lost";

                        Log("renamed");
                        var characterCostumeSwapper = baseSwapper.GetComponent<CharacterCostumeSwapper>();
                        Log("variables set0");
                        characterCostumeSwapper.TargetCharacter = (PlayableCharacters)CustomPlayableCharacters.Custom;
                        Log("variables set1");
                        characterCostumeSwapper.AlternateCostumeSprite = altSprite;
                        Log("variables set2");
                        characterCostumeSwapper.CostumeSprite = sprite;
                        Log("variables set3");
                        characterCostumeSwapper.HasCustomTrigger = false;
                        characterCostumeSwapper.CustomTriggerIsFlag = false;
                        characterCostumeSwapper.TriggerFlag = GungeonFlags.NONE;
                        characterCostumeSwapper.CustomTriggerIsSpecialReserve = false;
                        Log("variables set4");
                        characterCostumeSwapper.TargetLibrary = LostAltSkinAnimator;
                        Log("variables set5");



                        //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(UnityEngine.Object.Instantiate(altSwapper, new Vector3(16.4f, 25.1f, 25.6f), Quaternion.identity).GetComponent<IPlayerInteractable>());
                       // GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(UnityEngine.Object.Instantiate(baseSwapper, new Vector3(16.4f, 25.1f, 25.6f), Quaternion.identity).GetComponent<IPlayerInteractable>());

                        var baseObj = UnityEngine.Object.Instantiate(baseSwapper, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);
                        var altObj = UnityEngine.Object.Instantiate(altSprite, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);

                        baseObj.gameObject.SetActive(true);
                        altObj.gameObject.SetActive(true);

                        if (!Dungeonator.RoomHandler.unassignedInteractableObjects.Contains(baseObj.GetComponent<IPlayerInteractable>()))
                            Dungeonator.RoomHandler.unassignedInteractableObjects.Add(baseObj.GetComponent<IPlayerInteractable>());

                        if (!Dungeonator.RoomHandler.unassignedInteractableObjects.Contains(altObj.GetComponent<IPlayerInteractable>()))
                            Dungeonator.RoomHandler.unassignedInteractableObjects.Add(altObj.GetComponent<IPlayerInteractable>());


                        Log("finished!");
                        /*foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<tk2dSprite>())
                        {
                            if (dumbpeiceofshit.gameObject.name.Contains("costume"))
                            {
                                /*if (dumbpeiceofshit.gameObject.name == "costume_guide_alt")
                                {
                                    //altSwapper = FakePrefab.Clone(dumbpeiceofshit.gameObject);
                                    altSwapper = dumbpeiceofshit.gameObject;

                                    dumbpeiceofshit.SetSprite(altSprite.Collection, altSprite.spriteId);


                                    

                                    altSwapper.transform.position = GameManager.Instance.PrimaryPlayer.sprite.WorldCenter;

                                    BotsModule.Log($"{altSwapper.name}: {altSwapper.transform.position}");
                                    BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.Collection.spriteDefinitions[dumbpeiceofshit.gameObject.GetComponent<tk2dSprite>().spriteId].name}");
                                }*/


                        /*if (dumbpeiceofshit.gameObject.name == "costume_guide")
                        {
                            //baseSwapper = FakePrefab.Clone(dumbpeiceofshit.gameObject);
                            baseSwapper = dumbpeiceofshit.gameObject;


                            dumbpeiceofshit.SetSprite(sprite.Collection, sprite.spriteId);

                            baseSwapper.name = "costume_lost";
                            var characterCostumeSwapper = baseSwapper.GetComponent<CharacterCostumeSwapper>();
                            characterCostumeSwapper.TargetCharacter = (PlayableCharacters)CustomPlayableCharacters.Custom;
                            //characterCostumeSwapper.AlternateCostumeSprite = altSwapper.GetComponent<tk2dSprite>();
                            //characterCostumeSwapper.CostumeSprite = baseSwapper.GetComponent<tk2dSprite>();
                            characterCostumeSwapper.TargetLibrary = GameManager.Instance.PrimaryPlayer.AlternateCostumeLibrary;


                            baseSwapper.transform.position = GameManager.Instance.PrimaryPlayer.sprite.WorldCenter;

                            //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(UnityEngine.Object.Instantiate(swapper, new Vector3(16.4f, 25.1f, 25.6f), Quaternion.identity).GetComponent<IPlayerInteractable>());



                            //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper.GetComponent<IPlayerInteractable>());

                            BotsModule.Log($"{baseSwapper.name}: {baseSwapper.transform.position}");
                            BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.Collection.spriteDefinitions[dumbpeiceofshit.gameObject.GetComponent<tk2dSprite>().spriteId].name}");
                        }


                        //BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.gameObject.transform.position}");
                        //BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.Collection.spriteDefinitions[dumbpeiceofshit.gameObject.GetComponent<tk2dSprite>().spriteId].name}");
                    }
                }*/
                    }
                    catch (Exception e)
                    {
                        BotsModule.Log("swapper broke", "#eb1313");
                        BotsModule.Log(string.Format(e + ""), "#eb1313");
                    }
                });


                //$"<color={color}>{text}</color>"


                ETGModConsole.Commands.GetGroup("bot").AddUnit("listitemchance", delegate (string[] args)
                {
                    for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
                    {
                        if (PickupObjectDatabase.Instance.Objects[i] != null)
                        {
                            //GetItemForPlayer
                            var player = GameManager.Instance.PrimaryPlayer;
                            if (PickupObjectDatabase.Instance.Objects[i] is Gun)
                            {
                                var gunClass = (PickupObjectDatabase.Instance.Objects[i] as Gun).gunClass;
                                int num4 = (!(player == null)) ? player.inventory.ContainsGunOfClass(gunClass, true) : 0;
                                float modifierForClass = LootDataGlobalSettings.Instance.GetModifierForClass(gunClass);
                                ETGModConsole.Log($"Name: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].EncounterNameOrDisplayName}</color>, Id: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].PickupObjectId}</color>, Chance: <color={"#03fc0b"}>{RewardManager.GetMultiplierForItem(PickupObjectDatabase.Instance.Objects[i], player, false)}/{Mathf.Pow(modifierForClass, (float)num4)}</color>, Class: <color={"#03fc0b"}>{gunClass}</color>");

                            } else
                            {

                                ETGModConsole.Log($"Name: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].EncounterNameOrDisplayName}</color>, Id: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].PickupObjectId}</color>, Chance: <color={"#03fc0b"}>{RewardManager.GetMultiplierForItem(PickupObjectDatabase.Instance.Objects[i], player, false)}</color>");
                            }


                            
                            
                        }
                    }

                    

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("crime", delegate (string[] args)
                {
                    AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("foyer_002");
                    var idk = assetBundle3.LoadAsset<GameObject>("costume_bullet_shotgun_psspecial_alt");
                    var swapper = UnityEngine.GameObject.Instantiate(idk, GameManager.Instance.PrimaryPlayer.sprite.WorldTopCenter, Quaternion.identity);
                    //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                    //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();

                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper.GetComponent<CharacterCostumeSwapper>());
                    BotsModule.Log("Position: " + swapper.transform.position);

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("brave_asset_bundle_objects", delegate (string[] args)
                {
                    
                    AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("brave_resources_001");
                    foreach (string str in assetBundle3.GetAllAssetNames())
                    {
                        ETGModConsole.Log(str);
                    }
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("foyer2_asset_bundle_objects", delegate (string[] args)
                {

                    AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("foyer_002");
                    foreach (string str in assetBundle3.GetAllAssetNames())
                    {
                        ETGModConsole.Log(str);
                    }
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("audioSwitch", delegate (string[] args)
                {
                    var player = GameManager.Instance.PrimaryPlayer;
                    player.OverridePlayerSwitchState = args[0];
                    ETGModConsole.Log(args[0]);
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("room", delegate (string[] args)
                {
                    Log(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRoomName(), TEXT_COLOR);
                    

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("chest", delegate (string[] args)
                {
                    Chest.Spawn(KeyChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));
                });


                ETGModConsole.Commands.GetGroup("bot").AddUnit("giveSoulHeart", delegate (string[] args)
                {
                    SoulHeartController.soulHeartCount++;
                    //GameManager.Instance.PrimaryPlayer.healthHaver.SetHealthMaximum(GameManager.Instance.PrimaryPlayer.healthHaver.GetMaxHealth() + 1);
                    GameUIRoot.Instance.UpdatePlayerHealthUI(GameManager.Instance.PrimaryPlayer, GameManager.Instance.PrimaryPlayer.healthHaver);
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("item_info", new Action<string[]>(this.ItemInfo), _ItemIdAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("bot").AddUnit("enemy_info", new Action<string[]>(this.EnemyInfo), _SpawnAutocompletionSettings);

                

                ETGModConsole.Commands.GetGroup("bot").AddUnit("quickstartinfo", delegate (string[] args)
                {
                    Log(GameManager.Options.LastPlayedCharacter.ToString());
                    Log(GameManager.Options.PreferredQuickstartCharacter.ToString());
                    Log(GameManager.LastUsedPlayerPrefab.name);

                    Log(GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>().characterIdentity.ToString());
                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("docool", delegate (string[] args)
                {
                    GameManager.Instance.Dungeon.roomMaterialDefinitions[GameManager.Instance.PrimaryPlayer.CurrentRoom.RoomVisualSubtype] = BeyondPrefabs.shared_auto_002.LoadAsset<DungeonMaterial>("Boss_Cathedral_StainedGlass_Lights");
                });
                


                ETGModConsole.Commands.GetGroup("bot").AddUnit("asset_bundle_objects", delegate (string[] args)
                {
                    ETGModConsole.Log("===================================");
                    foreach (string str in Tools.AHHH.GetAllAssetNames())
                    {
                        ETGModConsole.Log(Tools.AHHH.name + ": " + str);
                    }
                    ETGModConsole.Log("===================================");
                    foreach (string str in Tools.BotsAssetBundle.GetAllAssetNames())
                    {
                        ETGModConsole.Log(Tools.BotsAssetBundle.name + ": " + str);
                    }
                    ETGModConsole.Log("===================================");
                });

                ETGModConsole.Commands.AddGroup("bot_saveapi");
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_flag", delegate (string[] args)
                {
                    ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_LOST_UNLOCKED).ToString());                    
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_flag", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }
                    SaveAPIManager.SetFlag(CustomDungeonFlags.BOT_LOST_UNLOCKED, bool.Parse(args[0]));
                    ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's new value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_LOST_UNLOCKED).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_stat", delegate (string[] args)
                {
                    ETGModConsole.Log("CustomTrackedStats.BOT_LOST_UNLOCKED's value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_stat", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }
                    SaveAPIManager.SetStat(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                    ETGModConsole.Log("CustomTrackedStats.BOT_LOST_UNLOCKED's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("increment_stat", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }
                    SaveAPIManager.RegisterStatChange(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                    ETGModConsole.Log("CustomTrackedStats.BOT_LOST_UNLOCKED's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_maximum", delegate (string[] args)
                {
                    ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_maximum", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }
                    SaveAPIManager.UpdateMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM, float.Parse(args[0]));
                    ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's new value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_char_flag", delegate (string[] args)
                {
                    ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
                });
                ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_char_flag", delegate (string[] args)
                {
                    if (!ETGModConsole.ArgCount(args, 1))
                    {
                        return;
                    }
                    SaveAPIManager.SetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG, bool.Parse(args[0]));
                    ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's new value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
                });

                ETGModConsole.Commands.AddGroup("floor", args =>
                {
                });
                ETGModConsole.Commands.GetGroup("floor").AddUnit("load", this.LoadFloor);

                ETGModConsole.Commands.AddGroup("past", args =>
                {
                });
                ETGModConsole.Commands.GetGroup("past").AddUnit("load", this.LoadPast);

                ModLogo = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/title_words_beyond_001.png");
                try
                {
                    MainMenuFoyerUpdateHook = new Hook(
                        typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
                        typeof(BotsModule).GetMethod("MainMenuUpdateHook", BindingFlags.NonPublic | BindingFlags.Instance),
                        typeof(MainMenuFoyerController)

                    );
                }
                catch (Exception ex)
                {

                    Debug.LogException(ex);
                    return;
                }





                ETGMod.StartGlobalCoroutine(this.DelayedStartCR());
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
            catch (Exception arg)
            {
                Log("[Bots Mod]: Something in the start method broke heres why: " + arg, "#eb1313");
            }
        }
        public static Texture2D ModLogo;
        public static tk2dSpriteAnimation LostAltSkinAnimator;
        public static Hook MainMenuFoyerUpdateHook;
        dfAtlas df;



        public IEnumerator DelayedStartCR()
        {
            yield return null;
            this.DelayedStart();
            yield break;
        }

        public static List<GameUIAmmoType> addedAmmoTypes = new List<GameUIAmmoType>();

        private void GameManager_Awake(Action<GameManager> orig, GameManager self)
        {
            orig(self);
            BeyondDungeon.InitCustomDungeon();
            LostPastDungeon.InitCustomDungeon();
        }


        private void LoadFloor(string[] obj)
        {
            GameManager.Instance.LoadCustomLevel(BeyondDungeon.BeyondDefinition.dungeonSceneName);
        }

        private void LoadPast(string[] obj)
        {
            GameManager.Instance.LoadCustomLevel(LostPastDungeon.LostPastDefinition.dungeonSceneName);
        }

        public void DelayedStart()
        {

            GameObject obj = new GameObject("InfinityAmmoTypeSprite").ProcessGameObject();
            dfTiledSprite sprite = obj.AddComponent<dfTiledSprite>();
            sprite.Atlas = ((dfFont)typeof(UINotificationController).GetField("EnglishFont", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(GameUIRoot.Instance.notificationController)).Atlas;
            sprite.IsLocalized = false;
            sprite.Size = Vector2.zero;
            sprite.SpriteName = "infinite-big";
            GameUIAmmoType uiammotype = new GameUIAmmoType
            {
                ammoBarBG = sprite,
                ammoBarFG = sprite,
                ammoType = GameUIAmmoType.AmmoType.CUSTOM,
                customAmmoType = "infinity"
            };

            addedAmmoTypes.Add(uiammotype);
            foreach (GameUIAmmoController uiammocontroller in GameUIRoot.Instance.ammoControllers)
            {
                Tools.Add(ref uiammocontroller.ammoTypes, uiammotype);
            }
        }
            private void MainMenuUpdateHook(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
        {
            orig(self);
            if (self.TitleCard != null)
            {
                if (((dfTextureSprite)self.TitleCard).Texture.name != ModLogo.name)
                {
                    ((dfTextureSprite)self.TitleCard).Texture = ModLogo;
                    //LogoEnabled = true;
                }
            }

        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
        private static bool hasInitialized;

        public static void LateStart(Action<Foyer> orig, Foyer self)
        {
            orig(self);

            try
            {
                

                ToolsGAPI.Print("Late start called");
                if (hasInitialized) return;
                ToolsGAPI.StartTimer("Initializing mod");

                //Loader.Init();

                ToolsGAPI.StopTimerAndReport("Initializing mod");
                hasInitialized = true;

                BotsModule.Log("List of Custom Characters From Enter the Beyond:", LOST_COLOR);
                
                foreach (var character in CharacterBuilder.storedCharacters)
                {
                    BotsModule.Log("    " + (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? "[Locked] " : "") + character.Value.First.nameShort, (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? LOCKED_CHARACTOR_COLOR : "#00ff44") );                  
                }

            }
            catch (Exception e)
            {
                BotsModule.Log("(late start) Characters Broke (crying is a valid response)", "#eb1313");
                BotsModule.Log(string.Format(e + ""), "#eb1313");
            }

        }

        private void ItemInfo(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1))
            {
                return;
            }
            string text = args[0];

            foreach(var comp in PickupObjectDatabase.GetById(int.Parse(text)) .gameObject.GetComponents<Component>())
            {
                ETGModConsole.Log($"{comp.GetType()}: {comp.name}");
            }


        }

        public void EnemyInfo(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1, 1))
            {
                return;
            }
            string text = args[0];
            if (!Game.Enemies.ContainsID(text))
            {
                ETGModConsole.Log(string.Format("Enemy with ID {0} doesn't exist", text), false);
                return;
            }
            AIActor enemyPrefab = Game.Enemies[text];
            //ETGModConsole.Log(text, false);

            EnemyTools.DebugInformation(enemyPrefab.behaviorSpeculator);


        }

        public AssetBundle LoadAssetBundleFromLiterallyAnywhere(string name)
        {
            AssetBundle assetBundle = null;
            if (File.Exists(this.Metadata.Archive))
            {
                ZipFile ModZIP = ZipFile.Read(this.Metadata.Archive);
                if (ModZIP != null && ModZIP.Entries.Count > 0)
                {
                    foreach (ZipEntry entry in ModZIP.Entries)
                    {
                        if (entry.FileName == name)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                entry.Extract(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                assetBundle = AssetBundle.LoadFromStream(ms);
                                break;
                            }
                        }
                    }
                }
            }
            else if (File.Exists(this.Metadata.Directory + "/" + name))
            {
                try
                {
                    assetBundle = AssetBundle.LoadFromFile(this.Metadata.Directory + "/" + name);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed loading asset bundle from file.");
                    Debug.LogError(ex.ToString());
                }
            }
            else
            {
                Debug.LogError("AssetBundle NOT FOUND!");
            }
            return assetBundle;
        }



        protected static AutocompletionSettings _GiveAutocompletionSettings;

        protected static AutocompletionSettings _ItemIdAutocompletionSettings;

        protected static AutocompletionSettings _SpawnAutocompletionSettings;
        public override void Exit() { }
        public override void Init() 
        {
            SaveAPIManager.Setup("bot");
        }

        
    }
}
