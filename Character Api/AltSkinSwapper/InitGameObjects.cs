using ItemAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{

    
    class InitGameObjects
    {

        public static string EnterTheBeyondConfigPath = Path.Combine(ETGMod.ResourcesDirectory, "EnterTheBeyondConfig.json");
        public static PlayMakerFSM CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayMakerFSM>(jsonString);
        }



        public static void Init()
        {




            GameObject costumeSwapper = new GameObject("LostCostumeSwapper");

            var stupidSprite = costumeSwapper.AddComponent<tk2dSprite>();



            costumeSwapper.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(costumeSwapper);
            UnityEngine.Object.DontDestroyOnLoad(costumeSwapper);
            Shader shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            //costumeSwapper.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(110, 80)).CollideWithOthers = false;//31
            //costumeSwapper.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.shader = shader;
            //costumeSwapper.GetComponent<tk2dSprite>().GetCurrentSpriteDef().materialInst.shader = shader;
            //costumeSwapper.GetComponent<tk2dSprite>().HeightOffGround = -0.7f;
            //costumeSwapper.GetComponent<tk2dSprite>().UpdateZDepth();




            LostCharacterCostumeSwapper costumeSwapper1 = costumeSwapper.gameObject.AddComponent<LostCharacterCostumeSwapper>();

            BotsModule.costumeSwapper = costumeSwapper1;





            GameObject shop = SpriteBuilder.SpriteFromResource("BotsMod/sprites/wip", new GameObject("StupidFuckingShopThatIHateWhy"));

            BaseShopController shopButUseful = shop.AddComponent<BaseShopController>();


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

            shopButUseful.placeableHeight = 5;
            shopButUseful.placeableWidth = 5;
            shopButUseful.difficulty = 0;
            shopButUseful.isPassable = true;
            shopButUseful.baseShopType = BaseShopController.AdditionalShopType.KEY;
            shopButUseful.FoyerMetaShopForcedTiers = false;
            shopButUseful.BeetleExhausted = false;
            shopButUseful.ExampleBlueprintPrefab = null;
            shopButUseful.shopItemsGroup2 = null;


            //BotsMod/JSONShit/PlayMakerFSM #11743.json
            var playMakerFSMTest = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/PlayMakerFSM #11743.json");
            
            //BotsModule.Log(text, BotsModule.TEXT_COLOR);

            PlayMakerFSM playMakerFSM = new PlayMakerFSM();

            JsonUtility.FromJsonOverwrite(playMakerFSMTest, playMakerFSM);

            shopButUseful.shopkeepFSM = playMakerFSM;

            //shopButUseful.shopItems = LootTables.testShopLable;

            shopButUseful.shopItems = Tools.LoadShopTable("Shop_Curse_Items_01");
            shopButUseful.spawnPositions = new Transform[]
            {
                itemPos1.transform,
                itemPos2.transform,
                itemPos3.transform,

            };
            //shopButUseful.spawnGroupTwoItem1Chance = 0.5f;
            //shopButUseful.spawnGroupTwoItem2Chance = 0.5f;
            //shopButUseful.spawnGroupTwoItem3Chance = 0.5f;

            shopButUseful.shopItemShadowPrefab = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cloak", new GameObject("StupidFuckingShadowCunt"));
            shopButUseful.shopItemShadowPrefab.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(shopButUseful.shopItemShadowPrefab);
            UnityEngine.Object.DontDestroyOnLoad(shopButUseful.shopItemShadowPrefab);


            shopButUseful.ShopCostModifier = 1000;

            itemPos1.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos1);
            UnityEngine.Object.DontDestroyOnLoad(itemPos1);

            itemPos2.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos2);
            UnityEngine.Object.DontDestroyOnLoad(itemPos2);

            itemPos3.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos3);
            UnityEngine.Object.DontDestroyOnLoad(itemPos3);


            shop.SetActive(false);
            ItemAPI.FakePrefab.MarkAsFakePrefab(shop);
            UnityEngine.Object.DontDestroyOnLoad(shop);

            BotsModule.Shop = shop;



            var itemJsonBullShit = JsonUtility.ToJson(shop.GetComponent<BaseShopController>(), true);
            

            try
            {

            



                //sprites n shit





            }
            catch (Exception e)
            {
                BotsModule.Log("Stupid Sprites Broke", "#eb1313");
                BotsModule.Log(string.Format(e + ""), "#eb1313");
            }
        }
    }
}
