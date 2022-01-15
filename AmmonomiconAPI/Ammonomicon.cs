//using ChallengeAPI;
using BotsMod;
using CustomCharacters;
using ItemAPI;
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
                var atlas = Tools.LoadAssetFromAnywhere<GameObject>("Ammonomicon Atlas").GetComponent<dfAtlas>(); //AmmonomiconController.Instance.Ammonomicon.bookmarks[2].AppearClip.Atlas;
                //CollectionDumper.DumpdfAtlas(atlas);
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_001.png"), "bookmark_beyond_001");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_002.png"), "bookmark_beyond_002");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_003.png"), "bookmark_beyond_003");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_004.png"), "bookmark_beyond_004");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_hover_001.png"), "bookmark_beyond_hover_001");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_001.png"), "bookmark_beyond_select_001");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_002.png"), "bookmark_beyond_select_002");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_003.png"), "bookmark_beyond_select_003");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_hover_001.png"), "bookmark_beyond_select_hover_001");

                StringHandler.AddDFStringDefinition("#AMMONOMICON_BEYOND", "SPELLS");

                var page = FakePrefab.Clone(BraveResources.Load<GameObject>("Global Prefabs/Ammonomicon Pages/Guns Page Left", ".prefab"));
                if(page == null)
                {
                    Tools.Log("Clone is returning a null object", "#eb1313");
                }

                page.name = "Beyond Page Left";

                

                var renderer = page.GetComponentInChildren<AmmonomiconPageRenderer>();
                Tools.Log("h", "#eb1313");
                foreach (var child in page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").gameObject.GetComponents<Component>())
                {
                    Tools.Log(child.ToString());
                }

                page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
                Tools.Log("1", "#eb1313");
                page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
                Tools.Log("2", "#eb1313");
                page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 2").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
                Tools.Log("3", "#eb1313");
                page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 3").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
                Tools.Log("4", "#eb1313");
                page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 4").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";

                foreach(Transform child in page.transform.Find("Scroll Panel").Find("Scroll Panel").Find("Guns Panel").GetChild(0))
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }

                renderer.pageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT;

                Tools.Log("a", "#eb1313");


                customPages.Add("Global Prefabs/Ammonomicon Pages/" + page.name, page);
               
                //GameManager.Instance.StartCoroutine(DoStartStuff());

            }
            catch (Exception e)
            {
                Tools.Log("Ammonomicon broken :(", "#eb1313");
                Tools.Log(string.Format(e + ""), "#eb1313");
            }

            

        }

        public static string GetItemNamesFromIdList(string baseString, string prefix, List<int> list)
        {
            if (list?.Count > 0)
            {
                prefix += list.Count > 1 ? "s: " : ": ";
               
                baseString += prefix;
                for (int i = 0; i < list.Count; i++)
                {
                    baseString += PickupObjectDatabase.GetById(list[i]).EncounterNameOrDisplayName + (i == list.Count -1 ? "." : ", ");
                }
                baseString += "\n";
            }         
            return baseString;
        }

        public static void InitializeItemsPageLeft(AmmonomiconPageRenderer self)
        {

            //EncounterDatabase.Instance.Entries

            Transform transform = self.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
            dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();

            
            List<KeyValuePair<int, EncounterDatabaseEntry>> list = new List<KeyValuePair<int, EncounterDatabaseEntry>>();
            for (int i = 0; i < CharacterDatabase.Instance.Objects.Count; i++)
            {
                PlayerController playerController = CharacterDatabase.Instance.Objects[i];
                if (playerController != null)
                {
                    string description = "";
                    description = GetItemNamesFromIdList(description, "Starting Gun", playerController.startingGunIds) + GetItemNamesFromIdList(description, "Starting Active", playerController.startingActiveItemIds) + GetItemNamesFromIdList(description, "Starting Item", playerController.startingPassiveItemIds);
                    description += "\n";
                    description += (GameStatsManager.Instance.GetCharacterSpecificFlag(playerController.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST) ? "\nPast Killed" : "");
                    description += (GameStatsManager.Instance.GetCharacterSpecificFlag(playerController.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME) ? "\nAlt Starter Unlocked" : "");
                    Tools.Log("items found", "#eb1313");

                    //var spriteName = SpriteBuilder.ammonomiconCollection.spriteDefinitions[SpriteBuilder.AddToAmmonomicon(playerController.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection.FirstValidDefinition)].name;

                    var id = SpriteBuilder.AddSpriteToCollection($"BotsMod/sprites/Ammonomicon/Character/{playerController.characterIdentity}.png", AmmonomiconController.ForceInstance.EncounterIconCollection);
                    Tools.Log("sprite id setup done", "#eb1313");
                    var spriteName = AmmonomiconController.ForceInstance.EncounterIconCollection.spriteDefinitions[id].name;
                    Tools.Log("sprite setup done", "#eb1313");
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_DESC", description);
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_SHORT_DESC", "It work? holy shit!");
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_NAME", playerController.characterIdentity.ToString());
                    Tools.Log("strings done", "#eb1313");
                    var enemyDatabaseEntry = new EncounterDatabaseEntry
                    {
                        isPassiveItem = false,
                        isInfiniteAmmoGun = false,
                        isPlayerItem = false,
                        pickupObjectId = -1,
                        prerequisites = new DungeonPrerequisite[0],
                        doesntDamageSecretWalls = false,
                        doNotificationOnEncounter = false,
                        IgnoreDifferentiator = true,
                        myGuid = Guid.NewGuid().ToString(),
                        unityGuid = "List<KeyValuePair<int, EncounterDatabaseEntry>> list = new List<KeyValuePair<int, EncounterDatabaseEntry>>();",
                        path = "assets/resourcesbundle/playerconvict.prefab",
                        ProxyEncounterGuid = "",
                        shootStyleInt = -1,
                        usesPurpleNotifications = false,
                        journalData = new JournalEntry
                        {
                            AmmonomiconFullEntry = $"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_DESC",
                            AmmonomiconSprite = spriteName,
                            SuppressInAmmonomicon = false,
                            DisplayOnLoadingScreen = false,
                            enemyPortraitSprite = null,
                            IsEnemy = false,
                            NotificationPanelDescription = $"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_SHORT_DESC",
                            PrimaryDisplayName = $"#AMMONOMICON_PLAYER_{playerController.characterIdentity.ToString().ToUpper()}_NAME",
                            RequiresLightBackgroundInLoadingScreen = false,
                            SpecialIdentifier = JournalEntry.CustomJournalEntryType.NONE,
                            SuppressKnownState = false,
                        }
                    };

                    list.Add(new KeyValuePair<int, EncounterDatabaseEntry>(100, enemyDatabaseEntry));
                }
            }

            /*for (int i = 0; i < ETGMod.AllMods.Count; i++)
            {
                var mod = ETGMod.AllMods[i];
                if (mod != null)
                {
                    string description = "";
                    

                    //var spriteName = SpriteBuilder.ammonomiconCollection.spriteDefinitions[SpriteBuilder.AddToAmmonomicon(playerController.GetComponent<tk2dSpriteAnimator>().Library.clips[0].frames[0].spriteCollection.FirstValidDefinition)].name;

                    //var id = SpriteBuilder.AddSpriteToCollection($"BotsMod/sprites/Ammonomicon/Character/{playerController.characterIdentity}.png", AmmonomiconController.ForceInstance.EncounterIconCollection);
                    Tools.Log("sprite id setup done", "#eb1313");
                    //var spriteName = AmmonomiconController.ForceInstance.EncounterIconCollection.spriteDefinitions[id].name;
                    Tools.Log("sprite setup done", "#eb1313");
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_DESC", description);
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_SHORT_DESC", mod.Metadata.Version.ToString().ToUpper());
                    ETGMod.Databases.Strings.Items.Set($"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_NAME", mod.Metadata.Name.ToString());
                    Tools.Log("strings done", "#eb1313");
                    var enemyDatabaseEntry = new EncounterDatabaseEntry
                    {
                        isPassiveItem = false,
                        isInfiniteAmmoGun = false,
                        isPlayerItem = false,
                        pickupObjectId = -1,
                        prerequisites = new DungeonPrerequisite[0],
                        doesntDamageSecretWalls = false,
                        doNotificationOnEncounter = false,
                        IgnoreDifferentiator = true,
                        myGuid = Guid.NewGuid().ToString(),
                        unityGuid = "List<KeyValuePair<int, EncounterDatabaseEntry>> list = new List<KeyValuePair<int, EncounterDatabaseEntry>>();",
                        path = "assets/resourcesbundle/playerconvict.prefab",
                        ProxyEncounterGuid = "",
                        shootStyleInt = -1,
                        usesPurpleNotifications = false,
                        journalData = new JournalEntry
                        {
                            AmmonomiconFullEntry = $"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_DESC",
                            AmmonomiconSprite = "",
                            SuppressInAmmonomicon = false,
                            DisplayOnLoadingScreen = false,
                            enemyPortraitSprite = null,
                            IsEnemy = false,
                            NotificationPanelDescription = $"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_SHORT_DESC",
                            PrimaryDisplayName = $"#AMMONOMICON_MOD_{mod.Metadata.Name.ToString().ToUpper()}_NAME",
                            RequiresLightBackgroundInLoadingScreen = false,
                            SpecialIdentifier = JournalEntry.CustomJournalEntryType.NONE,
                            SuppressKnownState = false,
                        }
                    };
                    Tools.Log("enemyDatabaseEntry done", "#eb1313");
                    list.Add(new KeyValuePair<int, EncounterDatabaseEntry>(100, enemyDatabaseEntry));
                }
            }*/
            Tools.Log("ssssssss", "#eb1313");
            list = (from e in list
                    orderby e.Key
                    select e).ToList<KeyValuePair<int, EncounterDatabaseEntry>>();
            List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
            dfPanel component2 = component.transform.GetChild(0).GetComponent<dfPanel>();
            for (int j = 0; j < list.Count; j++)
            {
                KeyValuePair<int, EncounterDatabaseEntry> keyValuePair = list[j];
                if (keyValuePair.Value != null)
                {
                    list2.Add(keyValuePair.Value);
                }
            }
            Tools.Log("Assa", "#eb1313");
            self.StartCoroutine(typeof(AmmonomiconPageRenderer).GetMethod("ConstructRectanglePageLayout", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { component2, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null }) as IEnumerator);
            component2.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal);
            component.Height = component2.Height;
            component2.Height = component.Height;

            /*List<KeyValuePair<int, EncounterDatabaseEntry>> list = new List<KeyValuePair<int, EncounterDatabaseEntry>>();
            for (int i = 0; i < EncounterDatabase.Instance.Entries.Count; i++)
            {
                EncounterDatabaseEntry enemyDatabaseEntry = EncounterDatabase.Instance.Entries[i];
                if (enemyDatabaseEntry != null)
                {
                    if (!string.IsNullOrEmpty(enemyDatabaseEntry.myGuid))
                    {
                        //if (!EncounterDatabase.IsProxy(enemyDatabaseEntry.myGuid))
                        //{

                        //}
                        
                        int key = 1000000000;//(enemyDatabaseEntry.ForcedPositionInAmmonomicon >= 0) ? enemyDatabaseEntry.ForcedPositionInAmmonomicon : 1000000000;
                        list.Add(new KeyValuePair<int, EncounterDatabaseEntry>(key, enemyDatabaseEntry));
                    }
                }
            }
            list = (from e in list
                    orderby e.Key
                    select e).ToList<KeyValuePair<int, EncounterDatabaseEntry>>();
            List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
            dfPanel component2 = component.transform.GetChild(0).GetComponent<dfPanel>();
            for (int j = 0; j < list.Count; j++)
            {
                KeyValuePair<int, EncounterDatabaseEntry> keyValuePair = list[j];
                if (keyValuePair.Value != null)
                {
                    list2.Add(keyValuePair.Value);
                }
            }

            self.StartCoroutine(typeof(AmmonomiconPageRenderer).GetMethod("ConstructRectanglePageLayout", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { component2, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null }) as IEnumerator);
            component2.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal);
            component.Height = component2.Height;
            component2.Height = component.Height;*/
        }

        private static IEnumerator DoStartStuff()
        {
            yield return new WaitUntil(() => AmmonomiconController.HasInstance == true);
            
            

            //BuildBookmark("Mods", "BotsMod/sprites/wip", "BotsMod/sprites/wip");
        }

        public static List<AmmonomiconBookmarkController> customBookmarks = new List<AmmonomiconBookmarkController>();
        public static Dictionary<string, GameObject> customPages = new Dictionary<string, GameObject>();

        public static void BuildBookmark(string name, string selectSpritePath, string deselectSelectedSpritePath)
        {

            var baseBookmark = AmmonomiconController.Instance.Ammonomicon.bookmarks[2];

            //var selectSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(selectSpritePath + ".png"));
            //var deselectSelectedSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(deselectSelectedSpritePath + ".png"));

            var dumbObj = FakePrefab.Clone(baseBookmark.gameObject);


            AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();

            Tools.Log("9");
            dumbObj.transform.parent = baseBookmark.gameObject.transform.parent;
            dumbObj.transform.position = baseBookmark.gameObject.transform.position;
            dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);

            tabController2.gameObject.name = name;

            tabController2.SelectSpriteName = "bookmark_guns_select_001";//selectSprite.name;//1967693681992645534
            tabController2.DeselectSelectedSpriteName = "bookmark_guns_002";//deselectSelectedSprite.name;

            tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Equipment Page Left";
            tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Equipment Page Right";
            tabController2.RightPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT;
            tabController2.LeftPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT;

            tabController2.AppearClip = baseBookmark.AppearClip;
            tabController2.SelectClip = baseBookmark.SelectClip;
            Tools.Log("9.5");
            FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
            var thing = m_sprite.GetValue(baseBookmark) as dfButton;
            m_sprite.SetValue(tabController2, thing);

            FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            var thing2 = m_animator.GetValue(baseBookmark) as dfSpriteAnimation;
            m_animator.SetValue(tabController2, thing2);

            dumbObj.SetActive(true);
            customBookmarks.Add(tabController2);
        }

        public static GameUIRoot UIRootPrefab;

        


    }
}
