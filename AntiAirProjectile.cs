using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class AntiAirProjectile : BraveBehaviour
	{
		public AntiAirProjectile()
		{
			this.HomingRadius = 2f;
			this.AngularVelocity = 180f;
		}

		private void Start()
		{
			if (!this.m_projectile)
			{
				this.m_projectile = base.GetComponent<Projectile>();
			}
			Projectile projectile = this.m_projectile;
			projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
		}

		public void AssignProjectile(Projectile source)
		{
			this.m_projectile = source;
			this.m_projectile.OnHitEnemy += HandleHitEnemy;
		}
		private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
		{
			if (hitEffectHandler.aiActor != null && hitEffectHandler.aiActor.IsFlying)
            {
				sourceProjectile.baseData.damage *= 1.5f;

			}
		}

		private Vector2 ModifyVelocity(Vector2 inVel)
		{
			Vector2 vector = inVel;
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_projectile.LastPosition.IntXY(VectorConversions.Floor));
			List<AIActor> activeEnemies = absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null || activeEnemies.Count == 0)
			{
				return inVel;
			}
			float num = float.MaxValue;
			Vector2 vector2 = Vector2.zero;
			AIActor x = null;
			Vector2 b = (!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor = activeEnemies[i];
				if (aiactor && aiactor.IsWorthShootingAt && !aiactor.IsGone && aiactor.IsFlying && aiactor.healthHaver.IsAlive)
				{
					Vector2 vector3 = aiactor.CenterPosition - b;
					float sqrMagnitude = vector3.sqrMagnitude;
					if (sqrMagnitude < num)
					{
						vector2 = vector3;
						num = sqrMagnitude;
						x = aiactor;
					}
				}
			}
			num = Mathf.Sqrt(num);
			if (num < this.HomingRadius && x != null)
			{
				float num2 = 1f - num / this.HomingRadius;
				float target = vector2.ToAngle();
				float num3 = inVel.ToAngle();
				float maxDelta = this.AngularVelocity * num2 * this.m_projectile.LocalDeltaTime;
				float num4 = Mathf.MoveTowardsAngle(num3, target, maxDelta);
				if (this.m_projectile is HelixProjectile)
				{
					float angleDiff = num4 - num3;
					(this.m_projectile as HelixProjectile).AdjustRightVector(angleDiff);
				}
				else
				{
					if (this.m_projectile.shouldRotate)
					{
						base.transform.rotation = Quaternion.Euler(0f, 0f, num4);
					}
					vector = BraveMathCollege.DegreesToVector(num4, inVel.magnitude);
				}
				if (this.m_projectile.OverrideMotionModule != null)
				{
					this.m_projectile.OverrideMotionModule.AdjustRightVector(num4 - num3);
				}
			}

			if (vector == Vector2.zero || float.IsNaN(vector.x) || float.IsNaN(vector.y))
			{
				return inVel;
			}
			if(x != null)
            {
				DoLaser(vector2.ToAngle(), vector2);
			}

			return vector;
		}

		protected override void OnDestroy()
		{
			if (this.m_projectile)
			{
				Projectile projectile = this.m_projectile;
				projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Remove(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
			}
			base.OnDestroy();
		}

		void DoLaser(float angle, Vector2 target)
		{

			if (this.m_extantLaserSight == null)
			{

				string path = "Global VFX/VFX_LaserSight";
				this.m_extantLaserSight = SpawnManager.SpawnVFX((GameObject)BraveResources.Load(path, ".prefab"), false).GetComponent<tk2dTiledSprite>();
				this.m_extantLaserSight.IsPerpendicular = false;
				this.m_extantLaserSight.HeightOffGround = 0.25f;
				this.m_extantLaserSight.renderer.enabled = this.sprite.renderer.enabled;
				this.m_extantLaserSight.transform.parent = base.transform;
				this.m_extantLaserSight.transform.localPosition = Vector3.zero;
				this.m_extantLaserSight.transform.rotation = Quaternion.Euler(0f, 0f, angle);
				if (this.m_extantLaserSight.renderer.enabled)
				{
					/*Func<SpeculativeRigidbody, bool> rigidbodyExcluder = (SpeculativeRigidbody otherRigidbody) => otherRigidbody.minorBreakable && !otherRigidbody.minorBreakable.stopsBullets;
					bool flag2 = false;
					float num9 = float.MaxValue;
					CollisionLayer layer2 = CollisionLayer.EnemyHitBox;
					int rayMask2 = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, layer2, CollisionLayer.BulletBreakable);
					RaycastResult raycastResult2;
					if (PhysicsEngine.Instance.Raycast(base.sprite.WorldCenter, angle, 30, out raycastResult2, true, true, rayMask2, null,
						false, rigidbodyExcluder, null))
					{
						flag2 = true;
						num9 = raycastResult2.Distance;
					}
					RaycastResult.Pool.Free(ref raycastResult2);*/
					this.m_extantLaserSight.dimensions = new Vector2(Vector2.Distance(target, sprite.WorldCenter), 1f);
					this.m_extantLaserSight.ForceRotationRebuild();
					this.m_extantLaserSight.UpdateZDepth();
				}
			}
			else if (this.m_extantLaserSight != null)
			{
				SpawnManager.Despawn(this.m_extantLaserSight.gameObject);
				this.m_extantLaserSight = null;
			}
			
		}

		private tk2dTiledSprite m_extantLaserSight;

		public float HomingRadius;
		public float AngularVelocity;
		protected Projectile m_projectile;
	}
}
