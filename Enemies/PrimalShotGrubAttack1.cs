using Brave.BulletScript;
using FrostAndGunfireItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LostItems
{

    public class PrimalShotGrubAttack1 : OverrideBehavior
    {
		public override string OverrideAIActorGUID => "bot:primalshotgrub3"; // Replace the GUID with whatever enemy you want to modify. This GUID is for the bullet kin.
														   // You can find a full list of GUIDs at https://github.com/ModTheGungeon/ETGMod/blob/master/Assembly-CSharp.Base.mm/Content/gungeon_id_map/enemies.txt
		public override void DoOverride()
		{

			// For this first change, we're just going to increase the lead amount of the bullet kin's ShootGunBehavior so its shots fire like veteran kin.
			ShootGunBehavior shootGunBehavior = behaviorSpec.AttackBehaviors[0] as ShootGunBehavior; // Get the ShootGunBehavior, at index 0 of the AttackBehaviors list
			shootGunBehavior.LeadAmount = 0.62f;

			// Next, we're going to change another few things on the ShootGunBehavior so that it has a custom BulletScript.
			shootGunBehavior.WeaponType = WeaponType.BulletScript; // Makes it so the bullet kin will shoot our bullet script instead of his own gun shot.
			shootGunBehavior.BulletScript = new CustomBulletScriptSelector(typeof(TestBulletScript)); // Sets the bullet kin's bullet script to our custom bullet script.
		}


		public class TestBulletScript : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
        {
			protected override IEnumerator Top()
			{
				//float num = -45f;
				float desiredAngle = this.GetAimDirection(1f, 12f);
				float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
				bool isBlackPhantom = this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.IsBlackPhantom;
				//Bullet bullet = new BurstingBullet(isBlackPhantom, num);


				for (int i = 0; i < 3; i++)
				{
					float num = -45f + (float)i * 30f;
					base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Aim), new Direction(num, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BurstingBullet(isBlackPhantom));
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
						//Bullet bullet = new Bullet(null, false, false, false);

						float num3 = -22.5f;
						float num4 = 9f;

						ShotgrubManAttack1.GrossBullet bullet = new ShotgrubManAttack1.GrossBullet(num3 + (float)i * num4);
						base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), bullet);
						if (!this.m_isBlackPhantom)
						{
							bullet.ForceBlackBullet = false;
							bullet.Projectile.ForceBlackBullet = false;
							bullet.Projectile.ReturnFromBlackBullet();
							
						}
					}
				}

				// Token: 0x04000697 RID: 1687
				private const int NumBullets = 18;

				// Token: 0x04000698 RID: 1688
				private bool m_isBlackPhantom;
			}

		}
    }
}
