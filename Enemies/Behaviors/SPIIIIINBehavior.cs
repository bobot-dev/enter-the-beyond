using Brave.BulletScript;
using FrostAndGunfireItems;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SPIIIIINBehavior : BasicAttackBehavior
    {
		public override void Start()
		{
			base.Start();
			SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}

		public override void Upkeep()
		{
			base.Upkeep();
			base.DecrementTimer(ref this.m_timer, false);
		}

		public override BehaviorResult Update()
		{
			BehaviorResult behaviorResult = base.Update();
			if (behaviorResult != BehaviorResult.Continue)
			{
				return behaviorResult;
			}
			if (!this.IsReady())
			{
				return BehaviorResult.Continue;
			}
			if (!this.m_aiActor.TargetRigidbody)
			{
				return BehaviorResult.Continue;
			}
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				if (!string.IsNullOrEmpty(this.TellAnimation))
				{
					this.m_aiAnimator.PlayUntilFinished(this.TellAnimation, false, null, -1f, false);
				}
				this.m_state = SPIIIIINBehavior.State.WaitingForTell;
			}
			else
			{
				this.StartSpinning();
			}
			this.m_aiActor.ClearPath();
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(true, "EnemyBlankBehavior");
			}
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}

		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			if (this.m_state == SPIIIIINBehavior.State.WaitingForTell)
			{
				if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
				{
					this.StartSpinning();
					return ContinuousBehaviorResult.Continue;
				}
			}
			else if (this.m_state == SPIIIIINBehavior.State.Spinning)
			{
				this.m_aiActor.transform.eulerAngles += new Vector3(0f, 0f, Speed);
				if (!m_isBouncing)
				{
					this.m_startingAngle = BraveMathCollege.ClampAngle360(BraveUtility.RandomElement<float>(this.startingAngles));
					this.m_aiActor.BehaviorOverridesVelocity = true;
					this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startingAngle, this.m_aiActor.MovementSpeed * Speed);
					this.m_isBouncing = true;
					Fire();

				}
				

				if (this.m_timer <= 0f)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
			return ContinuousBehaviorResult.Continue;
		}

		protected virtual void OnCollision(CollisionData collision)
		{
			if (!this.m_isBouncing)
			{
				return;
			}
			if (collision.OtherRigidbody && collision.OtherRigidbody.projectile)
			{
				return;
			}
			if (collision.CollidedX || collision.CollidedY)
			{
				Vector2 vector = collision.MyRigidbody.Velocity;
				if (collision.CollidedX)
				{
					vector.x *= -1f;
				}
				if (collision.CollidedY)
				{
					vector.y *= -1f;
				}
				vector = vector.normalized * (this.m_aiActor.MovementSpeed * Speed);
				PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
				this.m_aiActor.BehaviorVelocity = vector;
			}
		}

		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				this.m_aiAnimator.EndAnimationIf(this.TellAnimation);
			}
			if (!string.IsNullOrEmpty(this.SpinAnimation))
			{
				this.m_aiAnimator.EndAnimationIf(this.SpinAnimation);
			}
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(false, "EnemyBlankBehavior");
			}
			this.m_aiActor.transform.eulerAngles = new Vector3(0f, 0f, 0);
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_state = SPIIIIINBehavior.State.Idle;
			this.m_updateEveryFrame = false;
			this.m_isBouncing = false;
			this.m_bulletSource.ForceStop();
			this.UpdateCooldowns();
		}

		private void Fire()
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
			this.m_bulletSource.BulletScript = bulletScript;
			this.m_bulletSource.Initialize();
			
		}



		private void StartSpinning()
		{
			if (!string.IsNullOrEmpty(this.SpinAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.SpinAnimation, false, null, -1f, false);
			}



			this.m_timer = this.SpinTime;
			this.m_state = SPIIIIINBehavior.State.Spinning;
		}
		public float SpinTime;
		public float Speed;

		public BulletScriptSelector bulletScript;

		public GameObject ShootPoint;

		public float[] startingAngles;

		[InspectorCategory("Visuals")]
		public string TellAnimation;

		// Token: 0x040039E7 RID: 14823
		[InspectorCategory("Visuals")]
		public string SpinAnimation;

		private SPIIIIINBehavior.State m_state;

		// Token: 0x040039EB RID: 14827
		private float m_timer;

		private bool m_isBouncing;

		private float m_startingAngle;

		private BulletScriptSource m_bulletSource;
		private enum State
        {
            // Token: 0x040039ED RID: 14829
            Idle,
            // Token: 0x040039EE RID: 14830
            WaitingForTell,
            // Token: 0x040039EF RID: 14831
            Spinning
        }
    }
}
