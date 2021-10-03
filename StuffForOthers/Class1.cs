using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using System.Collections.Generic;
using System.Reflection;

namespace AAAAAAAAAA
{
    public class a
    {
		public static void Init()
        {
			if (UiTable().ContainsKey("#CASTLE_SHORTNAME"))
            {
				UiTable()["#CASTLE_SHORTNAME"].AddString("go fuck yourself", 1);
				UiTable()["#CASTLE_NAME"].AddString("please just work", 1);

			}
		}

		public static Dictionary<string, StringTableManager.StringCollection> UiTable()
		{
			FieldInfo _uiTable = typeof(StringTableManager).GetField("m_uiTable", BindingFlags.NonPublic | BindingFlags.Static);
			FieldInfo _backupUiTable = typeof(StringTableManager).GetField("m_backupUiTable", BindingFlags.NonPublic | BindingFlags.Static);
			FieldInfo _currentSubDirectory = typeof(StringTableManager).GetField("m_currentSubDirectory", BindingFlags.NonPublic | BindingFlags.Static);
			BotsMod.BotsModule.Log("1");
			
			BotsMod.BotsModule.Log(StringTableManager.GetUIString("#CASTLE_NAME"));
			if (((Dictionary<string, StringTableManager.StringCollection>)_uiTable.GetValue(null)) == null)
			{				
				_uiTable.SetValue(null, typeof(StringTableManager).GetMethod("LoadUITable", BindingFlags.NonPublic |
					BindingFlags.Static).Invoke(null, new object[] { (string)_currentSubDirectory.GetValue(null) }));
			}
			BotsMod.BotsModule.Log("2");
			if (((Dictionary<string, StringTableManager.StringCollection>)_backupUiTable.GetValue(null)) == null)
			{
				//StringTableManager.m_backupUiTable = StringTableManager.LoadUITable(StringTableManager.m_currentSubDirectory);


				_backupUiTable.SetValue(null, typeof(StringTableManager).GetMethod("LoadUITable", BindingFlags.NonPublic |
					BindingFlags.Static).Invoke(null, new object[] { (string)_currentSubDirectory.GetValue(null) }));

			}
			BotsMod.BotsModule.Log("3");
			return ((Dictionary<string, StringTableManager.StringCollection>)_uiTable.GetValue(null));
		}
	}
}