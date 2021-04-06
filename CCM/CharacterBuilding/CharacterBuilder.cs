using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using ItemAPI;
using BotsMod;
using GungeonAPI;

namespace CustomCharacters
{
    /*
     * Creates a prefab for the custom character and applies 
     * all the metadata to it
     */
    public static class CharacterBuilder
    {
        public static Dictionary<string, Tuple<CustomCharacterData, GameObject>> storedCharacters = new Dictionary<string, Tuple<CustomCharacterData, GameObject>>();
        //public static Dictionary<string, tk2dSpriteCollectionData> storedCollections = new Dictionary<string, tk2dSpriteCollectionData>();
        public static List<Gun> guns = new List<Gun>();

        public static void BuildCharacter(CustomCharacterData data)
        {
            var basePrefab = GetPlayerPrefab(data.baseCharacter);
            if (basePrefab == null)
            {
                ToolsGAPI.PrintError("Could not find prefab for: " + data.baseCharacter.ToString());
                return;
            }

            ToolsGAPI.Print("");
            ToolsGAPI.Print("--Building Character: " + data.nameShort + "--", "0000FF");
            
            PlayerController playerController;
            GameObject gameObject = GameObject.Instantiate(basePrefab);

            playerController = gameObject.GetComponent<PlayerController>();
            playerController.gameObject.AddComponent<CustomCharacter>().data = data;
            data.characterID = storedCharacters.Count;
            /*
            foreach (var clip in playerController.gameActor.spriteAnimator.Library.clips)
            {
                
                foreach (var frame in clip.frames)
                {
                    if (frame.eventAudio.Length > 0)
                    {
                        BotsModule.Log(clip.name + ": " + frame.eventAudio);
                        if (frame.eventAudio == "Play_FS")
                        {
                            frame.eventAudio = "Play_Bot_Hammer";
                        }
                    }
                           
                }
            }*/

            

            GameObject.DontDestroyOnLoad(gameObject);

            CustomizeCharacter(playerController, data);

            basePrefab = null;
            storedCharacters.Add(data.nameInternal.ToLower(), new Tuple<CustomCharacterData, GameObject>(data, gameObject));

            gameObject.SetActive(false);
            GungeonAPI.FakePrefab.MarkAsFakePrefab(gameObject);
        }

        public static void CustomizeCharacter(PlayerController player, CustomCharacterData data)
        {
            HandleStrings(player, data);

            ToolsGAPI.StartTimer("    Sprite Handling");
            SpriteHandler.HandleSprites(player, data);
            ToolsGAPI.StopTimerAndReport("    Sprite Handling");

            if (data.loadout != null)
                HandleLoadout(player, data.loadout, data.altGun);

            if (data.stats != null)
                HandleStats(player, data.stats);

            player.healthHaver.ForceSetCurrentHealth(data.health);
            player.healthHaver.Armor = (int)data.armor;

            BotsModule.LostAltCostume = data.AlternateCostumeLibrary;
            //player.OverridePlayerSwitchState = (PlayableCharacters.Ninja).ToString();

            foreach (int i in Enum.GetValues(typeof(CustomPlayableCharacters)))
            {
                if (i >= 11)
                {
                    player.characterIdentity = data.identity;
                    break;
                }
            }



            //AkSoundEngine.switch

            AkSoundEngine.SetSwitch("CHR_Player", (player.OverridePlayerSwitchState == null) ? data.nameShort : player.OverridePlayerSwitchState, player.gameObject);

            uint idk;

            AkSoundEngine.GetSwitch("CHR_Player", player.gameObject, out idk);

            BotsModule.Log(""+idk);

            //BotsModule.Log((player.OverridePlayerSwitchState == null) ? data.nameShort : player.OverridePlayerSwitchState);

            

            player.animationAudioEvents = new List<ActorAudioEvent>
            {
                new ActorAudioEvent
                {
                    eventTag = "step",
                    eventName = "Play_Bot_Hammer"
                },
            };

            //BotsModule.Strings.Core.Set("#PLAYER_NICK_LOST", "Dead Thing");
            //BotsModule.Strings.Core.Set("#PLAYER_NAME_LOST", "Lost");

            StringHandler.AddStringDefinition("#PLAYER_NAME_" + player.characterIdentity.ToString().ToUpper(), data.name);
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + player.characterIdentity.ToString().ToUpper(), data.nickname);

