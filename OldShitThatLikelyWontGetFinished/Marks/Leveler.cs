using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using StatType = PlayerStats.StatType;
using MonoMod.RuntimeDetour;
using SaveAPI;

namespace BotsMod
{
    public class ScrapBook : GunVolleyModificationItem
    {
        public static void Init()
        {
            string itemName = "Scrap Book"; //The name of the item
            string resourceName = "BotsMod/sprites/wip"; //Refers to an embedded png in the project. Make sure to embed your resources!

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ScrapBook>();

            //Generate a new GameObject with a sprite component
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Hold [Reload] to use";
            string longDesc = "haha fuck you no long description for you :)";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            

            //Set the rarity of the item
            //item.AddStatDowns();
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public float healthUps;
        public int levelsToSpend = 2;
        float xpToLevel = 700;
        int xp = 0;
        LevelerGUIController m_guiController;
        bool didStatDowns = false;
        ExplosionData IBombExplosionData = new ExplosionData()
        {
            damageRadius = 2.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 15,
            doDestroyProjectiles = false,
            doForce = false,
            debrisForce = 30f,
            preventPlayerForce = false,
            explosionDelay = 0.1f,
            usesComprehensiveDelay = false,
            doScreenShake = false,
            ss = new ScreenShakeSettings(),
            playDefaultSFX = true,
        };

        /*public override void MidGameSerialize(List<object> data)
        {
            base.MidGameSerialize(data);
            data.Add(levelsToSpend);
            data.Add(xp);
            data.Add(Owner.healthHaver.GetCurrentHealth());
            for (int i = 0; i < marks.Length; i++)
            {
                data.Add(marks[i].level);
            }
        }

        public override void MidGameDeserialize(List<object> data)
        {
            base.MidGameDeserialize(data);
            int levelsToSpend = (int)data[0];
            int xp = (int)data[1];
            float hp = (float)data[2];
            data.RemoveRange(0, 3);
            for (int i = 0; i < marks.Length; i++)
            {
                for (int l = 0; l < (int)data[i]; l++)
                {
                    AddLevel(i, true);
                }
            }

            Owner.healthHaver.ForceSetCurrentHealth(hp);
            this.levelsToSpend = levelsToSpend;
            this.xp = xp;
            //m_guiController.UpdateXP(xp / xpToLevel);
        }*/

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            var defaultExplosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            //IBombExplosionData.effect = defaultExplosion.effect;
            //IBombExplosionData.ignoreList = defaultExplosion.ignoreList;

            m_guiController = player.gameObject.GetOrAddComponent<LevelerGUIController>();
            m_guiController.Build(this, player, ref marks);
            //player.OnDealtDamageContext += OnDealtDamage;
        }


