using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Coin
    {
        public static GameObject coin;

        public static void Init()
        {
            coin = SpriteBuilder.SpriteFromResource("BotsMod/sprites/TempCoinSprite-export", new GameObject("MarksmanCoin"));

            FakePrefab.MarkAsFakePrefab(coin);

			coin.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(0, 0), new IntVector2(10, 10)).PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;//31

			coin.GetComponent<SpeculativeRigidbody>().AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
			coin.GetComponent<SpeculativeRigidbody>().CollideWithTileMap = true;
			coin.GetComponent<SpeculativeRigidbody>().enabled = true;

			/*DebrisObject coinDebris = new DebrisObject
			{
				Priority = EphemeralObject.EphemeralPriority.Critical,
				audioEventName = "Play_OBJ_ironcoin_flip_01",
				playAnimationOnTrigger = true,
				usesDirectionalFallAnimations = false,
				directionalAnimationData = new DebrisDirectionalAnimationInfo
				{
					fallDown = "",
					fallLeft = "",
					fallRight = "",
					fallUp = "",
				},
				breaksOnFall = true,
				breakOnFallChance = 1,
				groundedCollisionLayer = CollisionLayer.LowObstacle,
				followupBehavior = DebrisObject.DebrisFollowupAction.None,
				followupIdentifier = "",
				collisionStopsBullets = false,
				animatePitFall = false,
				pitFallSplash = true,
				inertialMass = 1,
				canRotate = false,
				angularVelocity = 900,
				angularVelocityVariance = 0,
				bounceCount = 1,
				additionalBounceEnglish = 0,
				decayOnBounce = 0.5f,
				optionalBounceVFX = null,
				shadowSprite = null,
				killTranslationOnBounce = false,
				usesLifespan = false,
				lifespanMax = 1,
				lifespanMin = 1,
				shouldUseSRBMotion = true,
				removeSRBOnGrounded = false,
				placementOptions = new DebrisObject.DebrisPlacementOptions
				{
					canBeFlippedHorizontally = false,
					canBeFlippedVertically = false,
					canBeRotated = false,
				},
				DoesGoopOnRest = false,
				AssignedGoop = null,
				GoopRadius = 1,
				groupManager = null,
				additionalHeightBoost = 0,
				detachedParticleSystems = new List<ParticleSystem>(),
				ForceUpdateIfDisabled = false,


			};

            coin.AddComponent(coinDebris);*/

			var trail2 = coin.gameObject.AddComponent<TrailRenderer>();
            trail2.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail2.receiveShadows = false;
            var mat2 = new Material(Shader.Find("Sprites/Default"));
            mat2.SetColor("_Color", Color.yellow);
            trail2.material = mat2;
            trail2.time = 0.4f;
            trail2.minVertexDistance = 0.1f;
            trail2.startWidth = 0.3f;
            trail2.endWidth = 0f;
            trail2.startColor = Color.white;
            trail2.endColor = new Color(1f, 1f, 1f, 0f);
            trail2.sortingLayerID = LayerMask.NameToLayer("Unpixelated");
			trail2.enabled = true;

           

            SpriteOutlineManager.AddOutlineToSprite(coin.GetComponent<tk2dSprite>(), Color.black);

            coin.AddComponent<CoinController>();

            coin.SetActive(false);

        }
    }
	class CoinController : MonoBehaviour
	{
		void Start()
		{
			this.GetComponent<SpeculativeRigidbody>().specRigidbody.OnPreRigidbodyCollision += PreventBulletCollisions;
			this.GetComponent<SpeculativeRigidbody>().specRigidbody.OnTileCollision += OnTileCollision;
		}
		private void OnTileCollision(CollisionData tileCollision)
        {
			UnityEngine.Object.Destroy(this.gameObject);
		}

		private void PreventBulletCollisions(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (otherRigidbody.projectile && GameManager.Instance.PrimaryPlayer)
			{
				Bounce(otherRigidbody.projectile);
				PhysicsEngine.SkipCollision = true;
				UnityEngine.Object.Destroy(this.gameObject);
			}
		}

		void Update()
        {
			//this.GetComponent<SpeculativeRigidbody>().Velocity -= new Vector2(0, Time.deltaTime * 5);
			//ETGModConsole.Log("" + (this.GetComponent<SpeculativeRigidbody>().Velocity.y));
			timer += Time.deltaTime;

			if (timer >= 1 && !doneDrop)
            {
				doneDrop = true;
				this.GetComponent<SpeculativeRigidbody>().Velocity.y *= -3;
				
			}

			if (timer >= 2)
			{
				timer = 0;
				UnityEngine.Object.Destroy(this.gameObject);

			}
		}
		bool doneDrop;
		public float RotateSpeed = 5f;
		public float Radius = 2f;
		public float timer = 0f;

		public void Bounce(Projectile p)
		{
			if (!p)
			{
				return;
			}
			p.baseData.damage *= 1.5f;

			if (p.GetComponent<Bleed>() != null)
			{
				p.GetComponent<Bleed>().bloodAmount++;

			}




			var rigidbodyCollisionMyRigidbody = this.GetComponent<SpeculativeRigidbody>();

			if (p.IsBulletScript)
			{
				p.RemoveBulletScriptControl();
			}
			Vector2 normal = (p.specRigidbody.UnitCenter - rigidbodyCollisionMyRigidbody.UnitCenter).normalized;
			if (rigidbodyCollisionMyRigidbody)
			{
				Vector2 velocity = rigidbodyCollisionMyRigidbody.Velocity;
				float num7 = (-velocity).ToAngle();
				float num8 = normal.ToAngle();
				float num9 = BraveMathCollege.ClampAngle360(num7 + 2f * (num8 - num7));
				if (p.shouldRotate)
				{
					p.transform.rotation = Quaternion.Euler(0f, 0f, num9);
				}
				p.Direction = BraveMathCollege.DegreesToVector(num9, 1f);
				//p.Speed *= 1.3f;
				p.baseData.range *= 5f;
				if (p.braveBulletScript && p.braveBulletScript.bullet != null)
				{
					p.braveBulletScript.bullet.Direction = num9;
					//p.braveBulletScript.bullet.Speed *= 1.3f;
				}
				Vector2 vector2 = p.Direction * p.Speed * 1;
				vector2 = AdjustBounceVector(p, vector2);
				if (p.shouldRotate && vector2.normalized != p.Direction)
				{
					p.transform.rotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector2.normalized));
				}
				p.Direction = vector2.normalized;
				if (p is HelixProjectile)
				{
					(p as HelixProjectile).AdjustRightVector(Mathf.DeltaAngle(velocity.ToAngle(), num9));
				}
				if (p.OverrideMotionModule != null)
				{
					p.OverrideMotionModule.UpdateDataOnBounce(Mathf.DeltaAngle(velocity.ToAngle(), num9));
				}
				PhysicsEngine.PostSliceVelocity = new Vector2?(vector2);
			}
		}

		public Vector2 AdjustBounceVector(Projectile source, Vector2 inVec)
		{
			Vector2 result = inVec;

			float num6 = 100;

			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(source.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round));
			Vector2 unitCenter = source.specRigidbody.UnitCenter;
			Vector2 w = unitCenter + inVec.normalized * 50f;
			List<AIActor> activeEnemies = absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			List<CoinController> activeCoins = FindObjectsOfType<CoinController>().ToList();
			float num = num6 * num6;
			AIActor aiactor = null;
			CoinController coin = null;
			float num2 = float.MaxValue;


			if (activeCoins != null)
			{
				var CenterPosition = Vector2.zero;

				for (int i = 0; i < activeCoins.Count; i++)
				{
					if (activeCoins[i] && activeCoins[i] != this)
					{
						CenterPosition = activeCoins[i].GetComponent<SpeculativeRigidbody>().UnitCenter;
						Vector2 b = BraveMathCollege.ClosestPointOnLineSegment(CenterPosition, unitCenter, w);
						float num3 = Vector2.SqrMagnitude(CenterPosition - b);
						if (num3 < num && num3 < num2)
						{
							num2 = num3;
							coin = activeCoins[i];
						}
					}
				}
			}
			if (activeEnemies != null && coin == null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i])
					{
						Vector2 b = BraveMathCollege.ClosestPointOnLineSegment(activeEnemies[i].CenterPosition, unitCenter, w);
						float num3 = Vector2.SqrMagnitude(activeEnemies[i].CenterPosition - b);
						if (num3 < num && num3 < num2)
						{
							num2 = num3;
							aiactor = activeEnemies[i];
						}
					}
				}
			}

			if (aiactor != null)
			{
				Vector2 centerPosition = aiactor.CenterPosition;
				result = (centerPosition - unitCenter).normalized * inVec.magnitude;
			}
			else if (coin != null)
			{
				Vector2 centerPosition = coin.GetComponent<SpeculativeRigidbody>().UnitCenter;
				result = (centerPosition - unitCenter).normalized * inVec.magnitude;
			}

			return result;
		}
	}
}
