using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class BlankShardModifier : MonoBehaviour
	{
		public BlankShardModifier()
		{
			this.DamagePercentGainPerSnack = 0.25f;
			this.MaxMultiplier = 3f;
			this.HungryRadius = 3f;
			this.MaximumBulletsEaten = 10;
		}

		private void Awake()
		{
			this.m_projectile = base.GetComponent<Projectile>();
			//this.m_projectile.AdjustPlayerProjectileTint(new Color(0.45f, 0.3f, 0.87f), 2, 0f);
			this.m_projectile.collidesWithProjectiles = true;
			SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision += this.HandlePreCollision;




			
		}
		bool hasDoneSetup;
		private void Update()
		{
			if (this.m_sated)
			{
				return;
			}

			if (!hasDoneSetup)
            {
				if (m_projectile.Owner != null && m_projectile.Owner is PlayerController)
				{
					var player = m_projectile.Owner as PlayerController;

					if (player.HasPickupID(321))
					{
						m_projectile.baseData.damage += 3;
					}
					if (player.HasPickupID(322))
					{
						m_projectile.baseData.force += 10;
						m_projectile.StunApplyChance = 100;
						m_projectile.AppliedStunDuration = 3;
						m_projectile.AppliesStun = true;
					}
					if (player.HasPickupID(342))
					{
						m_projectile.AppliesPoison = true;
						m_projectile.PoisonApplyChance = 0.5f;
						m_projectile.healthEffect = (PickupObjectDatabase.GetById(342) as BlankModificationItem).BlankPoisonEffect;

					}

					if (player.HasPickupID(343))
					{
						m_projectile.AppliesFire = true;
						m_projectile.FireApplyChance = 0.5f;
						m_projectile.fireEffect = (PickupObjectDatabase.GetById(343) as BlankModificationItem).BlankFireEffect;

					}

					if (player.HasPickupID(344))
					{
						m_projectile.AppliesFreeze = true;
						m_projectile.FreezeApplyChance = 0.5f;
						m_projectile.freezeEffect = (PickupObjectDatabase.GetById(344) as BlankModificationItem).BlankFreezeEffect;

					}

					if (player.HasPickupID(325))
					{

						m_projectile.baseData.force += 10;
						m_projectile.StunApplyChance = 100;
						m_projectile.AppliedStunDuration = 1;
						m_projectile.AppliesStun = true;

						m_projectile.AppliesFire = true;
						m_projectile.FireApplyChance = 0.5f;
						m_projectile.fireEffect = (PickupObjectDatabase.GetById(325) as BlankModificationItem).BlankFireEffect;

						m_projectile.AppliesPoison = true;
						m_projectile.PoisonApplyChance = 0.5f;
						m_projectile.healthEffect = (PickupObjectDatabase.GetById(325) as BlankModificationItem).BlankPoisonEffect;

						m_projectile.AppliesFreeze = true;
						m_projectile.FreezeApplyChance = 0.5f;
						m_projectile.freezeEffect = (PickupObjectDatabase.GetById(325) as BlankModificationItem).BlankFreezeEffect;

					}

                    

				}
				OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
				orbitProjectileMotionModule.lifespan = 15f;
			//	m_projectile.projectile.OverrideMotionModule = orbitProjectileMotionModule;

				hasDoneSetup = true;
			}

			Vector2 b = this.m_projectile.transform.position.XY();
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile = StaticReferenceManager.AllProjectiles[i];
				if (projectile && projectile.Owner is AIActor)
				{
					float sqrMagnitude = (projectile.transform.position.XY() - b).sqrMagnitude;
					if (sqrMagnitude < this.HungryRadius)
					{
						this.EatBullet(projectile);
					}
				}
			}
		}

		

		private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (this.m_sated)
			{
				return;
			}
			if (otherRigidbody && otherRigidbody.projectile)
			{
				if (otherRigidbody.projectile.Owner is AIActor)
				{
					this.EatBullet(otherRigidbody.projectile);
				}
				PhysicsEngine.SkipCollision = true;
			}
		}

		private void EatBullet(Projectile other)
		{
			if (this.m_sated)
			{
				return;
			}
			if (reflect)
            {
				PassiveReflectItem.ReflectBullet(other, true, this.m_projectile.Owner, 10f, 1f, 1f, 0f);
			}
			else
            {
				other.DieInAir(false, true, true, false);
			}
			


			/*float num = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
			this.m_numberOfBulletsEaten++;
			float num2 = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
			float b = num2 / num;
			float num3 = Mathf.Max(1f, b);
			if (num3 > 1f)
			{
				this.m_projectile.RuntimeUpdateScale(num3);
				this.m_projectile.baseData.damage *= num3;
			}
			if (this.m_numberOfBulletsEaten >= this.MaximumBulletsEaten)
			{
				this.m_sated = true;
				this.m_projectile.AdjustPlayerProjectileTint(this.m_projectile.DefaultTintColor, 3, 0f);
			}*/
		}

		public bool reflect;

		public float DamagePercentGainPerSnack;

		public float MaxMultiplier;

		public float HungryRadius;

		public int MaximumBulletsEaten;

		private Projectile m_projectile;

		private int m_numberOfBulletsEaten;

		private bool m_sated;
	}

}
