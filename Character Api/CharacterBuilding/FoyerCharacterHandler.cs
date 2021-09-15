using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;

using MonoMod.RuntimeDetour;
using HutongCharacter = HutongGames.PlayMaker.Actions.ChangeToNewCharacter;

using SaveAPI;
using ItemAPI;

namespace CustomCharacters

{
    public static class FoyerCharacterHandler
    {
        private static bool hasInitialized = false;
        private static FieldInfo m_isHighlighted = typeof(TalkDoerLite).GetField("m_isHighlighted", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Vector3[] foyerPositions =
        {
            new Vector3(15.8f, 26.6f, 27.1f),
            new Vector3(31.7f, 27.3f, 27.8f),
            new Vector3(28.9f, 62.8f, 63.3f),

        };

        public static void Init()
        {
            //ETGModConsole.Commands.AddUnit("pos", s =>
            //{
            //    Tools.Print(GameManager.Instance.PrimaryPlayer.transform.position, "55AAFF", true);
            //});

            Hook overheadElemAppearHook = new Hook(
                typeof(TalkDoerLite).GetMethod("CreateOverheadUI", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(FoyerCharacterHandler).GetMethod("CreateOverheadUI")
            );
        }

        public static void CreateOverheadUI(Action<TalkDoerLite> orig, TalkDoerLite self)
        {
            bool wasActive = self.OverheadUIElementOnPreInteract != null && self.OverheadUIElementOnPreInteract.activeSelf;

            self.OverheadUIElementOnPreInteract.SetActive(true);
            orig(self);
            if (!wasActive)
                self.OverheadUIElementOnPreInteract.SetActive(false);
            Tools.Print("Did overhead swippity swap");
        }

        public static List<FoyerCharacterSelectFlag> AddCustomCharactersToFoyer(List<FoyerCharacterSelectFlag> sortedByX)
        {
            if (!hasInitialized)
            {
                Init();
                hasInitialized = true;
            }

            List<FoyerCharacterSelectFlag> list = new List<FoyerCharacterSelectFlag>();

            foreach (var character in CharacterBuilder.storedCharacters)
            {
                try
                {
                    Tools.Print($"Adding {character.Key} to the breach.");
                    var identity = character.Value.First.baseCharacter;
                   
                    var selectCharacter = AddCharacterToFoyer(character.Key, GetFlagFromIdentity(identity, sortedByX).gameObject);
                    //This makes it so you can hover over them before choosing a character
                    //sortedByX.Insert(6, selectCharacter); 
                    sortedByX.Insert(6, selectCharacter);
                    selectCharacter.OverheadElement.SetActive(true);
                    list.Add(selectCharacter);
                }
                catch (Exception e)
                {
                    Tools.PrintError($"An error occured while adding character {character.Key} to the breach.");
                    Tools.PrintException(e);
                }
            }
            
            foreach(var flag in sortedByX)
            {
                ResetToIdle(flag);
            }

            return list;

        }

        public static FoyerCharacterSelectFlag GetFlagFromIdentity(PlayableCharacters character, List<FoyerCharacterSelectFlag> sortedByX)
        {
            string path;
            foreach (var flag in sortedByX)
            {
                path = flag.CharacterPrefabPath.ToLower();
                if (character == PlayableCharacters.Eevee && flag.IsEevee) return flag;
                if (character == PlayableCharacters.Gunslinger && flag.IsGunslinger) return flag;

                if (character == PlayableCharacters.Bullet && path.Contains("bullet")) return flag;
                if (character == PlayableCharacters.Convict && path.Contains("convict")) return flag;
                if (character == PlayableCharacters.Guide && path.Contains("guide")) return flag;
                if (character == PlayableCharacters.Soldier && path.Contains("marine")) return flag;
                if (character == PlayableCharacters.Robot && path.Contains("robot")) return flag;
                if (character == PlayableCharacters.Pilot && path.Contains("rogue")) return flag;
            }
            Tools.PrintError("Couldn't find foyer select flag for: " + character);
            Tools.PrintError("    Have you unlocked them yet?");
            return sortedByX[1];
        }

        private static FoyerCharacterSelectFlag AddCharacterToFoyer(string characterPath, GameObject selectFlagPrefab)
        {
            //Gather character data
            var customCharacter = CharacterBuilder.storedCharacters[characterPath.ToLower()];

            
            //if (!CheckUnlocked(customCharacter.First))
            //{
            //    return null;
            //}


            if (customCharacter.First.characterID >= foyerPositions.Length)
            {
                Tools.PrintError("Not enough room in the foyer for: " + customCharacter.First.nameShort);
                Tools.PrintError("    Use the character command instead.");
                Tools.PrintError("    Jeez, how many characters do you need?");
                return null;
            }
            Tools.Print("    Got custom character");

            //Create new object
            FoyerCharacterSelectFlag selectFlag = GameObject.Instantiate(selectFlagPrefab).GetComponent<FoyerCharacterSelectFlag>();
            selectFlag.transform.position = foyerPositions[customCharacter.First.characterID];
            selectFlag.CharacterPrefabPath = characterPath;
            selectFlag.name = "NPC_FoyerCharacter_" + customCharacter.First.nameShort;
            Tools.Print("    Made select flag");

            //Replace sprites
            HandleSprites(selectFlag, customCharacter.Second.GetComponent<PlayerController>());
            Tools.Print("    Replaced sprites");

            var td = selectFlag.talkDoer;

            //Setup overhead card
            CreateOverheadCard(selectFlag, customCharacter.First);
            td.OverheadUIElementOnPreInteract = selectFlag.OverheadElement;
            Tools.Print("    Made Overhead Card");

            //Change the effect of talking to the character
            foreach (var state in selectFlag.playmakerFsm.Fsm.FsmComponent.FsmStates)
            {
                foreach (var action in state.Actions)
                {
                    if (action is HutongCharacter)
                    {
                        ((HutongCharacter)action).PlayerPrefabPath = characterPath;
                    }
                }
            }

            //Make interactable
            if (!Dungeonator.RoomHandler.unassignedInteractableObjects.Contains(td))
                Dungeonator.RoomHandler.unassignedInteractableObjects.Add(td);
            Tools.Print("    Adjusted Talk-Doer");

            //Player changed callback - Hides and shows player select object
            Foyer.Instance.OnPlayerCharacterChanged += (player) =>
            {
                OnPlayerCharacterChanged(player, selectFlag, characterPath);
            };
            Tools.Print("    Added callback");

            return selectFlag;
        }

        public static bool CheckUnlocked(CustomCharacterData data)
        {
            if (data.unlockFlag == CustomDungeonFlags.NONE)
            {
                return true;
            }
            if (!SaveAPIManager.GetFlag(data.unlockFlag))
            {
                BotsMod.BotsModule.Log($"The character {data.name} requires the flag {data.unlockFlag} to be unlocked", BotsMod.BotsModule.LOCKED_CHARACTOR_COLOR);
            }

            return SaveAPIManager.GetFlag(data.unlockFlag);
        }

        private static void HandleSprites(BraveBehaviour selectCharacter, BraveBehaviour player)
        {


            selectCharacter.spriteAnimator.Library = player.spriteAnimator.Library;
            selectCharacter.sprite.Collection = player.sprite.Collection;


            selectCharacter.renderer.material = new Material(selectCharacter.renderer.material);


            BotsMod.BotsModule.Log($"{selectCharacter.spriteAnimator.gameObject}");
            
            selectCharacter.sprite.ForceBuild();
            string coreIdleAnimation = selectCharacter.GetComponent<CharacterSelectIdleDoer>().coreIdleAnimation;
            selectCharacter.spriteAnimator.Play(coreIdleAnimation);
        }

        private static void ResetToIdle(BraveBehaviour idler)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(idler.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(idler.sprite, Color.black);

            //var idle = idler.GetComponent<CharacterSelectIdleDoer>().coreIdleAnimation;
            //idler.sprite.SetSprite(idler.spriteAnimator.GetClipByName(idle).frames[0].spriteId);
            //idler.talkDoer.OnExitRange(null); 
        }

        private static void CreateOverheadCard(FoyerCharacterSelectFlag selectCharacter, CustomCharacterData data)
        {
            //Create new card instance
            selectCharacter.ClearOverheadElement();
            selectCharacter.OverheadElement = FakePrefab.Clone(selectCharacter.OverheadElement);
            selectCharacter.OverheadElement.SetActive(true);


            if (data.removeFoyerExtras)
            {
                foreach (var child in selectCharacter.gameObject.transform)
                {
                    if (((Transform)child).gameObject.name == "Doggy")
                    {
                        UnityEngine.Object.DestroyImmediate(((Transform)child).gameObject);
                    }
                }

                foreach (var phase in selectCharacter.gameObject.GetComponent<CharacterSelectIdleDoer>().phases)
                {
                    phase.vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE;
                    phase.endVFXSpriteAnimator = null;
                }

            }

            var customFoyerController = selectCharacter.gameObject.AddComponent<CustomCharacterFoyerController>();
            customFoyerController.customCharacterController = data.customCharacterController;
            customFoyerController.metaCost = data.metaCost;


            customFoyerController.useGlow = data.useGlow;
            customFoyerController.emissiveColor = data.emissiveColor;
            customFoyerController.emissiveColorPower = data.emissiveColorPower;
            customFoyerController.emissivePower = data.emissivePower;
            customFoyerController.emissiveThresholdSensitivity = data.emissiveThresholdSensitivity;

            string replaceKey = data.baseCharacter.ToString().ToUpper();
            if (data.baseCharacter == PlayableCharacters.Soldier)
                replaceKey = "MARINE";
            if (data.baseCharacter == PlayableCharacters.Pilot)
                replaceKey = "ROGUE";
            if (data.baseCharacter == PlayableCharacters.Eevee)
                replaceKey = "PARADOX";


            //Change text
            var infoPanel = selectCharacter.OverheadElement.GetComponent<FoyerInfoPanelController>();



            dfLabel nameLabel = infoPanel.textPanel.transform.Find("NameLabel").GetComponent<dfLabel>();
            nameLabel.Text = nameLabel.GetLocalizationKey().Replace(replaceKey, data.identity.ToString());
            BotsMod.BotsModule.Log(replaceKey, BotsMod.BotsModule.LOST_COLOR);
            dfLabel pastKilledLabel = infoPanel.textPanel.transform.Find("PastKilledLabel").GetComponent<dfLabel>();
            pastKilledLabel.Text = "(Past Killed)";
            pastKilledLabel.ProcessMarkup = true;
            pastKilledLabel.ColorizeSymbols = true;
            if (data.metaCost != 0)
            {
                pastKilledLabel.ModifyLocalizedText(pastKilledLabel.Text + " (" + data.metaCost.ToString() + "[sprite \"hbux_text_icon\"])");
            }


            infoPanel.itemsPanel.enabled = true;

            try
            {

                var spriteObject = FakePrefab.Clone(infoPanel.itemsPanel.GetComponentInChildren<dfSprite>().gameObject);

                //spriteObject.SetActive(false);
                var posList = new List<Vector3>();
                var locPosList = new List<Vector3>();


                foreach (var child in infoPanel.itemsPanel.GetComponentsInChildren<dfSprite>())
                { 

                    BotsMod.BotsModule.Log(child.name + " " + child.transform.position + " -- " + child.transform.localPosition);
                    posList.Add(child.transform.position);
                    locPosList.Add(child.transform.localPosition);
                    UnityEngine.Object.DestroyImmediate(child.gameObject);

                }

                for (int i = 0; i < data.loadoutSpriteNames.Count; i++)
                {

                    var sprite = FakePrefab.Clone(spriteObject).GetComponent<dfSprite>();
                    sprite.gameObject.SetActive(true);

                    sprite.SpriteName = data.loadoutSpriteNames[i];
                    sprite.Size = new Vector2(data.loadoutSprites[i].width * 3, data.loadoutSprites[i].height * 3);
                    sprite.Atlas = GameUIRoot.Instance.ConversationBar.portraitSprite.Atlas;


                    sprite.transform.parent = infoPanel.itemsPanel.transform;

                    infoPanel.itemsPanel.Controls.Add(sprite);
                    

                    sprite.transform.position = new Vector3(1 + ((i + 0.1f) * 0.1f), -((i + 0.1f) * 0.1f), 0);
                    sprite.transform.localPosition = new Vector3(((i + 0.1f) * 0.1f), 0, 0);

                    BotsMod.BotsModule.Log(data.loadoutSpriteNames[i] + sprite.transform.position + " -- " + sprite.transform.localPosition);

                }
                selectCharacter.CreateOverheadElement();

            }

            catch (Exception e)
            {
                BotsMod.BotsModule.Log("overhead thing broke: " + e, BotsMod.BotsModule.LOCKED_CHARACTOR_COLOR);
            }
            /*
            if (nameLabel.Text == "The Lost")
            {
                if (GameStatsManager.Instance.GetCharacterSpecificFlag(data.identity, CharacterSpecificGungeonFlags.KILLED_PAST))
                {
                    pastKilledLabel.Text = "(Past Killed)";
                } 
                else
                {
                    pastKilledLabel.Text = "(Past (Not) Killed)";
                }
                
                try
                {
                    Tools.ReplaceSpriteInAtlas("lost_sidearm_charselect_001", ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/lost_sidearm_charselect.png"), infoPanel.itemsPanel.Atlas.Texture, infoPanel.itemsPanel.Atlas.Items[0].region, infoPanel.itemsPanel.Atlas);
                    Tools.ReplaceSpriteInAtlas("lost_cloak_charselect_001", ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/lost_cloak_charselect.png"), infoPanel.itemsPanel.Atlas.Texture, infoPanel.itemsPanel.Atlas.Items[1].region, infoPanel.itemsPanel.Atlas);
                    Tools.ReplaceSpriteInAtlas("heart_full_purple_001", ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/heart_full_purple_001.png"), infoPanel.itemsPanel.Atlas.Texture, infoPanel.itemsPanel.Atlas.Items[2].region, infoPanel.itemsPanel.Atlas);
                    //Tools.ReplaceSpriteInAtlas("Sack", ResourceExtractor.GetTextureFromResource("FrostAndGunfireItems/Resources/char_select/sack_select.png"), infoPanel.itemsPanel.Atlas.Texture, infoPanel.itemsPanel.Atlas.Items[3].region, infoPanel.itemsPanel.Atlas);
                    var stupidlist = new List<dfSprite>();
                    for (int i = 0; i < infoPanel.itemsPanel.Controls.Count; i++)
                    {
                        stupidlist.Add(infoPanel.itemsPanel.Controls[i] as dfSprite);
                    }
                    stupidlist[2].SpriteName = "lost_sidearm_charselect_001";
                    stupidlist[2].Size = new Vector2(69, 39);

                    stupidlist[1].SpriteName = "lost_cloak_charselect_001";
                    stupidlist[1].Size = new Vector2(48, 63);

                    stupidlist[0].SpriteName = "heart_full_purple_001";
                    stupidlist[0].Size = new Vector2(78, 21);

                    //stupidlist[3].SpriteName = "Sack";
                    //stupidlist[3].Size = new Vector2(51, 48);
                }
                catch (Exception e)
                {
                    BotsMod.BotsModule.Log("stupid stupid Atlas: " + e, BotsMod.BotsModule.LOCKED_CHARACTOR_COLOR);
                }

            }
            */

            //Swap out face sprites
            if (data.foyerCardSprites != null)
            {
                var facecard = selectCharacter.OverheadElement.GetComponentInChildren<CharacterSelectFacecardIdleDoer>();
                var orig = facecard.sprite.Collection;
                var copyCollection = GameObject.Instantiate(orig);

                tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
                for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
                {
                    copyDefinitions[i] = orig.spriteDefinitions[i].Copy();
                }
                copyCollection.spriteDefinitions = copyDefinitions;

                for (int i = 0; i < data.foyerCardSprites.Count; i++)
                {
                    var tex = data.foyerCardSprites[i];
                    var def = copyCollection.GetSpriteDefinition(tex.name);

					if (def != null) {
						def.ReplaceTexture(tex);
						Console.WriteLine($"REPLACED {def.name} {def.material.mainTexture.width} {def.material.mainTexture.height}");
					}
                }
                facecard.sprite.Collection = copyCollection;

                facecard.spriteAnimator.Library = GameObject.Instantiate(facecard.spriteAnimator.Library);
                GameObject.DontDestroyOnLoad(facecard.spriteAnimator.Library);
                foreach (var clip in facecard.spriteAnimator.Library.clips)
                {
                    for (int i = 0; i < clip.frames.Length; i++)
                    {
                        clip.frames[i].spriteCollection = copyCollection;
                    }
                }

				Console.WriteLine($"FOYERCARDDBG");
				for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
				{
					var def = copyCollection.spriteDefinitions[i];
					Console.WriteLine(def.name);
					if (!def.name.Contains("facecard")) continue;

					Console.WriteLine("FACECARD");
					Console.WriteLine($"{def.material.mainTexture.width} {def.material.mainTexture.height} {def.uvs[0].x},{def.uvs[0].y} {def.uvs[1].x},{def.uvs[1].y} {def.uvs[2].x},{def.uvs[2].y} {def.uvs[3].x},{def.uvs[3].y} ");


				}
                
            }
            selectCharacter.OverheadElement.SetActive(false);
        }

        private static void OnPlayerCharacterChanged(PlayerController player, FoyerCharacterSelectFlag selectCharacter, string characterPath)
        {
            if (player.name.ToLower().Contains(characterPath))
            {
                Tools.Print("Selected: " + characterPath);
                if (selectCharacter.gameObject.activeSelf)
                {
                    selectCharacter.ClearOverheadElement();
                    selectCharacter.talkDoer.OnExitRange(null);
                    selectCharacter.gameObject.SetActive(false);
                    selectCharacter.GetComponent<SpeculativeRigidbody>().enabled = false;
                }
            }
            else if (!selectCharacter.gameObject.activeSelf)
            {
                selectCharacter.gameObject.SetActive(true);
                SpriteOutlineManager.RemoveOutlineFromSprite(selectCharacter.sprite, true);
                SpriteOutlineManager.AddOutlineToSprite(selectCharacter.sprite, Color.black);

                selectCharacter.specRigidbody.enabled = true;
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(selectCharacter.specRigidbody, null, false);

                CharacterSelectIdleDoer idleDoer = selectCharacter.GetComponent<CharacterSelectIdleDoer>();
                idleDoer.enabled = true;

            }
        }
    }
}
