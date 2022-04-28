
//using AmmonomiconAPI;
using Brave.BulletScript;
using CustomCharacters;
using Dungeonator;
using EnemyAPI;
using ETGGUI;
using ETGGUI.Inspector;

using GungeonAPI;
using HutongGames.PlayMaker;
using InControl;
using ItemAPI;
using MonoMod.RuntimeDetour;
using SaveAPI;
using SGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
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
				

				
				var getOrLoadByName_Hook = new Hook(
					typeof(DungeonDatabase).GetMethod("GetOrLoadByName", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetOrLoadByNameHook", BindingFlags.Static | BindingFlags.Public));

				//Hook LogoHook = new Hook(typeof(MainMenuFoyerController).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic), typeof(hooks).GetMethod("MainMenuUpdateHook", BindingFlags.Instance | BindingFlags.NonPublic), typeof(MainMenuFoyerController));

				//Hook Portal = new Hook(typeof(ParadoxPortalController).GetProperty("Interact", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("HookInteract"));

				
				//Hook DumbPastHook = new Hook(typeof(GameManager).GetMethod("LoadCustomLevel", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("LoadCustomLevel"));

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
					typeof(Hooks).GetMethod("PostEventHook", BindingFlags.Static | BindingFlags.Public));

				//BotsModule.Log("ahhhhh 1", "#eb1313");
				var dumbPainfulHook = new Hook(
					typeof(BraveOptionsMenuItem).GetMethod("DetermineAvailableOptions", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DetermineAvailableOptionsHook", BindingFlags.Static | BindingFlags.NonPublic));


				BotsModule.Log("aaa2");
				//BotsModule.Log("ahhhhh 2", "#eb1313");
				var lessPainfulButStillDumbHook = new Hook(
					typeof(FinalIntroSequenceManager).GetMethod("TriggerSequence", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("TriggerSequenceHook", BindingFlags.Static | BindingFlags.Public));
				//BotsModule.Log("ahhhhh 3", "#eb1313");
				var painlessHook = new Hook(
					typeof(CharacterSelectController).GetMethod("orig_GetCharacterPathFromQuickStart", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHook", BindingFlags.Static | BindingFlags.Public));
				//BotsModule.Log("ahhhhh 4", "#eb1313");

				var dumbHookINeedCozZatherzDumb = new Hook(
					typeof(CharacterSelectController).GetMethod("GetCharacterPathFromIdentity", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromIdentityHook", BindingFlags.Static | BindingFlags.Public));

				var dumbHookINeedCozZatherzDumb2 = new Hook(
					typeof(CharacterSelectController).GetMethod("GetCharacterPathFromQuickStart", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetCharacterPathFromQuickStartHookHook", BindingFlags.Static | BindingFlags.Public));
				/*
				ETGModConsole.Log("pre shitty hook");
				var stupidFuckingHookIMadeForAShittyJoke = new Hook(typeof(MinorBreakable).GetMethods().Single(
					m =>
						m.Name == "Break" &&
						m.GetParameters().Length >= 1 &&
						m.GetParameters()[0].ParameterType == typeof(Vector2)),
					typeof(Hooks).GetMethod("BreakHook", BindingFlags.Static | BindingFlags.Public));
				ETGModConsole.Log("post shitty hook");
				*/

				var doNotificationInternalHook = new Hook(
					typeof(UINotificationController).GetMethod("DoNotificationInternal", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DoNotificationInternalHook", BindingFlags.Static | BindingFlags.NonPublic));


				/*var purchaseItemHook = new Hook(
					typeof(BaseShopController).GetMethod("PurchaseItem", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("PurchaseItemHook", BindingFlags.Static | BindingFlags.Public));

				var DoSetupHook = new Hook(
					typeof(BaseShopController).GetMethod("DoSetup", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DoSetupHook", BindingFlags.Static | BindingFlags.Public));


				var interactHook = new Hook(
					typeof(ShopItemController).GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("InteractHook", BindingFlags.Static | BindingFlags.Public));

				var OnEnteredRangeHook = new Hook(
					typeof(ShopItemController).GetMethod("OnEnteredRange", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("OnEnteredRangeHook", BindingFlags.Static | BindingFlags.Public));
				
				
				var ModifiedPriceHook = new Hook(
				   typeof(ShopItemController).GetProperty("ModifiedPrice", BindingFlags.Public | BindingFlags.Instance).GetGetMethod(),
				   typeof(Hooks).GetMethod("ModifiedPriceHook"));


				var InitializeInternalHook = new Hook(
					typeof(ShopItemController).GetMethod("InitializeInternal", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("InitializeInternalHook", BindingFlags.Static | BindingFlags.NonPublic));*/


				/*BotsModule.Log("pre unfix hook");
				var HandlePreDropHook = new Hook(
					typeof(OnActiveItemUsedSynergyProcessor).GetMethod("HandlePreDrop", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("HandlePreDropHook", BindingFlags.Static | BindingFlags.NonPublic));

				BotsModule.Log("pre unfix hook2");
				var HandleActivationStatusChangedHook = new Hook(
					typeof(OnActiveItemUsedSynergyProcessor).GetMethod("HandleActivationStatusChanged", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("HandleActivationStatusChangedHook", BindingFlags.Static | BindingFlags.NonPublic));

				/*var HandlePostProcessProjectileHook = new Hook(
					typeof(AlphabetSoupSynergyProcessor).GetMethod("HandlePostProcessProjectile", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("HandlePostProcessProjectileHook", BindingFlags.Static | BindingFlags.NonPublic));
				

				
				//BotsModule.Log("aaa3");
				var HookToWriteLogToTxtFile = new Hook(
					typeof(ETGModConsole).GetMethod("Log", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("LogHook", BindingFlags.Static | BindingFlags.Public));

				var HookToWriteLogToTxtFile2 = new Hook(typeof(Debug).GetMethods().Single(
					m =>
						m.Name == "Log" &&
						m.GetParameters().Length == 1 &&
						m.GetParameters()[0].ParameterType == typeof(object)),
					typeof(Hooks).GetMethod("LogHookU", BindingFlags.Static | BindingFlags.Public));
				//ETGModConsole.Log("post shitty hook");
				if (BeyondSettings.Instance.debug)
                {
					var Crime = new Hook(
						typeof(CharacterCostumeSwapper).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic),
						typeof(Hooks).GetMethod("StartHook", BindingFlags.Static | BindingFlags.NonPublic));
				}*/



				var ReloadText = new Hook(
					typeof(GameUIRoot).GetMethod("InformNeedsReload", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("InformNeedsReloadHook", BindingFlags.Static | BindingFlags.Public));

				var SpawnProjectilesHook = new Hook(
					typeof(SuperReaperController).GetMethod("SpawnProjectiles", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("SpawnProjectilesHook", BindingFlags.Static | BindingFlags.NonPublic));

				/*var HandleMotionHook = new Hook(
					typeof(SuperReaperController).GetMethod("HandleMotion", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("HandleMotionHook", BindingFlags.Static | BindingFlags.NonPublic));

				var UpdateHealthHook = new Hook(
					typeof(GameUIHeartController).GetMethod("UpdateHealth", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("UpdateHealthHook", BindingFlags.Static | BindingFlags.Public));

				var AddHeartHook = new Hook(
					typeof(GameUIHeartController).GetMethod("AddHeart", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("AddHeart", BindingFlags.Static | BindingFlags.Public));

				var AddArmourHook = new Hook(
					typeof(GameUIHeartController).GetMethod("AddArmor", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("AddArmorHook", BindingFlags.Static | BindingFlags.Public));

				var IsValidHook = new Hook(
					typeof(GunFormeData).GetMethod("IsValid", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("IsValidHook", BindingFlags.Static | BindingFlags.Public));

				var UpdatePlayerConsumablesHook = new Hook(
					typeof(GameUIRoot).GetMethod("UpdatePlayerConsumables", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("UpdatePlayerConsumablesHook", BindingFlags.Static | BindingFlags.Public));*/

				/*var UpdateAnimationNamesselfdOnSacksHook = new Hook(
					typeof(SackKnightController).GetMethod("UpdateAnimationNamesselfdOnSacks", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("UpdateAnimationNamesselfdOnSacksHook", BindingFlags.Static | BindingFlags.NonPublic));*/


				var StartHookSC = new Hook(
					typeof(ShortcutElevatorController).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("StartHookSC", BindingFlags.Static | BindingFlags.NonPublic));

				var UpdateAnimationsHook = new Hook(
					typeof(GunExt).GetMethod("UpdateAnimations", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("UpdateAnimationsHook", BindingFlags.Static | BindingFlags.Public));

				/*var StupidFuckingHook = new Hook(
					typeof(DebrisObject).GetMethod("Trigger", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("StupidFuckingHook", BindingFlags.Static | BindingFlags.Public));*/
				var HandleHeroSwordSlashHook = new Hook(
					typeof(Gun).GetMethod("HandleHeroSwordSlash", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DodgeRollPleaseJustNullCheckShitIBegYou", BindingFlags.Static | BindingFlags.Public));

				var GetIndexFromTupleArrayHook = new Hook(
					typeof(TK2DDungeonAssembler).GetMethod("GetIndexFromTupleArray", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("GetIndexFromTupleArrayHook", BindingFlags.Static | BindingFlags.NonPublic));

				var TheresNoFuckingWayThisWorks = new Hook(
					typeof(Projectile).GetMethod("OnRigidbodyCollision", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("TheresNoFuckingWayThisWorks", BindingFlags.Static | BindingFlags.NonPublic));


				var TriggerSilencerHook = new Hook(
					typeof(SilencerInstance).GetMethod("TriggerSilencer", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("TriggerSilencerHook", BindingFlags.Static | BindingFlags.Public));

				var UpdateBlanksHook = new Hook(
					typeof(GameUIBlankController).GetMethod("UpdateBlanks", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("UpdateBlanksHook", BindingFlags.Static | BindingFlags.Public));
				//BotsModule.Log("aaa");
				/*
				var ProcessHeartSpriteModificationsHook = new Hook(
					typeof(GameUIHeartController).GetMethod("ProcessHeartSpriteModifications", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("ProcessHeartSpriteModificationsHook", BindingFlags.Static | BindingFlags.NonPublic));
				//ProcessHeartSpriteModificationsHook

				var ApplyDamageDirectionalHook = new Hook(
					typeof(HealthHaver).GetMethod("ApplyDamageDirectional", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(Hooks).GetMethod("ApplyDamageDirectionalHook", BindingFlags.Static | BindingFlags.Public));

				
				var LocalTimeScaleHook = new Hook(
				   typeof(Projectile).GetProperty("LocalTimeScale", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(),
				   typeof(Hooks).GetMethod("LocalTimeScaleHook", BindingFlags.Static | BindingFlags.Public));
				*/
				//new Hook(typeof(Projectile).GetProperty("LocalTimeScale", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(), typeof(Hooks).GetMethod("LocalTimeScaleHook"));

				//var BraveLogHook = new Hook(
				//typeof(BraveUtility).GetMethod("Log", BindingFlags.Static | BindingFlags.Public),
				//typeof(Hooks).GetMethod("BraveLogHook", BindingFlags.Static | BindingFlags.Public));


				var CheckSourceInKnockbacksHook = new Hook(
					typeof(KnockbackDoer).GetMethod("CheckSourceInKnockbacks", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("CheckSourceInKnockbacksHook", BindingFlags.Static | BindingFlags.NonPublic));

				/*var RegisterConnectedRoomHook = new Hook(
					typeof(RoomHandler).GetMethod("RegisterConnectedRoom", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("RegisterConnectedRoomHook", BindingFlags.Static | BindingFlags.Public)); */
				var AttackHook = new Hook(
					typeof(Gun).GetMethod("Attack", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("AttackHook", BindingFlags.Static | BindingFlags.Public));

				//var UpdateAmmoUIForModuleHook = new Hook(
				//	typeof(GameUIAmmoController).GetMethod("UpdateAmmoUIForModule", BindingFlags.Instance | BindingFlags.NonPublic),
				//	typeof(Hooks).GetMethod("UpdateAmmoUIForModuleHook", BindingFlags.Static | BindingFlags.NonPublic));

				var UpdateGunSpriteHook = new Hook(
					typeof(GameUIAmmoController).GetMethod("UpdateGunSprite", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("UpdateGunSpriteHook", BindingFlags.Static | BindingFlags.NonPublic));

				var UpdateUIGunHook = new Hook(
					typeof(GameUIAmmoController).GetMethod("UpdateUIGun", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("UpdateUIGunHook", BindingFlags.Static | BindingFlags.Public));

				var SpawnClipAtPositionHook = new Hook(
					typeof(Gun).GetMethod("SpawnClipAtPosition", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("SpawnClipAtPositionHook", BindingFlags.Static | BindingFlags.NonPublic));

				var BuildShadowIndexHook = new Hook(
					typeof(TK2DDungeonAssembler).GetMethod("BuildShadowIndex", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("BuildShadowIndexHook", BindingFlags.Static | BindingFlags.NonPublic));

				if (BotsModule.debugMode)
				{
					var AmmoPickupHook = new Hook(
						typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public),
						typeof(Hooks).GetMethod("AmmoPickupHook", BindingFlags.Static | BindingFlags.Public));
				} 

				var ExplodeHook = new Hook(
					typeof(ExplosiveModifier).GetMethod("Explode", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("ExplodeHook", BindingFlags.Static | BindingFlags.Public));

				/*var UpdateBlackPhantomShadersHook = new Hook(
					typeof(AIActor).GetMethod("UpdateBlackPhantomShaders", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("UpdateBlackPhantomShadersHook", BindingFlags.Static | BindingFlags.NonPublic));
				*/
				var ApplyCooldownHook = new Hook(
					typeof(PlayerItem).GetMethod("ApplyCooldown", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("ApplyCooldownHook", BindingFlags.Static | BindingFlags.NonPublic));

				var StopHavingFun = new Hook(
					typeof(GameActor).GetMethod("ApplyEffect", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("StopHavingFun", BindingFlags.Static | BindingFlags.Public));

				/*var ConfigureOnPlacementHook = new Hook(
					typeof(FloorChestPlacer).GetMethod("ConfigureOnPlacement", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("ConfigureOnPlacementHook", BindingFlags.Static | BindingFlags.Public));
				

				new Hook(
					typeof(PlayerController).GetMethod("HandleGunAttachPointInternal", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("HandleGunAttachPointInternalHook", BindingFlags.Static | BindingFlags.NonPublic));
				*/

				BotsModule.Log("hooks set up hopefully");

			}
			catch (Exception arg)
			{
				BotsModule.Log("oh no thats not good (hooks broke): " + arg, "#eb1313");
				//LostItemsMod.Log(string.Format("D:", ), "#eb1313");

			}
		}


		

		public static void ConfigureOnPlacementHook(Action<FloorChestPlacer, RoomHandler> orig, FloorChestPlacer self, RoomHandler room)
		{

			if (self.OverrideChestPrefab == null)
            {
				self.OverrideChestPrefab = BeyondPrefabs.beyondChestPrefab.GetComponent<Chest>();
				self.OverrideChestPrereq = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.CHARACTER,
					requiredCharacter = PlayableCharacters.Guide,
				};
				ETGModConsole.Log("yes");

				if (self.OverrideChestPrereq.CheckConditionsFulfilled()) ETGModConsole.Log("all good");
				if (self.OverrideChestPrefab == null) ETGModConsole.Log("nvm");
			}
			else
            {
				ETGModConsole.Log("no");
			}

			orig(self, room);
		}

		public static void StopHavingFun(Action<GameActor, GameActorEffect, float, Projectile> orig, GameActor self, GameActorEffect effect, float sourcePartialAmount = 1f, Projectile sourceProjectile = null)
		{
			if (self is AIActor && (self as AIActor).EnemyGuid == "bot:Overseer" && effect.effectIdentifier == "Lockdown")
			{
				return;
			}
			orig(self, effect, sourcePartialAmount, sourceProjectile);
		}

		private static void ApplyCooldownHook(Action<PlayerItem, PlayerController> orig, PlayerItem self, PlayerController user)
		{
			if (user.HasPassiveItem(BotsItemIds.BeyondBattery) && BeyondBattery.ShouldNegateCooldown()) {} else { orig(self, user); }
		}

		private static void UpdateBlackPhantomShadersHook(AIActor self)
		{

			FieldInfo _cachedBodySpriteCount = typeof(AIActor).GetField("m_cachedBodySpriteCount", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _cachedBodySpriteShader = typeof(AIActor).GetField("m_cachedBodySpriteShader", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _cachedGunSpriteShader = typeof(AIActor).GetField("m_cachedGunSpriteShader", BindingFlags.NonPublic | BindingFlags.Instance);


			if (self.healthHaver.bodySprites.Count != (int)_cachedBodySpriteCount.GetValue(self))
			{
				_cachedBodySpriteCount.SetValue(self, self.healthHaver.bodySprites.Count);
				for (int i = 0; i < self.healthHaver.bodySprites.Count; i++)
				{
					tk2dBaseSprite tk2dBaseSprite = self.healthHaver.bodySprites[i];
					tk2dBaseSprite.usesOverrideMaterial = true;
					Material material = tk2dBaseSprite.renderer.material;
					if (_cachedBodySpriteShader.GetValue(self) as Shader == null)
					{
						_cachedBodySpriteShader.SetValue(self, material.shader);
					}
					if (self.IsBlackPhantom)
					{
						if (self.OverrideBlackPhantomShader != null)
						{
							material.shader = self.OverrideBlackPhantomShader;
						}
						else
						{
							material.shader = BeyondPrefabs.BeyondJammedShader;
							material.SetColor("_JamColour", new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
							material.SetFloat("_PhantomGradientScale", self.BlackPhantomProperties.GradientScale);
							material.SetFloat("_PhantomContrastPower", self.BlackPhantomProperties.ContrastPower);
							if (tk2dBaseSprite != self.sprite)
							{
								material.SetFloat("_ApplyFade", 0f);
							}
						}
					}
					else
					{
						material.shader = _cachedBodySpriteShader.GetValue(self) as Shader;
					}
					tk2dBaseSprite.renderer.material = material;
				}
				if (self.aiShooter && self.aiShooter.CurrentGun)
				{
					tk2dBaseSprite sprite = self.aiShooter.CurrentGun.GetSprite();
					sprite.usesOverrideMaterial = true;
					Material material2 = sprite.renderer.material;
					if (_cachedGunSpriteShader.GetValue(self) as Shader == null)
					{
						_cachedGunSpriteShader.SetValue(self, material2.shader);
					}
					if (self.IsBlackPhantom)
					{
						if (self.OverrideBlackPhantomShader != null)
						{
							material2.shader = self.OverrideBlackPhantomShader;
						}
						else
						{
							material2.shader = BeyondPrefabs.BeyondJammedShader;
							material2.SetColor("_JamColour", new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
							material2.SetFloat("_PhantomGradientScale", self.BlackPhantomProperties.GradientScale);
							material2.SetFloat("_PhantomContrastPower", self.BlackPhantomProperties.ContrastPower);
							material2.SetFloat("_ApplyFade", 0.3f);
						}
					}
					else
					{
						material2.shader = _cachedBodySpriteShader.GetValue(self) as Shader;
					}
					sprite.renderer.material = material2;
				}
			}
		}

		public static void ExplodeHook(Action<ExplosiveModifier, Vector2, bool, CollisionData> orig, ExplosiveModifier self, Vector2 sourceNormal, bool ignoreDamageCaps = false, CollisionData cd = null)
		{
			if (self.gameObject.GetComponent<NukeModifer>() == null)
            {
				//BotsModule.Log("norm (1)");
				orig(self, sourceNormal, ignoreDamageCaps, cd);
			}
			else if (self.projectile && self.projectile.Owner && self.projectile.Owner is PlayerController)
            {
				BotsModule.Log("nuke");
				self.gameObject.GetComponent<NukeModifer>().DoNuke(self.projectile.Owner as PlayerController, self.projectile.sprite.WorldCenter);
			}
			else
            {
				//BotsModule.Log("norm (2)");
				orig(self, sourceNormal, ignoreDamageCaps, cd);
			}
		}

		public static void AmmoPickupHook(Action<AmmoPickup, PlayerController> orig, AmmoPickup self, PlayerController player)
		{
			orig(self, player);
			if (self.AppliesCustomAmmunition)
            {
				player.CurrentGun.RegisterNewCustomAmmunition(new ActiveAmmunitionData
				{
					DamageModifier = self.CustomAmmunitionDamageModifier,
					RangeModifier = self.CustomAmmunitionRangeModifier,
					SpeedModifier = self.CustomAmmunitionSpeedModifier,
					ShotsRemaining = player.CurrentGun.ClipCapacity,
				});

			}
		}


		public delegate void ActionC<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
		private static void BuildShadowIndexHook(ActionC<TK2DDungeonAssembler, CellData, Dungeon, tk2dTileMap, int, int> orig, TK2DDungeonAssembler self, CellData current, Dungeon d, tk2dTileMap map, int ix, int iy)
		{
			if (d.tileIndices.tilesetId != (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND && current.cellVisualData.roomVisualTypeIndex == 0)
			{
				orig(self, current, d, map, ix, iy);
            }
		}

		private static void SpawnClipAtPositionHook(Action<Gun, Vector3> orig, Gun self, Vector3 position)
		{


			if (self.PickupObjectId == BotsItemIds.BeyondChargeGun)
            {



				
				if (self.clipObject != null)
				{
					
					
					var dir = self.CurrentAngle < 0 ? self.CurrentAngle - 25 : -self.CurrentAngle + 25;

					Vector3 vector = Quaternion.Euler(0f, 0f, dir) * (self.transform.PositionVector2().normalized * 2).ToVector3ZUp(2);
					GameObject gameObject = SpawnManager.SpawnDebris(self.clipObject, position.WithZ(-0.05f), Quaternion.Euler(0f, 0f, dir));
					tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
					if (self.sprite.attachParent != null && component != null)
					{
						component.attachParent = self.sprite.attachParent;
						component.HeightOffGround = self.sprite.HeightOffGround;
					}
					DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
					vector = Vector3.Scale(vector, Vector3.one) * 0.7f;
					component2.Trigger(vector, 0.5f, 3);
				}
			}
			else
            {
				orig(self, position);
			}
		}

		private static void UpdateGunSpriteHook(Action<GameUIAmmoController, Gun, int, Gun> orig, GameUIAmmoController self, Gun newGun, int change, Gun secondaryGun = null)
		{
			orig(self, newGun, change, secondaryGun);
			if (newGun.gameObject.GetComponent<OverheatGunBehaviour>() != null)
			{

				var heat = newGun.gameObject.GetComponent<OverheatGunBehaviour>().heatLevel * 3;
				var maxHeat = newGun.gameObject.GetComponent<OverheatGunBehaviour>().GetModMaxOverheat(newGun.CurrentOwner) * 3;

				self.GunCooldownForegroundSprite.RelativePosition = self.GunBoxSprite.RelativePosition;
				self.GunCooldownFillSprite.RelativePosition = self.GunBoxSprite.RelativePosition + new Vector3(123f, 3f, 0f);
				self.GunCooldownFillSprite.ZOrder = self.GunBoxSprite.ZOrder + 1;
				self.GunCooldownForegroundSprite.ZOrder = self.GunCooldownFillSprite.ZOrder + 1;
				self.GunCooldownFillSprite.IsVisible = true;
				self.GunCooldownForegroundSprite.IsVisible = true;
				self.GunCooldownFillSprite.FillAmount = heat / maxHeat;
			}
		}

		public static void UpdateUIGunHook(Action<GameUIAmmoController, GunInventory, int> orig, GameUIAmmoController self, GunInventory guns, int inventoryShift)
		{
			orig(self, guns, inventoryShift);
			Gun currentGun = guns.CurrentGun;

			if (currentGun == null || GameUIRoot.Instance.ForceHideGunPanel || self.temporarilyPreventVisible || self.forceInvisiblePermanent)
			{
				self.ToggleRenderers(false);
				return;
			}

			if (currentGun.gameObject.GetComponent<OverheatGunBehaviour>() != null)
			{
				FieldInfo _cachedMaxAmmo = typeof(GameUIAmmoController).GetField("m_cachedMaxAmmo", BindingFlags.NonPublic | BindingFlags.Instance);

				FieldInfo _bgSpritesForModules = typeof(GameUIAmmoController).GetField("bgSpritesForModules", BindingFlags.NonPublic | BindingFlags.Instance);
				FieldInfo _fgSpritesForModules = typeof(GameUIAmmoController).GetField("fgSpritesForModules", BindingFlags.NonPublic | BindingFlags.Instance);

				FieldInfo _topCapsForModules = typeof(GameUIAmmoController).GetField("topCapsForModules", BindingFlags.NonPublic | BindingFlags.Instance);
				FieldInfo _bottomCapsForModules = typeof(GameUIAmmoController).GetField("bottomCapsForModules", BindingFlags.NonPublic | BindingFlags.Instance);
				
					
				var heat = Mathf.FloorToInt(currentGun.gameObject.GetComponent<OverheatGunBehaviour>().heatLevel);
				var maxHeat = Mathf.FloorToInt(currentGun.gameObject.GetComponent<OverheatGunBehaviour>().GetModMaxOverheat(currentGun.CurrentOwner));
				self.GunAmmoCountLabel.Text = $"{heat}/{maxHeat}";

				for (int l = 0; l < (_bgSpritesForModules.GetValue(self) as List<dfTiledSprite>).Count; l++)
				{
					(_fgSpritesForModules.GetValue(self) as List<dfTiledSprite>)[l].IsVisible = false;
					(_bgSpritesForModules.GetValue(self) as List<dfTiledSprite>)[l].IsVisible = false;
				}
				for (int m = 0; m < (_topCapsForModules.GetValue(self) as List<dfSprite>).Count; m++)
				{
					(_topCapsForModules.GetValue(self) as List<dfSprite>)[m].IsVisible = false;
					(_bottomCapsForModules.GetValue(self) as List<dfSprite>)[m].IsVisible = false;
				}
			}
		}

		/*
		public override void EffectTick(GameActorFireEffect self, GameActor actor, RuntimeGameActorEffectData effectData)
		{
			self.EffectTick(actor, effectData);
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.specRigidbody.HitboxPixelCollider != null)
			{
				Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
				self.m_emberCounter += 30f * BraveTime.DeltaTime;
				if (self.m_emberCounter > 1f)
				{
					int num = Mathf.FloorToInt(self.m_emberCounter);
					self.m_emberCounter -= (float)num;
					GlobalSparksDoer.DoRandomParticleBurst(num, unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
				}
			}
			if (actor && actor.specRigidbody)
			{
				Vector2 unitDimensions = actor.specRigidbody.HitboxPixelCollider.UnitDimensions;
				Vector2 a = unitDimensions / 2f;
				int num2 = Mathf.RoundToInt((float)self.flameNumPerSquareUnit * 0.5f * Mathf.Min(30f, Mathf.Min(new float[]
				{
				unitDimensions.x * unitDimensions.y
				})));
				self.m_particleTimer += BraveTime.DeltaTime * (float)num2;
				if (self.m_particleTimer > 1f)
				{
					int num3 = Mathf.FloorToInt(self.m_particleTimer);
					Vector2 vector = actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
					Vector2 vector2 = actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
					PixelCollider pixelCollider = actor.specRigidbody.GetPixelCollider(ColliderType.Ground);
					if (pixelCollider != null && pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
					{
						vector = Vector2.Min(vector, pixelCollider.UnitBottomLeft);
						vector2 = Vector2.Max(vector2, pixelCollider.UnitTopRight);
					}
					vector += Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2 -= Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2.y -= Mathf.Min(a.y * 0.1f, 0.1f);
					GlobalSparksDoer.DoRandomParticleBurst(num3, vector, vector2, Vector3.zero, 0f, 0f, null, null, null, (!self.IsGreenFire) ? GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE : GlobalSparksDoer.SparksType.STRAIGHT_UP_GREEN_FIRE);
					self.m_particleTimer -= Mathf.Floor(self.m_particleTimer);
				}
			}
			if (actor.IsGone)
			{
				effectData.elapsed = 10000f;
			}
			if ((actor.IsFalling || actor.IsGone) && effectData.vfxObjects != null && effectData.vfxObjects.Count > 0)
			{
				GameActorFireEffect.DestroyFlames(effectData);
			}
		}
		*/




		public static Gun.AttackResult AttackHook(Func<Gun, ProjectileData, GameObject, Gun.AttackResult> orig, Gun self, ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null)
		{

			if (self.gameObject.GetComponent<OverheatGunBehaviour>() != null && self.gameObject.GetComponent<OverheatGunBehaviour>().overheated)
            {
				return Gun.AttackResult.Fail;
			}

			return orig(self, overrideProjectileData, overrideBulletObject);
		}

		public static void RegisterConnectedRoomHook(RoomHandler self, RoomHandler other, RuntimeRoomExitData usedExit)
		{
			usedExit.oneWayDoor = true;
			usedExit.isLockedDoor = false;
			self.area.instanceUsedExits.Add(usedExit.referencedExit);
			self.area.exitToLocalDataMap.Add(usedExit.referencedExit, usedExit);
			self.connectedRooms.Add(other);
			self.connectedRoomsByExit.Add(usedExit.referencedExit, other);
		}

		private static bool CheckSourceInKnockbacksHook(Func<KnockbackDoer, GameObject, bool> orig, KnockbackDoer self, GameObject source)
		{
			
			FieldInfo _activeKnockbacks = typeof(KnockbackDoer).GetField("m_activeKnockbacks", BindingFlags.NonPublic | BindingFlags.Instance);

			if (source == null)
			{
				return false;
			}

			
			if ((_activeKnockbacks.GetValue(self) as List<ActiveKnockbackData>) != null)
			{
				List<ActiveKnockbackData> ok = new List<ActiveKnockbackData>();
				foreach (var a in (_activeKnockbacks.GetValue(self) as List<ActiveKnockbackData>))
				{
					if (a == null || a.sourceObject == null)
					{
						ok.Add(a);
						//BotsModule.Log($"_activeKnockbacks {a} nulled", "#91ff00");
					}
					else if (a.sourceObject == source)
					{
						ok.Add(a);
					}
				}
				foreach (var a in ok)
				{
					(_activeKnockbacks.GetValue(self) as List<ActiveKnockbackData>).Remove(a);
				}
				ok.Clear();
			}
			return orig(self, source);
		}


		public static void BraveLogHook(string s, Color c, BraveUtility.LogVerbosity v = BraveUtility.LogVerbosity.VERBOSE)
		{
			BotsModule.Log("Brave Log: " + s);
			BotsModule.Log("Brave Log: " + s, ColorUtility.ToHtmlStringRGB(c));
		}

		public static float LocalTimeScaleHook(Func<Projectile, float> orig, Projectile self)
		{
			if (self.gameObject?.GetComponent<DarkArtsSlowDown>()?.overrideTimeScale != 1)
			{
				return self.gameObject.GetComponent<DarkArtsSlowDown>().overrideTimeScale;
			}
			return orig(self);
		}

		private static void ProcessHeartSpriteModificationsHook(Action<GameUIHeartController, PlayerController> orig, GameUIHeartController self, PlayerController associatedPlayer)
		{
			FieldInfo _currentHalfHeartName = typeof(GameUIHeartController).GetField("m_currentHalfHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentEmptyHeartName = typeof(GameUIHeartController).GetField("m_currentEmptyHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentFullHeartName = typeof(GameUIHeartController).GetField("m_currentFullHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentArmorName = typeof(GameUIHeartController).GetField("m_currentArmorName", BindingFlags.NonPublic | BindingFlags.Instance);

			orig(self, associatedPlayer);

			if (associatedPlayer?.name == "PlayerShade(Clone)" && (string)_currentArmorName.GetValue(self) == self.armorSpritePrefab.SpriteName)
			{
				//_currentHalfHeartName.SetValue(self, "");
				//_currentEmptyHeartName.SetValue(self, "");
				//_currentFullHeartName.SetValue(self, "");
				_currentArmorName.SetValue(self, "shade_armour");
			}

		}

		private static void TheresNoFuckingWayThisWorks(Action<Projectile, CollisionData> orig, Projectile self, CollisionData rigidbodyCollision)
		{
			if (self.Owner && self.Owner is PlayerController && PassiveItem.IsFlagSetAtAll(typeof(FracturedRounds)) == true)
            {
				for (int i = 0; i < 2; i++)
				{
					orig(self, rigidbodyCollision);
				}
			} 
			else
            {
				orig(self, rigidbodyCollision);
			}
			
		}
		private static int GetIndexFromTupleArrayHook(TK2DDungeonAssembler self, CellData current, List<Tuple<int, TilesetIndexMetadata>> list, int roomTypeIndex)
		{
			float uniqueHash = current.UniqueHash;
			float num = 0f;
			if (current == null)
            {
				Debug.Log("ah fuck");
			}
			if (list == null)
			{
				Debug.Log("god fucking damn it");
			}

			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null)
				{
					Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				}
				if (list[i].Second.dungeonRoomSubType == roomTypeIndex || list[i].Second.secondRoomSubType == roomTypeIndex || list[i].Second.thirdRoomSubType == roomTypeIndex)
				{

					num += list[i].Second.weight;
				}
			}
			
			float num2 = uniqueHash * num;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Second.dungeonRoomSubType == roomTypeIndex || list[j].Second.secondRoomSubType == roomTypeIndex || list[j].Second.thirdRoomSubType == roomTypeIndex)
				{
					num2 -= list[j].Second.weight;
					if (num2 <= 0f)
					{
						return list[j].First;
					}
				}
			}

			return list[0].First;
		}


		public static void DodgeRollPleaseJustNullCheckShitIBegYou(Action<Gun, List<SpeculativeRigidbody>, Vector2, int> orig, Gun self, List<SpeculativeRigidbody> alreadyHit, Vector2 arcOrigin, int slashId)
		{
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = allProjectiles.Count - 1; i >= 0; i--)
			{
				Projectile projectile = allProjectiles[i];
				if(projectile != null && projectile.sprite == null)
                {
					BotsModule.Log(projectile.name);
					StaticReferenceManager.RemoveProjectile(projectile);

				}
			}
			orig(self, alreadyHit, arcOrigin, slashId);
		}


		public static void UpdateAnimationsHook(Action<Gun, tk2dSpriteCollectionData> orig, Gun gun, tk2dSpriteCollectionData collection = null)
		{
			orig(gun, collection);

			gun.dodgeAnimation = gun.UpdateAnimation("dodge", collection, true);
			gun.alternateIdleAnimation = gun.UpdateAnimation("alternate_idle", collection, false);
		}

		private static void InitializeInternalHook(Action<ShopItemController, PickupObject> orig, ShopItemController self, PickupObject i)
		{
			orig(self, i);
			FieldInfo _baseParentShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
			if ((_baseParentShop.GetValue(self) as BaseShopController) != null && (_baseParentShop.GetValue(self) as BaseShopController).baseShopType == (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL)
			{
				self.CurrentPrice = 1;
				if (self.item.quality == PickupObject.ItemQuality.A || self.item.quality == PickupObject.ItemQuality.S)
				{
					self.CurrentPrice = 2;
				}
			}
		}

		private static void UpdateAnimationNamesselfdOnSacksHook(Action<SackKnightController> orig, SackKnightController self)
		{
			orig(self);
			FieldInfo _owner = typeof(SackKnightController).GetField("m_owner", BindingFlags.NonPublic | BindingFlags.Instance);
			if (_owner.GetValue(self) as PlayerController)
			{
				bool flag2 = false;
				for (int i = 0; i < (_owner.GetValue(self) as PlayerController).passiveItems.Count; i++)
				{
					if ((_owner.GetValue(self) as PlayerController).passiveItems[i] is BasicStatPickup)
					{
						BasicStatPickup basicStatPickup = (_owner.GetValue(self) as PlayerController).passiveItems[i] as BasicStatPickup;
						if (basicStatPickup.IsJunk && basicStatPickup.PickupObjectId == GlobalItemIds.JunkTruth)
						{
							flag2 = true;
						}
					}
				}
				AIAnimator aiAnimator = self.aiAnimator;
				if (flag2)
				{
					self.CurrentForm = (SackKnightController.SackKnightPhase)CustomEnums.CustomSackKnightPhase.BOB_FROM_HR;
					aiAnimator.IdleAnimation.AnimNames[0] = "junk_h_idle_right";
					aiAnimator.IdleAnimation.AnimNames[1] = "junk_g_idle_left";
					aiAnimator.MoveAnimation.AnimNames[0] = "junk_sh_move_right";
					aiAnimator.MoveAnimation.AnimNames[1] = "junk_shs_move_left";
					aiAnimator.TalkAnimation.AnimNames[0] = "junk_shsp_talk_right";
					aiAnimator.TalkAnimation.AnimNames[1] = "junk_shspc_talk_left";
					aiAnimator.OtherAnimations[0].anim.AnimNames[0] = "junk_shspcg_attack_right";
					aiAnimator.OtherAnimations[0].anim.AnimNames[1] = "junk_a_attack_left";
				}							
			}
		}


		public static void StupidFuckingHook(DebrisObject self, Vector3 startingForce, float startingHeight, float angularVelocityModifier = 1f)
		{
			FieldInfo _hasBeenTriggered = typeof(DebrisObject).GetField("m_hasBeenTriggered", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _transform = typeof(DebrisObject).GetField("m_transform", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _renderer = typeof(DebrisObject).GetField("m_renderer", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _initialWorldDepth = typeof(DebrisObject).GetField("m_initialWorldDepth", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _startingHeightOffGround = typeof(DebrisObject).GetField("m_startingHeightOffGround", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _startPosition = typeof(DebrisObject).GetField("m_startPosition", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentPosition = typeof(DebrisObject).GetField("m_currentPosition", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _velocity = typeof(DebrisObject).GetField("m_velocity", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentLifespan = typeof(DebrisObject).GetField("m_currentLifespan", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo isStatic = typeof(DebrisObject).GetField("isStatic", BindingFlags.NonPublic | BindingFlags.Instance);


			if ((bool)_hasBeenTriggered.GetValue(self))
			{
				return;
			}
			if (self.specRigidbody != null && self.specRigidbody.enabled)
			{
				self.shouldUseSRBMotion = true;
				if (self.specRigidbody.PrimaryPixelCollider.CollisionLayer == CollisionLayer.BulletBlocker || self.specRigidbody.PrimaryPixelCollider.CollisionLayer == CollisionLayer.BulletBreakable)
				{
					if (self.GetComponent<CoinController>() == null)
                    {
						self.specRigidbody.CollideWithOthers = false;
					}
					
				}
			}
			else if (self.specRigidbody == null)
			{
				self.shouldUseSRBMotion = false;
			}
			if (self.groupManager != null)
			{
				self.groupManager.DeregisterDebris(self);
			}
			_transform.SetValue(self, self.transform);
			_renderer.SetValue(self, self.renderer);
			if (self.sprite == null)
			{
				self.sprite = self.GetComponentInChildren<tk2dSprite>();
			}
			_initialWorldDepth.SetValue(self, self.sprite.HeightOffGround);
			_startingHeightOffGround.SetValue(self, startingHeight);
			Vector2 vector = (_transform.GetValue(self) as Transform).position.XY();
			_startPosition.SetValue(self, new Vector3(vector.x, vector.y - startingHeight, startingHeight));
			_currentPosition.SetValue(self, (Vector3)_startPosition.GetValue(self));
			_velocity.SetValue(self, startingForce / self.inertialMass);
			if (self.usesLifespan)
			{
				_currentLifespan.SetValue(self, UnityEngine.Random.Range(self.lifespanMin, self.lifespanMax));
			}
			self.angularVelocity = (self.canRotate ? (self.angularVelocity + UnityEngine.Random.Range(-self.angularVelocityVariance, self.angularVelocityVariance)) : 0f);
			self.angularVelocity *= angularVelocityModifier;
			_hasBeenTriggered.SetValue(self, true);
			isStatic.SetValue(self, false);
			if (self.followupBehavior == DebrisObject.DebrisFollowupAction.FollowupAnimation && !string.IsNullOrEmpty(self.followupIdentifier))
			{
				tk2dSpriteAnimator spriteAnimator = self.spriteAnimator;
				spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(self.OnAnimationCompleted));
				self.spriteAnimator.Play();
			}
			else if (self.playAnimationOnTrigger)
			{
				if (self.usesDirectionalFallAnimations)
				{
					self.spriteAnimator.Play(self.directionalAnimationData.GetAnimationForVector(startingForce.XY()));
				}
				else
				{
					self.spriteAnimator.Play();
				}
			}
			if (self.OnTriggered != null)
			{
				self.OnTriggered();
			}
		}


		private static void StartHookSC(Action<ShortcutElevatorController> orig, ShortcutElevatorController self)
		{

			List<ShortcutDefinition> shortCuts = self.definedShortcuts.ToList();
			SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/elevator_bottom_floor_beyond.png", self.elevatorFloorSprite.Collection, "elevator_bottom_floor_beyond");
			shortCuts.Add(new ShortcutDefinition
			{
				elevatorFloorSpriteName = "elevator_bottom_floor_beyond",
				IsBossRush = false,
				IsSuperBossRush = false,
				requiredFlag = GungeonFlags.NONE,
				sherpaTextKey = "The Beyond",
				targetLevelName = BeyondDungeon.BeyondDefinition.dungeonSceneName,
			});

			self.definedShortcuts = shortCuts.ToArray();

			orig(self);
			
		}

		public static void TriggerSilencerHook(CoolerAction<SilencerInstance, Vector2, float, float, GameObject, float, float, float, float, float, float, float, PlayerController, bool, bool> orig, SilencerInstance self, Vector2 centerPoint, float expandSpeed, float maxRadius,
			GameObject silencerVFX, float distIntensity, float distRadius, float pushForce, float pushRadius, float knockbackForce, float knockbackRadius, float additionalTimeAtMaxRadius, PlayerController user, bool breaksWalls = true, bool skipBreakables = false)
		{
			BotsMod.BotsModule.Log("blank triggered");
			if (user != null && user.HasPickupID(BotsItemIds.VoidAmmolet))
			{
				BotsMod.BotsModule.Log("spawning shards");

				if (breaksWalls)
				{
					RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(centerPoint.ToIntVector2(VectorConversions.Floor));
					for (int j = 0; j < StaticReferenceManager.AllMajorBreakables.Count; j++)
					{
						if (StaticReferenceManager.AllMajorBreakables[j].IsSecretDoor)
						{
							RoomHandler absoluteRoomFromPosition2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(StaticReferenceManager.AllMajorBreakables[j].transform.position.IntXY(VectorConversions.Floor));
							if (absoluteRoomFromPosition2 == absoluteRoomFromPosition)
							{
								StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
								StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
								StaticReferenceManager.AllMajorBreakables[j].ApplyDamage(1E+10f, Vector2.zero, false, true, true);
							}
						}
					}
				}

				if (silencerVFX != null)
				{
					GameObject obj = UnityEngine.Object.Instantiate<GameObject>(silencerVFX, centerPoint.ToVector3ZUp(centerPoint.y), Quaternion.identity);
					UnityEngine.Object.Destroy(obj, 1f);
				}
				VoidAmmolet.VoidBlank(user, centerPoint.ToVector3ZUp(centerPoint.y), (int)maxRadius, self.ForceNoDamage);
			}
			else
			{
				orig(self, centerPoint, expandSpeed, maxRadius, silencerVFX, distIntensity, distRadius, pushForce, pushRadius, knockbackForce, knockbackRadius, additionalTimeAtMaxRadius, user, breaksWalls, skipBreakables);
			}
			
		}

		#region custom ui hooks (mostly broken)
		//public static dfLabel p_playerArmourLabel;
		//public static dfSprite p_playerArmourSprite;

		public static void UpdateBlanksHook(Action<GameUIBlankController, int> orig, GameUIBlankController self, int numBlanks)
		{
			orig(self, numBlanks);

			foreach(var blank in self.extantBlanks)
            {
				if(GameManager.Instance?.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.HasPassiveItem(BotsItemIds.VoidAmmolet))
                {
					blank.SpriteName = "void_blank";
				} 
				else
                {
					blank.SpriteName = "ui_blank";
				}
				//BotsModule.Log(blank.SpriteName);
            }
		}

		public static void UpdatePlayerConsumablesHook(GameUIRoot self, PlayerConsumables playerConsumables)
		{
			if (BotsModule.debugMode) ETGModConsole.Log("0");
			FieldInfo _playerCoinSprite = typeof(GameUIRoot).GetField("p_playerCoinSprite", BindingFlags.NonPublic | BindingFlags.Instance);
			//GameObject pannel = null;
			if (UiTesting.p_playerArmourLabel == null)
			{
				/*UiTesting.pannel = PrefabAPI.PrefabBuilder.Clone(self.p_playerKeyLabel.transform.parent.gameObject);
				p_playerArmourLabel = pannel.transform.Find("KeyLabel").gameObject.GetComponent<dfLabel>();
				p_playerArmourLabel.name = "ArmourLabel";
				pannel.name = "ArmourPannel";
				p_playerArmourLabel.transform.parent = pannel.transform;
				if (!p_playerArmourLabel.gameObject.activeSelf)
				{
					p_playerArmourLabel.gameObject.SetActive(true);
					ETGModConsole.Log("activated p_playerArmourLabel");

				}*/
				ETGModConsole.Log(self.p_playerKeyLabel.transform.parent.ToString());
				ETGModConsole.Log("was null... shit");
			}
			if (UiTesting.pannel == null)
            {
				UiTesting.pannel = UiTesting.p_playerArmourLabel.transform.parent.gameObject;

			}

			if (UiTesting.pannel?.transform.parent != null && UiTesting.pannel?.transform.parent != self.transform)
            {
				UiTesting.pannel.transform.parent = self.transform;

			}

			if (BotsModule.debugMode) ETGModConsole.Log("1");
			if (UiTesting.p_playerArmourSprite == null)
			{
				ETGModConsole.Log("was null... shit 2");
			}
			if (BotsModule.debugMode) ETGModConsole.Log("2");
			self.p_playerCoinLabel.Text = IntToStringSansGarbage.GetStringForInt(playerConsumables.Currency);
			self.p_playerKeyLabel.Text = IntToStringSansGarbage.GetStringForInt(playerConsumables.KeyBullets);
			UiTesting.p_playerArmourLabel.Text = IntToStringSansGarbage.GetStringForInt(10);
			if (BotsModule.debugMode) ETGModConsole.Log("3");

			typeof(GameUIRoot).GetMethod("UpdateSpecialKeys", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { playerConsumables });
			if (BotsModule.debugMode) ETGModConsole.Log("3.5");

			if(UiTesting.p_playerArmourLabel == null)
            {
				ETGModConsole.Log("p_playerArmourLabel");
			}
			if (UiTesting.p_playerArmourLabel.Parent == null)
			{
				ETGModConsole.Log("p_playerArmourLabel.Parent");
			}
			
			if (UiTesting.p_playerArmourLabel.Parent.Parent == null)
			{
				ETGModConsole.Log("p_playerArmourLabel.Parent.Parent");
				
			}
			if (BotsModule.debugMode) ETGModConsole.Log("4.05");
			if (GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.Blanks == 0)
			{
				if (BotsModule.debugMode) ETGModConsole.Log("4.15");
				//ETGModConsole.Log(self.p_playerCoinLabel.Parent.Parent.ToString());
				self.p_playerCoinLabel.Parent.Parent.RelativePosition = self.p_playerCoinLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y);
				if (BotsModule.debugMode) ETGModConsole.Log("4.1");
				self.p_playerKeyLabel.Parent.Parent.RelativePosition = self.p_playerKeyLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y);
				if (BotsModule.debugMode) ETGModConsole.Log("4.2");
				UiTesting.p_playerArmourLabel.Parent.Parent.RelativePosition = UiTesting.p_playerArmourLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y) + new Vector3(10, 0, 0);
				if (BotsModule.debugMode) ETGModConsole.Log("4.3");
			}
			else
			{
				if (BotsModule.debugMode) ETGModConsole.Log("4.35");
				//ETGModConsole.Log(self.p_playerCoinLabel.Parent.Parent.ToString());
				if (self.p_playerCoinLabel.Parent.Parent == null)
                {
					ETGModConsole.Log("AAAAAAAAAAAAAAAAA");
				}

				self.p_playerCoinLabel.Parent.Parent.RelativePosition = self.p_playerCoinLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y + self.blankControllers[0].Panel.Height - 9f);
				if (BotsModule.debugMode) ETGModConsole.Log("4.4");
				self.p_playerKeyLabel.Parent.Parent.RelativePosition = self.p_playerKeyLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y + self.blankControllers[0].Panel.Height - 9f);
				if (BotsModule.debugMode) ETGModConsole.Log("4.5");
				UiTesting.p_playerArmourLabel.Parent.Parent.RelativePosition = UiTesting.p_playerArmourLabel.Parent.Parent.RelativePosition.WithY(self.blankControllers[0].Panel.RelativePosition.y + self.blankControllers[0].Panel.Height - 9f)   + new Vector3(10, 0, 0);
				if (BotsModule.debugMode) ETGModConsole.Log("4.6");
			}
			if (BotsModule.debugMode) ETGModConsole.Log("4");
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY));
				if (num > 0)
				{
					self.p_playerCoinLabel.Text = IntToStringSansGarbage.GetStringForInt(num);
					if ((_playerCoinSprite.GetValue(self) as dfSprite) == null)
					{
						_playerCoinSprite.SetValue(self, self.p_playerCoinLabel.Parent.GetComponentInChildren<dfSprite>());
					}
					(_playerCoinSprite.GetValue(self) as dfSprite).SpriteName = "hbux_text_icon";
					(_playerCoinSprite.GetValue(self) as dfSprite).Size = (_playerCoinSprite.GetValue(self) as dfSprite).SpriteInfo.sizeInPixels * 3f;
				}
				else
				{
					if ((_playerCoinSprite.GetValue(self) as dfSprite) == null)
					{
						_playerCoinSprite.SetValue(self, self.p_playerCoinLabel.Parent.GetComponentInChildren<dfSprite>());
					}
					self.p_playerCoinLabel.IsVisible = false;
					(_playerCoinSprite.GetValue(self) as dfSprite).IsVisible = false;
				}
			}
			if (BotsModule.debugMode) ETGModConsole.Log("5");
		}


		public static bool IsValidHook(Func<GunFormeData, PlayerController, bool> orig, GunFormeData self, PlayerController p)
		{

			BotsModule.Log(self.GetType().ToString());

			if (self as CustomGunFormeData != null && self.RequiresSynergy)
            {
				BotsModule.Log(p.PlayerHasActiveSynergy((self as CustomGunFormeData).RequiredSynergyName).ToString());
				return p.PlayerHasActiveSynergy((self as CustomGunFormeData).RequiredSynergyName);
			}

			return orig(self, p);
		}

		public delegate TResult FuncC<T, T2, T3, T4, T5, TResult>(T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
		private static string GetBaseAnimationNameHook(FuncC<PlayerController, Vector2, float, bool, bool, string> orig, PlayerController self, Vector2 v, float gunAngle, bool invertThresholds = false, bool forceTwoHands = false)
        {
			if (self.characterIdentity == (PlayableCharacters)274131)
            {

            

			

				string text = string.Empty;
				bool flag = self.CurrentGun != null;
				if (flag && self.CurrentGun.Handedness == GunHandedness.NoHanded)
				{
					forceTwoHands = true;
				}
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
				{
					flag = false;
				}
				float num = 155f;
				float num2 = 25f;
				if (invertThresholds)
				{
					num = -155f;
					num2 -= 50f;
				}
				float num3 = 120f;
				float num4 = 60f;
				float num5 = -60f;
				float num6 = -120f;
				bool flag2 = gunAngle <= num && gunAngle >= num2;
				if (invertThresholds)
				{
					flag2 = (gunAngle <= num || gunAngle >= num2);
				}

				var renderBodyHand = !self.ForceHandless && self.CurrentSecondaryGun == null && (self.CurrentGun == null || self.CurrentGun.Handedness != GunHandedness.TwoHanded);



				if (!self.IsGhost && self.IsFlying && !self.IsPetting && (v == Vector2.zero || self.IsStationary))
				{
					if (flag2)
					{
						if (gunAngle < num3 && gunAngle >= num4)
						{
							string text2 = ((!forceTwoHands && flag) || self.ForceHandless) ? ((!renderBodyHand) ? "idle_backward" : "idle_backward_hand") : "idle_backward_twohands";
							text = text2;
						}
						else
						{
							string text3 = ((!forceTwoHands && flag) || self.ForceHandless) ? "idle_bw" : "idle_bw_twohands";
							text = text3;
						}
					}
					else if (gunAngle <= num5 && gunAngle >= num6)
					{
						string text4 = ((!forceTwoHands && flag) || self.ForceHandless) ? ((!renderBodyHand) ? "idle_forward" : "idle_forward_hand") : "idle_forward_twohands";
						text = text4;
					}
					else
					{
						string text5 = ((!forceTwoHands && flag) || self.ForceHandless) ? ((!renderBodyHand) ? "idle" : "idle_hand") : "idle_twohands";
						text = text5;
					}
				}
				else if (flag2 && !self.IsGhost && self.IsFlying)
				{
					string text6 = ((!forceTwoHands && flag) || self.ForceHandless) ? "run_right_bw" : "run_right_bw_twohands";
					if (gunAngle < num3 && gunAngle >= num4)
					{
						text6 = (((!forceTwoHands && flag) || self.ForceHandless) ? ((!renderBodyHand) ? "run_up" : "run_up_hand") : "run_up_twohands");
					}
					text = text6;
				}
				else if (!self.IsGhost && self.IsFlying)
				{
					string text7 = "run_right";
					if (gunAngle <= num5 && gunAngle >= num6)
					{
						text7 = "run_down";
					}
					if ((forceTwoHands || !flag) && !self.ForceHandless)
					{
						text7 += "_twohands";
					}
					else if (renderBodyHand)
					{
						text7 += "_hand";
					}
					text = text7;
				}

				return string.IsNullOrEmpty(text) ? orig(self, v, gunAngle, invertThresholds, forceTwoHands) : text;
			}
			return orig(self, v, gunAngle, invertThresholds, forceTwoHands);
		}
		

		public static void ApplyDamageDirectionalHook(ActionC<HealthHaver, float, Vector2, string, CoreDamageTypes, DamageCategory, bool, PixelCollider, bool> orig, HealthHaver self, float damage, Vector2 direction, string damageSource, CoreDamageTypes damageTypes, DamageCategory damageCategory = DamageCategory.Normal, bool ignoreInvulnerabilityFrames = false, PixelCollider hitPixelCollider = null, bool ignoreDamageCaps = false)
		{

			FieldInfo isPlayerCharacter = typeof(HealthHaver).GetField("isPlayerCharacter", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _player = typeof(HealthHaver).GetField("m_player", BindingFlags.NonPublic | BindingFlags.Instance);

			if (self.Armor <= 0)
			{
				OtherworldlyConnections.gotHitThisFloor = true;
			}

			if (!self.NextDamageIgnoresArmor && !self.NextShotKills)
			{
				if (self.Armor <= 0f && SoulHeartController.soulHeartCount > 0 && (bool)isPlayerCharacter.GetValue(self))
				{
					SoulHeartController.soulHeartCount -= 1f;
					damage = 0.00000000000000000000000000000000000000000001f;
					if ((bool)isPlayerCharacter.GetValue(self))
					{
						SoulHeartController.OnSoulHeartLost(_player.GetValue(self) as PlayerController);
					}
				}
			}

			orig(self, damage, direction, damageSource, damageTypes, damageCategory, ignoreInvulnerabilityFrames, hitPixelCollider, ignoreDamageCaps);
		}


		
		public static void UpdateHealthHook(GameUIHeartController self, HealthHaver hh)
		{

			FieldInfo _currentHalfHeartName = typeof(GameUIHeartController).GetField("m_currentHalfHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentEmptyHeartName = typeof(GameUIHeartController).GetField("m_currentEmptyHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentFullHeartName = typeof(GameUIHeartController).GetField("m_currentFullHeartName", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentArmorName = typeof(GameUIHeartController).GetField("m_currentArmorName", BindingFlags.NonPublic | BindingFlags.Instance);

			float num = hh.GetCurrentHealth();
			float maxHealth = hh.GetMaxHealth();

			float num2 = hh.Armor;
			if (hh.NextShotKills)
			{
				num = 0.5f;
				num2 = 0f;
			}
			int num3 = Mathf.CeilToInt(maxHealth);
			int num4 = Mathf.CeilToInt(num2);
			if (self.extantHearts.Count < num3)
			{
				for (int i = self.extantHearts.Count; i < num3; i++)
				{
					dfSprite dfSprite = self.AddHeart();
					if ((float)(i + 1) > num)
					{
						if ((float)Mathf.FloorToInt(num) != num && num + 1f > (float)(i + 1))
						{
							dfSprite.SpriteName = _currentHalfHeartName.GetValue(self) as string;
						}
						else
						{
							dfSprite.SpriteName = _currentEmptyHeartName.GetValue(self) as string;
						}
					}
				}
			}
			else if (self.extantHearts.Count > num3)
			{
				while (self.extantHearts.Count > num3)
				{
					self.RemoveHeart();
				}
			}
			if (self.extantArmors.Count < num4)
			{
				for (int j = self.extantArmors.Count; j < num4; j++)
				{
					self.AddArmor();
				}
			}
			else if (self.extantArmors.Count > num4)
			{
				while (self.extantArmors.Count > num4)
				{
					self.RemoveArmor();
				}
			}

			if (SoulHeartController.extantSoulHearts.Count < SoulHeartController.soulHeartCount)
			{
				for (int k = SoulHeartController.extantSoulHearts.Count; k < SoulHeartController.soulHeartCount; k++)
				{
					SoulHeartController.AddSoulHeart(self);
				}
			}
			else if (SoulHeartController.extantSoulHearts.Count > SoulHeartController.soulHeartCount)
			{
				while (SoulHeartController.extantSoulHearts.Count > SoulHeartController.soulHeartCount)
				{
					SoulHeartController.RemoveSoulHeart(self);
				}
			}

			int num5 = Mathf.FloorToInt(num);
			for (int k = 0; k < self.extantHearts.Count; k++)
			{
				dfSprite dfSprite2 = self.extantHearts[k];
				if (dfSprite2)
				{
					if (k < num5)
					{
						dfSprite2.SpriteName = _currentFullHeartName.GetValue(self) as string;
					}
					else if (k != num5 || num - (float)num5 <= 0f)
					{
						if (dfSprite2.SpriteName == _currentFullHeartName.GetValue(self) as string || dfSprite2.SpriteName == _currentHalfHeartName.GetValue(self) as string)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.damagedHeartAnimationPrefab.gameObject);
							gameObject.transform.parent = self.transform.parent;
							gameObject.layer = self.gameObject.layer;
							dfSprite component = gameObject.GetComponent<dfSprite>();
							component.BringToFront();
							dfSprite2.Parent.AddControl(component);
							dfSprite2.Parent.BringToFront();
							component.ZOrder = dfSprite2.ZOrder - 1;
							component.RelativePosition = dfSprite2.RelativePosition + self.damagedPrefabOffset;
						}
						dfSprite2.SpriteName = _currentEmptyHeartName.GetValue(self) as string;
					}
				}
			}
			if (num - (float)num5 > 0f && self.extantHearts != null && self.extantHearts.Count > 0)
			{
				dfSprite dfSprite3 = self.extantHearts[num5];
				if (dfSprite3)
				{
					if (dfSprite3.SpriteName == _currentFullHeartName.GetValue(self) as string)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(self.damagedHeartAnimationPrefab.gameObject);
						gameObject2.transform.parent = self.transform.parent;
						gameObject2.layer = self.gameObject.layer;
						dfSprite component2 = gameObject2.GetComponent<dfSprite>();
						component2.BringToFront();
						dfSprite3.Parent.AddControl(component2);
						dfSprite3.Parent.BringToFront();
						component2.ZOrder = dfSprite3.ZOrder - 1;
						component2.RelativePosition = dfSprite3.RelativePosition + self.damagedPrefabOffset;
					}
					dfSprite3.SpriteName = _currentHalfHeartName.GetValue(self) as string;
				}
			}
			PlayerController associatedPlayer = hh.gameActor as PlayerController;
			typeof(GameUIHeartController).GetMethod("ProcessHeartSpriteModifications", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { associatedPlayer });
			//self.ProcessHeartSpriteModifications(associatedPlayer);
			if (hh.HasCrest && num2 > 0f)
			{
				for (int l = 0; l < self.extantArmors.Count; l++)
				{
					dfSprite dfSprite4 = self.extantArmors[l];
					if (dfSprite4)
					{
						if (l < self.extantArmors.Count - 1)
						{
							if (dfSprite4.SpriteName != _currentArmorName.GetValue(self) as string)
							{
								dfSprite4.SpriteName = _currentArmorName.GetValue(self) as string;
								dfSprite4.Color = self.armorSpritePrefab.Color;
								dfPanel motionGroupParent = GameUIRoot.Instance.GetMotionGroupParent(dfSprite4);
								motionGroupParent.Width -= Pixelator.Instance.CurrentTileScale;
								motionGroupParent.Height -= Pixelator.Instance.CurrentTileScale;
								dfSprite4.RelativePosition = dfSprite4.RelativePosition.WithY(0f);
							}
						}
						else if (dfSprite4.SpriteName != self.crestSpritePrefab.SpriteName)
						{
							dfSprite4.SpriteName = self.crestSpritePrefab.SpriteName;
							dfSprite4.Color = self.crestSpritePrefab.Color;
							dfPanel motionGroupParent2 = GameUIRoot.Instance.GetMotionGroupParent(dfSprite4);
							motionGroupParent2.Width += Pixelator.Instance.CurrentTileScale;
							motionGroupParent2.Height += Pixelator.Instance.CurrentTileScale;
							dfSprite4.RelativePosition = dfSprite4.RelativePosition.WithY(Pixelator.Instance.CurrentTileScale);
						}
					}
				}
			}
			else
			{
				for (int m = 0; m < self.extantArmors.Count; m++)
				{
					dfSprite dfSprite5 = self.extantArmors[m];
					if (dfSprite5)
					{
						if (dfSprite5.SpriteName != _currentArmorName.GetValue(self) as string)
						{
							dfSprite5.SpriteName = _currentArmorName.GetValue(self) as string;
							dfPanel motionGroupParent3 = GameUIRoot.Instance.GetMotionGroupParent(dfSprite5);
							motionGroupParent3.Width -= Pixelator.Instance.CurrentTileScale;
							motionGroupParent3.Height -= Pixelator.Instance.CurrentTileScale;
							dfSprite5.RelativePosition = dfSprite5.RelativePosition.WithY(0f);
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite5, true, false, true);
							GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite5);
							GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite5, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
						}
						dfSprite5.Color = self.armorSpritePrefab.Color;
						dfSprite5.RelativePosition = dfSprite5.RelativePosition.WithY(0f);
					}
				}
			}
			for (int n = 0; n < self.extantHearts.Count; n++)
			{
				dfSprite dfSprite6 = self.extantHearts[n];
				if (dfSprite6)
				{
					dfSprite6.Size = dfSprite6.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
				}
			}
			for (int num6 = 0; num6 < self.extantArmors.Count; num6++)
			{
				dfSprite dfSprite7 = self.extantArmors[num6];
				if (dfSprite7)
				{
					dfSprite7.Size = dfSprite7.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
				}
			}
			for (int deez = 0; deez < SoulHeartController.extantSoulHearts.Count; deez++)
			{
				dfSprite dfSprite6 = SoulHeartController.extantSoulHearts[deez];
				if (dfSprite6)
				{
					dfSprite6.Size = dfSprite6.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
				}
			}
		}


		public dfSprite AddHeartHook(GameUIHeartController self)
		{
			FieldInfo _panel = typeof(GameUIHeartController).GetField("m_panel", BindingFlags.NonPublic | BindingFlags.Instance);

			int count = self.extantArmors.Count;
			typeof(GameUIHeartController).GetMethod("ClearAllArmor", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			Vector3 position = self.transform.position;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.heartSpritePrefab.gameObject, position, Quaternion.identity);
			gameObject.transform.parent = self.transform.parent;
			gameObject.layer = self.gameObject.layer;
			dfSprite component = gameObject.GetComponent<dfSprite>();
			if (self.IsRightAligned)
			{
				component.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Right);
			}
			Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
			component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			component.IsInteractive = false;
			if (!self.IsRightAligned)
			{
				float x = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)self.extantHearts.Count;
				component.RelativePosition = (_panel.GetValue(self) as dfPanel).RelativePosition + new Vector3(x, 0f, 0f);
			}
			else
			{
				component.RelativePosition = (_panel.GetValue(self) as dfPanel).RelativePosition - new Vector3(component.Width, 0f, 0f);
				for (int i = 0; i < self.extantHearts.Count; i++)
				{
					dfSprite dfSprite = self.extantHearts[i];
					if (dfSprite)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, true, false, true);
						dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
				for (int j = 0; j < self.extantArmors.Count; j++)
				{
					dfSprite dfSprite2 = self.extantArmors[j];
					if (dfSprite2)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
						dfSprite2.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}

				for (int j = 0; j < SoulHeartController.extantSoulHearts.Count; j++)
				{
					dfSprite dfSprite3 = SoulHeartController.extantSoulHearts[j];
					if (dfSprite3)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, true, false, true);
						dfSprite3.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite3);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
			self.extantHearts.Add(component);
			GameUIRoot.Instance.AddControlToMotionGroups(component, (!self.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
			for (int k = 0; k < count; k++)
			{
				self.AddArmor();
			}
			return component;
		}


		public void AddArmorHook(GameUIHeartController self)
		{

			FieldInfo _panel = typeof(GameUIHeartController).GetField("m_panel", BindingFlags.NonPublic | BindingFlags.Instance);

			Vector3 position = self.transform.position;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.armorSpritePrefab.gameObject, position, Quaternion.identity);
			gameObject.transform.parent = self.transform.parent;
			gameObject.layer = self.gameObject.layer;
			dfSprite component = gameObject.GetComponent<dfSprite>();
			if (self.IsRightAligned)
			{
				component.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Right);
			}
			Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
			component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			component.IsInteractive = false;
			if (!self.IsRightAligned)
			{
				float num = (self.extantHearts.Count <= 0) ? 0f : ((self.extantHearts[0].Width + Pixelator.Instance.CurrentTileScale) * (float)self.extantHearts.Count);
				float num2 = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)self.extantArmors.Count;
				component.RelativePosition = (_panel.GetValue(self) as dfPanel).RelativePosition + new Vector3(num + num2, 0f, 0f);
			}
			else
			{
				component.RelativePosition = (_panel.GetValue(self) as dfPanel).RelativePosition - new Vector3(component.Width, 0f, 0f);
				for (int i = 0; i < self.extantArmors.Count; i++)
				{
					dfSprite dfSprite = self.extantArmors[i];
					if (dfSprite)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, true, false, true);
						dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
				for (int j = 0; j < self.extantHearts.Count; j++)
				{
					dfSprite dfSprite2 = self.extantHearts[j];
					if (dfSprite2)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, true, false, true);
						dfSprite2.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite2, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}

				for (int j = 0; j < SoulHeartController.extantSoulHearts.Count; j++)
				{
					dfSprite dfSprite3 = SoulHeartController.extantSoulHearts[j];
					if (dfSprite3)
					{
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, true, false, true);
						dfSprite3.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite3);
						GameUIRoot.Instance.TransitionTargetMotionGroup(dfSprite3, GameUIRoot.Instance.IsCoreUIVisible(), false, true);
					}
				}
			}
			self.extantArmors.Add(component);
			GameUIRoot.Instance.AddControlToMotionGroups(component, (!self.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
		}

		private static void HandleMotionHook(Action<SuperReaperController> orig, SuperReaperController self)
		{
			self.specRigidbody.Velocity = Vector2.zero;
			if (self.aiAnimator.IsPlaying("attack"))
			{
				return;
			}
			orig(self);

		}

		private static void SpawnProjectilesHook(SuperReaperController self)
		{

			FieldInfo _bulletSource = typeof(SuperReaperController).GetField("m_bulletSource", BindingFlags.NonPublic | BindingFlags.Instance);

			if (GameManager.Instance.PreventPausing || BossKillCam.BossDeathCamRunning)
			{
				return;
			}
			if (SuperReaperController.PreventShooting)
			{
				return;
			}
			CellData cellData = GameManager.Instance.Dungeon.data[self.ShootPoint.position.IntXY(VectorConversions.Floor)];
			if (cellData == null || cellData.type == CellType.WALL)
			{
				return;
			}
			if (!(_bulletSource.GetValue(self) as BulletScriptSource))
			{
				_bulletSource.SetValue(self, self.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>());
			}

			self.StartCoroutine(Dash(self, self.specRigidbody.Velocity));

			(_bulletSource.GetValue(self) as BulletScriptSource).BulletManager = self.bulletBank;
			(_bulletSource.GetValue(self) as BulletScriptSource).BulletScript = self.BulletScript;
			BotsModule.Log(self.BulletScript.GetType().ToString());
			(_bulletSource.GetValue(self) as BulletScriptSource).BulletScript = new CustomBulletScriptSelector(typeof(PrimalShotgrubScrip));
			(_bulletSource.GetValue(self) as BulletScriptSource).Initialize();
		}
        #endregion
        public static IEnumerator Dash(SuperReaperController self, Vector3 dir)
        {
			
			self.specRigidbody.OnEnterTrigger += ReaperPreCollision;
			float duration = Mathf.Max(0.0001f, 45 / 20);
			float elapsed = -BraveTime.DeltaTime;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				float adjSpeed = Mathf.Min(20, 45 / BraveTime.DeltaTime);
				self.specRigidbody.Velocity = BraveMathCollege.DegreesToVector(self.ShootPoint.rotation.z) * adjSpeed;
				yield return null;
			}
			self.specRigidbody.OnEnterTrigger -= ReaperPreCollision;
		}

		private static void ReaperPreCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
		{
			BotsModule.Log(sourceSpecRigidbody.gameObject.name);
			if (sourceSpecRigidbody.healthHaver != null && sourceSpecRigidbody.healthHaver.gameObject.GetComponent<PlayerController>() != null)
			{
				sourceSpecRigidbody.healthHaver.PreventAllDamage = false;
				sourceSpecRigidbody.healthHaver.NextShotKills = true;
			}
		}

		public class PrimalShotgrubScrip : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{


			protected override IEnumerator Top()
			{

				//AkSoundEngine.PostEvent("Play_WPN_stickycrossbow_shot_01", this.BulletBank.aiActor.gameObject);
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					//base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross"));
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}

				//float num2 = -22.5f;
				float num2 = 0;
				float num3 = 9f;
				for (int i = 0; i < 40; i++)
				{
					base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new ShotgrubManAttack1.GrossBullet(num2 + (float)i * num3));
				}

				yield return base.Wait(20);

				//float num = -45f;
				float desiredAngle = this.GetAimDirection(1f, 12f);
				float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
				bool isBlackPhantom = this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.IsBlackPhantom;
				//Bullet bullet = new BurstingBullet(isBlackPhantom, num);


				for (int i = 0; i < 3; i++)
				{
					float num = -45f + (float)i * 30f;
					base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Aim), new Direction(num, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom));
				}

				yield return null;
			}
			public class BurstingBullet : Bullet
			{
				
				public BurstingBullet(bool isBlackPhantom) : base("bigBullet", false, false, false)
				{

					//this.deltaAngle = deltaAngle;
					base.ForceBlackBullet = true;
					this.m_isBlackPhantom = isBlackPhantom;

				}

				private float deltaAngle;


				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
				{
					if (preventSpawningProjectiles)
					{
						return;
					}
					float num = base.RandomAngle();
					float num2 = 20f;
					for (int i = 0; i < 18; i++)
					{
						//Bullet bullet = new Bullet(null, false, false, false);

						float num3 = -22.5f;
						float num4 = 9f;

						ShotgrubManAttack1.GrossBullet bullet = new ShotgrubManAttack1.GrossBullet(num3 + (float)i * num4);
						base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), bullet);
						if (!this.m_isBlackPhantom)
						{
							bullet.ForceBlackBullet = false;
							bullet.Projectile.ForceBlackBullet = false;
							bullet.Projectile.ReturnFromBlackBullet();

						}
					}
				}

				// Token: 0x04000697 RID: 1687
				private const int NumBullets = 18;

				// Token: 0x04000698 RID: 1688
				private bool m_isBlackPhantom;
			}

		}


		public static void InformNeedsReloadHook(GameUIRoot self, PlayerController attachPlayer, Vector3 offset, float customDuration = -1f, string customKey = "")
		{

			FieldInfo _displayingReloadNeeded = typeof(GameUIRoot).GetField("m_displayingReloadNeeded", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _extantReloadLabels = typeof(GameUIRoot).GetField("m_extantReloadLabels", BindingFlags.NonPublic | BindingFlags.Instance);
			
				
			if (!attachPlayer)
			{
				return;
			}
			int num = (!attachPlayer.IsPrimaryPlayer) ? 1 : 0;
			if ((_displayingReloadNeeded.GetValue(self) as List<bool>) == null || num >= (_displayingReloadNeeded.GetValue(self) as List<bool>).Count)
			{
				return;
			}
			if ((_extantReloadLabels.GetValue(self) as List<dfLabel>) == null || num >= (_extantReloadLabels.GetValue(self) as List<dfLabel>).Count)
			{
				return;
			}
			if ((_displayingReloadNeeded.GetValue(self) as List<bool>)[num])
			{
				return;
			}
			dfLabel dfLabel = (_extantReloadLabels.GetValue(self) as List<dfLabel>)[num];
			if (dfLabel == null || dfLabel.IsVisible)
			{
				return;
			}
			dfFollowObject component = dfLabel.GetComponent<dfFollowObject>();
			dfLabel.IsVisible = true;
			if (component)
			{
				component.enabled = false;
			}
			self.StartCoroutine(FlashReloadLabel(self, dfLabel, attachPlayer, offset, customDuration, customKey));

		}


		private static IEnumerator FlashReloadLabel(GameUIRoot self, dfControl target, PlayerController attachPlayer, Vector3 offset, float customDuration = -1f, string customStringKey = "")
		{

			FieldInfo _displayingReloadNeeded = typeof(GameUIRoot).GetField("m_displayingReloadNeeded", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _isDisplayingCustomReload = typeof(GameUIRoot).GetField("m_isDisplayingCustomReload", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _deltaTime = typeof(GameUIRoot).GetField("m_deltaTime", BindingFlags.NonPublic | BindingFlags.Instance);

			int targetIndex = (!attachPlayer.IsPrimaryPlayer) ? 1 : 0;
			(_displayingReloadNeeded.GetValue(self) as List<bool>)[targetIndex] = true;
			target.transform.localScale = Vector3.one / GameUIRoot.GameUIScalar;
			dfLabel targetLabel = target as dfLabel;
			string customString = string.Empty;
			if (!string.IsNullOrEmpty(customStringKey))
			{
				customString = typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, new object[] { customStringKey }) as string;
				
			}
			string reloadString = (string)typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, new object[] { "#RELOAD" });
			string emptyString = (string)typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, new object[] { "#RELOAD_EMPTY" });

			if (customDuration > 0f)
			{
				_isDisplayingCustomReload.SetValue(self, true);
				float outerElapsed = 0f;
				while (outerElapsed < customDuration && !GameManager.Instance.IsPaused)
				{
					target.IsVisible = true;
					targetLabel.Text = customString;
					targetLabel.Color = Color.white;
					outerElapsed += BraveTime.DeltaTime;
					yield return null;
				}
				_isDisplayingCustomReload.SetValue(self, false);
			}
			else
			{
				while ((_displayingReloadNeeded.GetValue(self) as List<bool>)[targetIndex] && !GameManager.Instance.IsPaused)
				{
					target.IsVisible = true;
					if (!string.IsNullOrEmpty(customString))
					{
						targetLabel.Text = customString;
						targetLabel.Color = Color.white;
					}
					else if (attachPlayer.CurrentGun.CurrentAmmo != 0)
					{
						if (attachPlayer.CurrentGun.name.Contains("Beholster_Gun") && GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
						{
							targetLabel.Text = typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, new object[] { "#RELOAD_BEHOLD" }) as string;
						} 
						else if (attachPlayer.CurrentGun.GetComponent<CustomReloadText>() != null && GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
						{
							targetLabel.Text = attachPlayer.CurrentGun.GetComponent<CustomReloadText>().customReloadMessage;
						}
						else
						{
							targetLabel.Text = reloadString;
						}
						targetLabel.Color = Color.white;
					}
					else
					{
						targetLabel.Text = emptyString;
						targetLabel.Color = Color.red;
					}
					bool shouldShowEver = customDuration > 0f || attachPlayer.CurrentGun.CurrentAmmo != 0 || attachPlayer.IsInCombat;
					float elapsed = 0f;
					while (elapsed < 0.6f)
					{
						elapsed += (float)_deltaTime.GetValue(self);
						if (!(_displayingReloadNeeded.GetValue(self) as List<bool>)[targetIndex])
						{
							target.IsVisible = false;
							yield break;
						}
						if (!shouldShowEver)
						{
							target.IsVisible = false;
						}
						if (GameManager.Instance.IsPaused)
						{
							target.IsVisible = false;
						}
						yield return null;
					}
					target.IsVisible = false;
					elapsed = 0f;
					while (elapsed < 0.6f)
					{
						elapsed += (float)_deltaTime.GetValue(self);
						if (!(_displayingReloadNeeded.GetValue(self) as List<bool>)[targetIndex])
						{
							target.IsVisible = false;
							yield break;
						}
						yield return null;
					}
				}
			}
			(_displayingReloadNeeded.GetValue(self) as List<bool>)[targetIndex] = false;
			target.IsVisible = false;
			yield break;
		}

		private static void StartHook(Action<CharacterCostumeSwapper> orig, CharacterCostumeSwapper self)
		{

			FieldInfo _active = typeof(CharacterCostumeSwapper).GetField("m_active", BindingFlags.NonPublic | BindingFlags.Instance);

			bool flag = GameStatsManager.Instance.GetCharacterSpecificFlag(self.TargetCharacter, CharacterSpecificGungeonFlags.KILLED_PAST);
			if (self.HasCustomTrigger)
			{
				if (self.CustomTriggerIsFlag)
				{
					flag = GameStatsManager.Instance.GetFlag(self.TriggerFlag);
				}
				else if (self.CustomTriggerIsSpecialReserve)
				{
					flag = (GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05));
				}
			}
			if (flag)
			{
				_active.SetValue(self, true);
				if (self.TargetCharacter == PlayableCharacters.Guide)
				{
					self.CostumeSprite.HeightOffGround = 0.25f;
					self.AlternateCostumeSprite.HeightOffGround = 0.25f;
					self.CostumeSprite.UpdateZDepth();
					self.AlternateCostumeSprite.UpdateZDepth();
				}
				self.AlternateCostumeSprite.renderer.enabled = true;
				self.CostumeSprite.renderer.enabled = false;
			}
			else
			{
				_active.SetValue(self, false);
				self.AlternateCostumeSprite.renderer.enabled = false;
				self.CostumeSprite.renderer.enabled = false;
			}
		}

		public static void LogHook(Action<string, bool> orig, string text, bool debuglog = false)
		{
			using (StreamWriter file2 = File.AppendText(ETGMod.ResourcesDirectory + "/etbLog.txt")) 
			{
				file2.WriteLine(text);
			}
				


			orig(text, debuglog);
		}


		public static void LogHookU(Action<object> orig, object message)
		{
			using (StreamWriter file2 = File.AppendText(ETGMod.ResourcesDirectory + "/etbLogDebug.txt"))
			{
				file2.WriteLine(message.ToString());
			}
			orig(message);
		}

		private static void HandlePostProcessProjectileHook(Action<AlphabetSoupSynergyProcessor, Projectile> orig, AlphabetSoupSynergyProcessor self, Projectile targetProjectile)
		{

			FieldInfo _gun = typeof(AlphabetSoupSynergyProcessor).GetField("m_gun", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentEntryCount = typeof(AlphabetSoupSynergyProcessor).GetField("m_currentEntryCount", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _currentEntry = typeof(AlphabetSoupSynergyProcessor).GetField("m_currentEntry", BindingFlags.NonPublic | BindingFlags.Instance);

			if (!targetProjectile || !targetProjectile.sprite)
			{
				return;
			}
			if (_gun.GetValue(self) as Gun && (_gun.GetValue(self) as Gun).gunClass == GunClass.EXPLOSIVE)
			{
				return;
			}

			if ((_currentEntry.GetValue(self) as string).ToLower() == "transrights")
			{
				switch ((int)_currentEntryCount.GetValue(self))
				{
					case 0:
					case 1:
					case 9:
					case 10:
						targetProjectile.AdjustPlayerProjectileTint(new Color32(148, 218, 255, 255), 100000);
						break;

					case 2:
					case 3:
					case 7:
					case 8:
						targetProjectile.AdjustPlayerProjectileTint(new Color32(239, 148, 255, 255), 100000);
						break;

					case 4:
					case 5:
					case 6:
						targetProjectile.AdjustPlayerProjectileTint(new Color32(255, 255, 255, 255), 100000);
						break;
				}
			}
			// tr-an-s ri-gh-ts
			// 01-23-4 56-78-910
			targetProjectile.sprite.SetSprite(typeof(AlphabetSoupSynergyProcessor).GetMethod("GetLetterForWordPosition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { _currentEntry.GetValue(self) as string }) as string);

			_currentEntryCount.SetValue(self, (int)_currentEntryCount.GetValue(self) + 1);
		}


        #region unfix that synergy
        private static void HandleActivationStatusChangedHook(Action<OnActiveItemUsedSynergyProcessor, PlayerItem> orig, OnActiveItemUsedSynergyProcessor self, PlayerItem sourceItem)
		{

			FieldInfo _item = typeof(OnActiveItemUsedSynergyProcessor).GetField("m_item", BindingFlags.NonPublic | BindingFlags.Instance);


			if ((_item.GetValue(self) as PlayerItem).LastOwner && (_item.GetValue(self) as PlayerItem).LastOwner.HasActiveBonusSynergy(self.SynergyToCheck, false))
			{
				if (sourceItem.IsCurrentlyActive)
				{
					typeof(OnActiveItemUsedSynergyProcessor).GetMethod("HandleActivated", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
					
				}
				else
				{
					//self.HandleDeactivated();
				}
			}
		}

		private static void HandlePreDropHook(Action<OnActiveItemUsedSynergyProcessor> orig, OnActiveItemUsedSynergyProcessor self)
		{
			if (self.CreatesHoveringGun)
			{
				//typeof(OnActiveItemUsedSynergyProcessor).GetMethod("DisableAllHoveringGuns", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
			}
		}

		Dictionary<string, List<object>> problemSolver = new Dictionary<string, List<object>>();

        #endregion


        public delegate void ActionC<T, T2, T3, T4, T5, T6, T7, T8, T9>(T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
        #region old shop hooks
        public static void DoSetupHook(Action<BaseShopController> orig, BaseShopController self)
		{
			orig(self);
			FieldInfo _numberThingsPurchased = typeof(BaseShopController).GetField("m_numberThingsPurchased", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _itemControllers = typeof(BaseShopController).GetField("m_itemControllers", BindingFlags.NonPublic | BindingFlags.Instance);

			for (int num6 = 0; num6 < (_itemControllers.GetValue(self) as List<ShopItemController>).Count; num6++)
			{
				if (self.baseShopType == BaseShopController.AdditionalShopType.KEY)
				{
					(_itemControllers.GetValue(self) as List<ShopItemController>)[num6].CurrencyType = ShopItemController.ShopCurrencyType.KEYS;
				}
				if (self.baseShopType == BaseShopController.AdditionalShopType.FOYER_META)
				{
					(_itemControllers.GetValue(self) as List<ShopItemController>)[num6].CurrencyType = ShopItemController.ShopCurrencyType.META_CURRENCY;
				}
				if (self.baseShopType == (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL)
				{
					(_itemControllers.GetValue(self) as List<ShopItemController>)[num6].CurrencyType = (ShopItemController.ShopCurrencyType)CustomEnums.CustomShopCurrencyType.HEARTS;
				}
			}
		}


		public static int ModifiedPriceHook(Func<ShopItemController, int> orig, ShopItemController self)//your return type shoud be bool. for Func<> unlike Action its basically Func<Arg1,arg2,arg3... however many , return type
		{

			FieldInfo _baseParentShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
			
			
			if (self.IsResourcefulRatKey) { }
			else
			{
				
				if (self.CurrencyType == (ShopItemController.ShopCurrencyType)CustomEnums.CustomShopCurrencyType.HEARTS)
				{
					if (GameManager.Instance.PrimaryPlayer.healthHaver.Armor >= self.CurrentPrice * 2)
                    {
						self.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour = true;
						return self.CurrentPrice * 2;

					}
					self.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour = false;
					return self.CurrentPrice;
				}
			}
			return orig(self);
		}
		

		public static void OnEnteredRangeHook(Action<ShopItemController, PlayerController> orig, ShopItemController self, PlayerController interactor)
		{

			FieldInfo _baseParentShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _parentShop = typeof(ShopItemController).GetField("m_parentShop", BindingFlags.NonPublic | BindingFlags.Instance);

			if (!self)
			{
				return;
			}
			SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, false);
			SpriteOutlineManager.AddOutlineToSprite(self.sprite, Color.white);
			Vector3 offset = new Vector3(self.sprite.GetBounds().max.x + 0.1875f, self.sprite.GetBounds().min.y, 0f);
			EncounterTrackable component = self.item.GetComponent<EncounterTrackable>();
			string arg = (!(component != null)) ? self.item.DisplayName : component.journalData.GetPrimaryDisplayName(false);
			string text = self.ModifiedPrice.ToString();
			if ((_baseParentShop.GetValue(self) as BaseShopController) != null)
			{
				if ((_baseParentShop.GetValue(self) as BaseShopController).baseShopType == BaseShopController.AdditionalShopType.FOYER_META)
				{
					text += "[sprite \"hbux_text_icon\"]";
				}
				else if ((_baseParentShop.GetValue(self) as BaseShopController).baseShopType == BaseShopController.AdditionalShopType.CURSE)
				{
					text += "[sprite \"ui_coin\"]?";
				}
				else if ((_baseParentShop.GetValue(self) as BaseShopController).baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
				{
					text = "0[sprite \"ui_coin\"]?";
				}
				else if ((_baseParentShop.GetValue(self) as BaseShopController).baseShopType == BaseShopController.AdditionalShopType.KEY)
				{
					text += "[sprite \"ui_key\"]";
				}
				else if ((_baseParentShop.GetValue(self) as BaseShopController).baseShopType == (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL)
				{
					if (self.gameObject.GetOrAddComponent<DevilDealShopHelper>() == null)
                    {
						BotsModule.Log("god fucking damn it");
                    }

					if (self.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour)
                    {
						text += "[sprite \"armor_shield_pickup_001\"]";
						
					} 
					else
                    {
						text += "[sprite \"heart_big_idle_001\"]";
						
					}
				}
				else
				{
					text += "[sprite \"ui_coin\"]";
				}
			}
			if ((_parentShop.GetValue(self) as ShopController) != null && (_parentShop.GetValue(self) as ShopController) is MetaShopController)
			{
				text += "[sprite \"hbux_text_icon\"]";
				MetaShopController metaShopController = (_parentShop.GetValue(self) as ShopController) as MetaShopController;
				if (metaShopController.Hologramophone && self.item is ItemBlueprintItem)
				{
					ItemBlueprintItem itemBlueprintItem = self.item as ItemBlueprintItem;
					tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
					metaShopController.Hologramophone.ChangeToSprite(self.gameObject, encounterIconCollection, encounterIconCollection.GetSpriteIdByName(itemBlueprintItem.HologramIconSpriteName));
				}
			}
			string text2;
			if (((_baseParentShop.GetValue(self) as BaseShopController) && (_baseParentShop.GetValue(self) as BaseShopController).IsCapableOfBeingStolenFrom) || interactor.IsCapableOfStealing)
			{
				text2 = string.Format("[color red]{0}: {1} {2}[/color]", arg, text, StringTableManager.GetString("#STEAL"));
			}
			else
			{
				text2 = string.Format("{0}: {1}", arg, text);
			}
			if (self.IsResourcefulRatKey)
			{
				int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY));
				if (num < 1000)
				{
					int num2 = Mathf.Min(interactor.carriedConsumables.Currency, 1000 - num);
					if (num2 > 0)
					{
						text2 = text2 + "[color green] (-" + num2.ToString() + ")[/color]";
					}
				}
			}
			GameObject gameObject = GameUIRoot.Instance.RegisterDefaultLabel(self.transform, offset, text2);
			dfLabel componentInChildren = gameObject.GetComponentInChildren<dfLabel>();
			componentInChildren.ColorizeSymbols = false;
			componentInChildren.ProcessMarkup = true;
		}

		public static void InteractHook(Action<ShopItemController, PlayerController> orig, ShopItemController self, PlayerController player)
		{
			FieldInfo _baseParentShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _parentShop = typeof(ShopItemController).GetField("m_parentShop", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _pickedUp = typeof(ShopItemController).GetField("pickedUp", BindingFlags.NonPublic | BindingFlags.Instance);



			orig(self, player);

			if (self.CurrencyType == (ShopItemController.ShopCurrencyType)CustomEnums.CustomShopCurrencyType.HEARTS)
			{
				bool flag = false;
				bool flag2 = true;
				
				if ((bool)typeof(ShopItemController).GetMethod("ShouldSteal", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { player }))
				{
					flag = (_baseParentShop.GetValue(self) as BaseShopController).AttemptToSteal();
					flag2 = false;
					if (!flag)
					{
						player.DidUnstealthyAction();
						(_baseParentShop.GetValue(self) as BaseShopController).NotifyStealFailed();
						return;
					}
				}
				if (flag2)
				{
					bool flag3 = false;
					
					if (self.CurrencyType == (ShopItemController.ShopCurrencyType)CustomEnums.CustomShopCurrencyType.HEARTS)
					{
						
						

						if (player.healthHaver.Armor >= self.ModifiedPrice)
                        {
							BotsModule.Log($"Armour is {player.healthHaver.Armor} and the Price is {self.ModifiedPrice}");
							flag3 = player.healthHaver.Armor >= self.ModifiedPrice;
							
						}
						else
                        {
							BotsModule.Log($"Health is {player.stats.GetStatValue(PlayerStats.StatType.Health)} and the Price is {self.ModifiedPrice}");
							flag3 = (player.stats.GetStatValue(PlayerStats.StatType.Health) > self.ModifiedPrice);
						}
						

					}

					
					
					if (!flag3)
					{
						AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", self.gameObject);
						if ((_parentShop.GetValue(self) as ShopController) != null)
						{
							(_parentShop.GetValue(self) as ShopController).NotifyFailedPurchase(self);
						}
						if ((_baseParentShop.GetValue(self) as BaseShopController) != null)
						{
							(_baseParentShop.GetValue(self) as BaseShopController).NotifyFailedPurchase(self);
						}
						return;
					}
				}
				if (!(bool)_pickedUp.GetValue(self))
				{
					_pickedUp.SetValue(self, !self.item.PersistsOnPurchase);
					LootEngine.GivePrefabToPlayer(self.item.gameObject, player);
					if (flag2)
					{
						if (self.CurrencyType == (ShopItemController.ShopCurrencyType)CustomEnums.CustomShopCurrencyType.HEARTS)
						{

							if (self.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour)
							{
								player.healthHaver.Armor -= self.ModifiedPrice;
							} 
							else
                            {

								StatModifier statModifier = new StatModifier();
								statModifier.statToBoost = PlayerStats.StatType.Health;
								statModifier.amount = (self.ModifiedPrice) * -1;
								statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
								player.ownerlessStatModifiers.Add(statModifier);
								player.stats.RecalculateStats(player, false, false);
							}


						}
					}
					if ((_parentShop.GetValue(self) as ShopController) != null)
					{
						(_parentShop.GetValue(self) as ShopController).PurchaseItem(self, !flag, true);
					}
					if ((_baseParentShop.GetValue(self) as BaseShopController) != null)
					{
						(_baseParentShop.GetValue(self) as BaseShopController).PurchaseItem(self, !flag, true);
					}
					if (flag)
					{
						StatModifier statModifier = new StatModifier();
						statModifier.statToBoost = PlayerStats.StatType.Curse;
						statModifier.amount = 1f;
						statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
						player.ownerlessStatModifiers.Add(statModifier);
						player.stats.RecalculateStats(player, false, false);
						player.HandleItemStolen(self);
						(_baseParentShop.GetValue(self) as BaseShopController).NotifyStealSucceeded();
						player.IsThief = true;
						GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_ITEMS_STOLEN, 1f);
						if (self.SetsFlagOnSteal)
						{
							GameStatsManager.Instance.SetFlag(self.FlagToSetOnSteal, true);
						}
					}
					else
					{
						if (self.CurrencyType == ShopItemController.ShopCurrencyType.BLANKS)
						{
							player.Blanks++;
						}
						player.HandleItemPurchased(self);
					}
					if (!self.item.PersistsOnPurchase)
					{
						GameUIRoot.Instance.DeregisterDefaultLabel(self.transform);
					}
					AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", self.gameObject);
				}
			}
			
		}


		public static void PurchaseItemHook(Action<BaseShopController, ShopItemController, bool, bool> orig, BaseShopController self, ShopItemController item, bool actualPurchase = true, bool allowSign = true)
		{
			FieldInfo _room = typeof(BaseShopController).GetField("m_room", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _numberThingsPurchased = typeof(BaseShopController).GetField("m_numberThingsPurchased", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _itemControllers = typeof(BaseShopController).GetField("m_itemControllers", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _shopkeep = typeof(BaseShopController).GetField("m_shopkeep", BindingFlags.NonPublic | BindingFlags.Instance);


			float heightOffGround = -1f;
			if (item && item.sprite)
			{
				heightOffGround = item.sprite.HeightOffGround;
			}
			if (actualPurchase)
			{
				if (self.baseShopType == BaseShopController.AdditionalShopType.TRUCK)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_TRUCK, 1f);
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_TRUCK_SHOP, (float)item.ModifiedPrice);
				}
				if (self.baseShopType == BaseShopController.AdditionalShopType.GOOP)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_GOOP, 1f);
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_GOOP_SHOP, (float)item.ModifiedPrice);
				}
				if (self.baseShopType == BaseShopController.AdditionalShopType.CURSE)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_CURSE, 1f);
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_CURSE_SHOP, (float)item.ModifiedPrice);
					StatModifier statModifier = new StatModifier();
					statModifier.amount = 2.5f;
					statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					statModifier.statToBoost = PlayerStats.StatType.Curse;
					item.LastInteractingPlayer.ownerlessStatModifiers.Add(statModifier);
					item.LastInteractingPlayer.stats.RecalculateStats(item.LastInteractingPlayer, false, false);
				}
				if (self.baseShopType == BaseShopController.AdditionalShopType.BLANK)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_BLANK, 1f);
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_BLANK_SHOP, (float)item.ModifiedPrice);
				}
				if (self.baseShopType == BaseShopController.AdditionalShopType.KEY)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_KEY, 1f);
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_KEY_SHOP, (float)item.ModifiedPrice);
				}

				if (self.baseShopType == (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL)
				{
					SaveAPIManager.RegisterStatChange(CustomTrackedStats.MERCHANT_PURCHASES_HEART, 1f);
					SaveAPIManager.RegisterStatChange(CustomTrackedStats.MONEY_SPENT_AT_HEART_SHOP, (float)item.ModifiedPrice);
				}
				if (self.shopkeepFSM != null && self.baseShopType != BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
				{
					FsmObject fsmObject = self.shopkeepFSM.FsmVariables.FindFsmObject("referencedItem");
					if (fsmObject != null)
					{
						fsmObject.Value = item;
					}
					self.shopkeepFSM.SendEvent("succeedPurchase");
				}
			}
			if (!item.item.PersistsOnPurchase)
			{
				if (allowSign)
				{
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/Sign_SoldOut", ".prefab"));
					tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
					component.PlaceAtPositionByAnchor(item.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
					gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
					component.HeightOffGround = heightOffGround;
					component.UpdateZDepth();
				}
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				component2.PlaceAtPositionByAnchor(item.sprite.WorldCenter.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
				component2.transform.position = component2.transform.position.Quantize(0.0625f);
				component2.HeightOffGround = 5f;
				component2.UpdateZDepth();



				(_room.GetValue(self) as RoomHandler).DeregisterInteractable(item);
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (self.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
			{
				_numberThingsPurchased.SetValue(self, (int)_numberThingsPurchased.GetValue(self) + 1);
				GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
				int num;
				if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
							{
								num = 1;
							}
							else
							{
								num = 3;
							}
						}
						else
						{
							num = 2;
						}
					}
					else
					{
						num = 1;
					}
				}
				else
				{
					num = 1;
				}
				if ((int)_numberThingsPurchased.GetValue(self) >= num)
				{
					for (int i = 0; i < (_itemControllers.GetValue(self) as List<ShopItemController>).Count; i++)
					{
						if (!(_itemControllers.GetValue(self) as List<ShopItemController>)[i].Acquired)
						{
							(_itemControllers.GetValue(self) as List<ShopItemController>)[i].ForceOutOfStock();
						}
					}
					if (self.shopkeepFSM != null)
					{
						FsmObject fsmObject2 = self.shopkeepFSM.FsmVariables.FindFsmObject("referencedItem");
						if (fsmObject2 != null)
						{
							fsmObject2.Value = item;
						}
						self.shopkeepFSM.SendEvent("succeedPurchase");
						(_shopkeep.GetValue(self) as TalkDoerLite).IsInteractable = false;
					}
				}
			}
		}
        #endregion

		private static void DoNotificationInternalHook(Action<UINotificationController, NotificationParams> orig, UINotificationController self, NotificationParams notifyParams)
		{
			//SpellPickupObject
			if ((!string.IsNullOrEmpty(notifyParams.EncounterGuid) && ((Tools.BeyondItems.Contains(EncounterDatabase.GetEntry(notifyParams.EncounterGuid).pickupObjectId) && GameStatsManager.Instance.QueryEncounterable(notifyParams.EncounterGuid) != 1)) ||
				EncounterDatabase.GetEntry(notifyParams.EncounterGuid) != null && PickupObjectDatabase.GetById(EncounterDatabase.GetEntry(notifyParams.EncounterGuid).pickupObjectId) is SpellPickupObject))
            {
				FieldInfo _queuedNotifications = typeof(UINotificationController).GetField("m_queuedNotifications", BindingFlags.NonPublic | BindingFlags.Instance);
				FieldInfo _queuedNotificationParams = typeof(UINotificationController).GetField("m_queuedNotificationParams", BindingFlags.NonPublic | BindingFlags.Instance);
				(_queuedNotifications.GetValue(self) as List<IEnumerator>).Add(BeyondNotificaionHandler.HandleBeyondNotification(self, notifyParams));
				(_queuedNotificationParams.GetValue(self) as List<NotificationParams>).Add(notifyParams);
				self.StartCoroutine((IEnumerator)typeof(UINotificationController).GetMethod("PruneQueuedNotifications", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null));
			}
			else
            {
				orig(self, notifyParams);
			}			
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

		

		public delegate void CoolerAction<in T1, in t2, in t3, in t4, in t5, in t6, in t7, in t8, in t9, in t10, in t11, in t12, in t13, in t14, in t15 >(T1 a, t2 b, t3 c, t4 d, t5 e, t6 f, t7 g, t8 h, t9 i, t10 j, t11 k, t12 l, t13 m, t14 n, t15 o);
		public delegate void CoolerAction2<in T1, in t2, in t3, in t4, in t5, in t6, in t7, in t8, in t9>(T1 a, t2 b, t3 c, t4 d, t5 e, t6 f, t7 g, t8 h, t9 i);

		


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


		
	}
}
