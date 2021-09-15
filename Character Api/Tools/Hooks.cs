using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;
using MonoMod.RuntimeDetour;
using Object = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;
using GungeonAPI;
using BotsMod;

namespace CustomCharacters
{
    public static class Hooks
    {
        public static void Init()
        {           
            try
            {
                
                //Hook getNicknamehook = new Hook(
                //    typeof(StringTableManager).GetMethod("GetTalkingPlayerNick", BindingFlags.NonPublic | BindingFlags.Static),
                //    typeof(Hooks).GetMethod("GetTalkingPlayerNickHook")
                //);
                
                //Hook getNamehook = new Hook(
                //    typeof(StringTableManager).GetMethod("GetTalkingPlayerName", BindingFlags.NonPublic | BindingFlags.Static),
                 //   typeof(Hooks).GetMethod("GetTalkingPlayerNameHook")
                //);
                
                Hook getValueHook = new Hook(
                    typeof(dfLanguageManager).GetMethod("GetValue", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("GetValueHook")
                );
                
                Hook lateStartHook = new Hook(
                    typeof(Foyer).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(BotsMod.BotsModule).GetMethod("LateStart")
                );
                
                Hook punchoutUIHook = new Hook(
                    typeof(PunchoutPlayerController).GetMethod("UpdateUI", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("PunchoutUpdateUI")
                );
                
                Hook foyerCallbacksHook = new Hook(
                    typeof(Foyer).GetMethod("SetUpCharacterCallbacks", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("FoyerCallbacks2")

                );
                
                Hook languageManagerHook = new Hook(
                    typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("DFGetLocalizedValue")
                );
                
                var braveSETypes = new Type[]
                {
                    typeof(string),
                    typeof(string),
                };
                Hook braveLoad = new Hook(
                    typeof(BraveResources).GetMethod("Load", BindingFlags.Public | BindingFlags.Static, null, braveSETypes, null),
                    typeof(Hooks).GetMethod("BraveLoadObject")
                );
                
                Hook playerSwitchHook = new Hook(
                    typeof(Foyer).GetMethod("PlayerCharacterChanged", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("OnPlayerChanged")
                );
                
                Hook clearP1Hook = new Hook(
                    typeof(ETGModConsole).GetMethod("SwitchCharacter", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("PrimaryPlayerSwitched")
                );
                
                Hook clearP2Hook = new Hook(
                    typeof(GameManager).GetMethod("ClearSecondaryPlayer", BindingFlags.Public | BindingFlags.Instance),
                    typeof(Hooks).GetMethod("OnP2Cleared")
                );

                Hook setWinPicHook = new Hook(
                    typeof(AmmonomiconDeathPageController).GetMethod("SetWinPic", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(Hooks).GetMethod("SetWinPicHook", BindingFlags.Static | BindingFlags.NonPublic)
                );

				Hook interactHook = new Hook(
					typeof(ArkController).GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("InteractHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook getNumMetasToQuickRestartHook = new Hook(
					typeof(AmmonomiconDeathPageController).GetMethod("GetNumMetasToQuickRestart", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetNumMetasToQuickRestartHook", BindingFlags.Static | BindingFlags.Public)
				);

				/*Hook updateHook = new Hook(
					typeof(CharacterSelectIdleDoer).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("UpdateHook", BindingFlags.Static | BindingFlags.NonPublic)
				);

				Hook onEnableHook = new Hook(
					typeof(CharacterSelectIdleDoer).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("OnEnableHook", BindingFlags.Static | BindingFlags.NonPublic)
				);*/
				
				Hook canBeSelectedHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("CanBeSelected", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("CanBeSelectedHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook onSelectedCharacterCallbackHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("OnSelectedCharacterCallback", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("OnSelectedCharacterCallbackHook", BindingFlags.Static | BindingFlags.Public)
				);


				BotsModule.Log("hooks done");
			}
			catch (Exception e)
            {
                ToolsGAPI.PrintException(e);
            }
        }


		private static void UpdateHook(Action<CharacterSelectIdleDoer> orig, CharacterSelectIdleDoer self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().useGlow)
			{
				if (self.sprite.renderer.material != new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material))
				{
					var character = self.GetComponent<CustomCharacterFoyerController>();

					//self.sprite.usesOverrideMaterial = true;

					//var mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);


					self.sprite.usesOverrideMaterial = true;
					self.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber_ColorEmissive");

					self.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
					self.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");

					self.sprite.renderer.sharedMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));


					//BotsModule.Log(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material.name);

					self.sprite.renderer.material.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));

					self.sprite.renderer.material.SetColor("_EmissiveColor", character.emissiveColor);

					self.sprite.renderer.material.SetFloat("_EmissiveColorPower", character.emissiveColorPower);

					self.sprite.renderer.material.SetFloat("_EmissivePower", character.emissivePower);

					//self.sprite.renderer.material = mat;
				}

			}
			/*
			if (self.IsEevee)
			{
				self.sprite.usesOverrideMaterial = true;
				self.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
				self.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", self.EeveeTex);
				self.m_lastEeveeSwitchTime += BraveTime.DeltaTime;
				if (self.m_lastEeveeSwitchTime > 2.5f)
				{
					self.m_lastEeveeSwitchTime -= 2.5f;
					int num = UnityEngine.Random.Range(0, self.AnimationLibraries.Length);
					self.spriteAnimator.Library = self.AnimationLibraries[num];
					self.spriteAnimator.Play(self.coreIdleAnimation);
				}
			}
			*/
			orig(self);
		}

		public static void OnSelectedCharacterCallbackHook(Action<FoyerCharacterSelectFlag, PlayerController> orig, FoyerCharacterSelectFlag self, PlayerController newCharacter)
		{
			FieldInfo _isAlternateCostume = typeof(FoyerCharacterSelectFlag).GetField("m_isAlternateCostume", BindingFlags.NonPublic | BindingFlags.Instance);

			Debug.Log(string.Concat(new object[]
			{
				newCharacter.name,
				"|",
				newCharacter.characterIdentity,
				" <===="
			}));
		//	BotsModule.Log($"{newCharacter.gameObject.name} - {self.CharacterPrefabPath}");

			if (newCharacter.gameObject.name.Contains(self.CharacterPrefabPath, false))
			{

				BotsModule.Log("fuck you die");
				self.gameObject.SetActive(false);
				self.GetComponent<SpeculativeRigidbody>().enabled = false;
				if (self.IsEevee)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, -5f);
				}
				if (self.IsGunslinger)
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, -7f);
				}
				
				if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().metaCost > 0)
				{
					BotsModule.Log("character cost " + self.GetComponent<CustomCharacterFoyerController>().metaCost.ToString());
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, (self.GetComponent<CustomCharacterFoyerController>().metaCost) * -1);
				}
			}
			else if (!self.gameObject.activeSelf)
			{
				self.gameObject.SetActive(true);
				SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, true);
				self.specRigidbody.enabled = true;
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(self.specRigidbody, null, false);
				if (!(bool)_isAlternateCostume.GetValue(self))
				{
					CharacterSelectIdleDoer component = self.GetComponent<CharacterSelectIdleDoer>();
					component.enabled = true;
				}
			}
		}

		private static void OnEnableHook(Action<CharacterSelectIdleDoer> orig, CharacterSelectIdleDoer self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() != null && self.sprite.renderer.material != new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material))
			{
				var character = self.GetComponent<CustomCharacterFoyerController>();

				//self.sprite.usesOverrideMaterial = true;

				//var mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);


				self.sprite.usesOverrideMaterial = true;
				self.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber_ColorEmissive");
				self.sprite.renderer.material.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));

				self.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
				self.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");


				BotsModule.Log(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material.shader.name);

				self.sprite.renderer.sharedMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));

				self.sprite.renderer.material.SetColor("_EmissiveColor", character.emissiveColor);

				self.sprite.renderer.material.SetFloat("_EmissiveColorPower", character.emissiveColorPower);

				self.sprite.renderer.material.SetFloat("_EmissivePower", character.emissivePower);

				//self.sprite.renderer.material = mat;
			}

			orig(self);
		}

		public static bool CanBeSelectedHook(FoyerCharacterSelectFlag self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() == null)
			{
				return (!self.IsEevee || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 5f) && (!self.IsGunslinger || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 7f);
			} else
			{
				return (!self.IsEevee || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 5f) && (!self.IsGunslinger || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 7f) && (self.GetComponent<CustomCharacterFoyerController>().metaCost == 0 || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= self.GetComponent<CustomCharacterFoyerController>().metaCost);
			}
			
		}

		public static QuickRestartOptions GetNumMetasToQuickRestartHook()
		{
			QuickRestartOptions result = default(QuickRestartOptions);
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CharacterUsesRandomGuns)
				{
					result.GunGame = true;
					result.NumMetas += 6;
					break;
				}
				if (GameManager.Instance.AllPlayers[i].characterIdentity == (PlayableCharacters)CustomPlayableCharacters.Custom)
				{
					result.NumMetas += GameManager.Instance.AllPlayers[i].GetComponent<CustomCharacterController>().metaCost;
				}
				if (GameManager.Instance.AllPlayers[i].characterIdentity == PlayableCharacters.Eevee)
				{
					result.NumMetas += 5;
				}
				else if (GameManager.Instance.AllPlayers[i].characterIdentity == PlayableCharacters.Gunslinger)
				{
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
					{
						result.NumMetas += 5;
					}
					else
					{
						result.NumMetas += 7;
					}
				}
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				result.BossRush = true;
				result.NumMetas += 3;
			}
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			{
				result.ChallengeMode = ChallengeManager.ChallengeModeType;
				if (ChallengeManager.ChallengeModeType == ChallengeModeType.ChallengeMode)
				{
					if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.CHALLENGE_MODE_ATTEMPTS) >= 30f)
					{
						result.NumMetas++;
					}
					else
					{
						result.NumMetas += 6;
					}
				}
				else if (ChallengeManager.ChallengeModeType == ChallengeModeType.ChallengeMegaMode)
				{
					if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.CHALLENGE_MODE_ATTEMPTS) >= 30f)
					{
						result.NumMetas += 2;
					}
					else
					{
						result.NumMetas += 12;
					}
				}
			}
			BotsModule.Log(result.NumMetas.ToString());
			return result;
		}


		public static void InteractHook(ArkController self, PlayerController interactor)
		{

			FieldInfo _hasBeenInteracted = typeof(ArkController).GetField("m_hasBeenInteracted", BindingFlags.NonPublic | BindingFlags.Instance);

			SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, false);
			SpriteOutlineManager.RemoveOutlineFromSprite(self.LidAnimator.sprite, false);
			if (!(bool)_hasBeenInteracted.GetValue(self))
			{
				_hasBeenInteracted.SetValue(self, true);
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
			self.StartCoroutine(OpenHook(self, interactor));
		}

		private static IEnumerator OpenHook(ArkController self, PlayerController interactor)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
				{
					GameManager.Instance.AllPlayers[i].SetInputOverride("ark");
				}
			}
			self.LidAnimator.Play();
			self.ChestAnimator.Play();
			self.PoofAnimator.PlayAndDisableObject(string.Empty, null);
			self.specRigidbody.Reinitialize();
			GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 2f;
			GameManager.Instance.MainCameraController.OverridePosition = self.ChestAnimator.sprite.WorldCenter + new Vector2(0f, 2f);
			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			self.StartCoroutine(typeof(ArkController).GetMethod("HandleLightSprite", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null) as IEnumerator);
			while (self.LidAnimator.IsPlaying(self.LidAnimator.CurrentClip))
			{
				yield return null;
			}
			yield return self.StartCoroutine(typeof(ArkController).GetMethod("HandleGun", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor }) as IEnumerator);
			yield return new WaitForSeconds(0.5f);
			Pixelator.Instance.DoFinalNonFadedLayer = true;
			yield return self.StartCoroutine(HandleClockhairHook(self, interactor));
			interactor.ClearInputOverride("ark");
			yield break;
		}


		private static IEnumerator HandleClockhairHook(ArkController self, PlayerController interactor)
		{
			FieldInfo _heldPastGun = typeof(ArkController).GetField("m_heldPastGun", BindingFlags.NonPublic | BindingFlags.Instance);

			Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
			ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
			float elapsed = 0f;
			float duration = clockhair.ClockhairInDuration;
			Vector3 clockhairTargetPosition = interactor.CenterPosition;
			Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
			clockhair.renderer.enabled = true;
			clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
			clockhair.spriteAnimator.Play("clockhair_intro");
			clockhair.hourAnimator.Play("hour_hand_intro");
			clockhair.minuteAnimator.Play("minute_hand_intro");
			clockhair.secondAnimator.Play("second_hand_intro");
			BraveInput currentInput = BraveInput.GetInstanceForPlayer(interactor.PlayerIDX);
			while (elapsed < duration)
			{
				typeof(ArkController).GetMethod("UpdateCameraPositionDuringClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor.CenterPosition });
				
				if (GameManager.INVARIANT_DELTA_TIME == 0f)
				{
					elapsed += 0.05f;
				}
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				float t = elapsed / duration;
				float smoothT = Mathf.SmoothStep(0f, 1f, t);
				clockhairTargetPosition = (Vector3)typeof(ArkController).GetMethod("GetTargetClockhairPosition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { currentInput, clockhairTargetPosition });
				//clockhairTargetPosition = self.GetTargetClockhairPosition(currentInput, clockhairTargetPosition);
				Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
				clockhairTransform.position = currentPosition.WithZ(0f);
				if (t > 0.5f)
				{
					clockhair.renderer.enabled = true;
				}
				if (t > 0.75f)
				{
					clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
					clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
					clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
					GameCursorController.CursorOverride.SetOverride("ark", true, null);
				}
				clockhair.sprite.UpdateZDepth();
				typeof(ArkController).GetMethod("PointGunAtClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor, clockhairTransform });
				yield return null;
			}
			clockhair.SetMotionType(1f);
			float shotTargetTime = 0f;
			float holdDuration = 4f;
			PlayerController shotPlayer = null;
			bool didShootHellTrigger = false;
			Vector3 lastJitterAmount = Vector3.zero;
			bool m_isPlayingChargeAudio = false;
			for (; ; )
			{
				typeof(ArkController).GetMethod("UpdateCameraPositionDuringClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor.CenterPosition });
				clockhair.transform.position = clockhair.transform.position - lastJitterAmount;
				clockhair.transform.position = (Vector3)typeof(ArkController).GetMethod("GetTargetClockhairPosition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { currentInput, clockhair.transform.position.XY() });
				clockhair.sprite.UpdateZDepth();
				bool isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckPlayerTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Instance.PrimaryPlayer, clockhairTransform });
				shotPlayer = GameManager.Instance.PrimaryPlayer;
				if (!isTargetingValidTarget && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckPlayerTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Instance.SecondaryPlayer, clockhairTransform });
					shotPlayer = GameManager.Instance.SecondaryPlayer;
				}
				if (!isTargetingValidTarget && GameStatsManager.Instance.AllCorePastsBeaten())
				{
					isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckHellTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self.HellCrackSprite, clockhairTransform });
					didShootHellTrigger = isTargetingValidTarget;
				}
				if (isTargetingValidTarget)
				{
					clockhair.SetMotionType(-10f);
				}
				else
				{
					clockhair.SetMotionType(1f);
				}
				if ((currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed) && isTargetingValidTarget)
				{
					if (!m_isPlayingChargeAudio)
					{
						m_isPlayingChargeAudio = true;
						AkSoundEngine.PostEvent("Play_OBJ_pastkiller_charge_01", self.gameObject);
					}
					shotTargetTime += BraveTime.DeltaTime;
				}
				else
				{
					shotTargetTime = Mathf.Max(0f, shotTargetTime - BraveTime.DeltaTime * 3f);
					if (m_isPlayingChargeAudio)
					{
						m_isPlayingChargeAudio = false;
						AkSoundEngine.PostEvent("Stop_OBJ_pastkiller_charge_01", self.gameObject);
					}
				}
				if ((currentInput.ActiveActions.ShootAction.WasReleased || currentInput.ActiveActions.InteractAction.WasReleased) && isTargetingValidTarget && shotTargetTime > holdDuration && !GameManager.Instance.IsPaused)
				{
					break;
				}
				if (shotTargetTime > 0f)
				{
					float distortionPower = Mathf.Lerp(0f, 0.35f, shotTargetTime / holdDuration);
					float distortRadius = 0.5f;
					float edgeRadius = Mathf.Lerp(4f, 7f, shotTargetTime / holdDuration);
					clockhair.UpdateDistortion(distortionPower, distortRadius, edgeRadius);
					float desatRadiusUV = Mathf.Lerp(2f, 0.25f, shotTargetTime / holdDuration);
					clockhair.UpdateDesat(true, desatRadiusUV);
					shotTargetTime = Mathf.Min(holdDuration + 0.25f, shotTargetTime + BraveTime.DeltaTime);
					float d = Mathf.Lerp(0f, 0.5f, (shotTargetTime - 1f) / (holdDuration - 1f));
					Vector3 vector = (UnityEngine.Random.insideUnitCircle * d).ToVector3ZUp(0f);
					BraveInput.DoSustainedScreenShakeVibration(shotTargetTime / holdDuration * 0.8f);
					clockhair.transform.position = clockhair.transform.position + vector;
					lastJitterAmount = vector;
					clockhair.SetMotionType(Mathf.Lerp(-10f, -2400f, shotTargetTime / holdDuration));
				}
				else
				{
					lastJitterAmount = Vector3.zero;
					clockhair.UpdateDistortion(0f, 0f, 0f);
					clockhair.UpdateDesat(false, 0f);
					shotTargetTime = 0f;
					BraveInput.DoSustainedScreenShakeVibration(0f);
				}
				typeof(ArkController).GetMethod("PointGunAtClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor, clockhairTransform });
				yield return null;
			}
			BraveInput.DoSustainedScreenShakeVibration(0f);
			BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Hard);
			clockhair.StartCoroutine(clockhair.WipeoutDistortionAndFade(0.5f));
			clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
			Pixelator.Instance.FadeToColor(1f, Color.white, true, 0.2f);
			Pixelator.Instance.DoRenderGBuffer = false;
			clockhair.spriteAnimator.Play("clockhair_fire");
			clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
			clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
			clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
			yield return null;
			TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			bool isShortTunnel = didShootHellTrigger || shotPlayer.characterIdentity == PlayableCharacters.CoopCultist || (bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity });
			UnityEngine.Object.Destroy((_heldPastGun.GetValue(self) as Transform).gameObject);
			interactor.ToggleGunRenderers(true, "ark");
			GameCursorController.CursorOverride.RemoveOverride("ark");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return self.StartCoroutine(ttcc.HandleTimeTubeCredits(clockhair.sprite.WorldCenter, isShortTunnel, clockhair.spriteAnimator, (!didShootHellTrigger) ? shotPlayer.PlayerIDX : 0, false));
			if (isShortTunnel)
			{
				Pixelator.Instance.FadeToBlack(1f, false, 0f);
				yield return new WaitForSeconds(1f);
			}
			if (didShootHellTrigger)
			{
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
				GameManager.Instance.LoadCustomLevel("tt_bullethell");
			}
			else if (shotPlayer.characterIdentity == PlayableCharacters.CoopCultist)
			{
				GameManager.IsCoopPast = true;
				typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
				
				GameManager.Instance.LoadCustomLevel("fs_coop");
			}
			else if ((bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity }) && shotPlayer.characterIdentity == PlayableCharacters.Gunslinger)
			{
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
				GameManager.IsGunslingerPast = true;
				typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { true });
				GameManager.Instance.LoadCustomLevel("tt_bullethell");
			}
			else if ((bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity }))
			{
				bool flag = false;
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
				switch (shotPlayer.characterIdentity)
				{
					case PlayableCharacters.Pilot:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_pilot");
						break;
					case PlayableCharacters.Convict:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_convict");
						break;
					case PlayableCharacters.Robot:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_robot");
						break;
					case PlayableCharacters.Soldier:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_soldier");
						break;
					case PlayableCharacters.Guide:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_guide");
						break;
					case PlayableCharacters.Bullet:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel("fs_bullet");
						break;

					case (PlayableCharacters)CustomPlayableCharacters.Custom:
						flag = true;
						typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
						GameManager.Instance.LoadCustomLevel(shotPlayer.GetComponent<CustomCharacterController>().past);
						break;
				}
				if (!flag)
				{
					AmmonomiconController.Instance.OpenAmmonomicon(true, true);
				}
				else
				{
					GameUIRoot.Instance.ToggleUICamera(false);
				}
			}
			else
			{
				AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			}
			for (; ; )
			{
				yield return null;
			}
			yield break;
		}

		//Hook for Punchout UI being updated (called when UI updates)
		public static void PunchoutUpdateUI(Action<PunchoutPlayerController> orig, PunchoutPlayerController self)
        {
            orig(self);
            var customChar = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>();
            if (customChar != null)
            {
                char index = self.PlayerUiSprite.SpriteName.Last();
                SpriteHandler.HandlePunchoutSprites(self, customChar.data);
                if (customChar.data.punchoutFaceCards != null)
                {
                    self.PlayerUiSprite.SpriteName = customChar.data.nameInternal + "_punchout_facecard" + index;
                    ToolsGAPI.Print(self.PlayerUiSprite.SpriteName);
                }
            }
        }

        public static string GetTalkingPlayerNickHook(Func<string> orig)
        {
            PlayerController talkingPlayer = Hooks.GetTalkingPlayer();
            if (talkingPlayer.IsThief)
            {
                return "#THIEF_NAME";
            }
            if(talkingPlayer.GetComponent<CustomCharacter>() != null)
            {
                if (talkingPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return "#PLAYER_NICK_" + talkingPlayer.GetComponent<CustomCharacter>().data.nickname.ToUpper();
                }
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
            {
                return "#PLAYER_NICK_RANDOM";
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
            {
                return "#PLAYER_NICK_GUNSLINGER";
            }
            return "#PLAYER_NICK_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
        }

        public static string GetValueHook(Func<dfLanguageManager, string, string> orig, dfLanguageManager self, string key)
        {
            if (characterDeathNames.Contains(key))
            {
                if(GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>() != null && GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.name;
                }
            }
            return orig(self, key);
        }

        public static string GetTalkingPlayerNameHook(Func<string> orig)
        {
            PlayerController talkingPlayer = Hooks.GetTalkingPlayer();
            if (talkingPlayer.IsThief)
            {
                return "#THIEF_NAME";
            }
            if (talkingPlayer.GetComponent<CustomCharacter>() != null)
            {
                if (talkingPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return "#PLAYER_NAME_" + talkingPlayer.GetComponent<CustomCharacter>().data.nameShort.ToUpper();
                }
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
            {
                return "#PLAYER_NAME_RANDOM";
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
            {
                return "#PLAYER_NAME_GUNSLINGER";
            }
            return "#PLAYER_NAME_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
        }

        private static PlayerController GetTalkingPlayer()
        {
            List<TalkDoerLite> allNpcs = StaticReferenceManager.AllNpcs;
            for (int i = 0; i < allNpcs.Count; i++)
            {
                if (allNpcs[i])
                {
                    if (!allNpcs[i].IsTalking || !allNpcs[i].TalkingPlayer || GameManager.Instance.HasPlayer(allNpcs[i].TalkingPlayer))
                    {
                        if (allNpcs[i].IsTalking && allNpcs[i].TalkingPlayer)
                        {
                            return allNpcs[i].TalkingPlayer;
                        }
                    }
                }
            }
            return GameManager.Instance.PrimaryPlayer;
        }
		

		//Triggers FoyerCharacterHandler (called from Foyer.SetUpCharacterCallbacks)
		
		public static List<FoyerCharacterSelectFlag> FoyerCallbacks2(Func<Foyer, List<FoyerCharacterSelectFlag>> orig, Foyer self)
		{
			var sortedByX = orig(self);

			var sortedByXCustom = FoyerCharacterHandler.AddCustomCharactersToFoyer(sortedByX);

			foreach(var character in sortedByXCustom)
			{
				sortedByX.Add(character);
			}

			return sortedByX;
		}

        //Used to add in strings 
        public static string DFGetLocalizedValue(Func<dfControl, string, string> orig, dfControl self, string key)
        {
            foreach (var pair in StringHandler.customStringDictionary)
            {
                if (pair.Key.ToLower() == key.ToLower())
                {
                    return pair.Value;
                }
            }
            return orig(self, key);
        }

        //Used to set fake player prefabs to active on instantiation (hook doesn't work on this call)
        public static Object BraveLoadObject(Func<string, string, Object> orig, string path, string extension = ".prefab")
        {
            var value = orig(path, extension);
            if (value == null)
            {
                path = path.ToLower();
                if (CharacterBuilder.storedCharacters.ContainsKey(path))
                {
                    var character = CharacterBuilder.storedCharacters[path].Second;
                    return character;
                }
            }
            return value;
        }

        public static void OnPlayerChanged(Action<Foyer, PlayerController> orig, Foyer self, PlayerController player)
        {
            ResetInfiniteGuns();
            orig(self, player);
        }

        public static void OnP2Cleared(Action<GameManager> orig, GameManager self)
        {
            orig(self);
            ResetInfiniteGuns();
        }

        public static void PrimaryPlayerSwitched(Action<ETGModConsole, string[]> orig, ETGModConsole self, string[] args)
        {
            try
            {
                orig(self, args);
            }
            catch { }
            ResetInfiniteGuns();
        }

        //Resets all the character-specific infinite guns 
        public static Dictionary<int, GunBackupData> gunBackups = new Dictionary<int, GunBackupData>();
        public static void ResetInfiniteGuns()
        {
            var player1 = GameManager.Instance?.PrimaryPlayer?.GetComponent<CustomCharacter>();
            var player2 = GameManager.Instance?.SecondaryPlayer?.GetComponent<CustomCharacter>();
            List<int> removables = new List<int>();
            foreach (var entry in gunBackups)
            {
                if ((player1 && player1.GetInfiniteGunIDs().Contains(entry.Key)) || (player2 && player2.GetInfiniteGunIDs().Contains(entry.Key))) continue;
                var gun = PickupObjectDatabase.GetById(entry.Key) as Gun;
                gun.InfiniteAmmo = entry.Value.InfiniteAmmo;
                gun.CanBeDropped = entry.Value.CanBeDropped;
                gun.PersistsOnDeath = entry.Value.PersistsOnDeath;
                gun.PreventStartingOwnerFromDropping = entry.Value.PreventStartingOwnerFromDropping;
                removables.Add(entry.Key);
                ToolsGAPI.Print($"Reset {gun.EncounterNameOrDisplayName} to infinite = {gun.InfiniteAmmo}");
            }
            foreach (var id in removables)
                gunBackups.Remove(id);

        }
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

					case (PlayableCharacters)CustomPlayableCharacters.Custom:
                        if (GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacterController>() != null)
                        {
                            self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacterController>().data.pastWinPic;
                            goto IL_1B4;
                        }
                        break;

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
                                case (PlayableCharacters)CustomPlayableCharacters.Custom:
                                    if (GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacterController>() != null)
                                    {
                                        self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacterController>().data.junkanWinPic;
                                        goto IL_384;
                                    }
                                    break;
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



		public static List<string> characterDeathNames = new List<string>
        {
            "#CHAR_ROGUE_SHORT",
            "#CHAR_CONVICT_SHORT",
            "#CHAR_ROBOT_SHORT",
            "#CHAR_MARINE_SHORT",
            "#CHAR_GUIDE_SHORT",
            "#CHAR_CULTIST_SHORT",
            "#CHAR_BULLET_SHORT",
            "#CHAR_PARADOX_SHORT",
            "#CHAR_GUNSLINGER_SHORT"
        };

        public struct GunBackupData
        {
            public bool InfiniteAmmo,
                CanBeDropped,
                PersistsOnDeath,
                PreventStartingOwnerFromDropping;
        }
    }
}
