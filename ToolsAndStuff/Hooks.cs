
using AmmonomiconAPI;
using CustomCharacters;
using Dungeonator;
using ETGGUI.Inspector;
using GungeonAPI;
using InControl;
using ItemAPI;
using MonoMod.RuntimeDetour;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
				//Hook PostEventHook = new Hook(typeof(AkSoundEngine).GetProperty("PostEvent", BindingFlags.Static | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("PostEventHook"));
				/*var PostEventHook = new Hook(typeof(AkSoundEngine).GetMethods().Single(
					m =>
						m.Name == "PostEvent" &&
						m.GetGenericArguments().Length <= 2 &&
						m.GetParameters().Length <= 2 &&
						m.GetParameters()[0].ParameterType == typeof(string)),
					typeof(Hooks).GetMethod("PostEventHook", BindingFlags.Static | BindingFlags.Public));*/

				BotsModule.Log("ahhhhh 1", "#eb1313");
				var dumbPainfulHook = new Hook(
					typeof(BraveOptionsMenuItem).GetMethod("DetermineAvailableOptions", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DetermineAvailableOptionsHook", BindingFlags.Static | BindingFlags.NonPublic));



				BotsModule.Log("ahhhhh 2", "#eb1313");
				var lessPainfulButStillDumbHook = new Hook(
					typeof(FinalIntroSequenceManager).GetMethod("TriggerSequence", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("TriggerSequenceHook", BindingFlags.Static | BindingFlags.Public));
				BotsModule.Log("ahhhhh 3", "#eb1313");
				var painlessHook = new Hook(
					typeof(CharacterSelectController).GetMethod("orig_GetCharacterPathFromQuickStart", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHook", BindingFlags.Static | BindingFlags.Public));
				BotsModule.Log("ahhhhh 4", "#eb1313");

				var dumbHookINeedCozZatherzDumb = new Hook(
					typeof(CharacterSelectController).GetMethod("GetCharacterPathFromIdentity", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromIdentityHook", BindingFlags.Static | BindingFlags.Public));

				var dumbHookINeedCozZatherzDumb2 = new Hook(
					typeof(CharacterSelectController).GetMethod("GetCharacterPathFromQuickStart", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHookHook", BindingFlags.Static | BindingFlags.Public));

				ETGModConsole.Log("pre shitty hook");
				var stupidFuckingHookIMadeForAShittyJoke = new Hook(typeof(MinorBreakable).GetMethods().Single(
					m =>
						m.Name == "Break" &&
						m.GetParameters().Length >= 1 &&
						m.GetParameters()[0].ParameterType == typeof(Vector2)),
					typeof(Hooks).GetMethod("BreakHook", BindingFlags.Static | BindingFlags.Public));
				ETGModConsole.Log("post shitty hook");

				/*
				var doNotificationInternalHook = new Hook(
					typeof(UINotificationController).GetMethod("DoNotificationInternal", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DoNotificationInternalHook", BindingFlags.Static | BindingFlags.NonPublic));
				*/
			}
			catch (Exception arg)
			{
				BotsModule.Log("oh no thats not good (hooks broke): " + arg, "#eb1313");
				//LostItemsMod.Log(string.Format("D:", ), "#eb1313");
			}
		}
		static bool setup = false;
		static dfAnimationClip beyondClip;
		private static void DoNotificationInternalHook(UINotificationController self, NotificationParams notifyParams)
		{
			if (!setup)
			{

				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_001" + ".png"), "notification_box_beyond_001");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_002" + ".png"), "notification_box_beyond_002");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_003" + ".png"), "notification_box_beyond_003");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_004" + ".png"), "notification_box_beyond_004");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_005" + ".png"), "notification_box_beyond_005");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_006" + ".png"), "notification_box_beyond_006");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_007" + ".png"), "notification_box_beyond_007");
				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_008" + ".png"), "notification_box_beyond_008");

				self.BoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/notification_box_beyond_001" + ".png"), "notification_box_beyondns_001");

				self.CrosshairSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/crosshair_beyond" + ".png"), "crosshair_beyond");
				self.ObjectBoxSprite.Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/NotificationSprites/object_box_beyond_001" + ".png"), "object_box_beyond_001");


				beyondClip = new dfAnimationClip
				{
					Atlas = self.BoxSprite.Atlas,
				};

				FieldInfo _sprites = typeof(dfAnimationClip).GetField("sprites", BindingFlags.NonPublic | BindingFlags.Instance);

				_sprites.SetValue(beyondClip, new List<string> { "notification_box_beyond_001", "notification_box_beyond_002", "notification_box_beyond_003", "notification_box_beyond_004", "notification_box_beyond_005", "notification_box_beyond_006", "notification_box_beyond_007", "notification_box_beyond_008", "notification_box_beyondns_001" });


				setup = true;
			}


			FieldInfo _queuedNotifications = typeof(UINotificationController).GetField("m_queuedNotifications", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _queuedNotificationParams = typeof(UINotificationController).GetField("m_queuedNotificationParams", BindingFlags.NonPublic | BindingFlags.Instance);

			(_queuedNotifications.GetValue(self) as List<IEnumerator>).Add(HandleNotification(self, notifyParams));
			(_queuedNotificationParams.GetValue(self) as List<NotificationParams>).Add(notifyParams);
			self.StartCoroutine((IEnumerator)typeof(UINotificationController).GetMethod("PruneQueuedNotifications", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null));
		}

		private static IEnumerator HandleNotification(UINotificationController self, NotificationParams notifyParams)
		{
			yield return null;
			typeof(UINotificationController).GetMethod("SetupSprite", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { notifyParams.SpriteCollection, notifyParams.SpriteID });

			FieldInfo _doingNotification = typeof(UINotificationController).GetField("m_doingNotification", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _panel = typeof(UINotificationController).GetField("m_panel", BindingFlags.NonPublic | BindingFlags.Instance);


			self.DescriptionLabel.ProcessMarkup = true;
			self.DescriptionLabel.ColorizeSymbols = true;
			self.NameLabel.Text = notifyParams.PrimaryTitleString.ToUpperInvariant();
			self.DescriptionLabel.Text = notifyParams.SecondaryDescriptionString;
			self.CenterLabel.Opacity = 1f;
			self.NameLabel.Opacity = 1f;
			self.DescriptionLabel.Opacity = 1f;
			self.CenterLabel.IsVisible = false;
			self.NameLabel.IsVisible = true;
			self.DescriptionLabel.IsVisible = true;
			dfSpriteAnimation component = self.BoxSprite.GetComponent<dfSpriteAnimation>();
			component.Stop();
			dfSpriteAnimation component2 = self.CrosshairSprite.GetComponent<dfSpriteAnimation>();
			component2.Stop();
			dfSpriteAnimation component3 = self.ObjectBoxSprite.GetComponent<dfSpriteAnimation>();
			component3.Stop();
			UINotificationController.NotificationColor forcedColor = notifyParams.forcedColor;
			string trackableGuid = notifyParams.EncounterGuid;
			bool isGold = forcedColor == UINotificationController.NotificationColor.GOLD || (!string.IsNullOrEmpty(trackableGuid) && GameStatsManager.Instance.QueryEncounterable(trackableGuid) == 1);
			bool isPurple = forcedColor == UINotificationController.NotificationColor.PURPLE || (!string.IsNullOrEmpty(trackableGuid) && EncounterDatabase.GetEntry(trackableGuid).usesPurpleNotifications);
			bool isBeyond = forcedColor == (UINotificationController.NotificationColor)CustomEnums.CustomNotificationColor.BEYOND || (!string.IsNullOrEmpty(trackableGuid));
			typeof(UINotificationController).GetMethod("ToggleGoldStatus", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { isGold });
			typeof(UINotificationController).GetMethod("TogglePurpleStatus", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { isPurple });
			ToggleBeyondStatus(self, isBeyond);
			bool singleLineSansSprite = notifyParams.isSingleLine;
			if (singleLineSansSprite || notifyParams.SpriteCollection == null)
			{
				self.ObjectBoxSprite.IsVisible = false;
				self.StickerSprite.IsVisible = false;
			}
			if (singleLineSansSprite)
			{
				self.CenterLabel.IsVisible = true;
				self.NameLabel.IsVisible = false;
				self.DescriptionLabel.IsVisible = false;
				self.CenterLabel.Text = self.NameLabel.Text;
			}
			else
			{
				self.NameLabel.IsVisible = true;
				self.DescriptionLabel.IsVisible = true;
				self.CenterLabel.IsVisible = false;
			}
			_doingNotification.SetValue(self, true);
			(_panel.GetValue(self) as dfPanel).IsVisible = false;
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen((_panel.GetValue(self) as dfPanel), false);
			float elapsed = 0f;
			float duration = 5f;
			bool hasPlayedAnim = false;
			if (singleLineSansSprite)
			{
				self.notificationObjectSprite.renderer.enabled = false;
				SpriteOutlineManager.ToggleOutlineRenderers(self.notificationObjectSprite, false);
			}
			while (elapsed < ((!notifyParams.HasAttachedSynergy) ? duration : (duration - 2f)))
			{
				elapsed += BraveTime.DeltaTime;
				if (!hasPlayedAnim && elapsed > 0.75f)
				{
					self.BoxSprite.GetComponent<dfSpriteAnimation>().Clip = ((!isPurple) ? ((!isGold) ? ((!isBeyond) ? self.SilverAnimClip : beyondClip) : self.GoldAnimClip) : self.PurpleAnimClip);
					hasPlayedAnim = true;
					self.ObjectBoxSprite.Parent.GetComponent<dfSpriteAnimation>().Play();
				}
				yield return null;
				(_panel.GetValue(self) as dfPanel).IsVisible = true;
				if (!singleLineSansSprite && notifyParams.SpriteCollection != null)
				{
					self.notificationObjectSprite.renderer.enabled = true;
					SpriteOutlineManager.ToggleOutlineRenderers(self.notificationObjectSprite, true);
				}
			}
			if (notifyParams.HasAttachedSynergy)
			{
				AdvancedSynergyEntry pairedSynergy = notifyParams.AttachedSynergy;
				EncounterDatabaseEntry encounterSource = EncounterDatabase.GetEntry(trackableGuid);
				int pickupObjectId = (encounterSource == null) ? -1 : encounterSource.pickupObjectId;
				PickupObject puo = PickupObjectDatabase.GetById(pickupObjectId);
				if (puo)
				{
					//int pID = self.GetIDOfOwnedSynergizingItem(puo.PickupObjectId, pairedSynergy);
					int pID = (int)typeof(UINotificationController).GetMethod("GetIDOfOwnedSynergizingItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { puo.PickupObjectId, pairedSynergy });
					PickupObject puo2 = PickupObjectDatabase.GetById(pID);
					if (puo2 && puo2.sprite)
					{
						typeof(UINotificationController).GetMethod("SetupSynergySprite", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { puo2.sprite.Collection, puo2.sprite.spriteId });
						elapsed = 0f;
						duration = 4f;
						self.notificationSynergySprite.renderer.enabled = true;
						SpriteOutlineManager.ToggleOutlineRenderers(self.notificationSynergySprite, true);
						dfSpriteAnimation boxSpriteAnimator = self.BoxSprite.GetComponent<dfSpriteAnimation>();
						boxSpriteAnimator.Clip = self.SynergyTransformClip;
						boxSpriteAnimator.Play();
						dfSpriteAnimation crosshairSpriteAnimator = self.CrosshairSprite.GetComponent<dfSpriteAnimation>();
						crosshairSpriteAnimator.Clip = self.SynergyCrosshairTransformClip;
						crosshairSpriteAnimator.Play();
						dfSpriteAnimation objectSpriteAnimator = self.ObjectBoxSprite.GetComponent<dfSpriteAnimation>();
						objectSpriteAnimator.Clip = self.SynergyBoxTransformClip;
						objectSpriteAnimator.Play();
						string synergyName = string.IsNullOrEmpty(pairedSynergy.NameKey) ? string.Empty : StringTableManager.GetSynergyString(pairedSynergy.NameKey, -1);
						bool synergyHasName = !string.IsNullOrEmpty(synergyName);
						if (synergyHasName)
						{
							self.CenterLabel.IsVisible = true;
							self.CenterLabel.Text = synergyName;
						}
						while (elapsed < duration)
						{
							float baseSpriteLocalX = self.notificationObjectSprite.transform.localPosition.x;
							float synSpriteLocalX = self.notificationSynergySprite.transform.localPosition.x;
							self.CrosshairSprite.Size = self.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
							float p2u = self.BoxSprite.PixelsToUnits();
							Vector3 endPosition = self.ObjectBoxSprite.GetCenter();
							Vector3 startPosition = endPosition + new Vector3(0f, -120f * p2u, 0f);
							Vector3 startPosition2 = endPosition;
							Vector3 endPosition2 = endPosition + new Vector3(0f, 12f * p2u, 0f);
							endPosition -= new Vector3(0f, 21f * p2u, 0f);
							float t = elapsed / duration;
							float quickT = elapsed / 1f;
							float smoothT = Mathf.SmoothStep(0f, 1f, quickT);
							if (synergyHasName)
							{
								float num = Mathf.SmoothStep(0f, 1f, elapsed / 0.5f);
								float opacity = Mathf.SmoothStep(0f, 1f, (elapsed - 0.5f) / 0.5f);
								self.NameLabel.Opacity = 1f - num;
								self.DescriptionLabel.Opacity = 1f - num;
								self.CenterLabel.Opacity = opacity;
							}
							Vector3 t2 = Vector3.Lerp(startPosition, endPosition, smoothT).Quantize(p2u * 3f).WithX(startPosition.x);
							Vector3 t3 = Vector3.Lerp(startPosition2, endPosition2, smoothT).Quantize(p2u * 3f).WithX(startPosition2.x);
							t3.y = Mathf.Max(startPosition2.y, t3.y);
							self.notificationSynergySprite.PlaceAtPositionByAnchor(t2, tk2dBaseSprite.Anchor.MiddleCenter);
							self.notificationSynergySprite.transform.position = self.notificationSynergySprite.transform.position + new Vector3(0f, 0f, -0.125f);
							self.notificationObjectSprite.PlaceAtPositionByAnchor(t3, tk2dBaseSprite.Anchor.MiddleCenter);
							self.notificationObjectSprite.transform.localPosition = self.notificationObjectSprite.transform.localPosition.WithX(baseSpriteLocalX);
							self.notificationSynergySprite.transform.localPosition = self.notificationSynergySprite.transform.localPosition.WithX(synSpriteLocalX);
							self.notificationSynergySprite.UpdateZDepth();
							self.notificationObjectSprite.UpdateZDepth();
							elapsed += BraveTime.DeltaTime;
							yield return null;
						}
					}
				}
			}
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen((_panel.GetValue(self) as dfPanel), true);
			elapsed = 0f;
			duration = 0.25f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			self.CenterLabel.Opacity = 1f;
			self.NameLabel.Opacity = 1f;
			self.DescriptionLabel.Opacity = 1f;
			self.CenterLabel.IsVisible = false;
			self.NameLabel.IsVisible = true;
			self.DescriptionLabel.IsVisible = true;

			typeof(UINotificationController).GetMethod("DisableRenderers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			_doingNotification.SetValue(self, false);
			yield break;
		}


		private static void ToggleBeyondStatus(UINotificationController self, bool beyond)
		{
			self.CrosshairSprite.SpriteName = ((!beyond) ? "crosshair" : "crosshair_beyond");
			self.CrosshairSprite.Size = self.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
			self.BoxSprite.SpriteName = ((!beyond) ? "notification_box" : "notification_box_beyondns_001");
			self.ObjectBoxSprite.IsVisible = true;
			self.ObjectBoxSprite.SpriteName = ((!beyond) ? "object_box" : "object_box_beyond_001");
			self.StickerSprite.IsVisible = beyond;
		}


		public static void BreakHook(Action<MinorBreakable, Vector2> orig, MinorBreakable self, Vector2 direction)
		{

			ETGModConsole.Log("fuck");

			bool spawnedFairy = false;

			FieldInfo _isBroken = typeof(MinorBreakable).GetField("m_isBroken", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _groupManager = typeof(MinorBreakable).GetField("m_groupManager", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _occupiedCells = typeof(MinorBreakable).GetField("m_occupiedCells", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _spriteAnimator = typeof(MinorBreakable).GetField("m_spriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _sprite = typeof(MinorBreakable).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _transform = typeof(MinorBreakable).GetField("m_transform", BindingFlags.NonPublic | BindingFlags.Instance);

			if (!self.enabled)
			{
				return;
			}
			if ((bool)_isBroken.GetValue(self))
			{
				return;
			}
			_isBroken.SetValue(self, true);
			if (_groupManager.GetValue(self) != null)
			{
				(_groupManager.GetValue(self) as MinorBreakableGroupManager).InformBroken(self, direction, self.heightOffGround);
			}
			bool flag = GameManager.Instance.InTutorial;
			if (GameManager.Instance.PrimaryPlayer.CurrentRoom != null)
			{
				flag = (flag || GameManager.Instance.PrimaryPlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL);
			}
			if (flag && !self.name.Contains("table", true) && !self.explodesOnBreak)
			{
				GameManager.BroadcastRoomTalkDoerFsmEvent("playerBrokeShit");
			}
			if (_occupiedCells.GetValue(self) != null)
			{
				(_occupiedCells.GetValue(self) as OccupiedCells).Clear();
			}
			IPlayerInteractable @interface = self.gameObject.GetInterface<IPlayerInteractable>();
			if (@interface != null)
			{
				RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(self.transform.position.IntXY(VectorConversions.Round));
				if (roomFromPosition == null)
				{
					roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(self.transform.position.IntXY(VectorConversions.Round) + IntVector2.Right);
				}
				if (roomFromPosition != null && roomFromPosition.IsRegistered(@interface))
				{
					roomFromPosition.DeregisterInteractable(@interface);
				}
			}
			if (self.specRigidbody != null)
			{
				self.specRigidbody.enabled = false;
			}
			bool flag2 = false;
			if (_spriteAnimator.GetValue(self) != null && self.breakAnimName != string.Empty)
			{
				tk2dSpriteAnimationClip clipByName = (_spriteAnimator.GetValue(self) as tk2dSpriteAnimator).GetClipByName(self.breakAnimName);
				if (clipByName != null)
				{
					(_spriteAnimator.GetValue(self) as tk2dSpriteAnimator).Play(clipByName);
					flag2 = true;
					self.Invoke("OnBreakAnimationComplete", clipByName.BaseClipLength);
				}
			}
			else if (!string.IsNullOrEmpty(self.breakAnimFrame))
			{
				(_sprite.GetValue(self) as tk2dSprite).SetSprite(self.breakAnimFrame);
			}
			if (!(_transform.GetValue(self) as Transform))
			{
				_transform.SetValue(self, self.transform);
			}
			if ((_transform.GetValue(self) as Transform))
			{
				AkSoundEngine.SetObjectPosition(self.gameObject, (_transform.GetValue(self) as Transform).position.x, (_transform.GetValue(self) as Transform).position.y, (_transform.GetValue(self) as Transform).position.z, (_transform.GetValue(self) as Transform).forward.x, (_transform.GetValue(self) as Transform).forward.y, (_transform.GetValue(self) as Transform).forward.z, (_transform.GetValue(self) as Transform).up.x, (_transform.GetValue(self) as Transform).up.y, (_transform.GetValue(self) as Transform).up.z);
			}
			if (!string.IsNullOrEmpty(self.breakAudioEventName))
			{
				AkSoundEngine.PostEvent(self.breakAudioEventName, self.gameObject);
			}
			typeof(MinorBreakable).GetMethod("HandleShardSpawns", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { direction });
			typeof(MinorBreakable).GetMethod("HandleParticulates", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { direction });
			typeof(MinorBreakable).GetMethod("HandleSynergies", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);

			SurfaceDecorator component = self.GetComponent<SurfaceDecorator>();
			if (component != null)
			{
				component.Destabilize(direction.normalized);
			}
			typeof(MinorBreakable).GetMethod("DestabilizeAttachedObjects", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { direction.normalized });
			if (self.canSpawnFairy && GameManager.Instance.Dungeon.sharedSettingsPrefab.RandomShouldSpawnPotFairy())
			{
				IntVector2 intVector = self.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
				RoomHandler roomFromPosition2 = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
				PotFairyEngageDoer.InstantSpawn = true;
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(GameManager.Instance.Dungeon.sharedSettingsPrefab.PotFairyGuid);
				AIActor.Spawn(orLoadByGuid, intVector, roomFromPosition2, true, AIActor.AwakenAnimationType.Default, true);
				spawnedFairy = true;
			}
			if (self.canSpawnFairy && UnityEngine.Random.RandomRange(0.0f, 1.0f) >= 0.5f && spawnedFairy == false)
			{
				IntVector2 intVector = self.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
				RoomHandler roomFromPosition2 = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
				//PotFairyEngageDoer.InstantSpawn = true;
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c");
				AIActor.Spawn(orLoadByGuid, intVector, roomFromPosition2, true, AIActor.AwakenAnimationType.Default, true);
			}
			if (self.OnBreak != null)
			{
				self.OnBreak();
			}
			if (self.OnBreakContext != null)
			{
				self.OnBreakContext(self);
			}
			if (self.destroyOnBreak && !flag2)
			{
				if (self.ForcedDestroyDelay > 0f)
				{
					UnityEngine.Object.Destroy(self.gameObject, self.ForcedDestroyDelay);
				}
				else
				{
					UnityEngine.Object.Destroy(self.gameObject);
				}
			}
		}
		public static uint PostEventHook(Func<string ,GameObject, uint> orig, string in_pszEventName, GameObject in_gameObjectID)
		{
			ETGModConsole.Log(in_pszEventName);
			return orig(in_pszEventName, in_gameObjectID);
		}


		public static string GetCharacterPathFromQuickStartHookHook()
		{
			return "PlayerLost";
			if (ETGMod.Player.QuickstartReplacement != null)
			{
				return ETGMod.Player.QuickstartReplacement;
			}
			if (GameManager.Options.PreferredQuickstartCharacter == GameOptions.QuickstartCharacter.LAST_USED)
			{
				return CharacterSelectController.GetCharacterPathFromIdentity(GameManager.Options.LastPlayedCharacter);
			}
			return GetCharacterPathFromQuickStartHook();
		}

		public static string GetCharacterPathFromIdentityHook(Func<PlayableCharacters, string> orig, PlayableCharacters id)
		{
			if (id == (PlayableCharacters)CustomPlayableCharacters.Lost)
			{
				return "PlayerLost";
			}
			return orig(id);
		}


		public static string GetCharacterPathFromQuickStartHook()
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
				BotsModule.Log($"lost not unlocked?? :/");
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
			}
			BotsModule.Log(quickstartCharacter.ToString());
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
				case (GameOptions.QuickstartCharacter)CustomEnums.QuickstartCharacter.LOST:
					return "PlayerLost";
				default:
					return "PlayerRogue";
			}
		}

		public static void TriggerSequenceHook(Action<FinalIntroSequenceManager> orig, FinalIntroSequenceManager self)
		{
			if (Foyer.DoIntroSequence)
			{
				GameManager.Instance.StartCoroutine((IEnumerator)typeof(FinalIntroSequenceManager).GetMethod("CoreSequence", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null));
				ETGModConsole.Log("first thing started");
				GameManager.Instance.StartCoroutine(HandleBackgroundSkipChecks(self));
				ETGModConsole.Log("second thing started");
			}
		}

		private static IEnumerator HandleBackgroundSkipChecks(FinalIntroSequenceManager self)
		{
			FieldInfo _skipCycle = typeof(FinalIntroSequenceManager).GetField("m_skipCycle", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _isDoingQuickStart = typeof(FinalIntroSequenceManager).GetField("m_isDoingQuickStart", BindingFlags.NonPublic | BindingFlags.Instance);

			yield return null;


			for (; ; )
			{
				if (self.QuickStartObject.activeSelf)
				{
					if (!BraveInput.PlayerlessInstance.IsKeyboardAndMouse(false))
					{
						self.QuickStartController.gameObject.SetActive(true);
						self.QuickStartController.renderer.enabled = true;
						self.QuickStartKeyboard.gameObject.SetActive(false);
					}
					else
					{
						self.QuickStartKeyboard.gameObject.SetActive(true);
						self.QuickStartController.gameObject.SetActive(false);
					}
				}
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
				{
					_skipCycle.SetValue(self, true);
				}
				if (!(bool)_isDoingQuickStart.GetValue(self) && !(bool)_skipCycle.GetValue(self))
				{
					if ((bool)typeof(FinalIntroSequenceManager).GetMethod("QuickStartAvailable", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null) && (BraveInput.PlayerlessInstance.ActiveActions.Device.Action4.WasPressed || Input.GetKeyDown(KeyCode.Q)))
					{
						_skipCycle.SetValue(self, true);
						_isDoingQuickStart.SetValue(self, true);
						self.StartCoroutine(DoQuickStart(self));
					}
					if (BraveInput.PlayerlessInstance.ActiveActions.Device.Action1.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.Action2.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.Action3.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.CommandWasPressed || BraveInput.PlayerlessInstance.ActiveActions.MenuSelectAction.WasPressed)
					{
						_skipCycle.SetValue(self, true);
					}
				}

				if ((bool)_skipCycle.GetValue(self) == true)
				{
					//ETGModConsole.Log("balls wide", true);
				}

				yield return null;
			}
			yield break;
		}

		private static IEnumerator DoQuickStart(FinalIntroSequenceManager self)
		{
			FieldInfo _inFoyer = typeof(FinalIntroSequenceManager).GetField("m_inFoyer", BindingFlags.NonPublic | BindingFlags.Instance);

			self.QuickStartObject.SetActive(false);
			self.StartCoroutine(FadeToBlack(0.1f, self, true, true));
			GameManager.PreventGameManagerExistence = false;
			GameManager.SKIP_FOYER = true;
			Foyer.DoMainMenu = false;
			if (!(bool)_inFoyer.GetValue(self))
			{
				uint num = 1U;
				DebugTime.RecordStartTime();
				AkSoundEngine.LoadBank("SFX.bnk", -1, out num);
				DebugTime.Log("FinalIntroSequenceManager.DoQuickStart.LoadBank()", new object[0]);
				GameManager.EnsureExistence();
			}
			AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", self.gameObject);
			MidGameSaveData saveToContinue = null;
			if (GameManager.VerifyAndLoadMidgameSave(out saveToContinue, true))
			{
				yield return null;
				Dungeon.ShouldAttemptToLoadFromMidgameSave = true;
				GameManager.Instance.SetNextLevelIndex(GameManager.Instance.GetTargetLevelIndexFromSavedTileset(saveToContinue.levelSaved));
				GameManager.Instance.GeneratePlayersFromMidGameSave(saveToContinue);
				if (!(bool)_inFoyer.GetValue(self))
				{
					GameManager.Instance.FlushAudio();
				}
				GameManager.Instance.IsFoyer = false;
				Foyer.DoIntroSequence = false;
				Foyer.DoMainMenu = false;
				GameManager.Instance.IsSelectingCharacter = false;
				GameManager.Instance.DelayedLoadMidgameSave(0.25f, saveToContinue);
			}
			else
			{
				BotsModule.Log("if you see this before \"List of Custom Characters From Enter the Beyond:\" then you fucked it up", BotsModule.LOST_COLOR);
				GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(CharacterSelectController.GetCharacterPathFromQuickStart(), ".prefab");



				GameManager.Instance.GlobalInjectionData.PreprocessRun(false);
				yield return null;
				PlayerController playerController = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
				GameStatsManager.Instance.BeginNewSession(playerController);
				GameObject instantiatedPlayer = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, Vector3.zero, Quaternion.identity);
				GameManager.PlayerPrefabForNewGame = null;
				instantiatedPlayer.SetActive(true);
				PlayerController extantPlayer = instantiatedPlayer.GetComponent<PlayerController>();
				extantPlayer.PlayerIDX = 0;
				GameManager.Instance.PrimaryPlayer = extantPlayer;
				if (!(bool)_inFoyer.GetValue(self))
				{
					GameManager.Instance.FlushAudio();
				}
				GameManager.Instance.FlushMusicAudio();
				GameManager.Instance.SetNextLevelIndex(1);
				GameManager.Instance.IsSelectingCharacter = false;
				GameManager.Instance.IsFoyer = false;
				GameManager.Instance.DelayedLoadNextLevel(0.5f);
				yield return null;
				yield return null;
				yield return null;
				Foyer.Instance.OnDepartedFoyer();
			}
			yield break;
		}

		private static IEnumerator FadeToBlack(float duration, FinalIntroSequenceManager self, bool startAtCurrent = false, bool force = false)
		{

			FieldInfo _skipCycle = typeof(FinalIntroSequenceManager).GetField("m_skipCycle", BindingFlags.NonPublic | BindingFlags.Instance);
			float elapsed = 0f;
			float startValue = 0f;
			if (startAtCurrent)
			{
				startValue = self.FadeMaterial.GetColor("_Color").a;
			}
			while (elapsed < duration)
			{
				if (!force && (bool)_skipCycle.GetValue(self))
				{
					yield break;
				}
				elapsed += Time.deltaTime;
				float t = elapsed / duration;
				self.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, Mathf.Lerp(startValue, 1f, t)));
				yield return null;
			}
			self.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 1f));
			yield break;
		}


		private static void DetermineAvailableOptionsHook(Action<BraveOptionsMenuItem> orig, BraveOptionsMenuItem self)
		{

			FieldInfo _scalingModes = typeof(BraveOptionsMenuItem).GetField("m_scalingModes", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _selectedIndex = typeof(BraveOptionsMenuItem).GetField("m_selectedIndex", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _self = typeof(BraveOptionsMenuItem).GetField("m_self", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _quickStartCharacters = typeof(BraveOptionsMenuItem).GetField("m_quickStartCharacters", BindingFlags.NonPublic | BindingFlags.Instance);

			BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = self.optionType;
			switch (braveOptionsOptionType)
			{
				case BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION:
					typeof(BraveOptionsMenuItem).GetMethod("HandleResolutionDetermination", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					//self.HandleResolutionDetermination();
					break;
				case BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE:
					{
						int width = Screen.width;
						int height = Screen.height;
						int num = BraveMathCollege.GreatestCommonDivisor(width, height);
						int num2 = width / num;
						int num3 = height / num;
						List<string> list = new List<string>();
						if (_scalingModes.GetValue(self) == null)
						{
							_scalingModes.SetValue(self, new List<GameOptions.PreferredScalingMode>());
						}
						var list1 = new List<GameOptions.PreferredScalingMode>();
						list1.Clear();
						_scalingModes.SetValue(self, list1);
						if (num2 == 16 && num3 == 9)
						{
							if (width % 480 == 0 && height % 270 == 0)
							{
								list.Add("#OPTIONS_PIXELPERFECT");

								List<GameOptions.PreferredScalingMode> list2 = (List<GameOptions.PreferredScalingMode>)_scalingModes.GetValue(self);
								list2.Add(GameOptions.PreferredScalingMode.PIXEL_PERFECT);

								_scalingModes.SetValue(self, list2);
							}
							else
							{


								List<GameOptions.PreferredScalingMode> list2 = (List<GameOptions.PreferredScalingMode>)_scalingModes.GetValue(self);
								list.Add("#OPTIONS_UNIFORMSCALING");
								list2.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING);
								list.Add("#OPTIONS_FORCEPIXELPERFECT");
								list2.Add(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
								list.Add("#OPTIONS_UNIFORMSCALINGFAST");
								list2.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST);

								_scalingModes.SetValue(self, list2);

							}
						}
						else
						{
							List<GameOptions.PreferredScalingMode> list2 = (List<GameOptions.PreferredScalingMode>)_scalingModes.GetValue(self);
							list.Add("#OPTIONS_UNIFORMSCALING");
							list2.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING);
							list.Add("#OPTIONS_FORCEPIXELPERFECT");
							list2.Add(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
							list.Add("#OPTIONS_UNIFORMSCALINGFAST");
							list2.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST);

							_scalingModes.SetValue(self, list2);
						}
						self.labelOptions = list.ToArray();
						List<GameOptions.PreferredScalingMode> list3 = (List<GameOptions.PreferredScalingMode>)_scalingModes.GetValue(self);
						if (list3.Contains(GameManager.Options.CurrentPreferredScalingMode))
						{
							//_selectedIndex.SetValue(self, (int)typeof(BraveOptionsMenuItem).InvokeMember("GetScalingIndex", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, self, new object[] { GameManager.Options.CurrentPreferredScalingMode }));

							_selectedIndex.SetValue(self, typeof(BraveOptionsMenuItem).GetMethod("GetScalingIndex", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Options.CurrentPreferredScalingMode }));

						}
						else
						{
							_selectedIndex.SetValue(self, 0);
							if (list.Count >= 2 && GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
							{
								_selectedIndex.SetValue(self, 1);
							}
						}
						typeof(BraveOptionsMenuItem).GetMethod("RelocalizeOptions", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
						break;
					}
				default:

					switch (braveOptionsOptionType)
					{
						case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT:
							{
								List<string> list2 = new List<string>();
								for (int i = 0; i < InputManager.Devices.Count; i++)
								{
									string name = InputManager.Devices[i].Name;
									int num4 = 1;
									string item = name;
									while (list2.Contains(item))
									{
										num4++;
										item = name + " " + num4.ToString();
									}
									list2.Add(item);
								}
								list2.Add((_self.GetValue(self) as dfControl).ForceGetLocalizedValue("#OPTIONS_KEYBOARDMOUSE"));
								self.labelOptions = list2.ToArray();
								break;
							}
						default:

							switch (braveOptionsOptionType)
							{
								case BraveOptionsMenuItem.BraveOptionsOptionType.LANGUAGE:
									self.labelOptions = new List<string>
									{
										"#LANGUAGE_ENGLISH",
										"#LANGUAGE_SPANISH",
										"#LANGUAGE_FRENCH",
										"#LANGUAGE_ITALIAN",
										"#LANGUAGE_GERMAN",
										"#LANGUAGE_PORTUGUESE",
										"#LANGUAGE_POLISH",
										"#LANGUAGE_RUSSIAN",
										"#LANGUAGE_JAPANESE",
										"#LANGUAGE_KOREAN",
										"#LANGUAGE_CHINESE"
									}.ToArray();
									typeof(BraveOptionsMenuItem).GetMethod("RelocalizeOptions", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
									break;

								case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSTART_CHARACTER:
									{
										if ((_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>) == null)
										{
											_quickStartCharacters.SetValue(self, new List<GameOptions.QuickstartCharacter>());
										}
										else
										{
											var list4 = new List<GameOptions.QuickstartCharacter>();
											list4.Clear();

											_quickStartCharacters.SetValue(self, list4);
										}

										List<string> list3 = new List<string>(8);


										(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.LAST_USED);
										list3.Add("#CHAR_LASTUSED");
										(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.PILOT);
										list3.Add("#CHAR_ROGUE");
										(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.CONVICT);
										list3.Add("#CHAR_CONVICT");
										(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.SOLDIER);
										list3.Add("#CHAR_MARINE");
										(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.GUIDE);
										list3.Add("#CHAR_GUIDE");
										if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
										{
											(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.BULLET);
											list3.Add("#CHAR_BULLET");
										}
										if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
										{
											(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add(GameOptions.QuickstartCharacter.ROBOT);
											list3.Add("#CHAR_ROBOT");
										}

										if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
										{
											(_quickStartCharacters.GetValue(self) as List<GameOptions.QuickstartCharacter>).Add((GameOptions.QuickstartCharacter)CustomEnums.QuickstartCharacter.LOST);
											list3.Add("#CHAR_LOST");
										}
										self.labelOptions = list3.ToArray();
										_selectedIndex.SetValue(self, typeof(BraveOptionsMenuItem).GetMethod("GetQuickStartCharIndex", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Options.PreferredQuickstartCharacter }));

										//self.m_selectedIndex = self.GetQuickStartCharIndex(GameManager.Options.PreferredQuickstartCharacter);
										if ((int)_selectedIndex.GetValue(self) < 0 || (int)_selectedIndex.GetValue(self) >= self.labelOptions.Length)
										{
											_selectedIndex.SetValue(self, 0);

										}
										typeof(BraveOptionsMenuItem).GetMethod("UpdateSelectedLabelText", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
										//self.();
										break;
									}
							}
							break;
						case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT:
							{
								List<string> list4 = new List<string>();
								for (int j = 0; j < InputManager.Devices.Count; j++)
								{
									string name2 = InputManager.Devices[j].Name;
									int num5 = 1;
									string item2 = name2;
									while (list4.Contains(item2))
									{
										num5++;
										item2 = name2 + " " + num5.ToString();
									}
									list4.Add(item2);
								}
								list4.Add((_self.GetValue(self) as dfControl).ForceGetLocalizedValue("#OPTIONS_KEYBOARDMOUSE"));
								self.labelOptions = list4.ToArray();
								break;
							}
					}
					break;
				case BraveOptionsMenuItem.BraveOptionsOptionType.MONITOR_SELECT:
					{
						List<string> list5 = new List<string>();
						for (int k = 0; k < Display.displays.Length; k++)
						{
							list5.Add((k + 1).ToString());
						}
						self.labelOptions = list5.ToArray();
						break;
					}
			}
			if (self.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || self.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
			{
				if ((int)_selectedIndex.GetValue(self) >= self.labelOptions.Length)
				{
					_selectedIndex.SetValue(self, 0);

				}
				if (self.labelOptions != null && (int)_selectedIndex.GetValue(self) > -1 && (int)_selectedIndex.GetValue(self) < self.labelOptions.Length)
				{
					typeof(BraveOptionsMenuItem).GetMethod("UpdateSelectedLabelText", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					if (self.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
					{
						//self.();
						typeof(BraveOptionsMenuItem).GetMethod("UpdateInfoControl", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					}
				}
				else
				{
					self.selectedLabelControl.Text = "?";
				}
			}
			if (self.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
			{
				typeof(BraveOptionsMenuItem).GetMethod("RepositionCheckboxControl", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			}
			if (self.labelControl)
			{
				self.labelControl.PerformLayout();
			}
			typeof(BraveOptionsMenuItem).GetMethod("UpdateSelectedLabelText", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			typeof(BraveOptionsMenuItem).GetMethod("UpdateInfoControl", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
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
