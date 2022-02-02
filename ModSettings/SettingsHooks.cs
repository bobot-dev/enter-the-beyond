using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using MonoMod.RuntimeDetour;
using Object = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;

namespace BotsMod
{
	static class SettingsHooks
    {
        public static void Init()
        {
			Hook LoadOptionHook = new Hook(
                    typeof(GameOptions).GetMethod("Load", BindingFlags.Static | BindingFlags.Public),
                    typeof(SettingsHooks).GetMethod("LoadOptionHook", BindingFlags.Static | BindingFlags.Public)
                );

            Hook SaveOptionsHook = new Hook(
                typeof(GameOptions).GetMethod("Save", BindingFlags.Static | BindingFlags.Public),
                typeof(SettingsHooks).GetMethod("SaveOptionsHook", BindingFlags.Static | BindingFlags.Public)
            );

        }

		public static bool SaveOptionsHook(Func<GameOptions, bool> orig)
		{
			if (!BeyondSettings.HasInstance)
			{
				BeyondSettings.Load();
			}


			BeyondSettings.Save();

			return orig(GameManager.Options);
		}

		public static void LoadOptionHook(Action<GameOptions> orig)
		{
			orig(GameManager.Options);

			if (!BeyondSettings.HasInstance)
			{
				BeyondSettings.Load();
			}
		}
	}
}
