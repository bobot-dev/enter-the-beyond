using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class TrailBullets : BulletStatusEffectItem
    {

        public static void Init()
        {

            string itemName = "Fuck You Bullets";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject();
            //var item = BotsModule.WarCrime2;//obj.AddComponent<PirmalShotgrub>().GetComponent<PickupObject>();
            var item = obj.AddComponent<TrailBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "good luck hitting shit dumbass";
            string longDesc = "fuck homing worthless garbage >:(";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.EXCLUDED;


           
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Player_PostProcessProjectile;
            base.Pickup(player);
        }

        private void Player_PostProcessProjectile(Projectile arg1, float arg2)
        {
            var howaboutyouhomingintogoodcodecunt = arg1.gameObject.GetOrAddComponent<HomingModifier>();
            howaboutyouhomingintogoodcodecunt.HomingRadius = 5;
            howaboutyouhomingintogoodcodecunt.AngularVelocity = -100000;
        }
    }
}
