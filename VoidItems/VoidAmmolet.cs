using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NpcApi;

namespace BotsMod
{
    class VoidAmmolet : BlankModificationItem
    {

        public static List<Projectile> shardProjectiles = new List<Projectile>();

        public static void Init()
        {
            string itemName = "Void Ammolet";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<VoidAmmolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Consumes Blanks";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            GameUIRoot.Instance.AreYouSurePanel.Atlas.AddNewItemToAtlas(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/UI/void_blank.png"), "void_blank");

            item.quality = PickupObject.ItemQuality.C;

            BotsItemIds.VoidAmmolet = item.PickupObjectId;


            for (int i = 1; i <= 5; i++)
            {
                var proj = Tools.SetupProjectile(15);
                BotsMod.BotsModule.Log("setting up shard " + i);
                proj.SetProjectileSpriteRight("blank_shard_00" + i, 15, 5, false, tk2dBaseSprite.Anchor.LowerLeft);
                shardProjectiles.Add(proj);
                proj.baseData.damage = 3f;
                proj.baseData.speed = 30f;
                proj.baseData.force = 5f;
                proj.baseData.range = 1000f;
                proj.AppliesKnockbackToPlayer = false;
                proj.shouldRotate = true;
                var bounce = proj.gameObject.GetOrAddComponent<BounceProjModifier>();

                bounce.numberOfBounces = 20;
                bounce.bouncesTrackEnemies = false;
                bounce.chanceToDieOnBounce = 0.1f;
                bounce.damageMultiplierOnBounce = 1;
                bounce.suppressHitEffectsOnBounce = true;
                bounce.percentVelocityToLoseOnBounce = -0.3f;

            }
            
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

        public static void VoidBlank(PlayerController source, Vector3 pos)
        {
            BotsMod.BotsModule.Log("VoidBlank triggered");


            int num = UnityEngine.Random.Range(16, 25);
            float num2 = 360f / (float)num;
            float num3 = UnityEngine.Random.Range(20, -20);
            
            
            for (int i = 0; i < num; i++)
            {
                
                Projectile projectile = shardProjectiles[UnityEngine.Random.Range(0, shardProjectiles.Count)];

                float z = (num2 * (float)i) + num3;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, pos, Quaternion.Euler(0f, 0f, z), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.IgnoreTileCollisionsFor(0.5f);
                component.Owner = source;
                component.Shooter = source.specRigidbody;
            }
        }
    }
}
