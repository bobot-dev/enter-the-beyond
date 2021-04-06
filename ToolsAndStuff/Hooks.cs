using CustomCharacters;
using Dungeonator;
using ETGGUI.Inspector;
using ItemAPI;
using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AIAnimator;

namespace BotsMod
{


	

	class Hooks
	{
		public static void Init()
		{

			

			try
			{
				getOrLoadByName_Hook = new Hook(
					typeof(DungeonDatabase).GetMethod("GetOrLoadByName", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetOrLoadByNameHook", BindingFlags.Static | BindingFlags.Public));

				//Hook LogoHook = new Hook(typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic), typeof(hooks).GetMethod("MainMenuUpdateHook", BindingFlags.Instance | BindingFlags.NonPublic), typeof(MainMenuFoyerController));

				//Hook Portal = new Hook(typeof(ParadoxPortalController).GetProperty("Interact", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("HookInteract"));

				//Hook DumbPastHook = new Hook(typeof(GameManager).GetMethod("LoadCustomLevel", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("LoadCustomLevel"));


				

				Hook hook3 = new Hook(typeof(PlayerController).GetProperty("LocalShaderName", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("LocalShaderNameGetHook"));


				//Hook setWinPicHook2 = new Hook(typeof(AmmonomiconDeathPageController).GetProperty("SetWinPic").GetGetMethod(), typeof(Hooks).GetMethod("SetWinPicHook"));

				//= new Hook(typeof(AmmonomiconDeathPageController).GetMethod("SetWinPic", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Hooks).GetMethod("SetWinPicHook"));


				setWinPicHook = new Hook(
					typeof(AmmonomiconDeathPageController).GetMethod("SetWinPic", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("SetWinPicHook", BindingFlags.Static | BindingFlags.NonPublic));




				//Hook HandleAnimationEventHook = new Hook(
				//typeof(PlayerController).GetMethod("HandleAnimationEvent", BindingFlags.Instance | BindingFlags.NonPublic),
				//typeof(Hooks).GetMethod("HandleAnimationEvent", BindingFlags.Static | BindingFlags.Public));



				//Hook fuckyouzatherz = new Hook(typeof(ETGModInspector).GetProperty("DrawProperty", BindingFlags.Static | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("DrawPropertyHook"));


				//Hook quickStartHook = new Hook(typeof(CharacterSelectController).GetProperty("orig_GetCharacterPathFromQuickStart", BindingFlags.Static | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHook"));
				//Hook determineAvailableOptionsHook = new Hook(typeof(CharacterSelectController).GetProperty("DetermineAvailableOptionsHook", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(), typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHook"));


				//Hook petHook = new Hook(typeof(CompanionController).GetProperty("DoPet", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("DoPetHook"));

				//Hook startHook = new Hook(typeof(CompanionController).GetProperty("Initialize", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("InitializeHook"));

			}
			catch (Exception arg)
			{
				BotsModule.Log("oh no thats not good (hooks broke): " + arg, "#eb1313");
				//LostItemsMod.Log(string.Format("D:", ), "#eb1313");
			}
		}
 

		/*public void Interact(Action<ArkController, PlayerController> orig, ArkController self,PlayerController interactor)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, false);
			SpriteOutlineManager.RemoveOutlineFromSprite(self.LidAnimator.sprite, false);
			if (!self.m_hasBeenInteracted)
			{
				self.m_hasBeenInteracted = true;
			}
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].RemoveBrokenInteractable(self);
			}
			BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Medium);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(interactor);
				float num = Vector2.Distance(otherPlayer.CenterPosition, interactor.CenterPosition);
				if (num > 8f || num < 0.75f)
				{
					Vector2 a = Vector2.right;
					if (interactor.CenterPosition.x < self.ChestAnimator.sprite.WorldCenter.x)
					{
						a = Vector2.left;
					}
					otherPlayer.WarpToPoint(otherPlayer.transform.position.XY() + a * 2f, true, false);
				}
			}
			self.StartCoroutine(self.Open(interactor));
		}*/



		public static Hook getOrLoadByName_Hook;
		public static Hook setWinPicHook;

		private static void SetWinPicHook(Action<AmmonomiconDeathPageController> orig, AmmonomiconDeathPageController self)
		{

			//BotsModule.Log("setWinPicHook: 1", BotsModule.LOCKED_CHARACTOR_COLOR);

			if (ShouldUseJunkPic() && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
				{
					case PlayableCharacters.Pilot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_pilot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Convict:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_convict_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Robot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_robot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Soldier:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_marine_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Guide:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_hunter_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Bullet:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_bullet_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Eevee:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_eevee_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Gunslinger:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_slinger_001", ".png") as Texture);
						goto IL_1B4;

					case (PlayableCharacters)CustomPlayableCharacters.Lost:
						self.photoSprite.Texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/win_pic_junkan_lost_001.png");
						goto IL_1B4;
				}
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
			IL_1B4:;
			}
			else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_BossRush_001", ".png") as Texture);
			}
			else
			{

				//BotsModule.Log("setWinPicHook: 2", BotsModule.LOCKED_CHARACTOR_COLOR);

				GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
				if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
						{
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
						}
						else
						{
							switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
							{
								case PlayableCharacters.Pilot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Pilot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Convict:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Convict_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Robot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Robot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Soldier:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Marine_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Guide:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Hunter_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Bullet:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Bullet_001", ".png") as Texture);
									goto IL_384;
								case (PlayableCharacters)CustomPlayableCharacters.Lost:
									self.photoSprite.Texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/Win_Pic_Lost_001.png");
									goto IL_384;
							}
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
						IL_384:;
						}
					}
					else if (GameManager.IsGunslingerPast)
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Slinger_001", ".png") as Texture);
					}
					else
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Lich_Kill_001", ".png") as Texture);
					}
				}
				else
				{
					self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
				}
			}
		}


