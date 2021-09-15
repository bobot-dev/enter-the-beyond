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
	class EnemyBlankBehavior : BasicAttackBehavior
	{
		// Token: 0x06004703 RID: 18179 RVA: 0x00198828 File Offset: 0x00196A28
		public override void Start()
		{
			base.Start();
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00198878 File Offset: 0x00196A78
		public override void Upkeep()
		{
			base.Upkeep();
			base.DecrementTimer(ref this.m_timer, false);
			if (this.m_behaviorSpeculator.AttackCooldown <= 0f && this.m_behaviorSpeculator.GlobalCooldown <= 0f && this.m_cooldownTimer < this.SkippableCooldown)
			{
				bool flag = false;
				Vector2 unitCenter = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;
				ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
				for (int i = 0; i < allProjectiles.Count; i++)
				{
					Projectile projectile = allProjectiles[i];
					if (projectile.Owner is PlayerController)
					{
						if (projectile.specRigidbody)
						{
							if (Vector2.Distance(unitCenter, projectile.specRigidbody.UnitCenter) <= this.SkippableRadius)
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					this.m_cooldownTimer = 0f;
				}
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x00198978 File Offset: 0x00196B78
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
				this.m_state = EnemyBlankBehavior.State.WaitingForTell;
			}
			else
			{
				this.StartBlanking();
			}
			this.m_aiActor.ClearPath();
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(true, "EnemyBlankBehavior");
			}
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		GameObject m_radialIndicator;
		Vector2 centerPoint;

		// Token: 0x06004706 RID: 18182 RVA: 0x00198A54 File Offset: 0x00196C54
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			if (this.m_state == EnemyBlankBehavior.State.WaitingForTell)
			{
				if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
				{
					this.StartBlanking();
					return ContinuousBehaviorResult.Continue;
				}
			}
			else if (this.m_state == EnemyBlankBehavior.State.Blanking)
			{
				this.DestroyBulletsInRange(centerPoint, Radius, false, true, null, false);
				

				if (this.m_timer <= 0f)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
			return ContinuousBehaviorResult.Continue;
		}
		public static int destroyedBulletCount;
		private void DestroyBulletsInRange(Vector2 centerPoint, float radius, bool destroysEnemyBullets, bool destroysPlayerBullets, PlayerController user = null, bool reflectsBullets = false, float? previousRadius = null, bool useCallback = false, Action<Projectile> callback = null)
		{
			float num = radius * radius;
			float num2 = (previousRadius == null) ? 0f : (previousRadius.Value * previousRadius.Value);
			List<Projectile> list = new List<Projectile>();
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile && projectile.sprite)
				{
					float sqrMagnitude = (projectile.sprite.WorldCenter - centerPoint).sqrMagnitude;
					if (sqrMagnitude <= num)
					{
						if (!projectile.ImmuneToBlanks)
						{
							if (previousRadius == null || !projectile.ImmuneToSustainedBlanks || sqrMagnitude >= num2)
							{
								if (projectile.Owner != null)
								{
									if (projectile.isFakeBullet || projectile.Owner is AIActor || (projectile.Shooter != null && projectile.Shooter.aiActor != null) || projectile.ForcePlayerBlankable)
									{
										if (destroysEnemyBullets)
										{
											list.Add(projectile);
										}
									}
									else if (projectile.Owner is PlayerController)
									{
										if (destroysPlayerBullets && projectile.Owner != user)
										{
											list.Add(projectile);
										}
									}
									else
									{
										Debug.LogError("Silencer is trying to process a bullet that is owned by something that is neither man nor beast!");
									}
								}
								else if (destroysEnemyBullets)
								{
									list.Add(projectile);
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!destroysPlayerBullets && reflectsBullets)
				{
					PassiveReflectItem.ReflectBullet(list[j], true, user, 10f, 1f, 1f, 0f);
				}
				else
				{
					if (list[j] && list[j].GetComponent<ChainLightningModifier>())
					{
						ChainLightningModifier component = list[j].GetComponent<ChainLightningModifier>();
						UnityEngine.Object.Destroy(component);
					}
					if (useCallback && callback != null)
					{
						callback(list[j]);
					}
					list[j].DieInAir(false, true, true, true);
				}
			}
			List<BasicTrapController> allTriggeredTraps = StaticReferenceManager.AllTriggeredTraps;
			for (int k = allTriggeredTraps.Count - 1; k >= 0; k--)
			{
				BasicTrapController basicTrapController = allTriggeredTraps[k];
				if (basicTrapController && basicTrapController.triggerOnBlank)
				{
					float sqrMagnitude2 = (basicTrapController.CenterPoint() - centerPoint).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						basicTrapController.Trigger();
					}
				}
			}
			destroyedBulletCount += list.Count;
		}


		// Token: 0x06004707 RID: 18183 RVA: 0x00198B8C File Offset: 0x00196D8C
		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				this.m_aiAnimator.EndAnimationIf(this.TellAnimation);
			}
			if (!string.IsNullOrEmpty(this.BlankAnimation))
			{
				this.m_aiAnimator.EndAnimationIf(this.BlankAnimation);
			}
			if (!string.IsNullOrEmpty(this.BlankVfx))
			{
				this.m_aiAnimator.StopVfx(this.BlankVfx);
			}
			if (this.m_aiActor && this.m_aiActor.knockbackDoer)
			{
				this.m_aiActor.knockbackDoer.SetImmobile(false, "EnemyBlankBehavior");
			}
			if (this.m_radialIndicator != null)
			{
				UnityEngine.Object.Destroy(m_radialIndicator);
			}
			Fire();
			this.m_state = EnemyBlankBehavior.State.Idle;
			this.m_updateEveryFrame = false;
			this.UpdateCooldowns();
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x00198C54 File Offset: 0x00196E54
		private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
		{
			tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
			if (this.m_state == EnemyBlankBehavior.State.WaitingForTell && frame.eventInfo == "blank")
			{
				this.StartBlanking();
			}
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00198C90 File Offset: 0x00196E90
		private void StartBlanking()
		{
			if (!string.IsNullOrEmpty(this.BlankAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.BlankAnimation, false, null, -1f, false);
			}
			if (!string.IsNullOrEmpty(this.BlankVfx))
			{
				this.m_aiAnimator.PlayVfx(this.BlankVfx, null, null, null);
			}

			centerPoint = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;

			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), centerPoint, Quaternion.identity, this.m_aiActor.transform));
			var ring = this.m_radialIndicator.GetComponent<HeatIndicatorController>();
			ring.IsFire = false;
			ring.CurrentRadius = Radius;
			ring.CurrentColor = Color.magenta;



			this.m_timer = this.BlankTime;
			this.m_state = EnemyBlankBehavior.State.Blanking;
		}

		private void Fire()
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
			this.m_bulletSource.BulletScript = new CustomBulletScriptSelector(typeof(RingScript));
			this.m_bulletSource.Initialize();
		}

		private BulletScriptSource m_bulletSource;

		public GameObject ShootPoint;

		// Token: 0x040039E1 RID: 14817
		public float SkippableCooldown;

		// Token: 0x040039E2 RID: 14818
		public float SkippableRadius;

		// Token: 0x040039E3 RID: 14819
		public float Radius;

		// Token: 0x040039E4 RID: 14820
		public float BlankTime;

		// Token: 0x040039E6 RID: 14822
		[InspectorCategory("Visuals")]
		public string TellAnimation;

		// Token: 0x040039E7 RID: 14823
		[InspectorCategory("Visuals")]
		public string BlankAnimation;

		// Token: 0x040039E8 RID: 14824
		[InspectorCategory("Visuals")]
		public string BlankVfx;

		// Token: 0x040039E9 RID: 14825
		[InspectorCategory("Visuals")]
		public GameObject OverrideHitVfx;

		// Token: 0x040039EA RID: 14826
		private EnemyBlankBehavior.State m_state;

		// Token: 0x040039EB RID: 14827
		private float m_timer;

		// Token: 0x02000D24 RID: 3364
		private enum State
		{
			// Token: 0x040039ED RID: 14829
			Idle,
			// Token: 0x040039EE RID: 14830
			WaitingForTell,
			// Token: 0x040039EF RID: 14831
			Blanking
		}
	}

	public class RingScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			for (int i = 0; i <= EnemyBlankBehavior.destroyedBulletCount; i++)
			{
				this.Fire(new Offset(new Vector2(0.1f * i, 0.1f * i)), new Direction((float)(i * 10), DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				if (i > 36)
                {
					
				} 
				else
                {
					//this.Fire(new Direction((float)(i), DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				}
			}
			EnemyBlankBehavior.destroyedBulletCount = 0;
			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}


	}

}
