using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    public class CurvedBeamMotionModule : ProjectileAndBeamMotionModule
    {
		public CurvedBeamMotionModule()
		{
			this.baseCurve = 1f;
			this.curveIntensity = 1f;
		}

		public override void UpdateDataOnBounce(float angleDiff)
		{
			
		}

		public override void AdjustRightVector(float angleDiff)
		{
			
		}

		public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
		{
			//base.Move(source, projectileTransform, projectileSprite, specRigidbody, ref m_timeElapsed, ref m_currentDirection, Inverted, shouldRotate);
		}

		public override void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
		{
		}

		public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
		{

			float to = (invert ? -1 : 1) * (1 - 1 / (1 + baseCurve * curveIntensity));
			
			return BraveMathCollege.DegreesToVector(bone.RotationAngle + 90f, Mathf.SmoothStep(0f, to, bone.PosX));
		}

		public float baseCurve;
		public float curveIntensity;
		public bool invert;
	}
}
