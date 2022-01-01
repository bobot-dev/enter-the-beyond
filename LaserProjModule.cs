using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class LaserProjModifier : BraveBehaviour
	{
		public LaserProjModifier()
		{
			this.maximumLinkDistance = 8f;
			this.damagePerHit = 5f;
			this.damageCooldown = 1f;
			this.DamagesEnemies = true;
			this.DispersalDensity = 3f;
			this.DispersalMinCoherency = 0.2f;
			this.DispersalMaxCoherency = 1f;
			this.m_damagedEnemies = new HashSet<AIActor>();
		}

		bool whichProj;
		bool hasLink;

		private void Start()
		{
			whichProj = !whichProj;

			PhysicsEngine.Instance.OnPostRigidbodyMovement += this.PostRigidbodyUpdate;
		}

		protected override void OnDestroy()
		{
			if (PhysicsEngine.Instance != null)
			{
				PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.PostRigidbodyUpdate;
			}
			this.ClearLink();
			if (this.BackLinkProjectile && this.BackLinkProjectile.GetComponent<LaserProjModifier>())
			{
				LaserProjModifier component = this.BackLinkProjectile.GetComponent<LaserProjModifier>();
				component.ClearLink();
				component.ForcedLinkProjectile = null;
			}
			base.OnDestroy();
		}

		private void Update()
		{
			this.m_frameLinkProjectile = null;
		}

		private void UpdateLinkToProjectile(Projectile targetProjectile)
		{
			if (this.m_extantLink == null)
			{
				this.m_extantLink = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				int num = -1;
				if (this.DamagesPlayers && !this.m_hasSetBlackBullet)
				{
					this.m_hasSetBlackBullet = true;
					Material material = this.m_extantLink.GetComponent<Renderer>().material;
					material.SetFloat("_BlackBullet", 0.995f);
					material.SetFloat("_EmissiveColorPower", 4.9f);
				}
				else if (!this.DamagesPlayers && PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.TESLA_UNBOUND, out num))
				{
					Material material2 = this.m_extantLink.GetComponent<Renderer>().material;
					material2.SetFloat("_BlackBullet", 0.15f);
					material2.SetFloat("_EmissiveColorPower", 0.1f);
				}
			}
			
			this.m_frameLinkProjectile = targetProjectile;
			Vector2 unitCenter = base.projectile.specRigidbody.UnitCenter;
			Vector2 unitCenter2 = targetProjectile.specRigidbody.UnitCenter;
			this.m_extantLink.transform.position = unitCenter;
			Vector2 vector = unitCenter2 - unitCenter;
			float z = BraveMathCollege.Atan2Degrees(vector.normalized);
			int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			this.m_extantLink.dimensions = new Vector2((float)num2, this.m_extantLink.dimensions.y);
			this.m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, z);
			this.m_extantLink.UpdateZDepth();
			bool flag = this.ApplyLinearDamage(unitCenter, unitCenter2);
			if (flag && this.UsesDispersalParticles)
			{
				this.DoDispersalParticles(unitCenter2, unitCenter);
			}
		}

		private void DoDispersalParticles(Vector2 posStart, Vector2 posEnd)
		{
			if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
			{
				if (!this.m_dispersalParticles)
				{
					this.m_dispersalParticles = GlobalDispersalParticleManager.GetSystemForPrefab(this.DispersalParticleSystemPrefab);
				}
				int num = Mathf.Max(Mathf.CeilToInt(Vector2.Distance(posStart, posEnd) * this.DispersalDensity), 1);
				for (int i = 0; i < num; i++)
				{
					float t = (float)i / (float)num;
					Vector3 vector = Vector3.Lerp(posStart, posEnd, t);
					vector += Vector3.back;
					float num2 = Mathf.PerlinNoise(vector.x / 3f, vector.y / 3f);
					Vector3 a = Quaternion.Euler(0f, 0f, num2 * 360f) * Vector3.right;
					Vector3 a2 = Vector3.Lerp(a, UnityEngine.Random.insideUnitSphere, UnityEngine.Random.Range(this.DispersalMinCoherency, this.DispersalMaxCoherency));
					ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
					{
						position = vector,
						velocity = a2 * this.m_dispersalParticles.startSpeed,
						startSize = this.m_dispersalParticles.startSize,
						startLifetime = this.m_dispersalParticles.startLifetime,
						startColor = this.m_dispersalParticles.startColor
					};
					this.m_dispersalParticles.Emit(emitParams, 1);
				}
			}
		}
		private IEnumerator HandleDamageCooldown(AIActor damagedTarget)
		{
			this.m_damagedEnemies.Add(damagedTarget);
			yield return new WaitForSeconds(this.damageCooldown);
			this.m_damagedEnemies.Remove(damagedTarget);
			yield break;
		}

		private bool ApplyLinearDamage(Vector2 p1, Vector2 p2)
		{
			bool result = false;
			if (this.DamagesEnemies)
			{
				for (int i = 0; i < StaticReferenceManager.AllEnemies.Count; i++)
				{
					AIActor aiactor = StaticReferenceManager.AllEnemies[i];
					if (!this.m_damagedEnemies.Contains(aiactor))
					{
						if (aiactor && aiactor.HasBeenEngaged && aiactor.IsNormalEnemy && aiactor.specRigidbody)
						{
							Vector2 zero = Vector2.zero;
							bool flag = BraveUtility.LineIntersectsAABB(p1, p2, aiactor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, aiactor.specRigidbody.HitboxPixelCollider.UnitDimensions, out zero);
							if (flag)
							{
								aiactor.healthHaver.ApplyDamage(this.damagePerHit, Vector2.zero, "Chain Lightning", this.damageTypes, DamageCategory.Normal, false, null, false);
								result = true;
								GameManager.Instance.StartCoroutine(this.HandleDamageCooldown(aiactor));
							}
						}
					}
				}
			}
			if (this.DamagesPlayers)
			{
				for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[j];
					if (playerController && !playerController.IsGhost && playerController.healthHaver && playerController.healthHaver.IsAlive && playerController.healthHaver.IsVulnerable)
					{
						Vector2 zero2 = Vector2.zero;
						bool flag2 = BraveUtility.LineIntersectsAABB(p1, p2, playerController.specRigidbody.HitboxPixelCollider.UnitBottomLeft, playerController.specRigidbody.HitboxPixelCollider.UnitDimensions, out zero2);
						if (flag2)
						{
							playerController.healthHaver.ApplyDamage(0.5f, Vector2.zero, base.projectile.OwnerName, this.damageTypes, DamageCategory.Normal, false, null, false);
							result = true;
						}
					}
				}
			}
			return result;
		}

		private void ClearLink()
		{
			if (this.m_extantLink != null)
			{
				SpawnManager.Despawn(this.m_extantLink.gameObject);
				this.m_extantLink = null;
			}
		}

		private Projectile GetLinkProjectile()
		{
			if (whichProj)
            {

				return null;
			}
			Projectile projectile = null;
			float num = float.MaxValue;
			float num2 = this.maximumLinkDistance * this.maximumLinkDistance;
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile2 = StaticReferenceManager.AllProjectiles[i];
				if (projectile2 && projectile2 != base.projectile && projectile2.GetComponent<LaserProjModifier>() && !projectile2.GetComponent<LaserProjModifier>().hasLink)
				{					
					LaserProjModifier component = projectile2.GetComponent<LaserProjModifier>();
					if (component && component.m_frameLinkProjectile == null)
					{
						float sqrMagnitude = (component.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter).sqrMagnitude;
						if (sqrMagnitude < num && sqrMagnitude < num2)
						{							
							projectile = projectile2;
							num = sqrMagnitude;
						}
					}					
				}				
			}
			if (projectile == null)
			{
				return null;
			}
			projectile.GetComponent<LaserProjModifier>().hasLink = true;
			return projectile;
		}

		private void PostRigidbodyUpdate()
		{
			if (base.projectile)
			{
				ForcedLinkProjectile = this.GetLinkProjectile();
				Projectile projectile = (!this.UseForcedLinkProjectile) ? this.GetLinkProjectile() : this.ForcedLinkProjectile;
				if (projectile)
				{
					this.UpdateLinkToProjectile(projectile);
				}
				else
				{
					this.ClearLink();
				}
			}
			else
			{
				this.ClearLink();
			}
		}

		public GameObject LinkVFXPrefab;

		public CoreDamageTypes damageTypes;

		public bool RequiresSameProjectileClass;

		public float maximumLinkDistance;

		public float damagePerHit;

		public float damageCooldown;

		[NonSerialized]
		public bool UseForcedLinkProjectile;

		[NonSerialized]
		public Projectile ForcedLinkProjectile;

		[NonSerialized]
		public Projectile BackLinkProjectile;

		[NonSerialized]
		public bool DamagesPlayers;

		[NonSerialized]
		public bool DamagesEnemies;

		[Header("Dispersal")]
		public bool UsesDispersalParticles;

		[ShowInInspectorIf("UsesDispersalParticles", false)]
		public float DispersalDensity;

		[ShowInInspectorIf("UsesDispersalParticles", false)]
		public float DispersalMinCoherency;

		[ShowInInspectorIf("UsesDispersalParticles", false)]
		public float DispersalMaxCoherency;

		[ShowInInspectorIf("UsesDispersalParticles", false)]
		public GameObject DispersalParticleSystemPrefab;

		private Projectile m_frameLinkProjectile;

		private tk2dTiledSprite m_extantLink;

		private bool m_hasSetBlackBullet;

		private ParticleSystem m_dispersalParticles;

		private HashSet<AIActor> m_damagedEnemies;
    }

	public class St4keProj : MonoBehaviour
	{
		public St4keProj()
		{
			DamagePerHit = 7;
			IsElectricitySource = false;
		}
		public GameObject LinkVFXPrefab;

		public static Projectile lastFiredSt4keBullet = null;
		private void Start()
		{
			this.m_projectile = base.GetComponent<Projectile>();
			if (this.m_projectile.Owner is PlayerController)
			{
				this.projOwner = this.m_projectile.Owner as PlayerController;
			}
			SpeculativeRigidbody specRigidBody = this.m_projectile.specRigidbody;
			specRigidBody.CollideWithTileMap = false;

			if (lastFiredSt4keBullet == null || lastFiredSt4keBullet.gameObject == null || !lastFiredSt4keBullet.isActiveAndEnabled)
			{
				lastFiredSt4keBullet = m_projectile;
			}
			else
			{
				if (!lastFiredSt4keBullet.GetComponent<St4keProj>().IsElectricitySource)
				{
					IsElectricitySource = true;
					electricTarget = lastFiredSt4keBullet;
				}
				lastFiredSt4keBullet = m_projectile;
			}
		}
		private void Update()
		{
			if (m_projectile && electricTarget && this.extantLink == null)
			{
				tk2dTiledSprite component = SpawnManager.SpawnVFX(LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();

				component.usesOverrideMaterial = true;
				component.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
				//component.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");

				component.renderer.material.SetFloat("_EmissivePower", 150);
				component.renderer.material.SetFloat("_EmissiveColorPower", 1.55f);
				Color laser = new Color32(92, 8, 140, 255);
				component.renderer.material.SetColor("_OverrideColor", laser);
				component.renderer.material.SetColor("_EmissiveColor", laser);

				this.extantLink = component;
			}
			else if (m_projectile && electricTarget && this.extantLink != null)
			{
				UpdateLink(electricTarget, this.extantLink);
			}
			else if (extantLink != null)
			{
				SpawnManager.Despawn(extantLink.gameObject);
				extantLink = null;
			}
		}
		private void UpdateLink(Projectile target, tk2dTiledSprite m_extantLink)
		{
			Vector2 unitCenter = m_projectile.specRigidbody.UnitCenter;
			Vector2 unitCenter2 = target.specRigidbody.HitboxPixelCollider.UnitCenter;
			m_extantLink.transform.position = unitCenter;
			Vector2 vector = unitCenter2 - unitCenter;
			float num = BraveMathCollege.Atan2Degrees(vector.normalized);
			int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			//m_extantLink.dimensions = new Vector2((float)num2, m_extantLink.dimensions.y);
			m_extantLink.dimensions = new Vector2((float)num2, 2);
			m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, num);
			m_extantLink.UpdateZDepth();
			this.ApplyLinearDamage(unitCenter, unitCenter2);
		}
		private void ApplyLinearDamage(Vector2 p1, Vector2 p2)
		{
			float num = this.DamagePerHit;
			num *= projOwner.stats.GetStatValue(PlayerStats.StatType.Damage);
			for (int i = 0; i < StaticReferenceManager.AllEnemies.Count; i++)
			{
				AIActor aiactor = StaticReferenceManager.AllEnemies[i];
				if (!this.m_damagedEnemies.Contains(aiactor))
				{
					if (aiactor && aiactor.HasBeenEngaged && aiactor.IsNormalEnemy && aiactor.specRigidbody)
					{
						Vector2 zero = Vector2.zero;
						if (BraveUtility.LineIntersectsAABB(p1, p2, aiactor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, aiactor.specRigidbody.HitboxPixelCollider.UnitDimensions, out zero))
						{
							aiactor.healthHaver.ApplyDamage(num, Vector2.zero, "Chain Lightning", CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
							GameManager.Instance.StartCoroutine(this.HandleDamageCooldown(aiactor));
						}
					}
				}
			}
		}
		private void OnDestroy()
		{
			if (extantLink)
			{
				SpawnManager.Despawn(extantLink.gameObject);
			}
		}
		private IEnumerator HandleDamageCooldown(AIActor damagedTarget)
		{
			this.m_damagedEnemies.Add(damagedTarget);
			yield return new WaitForSeconds(0.05f);
			this.m_damagedEnemies.Remove(damagedTarget);
			yield break;
		}
		public float DamagePerHit;
		private tk2dTiledSprite extantLink;
		private HashSet<AIActor> m_damagedEnemies = new HashSet<AIActor>();
		

		private Projectile electricTarget;
		public bool IsElectricitySource;
		private Projectile m_projectile;
		private float m_hitNormal;
		private PlayerController projOwner;
	}

}
