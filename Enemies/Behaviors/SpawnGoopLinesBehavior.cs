using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SpawnGoopLinesBehavior : BasicAttackBehavior
	{
		public SpawnGoopLinesBehavior()
		{
			this.goopDuration = 0.5f;
		}

		public override void Start()
		{
			base.Start();
			tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}

		public override void Upkeep()
		{
			base.Upkeep();
			base.DecrementTimer(ref this.m_goopTimer, false);
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
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_hasGooped = false;
			this.m_aiAnimator.PlayUntilFinished(this.spewAnimation, false, null, -1f, false);
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			base.ContinuousUpdate();
			if (!this.m_hasGooped || this.m_goopTimer > 0f)
			{
				return ContinuousBehaviorResult.Continue;
			}
			return ContinuousBehaviorResult.Finished;
		}
		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			this.m_aiAnimator.EndAnimationIf(this.spewAnimation);
			this.m_updateEveryFrame = false;
			this.UpdateCooldowns();
		}
		private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
		{
			if (!this.m_hasGooped && clip.GetFrame(frame).eventInfo == "spew")
			{
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToUse);

				goopManagerForGoopType.TimedAddGoopLine(this.goopPointR.transform.position, this.goopPointR.transform.position - new Vector3(0.25f, goopLineLength, 0), goopRadius, goopDuration);
				goopManagerForGoopType.TimedAddGoopLine(this.goopPointL.transform.position, this.goopPointL.transform.position - new Vector3(-0.25f, goopLineLength, 0), goopRadius, goopDuration);
				this.m_goopTimer = this.goopDuration;
				this.m_hasGooped = true;
			}
		}

		public string spewAnimation;
		public Transform goopPointR;
		public Transform goopPointL;
		public GoopDefinition goopToUse;
		public float goopLineLength;
		public float goopDuration;
		public float goopRadius;
		private float m_goopTimer;
		private bool m_hasGooped;
    }
}
