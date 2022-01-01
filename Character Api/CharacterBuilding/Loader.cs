using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using StatType = PlayerStats.StatType;
using UnityEngine;
using GungeonAPI;
using Ionic.Zip;

using BotsMod;
using SaveAPI;
using System.Reflection;
using ItemAPI;

namespace CustomCharacters
{
    /*
     * Loads all the character data from the characterdata.txt
     * and then ships it off to CharacterBuilder
     */
    public static class Loader
    {
        public static string CharacterDirectory = BotsModule.characterFilePath;
        public static string DataFile = "characterdata.txt";

        public static List<CustomCharacterData> characterData = new List<CustomCharacterData>();

        /*public static void Init()
        {
            try
            {
                if (!Directory.Exists(CharacterDirectory))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(CharacterDirectory);
                    ETGModConsole.Log("Created directory: " + dir.FullName);
                }
            }
            catch (Exception e)
            {
                ToolsGAPI.PrintError("Error creating custom character directory");
                ToolsGAPI.PrintException(e);
            }

            LoadCharacterData();
            foreach (CustomCharacterData data in characterData)
            {
                bool success = true;
                try
                {
                    CharacterBuilder.BuildCharacter(data);
                }
                catch (Exception e)
                {
                    success = false;
                    ToolsGAPI.PrintError("An error occured while creating the character: " + data.name);
                    ToolsGAPI.PrintException(e);
                }
                if (success)
                    ToolsGAPI.Print("Built prefab for: " + data.name);
            }
        }*/
        public static void AddPhase(string character, CharacterSelectIdlePhase phase)
        {
            character = character.ToLower();
            BotsModule.Log(CharacterBuilder.storedCharacters.GetFirst().Key);
            if (CharacterBuilder.storedCharacters.ContainsKey(character))
            {
                var arraysSuckBalls = CharacterBuilder.storedCharacters[character].First.idleDoer.phases.ToList();

                arraysSuckBalls.Add(phase);

                CharacterBuilder.storedCharacters[character].First.idleDoer.phases = arraysSuckBalls.ToArray();
            }
            else
            {
                ETGModConsole.Log($"No character found under the name \"{character}\" or tk2dSpriteAnimator is null");
            }
        }

        public static tk2dSpriteAnimationClip GetAnimation(string character, string animation)
        {
            character = character.ToLower();
            if (CharacterBuilder.storedCharacters.ContainsKey(character))
            {
                var library = CharacterBuilder.storedCharacters[character].Second.GetComponent<PlayerController>().spriteAnimator.Library;
                if (library.GetClipByName(animation) != null)
                {
                    return library.GetClipByName(animation);
                }
            }
            else
            {
                ETGModConsole.Log($"No character found under the name \"{character}\" or tk2dSpriteAnimator is null");
            }
            return null;
        }

        public static void AddFoyerObject(string character, GameObject obj, Vector2 offset)
        {
            character = character.ToLower();
            if (CharacterBuilder.storedCharacters.ContainsKey(character))
            {
                CharacterBuilder.storedCharacters[character].First.randomFoyerBullshitNNAskedFor.Add(new Tuple<GameObject, Vector2>(obj, offset));
            }
            else
            {
                ETGModConsole.Log($"No character found under the name \"{character}\" or tk2dSpriteAnimator is null");
            }
        }


