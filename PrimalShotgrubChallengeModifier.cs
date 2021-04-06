using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class PrimalShotgrubChallengeModifier : ChallengeModifier
    {
		public PrimalShotgrubChallengeModifier()
		{
			this.DisplayName = "Primordial Chaos";
			this.AlternateLanguageDisplayName = string.Empty;
			this.AtlasSpriteName = "Big_Boss_icon_001";
			this.ValidInBossChambers = false;
			this.MutuallyExclusive = new List<ChallengeModifier>(0);
			
		}
		private void Start()
		{
			PlayerController[] allPlayers = GameManager.Instance.AllPlayers;
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].OnEnteredCombat += this.OnNewRoom;
			}
		}

		PlayerController player = GameManager.Instance.PrimaryPlayer;

		public void OnNewRoom()
		{
			BotsModule.Log("im so sorry");
			AIActor aiactor = Tools.SummonAtRandomPosition(PirmalShotgrub.guid, player);
			aiactor.CanDropCurrency = false;
			aiactor.HandleReinforcementFallIntoRoom(0f);
		}
	}
}
