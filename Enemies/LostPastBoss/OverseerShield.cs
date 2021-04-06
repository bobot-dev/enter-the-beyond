using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    public class OverseerShield : MonoBehaviour
    {
		void Start()
		{
			SpeculativeRigidbody specRigidbody = this.GetComponent<SpeculativeRigidbody>();
			specRigidbody.specRigidbody.OnPreRigidbodyCollision += PreventBulletCollisions;
			BotsModule.Log("shield spawned", BotsModule.TEXT_COLOR);
			//StartCoroutine(Destrot());



			//UnityEngine.Object.Destroy(self, 5);
		}
		public static GameActor newOwner;


		public GameObject leader;
		public GameObject self;
		public Vector3 offset;


		public void CallShield(GameObject leader, Vector3 offset, GameObject self)
		{
			if (this.GetComponent<SpeculativeRigidbody>().Position.m_position != new Position(new Vector3(leader.GetComponent<tk2dSprite>().sprite.WorldCenter.x, leader.GetComponent<tk2dSprite>().sprite.WorldCenter.y, 0) + offset).m_position)
			{
				this.GetComponent<SpeculativeRigidbody>().Position = new Position(new Vector3(leader.GetComponent<tk2dSprite>().sprite.WorldCenter.x, leader.GetComponent<tk2dSprite>().sprite.WorldCenter.y, 0) + offset);
				this.transform.position = new Vector3(leader.GetComponent<tk2dSprite>().sprite.WorldCenter.x, leader.GetComponent<tk2dSprite>().sprite.WorldCenter.y, 0) + offset;
			}
			//self.transform.position = leader.transform.position;
		}

		private IEnumerator Destrot()
		{
			yield return new WaitForSeconds(5);
			while (leader != null && offset != null && self != null)
			{
				CallShield(leader, offset, self);
			}
			
		}

		void Update()
		{


		}

		private void PreventBulletCollisions(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (otherRigidbody.projectile)
			{
				ReflectBullet(otherRigidbody.projectile, true, newOwner, 10f, 1f, 1f, 0f);
				PhysicsEngine.SkipCollision = true;
			}
		}

		public static void ReflectBullet(Projectile p, bool retargetReflectedBullet, GameActor newOwner, float minReflectedBulletSpeed, float scaleModifier = 1f, float damageModifier = 1f, float spread = 0f)
		{
			if (p.Owner != GameManager.Instance.PrimaryPlayer)
			{
				return;
			}
			p.RemoveBulletScriptControl();
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", GameManager.Instance.gameObject);
			if (retargetReflectedBullet && p.Owner && p.Owner.specRigidbody)
			{
				p.Direction = (p.Owner.specRigidbody.GetUnitCenter(ColliderType.HitBox) - p.specRigidbody.UnitCenter).normalized;
			}
			else
			{
				Vector2 vector = p.LastVelocity;
				if (p.IsBulletScript && p.braveBulletScript && p.braveBulletScript.bullet != null)
				{
					vector = p.braveBulletScript.bullet.Velocity;
				}
				p.Direction = -vector.normalized;
				if (p.Direction == Vector2.zero)
				{
					p.Direction = UnityEngine.Random.insideUnitCircle.normalized;
				}
			}
			if (spread != 0f)
			{
				p.Direction = p.Direction.Rotate(UnityEngine.Random.Range(-spread, spread));
			}
			if (p.Owner && p.Owner.specRigidbody)
			{
				p.specRigidbody.DeregisterSpecificCollisionException(p.Owner.specRigidbody);
			}
			p.Owner = newOwner;
			p.SetNewShooter(newOwner.specRigidbody);
			p.allowSelfShooting = false;
			if (newOwner is AIActor)
			{
				p.collidesWithPlayer = true;
				p.collidesWithEnemies = false;
			}
			else
			{
				p.collidesWithPlayer = false;
				p.collidesWithEnemies = true;
			}
			if (scaleModifier != 1f)
			{
				SpawnManager.PoolManager.Remove(p.transform);
				p.RuntimeUpdateScale(scaleModifier);
			}
			if (p.Speed < minReflectedBulletSpeed)
			{
				p.Speed = minReflectedBulletSpeed;
			}
			if (p.baseData.damage < ProjectileData.FixedFallbackDamageToEnemies)
			{
				p.baseData.damage = ProjectileData.FixedFallbackDamageToEnemies;
			}
			p.baseData.damage *= damageModifier;
			if (p.baseData.damage < 10f)
			{
				p.baseData.damage = 15f;
			}
			p.UpdateCollisionMask();
			p.ResetDistance();
			p.Reflected();
			p.MakeLookLikeEnemyBullet(true);
		}
	}
}
