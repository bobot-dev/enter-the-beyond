using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class CrystalPlayerOrbital : BraveBehaviour
	{
		// Token: 0x06007547 RID: 30023 RVA: 0x002DBF40 File Offset: 0x002DA140
		public CrystalPlayerOrbital()
		{
			this.numKnives = 5;
			this.knifeDamage = 5f;
			this.circleRadius = 3f;
			this.rotationDegreesPerSecond = 360f;
			this.throwSpeed = 3f;
			this.throwRange = 2500f;
			this.throwRadius = 3f;
			this.radiusChangeDistance = 3f;
			this.m_currentShieldVelocity = Vector3.zero;
			this.m_currentShieldCenterOffset = Vector3.zero;
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06007548 RID: 30024 RVA: 0x002DBFC0 File Offset: 0x002DA1C0
		public bool IsActive
		{
			get
			{
				if (this.m_currentShieldVelocity != Vector3.zero)
				{
					return false;
				}
				for (int i = 0; i < this.m_knives.Count; i++)
				{
					if (this.m_knives[i] != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x002DC014 File Offset: 0x002DA214
		public void Initialize(PlayerController player, GameObject knifePrefab)
		{
			this.m_player = player;
			this.m_knives = new List<SpeculativeRigidbody>();			
			for (int i = 0; i < this.numKnives; i++)
			{
				Vector3 position = player.LockedApproximateSpriteCenter + Quaternion.Euler(0f, 0f, 360f / (float)this.numKnives * (float)i) * Vector3.right * this.circleRadius;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(knifePrefab, position, Quaternion.identity);
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.HeightOffGround = 1.5f;							
				SpeculativeRigidbody component3 = gameObject.GetComponent<SpeculativeRigidbody>();
				SpeculativeRigidbody speculativeRigidbody = component3;
				speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
				SpeculativeRigidbody speculativeRigidbody2 = component3;
				speculativeRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleCollision));
				SpeculativeRigidbody speculativeRigidbody3 = component3;
				speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
				this.m_knives.Add(component3);
			}
		}

		private void HandleTileCollision(CollisionData tileCollision)
		{
			this.m_currentShieldVelocity = -this.m_currentShieldVelocity;
			for (int i = 0; i < this.m_knives.Count; i++)
			{
				if (this.m_knives[i] != null && this.m_knives[i])
				{					
					this.m_knives[i].specRigidbody.CollideWithTileMap = false;
				}
			}
			//return code here
		}

		public void ThrowShield()
		{
			AkSoundEngine.PostEvent("Play_OBJ_daggershield_shot_01", base.gameObject);
			if (this.m_currentShieldVelocity == Vector3.zero)
			{
				Vector3 vector = this.m_player.unadjustedAimPoint - (Vector3)this.m_player.CenterPosition;
				this.m_currentShieldVelocity = vector.WithZ(0f).normalized * this.throwSpeed;
				for (int i = 0; i < this.m_knives.Count; i++)
				{
					if (this.m_knives[i] != null && this.m_knives[i])
					{
						this.m_knives[i].specRigidbody.CollideWithTileMap = true;
					}
				}
			}
		}

		protected Vector3 GetTargetPositionForKniveID(Vector3 center, int i, float radiusToUse)
		{
			float num = this.rotationDegreesPerSecond * this.m_elapsed % 360f;
			return center + Quaternion.Euler(0f, 0f, num + 360f / (float)this.numKnives * (float)i) * Vector3.right * radiusToUse;
		}

		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
		{			
			GameActor component2 = other.GetComponent<GameActor>();
			if (component2 is PlayerController)
			{
				PhysicsEngine.SkipCollision = true;
			}
			if (component2 is AIActor && !(component2 as AIActor).IsNormalEnemy)
			{
				PhysicsEngine.SkipCollision = true;
			}
		}

		private void HandleCollision(SpeculativeRigidbody other, SpeculativeRigidbody source, CollisionData collisionData)
		{
			if (other.GetComponent<AIActor>() != null)
			{
				HealthHaver component = other.GetComponent<HealthHaver>();
				float num = this.knifeDamage;
				component.ApplyDamage(num, Vector2.zero, "Guon Shard", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				int num2 = this.m_knives.IndexOf(source);
				if (num2 != -1)
				{
					this.m_knives[num2] = null;
				}
				//source.sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
				//UnityEngine.Object.Destroy(source.gameObject);
			}
			/*else if (other.GetComponent<Projectile>() != null)
			{
				Projectile component2 = other.GetComponent<Projectile>();
				if (component2.Owner is PlayerController)
				{
					return;
				}
				if (!this.m_lightknife)
				{
					component2.DieInAir(false, true, true, false);
				}
				this.remainingHealth -= component2.ModifiedDamage;
				if (this.remainingHealth <= 0f)
				{
					this.DestroyKnife(source);
				}
			}*/
		}

		private void Update()
		{
			if (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
			{
				return;
			}
			this.transform.rotation = Quaternion.Euler(0, 0, ((Vector2)this.m_player.unadjustedAimPoint - this.m_player.CenterPosition).ToAngle());
			this.m_elapsed += BraveTime.DeltaTime;
			//bool flag = this.m_currentShieldVelocity != Vector3.zero;
			Vector3 b = this.m_currentShieldVelocity * BraveTime.DeltaTime;
			this.m_currentShieldCenterOffset += b;
			if (this.m_currentShieldVelocity == Vector3.zero)
			{
				//BotsModule.Log("a");
				this.m_cachedOffsetBase = this.m_player.LockedApproximateSpriteCenter;
			}
			else
			{
				//BotsModule.Log("aa");
				this.m_traversedDistance += b.magnitude;
				if (this.m_traversedDistance > this.throwRange)
				{
					for (int i = 0; i < this.m_knives.Count; i++)
					{
						if (this.m_knives[i] != null && this.m_knives[i])
						{
							//this.m_knives[i].sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
							UnityEngine.Object.Destroy(this.m_knives[i].gameObject);
							this.m_knives[i] = null;
						}
					}
				}
			}
			Vector3 center = this.m_cachedOffsetBase + this.m_currentShieldCenterOffset;
			float radiusToUse = this.circleRadius;
			if (this.m_currentShieldVelocity != Vector3.zero)
			{
				radiusToUse = Mathf.Lerp(this.circleRadius, this.throwRadius, this.m_traversedDistance / this.radiusChangeDistance);
			}
			for (int j = 0; j < this.numKnives; j++)
			{
				if (this.m_knives[j] != null && this.m_knives[j])
				{
					Vector3 targetPositionForKniveID = this.GetTargetPositionForKniveID(center, j, radiusToUse);
					Vector3 vector = targetPositionForKniveID - this.m_knives[j].transform.position;
					Vector2 velocity = vector.XY() / BraveTime.DeltaTime;
					this.m_knives[j].Velocity = velocity;
					this.m_knives[j].sprite.UpdateZDepth();
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		public int numKnives;
		public float knifeDamage;
		public float circleRadius;
		public float rotationDegreesPerSecond;
		public float throwSpeed;
		public float throwRange;
		public float throwRadius;
		public float radiusChangeDistance;
		protected PlayerController m_player;
		public List<SpeculativeRigidbody> m_knives;
		protected float m_elapsed;
		protected float m_traversedDistance;
		protected Vector3 m_currentShieldVelocity;
		protected Vector3 m_currentShieldCenterOffset;
		private Vector3 m_cachedOffsetBase;
	}
}