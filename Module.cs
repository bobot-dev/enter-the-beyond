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
//using AmmonomiconAPI;
using HutongGames.PlayMaker;
using BotsMod.NPCs;
using BotsMod.ToolsAndStuff;
using LiveRecolor;
using NpcApi;
using ChamberGunApi;
using static BotsMod.RandomComps;
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
        public static BaseShopController shop;
        public static BaseShopController shop2;

        public static GameObject WarCrime;

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

                //GameManager.Instance.gameObject.AddComponent<DiscordController>();

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
                AmmonomiconAPI.Tools.Init(); // <--- fuck you 
                Ammonomicon.Init();

                CustomClipAmmoTypeToolbox.Init();

                BeyondPrefabs.InitCustomPrefabs();

                //ETGModMainBehaviour.Instance.gameObject.AddComponent<UiTesting>().Init();

                //my stuff
                Tools.Init();
                Hooks.Init();
                Rooms.Init();
                //BeyondPrefabs.Init();
                //RoomPrefabs.Init();
                CustomFire.Init();
                BeyondNotificaionHandler.Init();

                

                Tools.AHHH = this.LoadAssetBundleFromLiterallyAnywhere("coolshader");
                Tools.fucktilesets = this.LoadAssetBundleFromLiterallyAnywhere("fucktilesets");
                Tools.EtbAssetBundle = this.LoadAssetBundleFromLiterallyAnywhere("enterthebeyond");

                Tools.BotsAssetBundle = this.LoadAssetBundleFromLiterallyAnywhere("botsassetbundle");

                SoulHeartController.Init();

                FloorHooks.Init();

                //StuffIStoleFromApacheForChallengeMode.Init();

                //var testroom = RoomFactory.BuildFromResource("BotsMod/rooms/a.room", true, true);
                var testroom = RoomFactory.BuildFromResource("BotsMod/rooms/challengeShrineCodeIsShit.room", true, true);
                RoomFactory.BuildFromResource("BotsMod/rooms/challengeShrineOffsetTest.room", true, true);

                /*RoomFactory.RoomData roomData = new RoomFactory.RoomData
                {
                    room = testroom,
                    isSpecialRoom = false,
                    category = "NORMAL",
                    specialSubCategory = "COMBAT"
                };

                RoomFactory.rooms.Add("metestroomsyessss", roomData);
                DungeonHandler.Register(roomData);*/


                //InitGameObjects.Init();
                ChestInitStuff.Init();

                //GlobalWarning.Init();

                BeyondMasteryToken.Init();
                BeyondSniper.Add();
                BeyondChamberGun.Add();
               
                //ScrapBook.Init();



                Bob.Init();
                //CosmicSludge.Init();
                //BlessedOrb.Init();
                //BabyGoodMistake.Init();
                //LostsPast.Init();
                PirmalShotgrub.Init();
                BeyondKin.Init();
                OverseerFloor.Init();
                //OverseerDecoy.Init();
                TestActive.Init();
                //SpecialDungeon.Init();
                //SpecialDungeon2CozFuckYou.Init();
                //RichPresenceItem.Register();

                BeyondScout.Init();
                DeadEye.Init();
                Orb.Init();

                TestGun.Add();

                //AAAAAAAAAA.a.Init();

                Coin.Init();
                HellsRevolver2.Add();
                HellsRevolver.Add();
                HellsShotgun.Add();
                NailGun.Add();

                ChargeLance.Add();

                Kr82m.Add();

                Alternator.Add();

                CoolAssChargeGun.Add();

                BeyondUnlock.Add();
                BeyondUnlock.Add2();
                BeyondUnlock.Add3();

                Tools.shared_auto_002.LoadAsset<GameObject>("NPC_GunberMuncher").GetComponent<GunberMuncherController>().DefinedRecipes.Add(new GunberMuncherRecipe
                {
                    Annotation = "beyond unlock stuff",
                    gunIDs_A = new List<int> { BotsItemIds.Relic1 },
                    gunIDs_B = new List<int> { BotsItemIds.Relic2 },
                    resultID = BotsItemIds.Relic3,
                });


                //CompletlyRandomGun.Add();

                //PossetionItem.Init();

                LostFriend.Init();

                //PortalThing.Init();
                //NoTimeToExplain.Add();

                TheMessanger.Add();

                LostsCloak.Init();

                //LootTables.Init();

                //RedKey.Init();

                LostSidearm.Add();
                AltLostSidearm.Add();

                //TrailBullets.Init();

                SpellInit.Init();
                Wand.Add();

                HellsGrasp.Init();


                EnchantedEnemies.Init();

                TestPassive.Init();

                LightningRounds.Init();
                ChainedBullets.Init();
                FracturedRounds.Init();

                Sin.Init();
                PhyGun.Add();
                Roomba.Init();

                ThrowableTest.Init();

                SpinDownDice.Init();
                DarkArts.Init();
                //OtherworldlyConnections.Init();
                OtherwordlyFury.Init();

                VoidGlassGuonStone.Init();
                VoidAmmolet.Init();

                //ETGModMainBehaviour.Instance.gameObject.AddComponent<InspectorFixer>();

                #region shop setup

                List<string> testNpcIdleSprites = new List<string>
                {
                    "stairway_npc_idle_001.png",
                   
                };

                List<string> testNpcTalkSprites = new List<string>
                {
                    "stairway_npc_idle_001.png",
                    
                };

                List<string> beyondNpcIdleSprites = new List<string>
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

                List<string> beyondNpcTalkSprites = new List<string>
                {
                    "shopkeep_talk_001.png",
                    "shopkeep_talk_002.png",
                    "shopkeep_talk_003.png",
                    "shopkeep_talk_004.png",
                    "shopkeep_talk_005.png",
                };

                var devilLootTable = LootTableAPI.LootTableTools.CreateLootTable();
                devilLootTable.AddItemToPool(349);
                devilLootTable.AddItemToPool(279);
                devilLootTable.AddItemToPool(500);
                devilLootTable.AddItemToPool(350);
                //int z = 0;
                //int x = 0;
                for (int i = 0; i < testNpcIdleSprites.Count; i++)
                {
                    testNpcIdleSprites[i] = "BotsMod/sprites/Npcs/Stairway/" + testNpcIdleSprites[i];                   
                }

                for (int i = 0; i < testNpcTalkSprites.Count; i++)
                {
                    testNpcTalkSprites[i] = "BotsMod/sprites/Npcs/Stairway/" + testNpcTalkSprites[i];
                }

                for (int i = 0; i < beyondNpcIdleSprites.Count; i++)
                {
                    beyondNpcIdleSprites[i] = "BotsMod/sprites/Npcs/Beyond/" + beyondNpcIdleSprites[i];
                }

                for (int i = 0; i < beyondNpcTalkSprites.Count; i++)
                {
                    beyondNpcTalkSprites[i] = "BotsMod/sprites/Npcs/Beyond/" + beyondNpcTalkSprites[i];
                }

                var beyondLootTable = LootTableAPI.LootTableTools.CreateLootTable();
                foreach (var item in Tools.BeyondItems)
                {
                    beyondLootTable.AddItemToPool(item);
                }

                

                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "{wb}You are not one of us free of the masters control...");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "Not often we get outsiders here");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "even more words");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_RUNBASEDMULTILINE_GENERIC", "to many words");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_RUNBASEDMULTILINE_STOPPER", "Enough talk");

                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Yes yes good good");
                ETGMod.Databases.Strings.Core.AddComplex("#BEYOND_SHOP_PURCHASED", "Enjoy this one");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_PURCHASE_FAILED", "To weak, come back when you're in better condition");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_INTRO", "Welcome...");

                ETGMod.Databases.Strings.Core.Set("#BEYOND_TAKEPLAYERDAMAGE", "The master's fury will not be kind to you!");



                //NpcInitShit.Init();
                //BotsModule.shop = NpcApi.ItsDaFuckinShopApi.SetUpShop("beyond", "bot", testNpcIdleSprites, 6, testNpcTalkSprites, 8, devilLootTable, BaseShopController.AdditionalShopType.TRUCK, "", "", "", "", "", "", false);
                //BreachShopTools.Init();

                shop2 = NpcApi.ItsDaFuckinShopApi.SetUpShop("ItemForGuns", "bot", testNpcIdleSprites, 6, testNpcTalkSprites, 8, Tools.shared_auto_001.LoadAsset<GenericLootTable>("All_Item_Loot_Table"), CustomShopItemController.ShopCurrencyType.CUSTOM, "a", "a", "a", "a", "a", "a", ItsDaFuckinShopApi.defaultTalkPointOffset,
                    new Vector3[] { new Vector3(1.125f, 2.125f, 1), new Vector3(0f, 3.250f, 1), new Vector3(5.250f, 3.250f, 1), new Vector3(4.125f, 2.125f, 1), new Vector3(2.625f, 1f, 1) }, 1, false, null, ShopMerchantStuff.CustomCanBuyWeapons, ShopMerchantStuff.RemoveCurrencyWeapons,
                    ShopMerchantStuff.CustomPriceWeapons, null, null, "BotsMod/sprites/shopguygunmoneyicon.png", "gunTextIcon", true, false).GetComponent<CustomShopController>();
                //mapiconshop
                shop = NpcApi.ItsDaFuckinShopApi.SetUpShop("BeyondShopKeep", "bot", beyondNpcIdleSprites, 6, beyondNpcTalkSprites, 8, beyondLootTable, CustomShopItemController.ShopCurrencyType.CUSTOM, "#BEYOND_RUNBASEDMULTILINE_GENERIC", "#BEYOND_RUNBASEDMULTILINE_STOPPER", "#BEYOND_SHOP_PURCHASED",
                    "#BEYOND_PURCHASE_FAILED", "#BEYOND_INTRO", "#BEYOND_TAKEPLAYERDAMAGE", ItsDaFuckinShopApi.defaultTalkPointOffset, ItsDaFuckinShopApi.defaultItemPositions, 1, false, null, ShopMerchantStuff.CustomCanBuyBeyond, ShopMerchantStuff.RemoveCurrencyBeyond,
                    ShopMerchantStuff.CustomPriceBeyond, null, null, "", "heart_big_idle_001", false, true, "BotsMod/sprites/Npcs/beyond_merch_carpet_001.png", true, "BotsMod/sprites/Npcs/mapiconshop.png", true, 0.01f).GetComponent<CustomShopController>();

                shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().Library.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
                shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().Library.clips[1].loopStart = 3;


                GameObject m_ParadoxPortal = UnityEngine.Object.Instantiate(BraveResources.Load<GameObject>("Global Prefabs/VFX_ParadoxPortal"), shop2.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().transform);
                m_ParadoxPortal.transform.localPosition = new Vector3(1.25f, 0, 0);
                if (m_ParadoxPortal)
                {
                    if (m_ParadoxPortal?.GetComponent<MeshRenderer>()?.material)
                    {
                        // m_EXGlitchPortalRenderer.materials = new Material[] { new Material(m_ParadoxPortal.GetComponent<MeshRenderer>().materials[0]) };
                        m_ParadoxPortal.GetComponent<MeshRenderer>().material = new Material(m_ParadoxPortal.GetComponent<MeshRenderer>().material);
                        m_ParadoxPortal.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.magenta);
                        m_ParadoxPortal.GetComponent<MeshRenderer>().material.SetTexture("_PortalTex", GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/beyond_portal_texture.png"));
                    }
                }
                ForceToSortingLayer m_EXGlitchPortalSortingLayer = m_ParadoxPortal.AddComponent<ForceToSortingLayer>();
                m_EXGlitchPortalSortingLayer.sortingLayer = DepthLookupManager.GungeonSortingLayer.PLAYFIELD;
                m_EXGlitchPortalSortingLayer.targetSortingOrder = -1;
                //ItsDaFuckinShopApi.SetUpJailedNpc("BeyondShopKeep", "bot", beyondNpcIdleSprites, 6, ItsDaFuckinShopApi.defaultTalkPointOffset, GungeonFlags.ACHIEVEMENT_ACCESS_ABBEY);

                foreach (var frame in shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().Library.clips[1].frames)
                {
                    var sprite = frame.spriteCollection.spriteDefinitions[frame.spriteId];
                    sprite.position0 -= new Vector3(0.25f, 0, 0);
                    sprite.position1 -= new Vector3(0.25f, 0, 0);
                    sprite.position2 -= new Vector3(0.25f, 0, 0);
                    sprite.position3 -= new Vector3(0.25f, 0, 0);
                }

                Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                mat.mainTexture = shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().renderer.material.mainTexture;
                mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
                mat.SetFloat("_EmissiveColorPower", 1.55f);
                mat.SetFloat("_EmissivePower", 50);
                shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().sprite.renderer.material = mat;

                #endregion

                //Examples.Init();
                //GameManager.Instance.PrimaryPlayer.star
                /*
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
                */

                ModRoomPrefabs.InitCustomRooms();
                BeyondDungeonFlows.InitDungeonFlows();
                BeyondDungeon.InitCustomDungeon();
                LostPastDungeon.InitCustomDungeon();
                Hook hook = new Hook(
                   typeof(GameManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                   typeof(BotsModule).GetMethod("GameManager_Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                   typeof(GameManager)
               );

                StairWay.Init();

                metadata = this.Metadata;



                //GUI.Init();

                //CatogreyText = GUI.CreateText(null, new Vector2(15f, 265), "", TextAnchor.MiddleLeft, font_size: FontSize);

                //ETGModMainBehaviour.Instance.gameObject.AddComponent<CatogreyTeller>();

                InitSynergies.Init();

                var name = Loader.BuildCharacter("BotsMod/Characters/Lost", true, new Vector3(15.3f, 24.8f, 25.3f), true, true, true, new Color32(255, 69, 248, 255), 4.55f, 55, 2, true, "botfs_lost").nameInternal;
                

                //Loader.BuildCharacter("Shade", false, Vector2.zero, true, true, false, new Color32(0, 0, 0, 0), 0, 0, 0, false, "");

                Loader.SetupCustomAnimation(name, "dance", 12, tk2dSpriteAnimationClip.WrapMode.Loop);

                Loader.SetupCustomBreachAnimation(name, "float", 12, tk2dSpriteAnimationClip.WrapMode.Once);
                Loader.SetupCustomBreachAnimation(name, "float_hold", 5, tk2dSpriteAnimationClip.WrapMode.Loop);
                Loader.SetupCustomBreachAnimation(name, "float_out", 14, tk2dSpriteAnimationClip.WrapMode.Once);

                //Loader.AddFoyerObject(name, SpriteBuilder.SpriteFromResource("BotsMod/sprites/marker.png"), new Vector2(0.25f, 0));

                Loader.AddPhase(name, new CharacterSelectIdlePhase
                {
                    endVFXSpriteAnimator = null,
                    vfxSpriteAnimator = null,
                    holdAnimation = "float_hold",
                    inAnimation = "float",
                    outAnimation = "float_out",
                    optionalHoldIdleAnimation = "",
                    holdMax = 10,
                    holdMin = 5,
                    optionalHoldChance = 0,
                    vfxHoldPeriod = 0,
                    vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE
                });

                //Loader.BuildCharacter("The Blind", false, true, true, false, new Color32(0, 0, 0, 0), 0, 0, 0, false, "");

                //ShrineFactory.RegisterShrineRoom(NPC, protoroom, "bot:test_npc_shrine", new Vector2(1, 1));

                //CollectionDumper.DumpCollection();


                ChamberGunAPI.Init("EnterTheBeyond");

                //Ammonomicon.Init();

                /*
                ChamberGun = (PickupObjectDatabase.GetById(647) as Gun);
                if (ChamberGun.gameObject.GetComponent<ChamberGunProcessor>())
                {
                    UnityEngine.Object.Destroy(ChamberGun.gameObject.GetComponent<ChamberGunProcessor>());
                    ChamberGun.gameObject.AddComponent<BotChamberGunProcessor>();                    
                }
                ChamberGun.gameObject.AddComponent<BotsCustomChamberGun>();
                //MakeThemAllPetable.Init();
                //MakeThemAllPetable.Init();
                //RichPresence.init();

                //GameStatsManager.Instance.SetStat(TrackedStats.NUMBER_DEATHS, 27615);
                /*
                if (LostPastBoss.BulletBank && LostPastBoss.BulletBank.aiActor && LostPastBoss.BulletBank.aiActor.TargetRigidbody)
                {
                    LostPastBoss.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b").bulletBank.GetBullet("eye"));


                }*/


                AlphabetSoupSynergyProcessor alphabetSoupSynergyProcessor = PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>();



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


                var funnylist = PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>().Entries.ToList();
                // var funnylist = alphabetSoupSynergyProcessor.Entries.ToList();
                //funnylist.Add(iShouldntHaveBeenGivenThisPower6);
                funnylist.Add(TransRights);

                PickupObjectDatabase.GetById(340).gameObject.GetComponent<AlphabetSoupSynergyProcessor>().Entries = funnylist.ToArray();

                ModLogo = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/title_words_beyond_001.png");
                try
                {
                    MainMenuFoyerUpdateHook = new Hook(
                        typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
                        typeof(BotsModule).GetMethod("MainMenuUpdateHook", BindingFlags.NonPublic | BindingFlags.Instance),
                        typeof(MainMenuFoyerController));
                }
                catch (Exception ex)
                {

                    Log(ex.ToString());
                    return;
                }
                

                Commands.Init();


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
        
        int atlastesting = 0;


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




        public override void Exit() { }
        public override void Init() 
        {
            SaveAPIManager.Setup("bot");
        }

        
    }
}
