using NpcApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BotsMod
{
    class ShopMerchantStuff
    {
        #region weaponShopMethods
        public static int CustomPriceWeapons(CustomShopController shop, CustomShopItemController shopItem, PickupObject item)
        {
            //BotsModule.Log("CustomPrice runs");
            return 1;
        }
        public static int RemoveCurrencyWeapons(CustomShopController shop, PlayerController player, int cost)
        {
            //BotsModule.Log("RemoveCurrency runs");
            player.inventory.DestroyCurrentGun();
            return 1;
        }

        public static bool CustomCanBuyWeapons(CustomShopController shop, PlayerController player, int cost)
        {
            //BotsModule.Log("CustomCanBuy runs");
            if (player.CurrentGun != null && player.CurrentGun.CanActuallyBeDropped(player))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region BeyondShopMethods
        public static int CustomPriceBeyond(CustomShopController shop, CustomShopItemController shopItem, PickupObject item)
        {


            if (GameManager.Instance.PrimaryPlayer.name == "PlayerShade(Clone)")
            {
                shopItem.customPriceSprite = "armor_shield_pickup_001";
                return 0;
            }
 
            int price = 1;

            if (item.quality == PickupObject.ItemQuality.A || item.quality == PickupObject.ItemQuality.S)
            {
                price = 2;
            }

            if (GameManager.Instance.PrimaryPlayer.healthHaver.Armor >= price * 2)
            {
                shop.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour = true;
                shopItem.customPriceSprite = "armor_shield_pickup_001";
                return price * 2;

            }
            shop.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour = false;
            shopItem.customPriceSprite = "heart_big_idle_001";
            return price;
        }
        public static int RemoveCurrencyBeyond(CustomShopController shop, PlayerController player, int cost)
        {

            if (player.name == "PlayerShade(Clone)")
            {
                FieldInfo _itemControllers = typeof(CustomShopController).GetField("m_itemControllers", BindingFlags.NonPublic | BindingFlags.Instance);
                foreach(CustomShopItemController item in _itemControllers.GetValue(shop) as List<ShopItemController>)
                {
                    item.ForceOutOfStock();
                }
            }

            if (shop.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour)
            {
                player.healthHaver.Armor -= cost;
            }
            else
            {
                StatModifier statModifier = new StatModifier();
                statModifier.statToBoost = PlayerStats.StatType.Health;
                statModifier.amount = -(cost);
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                player.ownerlessStatModifiers.Add(statModifier);
                player.stats.RecalculateStats(player, false, false);
            }
            return 1;
        }

        public static bool CustomCanBuyBeyond(CustomShopController shop, PlayerController player, int cost)
        {
            if (shop.gameObject.GetOrAddComponent<DevilDealShopHelper>().usingArmour = true && player.healthHaver.Armor >= cost)
            {
                BotsModule.Log($"Armour is {player.healthHaver.Armor} and the Price is {cost}");
                return player.healthHaver.Armor >= cost;

            }
            else
            {
                BotsModule.Log($"Health is {player.stats.GetStatValue(PlayerStats.StatType.Health)} and the Price is {cost}");
                return (player.stats.GetStatValue(PlayerStats.StatType.Health) > cost);
            }

            return false;
        }
        #endregion
    }
}
