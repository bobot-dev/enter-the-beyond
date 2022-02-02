using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class OverheatGunBehaviour : GunBehaviour
    {
		void Update()
		{
			if (this.gun.CurrentOwner != null && this.gun.CurrentOwner is PlayerController)
			{
				if (this.gun.IsFiring)
				{
					waitTime = 0;
					if (heatLevel >= GetModMaxOverheat(this.gun.CurrentOwner))
					{
						if (!overheated)
						{
							overheated = true;
							this.gun.CeaseAttack(false, null);
							AkSoundEngine.PostEvent("Play_Overheat_Hiss", this.gameObject);
							if (burnOnOverHeat)
                            {
								(this.gun.CurrentOwner as PlayerController).IsOnFire = true;
							}
						}
					}
					else if (heatLevel < GetModMaxOverheat(this.gun.CurrentOwner))
					{
						heatLevel += (Time.deltaTime * 10);
					}
				}
				else
				{
					if (waitTime >= (overheated ? maxWaitTime : maxWaitTime * 0.8f))
					{
						if (heatLevel > 0)
						{
							//ETGModConsole.Log($"{overheated} w-{waitTime} h-{heatLevel}");
							heatLevel -= (Time.deltaTime * (overheated ? 8f : 12));
							if (heatLevel <= 0)
							{
								overheated = false;
								heatLevel = 0;
							}
						}
					}
					else if (waitTime < maxWaitTime)
					{
						waitTime += (Time.deltaTime * 10);
					}
				}
			}
		}

		public int GetModMaxOverheat(GameActor owner)
		{
			if (this.maxHeat == 1)
			{
				return this.maxHeat;
			}
			if (!(owner != null) || !(owner is PlayerController))
			{
				return this.maxHeat;
			}
			PlayerController playerController = owner as PlayerController;
			float statValue = playerController.stats.GetStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier);
			float statValue2 = playerController.stats.GetStatValue(PlayerStats.StatType.TarnisherClipCapacityMultiplier);
			int num = Mathf.FloorToInt((float)this.maxHeat * statValue * statValue2);
			if (num < 0)
			{
				return num;
			}
			return Mathf.Max(num, 1);
		}

		//public GameActorFireEffect fire;
		public float heatLevel;
		public int maxHeat = 30;
		public bool overheated;
		public bool burnOnOverHeat;

		float waitTime;
		float maxWaitTime = 1f;

	}
}
