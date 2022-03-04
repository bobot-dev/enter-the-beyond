using FrostAndGunfireItems;
using GungeonAPI;
using ItemAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
using BotsMod.NightmareNightmareNightmare;
using ModdedItemWeightBalancer;
//using ChallengeAPI;


namespace BotsMod
{
    public class BotsModule : ETGModule
    {
        public static readonly string MOD_NAME = "Enter The Beyond";
        public static readonly string VERSION = "0.5.5";
        public static readonly string TEXT_COLOR = "#e01279";
        public static readonly string LOST_COLOR = "#7732a8";
        public static readonly string LOCKED_CHARACTOR_COLOR = "#ba0c00";

        public static AdvancedStringDB Strings;
        public static OverseerShield overseerShield;
        public static tk2dSpriteAnimation LostAltCostume;
        public static bool debugMode = false;
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
                     BotsModule.Log("Debug flow " + str, color, false);
                 });
            }
            catch (Exception e)
            {
                BotsModule.Log("Flow Command Broke", "#eb1313", false);
                BotsModule.Log(string.Format(e + ""), "#eb1313", false);
            }
            try
            {

                //GlobalDungeonData.GUNGEON_EXPERIMENTAL = true;

                ZipFilePath = this.Metadata.Archive;
                FilePath = this.Metadata.Directory;

                AudioResourceLoader.InitAudio();

                Strings = new AdvancedStringDB();
                //item api
                ItemBuilder.Init();
                ItemAPI.FakePrefabHooks.Init();

                //ChallengeInit.Init();

                SettingsHooks.Init();
                BeyondSettings.Load();
                debugMode = BeyondSettings.Instance.debug;
                debugMode = true;
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

                CharApi.Init("Bot");

                //funny book api

                AmmonomiconAPI.Tools.Init();
                Ammonomicon.Init();


                Tools.Init();
                CustomClipAmmoTypeToolbox.Init();

                BeyondPrefabs.InitCustomPrefabs();
                VFXToolbox.BuildVFXList();


                //ETGModMainBehaviour.Instance.gameObject.AddComponent<UiTesting>().Init();

                //my stuff

                Hooks.Init();
                Rooms.Init();
                //BeyondPrefabs.Init();
                //RoomPrefabs.Init();
                CustomFire.Init();
                BeyondNotificaionHandler.Init();

                //Dungeon CatacombsPrefab = FloorHooks.GetOrLoadByName_Orig("Base_Catacombs");
                //foreach(var roomMat in CatacombsPrefab.roomMaterialDefinitions)
                //{
                //    if (roomMat.supportsIceSquares == true)
                //    {
                //        roomMat.overrideFloorType = CellVisualData.CellFloorType.Ice;
                //        roomMat.overrideStoneFloorType = true;
                //    }
                //}
                if (debugMode)
                {
                    NoMoreFun.Init();
                }

                //SoulHeartController.Init();

                FloorHooks.Init();

                //ModdedItemWeightController.Init();

                //StuffIStoleFromApacheForChallengeMode.Init();
                if (debugMode)
                {
                    var testroom = RoomFactory.BuildFromResource("BotsMod/rooms/a.room", true, true);
                    testroom.usesCustomAmbientLight = true;
                    testroom.customAmbientLight = new Color32(75, 5, 122, 255);
                    ChestInitStuff.Init();
                }
                //GlobalWarning.Init();

                BeyondMasteryToken.Init();
                if (debugMode) BeyondSniper.Add();
                BeyondChamberGun.Add();

                //ScrapBook.Init();



                if (debugMode) Bob.Init();
                //CosmicSludge.Init();
                //BlessedOrb.Init();
                //BabyGoodMistake.Init();
                //LostsPast.Init();
                if (debugMode) PirmalShotgrub.Init();
                if (debugMode) BeyondKin.Init();
                OverseerFloor.Init();
                //OverseerDecoy.Init();
                if (debugMode) TestActive.Init();
                //SpecialDungeon.Init();
                //SpecialDungeon2CozFuckYou.Init();
                //RichPresenceItem.Register();

                if (debugMode) LeshysCamera.Init();

                CarpetBurn.Init();
                BeyondBattery.Init();

                if (debugMode) BeyondScout.Init();
                if (debugMode) DeadEye.Init();


                if (debugMode) TestGun.Add();

                //AAAAAAAAAA.a.Init();

                Coin.Init();
                HellsRevolver2.Add();
                HellsRevolver.Add();
                if (debugMode) HellsShotgun.Add();
                NailGun.Add();

                if (debugMode) ChargeLance.Add();

                if (debugMode) Kr82m.Add();

                if (debugMode) Alternator.Add();

                CoolAssChargeGun.Add();
                if (debugMode) BeyondRailgun.Add();

                if (debugMode) BreachCutter.Add();

                BeyondSmg.Add();

                Judgment.Add();

                if (debugMode) GunParasite.Add();

                BeyondUnlock.Add();
                BeyondUnlock.Add2();
                BeyondUnlock.Add3();
                if (debugMode)
                {
                    Tools.shared_auto_002.LoadAsset<GameObject>("NPC_GunberMuncher").GetComponent<GunberMuncherController>().DefinedRecipes.Add(new GunberMuncherRecipe
                    {
                        Annotation = "beyond unlock stuff",
                        gunIDs_A = new List<int> { BotsItemIds.Relic1 },
                        gunIDs_B = new List<int> { BotsItemIds.Relic2 },
                        resultID = BotsItemIds.Relic3,
                    });
                    Orb.Init();
                    TheMessanger.Add();
                    HellsGrasp.Init();
                }
               

                //CompletlyRandomGun.Add();

                //PossetionItem.Init();

                LostFriend.Init();

                //PortalThing.Init();
                //NoTimeToExplain.Add();

                

                LostsCloak.Init();

                //LootTables.Init();

                //RedKey.Init();

                LostSidearm.Add();
                AltLostSidearm.Add();

                //TrailBullets.Init();

                StaticSpellReferences.Init();
                Wand.Add();

                


                EnchantedEnemies.Init();

                CustomAmmo.Init();


                if (debugMode) TestPassive.Init();

                LightningRounds.Init();
                if (debugMode) ChainedBullets.Init();
                FracturedRounds.Init();

                Sin.Init();
                if (debugMode) PhyGun.Add();
                if (debugMode) Roomba.Init();

                if (debugMode) ThrowableTest.Init();

                SpinDownDice.Init();
                if (debugMode) DarkArts.Init();
                //OtherworldlyConnections.Init();
                OtherwordlyFury.Init();

                if (debugMode) VoidGlassGuonStone.Init();
                VoidAmmolet.Init();

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
                shop = NpcApi.ItsDaFuckinShopApi.SetUpShop("BeyondShopKeep", "bot", beyondNpcIdleSprites, 6, beyondNpcTalkSprites, 8, BeyondPrefabs.beyondLootTable, CustomShopItemController.ShopCurrencyType.CUSTOM, "#BEYOND_RUNBASEDMULTILINE_GENERIC", "#BEYOND_RUNBASEDMULTILINE_STOPPER", "#BEYOND_SHOP_PURCHASED",
                    "#BEYOND_PURCHASE_FAILED", "#BEYOND_INTRO", "#BEYOND_TAKEPLAYERDAMAGE", ItsDaFuckinShopApi.defaultTalkPointOffset, ItsDaFuckinShopApi.defaultItemPositions, 1, false, null, ShopMerchantStuff.CustomCanBuyBeyond, ShopMerchantStuff.RemoveCurrencyBeyond,
                    ShopMerchantStuff.CustomPriceBeyond, null, null, "", "heart_big_idle_001", false, true, "BotsMod/sprites/Npcs/beyond_merch_carpet_001.png", true, "BotsMod/sprites/Npcs/mapiconshop.png", true, 0.01f).GetComponent<CustomShopController>();

                shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().Library.clips[1].wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
                shop.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().Library.clips[1].loopStart = 3;



                shop.GetAnySprite().usesOverrideMaterial = true;
                Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

                material.SetTexture("_MainTex", shop.GetAnySprite().renderer.material.mainTexture);
                material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
                material.SetFloat("_EmissiveColorPower", 1.55f);
                material.SetFloat("_EmissivePower", 55);
                shop.GetAnySprite().renderer.material = material;


                GameObject m_ParadoxPortal = UnityEngine.Object.Instantiate(BraveResources.Load<GameObject>("Global Prefabs/VFX_ParadoxPortal"), shop2.gameObject.GetComponentInChildren<tk2dSpriteAnimator>().transform);
                m_ParadoxPortal.transform.localPosition = new Vector3(1.25f, 0, 0);
                if (m_ParadoxPortal)
                {
                    if (m_ParadoxPortal?.GetComponent<MeshRenderer>()?.material)
                    {
                        // m_EXGlitchPortalRenderer.materials = new Material[] { new Material(m_ParadoxPortal.GetComponent<MeshRenderer>().materials[0]) };
                        m_ParadoxPortal.GetComponent<MeshRenderer>().material = new Material(m_ParadoxPortal.GetComponent<MeshRenderer>().material);
                        m_ParadoxPortal.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", UnityEngine.Color.magenta);
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
                #region Old Ui Shit
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
                #endregion

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

                //var name = Loader.BuildCharacter("BotsMod/Characters/Lost", CustomPlayableCharacters.Lost, new Vector3(15.8f, 26.6f, 27.1f), true, new Vector3(15.3f, 24.8f, 25.3f), true, false, false, true, true, new Color32(175, 19, 30, 255), 4.55f, 55, 2, true, "botfs_lost").nameInternal;
                var data = Loader.BuildCharacter("BotsMod/Characters/Lost", CustomPlayableCharacters.Lost, new Vector3(15.8f, 26.6f, 27.1f), true, new Vector3(15.3f, 24.8f, 25.3f), true, false, false, true, true, new GlowMatDoer (new Color32(255, 0, 38, 255), 4.55f, 55),
                    new GlowMatDoer(new Color32(255, 69, 248, 255), 1.55f, 55), 0, true, "botfs_lost");
                data.prerequisites = new DungeonPrerequisite[1]
                {
                    new CustomDungeonPrerequisite
                    {
                        requireCustomFlag = true,
                        customFlagToCheck = CustomDungeonFlags.BOT_LOST_UNLOCKED,  
                    }
                };

                if (!debugMode) data.prerequisites = new DungeonPrerequisite[0];
                var name = data.nameInternal;

                Loader.SetupCustomAnimation(name, "dance", 12, tk2dSpriteAnimationClip.WrapMode.Loop);

                Loader.SetupCustomBreachAnimation(name, "float", 12, tk2dSpriteAnimationClip.WrapMode.Once);
                Loader.SetupCustomBreachAnimation(name, "float_hold", 5, tk2dSpriteAnimationClip.WrapMode.Loop);
                Loader.SetupCustomBreachAnimation(name, "float_out", 14, tk2dSpriteAnimationClip.WrapMode.Once);
                Loader.SetupCustomBreachAnimation(name, "select_idle", 12, tk2dSpriteAnimationClip.WrapMode.LoopFidget, 1, 4);
               
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
                    vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE,
                });

                Loader.AddCoopBlankOverride(name, CoopBlankOverrides.LostOverrideBlank);
                Loader.GetAnimation(name, "pitfall_return").fps *= 2;

                var first = SpriteBuilder.SpriteFromResource("BotsMod/Characters/Lost/groundthings/randomstuffthatgoonground_001.png");


                first.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
                Material floorGlowMat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                floorGlowMat.mainTexture = first.GetComponent<tk2dSprite>().renderer.material.mainTexture;
                floorGlowMat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
                floorGlowMat.SetFloat("_EmissiveColorPower", 1.55f);
                floorGlowMat.SetFloat("_EmissivePower", 55);
                first.GetComponent<tk2dSprite>().renderer.material = floorGlowMat;
                FakePrefab.MarkAsFakePrefab(first);

                Loader.AddFoyerObject(name, first, new Vector3(-0.17f, 0.29f, 19), CustomDungeonFlags.BOT_LOST_UNLOCKED);

                ChamberGunAPI.Init("EnterTheBeyond");

                //Ammonomicon.Init();              
                if (debugMode)
                {
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

                }






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

                    Log(ex.ToString(), debug: false);
                    return;
                }
                

                Commands.Init();

                CollectionDumper.DumpdfAtlas(ResourceManager.LoadAssetBundle("enemies_base_001").LoadAsset<GameObject>("MetalGearRat").GetComponent<AIActor>().GetComponent<MetalGearRatDeathController>().PunchoutMinigamePrefab.GetComponent<PunchoutController>()
                    .Player.PlayerUiSprite.Atlas)
               ;


                ETGMod.StartGlobalCoroutine(this.DelayedStartCR());
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR, false);
            }
            catch (Exception arg)
            {
                Log("[Bots Mod]: Something in the start method broke heres why: " + arg, "#eb1313", false);
            }
        }
        public static Texture2D ModLogo;

        public static Hook MainMenuFoyerUpdateHook;
       
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
            if (self.TitleCard != null && BeyondSettings.Instance.titleScreenOverrideEnabled)
            {
                if (((dfTextureSprite)self.TitleCard).Texture.name != ModLogo.name)
                {
                    ((dfTextureSprite)self.TitleCard).Texture = ModLogo;
                    //LogoEnabled = true;
                }
            }

        }

        

        public static void Log(string text, string color="#FFFFFF", bool debug = true)
        {
            if ((debug && debugMode) || !debug)
            {
                ETGModConsole.Log($"<color={color}>{text}</color>");
            }
            
        }
        private static bool hasInitialized;

        

        

        public static AssetBundle LoadAssetBundleFromLiterallyAnywhere(string name)
        {
            AssetBundle assetBundle = null;
            if (File.Exists(ZipFilePath))
            {
                ZipFile ModZIP = ZipFile.Read(ZipFilePath);
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
            else if (File.Exists(FilePath + "/" + name))
            {
                try
                {
                    assetBundle = AssetBundle.LoadFromFile(FilePath + "/" + name);
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




        public override void Exit() 
        {
            //Client.Shutdown();
        }
        public override void Init() 
        {
            SaveAPIManager.Setup("bot");
        }

        
    }
}
