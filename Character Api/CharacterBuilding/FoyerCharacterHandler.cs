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
using Dungeonator;

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
                    //selectCharacter.OverheadElement.SetActive(true);
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
            FakePrefab.MarkAsFakePrefab(selectFlag.gameObject, true);
            selectFlag.transform.position = foyerPositions[customCharacter.First.characterID];
            selectFlag.CharacterPrefabPath = characterPath;
            selectFlag.name = "NPC_FoyerCharacter_" + customCharacter.First.nameShort;
            Tools.Print("    Made select flag");

            //Replace sprites
            HandleSprites(selectFlag, customCharacter.Second.GetComponent<PlayerController>());
            Tools.Print("    Replaced sprites");

            var td = selectFlag.talkDoer;

            //Setup overhead card
            if (!string.IsNullOrEmpty(customCharacter.First.pathForSprites))
            {
                var idleDoer = selectFlag.gameObject.GetComponent<CharacterSelectIdleDoer>();

                idleDoer.AnimationLibraries = customCharacter.First.idleDoer.AnimationLibraries;
                idleDoer.coreIdleAnimation = customCharacter.First.idleDoer.coreIdleAnimation;
                idleDoer.onSelectedAnimation = customCharacter.First.idleDoer.onSelectedAnimation;
                idleDoer.EeveeTex = customCharacter.First.idleDoer.EeveeTex;
                idleDoer.idleMax = customCharacter.First.idleDoer.idleMax;
                idleDoer.idleMin = customCharacter.First.idleDoer.idleMin;
                idleDoer.IsEevee = customCharacter.First.idleDoer.IsEevee;
                idleDoer.phases = customCharacter.First.idleDoer.phases;
            }

            
            

            if (customCharacter.First.removeFoyerExtras)
            {
                foreach (var child in selectFlag.gameObject.transform)
                {
                    //wow look i did a peta and killed a dog for no reason
                    if (((Transform)child).gameObject.name == "Doggy")
                    {
                        UnityEngine.Object.DestroyImmediate(((Transform)child).gameObject);
                    }
                }
                foreach (var phase in selectFlag.gameObject.GetComponent<CharacterSelectIdleDoer>().phases)
                {
                    phase.vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE;
                    phase.endVFXSpriteAnimator = null;
                }
            }

            foreach (var thing in customCharacter.First.randomFoyerBullshitNNAskedFor)
            {
                UnityEngine.Object.Instantiate<GameObject>(thing.First, selectFlag.transform).transform.localPosition = thing.Second;
            }

            CreateOverheadCard(selectFlag, customCharacter.First);
            //FakePrefab.MarkAsFakePrefab(selectFlag.OverheadElement);
            ETGModConsole.Log(selectFlag.OverheadElement.ToString());
            td.OverheadUIElementOnPreInteract = selectFlag.OverheadElement;
            //FakePrefab.MarkAsFakePrefab(td.OverheadUIElementOnPreInteract);
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


            MakeSkinSwapper(customCharacter.First);
            Tools.Print("    Added swapper");

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

        static void MakeSkinSwapper(CustomCharacterData data)
        {
            var baseSwapper = FakePrefab.Clone(Foyer.Instance.transform.Find("Livery xform").Find("costume_guide").gameObject);
            var altSwapper = FakePrefab.Clone(Foyer.Instance.transform.Find("Livery xform").Find("costume_guide_alt").gameObject);

            var sprite = baseSwapper.GetComponent<tk2dSprite>();
            var altSprite = altSwapper.GetComponent<tk2dSprite>();

            baseSwapper.transform.parent = Foyer.Instance.transform.Find("Livery xform");
            altSwapper.transform.parent = Foyer.Instance.transform.Find("Livery xform");

            sprite.SetSprite(sprite.Collection, SpriteHandler.AddSpriteToCollection(data.altObjSprite1, sprite.Collection));
            altSprite.SetSprite(altSprite.Collection, SpriteHandler.AddSpriteToCollection(data.altObjSprite2, sprite.Collection));

            altSwapper.name = $"costume_{data.nameShort}_alt";

            baseSwapper.name = $"costume_{data.nameShort}";

            var characterCostumeSwapper = baseSwapper.GetComponent<CharacterCostumeSwapper>();

            characterCostumeSwapper.TargetCharacter = (PlayableCharacters)CustomPlayableCharacters.Custom;

            characterCostumeSwapper.AlternateCostumeSprite = altSprite;

            characterCostumeSwapper.CostumeSprite = sprite;

            characterCostumeSwapper.HasCustomTrigger = false;
            characterCostumeSwapper.CustomTriggerIsFlag = false;
            characterCostumeSwapper.TriggerFlag = GungeonFlags.NONE;
            characterCostumeSwapper.CustomTriggerIsSpecialReserve = false;

            characterCostumeSwapper.TargetLibrary = data.AlternateCostumeLibrary;

            if (sprite.transform == null)
            {
                ETGModConsole.Log("somehow the transform nulled... god is fucking dead and BraveBehaviours killed him");
            }

            if (altSprite.transform == null)
            {
                ETGModConsole.Log("somehow the transform nulled... god is fucking dead (again) and BraveBehaviours killed him");
            }

            baseSwapper.gameObject.SetActive(true);
            altSwapper.gameObject.SetActive(true);

            baseSwapper.transform.position = data.skinSwapperPos;

            altSwapper.transform.position = data.skinSwapperPos;
            

            BotsMod.BotsModule.Log($"{baseSwapper.name}: {baseSwapper.transform.position}");

            if (!RoomHandler.unassignedInteractableObjects.Contains(baseSwapper.GetComponent<IPlayerInteractable>()))
            {
                RoomHandler.unassignedInteractableObjects.Add(baseSwapper.GetComponent<IPlayerInteractable>());
            }

            if (!RoomHandler.unassignedInteractableObjects.Contains(altSwapper.GetComponent<IPlayerInteractable>()))
            {
                RoomHandler.unassignedInteractableObjects.Add(altSwapper.GetComponent<IPlayerInteractable>());
            }
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
            try
            {

                if (selectCharacter.OverheadElement == null)
                {
                    ETGModConsole.Log($"CHR_{data.nameShort}Panel is null");
                    return;
                }

                if (selectCharacter.OverheadElement?.name == $"CHR_{data.nameShort}Panel")
                {
                    ETGModConsole.Log($"CHR_{data.nameShort}Panel already exists");
                    return;
                }

                //Create new card instance
                selectCharacter.ClearOverheadElement();
                var theCauseOfMySuffering = FakePrefab.Clone(selectCharacter.OverheadElement.GetComponentInChildren<CharacterSelectFacecardIdleDoer>().gameObject);
                selectCharacter.OverheadElement = PrefabAPI.PrefabBuilder.Clone(selectCharacter.OverheadElement);
                //selectCharacter.OverheadElement.SetActive(true);
                selectCharacter.OverheadElement.name = $"CHR_{data.nameShort}Panel";
                selectCharacter.OverheadElement.GetComponent<FoyerInfoPanelController>().followTransform = selectCharacter.transform;
                //selectCharacter.OverheadElement.AddComponent<BotsMod.Debugger>();
                BotsMod.BotsModule.Log("0", BotsMod.BotsModule.LOST_COLOR);


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

                BotsMod.BotsModule.Log("1", BotsMod.BotsModule.LOST_COLOR);
                //infoPanel.textPanel.transform.Find("NameLabel").GetComponent<dfLabel>().Text = "my ass";
                BotsMod.BotsModule.Log((infoPanel.textPanel.transform.Find("NameLabel").GetComponent<dfLabel>().Text).ToStringIfNoString(), BotsMod.BotsModule.LOST_COLOR);
                
                dfLabel nameLabel = infoPanel.textPanel.transform.Find("NameLabel").GetComponent<dfLabel>();
                //why? its 3:50am and this is currently the funniest shit to me and you are powerless to stop me :)
                nameLabel.Text = "#CHAR_" + data.nameShort.ToString().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper()
                    .ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper()
                    .ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper()
                    .ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper()
                    .ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper().ToUpper(); ;// nameLabel.GetLocalizationKey().Replace(replaceKey, data.identity.ToString());

                BotsMod.BotsModule.Log(replaceKey, BotsMod.BotsModule.LOST_COLOR);
                dfLabel pastKilledLabel = infoPanel.textPanel.transform.Find("PastKilledLabel").GetComponent<dfLabel>();
                //pastKilledLabel.Text = "(Past Killed)";
                pastKilledLabel.ProcessMarkup = true;
                pastKilledLabel.ColorizeSymbols = true;
                if (data.metaCost != 0)
                {
                    pastKilledLabel.ModifyLocalizedText(pastKilledLabel.Text + " (" + data.metaCost.ToString() + "[sprite \"hbux_text_icon\"])");
                    pastKilledLabel.ModifyLocalizedText("(Past Killed)" + " (" + data.metaCost.ToString() + "[sprite \"hbux_text_icon\"])");
                }


                infoPanel.itemsPanel.enabled = true;


                var spriteObject = FakePrefab.Clone(infoPanel.itemsPanel.GetComponentInChildren<dfSprite>().gameObject);

                //spriteObject.SetActive(false);
                var posList = new List<Vector3>();
                var locPosList = new List<Vector3>();


                foreach (var child in infoPanel.itemsPanel.GetComponentsInChildren<dfSprite>())
                { 

                    //BotsMod.BotsModule.Log(child.name + " " + child.transform.position + " -- " + child.transform.localPosition);
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

                    //BotsMod.BotsModule.Log(data.loadoutSpriteNames[i] + sprite.transform.position + " -- " + sprite.transform.localPosition);

                }

                
                if (data.foyerCardSprites != null)
                {

                    var facecard = selectCharacter.OverheadElement.GetComponentInChildren<CharacterSelectFacecardIdleDoer>();
                    theCauseOfMySuffering.transform.parent = facecard.transform.parent;
                    theCauseOfMySuffering.transform.localScale = new Vector3(1, 1, 1);
                    theCauseOfMySuffering.transform.localPosition = new Vector3(0, 1.687546f, 0.2250061f);
                    theCauseOfMySuffering.transform.parent.localPosition = new Vector3(0, 0, 0);
                    theCauseOfMySuffering.transform.parent.localScale = new Vector3(0.2f, 0.2f, 1);
                    theCauseOfMySuffering.transform.parent.localScale = new Vector3(0.1975309f, 0.1975309f, 1);
                    //theCauseOfMySuffering.transform.localScale = Vector3.one;
                    //facecard.gameObject.SetActive(false);
                    facecard.transform.parent = null;
                    UnityEngine.Object.Destroy(facecard.gameObject);
                    facecard = theCauseOfMySuffering.GetComponent<CharacterSelectFacecardIdleDoer>();
                    facecard.gameObject.AddComponent<BotsMod.Debugger>();
                    facecard.gameObject.name = data.nameShort + " Sprite FaceCard";
                    //facecard.RegenerateCache();
                    

                    ETGModConsole.Log($"foyer cards arent null. {facecard.gameObject.transform.parent.position}");
                    ETGModConsole.Log($"foyer cards arent null. {facecard.gameObject.activeSelf}");

                    var orig = facecard.sprite.Collection;

                    var idleAnimName = $"{data.nameShort}_facecard_idle";
                    var appearAnimName = $"{data.nameShort}_facecard_appear";

                    List<int> idleAnimIds = new List<int>();
                    List<int> appearAnimIds = new List<int>();

                    List<int> toCopyAppearAnimIds = new List<int>
                    {
                        230,
                        231,
                        232,
                        233,
                        234,
                        235,
                        236,
                        237,
                        238,
                        239,
                        240,
                    };
                    List<int> toCopyIdleAnimIds = new List<int>
                    {
                        241,
                        242,
                        243,
                        244,
                    };

                    foreach (var sprite in data.foyerCardSprites)
                    {
                        if (sprite.name.ToLower().Contains("appear"))
                        {
                            appearAnimIds.Add(SpriteHandler.AddSpriteToCollectionWithAnchor(sprite, orig, tk2dBaseSprite.Anchor.LowerCenter, $"{data.nameShort}_{sprite.name}"));
                            
                        }
                        else if (sprite.name.ToLower().Contains("idle"))
                        {
                            idleAnimIds.Add(SpriteHandler.AddSpriteToCollectionWithAnchor(sprite, orig, tk2dBaseSprite.Anchor.LowerCenter, $"{data.nameShort}_{sprite.name}"));
                        }
                        ETGModConsole.Log(sprite.name);
                    }
                    /*
                    orig.spriteDefinitions[appearAnimIds[0]].position0 = new Vector3(-0.3f, 0, 0);
                    orig.spriteDefinitions[appearAnimIds[0]].position1 = new Vector3(0.3f, 0, 0);
                    orig.spriteDefinitions[appearAnimIds[0]].position2 = new Vector3(-0.3f, 2.4f, 0);
                    orig.spriteDefinitions[appearAnimIds[0]].position3 = new Vector3(0.3f, 2.4f, 0);

                    orig.spriteDefinitions[appearAnimIds[1]].position0 = new Vector3(-0.9f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[1]].position1 = new Vector3(0.8f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[1]].position2 = new Vector3(-0.9f, 2.4f, 0);
                    orig.spriteDefinitions[appearAnimIds[1]].position3 = new Vector3(0.8f, 2.4f, 0);

                    orig.spriteDefinitions[appearAnimIds[2]].position0 = new Vector3(-1.2f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[2]].position1 = new Vector3(1.2f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[2]].position2 = new Vector3(-1.2f, 2.5f, 0);
                    orig.spriteDefinitions[appearAnimIds[2]].position3 = new Vector3(1.2f, 2.5f, 0);

                    orig.spriteDefinitions[appearAnimIds[3]].position0 = new Vector3(-0.9f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[3]].position1 = new Vector3(0.8f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[3]].position2 = new Vector3(-0.9f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[3]].position3 = new Vector3(0.8f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[4]].position0 = new Vector3(-0.3f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[4]].position1 = new Vector3(0.3f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[4]].position2 = new Vector3(-0.3f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[4]].position3 = new Vector3(0.3f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[5]].position0 = new Vector3(-0.9f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[5]].position1 = new Vector3(0.8f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[5]].position2 = new Vector3(-0.9f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[5]].position3 = new Vector3(0.8f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[6]].position0 = new Vector3(-1.2f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[6]].position1 = new Vector3(1.2f, 0.3f, 0);
                    orig.spriteDefinitions[appearAnimIds[6]].position2 = new Vector3(-1.2f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[6]].position3 = new Vector3(1.2f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[7]].position0 = new Vector3(-0.9f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[7]].position1 = new Vector3(0.8f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[7]].position2 = new Vector3(-0.9f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[7]].position3 = new Vector3(0.8f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[8]].position0 = new Vector3(-0.3f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[8]].position1 = new Vector3(0.3f, 0.2f, 0);
                    orig.spriteDefinitions[appearAnimIds[8]].position2 = new Vector3(-0.3f, 2.6f, 0);
                    orig.spriteDefinitions[appearAnimIds[8]].position3 = new Vector3(0.3f, 2.6f, 0);

                    orig.spriteDefinitions[appearAnimIds[9]].position0 = new Vector3(-0.9f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[9]].position1 = new Vector3(0.8f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[9]].position2 = new Vector3(-0.9f, 2.5f, 0);
                    orig.spriteDefinitions[appearAnimIds[9]].position3 = new Vector3(0.8f, 2.5f, 0);

                    orig.spriteDefinitions[appearAnimIds[10]].position0 = new Vector3(-1.2f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[10]].position1 = new Vector3(1.2f, 0.1f, 0);
                    orig.spriteDefinitions[appearAnimIds[10]].position2 = new Vector3(-1.2f, 2.4f, 0);
                    orig.spriteDefinitions[appearAnimIds[10]].position3 = new Vector3(1.2f, 2.4f, 0);
                    
                    for(int i = 11; i < appearAnimIds.Count; i++)
                    {
                        orig.spriteDefinitions[appearAnimIds[i]].position0 = new Vector3(-1.2f, 0.1f, 0);
                        orig.spriteDefinitions[appearAnimIds[i]].position1 = new Vector3(1.2f, 0.1f, 0);
                        orig.spriteDefinitions[appearAnimIds[i]].position2 = new Vector3(-1.2f, 2.4f, 0);
                        orig.spriteDefinitions[appearAnimIds[i]].position3 = new Vector3(1.2f, 2.4f, 0);
                    }

                    for (int i = 5; i < idleAnimIds.Count; i++)
                    {
                        orig.spriteDefinitions[idleAnimIds[i]].position0 = new Vector3(-1.2f, 0.1f, 0);
                        orig.spriteDefinitions[idleAnimIds[i]].position1 = new Vector3(1.2f, 0.1f, 0);
                        orig.spriteDefinitions[idleAnimIds[i]].position2 = new Vector3(-1.2f, 2.4f, 0);
                        orig.spriteDefinitions[idleAnimIds[i]].position3 = new Vector3(1.2f, 2.4f, 0);
                    }

                    var mat = new Material(orig.spriteDefinitions[0].material);*/
                    for (int i = 0; i < appearAnimIds.Count; i++)
                    {
                        orig.spriteDefinitions[appearAnimIds[i]].position0 = orig.spriteDefinitions[toCopyAppearAnimIds[i]] == null ? orig.spriteDefinitions[toCopyAppearAnimIds[10]].position0 : orig.spriteDefinitions[toCopyAppearAnimIds[i]].position0;
                        orig.spriteDefinitions[appearAnimIds[i]].position1 = orig.spriteDefinitions[toCopyAppearAnimIds[i]] == null ? orig.spriteDefinitions[toCopyAppearAnimIds[10]].position1 : orig.spriteDefinitions[toCopyAppearAnimIds[i]].position1;
                        orig.spriteDefinitions[appearAnimIds[i]].position2 = orig.spriteDefinitions[toCopyAppearAnimIds[i]] == null ? orig.spriteDefinitions[toCopyAppearAnimIds[10]].position2 : orig.spriteDefinitions[toCopyAppearAnimIds[i]].position2;
                        orig.spriteDefinitions[appearAnimIds[i]].position3 = orig.spriteDefinitions[toCopyAppearAnimIds[i]] == null ? orig.spriteDefinitions[toCopyAppearAnimIds[10]].position3 : orig.spriteDefinitions[toCopyAppearAnimIds[i]].position3;

                        /*
                        var safeForLaterName = orig.spriteDefinitions[appearAnimIds[i]].name;
                        var safeForLaterBoundsDataCenter = orig.spriteDefinitions[appearAnimIds[i]].boundsDataCenter;
                        var safeForLaterBoundsDataExtents = orig.spriteDefinitions[appearAnimIds[i]].boundsDataExtents;
                        var safeForLaterUntrimmedBoundsDataCenter = orig.spriteDefinitions[appearAnimIds[i]].untrimmedBoundsDataCenter;
                        var safeForLaterUntrimmedBoundsDataExtents = orig.spriteDefinitions[appearAnimIds[i]].untrimmedBoundsDataExtents;
                        var safeForLaterUv = orig.spriteDefinitions[appearAnimIds[i]].uvs;

                        

                        def.name = safeForLaterName;
                        def.boundsDataCenter = safeForLaterBoundsDataCenter;
                        def.boundsDataExtents = safeForLaterBoundsDataExtents;
                        def.untrimmedBoundsDataCenter = safeForLaterUntrimmedBoundsDataCenter;
                        def.untrimmedBoundsDataExtents = safeForLaterUntrimmedBoundsDataExtents;
                        def.uvs = safeForLaterUv;

                        orig.spriteDefinitions[appearAnimIds[i]] = def;*/
                    }

                    for (int i = 0; i < idleAnimIds.Count; i++)
                    {

                        orig.spriteDefinitions[idleAnimIds[i]].position0 = orig.spriteDefinitions[toCopyIdleAnimIds[i]] == null ? orig.spriteDefinitions[toCopyIdleAnimIds[10]].position0 : orig.spriteDefinitions[toCopyIdleAnimIds[i]].position0;
                        orig.spriteDefinitions[idleAnimIds[i]].position1 = orig.spriteDefinitions[toCopyIdleAnimIds[i]] == null ? orig.spriteDefinitions[toCopyIdleAnimIds[10]].position1 : orig.spriteDefinitions[toCopyIdleAnimIds[i]].position1;
                        orig.spriteDefinitions[idleAnimIds[i]].position2 = orig.spriteDefinitions[toCopyIdleAnimIds[i]] == null ? orig.spriteDefinitions[toCopyIdleAnimIds[10]].position2 : orig.spriteDefinitions[toCopyIdleAnimIds[i]].position2;
                        orig.spriteDefinitions[idleAnimIds[i]].position3 = orig.spriteDefinitions[toCopyIdleAnimIds[i]] == null ? orig.spriteDefinitions[toCopyIdleAnimIds[10]].position3 : orig.spriteDefinitions[toCopyIdleAnimIds[i]].position3;

                        /*
                        var safeForLaterName = orig.spriteDefinitions[idleAnimIds[i]].name;
                        var safeForLaterBoundsDataCenter = orig.spriteDefinitions[idleAnimIds[i]].boundsDataCenter;
                        var safeForLaterBoundsDataExtents = orig.spriteDefinitions[idleAnimIds[i]].boundsDataExtents;
                        var safeForLaterUntrimmedBoundsDataCenter = orig.spriteDefinitions[idleAnimIds[i]].untrimmedBoundsDataCenter;
                        var safeForLaterUntrimmedBoundsDataExtents = orig.spriteDefinitions[idleAnimIds[i]].untrimmedBoundsDataExtents;
                        var safeForLaterUv = orig.spriteDefinitions[idleAnimIds[i]].uvs;

                       

                        def.name = safeForLaterName;
                        def.boundsDataCenter = safeForLaterBoundsDataCenter;
                        def.boundsDataExtents = safeForLaterBoundsDataExtents;
                        def.untrimmedBoundsDataCenter = safeForLaterUntrimmedBoundsDataCenter;
                        def.untrimmedBoundsDataExtents = safeForLaterUntrimmedBoundsDataExtents;
                        def.uvs = safeForLaterUv;

                        orig.spriteDefinitions[idleAnimIds[i]] = def;*/
                    }


                    foreach (var def in orig.spriteDefinitions)
                    {
                        if (def.name.ToLower().Contains("appear") || def.name.ToLower().Contains("idle"))
                        {
                            //ETGModConsole.Log($"{def.name} [{orig.GetSpriteIdByName(def.name)}]: {def.position0} - {def.position1} - {def.position2} - {def.position3}");
                        }
                    }
                    facecard.gameObject.SetActive(true);
                    facecard.spriteAnimator = facecard.gameObject.GetComponent<tk2dSpriteAnimator>();

                    SpriteBuilder.AddAnimation(facecard.spriteAnimator, orig, idleAnimIds, idleAnimName, tk2dSpriteAnimationClip.WrapMode.Loop, 4);
                    var name = SpriteBuilder.AddAnimation(facecard.spriteAnimator, orig, appearAnimIds, appearAnimName, tk2dSpriteAnimationClip.WrapMode.Once, 17);

                    facecard.spriteAnimator.DefaultClipId = facecard.spriteAnimator.Library.GetClipIdByName(appearAnimName);

                    foreach(var anim in facecard.spriteAnimator.Library.clips)
                    {
                        ETGModConsole.Log($"{anim.name}: {anim.frames.Length}");
                    }

                    facecard.appearAnimation = appearAnimName;
                    facecard.coreIdleAnimation = idleAnimName;
                }

                    //selectCharacter.CreateOverheadElement();

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
 

                /*
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


				}*/

            
            //selectCharacter.OverheadElement.SetActive(false);
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
