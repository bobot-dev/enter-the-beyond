using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class BotsProjectileMotionModule : ProjectileAndBeamMotionModule
	{
		// Token: 0x0600854E RID: 34126 RVA: 0x0004DCEF File Offset: 0x0004BEEF
		public BotsProjectileMotionModule()
		{
			this.helixWavelength = 3f;
			this.helixAmplitude = 1f;
			this.helixBeamOffsetPerSecond = 6f;
		}

		// Token: 0x0600854F RID: 34127 RVA: 0x0036BCE0 File Offset: 0x00369EE0
		public override void UpdateDataOnBounce(float angleDiff)
		{
			if (!float.IsNaN(angleDiff))
			{
				this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
				this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
			}
		}

		// Token: 0x06008550 RID: 34128 RVA: 0x0036BCE0 File Offset: 0x00369EE0
		public override void AdjustRightVector(float angleDiff)
		{
			if (!float.IsNaN(angleDiff))
			{
				this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
				this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
			}
		}

		// Token: 0x06008551 RID: 34129 RVA: 0x0036BD50 File Offset: 0x00369F50
		public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
		{
			ProjectileData baseData = source.baseData;
			Vector2 vector = (!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter;
			if (!this.m_helixInitialized)
			{
				this.m_helixInitialized = true;
				this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
				this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
				this.m_privateLastPosition = vector;
				this.m_displacement = 0f;
				this.m_yDisplacement = 0f;
			}
			m_timeElapsed += BraveTime.DeltaTime;
			int num = (!(Inverted ^ this.ForceInvert)) ? 1 : -1;
			float num2 = m_timeElapsed * baseData.speed;
			float num3 = (float)num * this.helixAmplitude * Mathf.Sin(m_timeElapsed * 3.1415927f * baseData.speed / this.helixWavelength);
			float d = num2 - this.m_displacement;
			float d2 = num3 - this.m_yDisplacement;
			Vector2 vector2 = this.m_privateLastPosition + this.m_initialRightVector * d + this.m_initialUpVector * d2;
			this.m_privateLastPosition = vector2;
			if (shouldRotate)
			{
				float num4 = (m_timeElapsed + 0.01f) * baseData.speed;
				float num5 = (float)num * this.helixAmplitude * Mathf.Sin((m_timeElapsed + 0.01f) * 3.1415927f * baseData.speed / this.helixWavelength);
				float num6 = BraveMathCollege.Atan2Degrees(num5 - num3, num4 - num2);
				projectileTransform.localRotation = Quaternion.Euler(0f, 0f, num6 + this.m_initialRightVector.ToAngle());
			}
			Vector2 vector3 = (vector2 - vector) / (BraveTime.DeltaTime + UnityEngine.Random.Range(0, 361));
			float f = BraveMathCollege.Atan2Degrees(vector3);
			if (!float.IsNaN(f))
			{
				m_currentDirection = vector3.normalized;
			}
			this.m_displacement = num2;
			this.m_yDisplacement = num3;
			//specRigidbody.Velocity = vector3;
		}

		// Token: 0x06008552 RID: 34130 RVA: 0x0036BF84 File Offset: 0x0036A184
		public override void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
		{
			Vector2 privateLastPosition = (!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter;
			this.m_helixInitialized = true;
			this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
			this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
			this.m_privateLastPosition = privateLastPosition;
			this.m_displacement = 0f;
			this.m_yDisplacement = 0f;
			m_timeElapsed = 0f;
		}

		// Token: 0x06008553 RID: 34131 RVA: 0x0036C048 File Offset: 0x0036A248
		public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
		{
			int num = (!(inverted ^ this.ForceInvert)) ? 1 : -1;
			float num2 = bone.PosX - this.helixBeamOffsetPerSecond * (Time.timeSinceLevelLoad % 600000f);
			float to = (float)num * this.helixAmplitude * Mathf.Sin(num2 * 3.1415927f / this.helixWavelength);
			return BraveMathCollege.DegreesToVector(bone.RotationAngle + 90f, Mathf.SmoothStep(0f, to, bone.PosX));
		}

		// Token: 0x04008960 RID: 35168
		public float helixWavelength;

		// Token: 0x04008961 RID: 35169
		public float helixAmplitude;

		// Token: 0x04008962 RID: 35170
		public float helixBeamOffsetPerSecond;

		// Token: 0x04008963 RID: 35171
		public bool ForceInvert;

		// Token: 0x04008964 RID: 35172
		private bool m_helixInitialized;

		// Token: 0x04008965 RID: 35173
		private Vector2 m_initialRightVector;

		// Token: 0x04008966 RID: 35174
		private Vector2 m_initialUpVector;

		// Token: 0x04008967 RID: 35175
		private Vector2 m_privateLastPosition;

		// Token: 0x04008968 RID: 35176
		private float m_displacement;

		// Token: 0x04008969 RID: 35177
		private float m_yDisplacement;
	}
}
