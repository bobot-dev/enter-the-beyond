using System;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class CrownOfTheChosen : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crown Of The Chosen";

            string resourceName = "BotsMod/sprites/wip";
            //string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            

            var item = obj.AddComponent<CrownOfTheChosen>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "For The Worthy";
            string longDesc = "Gives a powerful blessing to those who wear it.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.S;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;
        }
        public override void Pickup(PlayerController player)
        {
            PassiveItem passive = this;
            PickupObject item = PickupObjectDatabase.GetById(this.PickupObjectId);
            Change(passive, item);
            base.Pickup(player);

        }
        private void Change(PassiveItem passive, PickupObject item)
        {
            //
            //do stuff where you clear all the behaviours here
            //
            if (CelsItems.SteamUsername == "UnstableStrafe")
            {
                item.SetName("The Electric Queen");
                item.SetShortDescription("Funeral Pyre");
                //item.SetLongDescription("");
                //Attacks release homing lightning 
                CelsItems.Log("Crown Form: UnstableStrafe");
            }
            else if (CelsItems.SteamUsername == "Nevernamed")
            {
                item.SetName("Crown Of The Nameless King");
                item.SetShortDescription("Eternal And Formless");
                item.gameObject.AddComponent<NevernamedForm>();
                
                CelsItems.Log("Crown Form: Nevernamed");
                
                //item.SetLongDescription("");
                //high damage aura around the player
            }
            else if (CelsItems.SteamUsername == "TheTurtleMelon")
            {
                item.SetName("Crown Of The Enduring");
                item.SetShortDescription("A'Tuin's Power");
                //item.SetLongDescription("");
                //Adds more hp, lower hp means more damage.   
                CelsItems.Log("Crown Form: TheTurtleMelon");
            }
            else if (CelsItems.SteamUsername == "blazeykat")
            {
                item.SetName("Prismatic Crown");
                item.SetShortDescription("Queen Of Frogs");
                //item.SetLongDescription("");
                CelsItems.Log("Crown Form: blazeykat");
            }
            else if (CelsItems.SteamUsername == "BleakBubbles")
            {
                item.SetName("Bleak Crown");
                item.SetShortDescription("Cooler Than You");
                //item.SetLongDescription("");
                //Increases coolness. More coolness means more damage and rate of fire.
                CelsItems.Log("Crown Form: BleakBubbles");
            }
            else if (CelsItems.SteamUsername == "Neighborino")
            {
                item.SetName("Frostburn Crown");
                item.SetShortDescription("Wandering...");
                //item.SetLongDescription("");
                //Adds two additional fire and ice projectiles that have orbital bullet effects with each shot.
                CelsItems.Log("Crown Form: Neighborino");
            }
            else if (CelsItems.SteamUsername == "Some Bunny")
            {
                item.SetName("Circle Of The Void");
                item.SetShortDescription("NullReferenceException");
                //item.SetLongDescription("");
                //Cuases periodic explosions around the player
                CelsItems.Log("Crown Form: Some Bunny");
            }
            else if (CelsItems.SteamUsername == "The explosive panda")
            {
                item.SetName("Shimmering Crown");
                item.SetShortDescription("Cult Of Shadows");
                //item.SetLongDescription("");
                //Causes random effects
                CelsItems.Log("Crown Form: The explosive panda");
            }
            else if (CelsItems.SteamUsername == "N0tAB0t")
            {

                item.SetName("N0R Crown");
                item.SetShortDescription("!Crown");
                //item.SetLongDescription("");
                CelsItems.Log("Crown Form: N0tAB0t");
            }
            else if (CelsItems.SteamUsername == "KyleTheScientist")
            {
                item.SetName("Creator's Crown");
                item.SetShortDescription("The One To Rule All");
                //item.SetLongDescription("");
                CelsItems.Log("Crown Form: KyleTheScientist");
            }
            else if (CelsItems.SteamUsername == "Retromation")
            {
                item.SetName("Lich Slayer's Crown");
                item.SetShortDescription("Supreme");
                //item.SetLongDescription("");
                //Massive Boss Damage up, ignores boss dps cap
                CelsItems.Log("Crown Form: Retromation");
            }
            else if (CelsItems.SteamUsername == "LordOfHippos")
            {
                item.SetName("Crown Of The Hipo");
                item.SetShortDescription("Epic");
                //item.SetLongDescription("");
                CelsItems.Log("Crown Form: LordOfHippos");
            }
            else if (CelsItems.SteamUsername == "YaBoiLazer")
            {
                item.SetName("Laser Crown");
                item.SetShortDescription("Pew pew");
                //item.SetLongDescription("");
                CelsItems.Log("Crown Form: YaBoiLazer");
            }
            else if (CelsItems.SteamUsername == "W.D Cipher")
            {
                item.SetName("Low-Quality Crown");
                item.SetShortDescription("Ascendant");
                //item.SetLongDescription("");
                //Like Key Of Chaos, but more random
                CelsItems.Log("Crown Form: W.D Cipher");

            }
            else if (CelsItems.SteamUsername == "Glaurung4567")
            {
                item.SetName("Dragon's Horns");
                item.SetShortDescription("The First Wyrm");
                //item.SetLongDescription(""); 
                //Gives the player 2 permanent orbiting Dragunfires
                CelsItems.Log("Crown Form: Glaurung4567");
            }
            else if (CelsItems.SteamUsername == "Retrash")
            {
                item.SetName("Blunderbeast Helmet");
                item.SetShortDescription("Cursed");
                //item.SetLongDescription(""); 
                CelsItems.Log("Crown Form: Retrash");
            }
            else if (CelsItems.SteamUsername == "SirWow")
            {
                item.SetName("Painted Crown");
                item.SetShortDescription("Colorful Death");
                //item.SetLongDescription(""); 
                //reloading creates a number of random goop pools around the player
                CelsItems.Log("Crown Form: Nevernamed");
            }
            else if (CelsItems.SteamUsername == "SirWow")
            {
                item.SetName("");
                item.SetShortDescription("");
                //item.SetLongDescription(""); 
                CelsItems.Log("Crown Form: An3s079");
            }
            else if (CelsItems.SteamUsername == "")
            {
                item.SetName("");
                item.SetShortDescription("");
                //item.SetLongDescription(""); 
                CelsItems.Log("Crown Form: ");
            } else
            {
                item.SetName("");
                item.SetShortDescription("");
                //item.SetLongDescription(""); 
                CelsItems.Log("Crown Form: ");
            }
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<CrownOfTheChosen>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
