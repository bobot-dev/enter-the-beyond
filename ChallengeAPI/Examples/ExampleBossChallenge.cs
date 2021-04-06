using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChallengeAPI
{
	/// <summary>
	/// An example boss challenge that multiplies the health of all bosses in the room by it's healthMultiplier variable.
	/// </summary>
    public class ExampleBossChallenge : ChallengeModifier
    {
        public void Start()
        {
			Dungeonator.RoomHandler room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
			List<AIActor> roomEnemies = room.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All);
			foreach(AIActor enemy in roomEnemies)
			{
				if (enemy && enemy.healthHaver && enemy.healthHaver.IsBoss)
				{
					enemy.healthHaver.SetHealthMaximum(enemy.healthHaver.GetMaxHealth() * healthMultiplier, null, true);
				}
			}
		}

		public float healthMultiplier;
    }
}
