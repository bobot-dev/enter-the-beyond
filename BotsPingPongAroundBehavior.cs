using System;
using UnityEngine;

// Token: 0x02000DEE RID: 3566
namespace BotsMod
{
    public class BotsPingPongAroundBehavior : MovementBehaviorBase
    {
        public BotsPingPongAroundBehavior()
        {
            this.startingAngles = new float[]
            {
                    45f,
                    135f,
                    225f,
                    315f
            };
            this.motionType = BotsPingPongAroundBehavior.MotionType.Diagonals;
        }

        private bool ReflectX
        {
            get
            {
                return this.motionType == MotionType.Diagonals || this.motionType == MotionType.Horizontal;
            }
        }

        private bool ReflectY
        {
            get
            {
                return this.motionType == MotionType.Diagonals || this.motionType == MotionType.Vertical;
            }
        }

        public override void Start()
        {
            base.Start();
            this.m_aiActor.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyBlocker));
            SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
            specRigidbody.OnTileCollision += OnCollision;
        }

        public override BehaviorResult Update()
        {
            this.m_startingAngle = BraveMathCollege.ClampAngle360(BraveUtility.RandomElement<float>(this.startingAngles));
            this.m_aiActor.BehaviorOverridesVelocity = true;
            this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_startingAngle, this.m_aiActor.MovementSpeed);
            this.m_isBouncing = true;
            return BehaviorResult.RunContinuousInClass;
        }

        public override ContinuousBehaviorResult ContinuousUpdate()
        {
            base.ContinuousUpdate();
            return this.m_aiActor.BehaviorOverridesVelocity ? ContinuousBehaviorResult.Continue : ContinuousBehaviorResult.Finished;
        }

        public override void EndContinuousUpdate()
        {
            base.EndContinuousUpdate();
            this.m_isBouncing = false;
        }

        protected virtual void OnCollision(CollisionData collision)
        {
            if (!this.m_isBouncing)
            {
                return;
            }
            if (collision.OtherRigidbody)
            {
                return;
            }
            if (collision.CollidedX || collision.CollidedY)
            {
                Vector2 vector = collision.MyRigidbody.Velocity;
                if (collision.CollidedX && this.ReflectX)
                {
                    vector.x *= -1f;
                }
                if (collision.CollidedY && this.ReflectY)
                {
                    vector.y *= -1f;
                }
                if (this.motionType == MotionType.Horizontal)
                {
                    //vector.y = 0f;
                }
                if (this.motionType == MotionType.Vertical)
                {
                    //vector.x = 0f;
                }
                vector = vector.normalized * this.m_aiActor.MovementSpeed;
                PhysicsEngine.PostSliceVelocity = new Vector2?(vector);
                this.m_aiActor.BehaviorVelocity = vector;
                
                float num2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;

                Quaternion target = Quaternion.Euler(0, 0, num2);

                //this.m_aiActor.gameObject.transform.localRotation.eulerAngles = Quaternion.Slerp(this.m_aiActor.transform.rotation, target, 0);



                BotsModule.Log("AHHHH");
                AkSoundEngine.PostEvent("Play_Fuck", this.m_aiActor.gameObject);
            }
        }

        public float[] startingAngles;

        public MotionType motionType;

        private bool m_isBouncing;

        private float m_startingAngle;

        public enum MotionType
        {
            Diagonals = 10,
            Horizontal = 20,
            Vertical = 30
        }
    }
}
