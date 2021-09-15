﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FullInspector;
using UnityEngine;

namespace BotsMod
{
	class DashButGoodBehavior : BasicAttackBehavior
	{
		// Token: 0x060046EA RID: 18154 RVA: 0x0002E882 File Offset: 0x0002CA82
		public DashButGoodBehavior()
		{
			this.dashDirection = DashButGoodBehavior.DashDirection.Random;
			this.warpDashAnimLength = true;
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x00197470 File Offset: 0x00195670
		public override void Start()
		{
			base.Start();
			this.m_trailRenderer = this.m_aiActor.GetComponentsInChildren<TrailRenderer>();
			if (this.toggleTrailRenderer && this.m_trailRenderer != null)
			{
				foreach(var dumbfuckingidiot in this.m_trailRenderer)
                {
					dumbfuckingidiot.enabled = false;
				}
			}
			this.m_shadowTrail = this.m_aiActor.GetComponent<AfterImageTrailController>();
			if (this.bulletScript != null && !this.bulletScript.IsNull)
			{
				tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
			}
			if (this.stopOnCollision)
			{
				SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			}
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x0002E899 File Offset: 0x0002CA99
		public override void Upkeep()
		{
			base.Upkeep();
			base.DecrementTimer(ref this.m_dashTimer, false);
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x0019754C File Offset: 0x0019574C
		public override BehaviorResult Update()
		{
			if (this.m_shadowSprite == null)
			{
				this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			}
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
			this.m_dashDirection = this.GetDashDirection();
			if (!string.IsNullOrEmpty(this.chargeAnim))
			{
				SetState(DashButGoodBehavior.DashState.Charge);
			}
			else
			{
				SetState(DashButGoodBehavior.DashState.Dash);
			}
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001975EC File Offset: 0x001957EC
		public override ContinuousBehaviorResult ContinuousUpdate()
		{
			base.ContinuousUpdate();
			if (this.State == DashButGoodBehavior.DashState.Charge)
			{
				if (!this.m_aiAnimator.IsPlaying(this.chargeAnim))
				{
					SetState(DashButGoodBehavior.DashState.Dash);
				}
			}
			else if (this.State == DashButGoodBehavior.DashState.Dash)
			{
				if (this.doDodgeDustUp)
				{
					bool flag = this.m_aiActor.spriteAnimator.QueryGroundedFrame();
					if (!this.m_cachedGrounded && flag && !this.m_aiActor.IsFalling)
					{
						GameManager.Instance.Dungeon.dungeonDustups.InstantiateLandDustup(this.m_aiActor.specRigidbody.UnitCenter);
						this.m_aiActor.DoDustUps = this.m_cachedDoDustups;
					}
					this.m_cachedGrounded = flag;
				}
				if (this.enableShadowTrail && this.m_dashTimer <= 0.1f)
				{
					this.m_shadowTrail.spawnShadows = false;
				}
				if (this.m_dashTimer <= 0f)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
			else if (this.State == DashButGoodBehavior.DashState.Idle)
			{
				return ContinuousBehaviorResult.Finished;
			}
			return ContinuousBehaviorResult.Continue;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00197704 File Offset: 0x00195904
		public override void EndContinuousUpdate()
		{
			base.EndContinuousUpdate();
			this.m_updateEveryFrame = false;
			if (this.postDashSpeed > 0f)
			{
				this.m_aiActor.BehaviorVelocity = this.m_dashDirection.normalized * this.postDashSpeed;
			}
			else
			{
				this.m_aiActor.BehaviorOverridesVelocity = false;
				this.m_aiAnimator.LockFacingDirection = false;
			}
			SetState(DashButGoodBehavior.DashState.Idle);
			this.UpdateCooldowns();
			if (!this.m_lastDashWasDouble)
			{
				if (this.doubleDashChance > 0f && UnityEngine.Random.value < this.doubleDashChance)
				{
					this.m_cooldownTimer = 0f;
					this.m_lastDashWasDouble = true;
				}
			}
			else
			{
				this.m_lastDashWasDouble = false;
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x0002E8AE File Offset: 0x0002CAAE
		public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
		{
			if (this.m_state == DashButGoodBehavior.DashState.Dash && this.m_shouldFire && clip.GetFrame(frame).eventInfo == "fire")
			{
				this.Fire();
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x001977C4 File Offset: 0x001959C4
		public void OnCollision(CollisionData collisionData)
		{
			if (this.m_state == DashButGoodBehavior.DashState.Dash)
			{
				if (collisionData.IsTriggerCollision)
				{
					return;
				}
				if (collisionData.OtherRigidbody && collisionData.OtherRigidbody.projectile)
				{
					return;
				}
				SetState(DashButGoodBehavior.DashState.Idle);
			}
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00197818 File Offset: 0x00195A18
		private float[] GetDirections()
		{
			float[] array = new float[0];
			if (this.dashDirection == DashButGoodBehavior.DashDirection.PerpendicularToTarget)
			{
				float num = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.specRigidbody.UnitCenter).ToAngle();
				array = new float[]
				{
				num + 90f,
				num - 90f
				};
				BraveUtility.RandomizeArray<float>(array, 0, -1);
			}
			else if (this.dashDirection == DashButGoodBehavior.DashDirection.KindaTowardTarget)
			{
				float num2 = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.specRigidbody.UnitCenter).ToAngle();
				array = new float[]
				{
				num2,
				num2 - this.quantizeDirection,
				num2 + this.quantizeDirection
				};
				BraveUtility.RandomizeArray<float>(array, 1, -1);
			}
			else if (this.dashDirection == DashButGoodBehavior.DashDirection.TowardTarget)
			{
				float num3 = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.specRigidbody.UnitCenter).ToAngle();
				array = new float[]
				{
				num3
				};
			}
			else if (this.dashDirection == DashButGoodBehavior.DashDirection.Random)
			{
				if (this.quantizeDirection <= 0f)
				{
					array = new float[16];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = UnityEngine.Random.Range(0f, 360f);
					}
				}
				else
				{
					int num4 = Mathf.RoundToInt(360f / this.quantizeDirection);
					array = new float[num4];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = (float)j * this.quantizeDirection;
					}
					BraveUtility.RandomizeArray<float>(array, 0, -1);
				}
			}
			else if (this.dashDirection == DashButGoodBehavior.DashDirection.Random)
			{
				if (this.quantizeDirection <= 0f)
				{
					array = new float[16];
					for (int k = 0; k < array.Length; k++)
					{
						array[k] = UnityEngine.Random.Range(0f, 360f);
					}
				}
				else
				{
					int num5 = Mathf.RoundToInt(360f / this.quantizeDirection);
					array = new float[num5];
					for (int l = 0; l < array.Length; l++)
					{
						array[l] = (float)l * this.quantizeDirection;
					}
					BraveUtility.RandomizeArray<float>(array, 0, -1);
				}
			}
			if (this.quantizeDirection > 0f)
			{
				for (int m = 0; m < array.Length; m++)
				{
					array[m] = BraveMathCollege.QuantizeFloat(array[m], this.quantizeDirection);
				}
			}
			return array;
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x00197ABC File Offset: 0x00195CBC
		private Vector2 GetDashDirection()
		{
			float[] directions = this.GetDirections();
			Vector2 lhs = Vector2.zero;
			Vector2 unitCenter = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
			for (int i = 0; i < directions.Length; i++)
			{
				bool flag = false;
				bool flag2 = false;
				Vector2 vector = BraveMathCollege.DegreesToVector(directions[i], 1f);
				RaycastResult raycastResult;
				bool flag3 = PhysicsEngine.Instance.Raycast(unitCenter, vector, this.dashDistance, out raycastResult, true, true, int.MaxValue, new CollisionLayer?(CollisionLayer.EnemyCollider), false, null, this.m_aiActor.specRigidbody);
				RaycastResult.Pool.Free(ref raycastResult);
				float num = 0.25f;
				while (num <= this.dashDistance && !flag && !flag3)
				{
					Vector2 vector2 = unitCenter + num * vector;
					if (!GameManager.Instance.Dungeon.CellExists(vector2))
					{
						flag = true;
					}
					else if (GameManager.Instance.Dungeon.ShouldReallyFall(vector2))
					{
						flag = true;
					}
					num += 0.25f;
				}
				num = 0.25f;
				while (num <= this.dashDistance && !flag && !flag2 && !flag3)
				{
					IntVector2 intVector = (unitCenter + num * vector).ToIntVector2(VectorConversions.Floor);
					if (!GameManager.Instance.Dungeon.CellExists(intVector))
					{
						flag2 = true;
					}
					else if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) && GameManager.Instance.Dungeon.data[intVector].isExitCell)
					{
						flag2 = true;
					}
					num += 0.25f;
				}
				if (this.avoidTarget && this.m_behaviorSpeculator.TargetRigidbody && !flag && !flag2 && !flag3)
				{
					Vector2 unitCenter2 = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
					Vector2 vector3 = this.m_behaviorSpeculator.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - unitCenter2;
					float num2 = this.dashDistance + 2f;
					if (vector3.magnitude < num2 && BraveMathCollege.AbsAngleBetween(vector3.ToAngle(), directions[i]) < 80f)
					{
						flag3 = true;
					}
					if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !flag3)
					{
						PlayerController playerController = this.m_aiActor.PlayerTarget as PlayerController;
						if (playerController)
						{
							PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(playerController);
							if (otherPlayer && otherPlayer.healthHaver.IsAlive)
							{
								vector3 = otherPlayer.specRigidbody.GetUnitCenter(ColliderType.HitBox) - unitCenter2;
								if (vector3.magnitude < num2 && BraveMathCollege.AbsAngleBetween(vector3.ToAngle(), directions[i]) < 80f)
								{
									flag3 = true;
								}
							}
						}
					}
				}
				if (!flag3 && !flag && !flag2)
				{
					lhs = vector;
					break;
				}
			}
			if (lhs != Vector2.zero)
			{
				return lhs.normalized;
			}
			if (directions.Length > 0)
			{
				return BraveMathCollege.DegreesToVector(directions[directions.Length - 1], 1f);
			}
			float num3 = UnityEngine.Random.Range(0f, 360f);
			if (this.quantizeDirection > 0f)
			{
				BraveMathCollege.QuantizeFloat(num3, this.quantizeDirection);
			}
			return BraveMathCollege.DegreesToVector(num3, 1f);
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00197E40 File Offset: 0x00196040
		private void Fire()
		{
			if (!this.m_bulletSource)
			{
				this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
			}
			this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
			this.m_bulletSource.BulletScript = this.bulletScript;
			this.m_bulletSource.Initialize();
			this.m_shouldFire = false;
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060046F5 RID: 18165 RVA: 0x0002E8E8 File Offset: 0x0002CAE8
		// (set) Token: 0x060046F6 RID: 18166 RVA: 0x0002E8F0 File Offset: 0x0002CAF0
		private DashButGoodBehavior.DashState State
		{
			get
			{
				return this.m_state;
			}
		}


		private void SetState(DashButGoodBehavior.DashState value)
		{

			if (this.m_state != value)
			{

				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);

			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x00197EA8 File Offset: 0x001960A8
		private void BeginState(DashButGoodBehavior.DashState state)
		{
			if (state == DashButGoodBehavior.DashState.Charge)
			{
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = this.m_dashDirection.ToAngle();
				this.m_aiAnimator.PlayUntilFinished(this.chargeAnim, true, null, -1f, false);
				this.m_aiActor.ClearPath();
				if (!this.m_aiActor.BehaviorOverridesVelocity)
				{
					this.m_aiActor.BehaviorOverridesVelocity = true;
					this.m_aiActor.BehaviorVelocity = Vector2.zero;
				}
			}
			else if (state == DashButGoodBehavior.DashState.Dash)
			{
				if (this.bulletScript != null && !this.bulletScript.IsNull)
				{
					this.m_shouldFire = true;
				}
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = this.m_dashDirection.ToAngle();
				if (!string.IsNullOrEmpty(this.dashAnim))
				{
					if (this.warpDashAnimLength)
					{
						AIAnimator aiAnimator = this.m_aiAnimator;
						string name = this.dashAnim;
						bool suppressHitStates = true;
						float warpClipDuration = this.dashTime;
						aiAnimator.PlayUntilFinished(name, suppressHitStates, null, warpClipDuration, false);
					}
					else
					{
						this.m_aiAnimator.PlayUntilFinished(this.dashAnim, true, null, -1f, false);
					}
				}
				if (this.doDodgeDustUp)
				{
					this.m_cachedDoDustups = this.m_aiActor.DoDustUps;
					this.m_aiActor.DoDustUps = false;
					GameManager.Instance.Dungeon.dungeonDustups.InstantiateDodgeDustup(this.m_dashDirection, this.m_aiActor.specRigidbody.UnitBottomCenter);
					this.m_cachedGrounded = true;
				}
				if (this.hideShadow && this.m_shadowSprite)
				{
					this.m_shadowSprite.renderer.enabled = false;
				}
				if (this.hideGun && this.m_aiShooter)
				{
					this.m_aiShooter.ToggleGunAndHandRenderers(false, "DashButGoodBehavior");
				}
				if (this.toggleTrailRenderer && this.m_trailRenderer != null)
				{
					foreach (var dumbfuckingidiot in this.m_trailRenderer)
					{
						dumbfuckingidiot.enabled = true;
					}
				}
				if (this.enableShadowTrail)
				{
					this.m_shadowTrail.spawnShadows = true;
					AkSoundEngine.PostEvent("Play_ENM_highpriest_dash_01", GameManager.Instance.PrimaryPlayer.gameObject);
				}
				float d = this.dashDistance / this.dashTime;
				this.m_dashTimer = this.dashTime;
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = d * this.m_dashDirection;
				if (this.bulletScript != null && !this.bulletScript.IsNull && this.fireAtDashStart)
				{
					this.Fire();
				}
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x0019815C File Offset: 0x0019635C
		private void EndState(DashButGoodBehavior.DashState state)
		{
			if (state == DashButGoodBehavior.DashState.Dash)
			{
				if (!string.IsNullOrEmpty(this.dashAnim))
				{
					this.m_aiAnimator.EndAnimationIf(this.dashAnim);
				}
				if (this.bulletScript != null && !this.bulletScript.IsNull && this.m_shouldFire)
				{
					this.Fire();
				}
				if (this.doDodgeDustUp)
				{
					this.m_aiActor.DoDustUps = this.m_cachedDoDustups;
				}
				if (this.hideShadow && this.m_shadowSprite)
				{
					this.m_shadowSprite.renderer.enabled = true;
				}
				if (this.hideGun && this.m_aiShooter)
				{
					this.m_aiShooter.ToggleGunAndHandRenderers(true, "DashButGoodBehavior");
				}
				if (this.toggleTrailRenderer && this.m_trailRenderer != null)
				{
					foreach (var dumbfuckingidiot in this.m_trailRenderer)
					{
						dumbfuckingidiot.enabled = false;
					}
				}
				if (this.enableShadowTrail)
				{
					this.m_shadowTrail.spawnShadows = false;
				}
				if (this.postDashSpeed <= 0f)
				{
					this.m_aiActor.BehaviorVelocity = Vector2.zero;
				}
				if (this.m_bulletSource != null)
				{
					this.m_bulletSource.ForceStop();
				}
			}
		}

		// Token: 0x040039AD RID: 14765
		public DashButGoodBehavior.DashDirection dashDirection;

		// Token: 0x040039AE RID: 14766
		public float quantizeDirection;

		// Token: 0x040039AF RID: 14767
		public float dashDistance;

		// Token: 0x040039B0 RID: 14768
		public float dashTime;

		// Token: 0x040039B1 RID: 14769
		public float postDashSpeed;

		// Token: 0x040039B2 RID: 14770
		public float doubleDashChance;

		// Token: 0x040039B3 RID: 14771
		public bool avoidTarget;

		// Token: 0x040039B4 RID: 14772
		[InspectorCategory("Attack")]
		public GameObject ShootPoint;

		// Token: 0x040039B5 RID: 14773
		[InspectorCategory("Attack")]
		public BulletScriptSelector bulletScript;

		// Token: 0x040039B6 RID: 14774
		[InspectorCategory("Attack")]
		public bool fireAtDashStart;

		// Token: 0x040039B7 RID: 14775
		[InspectorCategory("Attack")]
		public bool stopOnCollision;

		// Token: 0x040039B8 RID: 14776
		[InspectorCategory("Visuals")]
		public string chargeAnim;

		// Token: 0x040039B9 RID: 14777
		[InspectorCategory("Visuals")]
		public string dashAnim;

		// Token: 0x040039BA RID: 14778
		[InspectorCategory("Visuals")]
		public bool doDodgeDustUp;

		// Token: 0x040039BB RID: 14779
		[InspectorCategory("Visuals")]
		public bool warpDashAnimLength;

		// Token: 0x040039BC RID: 14780
		[InspectorCategory("Visuals")]
		public bool hideShadow;

		// Token: 0x040039BD RID: 14781
		[InspectorCategory("Visuals")]
		public bool hideGun;

		// Token: 0x040039BE RID: 14782
		[InspectorCategory("Visuals")]
		public bool toggleTrailRenderer;

		// Token: 0x040039BF RID: 14783
		[InspectorCategory("Visuals")]
		public bool enableShadowTrail;

		// Token: 0x040039C0 RID: 14784
		private tk2dBaseSprite m_shadowSprite;

		// Token: 0x040039C1 RID: 14785
		private TrailRenderer[] m_trailRenderer;

		// Token: 0x040039C2 RID: 14786
		private AfterImageTrailController m_shadowTrail;

		// Token: 0x040039C3 RID: 14787
		private BulletScriptSource m_bulletSource;

		// Token: 0x040039C4 RID: 14788
		private bool m_cachedDoDustups;

		// Token: 0x040039C5 RID: 14789
		private bool m_cachedGrounded;

		// Token: 0x040039C6 RID: 14790
		private Vector2 m_dashDirection;

		// Token: 0x040039C7 RID: 14791
		private float m_dashTimer;

		// Token: 0x040039C8 RID: 14792
		private bool m_shouldFire;

		// Token: 0x040039C9 RID: 14793
		private bool m_lastDashWasDouble;

		// Token: 0x040039CA RID: 14794
		private DashButGoodBehavior.DashState m_state;

		// Token: 0x02000D1F RID: 3359
		public enum DashDirection
		{
			// Token: 0x040039CC RID: 14796
			PerpendicularToTarget = 10,
			// Token: 0x040039CD RID: 14797
			KindaTowardTarget = 20,
			// Token: 0x040039CE RID: 14798
			TowardTarget = 25,
			// Token: 0x040039CF RID: 14799
			Random = 30
		}

		// Token: 0x02000D20 RID: 3360
		private enum DashState
		{
			// Token: 0x040039D1 RID: 14801
			Idle,
			// Token: 0x040039D2 RID: 14802
			Charge,
			// Token: 0x040039D3 RID: 14803
			Dash
		}
	}
}

