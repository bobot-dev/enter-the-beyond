using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Text;
using ETGGUI.Inspector;
using GungeonAPI;
using UnityEngine;
using BotsMod;
using System.Collections;

namespace AmmonomiconAPI
{
    class AmmonomiconHooks
    {
		public static void Init()
		{

			try
			{
				var startHook = new Hook(
					typeof(AmmonomiconInstanceManager).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public),
					typeof(AmmonomiconHooks).GetMethod("OpenHook", BindingFlags.Static | BindingFlags.Public));

				BotsModule.Log("AH: startHook Setup", "#03fc0b");

				var ammonomiconInitializeHook = new Hook(
					typeof(AmmonomiconPageRenderer).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public),
					typeof(AmmonomiconHooks).GetMethod("AmmonomiconInitializeHook", BindingFlags.Static | BindingFlags.Public));

				BotsModule.Log("AH: ammonomiconInitializeHook Setup", "#03fc0b");

				var toggleHeaderImageHook = new Hook(
					typeof(AmmonomiconPageRenderer).GetMethod("ToggleHeaderImage", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(AmmonomiconHooks).GetMethod("ToggleHeaderImageHook", BindingFlags.Static | BindingFlags.Public));

				BotsModule.Log("AH: toggleHeaderImageHook Setup", "#03fc0b");

				var updateOnBecameActiveHook = new Hook(
					typeof(AmmonomiconPageRenderer).GetMethod("UpdateOnBecameActive", BindingFlags.Instance | BindingFlags.Public),
					typeof(AmmonomiconHooks).GetMethod("UpdateOnBecameActiveHook", BindingFlags.Static | BindingFlags.Public));
				BotsModule.Log("AH: updateOnBecameActiveHook Setup", "#03fc0b");


				var checkLanguageFontsHook = new Hook(
					typeof(AmmonomiconPageRenderer).GetMethod("CheckLanguageFonts", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(AmmonomiconHooks).GetMethod("CheckLanguageFontsHook", BindingFlags.Static | BindingFlags.NonPublic));
				BotsModule.Log("AH: checkLanguageFontsHook Setup", "#03fc0b");

			}
			catch (Exception arg)
			{
				Tools.Log("oh no thats not good (AmmonomiconHooks broke): " + arg, "#eb1313");
				//LostItemsMod.Log(string.Format("D:", ), "#eb1313");
			}
		}

