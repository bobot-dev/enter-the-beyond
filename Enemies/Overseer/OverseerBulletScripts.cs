using Brave.BulletScript;
using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{

	public class SixBeamScript2 : Script
	{
		static SixBeamScript2()
		{
			SixBeamScript2.RampHeights = new float[]
			{
				2f,
				1f,
				0f,
				1f,
				2f,
				3f,
				4f,
				2f
			};

			SixBeamScript2.TargetOffsets = new Vector2[]
			{
			new Vector2(0f, 0.0625f),
			new Vector2(0.0625f, -0.0625f),
			new Vector2(0.0625f, 0f),
			new Vector2(0.0625f, -0.0625f),
			new Vector2(0.0625f, 0.0625f),
			new Vector2(0f, 0f),
			new Vector2(0.0625f, 0f),
			new Vector2(0.125f, -0.125f)
			};
		}

		private static Vector2[] TargetOffsets;
		private static float[] RampHeights;


		protected override IEnumerator Top()
		{

			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("default"));

			}

			CellArea area = this.BulletBank.aiActor.ParentRoom.area;
			Vector2 roomLowerLeft = area.UnitBottomLeft;
			Vector2 roomUpperRight = area.UnitTopRight - new Vector2(0f, 3f);
			Vector2 roomCenter = this.BulletBank.aiActor.Position;
			
			for (int j = 0; j < 18; j++)
			{
				for (int i = 0; i < 84; i++)
				{
					Vector2 vector = this.BulletBank.aiActor.CenterPosition;
					float angle = base.SubdivideCircle(0f, 84, i, 1f, false);
					Vector2 vector2 = vector + (BraveMathCollege.DegreesToVector(angle, 5f) * 6);
					Bullet bullet = new Default(vector, 12 + j);
					if (j == 0)
                    {
						bullet = new SkellBullet(vector, 12 + j);

					}
					base.Fire(Offset.OverridePosition(vector2), bullet);
				}
				yield return this.Wait(15);
			}

			

			yield return this.Wait(125);
		}
		public class Default : Bullet
		{
			public Default(Vector2 targetPos, float endingDist) : base("default", false, false, false)
			{
				this.m_targetPos = targetPos;

				this.m_endDist = endingDist;

			}
			protected override IEnumerator Top()
			{
				this.Direction = (this.m_targetPos - this.Position).ToAngle();
				this.Projectile.IgnoreTileCollisionsFor(90f);
				this.Projectile.IgnoreCollisionsFor(2f);

				this.Speed = 7f;
				while (Vector3.Distance(m_targetPos, this.Position) > m_endDist)
				{
					yield return this.Wait(1);
				}

				//yield return this.Wait(travelTime);
				this.Speed = 0f;
				yield return this.Wait(14*60);
				this.Vanish(true);
			}

			private Vector2 m_targetPos;
			private float m_endDist;
		}


		public class SkellBullet : Bullet
        {
            public SkellBullet(Vector2 targetPos, float endingDist) : base("sweep", false, false, false)
            {
				this.m_targetPos = targetPos;

				this.m_endDist = endingDist;

			}
            protected override IEnumerator Top()
            {
				this.Direction = (this.m_targetPos - this.Position).ToAngle();
				this.Projectile.IgnoreTileCollisionsFor(90f);
				this.Projectile.IgnoreCollisionsFor(2f);
				
				this.Speed = 7f;
				while (Vector3.Distance(m_targetPos, this.Position) > m_endDist)
				{
					yield return this.Wait(1);
				}

				//yield return this.Wait(travelTime);
				this.Speed = 0f;
				yield return this.Wait(14 * 60);
				this.Vanish(true);
			}

			private Vector2 m_targetPos;
			private float m_endDist;
		}


        public class CheeseWedgeBullet : Bullet
		{
			public CheeseWedgeBullet(SixBeamScript2 parent,  float additionalRampHeight, Vector2 targetPos, float endingAngle, float endingDist) : base("sweep", true, false, false)
			{
				this.m_parent = parent;
				this.m_targetPos = targetPos;
				this.m_endingAngle = endingAngle;
				this.m_additionalRampHeight = additionalRampHeight;
				this.m_endDist = endingDist;
			}

			protected override IEnumerator Top()
			{
				for(int i = 0; i < 5; i++)
                {
					int travelTime = 136;//UnityEngine.Random.RandomRange(90, 136);
					this.Projectile.IgnoreTileCollisionsFor(90f);
					this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle));
					//this.Projectile.sprite.HeightOffGround = 10f + this.m_additionalRampHeight;
					this.Projectile.sprite.ForceRotationRebuild();
					this.Projectile.sprite.UpdateZDepth();
					int r = 0;//UnityEngine.Random.Range(0, 20);
					yield return this.Wait(15);
					this.Speed = 2.5f;
					yield return this.Wait(50);
					this.Speed = 0f;

					this.Direction = (this.m_targetPos - this.Position).ToAngle();
					this.ChangeSpeed(new Speed((this.m_targetPos - this.Position).magnitude / ((float)(travelTime - 15) / 60f), SpeedType.Absolute), 30);

					while (Vector3.Distance(m_targetPos, this.Position) > m_endDist)
                    {
						yield return this.Wait(1);
					}

					//yield return this.Wait(travelTime);
					this.Speed = 0f;
					//this.Position = this.m_targetPos;
					this.Direction = this.m_endingAngle;
					if (this.Projectile && this.Projectile.sprite)
					{
						this.Projectile.sprite.HeightOffGround -= 1f;
						this.Projectile.sprite.UpdateZDepth();
					}
					int totalTime = 350;
					yield return this.Wait(totalTime - this.m_parent.Tick);
					this.Vanish(true);

					yield return this.Wait(30);
				}
				
				yield break;
			}

			// Token: 0x04000B82 RID: 2946
			private SixBeamScript2 m_parent;

			// Token: 0x04000B83 RID: 2947
			private Vector2 m_targetPos;
			private float m_endDist;
			// Token: 0x04000B84 RID: 2948
			private float m_endingAngle;



			// Token: 0x04000B86 RID: 2950
			private float m_additionalRampHeight;
		}

	}

	public class SixBeamScript : Script
	{

		static SixBeamScript()
		{

			SixBeamScript.QuarterPi = 0.785f;
			SixBeamScript.SkipSetupBulletNum = 6;
			SixBeamScript.SetupTime = 90;
			SixBeamScript.BulletCount = 25;
			SixBeamScript.Radius = 13f;
			SixBeamScript.QuaterRotTime = 36;
			SixBeamScript.SpinTime = 600;
			SixBeamScript.PulseInitialDelay = 120;
			SixBeamScript.PulseDelay = 120;
			SixBeamScript.PulseCount = 4;
			SixBeamScript.PulseTravelTime = 90;
		}



		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			this.EndOnBlank = true;
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("default"));

			}





			//FireCircleSegment(0);
			//FireCircleSegment(90);			
			//FireCircleSegment(180);
			//FireCircleSegment(270);
			yield return this.Wait(15);
			float num = 4.5f;



			for (int i = 0; i < 80; i++)
			{
				base.Fire(new Direction(((float)i + 0.5f) * num, DirectionType.Absolute, -1f), new Speed((SixBeamScript.Radius * 1.4f) / ((float)SixBeamScript.PulseTravelTime / 60f), SpeedType.Absolute), new CircleBullet(15));
			}

			for (int j = 0; j < 100000; j++)
			{
				var dir = BraveMathCollege.ClampAngle360(this.AimDirection + ((32) * 1.5f));

				for (int i = 0; i < 6; i++)
				{
					//this.Fire(new Direction(dir + (60 * i), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new SkellBullet());
				}
				yield return this.Wait(30);
			}



			yield break;
		}

		public static float QuarterPi;

		public static int SkipSetupBulletNum;

		public static int ExtraSetupBulletNum;

		public static int SetupTime;

		public static int BulletCount;

		public static float Radius;

		public static int QuaterRotTime;

		public static int SpinTime;

		public static int PulseInitialDelay;

		public static int PulseDelay;

		public static int PulseCount;

		public static int PulseTravelTime;

		private void FireCircleSegment(float dir)
		{
			for (int i = 0; i < SixBeamScript.BulletCount; i++)
			{
				base.Fire(new Direction(dir, DirectionType.Absolute, -1f), new Speed(SixBeamScript.Radius * 2f * (60f / (float)SixBeamScript.SetupTime), SpeedType.Absolute), new SkellBullet());
			}
		}

		public class CircleBullet : Bullet
		{

			public CircleBullet(int spawnTime) : base("default", false, false, false)
			{
				this.spawnTime = spawnTime;

				this.ForceBlackBullet = true;
			}


			protected override IEnumerator Top()
			{
				this.Projectile.specRigidbody.CollideWithTileMap = false;
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), SixBeamScript.SetupTime);
				yield return this.Wait(SixBeamScript.SetupTime);
				this.ChangeDirection(new Direction(90f, DirectionType.Relative, -1f), 1);
				yield return this.Wait(1);
				this.ChangeSpeed(new Speed(SixBeamScript.Radius * 2f * SixBeamScript.QuarterPi * (60f / (float)SixBeamScript.QuaterRotTime), SpeedType.Relative), 1);
				this.ChangeDirection(new Direction(90f / (float)SixBeamScript.QuaterRotTime, DirectionType.Sequence, -1f), SixBeamScript.QuaterRotTime);
				yield return this.Wait((float)this.spawnTime * ((float)SixBeamScript.QuaterRotTime / (float)SixBeamScript.BulletCount));
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 1);
				for (int i = 1; i < 14; i++)
				{
					this.Fire(new Direction(-90f, DirectionType.Relative, -1f), new Speed((float)(i * 3), SpeedType.Absolute), new CircleExtraBullet(this.spawnTime));
				}
				yield return this.Wait((float)(SixBeamScript.BulletCount - this.spawnTime) * ((float)SixBeamScript.QuaterRotTime / (float)SixBeamScript.BulletCount));
				yield return this.Wait(SixBeamScript.SpinTime - SixBeamScript.QuaterRotTime);
				this.Vanish(false);
				yield break;
			}

			// Token: 0x040002FE RID: 766
			public int spawnTime;
		}

		public class CircleExtraBullet : Bullet
		{
			public CircleExtraBullet(int spawnTime) : base("default", false, false, false)
			{
				this.spawnTime = spawnTime;



			}
			protected override IEnumerator Top()
			{

				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 60);
				yield return this.Wait((float)(SixBeamScript.BulletCount - this.spawnTime) * ((float)SixBeamScript.QuaterRotTime / (float)SixBeamScript.BulletCount));
				yield return this.Wait(SixBeamScript.SpinTime - SixBeamScript.QuaterRotTime);
				this.ChangeSpeed(new Speed(6f, SpeedType.Absolute), 1);
				yield return this.Wait(120);
				this.Vanish(false);
				yield break;
			}

			public int spawnTime;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class SPIIIIIIIIIIIIIN : Script
	{
		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			for (int i = 0; i < 10000; i++)
			{
				this.Fire(new Direction(-8, DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new SkellBullet());
				this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new SkellBullet());
				this.Fire(new Direction(8, DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), new SkellBullet());
				yield return this.Wait(1);
			}


			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class OverseerRemoteBullets : Script
	{

		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("3e98ccecf7334ff2800188c417e67c15").bulletBank.GetBullet("disruption"));

			}
			int numShots = UnityEngine.Random.Range(4, 8);
			OverseerRemoteBullets.AstralBullet astralBullet = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet2 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet3 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet4 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet5 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet6 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet7 = new OverseerRemoteBullets.AstralBullet(numShots);
			OverseerRemoteBullets.AstralBullet astralBullet8 = new OverseerRemoteBullets.AstralBullet(numShots);

			this.Fire(new Offset(0f, 3.4f, 0, string.Empty, DirectionType.Absolute), new Direction(45, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet);
			yield return this.Wait(8);
			this.Fire(new Offset(2.5f, 2.5f, 0, string.Empty, DirectionType.Absolute), new Direction(45, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet2);
			yield return this.Wait(8);
			this.Fire(new Offset(3.4f, 0f, 0, string.Empty, DirectionType.Absolute), new Direction(90, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet3);
			yield return this.Wait(8);
			this.Fire(new Offset(-2.5f, 2.5f, 0, string.Empty, DirectionType.Absolute), new Direction(135, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet4);
			yield return this.Wait(8);
			this.Fire(new Offset(0f, -3.4f, 0, string.Empty, DirectionType.Absolute), new Direction(180, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet5);
			yield return this.Wait(8);
			this.Fire(new Offset(-2.5f, -2.5f, 0, string.Empty, DirectionType.Absolute), new Direction(225, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet6);
			yield return this.Wait(8);
			this.Fire(new Offset(-3.4f, 0f, 0, string.Empty, DirectionType.Absolute), new Direction(270, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet7);
			yield return this.Wait(8);
			this.Fire(new Offset(2.5f, -2.5f, 0, string.Empty, DirectionType.Absolute), new Direction(315, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), astralBullet8);

			while (!astralBullet.Destroyed || !astralBullet2.Destroyed || !astralBullet3.Destroyed || !astralBullet4.Destroyed || !astralBullet5.Destroyed || !astralBullet6.Destroyed || !astralBullet7.Destroyed || !astralBullet8.Destroyed)
			{
				yield return null;
			}
			yield break;
		}

		

		// Token: 0x0200021B RID: 539
		public class AstralBullet : Bullet
		{

			public AstralBullet(int shotCount) : base("disruption", false, false, false)
			{
				numShots = shotCount;
			}
			int numShots;

			protected override IEnumerator Top()
			{
				yield return this.Wait(113);
				if (this.BulletBank.aiActor.healthHaver.IsDead)
				{
					this.Vanish(false);
				}
				this.Projectile.specRigidbody.CollideWithOthers = true;
				this.Direction = this.AimDirection;
				this.Speed = 0f;

				for (int i = 0; i < numShots; i++)
				{
					yield return this.Wait(UnityEngine.Random.Range(20, 70));
					if (this.BulletBank.aiActor.healthHaver.IsDead)
					{
						this.Vanish(false);
					}
					this.Projectile.spriteAnimator.PlayFromFrame("killithid_disruption_attack", 0);
					yield return this.Wait(15);
					//this.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), null);
					this.Fire(new Direction(-16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
					this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
					this.Fire(new Direction(16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
					yield return this.Wait(5);
					this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

				}
				yield return this.Wait(30);
				this.Vanish(false);
				yield break;
			}
		}
		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}


	public class OverseerDashAtPlayerScript : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			this.Fire(new Direction(-8, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(8, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			yield return this.Wait(5);
			this.Fire(new Direction(-12, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(-16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(12, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

			yield return this.Wait(5);

			this.Fire(new Direction(-8, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(8, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			yield return this.Wait(5);
			this.Fire(new Direction(-12, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(-16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(12, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}


	public class OverseerDashStartScript : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			this.Fire(new Direction(-16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			this.Fire(new Direction(16, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class OverseerTeleportStartLinesScript : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			var bulletCount = 6;



			for (int j = 0; j <= (bulletCount); j++)
			{
				this.Fire(new Direction((float)(j * (360 / bulletCount)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			}

			//yield return this.Wait(4);

			for (int i = 0; i <= 3; i++)
			{

				for (int j = 0; j <= (bulletCount); j++)
				{
					this.Fire(new Direction((float)(j * (360 / bulletCount)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				}
				yield return this.Wait(4);

				for (int j = 0; j <= (bulletCount); j++)
				{
					this.Fire(new Direction((float)((j * (360 / bulletCount)) + 360 / (bulletCount * 2)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				}
				yield return this.Wait(4);
			}
			for (int j = 0; j <= (bulletCount); j++)
			{
				this.Fire(new Direction((float)((j * (360 / bulletCount)) + 360 / (bulletCount * 2)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
			}

			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class OverseerRapidFireLines2 : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			var bulletCount = 3;
			var lineCount = 8;
			var repCount = UnityEngine.Random.Range(2, 5);



			//for (int k = 0; k <= repCount; k++)
			//{
				for (int i = 0; i <= bulletCount; i++)
				{
					for (int j = 0; j <= lineCount; j++)
					{
						this.Fire(new Direction((float)((j * (360 / bulletCount)) + 360 / (bulletCount * 2)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

					}
					yield return this.Wait(4);
				}
				yield return this.Wait(12);

			//}

			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class OverseerRapidFireLines : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			var bulletCount = 3;
			var lineCount = 8;
			var repCount = UnityEngine.Random.Range(2, 5);



			//for (int k = 0; k <= repCount; k++)
			//{
				for (int i = 0; i <= bulletCount; i++)
				{
					for (int j = 0; j <= lineCount; j++)
					{
						this.Fire(new Direction((float)(j * (360 / bulletCount)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

					}
					yield return this.Wait(4);
				}
				yield return this.Wait(12);

				for (int i = 0; i <= bulletCount; i++)
				{
					for (int j = 0; j <= lineCount; j++)
					{
						this.Fire(new Direction((float)((j * (360 / bulletCount)) + 360 / (bulletCount * 2)), DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

					}
					yield return this.Wait(4);
				}
				yield return this.Wait(12);

			//}

			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}

	public class OverseerTeleportStartDoubleLinesScript : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			var bulletCount = 7;
			var dir = AimDirection;

			for (int j = 0; j <= bulletCount; j++)
			{
				this.Fire(new Direction(dir + 25, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				this.Fire(new Direction(dir + -25, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

				this.Fire(new Direction(dir + 35, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());						
				this.Fire(new Direction(dir + -35, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());

				this.Fire(new Direction(dir + 45, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				this.Fire(new Direction(dir + -45, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				yield return this.Wait(4);
			}
			

			yield break;
		}
		public class Default : Bullet
		{
			public Default() : base("default", false, false, false)
			{
			}
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{


			}
		}
	}


	public class OverseerTeleportStartScript : Script
	{

		protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

			}
			for (int i = 0; i <= 36; i++)
			{
				this.Fire(new Direction((float)(i * 10), DirectionType.Relative, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet());
				this.Fire(new Direction((float)(i * 10), DirectionType.Relative, -1f), new Speed(11f, SpeedType.Absolute), new SkellBullet2());

			}
			yield break;
		}

		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{

				this.AutoRotation = true;
			}

			protected override IEnumerator Top()
			{
				yield return this.Wait(4f);
				this.ChangeDirection(new Direction(90, DirectionType.Relative, -1f), 1);
				//while (this != null)
				//{

				//yield return this.Wait(16f);
				//this.ChangeDirection(new Direction(90, DirectionType.Relative, -1f), 1);
				///yield return this.Wait(16f);
				//this.ChangeDirection(new Direction(-90, DirectionType.Relative, -1f), 1);
				//}


				var SetupTime = 30;

				//yield return this.Wait(SetupTime);
				//this.ChangeDirection(new Direction(180f, DirectionType.Relative, -1f), 1);

			}
		}

		public class SkellBullet2 : Bullet
		{
			public SkellBullet2() : base("sweep", false, false, false)
			{

				this.AutoRotation = true;
			}

			protected override IEnumerator Top()
			{
				yield return this.Wait(4f);
				this.ChangeDirection(new Direction(-90, DirectionType.Relative, -1f), 1);
				//while (this != null)
				//{
				//yield return this.Wait(16f);
				//this.ChangeDirection(new Direction(-90, DirectionType.Relative, -1f), 1);
				//yield return this.Wait(16f);
				//this.ChangeDirection(new Direction(90, DirectionType.Relative, -1f), 1);
				//}



				var SetupTime = 30;

				//yield return this.Wait(SetupTime);
				//this.ChangeDirection(new Direction(180f, DirectionType.Relative, -1f), 1);

			}
		}

	}


	public class OverseerTwinBeams1 : Script
	{

		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").bulletBank.GetBullet("default_novfx"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
			}

			this.EndOnBlank = true;
			float startDirection = this.AimDirection;
			float sign = BraveUtility.RandomSign();
			for (int i = 0; i < 210; i++)
			{
				float offset = 0f;
				if (i < 30)
				{
					offset = Mathf.Lerp(135f, 0f, (float)i / 29f);
				}
				float currentAngle = startDirection + sign * Mathf.Max(0f, (float)(i - 60) * 1f);
				for (int j = 0; j < 5; j++)
				{
					float num = offset + 20f + (float)j * 10f;
					this.Fire(new Offset("right eye"), new Direction(currentAngle + num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
					this.Fire(new Offset("left eye"), new Direction(currentAngle - num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
				}
				if (i > 30 && i % 30 == 29)
				{
					this.Fire(new Direction(currentAngle + UnityEngine.Random.Range(-1f, 1f) * 20f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new SkellBullet());
				}				
				if (i > 60)
				{
					float num2 = Vector2.Distance(this.BulletManager.PlayerPosition(), this.Position);
					float num3 = num2 / 18f * 30f;
					if (num3 > (float)(i - 60))
					{
						num3 = (float)Mathf.Max(0, i - 60);
					}
					float num4 = -sign * num3 * 1f;
					float num5 = currentAngle + 40f + num4;
					float num6 = currentAngle - 40f + num4;
					if (BraveMathCollege.ClampAngle180(num5 - this.GetAimDirection("right eye")) < 0f)
					{
						yield break;
					}
					if (BraveMathCollege.ClampAngle180(num6 - this.GetAimDirection("left eye")) > 0f)
					{
						yield break;
					}
				}


				yield return this.Wait(2);
			}
			yield break;
		}
		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{

			}
		}

		private const float CoreSpread = 20f;

		private const float IncSpread = 10f;

		private const float TurnSpeed = 1f;

		private const float BulletSpeed = 18f;
	}


	public class BossFinalLostSword1 : Script
	{

		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("8d441ad4e9924d91b6070d5b3438d066").bulletBank.GetBullet("default"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").bulletBank.GetBullet("default_novfx"));


			}
			this.EndOnBlank = true;
			this.m_sign = BraveUtility.RandomSign();
			this.m_doubleSwing = BraveUtility.RandomBool();
			Vector2 leftOrigin = new Vector2(this.m_sign * 2f, -0.2f);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, 0.2f), new Vector2(this.m_sign * 11f, 0.2f), 14);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 11.6f, -0.2f), new Vector2(this.m_sign * 11.6f, -0.2f), 2);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, -0.6f), new Vector2(this.m_sign * 11f, -0.6f), 14);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.1f, -1.2f), new Vector2(this.m_sign * 3.1f, 0.8f), 4);
			this.FireLine(leftOrigin, new Vector2(this.m_sign * 2.2f, -0.2f), new Vector2(this.m_sign * 2.7f, -0.2f), 2);
			yield return this.Wait(75);
			yield break;
		}


		private void FireLine(Vector2 spawnPoint, Vector2 start, Vector2 end, int numBullets)
		{
			Vector2 a = (end - start) / (float)Mathf.Max(1, numBullets - 1);
			float num = 0.33333334f;
			for (int i = 0; i < numBullets; i++)
			{
				Vector2 a2 = (numBullets != 1) ? (start + a * (float)i) : end;
				float speed = Vector2.Distance(a2, spawnPoint) / num;
				base.Fire(new Offset(spawnPoint, 0f, string.Empty, DirectionType.Absolute), new Direction((a2 - spawnPoint).ToAngle(), DirectionType.Absolute, -1f), new Speed(speed, SpeedType.Absolute), new BossFinalLostSword1.SwordBullet(base.Position, this.m_sign, this.m_doubleSwing));
			}
		}

		// Token: 0x0400023D RID: 573
		private const int SetupTime = 20;

		// Token: 0x0400023E RID: 574
		private const int HoldTime = 30;

		// Token: 0x0400023F RID: 575
		private const int SwingTime = 25;

		// Token: 0x04000240 RID: 576
		private float m_sign;

		// Token: 0x04000241 RID: 577
		private bool m_doubleSwing;



		public class SwordBullet : Bullet
		{

			public SwordBullet(Vector2 origin, float sign, bool doubleSwing) : base("default", false, false, false)
			{
				this.m_origin = origin;
				this.m_sign = sign;
				this.m_doubleSwing = doubleSwing;

				//this.Projectile.PenetratesInternalWalls = true;
				//this.Projectile.pierceMinorBreakables = true;
				//this.Projectile.specRigidbody.CollideWithTileMap = false;
				//this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true; 
			}


			protected override IEnumerator Top()
			{
				yield return this.Wait(20);
				float angle = (this.Position - this.m_origin).ToAngle();
				float dist = Vector2.Distance(this.Position, this.m_origin);
				this.Speed = 0f;
				yield return this.Wait(30);
				this.ManualControl = true;
				int swingtime = (!this.m_doubleSwing) ? 25 : 100;
				float swingDegrees = (float)((!this.m_doubleSwing) ? 180 : 540);
				for (int i = 0; i < swingtime; i++)
				{
					float newAngle = angle - Mathf.SmoothStep(0f, this.m_sign * swingDegrees, (float)i / (float)swingtime);
					this.Position = this.m_origin + BraveMathCollege.DegreesToVector(newAngle, dist);
					yield return this.Wait(1);
				}
				//UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position, Quaternion.identity);
				yield return this.Wait(5);
				this.Vanish(false);
				yield break;
			}

			// Token: 0x04000242 RID: 578
			private Vector2 m_origin;

			// Token: 0x04000243 RID: 579
			private float m_sign;

			// Token: 0x04000244 RID: 580
			private bool m_doubleSwing;
		}
	}


	public class ShockwaveChallengeCircleBurst : Script
	{

		protected override IEnumerator Top()
		{
			float num = base.RandomAngle();
			float num2 = 30f;
			for (int i = 0; i < 12; i++)
			{
				base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), null);
			}
			return null;
		}

		// Token: 0x04000C7A RID: 3194
		private const int NumBullets = 12;
	}

	public class SheildScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				//base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("bigBullet"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
			}
			//float num = -45f;
			float desiredAngle = this.GetAimDirection(1f, 12f);
			float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
			bool isBlackPhantom = this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.IsBlackPhantom;
			//Bullet bullet = new BurstingBullet(isBlackPhantom, num);

			OverseerShield shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(0, -2), Quaternion.Euler(0, 0, 0));
			OverseerShield shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(2, 2), Quaternion.Euler(0, 0, 120));
			OverseerShield shield3 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, this.Position + new Vector2(-2, 2), Quaternion.Euler(0, 0, 240));
			//UnityEngine.Object.Instantiate(BotsModule.overseerShield, LostPastBoss.overseerAiActor.transform.Find("attach").position - new Vector3(1, 0, 0), Quaternion.identity);

			//UnityEngine.Object.Instantiate(BotsModule.overseerShield, LostPastBoss.overseerAiActor.transform.Find("attach").position + new Vector3(1, 0, 0), Quaternion.identity);


			shield1.self = shield1.gameObject;
			shield2.self = shield2.gameObject;
			shield3.self = shield3.gameObject;

			shield1.leader = OverseerFloor.OverseerPrefab;
			shield2.leader = OverseerFloor.OverseerPrefab;
			shield3.leader = OverseerFloor.OverseerPrefab;


			shield1.offset = new Vector3(0, -2, 0);    //-1-
			shield2.offset = new Vector3(2, 2, 0);  //---
			shield3.offset = new Vector3(-2, 2, 0);   //2-3



			//shield3.StartCoroutine(removeShield(shield3));
			for (int i = 0; i < 3; i++)
			{
				float num = 120 * (float)i;
				base.Fire(null, new Direction(num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom)); ;
			}

			yield return null;
		}


		public class BurstingBullet : Bullet
		{

			public BurstingBullet(bool isBlackPhantom) : base("bigBullet", false, false, false)
			{
				//this.deltaAngle = deltaAngle;
				base.ForceBlackBullet = true;
				this.m_isBlackPhantom = isBlackPhantom;

			}

			private float deltaAngle;

			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (preventSpawningProjectiles)
				{
					return;
				}
				float num = base.RandomAngle();
				float num2 = 20f;
				for (int i = 0; i < 18; i++)
				{
					Bullet bullet = new Bullet(null, false, false, false);
					base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), bullet);
					if (!this.m_isBlackPhantom)
					{
						bullet.ForceBlackBullet = false;
						bullet.Projectile.ForceBlackBullet = false;
						bullet.Projectile.ReturnFromBlackBullet();
					}
				}
			}

			private const int NumBullets = 18;

			private bool m_isBlackPhantom;
		}
	}


	public class SwordScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
			}
			//this.EndOnBlank = true;
			for (int i = 0; i < 22; i++)
			{
				this.Fire(new Direction(0, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedUpSwordBullet(i));
				//base.Fire(new Direction(-40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet -40 degrees from the enemy aim angle, with a bullet speed of 6.
				//base.Fire(new Direction(40f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), null); // Shoot a bullet 40 degrees from the enemy aim angle, with a bullet speed of 6.

				yield return Wait(1); // Wait for 20 frames

				//base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet -20 degrees from the enemy aim angle, with a bullet speed of 9.
				//base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null); // Shoot a bullet 20 degrees from the enemy aim angle, with a bullet speed of 9.
			}
			yield break;
		}

		private class SwordBullet : Bullet
		{

			public SwordBullet() : base("default", false, false, false)
			{
			}
		}

		public class SpeedUpSwordBullet : Bullet
		{

			public SpeedUpSwordBullet(float pointInLine) : base("reversible", false, false, false)
			{
				possitsion = pointInLine;
				//this.spawnTime = spawnTime;

			}
			public int spawnTime;
			float possitsion = 1;
			int SetupTime = 90;
			int BulletCount = 22;
			float QuarterPi = 0.785f;
			int SkipSetupBulletNum = 6;

			float Radius = 11f;
			int QuaterRotTime = 120;
			int SpinTime = 600;
			int PulseInitialDelay = 120;
			int PulseDelay = 120;
			int PulseCount = 4;
			int PulseTravelTime = 100;

			protected override IEnumerator Top()
			{
				this.ChangeSpeed(new Speed(0f, SpeedType.Absolute), SetupTime);
				yield return this.Wait(SetupTime);
				this.ChangeDirection(new Direction(90f, DirectionType.Relative, -1f), 1);
				yield return this.Wait(1);
				this.ChangeSpeed(new Speed((float)this.spawnTime / (float)BulletCount * (Radius * 2f) * QuarterPi * (60f / (float)QuaterRotTime), SpeedType.Relative), 1);
				this.ChangeDirection(new Direction(90f / (float)QuaterRotTime, DirectionType.Sequence, -1f), SpinTime);
				yield return this.Wait(SpinTime - 1);
				this.Vanish(false);
				yield break;
			}
		}
	}


	public class SpawnSkeletonBullet : Bullet
	{

		public SpawnSkeletonBullet() : base("skull", false, false, false)
		{
		}
		/*
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			var list = new List<string> {
				//"shellet",
				"336190e29e8a4f75ab7486595b700d4a"
			};
			string guid = BraveUtility.RandomElement<string>(list);
			var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
			AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
		}
		*/
	}




	
}
