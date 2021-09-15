using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class ProjBoostModifer : BraveBehaviour
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
			BotsModule.Log("fish");
			if (otherRigidbody.GetComponent<PlayerController>() != null)
            {
				var player = otherRigidbody.GetComponent<PlayerController>();
				if (player.CurrentRollState == PlayerController.DodgeRollState.InAir)
                {
					myRigidbody.projectile.baseData.speed *= 3;
					myRigidbody.projectile.OnDestruction += Obj_OnDestruction;

				}

			}
        }

		private void Obj_OnDestruction(Projectile obj)
		{
			Exploder.DoDefaultExplosion(obj.sprite.WorldCenter, Vector2.zero);
		}
	}
}
