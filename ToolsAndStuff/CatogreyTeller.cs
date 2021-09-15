using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class CatogreyTeller : MonoBehaviour
	{
		void Update()
		{
			if (GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.specRigidbody != null)
			{
				BotsModule.CatogreyText.text = "Velocity: " + GameManager.Instance.PrimaryPlayer.specRigidbody.Velocity;
				
			} else
            {
				BotsModule.CatogreyText.text = "Velocity: " + "???";
			}

		}
	}
}