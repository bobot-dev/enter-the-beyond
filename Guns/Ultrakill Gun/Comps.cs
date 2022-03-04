using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class Bleed : MonoBehaviour
	{
		public int bloodAmount = 1;

		void Start()
		{
			if (this.GetComponent<Projectile>() != null)
			{
				this.GetComponent<Projectile>().OnHitEnemy += OnHit;
			}
		}

		private void OnHit(Projectile projectile, SpeculativeRigidbody target, bool fatal)
		{
			//ETGModConsole.Log("hit something without a brain");
			if (target.aiActor != null && fatal)
			{
				//ETGModConsole.Log("murder");

				var blood = FakePrefab.Clone(PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject).GetComponent<ParticleSystem>();			

				for (int i = 0; i < bloodAmount; i++)
				{
					Instantiate(blood, target.UnitCenter, Quaternion.identity);
				}

				if (projectile.Owner != null && projectile.Owner is PlayerController)
                {
					var player = projectile.Owner as PlayerController;
					//BotsModule.Log(Vector2.Distance(player.specRigidbody.UnitCenter, target.UnitCenter).ToString());
					if (Vector2.Distance(player.specRigidbody.UnitCenter, target.UnitCenter) <= 4.5f && player.CurrentGun?.GetComponent<UltraKillGun>() != null)
					{
						//BotsModule.Log((50 - (Vector2.Distance(player.specRigidbody.UnitCenter, target.UnitCenter)) * 10).ToString());
						player.CurrentGun.GainAmmo((int)(50 - (Vector2.Distance(player.specRigidbody.UnitCenter, target.UnitCenter)) * 10));
					}
				}				
			}

		}
	}


	class UltraKillGun : MonoBehaviour
	{
		void Start()
		{
			if (this.GetComponent<Gun>() != null)
			{

			}
		}
	}
	class ProjBoost : BraveBehaviour
	{
		public void Start()
		{

			if (this.specRigidbody != null)
			{
				this.specRigidbody.OnPreRigidbodyCollision += OnHitSomething;
			}

		}


		void OnHitSomething(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			
			if (otherRigidbody.gameObject.GetComponent<PlayerController>() != null)
			{
				BotsModule.Log("fish");
				var player = otherRigidbody.GetComponent<PlayerController>();
				//if (player.CurrentRollState == PlayerController.DodgeRollState.InAir)
				//{
					myRigidbody.projectile.baseData.speed *= 3;
				myRigidbody.projectile.UpdateSpeed();
					myRigidbody.projectile.OnDestruction += Obj_OnDestruction;
				Exploder.DoDefaultExplosion(myRigidbody.sprite.WorldCenter, Vector2.zero);
				//}

			}
		}

		private void Obj_OnDestruction(Projectile obj)
		{
			
		}
	}
    #region Nail Gun
    public class NailHomingModifier : BraveBehaviour
	{

		public NailHomingModifier()
		{
			this.HomingRadius = 2f;
			this.AngularVelocity = 180f;

		}

		private void Start()
		{
			if (!this.m_projectile)
			{
				this.m_projectile = base.GetComponent<Projectile>();
			}
			Projectile projectile = this.m_projectile;
			projectile.specRigidbody.OnCollision += this.OnCollision;
			//BotsModule.Log("ahhhh");
			projectile.ModifyVelocity += this.ModifyVelocity;
		}

		private void OnCollision(CollisionData tileCollision)
		{
			if (tileCollision.OtherRigidbody && tileCollision.OtherRigidbody.gameObject && tileCollision.OtherRigidbody.gameObject.GetComponent<NailMagnet>())
			{
				//BotsModule.Log("magnet");
				//this.m_projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
				PhysicsEngine.SkipCollision = true;
			}
		}

		public void AssignProjectile(Projectile source)
		{
			this.m_projectile = source;
		}

		private Vector2 ModifyVelocity(Vector2 inVel)
		{
			Vector2 vector = inVel;
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_projectile.LastPosition.IntXY(VectorConversions.Floor));

			List<NailMagnet> activeEnemies = NailMagnet.magnets;
			if (activeEnemies == null || activeEnemies.Count == 0)
			{
				return inVel;
			}
			float num = float.MaxValue;
			Vector2 vector2 = Vector2.zero;
			NailMagnet x = null;
			Vector2 b = (!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				NailMagnet aiactor = activeEnemies[i];
				if (aiactor && aiactor.GetComponent<SpeculativeRigidbody>())
				{
					//BotsModule.Log("ok no nulls good good");
					base.specRigidbody.RegisterSpecificCollisionException(aiactor.GetComponent<SpeculativeRigidbody>());
					Vector2 vector3 = aiactor.GetComponent<SpeculativeRigidbody>().UnitCenter - b;
					float sqrMagnitude = vector3.sqrMagnitude;
					if (sqrMagnitude < num)
					{
						vector2 = vector3;
						num = sqrMagnitude;
						x = aiactor;
					}
				}
			}


			num = Mathf.Sqrt(num);
			if (num < this.HomingRadius && x != null)
			{
				float num2 = 1f - num / this.HomingRadius;
				float target = vector2.ToAngle();
				float num3 = inVel.ToAngle();
				float maxDelta = this.AngularVelocity * num2 * this.m_projectile.LocalDeltaTime;
				float num4 = Mathf.MoveTowardsAngle(num3, target, maxDelta);
				if (this.m_projectile is HelixProjectile)
				{
					float angleDiff = num4 - num3;
					(this.m_projectile as HelixProjectile).AdjustRightVector(angleDiff);
				}
				else
				{
					if (this.m_projectile.shouldRotate)
					{
						base.transform.rotation = Quaternion.Euler(0f, 0f, num4);
					}
					vector = BraveMathCollege.DegreesToVector(num4, inVel.magnitude);
				}
				if (this.m_projectile.OverrideMotionModule != null)
				{
					this.m_projectile.OverrideMotionModule.AdjustRightVector(num4 - num3);
				}

			}

			if (x && Vector2.Distance(b, vector2) < 1)
            {
				x.nails.Add(this);
            } 
			else
            {
				if(x.nails.Contains(this))
                {
					x.nails.Remove(this);
				}
            }

			if (vector == Vector2.zero || float.IsNaN(vector.x) || float.IsNaN(vector.y))
			{
				return inVel;
			}

			

			return vector;
		}

		protected override void OnDestroy()
		{
			if (this.m_projectile)
			{
				Projectile projectile = this.m_projectile;
				projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Remove(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
			}
			base.OnDestroy();
		}

		public float HomingRadius;

		public float AngularVelocity;

		protected Projectile m_projectile;
	}

	public class NailMagnet : MonoBehaviour
    {
		public static List<NailMagnet> magnets = new List<NailMagnet>();
		public List<NailHomingModifier> nails = new List<NailHomingModifier>();
		private float m_hitNormal;
		private PlayerController projOwner;
		private Projectile m_projectile;
		bool hasHitSomething;

		private void Start()
		{
			this.m_projectile = base.GetComponent<Projectile>();
			if (this.m_projectile.Owner is PlayerController)
			{
				this.projOwner = this.m_projectile.Owner as PlayerController;
			}
			SpeculativeRigidbody specRigidBody = this.m_projectile.specRigidbody;
			this.m_projectile.BulletScriptSettings.surviveTileCollisions = true;
			this.m_projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			this.m_projectile.gameObject.GetOrAddComponent<PierceProjModifier>().penetration = 3;
			specRigidBody.OnCollision += this.OnCollision;
			this.m_projectile.OnHitEnemy += OnHitEnemy;
            //this.m_projectile.OnDestruction += M_projectile_OnDestruction;
			if (this.m_projectile)
			{
				UnityEngine.Object.Destroy(this.m_projectile.gameObject, 20);
			}
		}

        private void OnDestroy()
        {
			Exploder.Explode(this.m_projectile.specRigidbody.UnitCenter, (PickupObjectDatabase.GetById(304) as ComplexProjectileModifier).ExplosionData, this.m_projectile.specRigidbody.UnitCenter.normalized, null, true, CoreDamageTypes.None, false);
			if (magnets.Contains(this))
			{
				magnets.Remove(this);
			}
		}
		Vector3 enemyOffset;
		bool attachedToEnemy = false;
		Transform enemyTransform;

        void OnHitEnemy(Projectile proj, SpeculativeRigidbody target, bool fatal)
        {
			if (target.aiActor != null && !fatal)
            {
				

				if (this.m_projectile)
				{
					//this.m_projectile.gameObject.transform.parent = target.healthHaver.transform;
					m_projectile.ManualControl = true;
					m_projectile.collidesWithEnemies = false;
					enemyOffset = m_projectile.transform.position - target.healthHaver.transform.position;
					enemyTransform = target.healthHaver.transform;
					attachedToEnemy = true;
				}

				var enemy = target.aiActor;

                enemy.healthHaver.OnPreDeath += HealthHaver_OnPreDeath;

				proj.OnHitEnemy -= OnHitEnemy;
			}
		}

		private void Update()
		{
			if (this.m_projectile && nails.Count >= 100)
			{
				UnityEngine.Object.Destroy(this.m_projectile.gameObject);
			}

			if (attachedToEnemy && enemyTransform && m_projectile) m_projectile.transform.position = enemyTransform.position + enemyOffset;
		}

		private void HealthHaver_OnPreDeath(Vector2 obj)
        {			
			if (this.m_projectile)
            {
				UnityEngine.Object.Destroy(this.m_projectile.gameObject);
			}
			//UnityEngine.Object.Destroy(this);
			
		}

		private void OnCollision(CollisionData tileCollision)
		{
			//BotsModule.Log("hit");
			if (hasHitSomething)
			{
				
				if (tileCollision.OtherRigidbody && tileCollision.OtherRigidbody.projectile && (((tileCollision.OtherRigidbody.projectile.Owner is PlayerController) && attachedToEnemy)) || !attachedToEnemy)// && !tileCollision.OtherRigidbody.GetComponent<NailHomingModifier>())
                {
					if (this.m_projectile)
					{
						UnityEngine.Object.Destroy(this.m_projectile.gameObject);
					}
				}
			}
			else
            {
				this.m_projectile.baseData.speed *= 0.0001f;
				this.m_projectile.UpdateSpeed();
				this.m_hitNormal = tileCollision.Normal.ToAngle();

				this.m_projectile.collidesWithEnemies = false;
				this.m_projectile.collidesWithPlayer = false;
				this.m_projectile.collidesWithProjectiles = true;
				this.m_projectile.collidesOnlyWithPlayerProjectiles = true;
				
				for (int i = 0; i < this.m_projectile.specRigidbody.PixelColliders.Count; i++)
				{
					this.m_projectile.specRigidbody.PixelColliders[i].CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
				}

				//this.m_projectile.specRigidbody.CollideWithOthers = false;

				PhysicsEngine.PostSliceVelocity = new Vector2?(default(Vector2));
				SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
				magnets.Add(this);
				//specRigidbody.OnCollision -= this.OnCollision;
				hasHitSomething = true;
			}
		}
	}

	public class NailBleedBuff : AppliedEffectBase
	{
		public NailBleedBuff()
		{
			this.CascadeTime = 3f;
			this.m_cachedSourceVector = Vector2.zero;

		}

		private void InitializeSelf(NailBleedBuff source)
		{
			if (!source)
			{
				return;
			}
			this.m_initialized = true;
			this.hh = base.GetComponent<HealthHaver>();
			if (this.hh == null)
			{
				UnityEngine.Object.Destroy(this);
			}
		}


		private void Update()
		{
			if (this.m_initialized)
			{
				this.m_elapsed += BraveTime.DeltaTime;
				if (this.m_elapsed > this.CascadeTime)
				{
					this.DoEffect();
				}
			}
		}



		public override void Initialize(AppliedEffectBase source)
		{
			if (source is NailBleedBuff)
			{
				NailBleedBuff NailBleedBuff = source as NailBleedBuff;
				if (base.GetComponent<NailBleedBuff>() == this && NailBleedBuff.additionalVFX != null && base.GetComponent<SpeculativeRigidbody>())
				{
					SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
					GameObject gameObject = SpawnManager.SpawnVFX(NailBleedBuff.additionalVFX, component.UnitCenter, Quaternion.identity, true);
					gameObject.transform.parent = base.transform;
				}
				this.InitializeSelf(NailBleedBuff);
				if (NailBleedBuff.vfx != null)
				{
					this.instantiatedVFX = SpawnManager.SpawnVFX(NailBleedBuff.vfx, base.transform.position, Quaternion.identity, true);
					tk2dSprite component2 = this.instantiatedVFX.GetComponent<tk2dSprite>();
					tk2dSprite component3 = base.GetComponent<tk2dSprite>();
					if (component2 != null && component3 != null)
					{
						component3.AttachRenderer(component2);
						component2.HeightOffGround = 0.1f;
						component2.IsPerpendicular = true;
						component2.usesOverrideMaterial = true;
					}
					BuffVFXAnimator component4 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
					if (component4 != null)
					{
						Projectile component5 = source.GetComponent<Projectile>();
						if (component5 && component5.LastVelocity != Vector2.zero)
						{
							this.m_cachedSourceVector = component5.LastVelocity;
							component4.InitializePierce(base.GetComponent<GameActor>(), component5.LastVelocity);
						}
						else
						{
							component4.Initialize(base.GetComponent<GameActor>());
						}
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		public override void AddSelfToTarget(GameObject target)
		{
			if (target.GetComponent<HealthHaver>() == null)
			{
				return;
			}
			NailBleedBuff NailBleedBuff = target.AddComponent<NailBleedBuff>();
			NailBleedBuff.Initialize(this);
		}

		private void DoEffect()
		{
			if (this.instantiatedVFX)
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX);
			}
			UnityEngine.Object.Destroy(this);
		}

		public GameObject vfx;
		public GameObject additionalVFX;
		public float CascadeTime;
		private GameObject instantiatedVFX;
		private Gun m_attachedGun;
		private HealthHaver hh;
		private bool m_initialized;
		private float m_elapsed;
		private Vector2 m_cachedSourceVector;
	}
    #endregion
}
