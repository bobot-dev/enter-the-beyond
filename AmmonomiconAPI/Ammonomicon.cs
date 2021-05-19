//using ChallengeAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AmmonomiconAPI
{
    class Ammonomicon
    {
        public static void Init()
        {

            try
            {
                UIRootPrefab = Tools.LoadAssetFromAnywhere<GameObject>("UI Root").GetComponent<GameUIRoot>();
                GameManager.Instance.StartCoroutine(DoStartStuff());
                
            }
            catch (Exception e)
            {
                Tools.Log("Ammonomicon broken :(", "#eb1313");
                Tools.Log(string.Format(e + ""), "#eb1313");
            }

            

        }

        private static IEnumerator DoStartStuff()
        {
            yield return new WaitUntil(() => AmmonomiconController.HasInstance == true);
            BuildBookmark("Mods", "BotsMod/sprites/wip", "BotsMod/sprites/wip");
        }

        public static void BuildBookmark(string name, string selectSpritePath, string deselectSelectedSpritePath)
        {

            var baseBookmark = AmmonomiconController.Instance.Ammonomicon.bookmarks[2];

            var selectSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(selectSpritePath + ".png"));
            var deselectSelectedSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(deselectSelectedSpritePath + ".png"));

            var dumbObj = UnityEngine.Object.Instantiate(baseBookmark.gameObject);


            AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();

            Tools.Log("9");
            dumbObj.transform.parent = baseBookmark.gameObject.transform.parent;
            dumbObj.transform.position = baseBookmark.gameObject.transform.position;
            dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);

            tabController2.gameObject.name = name;

            tabController2.SelectSpriteName = selectSprite.name;
            tabController2.DeselectSelectedSpriteName = deselectSelectedSprite.name;

            tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Test Page Left";
            tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Test Page Right";
            //tabController2.RightPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT;
            tabController2.RightPageType = AmmonomiconPageRenderer.PageType.ITEMS_RIGHT;
            //tabController2.LeftPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT;
            tabController2.LeftPageType = AmmonomiconPageRenderer.PageType.ITEMS_LEFT;
            //tabController2.AppearClip = baseBookmark.AppearClip;
            //tabController2.SelectClip = baseBookmark.SelectClip;
            Tools.Log("9.5");
            FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
            var thing = m_sprite.GetValue(baseBookmark);
            m_sprite.SetValue(tabController2, thing);

            FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            var thing2 = m_animator.GetValue(baseBookmark);
            m_animator.SetValue(tabController2, thing2);

            FieldInfo m_isCurrentPage = typeof(AmmonomiconBookmarkController).GetField("m_isCurrentPage", BindingFlags.NonPublic | BindingFlags.Instance);
            var thing3 = m_isCurrentPage.GetValue(baseBookmark);
            m_isCurrentPage.SetValue(tabController2, thing3);
        }

        public static GameUIRoot UIRootPrefab;

        


    }
}