        public static void SetupCustomBreachAnimation(string character, string animation, int fps, tk2dSpriteAnimationClip.WrapMode wrapMode, int loopStart = 0, float maxFidgetDuration = 0, float minFidgetDuration = 0)
        {
            character = character.ToLower();
            BotsModule.Log(CharacterBuilder.storedCharacters.GetFirst().Key);
            if (CharacterBuilder.storedCharacters.ContainsKey(character) && CharacterBuilder.storedCharacters[character].Second.GetComponent<PlayerController>().spriteAnimator != null)
            {
                var library = CharacterBuilder.storedCharacters[character].Second.GetComponent<PlayerController>().spriteAnimator.Library;
                if (library.GetClipByName(animation) != null)
                {
                    library.GetClipByName(animation).fps = fps;
                    library.GetClipByName(animation).wrapMode = wrapMode;
                    if (wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
                    {
                        library.GetClipByName(animation).loopStart = loopStart;
                    }
                    if (wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopFidget)
                    {
                        library.GetClipByName(animation).maxFidgetDuration = maxFidgetDuration;
                        library.GetClipByName(animation).minFidgetDuration = minFidgetDuration;
                    }
                }
                else
                {
                    ETGModConsole.Log($"No animation found under the name \"{animation}\"");
                }
            }
            else
            {
                ETGModConsole.Log($"No character found under the name \"{character}\" or tk2dSpriteAnimator is null");
            }

        }


        public static void SetupCustomAnimation(string character, string animation, int fps, tk2dSpriteAnimationClip.WrapMode wrapMode, int loopStart = 0)
        {
            SetupCustomBreachAnimation(character, animation, fps, wrapMode, loopStart);
            /*character = character.ToLower();
            BotsModule.Log(CharacterBuilder.storedCharacters.GetFirst().Key);
            if (CharacterBuilder.storedCharacters.ContainsKey(character) && CharacterBuilder.storedCharacters[character].Second.GetComponent<PlayerController>().spriteAnimator != null)
            {
                var library = CharacterBuilder.storedCharacters[character].Second.GetComponent<PlayerController>().spriteAnimator.Library;
                if (library.GetClipByName(animation) != null)
                {
                    library.GetClipByName(animation).fps = fps;
                    library.GetClipByName(animation).wrapMode = wrapMode;
                    if (wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
                    {
                        library.GetClipByName(animation).loopStart = loopStart;
                    }
                } 
                else
                {
                    ETGModConsole.Log($"No animation found under the name \"{animation}\"");
                }
                
            }
            else
            {
                ETGModConsole.Log($"No character found under the name \"{character}\" or tk2dSpriteAnimator is null");
            }*/

        }

        public static CustomCharacterData BuildCharacter(string filePath, bool hasAltSkin, Vector3 altSwapperPos, bool removeFoyerExtras, bool paradoxUsesSprites, bool useGlow, Color emissiveColor, float emissiveColorPower, float emissivePower, int metaCost = 0, bool hasCustomPast = false, string customPast = "")
        {
            ETGModConsole.Log(BotsModule.FilePath);
            ETGModConsole.Log(BotsModule.ZipFilePath);

            var data = GetCharacterData(filePath);

            data.idleDoer = new CharacterSelectIdleDoer
            {
                onSelectedAnimation = "select_choose",
                coreIdleAnimation = "select_idle",
                idleMax = 10,
                idleMin = 4,
                EeveeTex = null,
                IsEevee = false,
                AnimationLibraries = new tk2dSpriteAnimation[0],
                phases = new CharacterSelectIdlePhase[0],

            };

            BotsModule.Log(data.nameInternal);
            data.atlas = GameUIRoot.Instance.ConversationBar.portraitSprite.Atlas;//obj.GetOrAddComponent<dfAtlas>();




            data.skinSwapperPos = altSwapperPos;
            bool success = true;
            try
            {
                CharacterBuilder.BuildCharacter(data, hasAltSkin, paradoxUsesSprites, removeFoyerExtras, hasCustomPast, customPast, metaCost, useGlow, emissiveColor, emissiveColorPower, emissivePower);
            }
            catch (Exception e)
            {
                success = false;
                ToolsGAPI.PrintError("An error occured while creating the character: " + data.name);
                ToolsGAPI.PrintException(e);
            }
            if (success)
            {
                ToolsGAPI.Print("Built prefab for: " + data.name);
                return data;
            }
            return null;
        }

        //Finds sprite folders, sprites, and characterdata.txt (and parses it)
        public static void LoadCharacterData()
        {
            //LoadDirectories();
            //LoadZips();
        }        

        private static CustomCharacterData GetCharacterData(string filePath)
        {

            filePath = filePath.Replace("/", ".").Replace("\\", ".");

            ToolsGAPI.StartTimer("Loading data for " + Path.GetFileName(filePath));
            ToolsGAPI.Print("");
            ToolsGAPI.Print("--Loading " + Path.GetFileName(filePath) + "--", "0000FF");
            //string customCharacterDir = Path.Combine(CharacterDirectory, filePath).Replace("/", ".").Replace("\\", ".");
            string dataFilePath = Path.Combine(filePath, "characterdata.txt").Replace("/", ".").Replace("\\", ".");


            var assembly = Assembly.GetCallingAssembly();
            var lines = new string[0];

            using (Stream stream = assembly.GetManifestResourceStream(dataFilePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                var linesList = new List<string>();
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    linesList.Add(line);
                }
                //ToolsGAPI.PrintError(linesList.Count().ToString());
                lines = linesList.ToArray();
            }



            if (lines.Count() <= 0)
            {
                ToolsGAPI.PrintError($"No \"{DataFile}\" file found for " + Path.GetFileName(filePath));
                return null;
            }
            

            //var lines = GungeonAPI.ResourceExtractor.GetLinesFromFile(dataFilePath);
            var data = ParseCharacterData(lines);

            string spritesDir = Path.Combine(filePath, "sprites").Replace("/", ".").Replace("\\", ".");
            string newSpritesDir = Path.Combine(filePath, "newspritesetup").Replace("/", ".").Replace("\\", ".");
            string newAltSpritesDir = Path.Combine(filePath, "newaltspritesetup").Replace("/", ".").Replace("\\", ".");

            string[] resources = GungeonAPI.ResourceExtractor.GetResourceNames();
            
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i].Contains(filePath))
                {

                    if (resources[i].StartsWith(spritesDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && data.sprites == null)
                    {
                        ToolsGAPI.PrintError("Found: Sprites folder");
                        data.sprites = GungeonAPI.ResourceExtractor.GetTexturesFromResource(spritesDir);
                    }

                    string altSpritesDir = Path.Combine(filePath, "alt_sprites").Replace("/", ".").Replace("\\", ".");

                    if (resources[i].StartsWith(altSpritesDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && data.altSprites == null)
                    {
                        ToolsGAPI.PrintError("Found: Alt Sprites folder");
                        data.altSprites = GungeonAPI.ResourceExtractor.GetTexturesFromResource(altSpritesDir);
                    }

                    if (resources[i].StartsWith(newSpritesDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(data.pathForSprites))
                    {
                        ToolsGAPI.PrintError("Found: New Sprites folder");
                        data.pathForSprites = newSpritesDir;
                    }

                    if (resources[i].StartsWith(newAltSpritesDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(data.pathForAltSprites))
                    {
                        ToolsGAPI.PrintError("Found: New Sprites folder");
                        data.pathForAltSprites = newAltSpritesDir;
                    }

                    string foyerDir = Path.Combine(filePath, "foyercard").Replace("/", ".").Replace("\\", ".");
                    if (resources[i].StartsWith(foyerDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && data.foyerCardSprites == null)
                    {
                        ToolsGAPI.PrintError("Found: Foyer card folder");
                        data.foyerCardSprites = GungeonAPI.ResourceExtractor.GetTexturesFromResource(foyerDir);
                    }
                   

                    string loadoutDir = Path.Combine(filePath, "loadoutsprites").Replace("/", ".").Replace("\\", ".");
                    if (resources[i].StartsWith(loadoutDir.Replace('/', '.'), StringComparison.OrdinalIgnoreCase) && data.loadoutSprites == null)
                    {
                        ToolsGAPI.PrintError("Found: Loadout card folder");

                        data.loadoutSprites = GungeonAPI.ResourceExtractor.GetTexturesFromResource(loadoutDir);

                        ToolsGAPI.PrintError(data.loadoutSprites.Count.ToString());
                    }

                }
            }


            ToolsGAPI.PrintError("new sprites");

            ToolsGAPI.PrintError("alt sprites");
            ToolsGAPI.PrintError("foyer card sprites");

            ToolsGAPI.PrintError("loadout sprites");
            List<Texture2D> miscTextures = GungeonAPI.ResourceExtractor.GetTexturesFromResource(filePath);
            foreach (var tex in miscTextures)
            {
                string name = tex.name.ToLower();
                if (name.Equals("icon"))
                {
                    ToolsGAPI.PrintError("Found: Icon ");
                    data.minimapIcon = tex;
                }
                if (name.Contains("bosscard_"))
                {
                    ToolsGAPI.PrintError("Found: Bosscard");
                    //BotsModule.Log(name.ToLower().Replace("bosscard_", "").Replace("0", ""));
                    data.bossCard.Add(tex);
                }
                if (name.Equals("playersheet"))
                {
                    ToolsGAPI.PrintError("Found: Playersheet");
                    data.playerSheet = tex;
                }
                if (name.Equals("facecard"))
                {
                    ToolsGAPI.PrintError("Found: Facecard");
                    data.faceCard = tex;
                }
                if (name.Equals("win_pic_junkan"))
                {
                    ToolsGAPI.PrintError("Found: Junkan Win Pic");
                    data.junkanWinPic = tex;
                }
                if (name.Equals("win_pic"))
                {
                    ToolsGAPI.PrintError("Found: Past Win Pic");
                    data.pastWinPic = tex;
                }

                if (name.Equals("alt_skin_obj_sprite_001"))
                {
                    ToolsGAPI.PrintError("Found: alt_skin_obj_sprite_001");
                    data.altObjSprite1 = tex;
                }
                if (name.Equals("alt_skin_obj_sprite_002"))
                {
                    ToolsGAPI.PrintError("Found: alt_skin_obj_sprite_002");
                    data.altObjSprite2 = tex;
                }


            }
            ToolsGAPI.PrintError("other sprites");
            string punchoutDir = Path.Combine(filePath, "punchout/").Replace("/", ".").Replace("\\", ".");

            string punchoutSpritesDir = Path.Combine(punchoutDir, "sprites").Replace("/", ".").Replace("\\", ".");
            if (resources.Contains(punchoutSpritesDir))
            {
                ToolsGAPI.Print("Found: Punchout Sprites folder");
                data.punchoutSprites = GungeonAPI.ResourceExtractor.GetTexturesFromDirectory(punchoutSpritesDir);
            }

            if (resources.Contains(punchoutDir))
            {
                data.punchoutFaceCards = new List<Texture2D>();
                var punchoutSprites = GungeonAPI.ResourceExtractor.GetTexturesFromDirectory(punchoutDir);
                foreach (var tex in punchoutSprites)
                {
                    string name = tex.name.ToLower();
                    if (name.Contains("facecard1") || name.Contains("facecard2") || name.Contains("facecard3"))
                    {
                        data.punchoutFaceCards.Add(tex);
                        ToolsGAPI.Print("Found: Punchout facecard " + tex.name);
                    }
                }
            }
                
            //ToolsGAPI.StopTimerAndReport("Loading data for " + Path.GetFileName(directories[i]));

            return data;
            
        }       

        //Main parse loop
        public static CustomCharacterData ParseCharacterData(string[] lines)
        {
            CustomCharacterData data = new CustomCharacterData();
            
            for (int i = 0; i < lines.Length; i++)
            {
                
                string line = lines[i].ToLower().Trim();
                string lineCaseSensitive = lines[i].Trim();

                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("#")) continue;

                if (line.StartsWith("<loadout>"))
                {
                    data.loadout = GetLoadout(lines, i + 1, out i);
                    continue;
                }

                if (line.StartsWith("<altguns>"))
                {
                    ToolsGAPI.PrintError("alt guns found");
                    data.altGun = GetAltguns(lines, i + 1, out i);
                    continue;
                }

                if (line.StartsWith("<stats>"))
                {
                    data.stats = GetStats(lines, i + 1, out i);
                    continue;
                }

                int dividerIndex = line.IndexOf(':');
                if (dividerIndex < 0) continue;


                string value = lineCaseSensitive.Substring(dividerIndex + 1).Trim();
                if (line.StartsWith("base:"))
                {
                    data.baseCharacter = GetCharacterFromString(value);
                    if (data.baseCharacter == PlayableCharacters.Robot)
                        data.armor = 6;
                    continue;
                }
                if (line.StartsWith("name:"))
                {
                    data.name = value;
                    continue;
                }


                /*if (line.StartsWith("unlock:"))
                {
                    data.unlockFlag = GetFlagFromString(value);
                    //BotsModule.Log(data.unlockFlag+"", BotsModule.LOST_COLOR);
                    continue;
                }*/


               
                if (line.StartsWith("name short:"))
                {
                    data.nameShort = value.Replace(" ", "_");
                    data.nameInternal = "Player" + data.nameShort;
                    continue;
                }
                if (line.StartsWith("nickname:"))
                {
                    data.nickname = value;
                    continue;
                }
                if (line.StartsWith("identity:"))
                {
                    data.identity = (PlayableCharacters)GetIdentityFromString(value);
                    BotsModule.Log(data.identity + "", BotsModule.LOST_COLOR);
                    continue;
                }
                if (line.StartsWith("armor:"))
                {
                    float floatValue;
                    if (!float.TryParse(value, out floatValue))
                    {
                        ToolsGAPI.PrintError("Invalid armor value: " + line);
                        continue;
                    }
                    data.armor = floatValue;
                    continue;
                }

                ToolsGAPI.PrintError($"Line {i} in {DataFile} did not meet any expected criteria:");
                ToolsGAPI.PrintRaw("----" + line, true);
            }
            return data;
        }

        public static CustomPlayableCharacters GetIdentityFromString(string characterName)
        {
            characterName = characterName.ToLower();
            foreach (CustomPlayableCharacters character in Enum.GetValues(typeof(CustomPlayableCharacters)))
            {
                var name = character.ToString().ToLower().Replace("coop", "");
                if (name.Equals(characterName))
                {
                    return character;
                }
            }

            ToolsGAPI.Print("Failed to find character: " + characterName);
            return CustomPlayableCharacters.Pilot;
        }

        //Character name aliasing
        public static PlayableCharacters GetCharacterFromString(string characterName)
        {
            characterName = characterName.ToLower();
            foreach (PlayableCharacters character in Enum.GetValues(typeof(PlayableCharacters)))
            {
                var name = character.ToString().ToLower().Replace("coop", "");
                if (name.Equals(characterName))
                {
                    return character;
                }
            }

            if (characterName.Equals("marine"))
                return PlayableCharacters.Soldier;
            if (characterName.Equals("hunter"))
                return PlayableCharacters.Guide;
            if (characterName.Equals("paradox"))
                return PlayableCharacters.Eevee;

            ToolsGAPI.Print("Failed to find character base: " + characterName);
            return PlayableCharacters.Pilot;
        }

        public static CustomDungeonFlags GetFlagFromString(string flagName)
        {
            //flagName = flagName.ToLower();
            foreach (CustomDungeonFlags flag in Enum.GetValues(typeof(CustomDungeonFlags)))
            {
                var name = flag.ToString();
                //BotsModule.Log(name + " / " + flagName, BotsModule.LOCKED_CHARACTOR_COLOR);
                //BotsModule.Log("ahhh: " + name.Equals(flagName), BotsModule.LOCKED_CHARACTOR_COLOR);
                if (name.Equals(flagName))
                {
                    BotsModule.Log("flag: " + flag, BotsModule.LOCKED_CHARACTOR_COLOR);
                    return flag;

                }
            }

            BotsModule.Log("Failed to find flag: " + flagName, BotsModule.LOCKED_CHARACTOR_COLOR);
            return CustomDungeonFlags.NONE;
        }

        //Stats
        public static Dictionary<StatType, float> GetStats(string[] lines, int startIndex, out int endIndex)
        {
            endIndex = startIndex;

            Dictionary<PlayerStats.StatType, float> stats = new Dictionary<PlayerStats.StatType, float>();
            string line;
            string[] args;
            for (int i = startIndex; i < lines.Length; i++)
            {
                endIndex = i;
                line = lines[i].ToLower().Trim();
                if (line.StartsWith("</stats>")) return stats;

                args = line.Split(':');
                if (args.Length == 0) continue;
                if (string.IsNullOrEmpty(args[0])) continue;
                if (args.Length < 2)
                {
                    ToolsGAPI.PrintError("Invalid stat line: " + line);
                    continue;
                }

                StatType stat = StatType.Accuracy;
                bool foundStat = false;
                foreach (StatType statType in Enum.GetValues(typeof(StatType)))
                {
                    if (statType.ToString().ToLower().Equals(args[0].ToLower()))
                    {
                        stat = statType;
                        foundStat = true;
                        break;
                    }
                }
                if (!foundStat)
                {
                    ToolsGAPI.PrintError("Unable to find stat: " + line);
                    continue;
                }

                float value;
                bool foundValue = float.TryParse(args[1].Trim(), out value);
                if (!foundValue)
                {
                    ToolsGAPI.PrintError("Invalid stat value: " + line);
                    continue;
                }

                stats.Add(stat, value);
            }
            ToolsGAPI.PrintError("Invalid stats setup, expecting '</stats>' but found none");
            return new Dictionary<StatType, float>();
        }

        //Loadout
        public static List<Tuple<PickupObject, bool>> GetLoadout(string[] lines, int startIndex, out int endIndex)
        {
            endIndex = startIndex;

            ToolsGAPI.Print("Getting loadout...");
            List<Tuple<PickupObject, bool>> items = new List<Tuple<PickupObject, bool>>();

            string line;
            string[] args;
            for (int i = startIndex; i < lines.Length; i++)
            {
                endIndex = i;
                line = lines[i].ToLower().Trim();
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("</loadout>")) return items;

                args = line.Split(' ');
                if (args.Length == 0) continue;

                if (!Gungeon.Game.Items.ContainsID(args[0]))
                {
                    ToolsGAPI.PrintError("Could not find item with ID: \"" + args[0] + "\"");
                    continue;
                }
                var item = Gungeon.Game.Items[args[0]];
                if (item == null)
                {
                    ToolsGAPI.PrintError("Could not find item with ID: \"" + args[0] + "\"");
                    continue;
                }

                if (args.Length > 1 && args[1].Contains("infinite"))
                {
                    var gun = item.GetComponent<Gun>();

                    if (gun != null)
                    {
                        if (!CharacterBuilder.guns.Contains(gun) && !gun.InfiniteAmmo)
                            CharacterBuilder.guns.Add(gun);

                        items.Add(new Tuple<PickupObject, bool>(item, true));
                        ToolsGAPI.Print("    " + item.EncounterNameOrDisplayName + " (infinite)");
                        continue;
                    }
                    else
                    {
                        ToolsGAPI.PrintError(item.EncounterNameOrDisplayName + " is not a gun, and therefore cannot be infinite");
                    }
                }
                else
                {
                    items.Add(new Tuple<PickupObject, bool>(item, false));
                    ToolsGAPI.Print("    " + item.EncounterNameOrDisplayName);
                }

            }

            ToolsGAPI.PrintError("Invalid loadout setup, expecting '</loadout>' but found none");
            return new List<Tuple<PickupObject, bool>>();
        }

        public static List<Tuple<PickupObject, bool>> GetAltguns(string[] lines, int startIndex, out int endIndex)
        {
            endIndex = startIndex;

            ToolsGAPI.Print("altguns loadout...");
            List<Tuple<PickupObject, bool>> items = new List<Tuple<PickupObject, bool>>();
            ToolsGAPI.PrintError("go fuck yourself");
            string line;
            string[] args;
            for (int i = startIndex; i < lines.Length; i++)
            {
                endIndex = i;
                line = lines[i].ToLower().Trim();
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("</altguns>")) return items;

                args = line.Split(' ');
                if (args.Length == 0) continue;

                if (!Gungeon.Game.Items.ContainsID(args[0]))
                {
                    ToolsGAPI.PrintError("Could not find item with ID: \"" + args[0] + "\"");
                    continue;
                }
                var item = Gungeon.Game.Items[args[0]];
                if (item == null)
                {
                    ToolsGAPI.PrintError("Could not find item with ID: \"" + args[0] + "\"");
                    continue;
                }
                var gun = item.GetComponent<Gun>();

                if (gun == null)
                {
                    ToolsGAPI.PrintError("\"" + args[0] + "\" isn't a gun...");
                    continue;
                }

                if (args.Length > 1 && args[1].Contains("infinite"))
                {
                    

                    if (gun != null)
                    {
                        if (!CharacterBuilder.guns.Contains(gun) && !gun.InfiniteAmmo)
                            CharacterBuilder.guns.Add(gun);

                        items.Add(new Tuple<PickupObject, bool>(item, true));
                        ToolsGAPI.Print("    " + item.EncounterNameOrDisplayName + " (infinite)");
                        continue;
                    }
                    else
                    {
                        ToolsGAPI.PrintError(item.EncounterNameOrDisplayName + " is not a gun, and therefore cannot be infinite");
                    }
                }
                else
                {
                    items.Add(new Tuple<PickupObject, bool>(item, false));
                    ToolsGAPI.Print("    " + item.EncounterNameOrDisplayName);
                }

            }

            ToolsGAPI.PrintError("Invalid loadout setup, expecting '</altguns>' but found none");
            return new List<Tuple<PickupObject, bool>>();
        }

    }
}
