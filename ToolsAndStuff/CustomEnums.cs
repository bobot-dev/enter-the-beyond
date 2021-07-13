using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class CustomEnums
    {
		public enum QuickstartCharacter
		{

			LAST_USED,

			PILOT,

			CONVICT,

			SOLDIER,

			GUIDE,

			BULLET,

			ROBOT,

			LOST

		}

		public enum CustomShopCurrencyType
		{
			COINS,
			
			META_CURRENCY,
			
			KEYS,

			BLANKS,

			HEARTS,
		}

		public enum CustomAdditionalShopType
		{
			NONE = 0,
			GOOP = 1,
			BLANK = 2,
			KEY = 3,
			CURSE = 4,
			TRUCK = 5,
			FOYER_META = 6,
			BLACKSMITH = 7,
			RESRAT_SHORTCUT = 8,
			DEVIL_DEAL = 9,
		}

		public enum CustomNotificationColor
		{
			SILVER,
			GOLD,
			PURPLE,
			BEYOND
		}

		public enum CustomSpecialChestIdentifier
		{
			NORMAL = 0,
			RAT = 1,
			SECRET_RAINBOW = 2,
			TEST = 3
		}

		public enum CustomPageType
		{
			NONE = 0,
			EQUIPMENT_LEFT = 1,
			EQUIPMENT_RIGHT = 2,
			GUNS_LEFT = 3,
			GUNS_RIGHT = 4,
			ITEMS_LEFT = 5,
			ITEMS_RIGHT = 6,
			ENEMIES_LEFT = 7,
			ENEMIES_RIGHT = 8,
			BOSSES_LEFT = 9,
			BOSSES_RIGHT = 10,
			DEATH_LEFT = 11,
			DEATH_RIGHT = 12,
			MODS_LEFT = 13,
			MODS_RIGHT = 14
		}
	}
}
