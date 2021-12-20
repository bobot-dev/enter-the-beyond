using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class UiTesting : MonoBehaviour
    {

        public static dfLabel p_playerArmourLabel;
        public static dfSprite p_playerArmourSprite;
        public static GameObject pannel;

        public void Init()
        {


         

            try
            {
                
                /*if (GameUIRoot.Instance.gameObject == null)
                {
                    ETGModConsole.Log("aaa");
                }
                ETGModConsole.Log("1");
                foreach(Transform child in GameUIRoot.Instance.gameObject.transform)
                {
                    ETGModConsole.Log($"{child.name}");
                }
                
                var thing = FakePrefab.Clone(GameUIRoot.Instance.);
                thing.SetActive(true);
                thing.AddComponent<Debugger>();
                //UnityEngine.Object.Destroy(thing.transform.GetChild(1).gameObject);
                ETGModConsole.Log("2");
                if (thing == null)
                {
                    ETGModConsole.Log("aaa2");
                }

                thing.transform.parent = GameUIRoot.Instance.gameObject.transform;
                thing.transform.localPosition -= new UnityEngine.Vector3(0, 0.6518555f, 0);
                ETGModConsole.Log("3");*/


                pannel = PrefabAPI.PrefabBuilder.Clone(GameUIRoot.Instance.p_playerKeyLabel.transform.parent.gameObject);
                p_playerArmourLabel = pannel.transform.Find("KeyLabel").gameObject.GetComponent<dfLabel>();
                p_playerArmourLabel.name = "ArmourLabel";
                pannel.name = "ArmourPannel";
                pannel.transform.parent = GameUIRoot.Instance.transform;
                p_playerArmourLabel.transform.parent = pannel.transform;
                if (!p_playerArmourLabel.gameObject.activeSelf)
                {
                    p_playerArmourLabel.gameObject.SetActive(true);
                    ETGModConsole.Log("activated p_playerArmourLabel");

                }
                ETGModConsole.Log(GameUIRoot.Instance.p_playerKeyLabel.transform.parent.ToString());
                ETGModConsole.Log("was null... setup done");



                p_playerArmourSprite = p_playerArmourLabel.transform.parent.Find("KeySprite").gameObject.GetComponent<dfSprite>(); //Tools.shared_auto_001.LoadAsset<GameObject>("CoinSprite");//Tools.ReflectionHelpers.ReflectGetField<dfSprite>(typeof(GameUIRoot), "p_playerCoinSprite", self);
                p_playerArmourSprite.name = "ArmourSprite";
               
                p_playerArmourSprite.SpriteName = "heart_shield_full_001";

                if (!p_playerArmourSprite.gameObject.activeSelf)
                {
                    p_playerArmourSprite.gameObject.SetActive(true);
                    ETGModConsole.Log("activated p_playerArmourSprite");
                }
                GameUIRoot.Instance.AddControlToMotionGroups(p_playerArmourSprite, DungeonData.Direction.WEST, false);
                //p_playerArmourSprite.Parent.AddControl<dfPanel>();

            }
            catch(Exception e)
            {
                ETGModConsole.Log("Ui set up broke coz of cource it did heres youre likely useless error: " + e.ToString());
            }

        }
    }
}