        /*public void UpdateCost()
        {
            xpToLevel *= 1.05f;
        }

        public void AddLevel(int selectedStat, bool deserialization = false)
        {

            if (levelsToSpend <= 0 && !deserialization)
            {
                AkSoundEngine.PostEvent("Play_obj_computer_break_01", Owner.gameObject);
                return;
            }
            else
            {
                try
                {
                    Mark stat = marks[selectedStat];
                    if (stat.level < 8)
                    {
                        stat.level++;
                        if (!deserialization)
                        {
                            levelsToSpend--;
                            AkSoundEngine.PostEvent("Play_OBJ_metacoin_collect_01", Owner.gameObject);
                        }
                        //m_guiController.SetPoints(levelsToSpend);
                        UpdateCost();
                        if (stat.name.Equals("vitality"))
                        {
                            HandleHealthUp(stat, deserialization);
                        }
                        else
                        {
                            foreach (var modifier in stat.modifiers)
                            {
                                this.AddPassiveStatModifier(modifier);
                            }
                        }
                        Owner.stats.RecalculateStats(Owner, true, true);
                    }
                    else
                    {
                        AkSoundEngine.PostEvent("Play_obj_computer_break_01", Owner.gameObject);
                    }
                }
                catch (Exception e)
                {
                    DebugUtility.PrintException(e);
                }
            }
        }
        private void HandleHealthUp(Stat stat, bool deserialization = false)
        {
            if (Owner.characterIdentity == PlayableCharacters.Robot && !deserialization)
            {
                healthUps += .5f;
                if (healthUps >= 1)
                {
                    healthUps -= 1;
                    Owner.healthHaver.Armor++;
                }
            }
            else
            {
                healthUps += .25f;
                if (healthUps >= 1)
                {
                    this.AddPassiveStatModifier(stat.modifiers[0]);
                    healthUps -= 1;
                }
                else if (!deserialization)
                {
                    LootEngine.SpawnHealth(Owner.sprite.WorldCenter, 1, null);
                }
            }
        }

        private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
        {
            if (!fatal) return;
            xp += (int)target.GetMaxHealth();
            if (xp >= xpToLevel)
            {
                int levels = xp / (int)xpToLevel;
                xp %= (int)xpToLevel;
                levelsToSpend += levels;
                //m_guiController.SetPoints(levelsToSpend);
                if (!blinking)
                {
                    blinking = true;
                    StartCoroutine(LevelUpEffect(player));
                    var fx = Tools.shared_auto_001.LoadAsset<GameObject>("vfx_synergrace_bless");
                    player.PlayEffectOnActor(fx, new Vector3(-10 / 16f, 0, 0));
                    AkSoundEngine.PostEvent("Play_NPC_magic_blessing_01", player.gameObject);
                }
            }
            //m_guiController.UpdateXP(xp / xpToLevel);
        }*/


        Color onColor = new Color(0, 100, 100);
        bool blinking = false;
        IEnumerator LevelUpEffect(PlayerController player)
        {
            bool on = false;
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);
            Color orig = outlineMaterial.GetColor("_OverrideColor");
            for (int i = 0; i <= 6; i++)
            {
                on = !on;
                if (on)
                    outlineMaterial.SetColor("_OverrideColor", onColor);
                else
                    outlineMaterial.SetColor("_OverrideColor", orig);
                if (i == 6) break;
                yield return new WaitForSeconds(.2f);
            }

            outlineMaterial.SetColor("_OverrideColor", orig);
            blinking = false;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            //player.OnDealtDamageContext -= OnDealtDamage;

            player.gameObject.GetComponent<LevelerGUIController>()?.Destroy();
            return base.Drop(player);
        }

        public Mark[] marks = new Mark[]
        {
            new Mark() { name = "Dragun", flag = CharacterSpecificGungeonFlags.NONE},
            new Mark() { name = "Lich", flag = CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL},
            new Mark() { name = "Past", flag = CharacterSpecificGungeonFlags.KILLED_PAST},
            new Mark() { name = "PastAlt", flag = CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME},

        };

        //negate damage
        //dodge roll as separate stat

        public class Mark
        {
            public string name;
            //public bool usesCustomFlag;
            public CharacterSpecificGungeonFlags flag;
            //public CustomCharacterSpecificGungeonFlags customFlags;


            public Mark Copy()
            {
                Mark copy = new Mark()
                {
                    name = this.name,
                    flag = this.flag,
                    
                };
                return copy;
            }
        }

        public class Stat
        {
            public string name;
            public int level;
            public List<StatModifier> modifiers;
            public float[] penalties;

            public Stat Copy()
            {
                Stat copy = new Stat()
                {
                    name = this.name,
                    level = this.level,
                    modifiers = new List<StatModifier>()
                };
                foreach (var mod in modifiers)
                {
                    copy.modifiers.Add(new StatModifier()
                    {
                        amount = mod.amount,
                        modifyType = mod.modifyType,
                        statToBoost = mod.statToBoost
                    });
                }
                return copy;
            }
        }
    }
}
