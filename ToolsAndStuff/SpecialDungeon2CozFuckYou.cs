using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Dungeonator;
using UnityEngine;
using MonoMod.RuntimeDetour;
using BotsMod;

namespace BotsMod
{
    abstract class SpecialDungeon2CozFuckYou
    {
        public static void Init()
        {
            Hook hook = new Hook(
                typeof(DungeonDatabase).GetMethod("GetOrLoadByName", BindingFlags.Public | BindingFlags.Static),
                typeof(SpecialDungeon2CozFuckYou).GetMethod("GetOrLoadByNameHook")
            );
            foreach (Type type in Assembly.GetAssembly(typeof(SpecialDungeon2CozFuckYou)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SpecialDungeon2CozFuckYou))))
            {
                SpecialDungeon2CozFuckYou dungeon = (SpecialDungeon2CozFuckYou)Activator.CreateInstance(type);
                SpecialDungeon2CozFuckYou.RegisterCustomDungeon(dungeon.BuildDungeon, dungeon.PrefabPath);

                self = dungeon;

                GameLevelDefinition def = new GameLevelDefinition
                {
                    bossDpsCap = dungeon.BossDPSCap,
                    dungeonPrefabPath = dungeon.PrefabPath,
                    damageCap = dungeon.DamageCap,
                    dungeonSceneName = dungeon.SceneName,
                    enemyHealthMultiplier = dungeon.EnemyHealthMultiplier,
                    flowEntries = dungeon.FlowEntries,
                    priceMultiplier = dungeon.PriceMultiplier,
                    secretDoorHealthMultiplier = dungeon.SecretDoorHealthMultiplier
                };
                GameManager.Instance.customFloors.Add(def);
                addedLevelDefs.Add(def);
            }
            ETGModMainBehaviour.Instance.gameObject.AddComponent<DetectMissingDefinitions>();
        }

        private static SpecialDungeon2CozFuckYou self;

        public static Dungeon GetOrLoadByNameHook(Func<string, Dungeon> orig, string prefabPath)
        {
            Func<Dungeon, Dungeon> buildDungeon;
            if (customDungeons.TryGetValue(prefabPath, out buildDungeon))
            {
                Dungeon d = buildDungeon(GetOrLoadByNameOrig("base_sewer"));
                DebugTime.RecordStartTime();
                DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { prefabPath });
                return d;
            }
            else
            {
                return orig(prefabPath);
            }
        }

        public static Dungeon GetOrLoadByNameOrig(string name)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("dungeons/" + name.ToLower());
            DebugTime.RecordStartTime();
            Dungeon component = assetBundle.LoadAsset<GameObject>(name).GetComponent<Dungeon>();
            DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[]
            {
                name
            });
            return component;
        }
        static int number = 0;
        public static void RegisterCustomDungeon(Func<Dungeon, Dungeon> buildDungeon, string prefabPath)
        {
            if (number < 1)
            {
                customDungeons.Add(prefabPath, buildDungeon);
                number++;
            }
            
        }

        public static Dictionary<string, Func<Dungeon, Dungeon>> customDungeons = new Dictionary<string, Func<Dungeon, Dungeon>>();
        public static List<GameLevelDefinition> addedLevelDefs = new List<GameLevelDefinition>();
        public abstract Dungeon BuildDungeon(Dungeon orig);
        public abstract string PrefabPath { get; }
        public abstract string SceneName { get; }
        //public abstract int BaseDungeon { get; }
        public abstract float BossDPSCap { get; }
        public abstract float DamageCap { get; }
        public abstract float EnemyHealthMultiplier { get; }
        public abstract float PriceMultiplier { get; }
        public abstract float SecretDoorHealthMultiplier { get; }
        public virtual List<DungeonFlowLevelEntry> FlowEntries { get { return new List<DungeonFlowLevelEntry>(0); } }
    }

    public class DetectMissingDefinitions2CozFuckYou : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.HasInstance)
            {
                foreach (GameLevelDefinition def in SpecialDungeon2CozFuckYou.addedLevelDefs)
                {
                    if (!GameManager.Instance.customFloors.Contains(def))
                    {
                        GameManager.Instance.customFloors.Add(def);
                    }
                }
            }
 
        }
    }
}
