using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using BotsMod;
using System.Runtime.CompilerServices;
using System.Reflection;
using static BotsMod.thingistolefromapache;
using System.IO;

namespace BotsMod
{
    public class LostsCloak : PassiveItem
    {

        public static string EnterTheBeyondConfigPath = Path.Combine(ETGMod.ResourcesDirectory, "EnterTheBeyondConfig.json");

        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Lost Robe";
            string resourceName = "BotsMod/sprites/cloak";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<LostsCloak>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "idk ill put something here";
            string longDesc = "This robe seems to b";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.SPECIAL;

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 1f, StatModifier.ModifyMethod.ADDITIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.PreventStartingOwnerFromDropping = true;
            m_item = item;
            BotsItemIds.LostCloak = item.PickupObjectId;

            item.PlaceItemInAmmonomiconAfterItemById(414);

            Tools.BeyondItems.Add(item.PickupObjectId);

        }

        static PassiveItem m_item;

        private ImprovedAfterImage zoomy;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            player.OnNewFloorLoaded += UpdateHearts;

            Tools.AddGlow(player, 55, 4.55f, new Color32(255, 0, 38, 255));


            if (m_pickedUp) { return; }



            zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
            zoomy.dashColor = new Color(180, 32, 42);
            zoomy.spawnShadows = false;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowTimeDelay = 0.05f;
            zoomy.shadowLifetime = 0.3f;
            zoomy.minTranslation = 0.05f;
            zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");

            this.m_currentTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;
        }

        private GlobalDungeonData.ValidTilesets m_currentTileset;

        private static void UpdateHearts(PlayerController player)
        {
            dfAtlas.ItemInfo pain = new dfAtlas.ItemInfo();
            pain.name = "heart_full_purple_001";
            pain.texture = ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/heart_full_purple_001.png");
            pain.sizeInPixels = new Vector2(15f, 13f);
            Texture2D atlas = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Texture;
            Rect region = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Items[1].region;

            for (int x = 0; x < pain.texture.width; x++)
            {
                for (int y = 0; y < pain.texture.height; y++)
                {
                    atlas.SetPixel(x + (int)(region.xMin * 2048), y + (int)(region.yMin * 2048), pain.texture.GetPixel(x, y));
                }
            }
            atlas.Apply(false, false);
            pain.region = new Rect(region.xMin, region.yMin, (float)pain.texture.width / 2048f, (float)pain.texture.height / 2048f);
            GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.AddItem(pain);

            dfAtlas.ItemInfo pain2 = new dfAtlas.ItemInfo();
            pain2.name = "heart_half_purple_001";
            pain2.texture = ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/heart_half_purple_001.png");
            pain2.sizeInPixels = new Vector2(15f, 13f);
            Texture2D atlas2 = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Texture;
            Rect region2 = GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.Items[0].region;

            for (int x = 0; x < pain2.texture.width; x++)
            {
                for (int y = 0; y < pain2.texture.height; y++)
                {
                    atlas2.SetPixel(x + (int)(region2.xMin * 2048), y + (int)(region2.yMin * 2048), pain2.texture.GetPixel(x, y));
                }
            }
            atlas2.Apply(false, false);
            pain2.region = new Rect(region2.xMin, region2.yMin, (float)pain2.texture.width / 2048f, (float)pain2.texture.height / 2048f);
            GameUIRoot.Instance.heartControllers[0].extantHearts[0].Atlas.AddItem(pain2);
            GameUIRoot.Instance.heartControllers[0].fullHeartSpriteName = "heart_full_purple_001";
            GameUIRoot.Instance.heartControllers[0].halfHeartSpriteName = "heart_half_purple_001";


            Tools.AddGlow(player, 55, 4.55f, new Color32(255, 0, 38, 255));
        }

        protected override void Update()
        {
            base.Update();
            return;
            PlayerController player = this.m_owner;

            GlobalDungeonData.ValidTilesets validTilesets = this.GetFloorTileset();
            if (validTilesets == (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND && hasBuff == false)
            {
                if (!zoomy.spawnShadows)
                {
                    zoomy.spawnShadows = true;
                }
                AddStat(PlayerStats.StatType.MovementSpeed, 1.3f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                player.stats.RecalculateStats(player, false, false);

                hasBuff = true;
            }
            else if (validTilesets != (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND && hasBuff == true) 
            {

                player.ClearOverrideShader();
                RemoveStat(PlayerStats.StatType.MovementSpeed);
                if (zoomy.spawnShadows)
                {
                    zoomy.spawnShadows = false;
                }
                player.stats.RecalculateStats(player, false, false);

                hasBuff = false;
            }


        }


        bool hasBuff = false;

        private bool IsValidTileset(GlobalDungeonData.ValidTilesets t)
        {
            if (t == this.GetFloorTileset())
            {
                return true;
            }
            PlayerController playerController = this.Owner as PlayerController;
            if (playerController)
            {
                if (t == GlobalDungeonData.ValidTilesets.CASTLEGEON)
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.GUNGEON)
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.MINEGEON)
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.FORGEGEON)
                {
                    return true;
                }
                if (t == (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier modifier = new StatModifier();
            modifier.amount = amount;
            modifier.statToBoost = statType;
            modifier.modifyType = method;

            foreach (var m in passiveStatModifiers)
            {
                if (m.statToBoost == statType) return; //don't add duplicates
            }

            if (this.passiveStatModifiers == null)
                this.passiveStatModifiers = new StatModifier[] { modifier };
            else
                this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[] { modifier }).ToArray();
        }


        //Removes a stat
        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }

        private GlobalDungeonData.ValidTilesets GetFloorTileset()
        {
            if (GameManager.Instance.IsLoadingLevel || !GameManager.Instance.Dungeon)
            {
                return GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }
            return GameManager.Instance.Dungeon.tileIndices.tilesetId;
        }


    }        
}

