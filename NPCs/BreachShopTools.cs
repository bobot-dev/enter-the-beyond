using Dungeonator;
using GungeonAPI;
using HutongGames.PlayMaker;
using ItemAPI;
using Alexandria.Helpers.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NpcApi
{
    class BreachShopTools
    {


        public static void Init()
        {
            bool initialized = BreachShopTools.m_initialized;
            if (!initialized)
            {
                DungeonHooks.OnFoyerAwake += BreachShopTools.PlaceBreachShops;
                DungeonHooks.OnPreDungeonGeneration += delegate (LoopDungeonGenerator generator, Dungeon dungeon, DungeonFlow flow, int dungeonSeed)
                {
                    bool flag = flow.name != "Foyer Flow" && !GameManager.IsReturningToFoyerWithPlayer;
                    if (flag)
                    {
                        BreachShopTools.CleanupBreachShops();
                    }
                };
                BreachShopTools.m_initialized = true;
            }
        }

        private static bool m_initialized;


        private static void CleanupBreachShops()
        {
            foreach (BreachShopComp customShrineController in UnityEngine.Object.FindObjectsOfType<BreachShopComp>())
            {
                bool flag = !FakePrefab.IsFakePrefab(customShrineController);
                if (flag)
                {
                    UnityEngine.Object.Destroy(customShrineController.gameObject);
                }
                else
                {
                    customShrineController.gameObject.SetActive(false);
                }
            }
        }

        public static Dictionary<string, GameObject> registeredShops = new Dictionary<string, GameObject>();

        public static void PlaceBreachShops()
        {
            BreachShopTools.CleanupBreachShops();
            DebugUtility.Print<string>("Placing breach shops: ", "FFFFFF", true);
            foreach (GameObject gameObject in BreachShopTools.registeredShops.Values)
            {
                try
                {
                   
                    if (gameObject.GetComponent<BreachShopComp>() != null)
                    {
                        BotsMod.BotsModule.Log("    " + gameObject.name, "FFFFFF");
                        var shop = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                        var comp = gameObject.GetComponent<BreachShopComp>();
                        shop.SetActive(true);
                        //shop.sprite.PlaceAtPositionByAnchor(comp.offset, tk2dBaseSprite.Anchor.LowerCenter);

                        //Vector2 relativePositionFromAnchor = this.GetRelativePositionFromAnchor(anchor);
                        shop.transform.position = comp.offset;// - relativePositionFromAnchor.ToVector3ZUp(0f);


                        IPlayerInteractable component3 = shop.GetComponent<IPlayerInteractable>();

                        if (!RoomHandler.unassignedInteractableObjects.Contains(component3))
                        {
                            RoomHandler.unassignedInteractableObjects.Add(component3);
                        }
                    }
                }
                catch (Exception e)
                {
                    BotsMod.BotsModule.Log(e.ToString(), "FF0000", false);
                }
            }
        }
    }

    class BreachShopComp : MonoBehaviour
    {
        public Vector3 offset;
    }
}
