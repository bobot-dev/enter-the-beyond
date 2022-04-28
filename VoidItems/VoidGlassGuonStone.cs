using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NpcApi;

namespace BotsMod
{
    class VoidGlassGuonStone : PassiveItem
    {

        public static PlayerOrbital orbitalPrefab;

        public static void Init()
        {
            string itemName = "Crystal Guon Stone";
            string resourceName = "BotsMod/sprites/void_glass_guon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<VoidGlassGuonStone>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Absorbs Glass Guon Stones";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            shardPrefab = BuildPrefab();

            item.quality = PickupObject.ItemQuality.SPECIAL;
        }

        public static GameObject BuildPrefab()
        {
            var prefab = new GameObject("Guon Shard");

            FakePrefab.MarkAsFakePrefab(prefab);
            prefab.SetActive(false);

            spriteIds.Add("Glass", SpriteBuilder.AddSpriteToCollection("BotsMod/sprites/GuonShards/crystal_guon_stone_orbital", ETGMod.Databases.Items.ProjectileCollection, "crystal_guon_stone_orbital"));
            var sprite = prefab.AddComponent<tk2dSprite>();

            sprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteIds["Glass"]);
            sprite.SortingOrder = 0;

            sprite.IsPerpendicular = true;

            prefab.GetComponent<BraveBehaviour>().sprite = sprite;

            ShopAPI.GenerateOrAddToRigidBody(prefab, CollisionLayer.HighObstacle, PixelCollider.PixelColliderGeneration.Manual, false, false, true, false, false, false, true, true, new IntVector2(5, 8));

            return prefab;
        }

        public override void Pickup(PlayerController player)
        {
            while (player.HasPassiveItem(GlobalItemIds.GlassGuonStone))
            {
                player.RemovePassiveItem(GlobalItemIds.GlassGuonStone);
                guonsDestroyed++;
            }
            BotsModule.Log(guonsDestroyed.ToString());
            this.m_extantEffect = CreateShard(player, null);
            guonsDestroyed = 0;

            ETGMod.Gun.OnPostFired += FireShards;
            base.Pickup(player);
        }

        void FireShards(PlayerController owner, Gun gun)
        {
            BotsModule.Log("aaaaaaa");
            this.m_extantEffect.ThrowShield();
            if (this.m_extantEffect != null && this.m_extantEffect.IsActive)
            {
                
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            if (m_extantEffect?.m_knives != null)
            {
                foreach (var source in m_extantEffect.m_knives)
                {
                    LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(GlobalItemIds.GlassGuonStone).gameObject, player);
                    UnityEngine.Object.Destroy(source.gameObject);
                }
                this.m_extantEffect.m_knives.Clear();
            }
           
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private CrystalPlayerOrbital CreateShard(PlayerController user, PlayerOrbital baseOrbital, float radiusMultiplier = 1f, float rotationSpeedMultiplier = 1f)
        {
            CrystalPlayerOrbital knifeShieldEffect = new GameObject("Guon shard effect")
            {
                transform = { position = user.LockedApproximateSpriteCenter, parent = user.transform }
            }.AddComponent<CrystalPlayerOrbital>();
            knifeShieldEffect.numKnives = guonsDestroyed;
            knifeShieldEffect.knifeDamage = 1;
            knifeShieldEffect.circleRadius = (baseOrbital ? baseOrbital.orbitRadius/2 : 1.25f) * radiusMultiplier;
            knifeShieldEffect.rotationDegreesPerSecond = (baseOrbital ? baseOrbital.orbitDegreesPerSecond : 120f) * rotationSpeedMultiplier;
            knifeShieldEffect.throwSpeed = 9;
            knifeShieldEffect.throwRange = 1000;
            knifeShieldEffect.throwRadius = 0.5f;
            knifeShieldEffect.radiusChangeDistance = 2;
            knifeShieldEffect.Initialize(user, shardPrefab);
            return knifeShieldEffect;
        }
        static GameObject shardPrefab;        
        static tk2dSpriteCollection orbitalCollection;
        static Dictionary<string, int> spriteIds = new Dictionary<string, int>();



        int guonsDestroyed = 0;
        CrystalPlayerOrbital m_extantEffect;
    }
}
