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

        public static void BuildCharacter(CustomCharacterData data, bool hasAltSkin, bool paradoxUsesSprites, bool removeFoyerExtras, bool hasCustomPast, string customPast, int metaCost, bool useGlow, Color emissiveColor, float emissiveColorPower, float emissivePower)
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


            

            GameObject.DontDestroyOnLoad(gameObject);

            CustomizeCharacter(playerController, data, paradoxUsesSprites);

            basePrefab = null;
            storedCharacters.Add(data.nameInternal.ToLower(), new Tuple<CustomCharacterData, GameObject>(data, gameObject));
            //BotsModule.Log("nameInternal: " + data.nameInternal, BotsModule.TEXT_COLOR);
            var character = gameObject.AddComponent<CustomCharacterController>();   
            
            character.data = data;
            character.past = customPast;
            character.hasPast = hasCustomPast;
            character.metaCost = metaCost;

            character.useGlow = useGlow;
            character.emissiveColor = emissiveColor;
            character.emissiveColorPower = emissiveColorPower;
            character.emissivePower = emissivePower;

            data.useGlow = useGlow;
            data.emissiveColor = emissiveColor;
            data.emissiveColorPower = emissiveColorPower;
            data.emissivePower = emissivePower;

            data.removeFoyerExtras = removeFoyerExtras;
            data.metaCost = metaCost;
            data.customCharacterController = character;


            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
        }

        public static void CustomizeCharacter(PlayerController player, CustomCharacterData data, bool paradoxUsesSprites)
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


            player.characterIdentity = (PlayableCharacters)CustomPlayableCharacters.Custom;


            //AkSoundEngine.switch

            //AkSoundEngine.SetSwitch("CHR_Player", (player.OverridePlayerSwitchState == null) ? data.nameShort : player.OverridePlayerSwitchState, player.gameObject);


            //AkSoundEngine.GetSwitch("CHR_Player", player.gameObject, out idk);


            

            //BotsModule.Log((player.OverridePlayerSwitchState == null) ? data.nameShort : player.OverridePlayerSwitchState);



            //BotsModule.Strings.Core.Set("#PLAYER_NICK_LOST", "Dead Thing");
            //BotsModule.Strings.Core.Set("#PLAYER_NAME_LOST", "Lost");

            StringHandler.AddStringDefinition("#PLAYER_NAME_" + data.nameShort.ToString().ToUpper(), data.name);
            StringHandler.AddStringDefinition("#PLAYER_NICK_" + data.nameShort.ToString().ToUpper(), data.nickname);

            StringHandler.AddDFStringDefinition("#CHAR_" + data.nameShort.ToString().ToUpper(), data.name);
            StringHandler.AddDFStringDefinition("#CHAR_" + data.nameShort.ToString().ToUpper() + "_SHORT", data.nameShort);

            //BotsModule.Log("Player is: " + data.nameShort.ToString(), BotsModule.LOST_COLOR);
            //BotsModule.Log("#CHAR_" + data.nameShort.ToUpper(), BotsModule.LOST_COLOR);

            if (!hasClearedEeveeAnims)
            {
                var eevee = (GameObject)ResourceCache.Acquire("PlayerEevee");
                if (eevee != null)
                {
                    eevee.GetComponent<CharacterAnimationRandomizer>().AnimationLibraries.Clear();
                }
                hasClearedEeveeAnims = true;
            }

            if (paradoxUsesSprites)
            {
                var eevee = (GameObject)ResourceCache.Acquire("PlayerEevee");
                if (player.spriteAnimator.Library != null && eevee != null)
                {
                    eevee.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(player.spriteAnimator.Library);
                    //BotsModule.Log("player.spriteAnimator.Library added");
                }
                if (player.AlternateCostumeLibrary != null && eevee != null)
                {
                    eevee.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(player.AlternateCostumeLibrary);
                    //BotsModule.Log("AlternateCostumeLibrary added");
                }
            }

            
            
            //GameManager.Instance.PrimaryPlayer.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(player.spriteAnimator.Library);
        }

        static bool hasClearedEeveeAnims = false;

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

        public static void HandleLoadout(PlayerController player, List<Tuple<PickupObject, bool>> loadout, List<Tuple<PickupObject, bool>> altGun)
        {

            if (loadout == null)
            {
                ToolsGAPI.PrintError("loadout is null :((((((((");
            }

            if (altGun == null)
            {
                ToolsGAPI.PrintError("altGun is null :((((((((");
            }

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
                }
                else
                {
                    ToolsGAPI.PrintError("Is this even an item? It has no passive, active or gun component! " + item.EncounterNameOrDisplayName);
                }
            }

            foreach (var tuple in altGun)
            {
                var item = tuple.First;
                int id = item.PickupObjectId;
                var gun = item.GetComponent<Gun>();

                if (gun)
                {
                    player.startingAlternateGunIds.Add(id);
                }
                else
                {
                    ToolsGAPI.PrintError("Is this even an gun? It has no gun component! " + item.EncounterNameOrDisplayName);
                }
            }
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