		public static void UpdateOnBecameActiveHook(Action<AmmonomiconPageRenderer> orig, AmmonomiconPageRenderer self)
		{
			self.ForceUpdateLanguageFonts();
			if (AmmonomiconController.Instance.ImpendingLeftPageRenderer == null || AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
			{
				switch (self.pageType)
				{
					case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetFirstVisibleTexts", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
					case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetFirstVisibleTexts", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
					case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetFirstVisibleTexts", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
					case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetFirstVisibleTexts", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
					case (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
				}
			}
		}

		private static void CheckLanguageFontsHook(Action<AmmonomiconPageRenderer, dfLabel> orig, AmmonomiconPageRenderer self, dfLabel mainText)
		{
			FieldInfo _englishFont = typeof(AmmonomiconPageRenderer).GetField("EnglishFont", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _otherLanguageFont = typeof(AmmonomiconPageRenderer).GetField("OtherLanguageFont", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _cachedLanguage = typeof(AmmonomiconPageRenderer).GetField("m_cachedLanguage", BindingFlags.NonPublic | BindingFlags.Instance);


			if (_englishFont.GetValue(self) == null)
			{
				_englishFont.SetValue(self, mainText.Font);
				_otherLanguageFont.SetValue(self, GameUIRoot.Instance.Manager.DefaultFont);
			}
			typeof(AmmonomiconPageRenderer).GetMethod("AdjustForChinese", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			if ((StringTableManager.GungeonSupportedLanguages)_cachedLanguage.GetValue(self) != GameManager.Options.CurrentLanguage)
			{
				_cachedLanguage.SetValue(self, GameManager.Options.CurrentLanguage);
				switch (self.pageType)
				{
					case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
						break;
					case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
						break;
					case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
						break;
					case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
						break;
					case (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT:
						typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
						break;
				}
			}
			if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				if (mainText.Font != (dfFontBase)_englishFont.GetValue(self))
				{
					mainText.Atlas = self.guiManager.DefaultAtlas;
					mainText.Font = (dfFontBase)_englishFont.GetValue(self);
				}
			}
			else if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.KOREAN && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.RUSSIAN)
			{
				if (mainText.Font != (dfFontBase)_otherLanguageFont.GetValue(self))
				{
					mainText.Atlas = GameUIRoot.Instance.Manager.DefaultAtlas;
					mainText.Font = (dfFontBase)_otherLanguageFont.GetValue(self);
				}
			}
		}

		public static void AmmonomiconInitializeHook(Action<AmmonomiconPageRenderer, MeshRenderer> orig, AmmonomiconPageRenderer self, MeshRenderer ts)
		{

			FieldInfo _camera = typeof(AmmonomiconPageRenderer).GetField("m_camera", BindingFlags.NonPublic | BindingFlags.Instance);

			FieldInfo _topBezierPropID = typeof(AmmonomiconPageRenderer).GetField("topBezierPropID", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _leftBezierPropID = typeof(AmmonomiconPageRenderer).GetField("leftBezierPropID", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _rightBezierPropID = typeof(AmmonomiconPageRenderer).GetField("rightBezierPropID", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _bottomBezierPropID = typeof(AmmonomiconPageRenderer).GetField("bottomBezierPropID", BindingFlags.NonPublic | BindingFlags.Instance);

			self.targetRenderer = ts;

			_camera.SetValue(self, self.GetComponent<Camera>());
			(_camera.GetValue(self) as Camera).aspect = 0.8888889f;

			self.guiManager = self.transform.parent.GetComponent<dfGUIManager>();
			self.guiManager.UIScale = 1f;
			Transform transform = self.guiManager.transform.Find("Scroll Panel");
			if (transform != null)
			{
				transform.GetComponent<dfScrollPanel>().LockScrollPanelToZero = true;
			}
			typeof(AmmonomiconPageRenderer).GetMethod("RebuildRenderData", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);

			_topBezierPropID.SetValue(self, Shader.PropertyToID("_TopBezier"));
			_leftBezierPropID.SetValue(self, Shader.PropertyToID("_LeftBezier"));
			_rightBezierPropID.SetValue(self, Shader.PropertyToID("_RightBezier"));
			_bottomBezierPropID.SetValue(self, Shader.PropertyToID("_BottomBezier"));

			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetRow(0, new Vector4(0f, 0f, 0f, 0f));
			matrix.SetRow(1, new Vector4(0f, 0f, 0f, 0f));
			matrix.SetRow(2, new Vector4(1f, 1f, 1f, 1f));
			matrix.SetRow(3, new Vector4(1f, 1f, 1f, 1f));
			self.SetMatrix(matrix);

			self.StartCoroutine(DelayedBuildPageHook(self));
		}

		private static IEnumerator DelayedBuildPageHook(AmmonomiconPageRenderer self)
		{
			if (self.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
			{
				while (GameManager.Instance.IsSelectingCharacter)
				{
					yield return null;
				}
			}
			switch (self.pageType)
			{
				case AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT:
					self.InitializeEquipmentPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT:
					self.InitializeEquipmentPageRight();
					break;
				case AmmonomiconPageRenderer.PageType.GUNS_LEFT:
					self.InitializeGunsPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:

					typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
					//self.SetPageDataUnknown(self);
					break;
				case AmmonomiconPageRenderer.PageType.ITEMS_LEFT:
					self.InitializeItemsPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
					typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
					break;
				case AmmonomiconPageRenderer.PageType.ENEMIES_LEFT:
					self.InitializeEnemiesPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
					typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
					break;
				case AmmonomiconPageRenderer.PageType.BOSSES_LEFT:
					self.InitializeBossesPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
					typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
					break;

				case (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT:
					self.InitializeItemsPageLeft();
					//InitializeModsPageLeft(self);
					break;

				case (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT:
					typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
					break;


				case AmmonomiconPageRenderer.PageType.DEATH_LEFT:
					typeof(AmmonomiconPageRenderer).GetMethod("InitializeDeathPageLeft", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					//self.InitializeDeathPageLeft();
					break;
				case AmmonomiconPageRenderer.PageType.DEATH_RIGHT:
					typeof(AmmonomiconPageRenderer).GetMethod("InitializeDeathPageLeft", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					//self.InitializeDeathPageRight();
					break;

			}
			yield break;
		}

		public static void ToggleHeaderImageHook(Action<AmmonomiconPageRenderer> orig, AmmonomiconPageRenderer self)
		{
			if (self.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT || self.pageType == AmmonomiconPageRenderer.PageType.GUNS_LEFT || self.pageType == AmmonomiconPageRenderer.PageType.ITEMS_LEFT || self.pageType == AmmonomiconPageRenderer.PageType.ENEMIES_LEFT || self.pageType == AmmonomiconPageRenderer.PageType.BOSSES_LEFT || self.pageType == (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT)
			{
				if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH && self.HeaderBGSprite != null)
				{
					self.HeaderBGSprite.IsVisible = false;
				}
				else if (self.HeaderBGSprite != null)
				{
					self.HeaderBGSprite.IsVisible = true;
				}
			}
		}


		public static void InitializeModsPageLeft(AmmonomiconPageRenderer stupidFuckingDumbCunt)
		{


			Transform transform = stupidFuckingDumbCunt.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
			dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
			List<KeyValuePair<string, Texture2D>> list = new List<KeyValuePair<string, Texture2D>>();

			for (int i = 0; i < ETGMod.GameMods.Count; i += 1)
			{
				ETGModuleMetadata metadata = ETGMod.GameMods[i].Metadata;

				list.Add(new KeyValuePair<string, Texture2D>(metadata.Name, metadata.Icon));
			}

			list = (from e in list orderby e.Key select e).ToList<KeyValuePair<string, Texture2D>>();
			List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
			dfPanel component3 = component.transform.GetChild(0).GetComponent<dfPanel>();

			//stupidFuckingDumbCunt.StartCoroutine(stupidFuckingDumbCunt.ConstructRectanglePageLayout(component3, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null));
			component3.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal);
			component.Height = component3.Height;
			component3.Height = component.Height;
		}

		public static void OpenHook(Action<AmmonomiconInstanceManager> orig, AmmonomiconInstanceManager self)
		{
			try
			{
				List<AmmonomiconBookmarkController> ammonomiconBookmarks = new List<AmmonomiconBookmarkController>();

				//var tabController = Ammonomicon.BuildBookmark("bookmark_bosses_hover_001", "bookmark_bosses_select_hover_001", AmmonomiconPageRenderer.PageType.BOSSES_RIGHT, AmmonomiconPageRenderer.PageType.BOSSES_LEFT);

				foreach (var bookmark in self.bookmarks)
				{
					Tools.Log(bookmark.gameObject.name);
					//if (bookmark.gameObject.name == "Death")
					//{
					//	deathBookmark = bookmark;
					//}
					//else
					//{
					ammonomiconBookmarks.Add(bookmark);
					//}


				}
				Tools.Log("8");
				var dumbObj = UnityEngine.Object.Instantiate(ammonomiconBookmarks[2].gameObject);


				AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();

				Tools.Log("9");
				dumbObj.transform.parent = ammonomiconBookmarks[2].gameObject.transform.parent;
				dumbObj.transform.position = ammonomiconBookmarks[2].gameObject.transform.position;
				dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);

				tabController2.gameObject.name = "Mods";
				//tabController2.DeselectSelectedSpriteName = "bookmark_bosses_select_hover_001";
				//tabController2.SelectSpriteName = "bookmark_enemies_hover_001";
				tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Test Page Left";
				tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Test Page Right";
				tabController2.RightPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_RIGHT;
				//tabController2.RightPageType = AmmonomiconPageRenderer.PageType.ITEMS_RIGHT;
				tabController2.LeftPageType = (AmmonomiconPageRenderer.PageType)CustomEnums.CustomPageType.MODS_LEFT;
				//tabController2.LeftPageType = AmmonomiconPageRenderer.PageType.ITEMS_LEFT;
				//tabController2.AppearClip = ammonomiconBookmarks[2].AppearClip;
				//tabController2.SelectClip = ammonomiconBookmarks[2].SelectClip;
				Tools.Log("9.5");
				FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
				//var thing = m_sprite.GetValue(ammonomiconBookmarks[2]);
				m_sprite.SetValue(tabController2, m_sprite.GetValue(ammonomiconBookmarks[2]));

				FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
				//var thing2 = m_animator.GetValue(ammonomiconBookmarks[2]);
				m_animator.SetValue(tabController2, m_animator.GetValue(ammonomiconBookmarks[2]));

				FieldInfo m_isCurrentPage = typeof(AmmonomiconBookmarkController).GetField("m_isCurrentPage", BindingFlags.NonPublic | BindingFlags.Instance);
				//var thing3 = m_isCurrentPage.GetValue(ammonomiconBookmarks[2]);
				m_isCurrentPage.SetValue(tabController2, m_isCurrentPage.GetValue(ammonomiconBookmarks[2]));

				Tools.Log("10");

				if (dumbObj.GetComponent<dfButton>() == null)
				{
					Tools.Log("dfButton nulled :(");
				}

				ammonomiconBookmarks.Insert(3, tabController2);

				//ammonomiconBookmarks.Add(deathBookmark);

				Tools.Log("11");
				//self.bookmarks = ammonomiconBookmarks.ToArray();
				self.bookmarks[2] = tabController2;
				Tools.Log("12");
				/*Tools.Log("=========Start========");

				foreach (var bookmark in self.bookmarks)
				{

					var texture = bookmark.AppearClip.Atlas.Texture;
					foreach (var item in bookmark.AppearClip.Atlas.Items)
					{
						int x = (int)item.region.x;
						int y = (int)item.region.y;
						int width = (int)item.region.width;
						int height = (int)item.region.height;

						Color[] pix = texture.GetPixels(x, y, width, height);
						Texture2D destTex = new Texture2D(width, height);
						destTex.SetPixels(pix);
						destTex.Apply();
						ToolsGAPI.ExportTexture(destTex, "BotsDUMPsprites", item.name);

					}
					

					if (false != false)//bookmark.gameObject != null)
					{
						Tools.Log(bookmark.gameObject.name);
						foreach (var comp in bookmark.gameObject.GetComponents<Component>())
						{
							if (comp != null)
							{
								Tools.Log($"======={comp} Start=======");
								PropertyInfo[] properties = comp.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
								var _Crawled = new List<string>();
								foreach (PropertyInfo propertyInfo in properties)
								{
									string text = propertyInfo.DeclaringType.FullName + "::" + propertyInfo.Name;
									if (!GenericComponentInspector.PropertyBlacklist.Contains(text) && !_Crawled.Contains(text) && propertyInfo.MemberType != MemberTypes.Method && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)// && propertyInfo.CanWrite)
									{
										_Crawled.Add(propertyInfo.Name);
										try
										{
											object value = ReflectionHelper.GetValue(propertyInfo, comp);

											Tools.Log("	" + propertyInfo.Name + ": " + value.ToStringIfNoString());
										}
										catch (Exception message)
										{
											Tools.Log("GenericComponentInspector: Blacklisting " + text);//, Tools.LOCKED_CHARACTOR_COLOR);
											Tools.Log(message.ToString());//, Tools.LOCKED_CHARACTOR_COLOR);
											GenericComponentInspector.PropertyBlacklist.Add(text);
										}
									}
								}

								Tools.Log($"========{comp} End========");
							}



						}
						Tools.Log($"======End for {bookmark.gameObject.name}=====");
					}

				}
				Tools.Log("=========End========");*/

				dumbObj.SetActive(false);
				//ItemAPI.FakePrefab.MarkAsFakePrefab(dumbObj);

				Tools.Log("13");

				orig(self);
			}
			catch (Exception e)
			{
				Tools.Log("Ammonomicon broken :(", "#eb1313");
				Tools.Log(string.Format(e + ""), "#eb1313");
			}
		}

	}
}
