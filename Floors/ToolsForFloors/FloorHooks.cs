using Dungeonator;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class FloorHooks
    {
        public static void Init()
        {
            new Hook(typeof(FlowDatabase).GetMethod("GetOrLoadByName", BindingFlags.Static | BindingFlags.Public), typeof(BeyondDungeonFlows).GetMethod("LoadCustomFlow", BindingFlags.Static | BindingFlags.Public));


        }

        public static Dungeon GetOrLoadByNameHook(Func<string, Dungeon> orig, string name)
        {
            Dungeon dungeon = null;
            string dungeonPrefabTemplate = "Base_ResourcefulRat";
            if (name.ToLower() == "base_lostpast")
            {
                dungeon = LostPastDungeon.LostPastGeon(GetOrLoadByName_Orig(dungeonPrefabTemplate));
            }
            if (name.ToLower() == "base_beyond")
            {
                dungeon = BeyondDungeon.BeyondGeon(GetOrLoadByName_Orig(dungeonPrefabTemplate));
            }
            if (dungeon)
            {
                DebugTime.RecordStartTime();
                DebugTime.Log("AssetBundle.LoadAsset<Dungeon>({0})", new object[] { name });
                return dungeon;
            }
            else
            {
                return orig(name);
            }
        }

        public static Dungeon GetOrLoadByName_Orig(string name)
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
    }
}