            StringHandler.AddDFStringDefinition("#CHAR_" + player.characterIdentity.ToString().ToUpper(), data.name);
            StringHandler.AddDFStringDefinition("#CHAR_" + player.characterIdentity.ToString().ToUpper() + "_SHORT", data.nameShort);

            BotsModule.Log("Player is: " + player.characterIdentity.ToString(), BotsModule.LOST_COLOR);
            BotsModule.Log("#CHAR_" + player.characterIdentity.ToString().ToUpper(), BotsModule.LOST_COLOR);
            //GameManager.Instance.PrimaryPlayer.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(player.spriteAnimator.Library);
        }

        public static void HandleStrings(PlayerController player, CustomCharacterData data)
        {
            player.name = data.nameInternal;
            if (data.faceCard != null)
                player.uiPortraitName = data.nameInternal + "_facecard";

            //HandleDictionaries(data);
        }

        public static void HandleDictionaries(CustomCharacterData data)
        {
            string keyBase = data.nameShort.ToUpper();
            StringHandler.AddStringDefinition("#PLAYER_NAME_" + keyBase, data.name); //TODO override the get methods instead of overwriting!
           // StringHandler.//AddStringDefinition("#PLAYER_NAME_" + keyBase, data.name); //TODO override the get methods instead of overwriting!
            //StringHandler.//AddStringDefinition("#PLAYER_NICK_" + keyBase, data.nickname);
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + data.nickname.ToUpper(), data.nickname);

            StringHandler.AddDFStringDefinition("#CHAR_" + keyBase, data.name);
            StringHandler.AddDFStringDefinition("#CHAR_" + keyBase + "_SHORT", data.nameShort);
        }

        public static void HandleLoadout(PlayerController player, List<Tuple<PickupObject, bool>> loadout, string altGun)
        {
            StripPlayer(player);
            foreach (var tuple in loadout)
            {
                var item = tuple.First;
                int id = item.PickupObjectId;
                var passive = item.GetComponent<PassiveItem>();
                var active = item.GetComponent<PlayerItem>();
                var gun = item.GetComponent<Gun>();

                if (passive)
                    player.startingPassiveItemIds.Add(id);
                else if (active)
                    player.startingActiveItemIds.Add(id);
                else if (gun)
                {
                    player.startingGunIds.Add(id);
                    //if (altGun == "")
                    //{
                        player.startingAlternateGunIds.Add(id);
                   // }
                    
                }
                else
                {
                    ToolsGAPI.PrintError("Is this even an item? It has no passive, active or gun component! " + item.EncounterNameOrDisplayName);
                }
            }
           // if (altGun != "")
            //{
            //    player.startingAlternateGunIds.Add(PickupObjectDatabase.GetByEncounterName(altGun).PickupObjectId);
            //}
        }

        public static void StripPlayer(PlayerController player)
        {
            List<int> starters = player.startingPassiveItemIds; //remove all the starter passives
            foreach (int passiveid in starters)
            {
                player.RemovePassiveItem(passiveid);
            }
            player.passiveItems = new List<PassiveItem>();
            player.startingPassiveItemIds = new List<int>();
            player.RemoveAllPassiveItems(); //removes all passives except starters

            if (player.inventory != null)
                player.inventory.DestroyAllGuns(); //clear guns
            player.startingGunIds = new List<int>();
            player.startingAlternateGunIds = new List<int>();

            player.activeItems.Clear(); //clear actives
            player.startingActiveItemIds = new List<int>();
        }

        public static void HandleStats(PlayerController player, Dictionary<PlayerStats.StatType, float> stats)
        {
            foreach (var stat in stats)
            {
                player.stats.SetBaseStatValue(stat.Key, stat.Value, player);
                if (stat.Key == PlayerStats.StatType.DodgeRollDistanceMultiplier)
                {
                    player.rollStats.distance *= stat.Value;
                }
                if (stat.Key == PlayerStats.StatType.DodgeRollSpeedMultiplier)
                {
                    player.rollStats.time *= 1f / (stat.Value + Mathf.Epsilon);
                }
            }
        }

        public static GameObject GetPlayerPrefab(PlayableCharacters character)
        {
            string resourceName;

            if (character == PlayableCharacters.Soldier)
                resourceName = "marine";
            else if (character == PlayableCharacters.Pilot)
                resourceName = "rogue";
            else
                resourceName = character.ToString().ToLower();

            return (GameObject)BraveResources.Load("player" + resourceName);

        }
    }
}
