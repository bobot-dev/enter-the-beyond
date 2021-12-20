using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NpcApi;
using CustomCharacters;

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

            item.AddPassiveStatModifier(PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = PickupObject.ItemQuality.C;

            BotsItemIds.VoidAmmolet = item.PickupObjectId;


            for (int i = 1; i <= 5; i++)
            {
                var proj = Tools.SetupProjectile(15);
                BotsMod.BotsModule.Log("setting up shard " + i);
                proj.SetProjectileSpriteRight("blank_shard_00" + i, 15, 5, false, tk2dBaseSprite.Anchor.LowerLeft, 14, 4);
                shardProjectiles.Add(proj);
                proj.baseData.damage = 1f;
                proj.baseData.speed = 30f;
                proj.baseData.force = 5f;
                proj.baseData.range = 1000f;
                proj.AppliesKnockbackToPlayer = false;
                proj.shouldRotate = true;

                //proj.collidesWithEnemies = false;

                proj.baseData.UsesCustomAccelerationCurve = true;
                proj.baseData.AccelerationCurve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[]
                    {
                    new Keyframe
                    {
                        time = 0f,
                        value = 0f,
                        inTangent = 0f,
                        outTangent = 0f
                    },
                    new Keyframe
                    {
                        time = 0.5f,
                        value = 0.3f,
                        inTangent = 1f,
                        outTangent = 1f
                    },
                    new Keyframe
                    {
                        time = 1f,
                        value = 1f,
                        inTangent = 2f,
                        outTangent = 2f
                    },
                    }
                };
                proj.baseData.CustomAccelerationCurveDuration = 0.3f;
                proj.baseData.IgnoreAccelCurveTime = 0f;

                var bounce = proj.gameObject.GetOrAddComponent<BounceProjModifier>();

                var pierce = proj.gameObject.GetOrAddComponent<PierceProjModifier>();

                pierce.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
                pierce.penetratesBreakables = true;
                pierce.penetration = 10;
                pierce.MaxBossImpacts = 10;
                pierce.UsesMaxBossImpacts = false;
                pierce.preventPenetrationOfActors = false;


                bounce.numberOfBounces = 20;
                bounce.bouncesTrackEnemies = false;
                bounce.chanceToDieOnBounce = 0.3f;
                bounce.damageMultiplierOnBounce = 1;
                bounce.suppressHitEffectsOnBounce = true;
                bounce.percentVelocityToLoseOnBounce = -0.3f;

                var shard = proj.gameObject.GetOrAddComponent<BlankShardModifier>();

                UnityEngine.Object.Instantiate(Tools.AHHH.LoadAsset<GameObject>("BlankShardTrail"), proj.transform);

            }
            //CollectionDumper.DumpCollection(ETGMod.Databases.Items.ProjectileCollection);

            //GungeonAPI.ToolsGAPI.ExportTexture(ETGMod.Databases.Items.ProjectileCollection.textures[0], "SpriteDump/ProjectileSprites");
            


        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }

        public static void VoidBlank(PlayerController source, Vector3 pos, float amount, bool noDamage)
        {
            if (source.HasModdedItem("Ivory Ammolet"))
            {
                amount /= 5;
            }

            BotsMod.BotsModule.Log("VoidBlank triggered" + amount);
            bool flag = true;
            float num = 10f;
            float num2 = 7f;
            float num3 = 1f;
            if (amount < 5f)
            {
                flag = true;
                num = 10f;
                num2 = amount;
            }
            float? num4 = SilencerInstance.s_MaxRadiusLimiter;
            if (num4 != null)
            {
                amount = SilencerInstance.s_MaxRadiusLimiter.Value;
            }

            bool shouldReflectInstead = false;
            for (int i = 0; i < source.passiveItems.Count; i++)
            {
                BlankModificationItem blankModificationItem = source.passiveItems[i] as BlankModificationItem;
                if (blankModificationItem != null)
                {
                    if (blankModificationItem.BlankReflectsEnemyBullets)
                    {
                        shouldReflectInstead = true;
                    }
                    if (blankModificationItem.MakeBlankDealDamage)
                    {
                        flag = true;
                        num += blankModificationItem.BlankDamage;
                        num2 = Mathf.Max(num2, blankModificationItem.BlankDamageRadius);
                    }
                    num3 *= blankModificationItem.BlankForceMultiplier;
                    //this.ProcessBlankModificationItemAdditionalEffects(blankModificationItem, centerPoint, user);
                }
            }

            if (source && source.HasActiveBonusSynergy(CustomSynergyType.ELDER_BLANK_BULLETS, false))
            {
                shouldReflectInstead = true;
            }

            float dir = 360f / (float)amount;

            for (int i = 0; i < amount; i++)
            {
                float offset = UnityEngine.Random.Range(20, -20);
                Projectile projectile = shardProjectiles[UnityEngine.Random.Range(0, shardProjectiles.Count)];

                float z = (dir * (float)i) + offset;
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, pos, Quaternion.Euler(0f, 0f, z), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (noDamage)
                {
                    component.baseData.damage = 0;
                }

                component.IgnoreTileCollisionsFor(0.1f);
                component.Owner = source;
                component.Shooter = source.specRigidbody;
                if (gameObject.GetComponent<BlankShardModifier>())
                {
                    gameObject.GetComponent<BlankShardModifier>().reflect = shouldReflectInstead;
                }
                
            }

            if (amount > 10f)
            {
                List<BulletScriptSource> allBulletScriptSources = StaticReferenceManager.AllBulletScriptSources;
                for (int k = 0; k < allBulletScriptSources.Count; k++)
                {
                    BulletScriptSource bulletScriptSource = allBulletScriptSources[k];
                    if (!bulletScriptSource.IsEnded && bulletScriptSource.RootBullet != null && bulletScriptSource.RootBullet.EndOnBlank)
                    {
                        bulletScriptSource.ForceStop();
                    }
                }
            }
        }
    }
}
