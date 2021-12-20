using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CharacterDatabase : ObjectDatabase<PlayerController>
	{
		public static CharacterDatabase Instance
		{
			get
			{
				if (CharacterDatabase.m_instance == null)
				{
					
					CharacterDatabase.m_instance = ScriptableObject.CreateInstance(typeof(CharacterDatabase)) as CharacterDatabase;
					CharacterDatabase.m_instance.Objects = new List<PlayerController>();
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerRogue").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerConvict").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerRobot").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerMarine").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerGuide").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerCoopCultist").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerBullet").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerEevee").GetComponent<PlayerController>());
					CharacterDatabase.m_instance.Objects.Add(AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("PlayerGunslinger").GetComponent<PlayerController>());
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
