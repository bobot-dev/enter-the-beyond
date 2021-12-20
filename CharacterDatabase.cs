using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	/*
    class CharacterDatabase : SmartObjectDatabase<PlayerController, CharacterEntery>
    {
		public static CharacterDatabase Instance
		{
			get
			{
				if (CharacterDatabase.m_instance == null)
				{
					CharacterDatabase.m_instance = ScriptableObject.CreateInstance(this);
				}
				return CharacterDatabase.m_instance;
			}
		}
		public static bool HasInstance
		{
			get
			{
				return CharacterDatabase.m_instance != null;
			}
		}

		public static PlayerController GetByName(string name)
		{
			return CharacterDatabase.Instance.InternalGetByName(name);
		}

		private static CharacterDatabase m_instance;
	}

    class CharacterEntery : DatabaseEntry
    {
        public string name;
        public int id;
    }*/
}
