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

            string itemName = "TrailBullets";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject();
            //var item = BotsModule.WarCrime2;//obj.AddComponent<PirmalShotgrub>().GetComponent<PickupObject>();
            var item = obj.AddComponent<TrailBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);


            string shortDesc = "testing item";
            string longDesc = "this item is purly for testing";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.EXCLUDED;


            item.ParticlesToAdd = UnityEngine.Object.Instantiate(Tools.BotsAssetBundle.LoadAsset<GameObject>("ParticleSystemObj 1"));
        }




    }
}
