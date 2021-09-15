using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class CoinGunModifier : MonoBehaviour
	{
		public CoinGunModifier()
		{
			this.m_counter = 1;
		}

		public void Start()
		{
			this.m_gun = base.GetComponent<Gun>();
		}

		private void Update()
		{
			if (!this.m_callbackInitialized && this.m_gun.CurrentOwner is PlayerController)
			{
				this.m_callbackInitialized = true;
				this.m_cachedPlayer = (this.m_gun.CurrentOwner as PlayerController);
				this.m_cachedPlayer.OnTriedToInitiateAttack += this.HandleTriedToInitiateAttack;
			}
			else if (this.m_callbackInitialized && !(this.m_gun.CurrentOwner is PlayerController))
			{
				this.m_callbackInitialized = false;
				if (this.m_cachedPlayer)
				{
					this.m_cachedPlayer.OnTriedToInitiateAttack -= this.HandleTriedToInitiateAttack;
					this.m_cachedPlayer = null;
				}
			}
			if (this.m_wasReloading && this.m_gun && !this.m_gun.IsReloading)
			{
				this.m_wasReloading = false;
			}
		}
		private void HandleTriedToInitiateAttack(PlayerController sourcePlayer)
		{
			if (m_gun.IsReloading)
			{
				if (!this.m_wasReloading)
				{
					this.m_counter = 0;
					this.m_wasReloading = true;
				}
				if (m_counter <= 4)
				{

					this.m_counter++;
					var coinObj = UnityEngine.Object.Instantiate(HellsRevolver.coinPrefab, GameManager.Instance.PrimaryPlayer.gameObject.transform.position, Quaternion.identity);
					AkSoundEngine.PostEvent("Play_OBJ_ironcoin_flip_01", base.gameObject);

					var num = sourcePlayer.CurrentGun.CurrentAngle;

					if (num >= 0 && num <= 90)
                    {

                    } else if (num >= 91 && num <= 180)
					{

					}
					else if (num >= 181 && num <= 270)
					{

					}
					else if (num >= 271 && num <= 360)
					{

					}
					
					coinObj.GetComponent<SpeculativeRigidbody>().Velocity = (BraveMathCollege.DegreesToVector(num) *5) + sourcePlayer.specRigidbody.Velocity;//new Vector3(0, sourcePlayer.CurrentGun.transform.eulerAngles.z, 0) * 3 * Time.timeScale;
					//coinObj.GetComponent<SpeculativeRigidbody>().Velocity.y *= 1.4f;
					
				}

			}
			else
			{
				this.m_wasReloading = false;
			}
		}

		private Gun m_gun;

		private bool m_callbackInitialized;

		private PlayerController m_cachedPlayer;

		private int m_counter;

		private bool m_wasReloading;
	}
}
