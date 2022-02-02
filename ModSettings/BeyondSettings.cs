using FullSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    [fsObject]
    class BeyondSettings
    {



		public static bool HasInstance
		{
			get
			{
				return BeyondSettings.m_instance != null;
			}
		}

		public static BeyondSettings Instance
		{
			get
			{
				if (BeyondSettings.m_instance == null)
				{
					BotsModule.Log("Trying to access BeyondSettings before it has been initialized!! aka go yell at bot", "FF0000");
				}
				return BeyondSettings.m_instance;
			}
		}


		public static void Load()
		{
			SaveManager.Init();
			BeyondSettings gameOptions = null;
			bool flag = SaveManager.Load<BeyondSettings>(BeyondSettingSave, out gameOptions, true, 0U, null, null);
			if (!flag)
			{
				int num = 0;
				while (num < 3 && !flag)
				{
					if (num != (int)SaveManager.CurrentSaveSlot)
					{
						gameOptions = null;
						SaveManager.SaveType optionsSave = BeyondSettingSave;
						ref BeyondSettings obj = ref gameOptions;
						bool allowDecrypted = true;
						SaveManager.SaveSlot? overrideSaveSlot = new SaveManager.SaveSlot?((SaveManager.SaveSlot)num);
						flag = SaveManager.Load<BeyondSettings>(optionsSave, out obj, allowDecrypted, 0U, null, overrideSaveSlot);
						flag &= (gameOptions != null);
					}
					num++;
				}
			}
			if (!flag || gameOptions == null)
			{
				BeyondSettings.m_instance = new BeyondSettings();
			}
			else
			{
				BeyondSettings.m_instance = gameOptions;
			}
		}

		public static bool Save()
		{
			return SaveManager.Save<BeyondSettings>(BeyondSettings.Instance, BeyondSettingSave, 0, 0U, null);
		}


		[fsProperty]
		public bool allowSpindownInsanity = false;

		[fsProperty]
		public bool titleScreenOverrideEnabled = true;

		[fsProperty]
		public bool debug = false;

		private static BeyondSettings m_instance;

		public static SaveManager.SaveType BeyondSettingSave = new SaveManager.SaveType
		{
			filePattern = "Slot{0}.beyondOptions",
			encrypted = false,
			legacyFilePattern = "beyondOptionsSlot{0}.txt"
		};
	}
}
