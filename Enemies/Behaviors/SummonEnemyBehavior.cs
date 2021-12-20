using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

namespace BotsMod
{
	public class SummonEnemyViaWaveBehavior : BasicAttackBehavior
	{
		public SummonEnemyViaWaveBehavior()
		{			
			this.StopDuringAnimation = true;
			this.TargetVfxLoops = true;
		}

		public override void Start()
		{
			base.Start();
			
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
			
			if (!string.IsNullOrEmpty(this.SummonAnim))
			{
				this.m_aiAnimator.PlayUntilFinished(this.SummonAnim, true, null, -1f, false);
				if (this.StopDuringAnimation)
				{
					if (this.HideGun)
					{
						this.m_aiShooter.ToggleGunAndHandRenderers(false, "SummonEnemyViaWaveBehavior");
					}
					this.m_aiActor.ClearPath();
				}
			}
			if (!string.IsNullOrEmpty(this.SummonVfx))
			{
				this.m_aiAnimator.PlayVfx(this.SummonVfx, null, null, null);
			}
			
			this.m_timer = this.SummonTime;
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(true, "SummonEnemyViaWaveBehavior");
			}			
			this.m_state = SummonEnemyViaWaveBehavior.State.Summoning;
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			if (this.m_state == SummonEnemyViaWaveBehavior.State.Summoning)
			{
				if (this.m_timer <= 0f)
				{
					this.m_aiActor.ParentRoom.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ENEMY_BEHAVIOR, false);
					this.m_aiActor.ParentRoom.HandleRoomAction(RoomEventTriggerAction.BECOME_TERRIFYING_AND_DARK);

					this.m_timer = this.SummonTime;
					return ContinuousBehaviorResult.Continue;
					
					if (this.m_spawnClip != null && this.m_spawnClip.wrapMode != tk2dSpriteAnimationClip.WrapMode.Loop)
					{
						this.m_state = SummonEnemyViaWaveBehavior.State.WaitingForSummonAnim;
						return ContinuousBehaviorResult.Continue;
					}
					if (!string.IsNullOrEmpty(this.PostSummonAnim))
					{
						this.m_state = SummonEnemyViaWaveBehavior.State.WaitingForPostAnim;
						this.m_aiAnimator.PlayUntilFinished(this.PostSummonAnim, false, null, -1f, false);
						return ContinuousBehaviorResult.Continue;
					}
					return ContinuousBehaviorResult.Finished;
				}
			}
			else if (this.m_state == SummonEnemyViaWaveBehavior.State.WaitingForSummonAnim)
			{
				if (!string.IsNullOrEmpty(this.PostSummonAnim))
				{
					this.m_state = SummonEnemyViaWaveBehavior.State.WaitingForPostAnim;
					this.m_aiAnimator.PlayUntilFinished(this.PostSummonAnim, false, null, -1f, false);
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
			else if (this.m_state == SummonEnemyViaWaveBehavior.State.WaitingForPostAnim && !this.m_aiActor.spriteAnimator.IsPlaying(this.PostSummonAnim))
			{
				return ContinuousBehaviorResult.Finished;
			}
			return ContinuousBehaviorResult.Continue;
		}

		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			if (!string.IsNullOrEmpty(this.SummonAnim))
			{
				this.m_aiAnimator.EndAnimationIf(this.SummonAnim);
			}
			if (!string.IsNullOrEmpty(this.SummonVfx))
			{
				this.m_aiAnimator.StopVfx(this.SummonVfx);
			}
			
			if (!string.IsNullOrEmpty(this.PostSummonAnim))
			{
				this.m_aiAnimator.EndAnimationIf(this.PostSummonAnim);
			}
			if (this.HideGun)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "SummonEnemyViaWaveBehavior");
			}
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(false, "SummonEnemyViaWaveBehavior");
			}
			this.m_state = SummonEnemyViaWaveBehavior.State.Idle;
			this.m_updateEveryFrame = false;
			this.UpdateCooldowns();
		}

		

		public float SummonTime;

		public bool HideGun;

		public bool StopDuringAnimation;

		public string SummonAnim;

		public string SummonVfx;

		public bool TargetVfxLoops;

		public string PostSummonAnim;

		private SummonEnemyViaWaveBehavior.State m_state;
		
		private float m_timer;

		
		private tk2dSpriteAnimationClip m_spawnClip;

		private IntVector2 m_enemyClearance;

		

		private enum State
		{
			Idle,
			Summoning,
			WaitingForSummonAnim,
			WaitingForPostAnim
		}
	}
}
