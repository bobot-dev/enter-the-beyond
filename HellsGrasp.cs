using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class HellsGrasp : PassiveItem
    {
        public static HellDragZoneController hellDrag;

        public static void Init()
        {
            //The name of the item
            string itemName = "Hell's Grasp";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "BotsMod/sprites/wip";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<HellsGrasp>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Got Ya!";
            string longDesc = "";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");

            //Adds the actual passive effect to the item


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;


            var forgeDungeon = DungeonDatabase.GetOrLoadByName("Base_Forge");
            hellDrag = forgeDungeon.PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("EndTimes")).First().overrideExactRoom.placedObjects
            .Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<HellDragZoneController>()[0];
        }


        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Player_PostProcessProjectile;
            base.Pickup(player);
        }



        private void Player_PostProcessProjectile(Projectile prog, float arg2)
        {
            float arg = 1f;
            if (Owner.CurrentGun && Owner.CurrentGun.DefaultModule != null)
            {
                float num = 0f;
                if (Owner.CurrentGun.Volley != null)
                {
                    List<ProjectileModule> projectiles = Owner.CurrentGun.Volley.projectiles;
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        num += projectiles[i].GetEstimatedShotsPerSecond(Owner.CurrentGun.reloadTime);
                    }
                }
                else if (Owner.CurrentGun.DefaultModule != null)
                {
                    num += Owner.CurrentGun.DefaultModule.GetEstimatedShotsPerSecond(Owner.CurrentGun.reloadTime);
                }
                if (num > 0f)
                {
                    arg = 3.5f / num;
                }
            }
            //if (UnityEngine.Random.Range(0, 1f) < 0.4/arg)
           // {
                prog.OnHitEnemy += OnHit;
                prog.AdjustPlayerProjectileTint(new Color(1.64f, 0.5f, 0), 100);
           // }


        }


        void OnHit(Projectile proj, SpeculativeRigidbody target, bool fatal)
        {
            if (!fatal && target.aiActor != null && target.aiActor.healthHaver != null && !target.aiActor.healthHaver.IsBoss && !target.aiActor.healthHaver.IsSubboss)
            {
                //VFX_PortalDirectlyToHell
                //var hand = (EnemyDatabase.GetOrLoadByGuid("cd88c3ce60c442e9aa5b3904d31652bc")).GetComponent<LichDeathController>().HellDragVFX;
                var hand = hellDrag.HellDragVFX;

                //var portal = (PickupObjectDatabase.GetById(155) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<BlackHoleDoer>().HellSynergyVFX;
                var portal = hellDrag.HoleObject;
                GameObject portalButReal = UnityEngine.Object.Instantiate<GameObject>(portal, target.UnitCenter, Quaternion.Euler(0f, 0f, 0f));
                portalButReal.GetComponent<MeshRenderer>().material = new Material(portalButReal.GetComponent<MeshRenderer>().material);
                portalButReal.GetComponent<MeshRenderer>().material.SetFloat("_UVDistCutoff", 0f);

                base.StartCoroutine(this.LerpHoleSize(portalButReal.GetComponent<MeshRenderer>().material, 0f, 0.15f, 0.3f, portalButReal, target.aiActor));

                //HellDraggerArbitrary component2 = UnityEngine.Object.Instantiate<GameObject>(hand).GetComponent<HellDraggerArbitrary>();
                //Grab(target.aiActor, component2.HellDragVFX);
                GrabPlayer(target.aiActor);
                target.aiActor.CorpseObject = null;
                target.aiActor.healthHaver.ApplyDamage(100000, Vector2.zero, "got ya bitch!", CoreDamageTypes.None, DamageCategory.Unstoppable, true);

                //base.StartCoroutine(this.LerpHoleSize(portalButReal.GetComponent<MeshRenderer>().material, 0.15f, 0, 0.3f, portalButReal, target.aiActor));
            }
        }
        private void GrabPlayer(AIActor enteredPlayer)
        {
            GameObject gameObject = enteredPlayer.PlayEffectOnActor(hellDrag.HellDragVFX, Vector3.zero, true, false, false);
            gameObject.SetActive(true);
            tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
            component.UpdateZDepth();
            component.attachParent = null;
            component.IsPerpendicular = false;
            component.HeightOffGround = 1f;
            component.UpdateZDepth();
            component.transform.position = component.transform.position.WithX(component.transform.position.x + 0.25f);

            if (enteredPlayer.ParentRoom != null)
            {
                component.transform.position = component.transform.position.WithY((float)enteredPlayer.ParentRoom.area.basePosition.y + 55f);
            }
            BotsModule.Log($"{component.transform.position} : {enteredPlayer.transform.position}");
            component.usesOverrideMaterial = true;
            component.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
        }

        private void Grab(AIActor enemy, GameObject HellDragVFX)
        {
            GameObject gameObject = enemy.PlayEffectOnActor(HellDragVFX, new Vector3(0f, 0, 0f), true, false, false);
           
            tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
            component.UpdateZDepth();
            component.attachParent = null;
            component.IsPerpendicular = false;
            component.HeightOffGround = 1f;
            component.UpdateZDepth();
            component.transform.position = component.transform.position.WithX(component.transform.position.x + 0.25f);
            component.transform.position = component.transform.position.WithY((float)enemy.ParentRoom.area.basePosition.y + 55f);
            component.usesOverrideMaterial = true;
            component.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
        }

        private IEnumerator LerpHoleSize(Material mat, float startSize, float endSize, float duration, GameObject obj, AIActor target)
        {
            float ela = 0f;
            while (ela < duration && target.sprite != null)
            {
                
                obj.transform.position = SpriteBottomCenter(target.sprite).XY().ToVector3ZisY(0f);
                ela += BraveTime.DeltaTime;
                this.SetHoleSize(mat, Mathf.Lerp(startSize, endSize, ela / duration));
                yield return null;
            }
            yield break;
        }

        private void SetHoleSize(Material mat, float size)
        {
            mat.SetFloat("_UVDistCutoff", size);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override DebrisObject Drop(PlayerController player)
        {
            return base.Drop(player);
        }


        public Vector3 SpriteBottomCenter(tk2dBaseSprite sprite)
        {
            return sprite.transform.position.WithX(sprite.transform.position.x + ((!sprite.FlipX) ? (sprite.GetUntrimmedBounds().size.x / 2f) : (-1f * sprite.GetUntrimmedBounds().size.x / 2f)));
        }
    }
}
