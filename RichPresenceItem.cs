using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Discord;
using CustomCharacters;

namespace BotsMod
{ 
    public class RichPresenceItem : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Register()
        {
            //The name of the item
            string itemName = "Rich Presence";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "BotsMod/sprites/wip";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<RichPresenceItem>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "AHHHHHHHHHHHHHHHHHHH";
            string longDesc = "Enables Discord Rich Presence";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public Discord.Discord discord;

        public override void Pickup(PlayerController player)
        {
           
            try
            {
                discord = new Discord.Discord(793142591637684244, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

                var activityManager = discord.GetActivityManager();


                var activity = new Discord.Activity
                {
                    State = "someone fucking help me please",
                    Details = "Bigger Test",
                    Assets = new ActivityAssets()
                    {
                        LargeImage = "etggif",
                        LargeText = "",
                        SmallImage = "slinger",
                        SmallText = "slinger",
                    }
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res == Discord.Result.Ok)
                    {
                        BotsModule.Log("Everything is fine!");
                        help = true;
                    }
                    else
                    {
                        BotsModule.Log("Everything is **NOT** fine!");
                    }
                });
            }
            catch (Exception e)
            {
                BotsModule.Log("Discord stuff did a broken now you can cry", "#eb1313");
                BotsModule.Log(string.Format(e + ""), "#eb1313");
            }
            base.Pickup(player);
        }
        bool help = false;
        protected override void Update()
        {
            if (help == true)
            {
                var activityManager = discord.GetActivityManager();
                if (GameManager.Instance.PrimaryPlayer != null)
                {
                    var player = GameManager.Instance.PrimaryPlayer;

                    var activity = new Discord.Activity
                    {
                        State = "someone fucking help me please",
                        Details = GetFloorTileset().ToString(),
                        Assets = new ActivityAssets()
                        {
                            LargeImage = "etggif",
                            LargeText = "Time: 0:00",
                            SmallImage = GetPlayerName(player).ToLower(),
                            SmallText = player.name,
                        }
                    };
                    activityManager.UpdateActivity(activity, (res) =>
                    {
                        if (res != Discord.Result.Ok)
                        {
                            BotsModule.Log("Everything is **NOT** fine!");
                        }
                    });
                }

                discord.RunCallbacks();
            }

            base.Update();
        }

        private string GetPlayerName(PlayerController player)
        {
            switch (player.characterIdentity)
            {
                case PlayableCharacters.Bullet:
                    return "Bullet";

                case PlayableCharacters.Convict:
                    return "Convict";

                case PlayableCharacters.CoopCultist:
                    return "CoopCultist";

                case PlayableCharacters.Cosmonaut:
                    return "Cosmonaut";

                case PlayableCharacters.Eevee:
                    return "Eevee";

                case PlayableCharacters.Guide:
                    return "Guide";

                case PlayableCharacters.Gunslinger:
                    return "Gunslinger";

                case PlayableCharacters.Ninja:
                    return "Ninja";

                case PlayableCharacters.Pilot:
                    return "Pilot";

                case PlayableCharacters.Robot:
                    return "Robot";

                case PlayableCharacters.Soldier:
                    return "Soldier";

                case (PlayableCharacters)CustomPlayableCharacters.Lost:
                    return "11";

                default:
                    return "Pilot";
            }
        }


        private GlobalDungeonData.ValidTilesets GetFloorTileset()
        {
            if (GameManager.Instance.IsLoadingLevel || !GameManager.Instance.Dungeon)
            {
                return GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }
            return GameManager.Instance.Dungeon.tileIndices.tilesetId;
        }


        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }
    }
}