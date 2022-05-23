using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class UnstableRelocationStone : PassiveItem
    {
        public static void Init()
        {


            ItemBuilder.BuildItem<UnstableRelocationStone>("Unstable Relocation Core", "bot", "BotsMod/sprites/unstable_relocation_core", "", "", ItemQuality.A);


            /*
            string itemName = "Unstable Relocation Core";
            string resourceName = "BotsMod/sprites/unstable_relocation_core";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<UnstableRelocationStone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            item.quality = PickupObject.ItemQuality.A;*/
        }

        public override void Pickup(PlayerController player)
        {
            player.healthHaver.ModifyDamage += ModifyIncomingDamage;
            base.Pickup(player);
        }

        private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            if (source.gameActor is PlayerController && args.InitialDamage > 0)
            {
                var player = source.gameActor as PlayerController;


                if (player.CurrentRoom != null)
                {
                    IntVector2? pos = player.CurrentRoom.GetRandomAvailableCell(passableCellTypes: Dungeonator.CellTypes.FLOOR);
                    player.WarpToPoint(((Vector2)pos.Value), true);
                    player.healthHaver.TriggerInvulnerabilityPeriod(0.25f);
                    args.ModifiedDamage = 0;
                } 
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.ModifyDamage -= ModifyIncomingDamage;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner != null) Owner.healthHaver.ModifyDamage -= ModifyIncomingDamage;
            base.OnDestroy();
        }
    }
}
