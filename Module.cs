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

        public static GameObject WarCrime;
        public static Chest KeyChest;
        public static PickupObject WarCrime2;
        public static GameObject Shop;


        public static string ZipFilePath;
        public static string FilePath;


        public static string characterFilePath;


        public static Gun ChamberGun;



        public override void Start()
        {

            try
            {

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

                GungeonAPI.FakePrefabHooks.Init();
                ToolsGAPI.Init();
                CustomCharacters.Hooks.Init();
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
                OverseerDecoy.Init();
                TestActive.Init();
                SpecialDungeon.Init();
                SpecialDungeon2CozFuckYou.Init();
                RichPresenceItem.Register();

                CompletlyRandomGun.Add();

                PossetionItem.Init();

                LostFriend.Init();

                PortalThing.Init();
                NoTimeToExplain.Add();

                LostsCloak.Init();

                //LootTables.Init();

                RedKey.Init();

                LostSidearm.Add();

                TestPassive.Init();

                LightningRounds.Init();

                Sin.Init();

                Roomba.Init();

                //GameManager.Instance.PrimaryPlayer.star

                InitSynergies.Init();

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


                var bulletGun = (PickupObjectDatabase.GetById(503) as Gun);
                var shotgunGun = (PickupObjectDatabase.GetById(512) as Gun);
                var anotherGun = (PickupObjectDatabase.GetById(169) as Gun);

                var moreBullet = bulletGun.DefaultModule.projectiles[0].gameObject.GetComponent<SpawnProjModifier>();
                var moreBulletShotgun = shotgunGun.DefaultModule.projectiles[0].gameObject.GetComponent<SpawnProjModifier>();

                moreBullet.projectileToSpawnInFlight = bulletGun.DefaultModule.projectiles[0];


                AlphabetSoupSynergyProcessor alphabetSoupSynergyProcessor = PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>();

                foreach (AlphabetSoupEntry entry in alphabetSoupSynergyProcessor.Entries)
                {
                    Log(entry.BaseProjectile.name, TEXT_COLOR);
                    
                };
                Log(((Gun)PickupObjectDatabase.GetById(340)).DefaultModule.projectiles[0].name, TEXT_COLOR);


                AlphabetSoupEntry iShouldntHaveBeenGivenThisPower6 = new AlphabetSoupEntry
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

                var funnylist = alphabetSoupSynergyProcessor.Entries.ToList();
                funnylist.Add(iShouldntHaveBeenGivenThisPower6);

                
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


                ETGModConsole.Commands.GetGroup("bot").AddUnit("shop", delegate (string[] args)
                {

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
                    var asset = BraveResources.Load<GameObject>("merchant_rat_placeable");

                    asset.SetActive(false);
                    ItemAPI.FakePrefab.MarkAsFakePrefab(asset);
                    UnityEngine.Object.DontDestroyOnLoad(asset);

                    UnityEngine.Object.Instantiate(asset, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);
                    //ToolsGAPI.Print($"Added {asset.name} to room.");
                    return;

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


                ETGModConsole.Commands.GetGroup("bot").AddUnit("lost_unlocks", delegate (string[] args)
                {

                    Log("Killed Past: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST), LOST_COLOR);
                    Log("Killed Past Alt: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME), LOST_COLOR);


                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("swapper", delegate (string[] args)
                {

                    var swapper = UnityEngine.Object.Instantiate(BotsModule.costumeSwapper, GameManager.Instance.PrimaryPlayer.sprite.WorldTopCenter, Quaternion.identity).GetComponent<LostCharacterCostumeSwapper>();
                    //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                    //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();
                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper);
                    BotsModule.Log("Position: " + swapper.transform.position);

                });

                ETGModConsole.Commands.GetGroup("bot").AddUnit("swapper_dupe", delegate (string[] args)
                {
                    AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("foyer_002");
                    var idk = assetBundle3.LoadAsset<GameObject>("costume_guide");
                    //idk.GetComponent<CharacterCostumeSwapper>().TargetCharacter = (PlayableCharacters)CustomPlayableCharacters.Lost;
                    
                    
                    //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                    //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();
                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(UnityEngine.Object.Instantiate(idk, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity).GetComponent<IPlayerInteractable>());
                    //BotsModule.Log("Position: " + swapper.transform.position);

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

                ETGModConsole.Commands.GetGroup("bot").AddUnit("item_info", new Action<string[]>(this.ItemInfo), _GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("bot").AddUnit("enemy_info", new Action<string[]>(this.EnemyInfo), _SpawnAutocompletionSettings);

                

                ETGModConsole.Commands.GetGroup("bot").AddUnit("asset_bundle_objects", delegate (string[] args)
                {
                    foreach (string str in Tools.AHHH.GetAllAssetNames())
                    {
                        ETGModConsole.Log(str);
                    }
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
                
                Tools.AHHH = this.LoadAssetBundleFromLiterallyAnywhere();
                ETGMod.StartGlobalCoroutine(this.DelayedStartCR());
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
            catch (Exception arg)
            {
                Log("[Bots Mod]: Something in the start method broke heres why: " + arg, "#eb1313");
            }
        }
        public static Texture2D ModLogo;
        public static Hook MainMenuFoyerUpdateHook;
        dfAtlas df;

        public IEnumerator DelayedStartCR()
        {
            yield return null;
            this.DelayedStart();
            yield break;
        }

        public static List<GameUIAmmoType> addedAmmoTypes = new List<GameUIAmmoType>();

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

                Loader.Init();

                ToolsGAPI.StopTimerAndReport("Initializing mod");
                hasInitialized = true;

                //ToolsGAPI.Print($"Custom Character Mod v'LIGMA FUCKIN BALLS *dab*' Initialized", "#00FF00", true);
                BotsModule.Log("List of Custom Characters From Enter the Beyond:", LOST_COLOR);
                //ToolsGAPI.Print(, LOST_COLOR, true);
                
                foreach (var character in CharacterBuilder.storedCharacters)
                {
                    BotsModule.Log("    " + (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? "[Locked] " : "") + character.Value.First.nameShort, (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? LOCKED_CHARACTOR_COLOR : character.Value.First.color) );                  
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
            ETGModConsole.Log(string.Concat(new object[]{"Attempting to spawn item ID ", args[0], " (numeric ", text,"), class ", Game.Items.Get(text).GetType() }), false);

            ETGModConsole.Log(PickupObjectDatabase.GetByEncounterName(text).sprite.renderer.material.shader.name, false);


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

        public AssetBundle LoadAssetBundleFromLiterallyAnywhere()
        {
            AssetBundle assetBundle = null;
            if (File.Exists(this.Metadata.Archive))
            {
                ZipFile ModZIP = ZipFile.Read(this.Metadata.Archive);
                if (ModZIP != null && ModZIP.Entries.Count > 0)
                {
                    foreach (ZipEntry entry in ModZIP.Entries)
                    {
                        if (entry.FileName == "customglitchshader")
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
            else if (File.Exists(this.Metadata.Directory + "/customglitchshader"))
            {
                try
                {
                    assetBundle = AssetBundle.LoadFromFile(this.Metadata.Directory + "/customglitchshader");
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

        protected static AutocompletionSettings _SpawnAutocompletionSettings;
        public override void Exit() { }
        public override void Init() 
        {
            SaveAPIManager.Setup("bot");
        }

        
    }
}