		private static bool ShouldUseJunkPic()
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController)
				{
					for (int j = 0; j < playerController.passiveItems.Count; j++)
					{
						if (playerController.passiveItems[j] is CompanionItem)
						{
							CompanionItem companionItem = playerController.passiveItems[j] as CompanionItem;
							if (companionItem.ExtantCompanion && companionItem.ExtantCompanion.GetComponent<SackKnightController>() && companionItem.ExtantCompanion.GetComponent<SackKnightController>().CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static object DrawPropertyHook(Action<ETGModInspector, PropertyInfo, object> orig, ETGModInspector self, PropertyInfo inf, object input)
		{
			if (input == null)
			{
				return null;
			}
			IBasePropertyInspector basePropertyInspector;
			if (ETGModInspector.PropertyInspectorRegistry.TryGetValue(input.GetType(), out basePropertyInspector))
			{
				return basePropertyInspector.OnGUI(inf, input);
			}
			GUILayout.Label(inf.Name + ": " + input.ToStringIfNoString(), new GUILayoutOption[0]);
			//GUILayout.
			//GUILayout.Space(8f);
			//BotsModule.Log("fuck you zatherz", "#eb1313");
			return input;
		}

		public static Dungeon GetOrLoadByNameHook(Func<string, Dungeon> orig, string name)
		{
			Dungeon dungeon = null;
			if (dungeon)
			{
				DebugTime.RecordStartTime();
				DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
				return dungeon;
			}
			else
			{
				return orig(name);
			}
		}

		private static void DoQuickRestartHook(Action<AmmonomiconDeathPageController, dfControl, dfMouseEventArgs> orig, AmmonomiconDeathPageController self, dfControl control, dfMouseEventArgs mouseEvent)
		{
			if (AmmonomiconController.Instance.IsOpening || AmmonomiconController.Instance.IsClosing)
			{
				return;
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
			{
				return;
			}
			SaveManager.DeleteCurrentSlotMidGameSave(null);
			//GameManager.Instance.StartCoroutine(HandleQuickRestart(self));
		}


		private static FieldInfo m_doingSomething = typeof(BraveOptionsMenuItem).GetField("m_doingSomething", BindingFlags.NonPublic | BindingFlags.Instance);
		private static FieldInfo m_temporaryPhotoTex = typeof(BraveOptionsMenuItem).GetField("m_temporaryPhotoTex", BindingFlags.NonPublic | BindingFlags.Instance);

		static AmmonomiconDeathPageController death = new AmmonomiconDeathPageController();
		static private IEnumerator HandleQuickRestart(AmmonomiconDeathPageController self)
		{

			

			if (GameManager.Instance.IsLoadingLevel || bool.Parse(m_doingSomething.GetValue(self) + ""))
			{
				yield break;
			}
			m_doingSomething.SetValue(self, true);
			if (BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				self.quickRestartButton.Text = "[sprite \"space_bar_down_001\"" + self.quickRestartButton.ForceGetLocalizedValue("#DEATH_QUICKRESTART");
			}
			AkSoundEngine.PostEvent("Play_UI_menu_characterselect_01", self.gameObject);
			Pixelator.Instance.ClearCachedFrame();
			if (bool.Parse(m_temporaryPhotoTex.GetValue(self) + ""))
			{
				//RenderTexture.ReleaseTemporary(m_temporaryPhotoTex.GetValue(self).);
				m_temporaryPhotoTex.SetValue(self, null);
			}
			AmmonomiconController.Instance.CloseAmmonomicon(false);
			while (AmmonomiconController.Instance.IsOpen)
			{
				if (!AmmonomiconController.Instance.IsClosing)
				{
					AmmonomiconController.Instance.CloseAmmonomicon(false);
				}
				yield return null;
			}
			if (AmmonomiconController.Instance.CurrentLeftPageRenderer != null)
			{
				AmmonomiconController.Instance.CurrentLeftPageRenderer.Disable(false);
				AmmonomiconController.Instance.CurrentLeftPageRenderer.Dispose();
			}
			if (AmmonomiconController.Instance.CurrentRightPageRenderer != null)
			{
				AmmonomiconController.Instance.CurrentRightPageRenderer.Disable(false);
				AmmonomiconController.Instance.CurrentRightPageRenderer.Dispose();
			}
			yield return null;
			if (GameManager.LastUsedPlayerPrefab && GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
			{
				GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire("PlayerEevee");
			}
			QuickRestartOptions qrOptions = AmmonomiconDeathPageController.GetNumMetasToQuickRestart();
			if (qrOptions.NumMetas > 0)
			{
				GameUIRoot.Instance.CheckKeepModifiersQuickRestart(qrOptions.NumMetas);
				while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
				{
					yield return null;
				}
				if (!GameUIRoot.Instance.GetAreYouSureOption())
				{
					qrOptions = default(QuickRestartOptions);
					if (GameManager.LastUsedPlayerPrefab && (GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Eevee || GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger))
					{
						GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire(CharacterSelectController.GetCharacterPathFromQuickStart());
					}
				}
			}
			GameUIRoot.Instance.ToggleUICamera(false);
			Pixelator.Instance.DoFinalNonFadedLayer = false;
			Pixelator.Instance.FadeToBlack(0.4f, false, 0f);
			GameManager.Instance.DelayedQuickRestart(0.5f, qrOptions);
			yield break;
		}


		public static List<GameOptions.QuickstartCharacter> quickstartCharacters;// = new List<GameOptions.QuickstartCharacter>();
		private static FieldInfo m_quickStartCharacters = typeof(BraveOptionsMenuItem).GetField("m_quickStartCharacters", BindingFlags.NonPublic | BindingFlags.Instance);
		private static FieldInfo m_selectedIndex = typeof(BraveOptionsMenuItem).GetField("m_selectedIndex", BindingFlags.NonPublic | BindingFlags.Instance);
		private static void DetermineAvailableOptionsHook(Action<BraveOptionsMenuItem> orig, BraveOptionsMenuItem self)
		{
			orig(self);

			


			if (quickstartCharacters == null)
			{
				quickstartCharacters = new List<GameOptions.QuickstartCharacter>();
			}
			else
			{
				quickstartCharacters.Clear();
			}
			List<string> list3 = new List<string>(8);
			quickstartCharacters.Add(GameOptions.QuickstartCharacter.LAST_USED);
			list3.Add("#CHAR_LASTUSED");
			quickstartCharacters.Add(GameOptions.QuickstartCharacter.PILOT);
			list3.Add("#CHAR_ROGUE");
			quickstartCharacters.Add(GameOptions.QuickstartCharacter.CONVICT);
			list3.Add("#CHAR_CONVICT");
			quickstartCharacters.Add(GameOptions.QuickstartCharacter.SOLDIER);
			list3.Add("#CHAR_MARINE");
			quickstartCharacters.Add(GameOptions.QuickstartCharacter.GUIDE);
			list3.Add("#CHAR_GUIDE");
			if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
			{
				quickstartCharacters.Add(GameOptions.QuickstartCharacter.BULLET);
				list3.Add("#CHAR_BULLET");
			}
			if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
			{
				quickstartCharacters.Add(GameOptions.QuickstartCharacter.ROBOT);
				list3.Add("#CHAR_ROBOT");
			}

			if (GameStatsManager.HasInstance && SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_LOST_UNLOCKED))
			{
				quickstartCharacters.Add((GameOptions.QuickstartCharacter)CustomEnums.QuickstartCharacter.LOST);
				list3.Add("#CHAR_LOST");
				
			}

			self.labelOptions = list3.ToArray();
			m_selectedIndex.SetValue(self, GetQuickStartCharIndex(GameManager.Options.PreferredQuickstartCharacter));
			if (Int32.Parse(m_selectedIndex.GetValue(self) + "") < 0 || Int32.Parse(m_selectedIndex.GetValue(self) + "") >= self.labelOptions.Length)
			{
				m_selectedIndex.SetValue(self, 0);
			}
			typeof(BraveOptionsMenuItem).GetMethod("UpdateSelectedLabelText", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new BraveOptionsMenuItem(), null);
			//self.UpdateSelectedLabelText();

			m_quickStartCharacters.SetValue(self, quickstartCharacters);
		}


		static private int GetQuickStartCharIndex(GameOptions.QuickstartCharacter quickstartChar)
		{
			for (int i = 0; i < quickstartCharacters.Count; i++)
			{
				if (quickstartCharacters[i] == quickstartChar)
				{
					return i;
				}
			}
			return -1;
		}

		public static string GetCharacterPathFromQuickStartHook(Action<CharacterSelectController> orig, CharacterSelectController self)
		{
			GameOptions.QuickstartCharacter quickstartCharacter = GameManager.Options.PreferredQuickstartCharacter;
			if (quickstartCharacter == GameOptions.QuickstartCharacter.LAST_USED)
			{
				switch (GameManager.Options.LastPlayedCharacter)
				{
					case PlayableCharacters.Pilot:
						quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
						goto IL_7C;
					case PlayableCharacters.Convict:
						quickstartCharacter = GameOptions.QuickstartCharacter.CONVICT;
						goto IL_7C;
					case PlayableCharacters.Robot:
						quickstartCharacter = GameOptions.QuickstartCharacter.ROBOT;
						goto IL_7C;
					case PlayableCharacters.Soldier:
						quickstartCharacter = GameOptions.QuickstartCharacter.SOLDIER;
						goto IL_7C;
					case PlayableCharacters.Guide:
						quickstartCharacter = GameOptions.QuickstartCharacter.GUIDE;
						goto IL_7C;
					case PlayableCharacters.Bullet:
						quickstartCharacter = GameOptions.QuickstartCharacter.BULLET;
						goto IL_7C;

					case (PlayableCharacters)CustomPlayableCharacters.Lost:
						quickstartCharacter = (GameOptions.QuickstartCharacter)CustomEnums.QuickstartCharacter.LOST;
						goto IL_7C;
				}
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
			}
			IL_7C:
			if (quickstartCharacter == GameOptions.QuickstartCharacter.BULLET && !GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
			{
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
			}
			if (quickstartCharacter == GameOptions.QuickstartCharacter.ROBOT && !GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
			{
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
			}

			if (quickstartCharacter == (GameOptions.QuickstartCharacter)CustomEnums.QuickstartCharacter.LOST && !SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_LOST_UNLOCKED))
			{
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
			}

			switch (quickstartCharacter)
			{
				case GameOptions.QuickstartCharacter.PILOT:
					return "PlayerRogue";
				case GameOptions.QuickstartCharacter.CONVICT:
					return "PlayerConvict";
				case GameOptions.QuickstartCharacter.SOLDIER:
					return "PlayerMarine";
				case GameOptions.QuickstartCharacter.GUIDE:
					return "PlayerGuide";
				case GameOptions.QuickstartCharacter.BULLET:
					return "PlayerBullet";
				case GameOptions.QuickstartCharacter.ROBOT:
					return "PlayerRobot";
				default:
					return "PlayerRogue";
			}
		}


		/*
		private static void ProcessHeartSpriteModifications(GameUIHeartController self, PlayerController associatedPlayer)
		{
			bool flag = false;
			if (associatedPlayer)
			{

				if (associatedPlayer.HealthAndArmorSwapped)
				{
					self.m_currentFullHeartName = "heart_shield_full_001";
					self.m_currentHalfHeartName = "heart_shield_half_001";
					self.m_currentEmptyHeartName = "heart_shield_empty_001";
					self.m_currentArmorName = "armor_shield_heart_idle_001";
					flag = true;
				}
				else if (associatedPlayer.CurrentGun)
				{
					if (associatedPlayer.name == "PlayerLost(Clone)")
					{
						self.m_currentFullHeartName = "heart_full_yellow_001";
						self.m_currentHalfHeartName = "heart_half_yellow_001";
						flag = true;
					}
				}
				else if (associatedPlayer.HasPassiveItem(BotsItemIds.LostCloak))
				{
					if (associatedPlayer.name == "PlayerLost(Clone)")
					{
						self.m_currentFullHeartName = "heart_full_yellow_001";
						self.m_currentHalfHeartName = "heart_half_yellow_001";
						flag = true;
					}
				}
			}
			if (!flag)
			{
				self.m_currentFullHeartName = self.fullHeartSpriteName;
				self.m_currentHalfHeartName = self.halfHeartSpriteName;
				self.m_currentEmptyHeartName = self.emptyHeartSpriteName;
				self.m_currentArmorName = self.armorSpritePrefab.SpriteName;
			}
		}
		*/
		public static void HookInteract(Action<ParadoxPortalController, PlayerController> orig, ParadoxPortalController self, PlayerController interactor)
		{
			
			orig(self, interactor);
			BotsModule.Log("texture: " + self.CosmicTex + ". name: " + self.CosmicTex.name, BotsModule.TEXT_COLOR);
		}

		public static void InitializeHook(Action<CompanionController, PlayerController> orig, CompanionController self, PlayerController owner)
		{
			self.CanBePet = true;
			orig(self, owner);
		}


		static bool playPetAnimation = false;
		public static void DoPetHook(Action<CompanionController, PlayerController> orig,CompanionController self, PlayerController player)
		{
			BotsModule.Log("1", BotsModule.TEXT_COLOR);
			self.aiAnimator.LockFacingDirection = true;
			if (self.specRigidbody.UnitCenter.x > player.specRigidbody.UnitCenter.x)
			{
				self.aiAnimator.FacingDirection = 180f;
				self.m_petOffset = new Vector2(0.3125f, -0.625f);
			}
			else
			{
				self.aiAnimator.FacingDirection = 0f;
				self.m_petOffset = new Vector2(-0.8125f, -0.625f);
			}
			BotsModule.Log("2", BotsModule.TEXT_COLOR);
			foreach (NamedDirectionalAnimation animation in self.aiAnimator.OtherAnimations)
			{
				if (animation.name == "pet")
				{
					playPetAnimation = true;
				}
				
			}
			BotsModule.Log("3", BotsModule.TEXT_COLOR);
			if (playPetAnimation == true)
			{
				self.aiAnimator.PlayUntilCancelled("pet", false, null, -1f, false);
			}
			else
			{
				self.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
			}
			BotsModule.Log("4", BotsModule.TEXT_COLOR);

			self.m_pettingDoer = player;
			BotsModule.Log("5", BotsModule.TEXT_COLOR);
		}

		public static void LoadCustomLevel(Action<GameManager, string> orig, GameManager self, string custom)
		{
			if (custom == "fs_guide" && self.PrimaryPlayer.name == "PlayerLost(Clone)")
			{
				custom = "botfs_lost";
			}
			orig(self, custom);
		}

		public delegate void CoolerAction<in T1, in t2, in t3, in t4, in t5, in t6, in t7, in t8, in t9, in t10, in t11, in t12, in t13, in t14, in t15 >(T1 a, t2 b, t3 c, t4 d, t5 e, t6 f, t7 g, t8 h, t9 i, t10 j, t11 k, t12 l, t13 m, t14 n, t15 o);
		public delegate void CoolerAction2<in T1, in t2, in t3, in t4, in t5, in t6, in t7, in t8, in t9>(T1 a, t2 b, t3 c, t4 d, t5 e, t6 f, t7 g, t8 h, t9 i);

		public static void TriggerSilencerHook(CoolerAction<SilencerInstance, Vector2, float, float, GameObject, float, float, float, float, float, float, float, PlayerController, bool, bool> orig, SilencerInstance self, Vector2 centerPoint, float expandSpeed, float maxRadius, GameObject silencerVFX, float distIntensity, float distRadius, float pushForce, float pushRadius, float knockbackForce, float knockbackRadius, float additionalTimeAtMaxRadius, PlayerController user, bool breaksWalls = true, bool skipBreakables = false)
		{
			var enemyList = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

			foreach (AIActor aIActor in enemyList)
			{
				if (aIActor.EnemyGuid == PirmalShotgrub.guid)
				{
					if (silencerVFX != null)
					{
						GameObject obj = UnityEngine.Object.Instantiate<GameObject>(silencerVFX, centerPoint.ToVector3ZUp(centerPoint.y), Quaternion.identity);
						UnityEngine.Object.Destroy(obj, 1f);
					}
					return;
				}
			}

			orig(self, centerPoint, expandSpeed, maxRadius, silencerVFX, distIntensity, distRadius, pushForce, pushRadius,  knockbackForce,  knockbackRadius,  additionalTimeAtMaxRadius,  user, breaksWalls, skipBreakables);
		}


		public static void DestroyBulletsInRangeHook(CoolerAction2< Vector2, float, bool, bool, PlayerController, bool, float?, bool, Action<Projectile>> orig, Vector2 centerPoint, float radius, bool destroysEnemyBullets, bool destroysPlayerBullets, PlayerController user = null, bool reflectsBullets = false, float? previousRadius = null, bool useCallback = false, Action<Projectile> callback = null)
		{

			var enemyList = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

			foreach (AIActor aIActor in enemyList)
			{
				if (aIActor.EnemyGuid == PirmalShotgrub.guid)
				{
					destroysEnemyBullets = false;
					destroysPlayerBullets = true;
					return;
				}
			}

			float num = radius * radius;
			float num2 = (previousRadius == null) ? 0f : (previousRadius.Value * previousRadius.Value);
			List<Projectile> list = new List<Projectile>();
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile && projectile.sprite)
				{
					float sqrMagnitude = (projectile.sprite.WorldCenter - centerPoint).sqrMagnitude;
					if (sqrMagnitude <= num)
					{
						if (!projectile.ImmuneToBlanks)
						{
							if (previousRadius == null || !projectile.ImmuneToSustainedBlanks || sqrMagnitude >= num2)
							{
								if (projectile.Owner != null)
								{
									if (projectile.isFakeBullet || projectile.Owner is AIActor || (projectile.Shooter != null && projectile.Shooter.aiActor != null) || projectile.ForcePlayerBlankable)
									{
										if (destroysEnemyBullets)
										{
											list.Add(projectile);
										}
									}
									else if (projectile.Owner is PlayerController)
									{
										if (destroysPlayerBullets)
										{
											list.Add(projectile);
										}
									}
									else
									{
										Debug.LogError("Silencer is trying to process a bullet that is owned by something that is neither man nor beast!");
									}
								}
								else if (destroysEnemyBullets)
								{
									list.Add(projectile);
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!destroysPlayerBullets && reflectsBullets)
				{
					PassiveReflectItem.ReflectBullet(list[j], true, user, 10f, 1f, 1f, 0f);
				}
				else
				{
					if (list[j] && list[j].GetComponent<ChainLightningModifier>())
					{
						ChainLightningModifier component = list[j].GetComponent<ChainLightningModifier>();
						UnityEngine.Object.Destroy(component);
					}
					if (useCallback && callback != null)
					{
						callback(list[j]);
					}
					list[j].DieInAir(false, true, true, true);
				}
			}
			List<BasicTrapController> allTriggeredTraps = StaticReferenceManager.AllTriggeredTraps;
			for (int k = allTriggeredTraps.Count - 1; k >= 0; k--)
			{
				BasicTrapController basicTrapController = allTriggeredTraps[k];
				if (basicTrapController && basicTrapController.triggerOnBlank)
				{
					float sqrMagnitude2 = (basicTrapController.CenterPoint() - centerPoint).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						basicTrapController.Trigger();
					}
				}
			}

			//orig(centerPoint, radius, destroysEnemyBullets, destroysPlayerBullets, user, reflectsBullets, previousRadius, useCallback, callback);
		}


		public static string LocalShaderNameGetHook(Func<PlayerController, string> orig, PlayerController self)
		{
			bool flag = !GameOptions.SupportsStencil;
			string result;
			if (flag)
			{
				result = "Brave/PlayerShaderNoStencil";
			}
			else
			{
				bool flag2 = self.name == "PlayerLost(Clone)";
				if (flag2)
				{

					Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
					material.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
					material.SetColor("_EmissiveColor", new Color32(255, 0, 38, 255));
					material.SetFloat("_EmissiveColorPower", 4.55f);
					material.SetFloat("_EmissivePower", 55);
					self.sprite.renderer.material = material;
					result = material.shader.name;
					


				}
				else
				{
					result = orig(self);
				}
			}
			return result;
		}
	}
}
