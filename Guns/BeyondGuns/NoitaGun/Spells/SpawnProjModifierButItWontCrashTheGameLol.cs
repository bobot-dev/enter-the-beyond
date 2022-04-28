using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SpawnProjModifierButItWontCrashTheGameLol : MonoBehaviour
	{
			// Token: 0x06008664 RID: 34404 RVA: 0x003687E8 File Offset: 0x003669E8
		public SpawnProjModifierButItWontCrashTheGameLol()
		{
			this.inFlightSpawnCooldown = 1f;
			this.inFlightSpawnAngle = 90f;
			this.numToSpawnInFlight = 2;
			this.numberToSpawnOnCollison = 2;
			this.startAngle = 90;
			this.spawnOnObjectCollisions = true;
			this.SpawnedProjectileScaleModifier = 1f;
			this.spawnAudioEvent = string.Empty;
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06008665 RID: 34405 RVA: 0x00368844 File Offset: 0x00366A44
		private Vector2 SpawnPos
		{
			get
			{
				if (this.m_srb)
				{
					return this.m_srb.UnitCenter;
				}
				if (base.transform)
				{
					return base.transform.position.XY();
				}
				if (this.m_projectile)
				{
					return this.m_projectile.LastPosition;
				}
				return GameManager.Instance.BestActivePlayer.CenterPosition;
			}
		}

		// Token: 0x06008666 RID: 34406 RVA: 0x003688C0 File Offset: 0x00366AC0
		private void Update()
		{
			if (this.p == null)
			{
				this.p = base.GetComponent<Projectile>();
			}
			if (this.m_srb == null)
			{
				this.m_srb = base.GetComponent<SpeculativeRigidbody>();
			}
			if (this.spawnProjectilesInFlight)
			{
				this.elapsed += BraveTime.DeltaTime;
				if (this.elapsed > this.inFlightSpawnCooldown)
				{
					if (this.usesComplexSpawnInFlight)
					{
						this.elapsed -= this.inFlightSpawnCooldown;
						if (this.inFlightAimAtEnemies)
						{
							AIActor y = null;
							RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
							for (int i = 0; i < this.numToSpawnInFlight; i++)
							{
								AIActor randomActiveEnemy = absoluteRoomFromPosition.GetRandomActiveEnemy(false);
								if (!(randomActiveEnemy != null) || !(randomActiveEnemy != y))
								{
									break;
								}
								y = randomActiveEnemy;
								this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : this.projectileToSpawnInFlight, this.SpawnPos.ToVector3ZUp(0f), BraveMathCollege.Atan2Degrees(randomActiveEnemy.CenterPosition - this.SpawnPos), null);
							}
						}
						else
						{
							for (int j = 0; j < this.numToSpawnInFlight; j++)
							{
								float num = this.inFlightSpawnAngle / (float)(this.numToSpawnInFlight - 1) * (float)j - this.inFlightSpawnAngle / 2f;
								if (this.fireRandomlyInAngle)
								{
									num = UnityEngine.Random.value * this.inFlightSpawnAngle - this.inFlightSpawnAngle / 2f;
								}
								this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : this.projectileToSpawnInFlight, this.SpawnPos.ToVector3ZUp(0f), this.p.transform.eulerAngles.z + num, null);
							}
						}
					}
					else if (this.InFlightSourceTransform)
					{
						this.elapsed -= this.inFlightSpawnCooldown;
						this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : this.projectileToSpawnInFlight, this.InFlightSourceTransform.position, this.InFlightSourceTransform.eulerAngles.z, null);
					}
					else
					{
						this.elapsed -= this.inFlightSpawnCooldown;
						this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : this.projectileToSpawnInFlight, this.SpawnPos.ToVector3ZUp(0f), this.p.transform.eulerAngles.z + this.inFlightSpawnAngle, null);
						this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : this.projectileToSpawnInFlight, this.SpawnPos.ToVector3ZUp(0f), this.p.transform.eulerAngles.z - this.inFlightSpawnAngle, null);
					}
					if (!string.IsNullOrEmpty(this.inFlightSpawnAnimation))
					{
						this.p.sprite.spriteAnimator.PlayForDuration(this.inFlightSpawnAnimation, -1f, this.p.sprite.spriteAnimator.CurrentClip.name, true);
					}
					if (!string.IsNullOrEmpty(this.spawnAudioEvent))
					{
						AkSoundEngine.PostEvent(this.spawnAudioEvent, base.gameObject);
					}
				}
			}
		}

		// Token: 0x06008667 RID: 34407 RVA: 0x00368BF4 File Offset: 0x00366DF4
		public void SpawnCollisionProjectiles(Vector2 contact, Vector2 normal, SpeculativeRigidbody collidedRigidbody, bool hitObject = false)
		{
			if (!this || !this.m_srb)
			{
				return;
			}
			SpawnProjModifier.CollisionSpawnStyle collisionSpawnStyle = this.collisionSpawnStyle;
			if (hitObject && this.doOverrideObjectCollisionSpawnStyle)
			{
				collisionSpawnStyle = this.overrideObjectSpawnStyle;
			}
			if (collisionSpawnStyle != SpawnProjModifier.CollisionSpawnStyle.RADIAL)
			{
				if (collisionSpawnStyle != SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST)
				{
					if (collisionSpawnStyle == SpawnProjModifier.CollisionSpawnStyle.REVERSE_FLAK_BURST)
					{
						this.HandleReverseSpawnFlakBurst(contact, normal, collidedRigidbody);
					}
				}
				else
				{
					this.HandleSpawnFlakBurst(contact, normal, collidedRigidbody);
				}
			}
			else
			{
				this.HandleSpawnRadial(contact, normal, collidedRigidbody);
			}
			if (!string.IsNullOrEmpty(this.spawnAudioEvent))
			{
				AkSoundEngine.PostEvent(this.spawnAudioEvent, base.gameObject);
			}
		}

		// Token: 0x06008668 RID: 34408 RVA: 0x00368CA4 File Offset: 0x00366EA4
		private void HandleReverseSpawnFlakBurst(Vector2 contact, Vector2 normal, SpeculativeRigidbody collidedRigidbody)
		{
			int num = UnityEngine.Random.Range(0, 20);
			Vector2 unitBottomLeft = this.m_srb.UnitBottomLeft;
			Vector2 unitTopRight = this.m_srb.UnitTopRight;
			for (int i = 0; i < this.numberToSpawnOnCollison; i++)
			{
				Projectile proj = (!this.UsesMultipleCollisionSpawnProjectiles) ? this.projectileToSpawnOnCollision : this.collisionSpawnProjectiles[UnityEngine.Random.Range(0, this.collisionSpawnProjectiles.Length)];
				float num2 = 15f - BraveMathCollege.GetLowDiscrepancyRandom(i + num) * 30f;
				float num3 = BraveMathCollege.Atan2Degrees(normal) + num2;
				if (this.alignToSurfaceNormal)
				{
					num3 = BraveMathCollege.Atan2Degrees(-1f * normal) + num2;
				}
				Vector2 vector = new Vector2(UnityEngine.Random.Range(unitBottomLeft.x, unitTopRight.x), UnityEngine.Random.Range(unitBottomLeft.y, unitTopRight.y));
				this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : proj, vector.ToVector3ZUp(base.transform.position.z), 180f + num3, collidedRigidbody);
			}
		}

		// Token: 0x06008669 RID: 34409 RVA: 0x00368DB0 File Offset: 0x00366FB0
		private void HandleSpawnFlakBurst(Vector2 contact, Vector2 normal, SpeculativeRigidbody collidedRigidbody)
		{
			int num = UnityEngine.Random.Range(0, 20);
			Vector2 unitBottomLeft = this.m_srb.UnitBottomLeft;
			Vector2 unitTopRight = this.m_srb.UnitTopRight;
			for (int i = 0; i < this.numberToSpawnOnCollison; i++)
			{
				Projectile proj = (!this.UsesMultipleCollisionSpawnProjectiles) ? this.projectileToSpawnOnCollision : this.collisionSpawnProjectiles[UnityEngine.Random.Range(0, this.collisionSpawnProjectiles.Length)];
				float num2 = 15f - BraveMathCollege.GetLowDiscrepancyRandom(i + num) * 30f;
				float zRotation = BraveMathCollege.Atan2Degrees(normal) + num2;
				Vector2 vector = new Vector2(UnityEngine.Random.Range(unitBottomLeft.x, unitTopRight.x), UnityEngine.Random.Range(unitBottomLeft.y, unitTopRight.y));
				this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : proj, vector.ToVector3ZUp(base.transform.position.z), zRotation, collidedRigidbody);
			}
		}

		// Token: 0x0600866A RID: 34410 RVA: 0x00368E98 File Offset: 0x00367098
		private void HandleSpawnRadial(Vector2 contact, Vector2 normal, SpeculativeRigidbody collidedRigidbody)
		{
			float num = 360f / (float)this.numberToSpawnOnCollison;
			for (int i = 0; i < this.numberToSpawnOnCollison; i++)
			{
				Projectile proj = (!this.UsesMultipleCollisionSpawnProjectiles) ? this.projectileToSpawnOnCollision : this.collisionSpawnProjectiles[UnityEngine.Random.Range(0, this.collisionSpawnProjectiles.Length)];
				float num2 = 0.5f;
				if (this.randomRadialStartAngle)
				{
					num2 = (float)UnityEngine.Random.Range(0, 360);
				}
				float zRotation = (!this.alignToSurfaceNormal) ? (this.p.transform.eulerAngles.z + num2 + (float)this.startAngle + num * (float)i) : (Mathf.Atan2(normal.y, normal.x) * 57.29578f + num2 + (float)this.startAngle + num * (float)i);
				this.SpawnProjectile(useSelfAsProj ? base.GetComponent<Projectile>() : proj, (contact + normal * 0.5f).ToVector3ZUp(base.transform.position.z), zRotation, collidedRigidbody);
			}
		}

		// Token: 0x0600866B RID: 34411 RVA: 0x00368FAC File Offset: 0x003671AC
		private void SpawnProjectile(Projectile proj, Vector3 spawnPosition, float zRotation, SpeculativeRigidbody collidedRigidbody = null)
		{
			//ETGModConsole.Log("spawned!!");
			GameObject gameObject = SpawnManager.SpawnProjectile(proj.gameObject, spawnPosition, Quaternion.Euler(0f, 0f, zRotation), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			if (component)
			{
				component.SpawnedFromOtherPlayerProjectile = true;
				if (component is HelixProjectile)
				{
					component.Inverted = (UnityEngine.Random.value < 0.5f);
				}
			}
			if (gameObject.GetComponent<SpawnProjModifierButItWontCrashTheGameLol>())
            {
				UnityEngine.Object.Destroy(gameObject.GetComponent<SpawnProjModifierButItWontCrashTheGameLol>());
            }

			if (!this.m_hasCheckedProjectile)
			{
				this.m_hasCheckedProjectile = true;
				this.m_projectile = base.GetComponent<Projectile>();
			}
			if (this.m_projectile && this.PostprocessSpawnedProjectiles && this.m_projectile.Owner && this.m_projectile.Owner is PlayerController)
			{
				PlayerController playerController = this.m_projectile.Owner as PlayerController;
				playerController.DoPostProcessProjectile(component);
			}
			if (this.SpawnedProjectilesInheritAppearance && component.sprite && this.m_projectile.sprite)
			{
				component.shouldRotate = this.m_projectile.shouldRotate;
				component.shouldFlipHorizontally = this.m_projectile.shouldFlipHorizontally;
				component.shouldFlipVertically = this.m_projectile.shouldFlipVertically;
				component.sprite.SetSprite(this.m_projectile.sprite.Collection, this.m_projectile.sprite.spriteId);
				Vector2 vector = component.transform.position.XY() - component.sprite.WorldCenter;
				component.transform.position += vector.ToVector3ZUp(0f);
				component.specRigidbody.Reinitialize();
			}
			if (this.SpawnedProjectileScaleModifier != 1f)
			{
				component.AdditionalScaleMultiplier *= this.SpawnedProjectileScaleModifier;
			}
			if (this.m_projectile && this.m_projectile.GetCachedBaseDamage > 0f)
			{
				component.baseData.damage = component.baseData.damage * Mathf.Min(this.m_projectile.baseData.damage / this.m_projectile.GetCachedBaseDamage, 1f);
			}
			if (this.p)
			{
				component.Owner = this.p.Owner;
				component.Shooter = this.p.Shooter;
				if (component is RobotechProjectile)
				{
					RobotechProjectile robotechProjectile = component as RobotechProjectile;
					robotechProjectile.initialOverrideTargetPoint = new Vector2?(spawnPosition.XY() + (Quaternion.Euler(0f, 0f, zRotation) * Vector2.right * 10f).XY());
				}
				if (this.SpawnedProjectilesInheritData)
				{
					component.baseData.damage = Mathf.Max(component.baseData.damage, this.p.baseData.damage / (float)this.numberToSpawnOnCollison);
					component.baseData.speed = Mathf.Max(component.baseData.speed, this.p.baseData.speed / ((float)this.numberToSpawnOnCollison / 2f));
					component.baseData.force = Mathf.Max(component.baseData.force, this.p.baseData.force / (float)this.numberToSpawnOnCollison);
				}
			}
			if (component.specRigidbody)
			{
				if (collidedRigidbody)
				{
					component.specRigidbody.RegisterTemporaryCollisionException(collidedRigidbody, 0.25f, new float?(0.5f));
				}
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(component.specRigidbody, null, false);
			}
		}

		// Token: 0x04008B35 RID: 35637
		public bool PostprocessSpawnedProjectiles;

		public bool useSelfAsProj;

		[Header("Spawn in Flight")]
		public bool spawnProjectilesInFlight;

		// Token: 0x04008B37 RID: 35639
		public Projectile projectileToSpawnInFlight;

		public float inFlightSpawnCooldown;

		public float inFlightSpawnAngle;

		// Token: 0x04008B3A RID: 35642
		public Transform InFlightSourceTransform;

		// Token: 0x04008B3B RID: 35643
		public bool usesComplexSpawnInFlight;

		// Token: 0x04008B3C RID: 35644
		[ShowInInspectorIf("usesComplexSpawnInFlight", false)]
		public int numToSpawnInFlight;

		// Token: 0x04008B3D RID: 35645
		[ShowInInspectorIf("usesComplexSpawnInFlight", false)]
		public bool fireRandomlyInAngle;

		// Token: 0x04008B3E RID: 35646
		[ShowInInspectorIf("usesComplexSpawnInFlight", false)]
		public bool inFlightAimAtEnemies;

		// Token: 0x04008B3F RID: 35647
		public string inFlightSpawnAnimation;

		// Token: 0x04008B40 RID: 35648
		[Header("Spawn on Collision")]
		public bool spawnProjectilesOnCollision;

		// Token: 0x04008B41 RID: 35649
		public SpawnProjModifier.CollisionSpawnStyle collisionSpawnStyle;

		// Token: 0x04008B42 RID: 35650
		public bool doOverrideObjectCollisionSpawnStyle;

		// Token: 0x04008B43 RID: 35651
		public SpawnProjModifier.CollisionSpawnStyle overrideObjectSpawnStyle;

		// Token: 0x04008B44 RID: 35652
		public bool spawnCollisionProjectilesOnBounce;

		// Token: 0x04008B45 RID: 35653
		public Projectile projectileToSpawnOnCollision;

		// Token: 0x04008B46 RID: 35654
		public bool UsesMultipleCollisionSpawnProjectiles;

		// Token: 0x04008B47 RID: 35655
		public Projectile[] collisionSpawnProjectiles;

		// Token: 0x04008B48 RID: 35656
		public int numberToSpawnOnCollison;

		// Token: 0x04008B49 RID: 35657
		public int startAngle;

		// Token: 0x04008B4A RID: 35658
		public bool randomRadialStartAngle;

		// Token: 0x04008B4B RID: 35659
		public bool spawnOnObjectCollisions;

		// Token: 0x04008B4C RID: 35660
		public bool alignToSurfaceNormal;

		// Token: 0x04008B4D RID: 35661
		public bool spawnProjecitlesOnDieInAir;

		// Token: 0x04008B4E RID: 35662
		public bool SpawnedProjectilesInheritData;

		// Token: 0x04008B4F RID: 35663
		[NonSerialized]
		public bool SpawnedProjectilesInheritAppearance;

		// Token: 0x04008B50 RID: 35664
		[NonSerialized]
		public float SpawnedProjectileScaleModifier;

		// Token: 0x04008B51 RID: 35665

		public string spawnAudioEvent;

		// Token: 0x04008B52 RID: 35666
		private SpeculativeRigidbody m_srb;

		// Token: 0x04008B53 RID: 35667
		private Projectile p;

		// Token: 0x04008B54 RID: 35668
		private float elapsed;

		// Token: 0x04008B55 RID: 35669
		protected bool m_hasCheckedProjectile;

		// Token: 0x04008B56 RID: 35670
		protected Projectile m_projectile;

		// Token: 0x02001681 RID: 5761
		public enum CollisionSpawnStyle
		{
			// Token: 0x04008B58 RID: 35672
			RADIAL,
			// Token: 0x04008B59 RID: 35673
			FLAK_BURST,
			// Token: 0x04008B5A RID: 35674
			REVERSE_FLAK_BURST
		}
	}
}
