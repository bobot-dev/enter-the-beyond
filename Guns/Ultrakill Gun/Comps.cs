using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class Bleed : MonoBehaviour
	{
		public int bloodAmount = 1;

		void Start()
		{
			if (this.GetComponent<Projectile>() != null)
			{
				this.GetComponent<Projectile>().OnHitEnemy += OnHit;
			}
		}

		private void OnHit(Projectile projectile, SpeculativeRigidbody target, bool fatal)
		{
			//ETGModConsole.Log("hit something without a brain");
			if (target.aiActor != null && fatal)
			{
				//ETGModConsole.Log("murder");
				for (int i = 0; i < bloodAmount; i++)
				{
					Instantiate(PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject, target.UnitCenter, Quaternion.identity);
				}

			}

		}
	}

	class ProjBoost : BraveBehaviour
	{
		public void Start()
		{

			if (this.specRigidbody != null)
			{
				this.specRigidbody.OnPreRigidbodyCollision += OnHitSomething;
			}

		}


		void OnHitSomething(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			
			if (otherRigidbody.gameObject.GetComponent<PlayerController>() != null)
			{
				BotsModule.Log("fish");
				var player = otherRigidbody.GetComponent<PlayerController>();
				//if (player.CurrentRollState == PlayerController.DodgeRollState.InAir)
				//{
					myRigidbody.projectile.baseData.speed *= 3;
				myRigidbody.projectile.UpdateSpeed();
					myRigidbody.projectile.OnDestruction += Obj_OnDestruction;
				Exploder.DoDefaultExplosion(myRigidbody.sprite.WorldCenter, Vector2.zero);
				//}

			}
		}

		private void Obj_OnDestruction(Projectile obj)
		{
			
		}
	}
}
