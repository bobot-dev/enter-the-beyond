using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CarpetBurn : PassiveItem
    {
        static int id;
        public static void Init()
        {
            string itemName = "Carpet Burn";
            string resourceName = "BotsMod/sprites/carpet_burn";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<CarpetBurn>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "IT BURNS";
            string longDesc = "Causes any gundead unfortunate enough to stand on carpet to burst into flames.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            item.quality = PickupObject.ItemQuality.C;

            id = item.PickupObjectId;

            new Hook(typeof(AIActor).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public),
                    typeof(CarpetBurn).GetMethod("UpdateHook", BindingFlags.Static | BindingFlags.Public));
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

        public static void UpdateHook(Action<AIActor> orig, AIActor self)
        {
            orig(self);
            CellVisualData.CellFloorType floorTypeFromPosition = GameManager.Instance.Dungeon.GetFloorTypeFromPosition(self.specRigidbody.UnitBottomCenter);
            if (self.GetEffect(EffectResistanceType.Fire) == null && !self.IsFlying && floorTypeFromPosition == CellVisualData.CellFloorType.Carpet && GameManager.HasInstance && GameManager.Instance.PrimaryPlayer &&
                GameManager.Instance.PrimaryPlayer.HasPassiveItem(id))
            {
                self.ApplyEffect((PickupObjectDatabase.GetById(295) as BulletStatusEffectItem).FireModifierEffect, 1f, null);
            }
        }
    }
}
