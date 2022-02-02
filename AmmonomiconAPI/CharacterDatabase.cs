using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CharacterDatabase : ObjectDatabase<PickupObject>
	{
		public static CharacterDatabase Instance
		{
			get
			{
				if (CharacterDatabase.m_instance == null)
				{
					
					CharacterDatabase.m_instance = ScriptableObject.CreateInstance(typeof(CharacterDatabase)) as CharacterDatabase;
					CharacterDatabase.m_instance.Objects = new List<PickupObject>();

					foreach(var id in Tools.Spells)
                    {
						CharacterDatabase.m_instance.Objects.Add(PickupObjectDatabase.GetById(id));
                    }
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

		private static CharacterDatabase m_instance;
	}
}
