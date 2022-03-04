using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CustomAmmo
    {
        public static GameObject statAmmo;

        public static void Init()
        {
            statAmmo = FakePrefab.Clone(PickupObjectDatabase.GetById(600).gameObject);
            statAmmo.name = "Heavy Ammo";
            var ammo = statAmmo.GetComponent<AmmoPickup>();
            ammo.AppliesCustomAmmunition = true;
            ammo.mode = AmmoPickup.AmmoPickupMode.ONE_CLIP;
            ammo.CustomAmmunitionDamageModifier = 3;
            ammo.CustomAmmunitionSpeedModifier = 0.5f;
            ItemBuilder.SetupItem(ammo, "", "", "bot");

        }

    }
}
