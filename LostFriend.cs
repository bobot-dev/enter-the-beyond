using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using FrostAndGunfireItems;
using System.Collections;

namespace BotsMod
{
	public class LostFriend : CompanionItem
	{
		public static GameObject prefab;
		private static readonly string guid = "bot:lost_friend"; //give your chanceKinBehavioar some unique guid
		float damageBuff = -1;

		public static void Init()
		{
			string itemName = "IDFK what to call this thing";
			string resourceName = "BotsMod/sprites/wip";

			GameObject obj = new GameObject();
			var item = obj.AddComponent<LostFriend>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "A Lost Friend";
			string longDesc = "no";

			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.CompanionGuid = guid; //this will be used by the item later to pull your chanceKinBehavioar from the enemy database
			item.Synergies = new CompanionTransformSynergy[0]; //this just needs to not be null
			//item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 5f);
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, -0.05f, StatModifier.ModifyMethod.ADDITIVE);
			item.CompanionGuid = LostFriend.guid;
			item.CanBeDropped = false;
			BuildPrefab();
			item.PlaceItemInAmmonomiconAfterItemById(664);

		}

		public static void BuildPrefab()
		{
			if (prefab != null || CompanionBuilder.companionDictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			//prefab = CompanionBuilder.BuildPrefab("mistake", guid, "BotsMod/sprites/Pets/mistake/Idle_front/mistake_pet_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));
			prefab = CompanionBuilder.BuildPrefab("lostfriend", guid, "BotsMod/sprites/lostfriend", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a chanceKinBehavioar component to the prefab (could be a custom class)
			//var chanceKinBehavioar = prefab.AddComponent<LostFriendBehavior>();
			LostFriend.LostFriendBehavior chanceKinBehavioar = prefab.AddComponent<LostFriend.LostFriendBehavior>();
			chanceKinBehavioar.aiActor.MovementSpeed = 5f;

			chanceKinBehavioar.CanBePet = true;

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			//prefab.AddAnimation("idle_right", "ItemAPI/Resources/BigSlime/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);
			prefab.AddAnimation("idle", "BotsMod/sprites/lostfriend", fps: 5, AnimationType.Idle, DirectionType.None);
			prefab.AddAnimation("pet", "BotsMod/sprites/lostfriend_peting", fps: 5, AnimationType.Idle, DirectionType.None);

			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that

			var bs = prefab.GetComponent<BehaviorSpeculator>();
			bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior() { IdleAnimations = new string[] { "idle" } });

			bs.AttackBehaviors.Add(new LostFriend.TableAttackBehavior());
			bs.MovementBehaviors.Add(new LostFriend.ApproachEnemiesBehavior());
			//bs.MovementBehaviors.Add(new CompanionFollowPlayerBehavior());


			chanceKinBehavioar.CanInterceptBullets = false;
			//chanceKinBehavioar.CanInterceptBullets = true;
			chanceKinBehavioar.aiActor.healthHaver.PreventAllDamage = true;
			chanceKinBehavioar.aiActor.specRigidbody.CollideWithOthers = true;
			chanceKinBehavioar.aiActor.specRigidbody.CollideWithTileMap = false;
			chanceKinBehavioar.aiActor.healthHaver.ForceSetCurrentHealth(1f);
			chanceKinBehavioar.aiActor.healthHaver.SetHealthMaximum(1f, null, false);
			chanceKinBehavioar.aiActor.specRigidbody.PixelColliders.Clear();
			chanceKinBehavioar.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				//CollisionLayer = CollisionLayer.PlayerCollider,
				CollisionLayer = CollisionLayer.EnemyBulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 12,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});
			/*
			chanceKinBehavioar.aiAnimator.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.BulletBlocker,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 16,
				ManualHeight = 16,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});
			*/
		}
		protected override void Update()
		{
			base.Update();
		}
		public override void Pickup(PlayerController player)
		{
			//this.CreateNewCompanion(player);
			base.Pickup(player);
		}



		public class LostFriendBehavior : CompanionController
		{
			public void Start()
			{
				Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
				mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
				mat.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
				mat.SetFloat("_EmissiveColorPower", 1.55f);
				mat.SetFloat("_EmissivePower", 50);
				aiActor.sprite.renderer.material = mat;
				this.Owner = this.m_owner;
			}
			public PlayerController Owner;
		}

		public class ApproachEnemiesBehavior : MovementBehaviorBase
		{
			// Token: 0x06000445 RID: 1093 RVA: 0x000215DF File Offset: 0x0001F7DF
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
			}

			// Token: 0x06000446 RID: 1094 RVA: 0x00027263 File Offset: 0x00025463
			public override void Upkeep()
			{
				base.Upkeep();
				base.DecrementTimer(ref this.repathTimer, false);
			}

			// Token: 0x06000447 RID: 1095 RVA: 0x0002727C File Offset: 0x0002547C
			public override BehaviorResult Update()
			{
				SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
				bool flag = this.repathTimer > 0f;
				bool flag2 = flag;
				BehaviorResult result;
				if (flag2)
				{
					result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
				}
				else
				{
					bool flag3 = overrideTarget == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.PickNewTarget();
						result = BehaviorResult.Continue;
					}
					else
					{
						this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
						bool flag5 = overrideTarget != null && !this.isInRange;
						bool flag6 = flag5;
						if (flag6)
						{
							this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
							this.repathTimer = this.PathInterval;
							result = BehaviorResult.SkipRemainingClassBehaviors;
						}
						else
						{
							bool flag7 = overrideTarget != null && this.repathTimer >= 0f;
							bool flag8 = flag7;
							if (flag8)
							{
								this.m_aiActor.ClearPath();
								this.repathTimer = -1f;
							}
							result = BehaviorResult.Continue;
						}
					}
				}
				return result;
			}

			// Token: 0x06000448 RID: 1096 RVA: 0x000273B4 File Offset: 0x000255B4
			private void PickNewTarget()
			{
				bool flag = this.m_aiActor == null;
				bool flag2 = !flag;
				if (flag2)
				{
					bool flag3 = this.Owner == null;
					bool flag4 = flag3;
					if (flag4)
					{
						this.Owner = this.m_aiActor.GetComponent<LostFriend.LostFriendBehavior>().Owner;
					}
					this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.roomEnemies);
					for (int i = 0; i < this.roomEnemies.Count; i++)
					{
						AIActor aiactor = this.roomEnemies[i];
						bool flag5 = aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc";
						bool flag6 = flag5;
						if (flag6)
						{
							this.roomEnemies.Remove(aiactor);
						}
					}
					bool flag7 = this.roomEnemies.Count == 0;
					bool flag8 = flag7;
					if (flag8)
					{
						this.m_aiActor.OverrideTarget = null;
					}
					else
					{
						AIActor aiActor = this.m_aiActor;
						AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
						aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
					}
				}
			}

			// Token: 0x04000251 RID: 593
			public float PathInterval = 0.25f;

			// Token: 0x04000252 RID: 594
			public float DesiredDistance = 3f;

			// Token: 0x04000253 RID: 595
			private float repathTimer;

			// Token: 0x04000254 RID: 596
			private List<AIActor> roomEnemies = new List<AIActor>();

			// Token: 0x04000255 RID: 597
			private bool isInRange;

			// Token: 0x04000256 RID: 598
			private PlayerController Owner;
		}
	

		public class TableAttackBehavior : AttackBehaviorBase
		{
			// Token: 0x0600043A RID: 1082 RVA: 0x00020F4B File Offset: 0x0001F14B
			public override void Destroy()
			{
				base.Destroy();
			}

			// Token: 0x0600043B RID: 1083 RVA: 0x00026BD2 File Offset: 0x00024DD2
			public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
			{
				base.Init(gameObject, aiActor, aiShooter);
				this.Owner = this.m_aiActor.GetComponent<LostFriend.LostFriendBehavior>().Owner;
			}

			// Token: 0x0600043C RID: 1084 RVA: 0x00026BF8 File Offset: 0x00024DF8
			public override BehaviorResult Update()
			{
				bool flag = this.attackTimer > 0f && this.isAttacking;
				bool flag2 = flag;
				if (flag2)
				{
					base.DecrementTimer(ref this.attackTimer, false);
				}
				else
				{
					bool flag3 = this.attackCooldownTimer > 0f && !this.isAttacking;
					bool flag4 = flag3;
					if (flag4)
					{
						base.DecrementTimer(ref this.attackCooldownTimer, false);
					}
				}
				bool flag5 = this.IsReady();
				bool flag6 = (!flag5 || this.attackCooldownTimer > 0f || this.attackTimer == 0f || this.m_aiActor.TargetRigidbody == null) && this.isAttacking;
				bool flag7 = flag6;
				BehaviorResult result;
				if (flag7)
				{
					this.StopAttacking();
					result = BehaviorResult.Continue;
				}
				else
				{
					bool flag8 = flag5 && this.attackCooldownTimer == 0f && !this.isAttacking;
					bool flag9 = flag8;
					if (flag9)
					{
						this.attackTimer = this.attackDuration;
						this.m_aiAnimator.PlayUntilFinished(this.attackAnimation, false, null, -1f, false);
						AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", this.m_aiActor.gameObject);
						this.isAttacking = true;
					}
					bool flag10 = this.attackTimer > 0f && flag5;
					bool flag11 = flag10;
					if (flag11)
					{
						this.Attack();
						result = BehaviorResult.SkipAllRemainingBehaviors;
					}
					else
					{
						result = BehaviorResult.Continue;
					}
				}
				return result;
			}

			// Token: 0x0600043D RID: 1085 RVA: 0x00026D61 File Offset: 0x00024F61
			private void StopAttacking()
			{
				this.isAttacking = false;
				this.attackTimer = 0f;
				this.attackCooldownTimer = this.attackCooldown;
			}

			// Token: 0x0600043E RID: 1086 RVA: 0x00026D84 File Offset: 0x00024F84
			public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
			{
				AIActor aiactor = null;
				nearestDistance = float.MaxValue;
				bool flag = activeEnemies == null;
				bool flag2 = flag;
				bool flag3 = flag2;
				bool flag4 = flag3;
				AIActor result;
				if (flag4)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						bool flag5 = aiactor2.healthHaver && aiactor2.healthHaver.IsVulnerable;
						bool flag6 = flag5;
						bool flag7 = flag6;
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !aiactor2.healthHaver.IsDead;
							bool flag10 = flag9;
							bool flag11 = flag10;
							bool flag12 = flag11;
							if (flag12)
							{
								bool flag13 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
								bool flag14 = flag13;
								bool flag15 = flag14;
								bool flag16 = flag15;
								if (flag16)
								{
									float num = Vector2.Distance(position, aiactor2.CenterPosition);
									bool flag17 = num < nearestDistance;
									bool flag18 = flag17;
									bool flag19 = flag18;
									bool flag20 = flag19;
									if (flag20)
									{
										nearestDistance = num;
										aiactor = aiactor2;
									}
								}
							}
						}
					}
					result = aiactor;
				}
				return result;
			}

			// Token: 0x0600043F RID: 1087 RVA: 0x00026EA4 File Offset: 0x000250A4
			private void Attack()
			{
				bool flag = this.Owner == null;
				bool flag2 = flag;
				if (flag2)
				{
					this.Owner = this.m_aiActor.GetComponent<LostFriend.LostFriendBehavior>().Owner;
				}
				float num = -1f;
				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag3 = activeEnemies == null | activeEnemies.Count <= 0;
				bool flag4 = !flag3;
				if (flag4)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag5 = nearestEnemy && num < 10f;
					bool flag6 = flag5;
					if (flag6)
					{
						bool flag7 = this.IsInRange(nearestEnemy);
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							bool flag10 = flag9;
							if (flag10)
							{
								Vector2 worldBottomLeft = this.m_aiActor.specRigidbody.sprite.WorldBottomLeft;
								Vector2 unitCenter = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter - worldBottomLeft).normalized);
								//Projectile projectile = ((Gun)PickupObjectDatabase.GetById(100)).DefaultModule.projectiles[0];
								Projectile currentProjectile = ((Gun)PickupObjectDatabase.GetById(100)).DefaultModule.GetCurrentProjectile();
								this.Owner.StartCoroutine(this.HandleFireShortBeam(currentProjectile, this.Owner, 10));
								//GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_aiActor.sprite.WorldBottomLeft, Quaternion.Euler(0.5f, 0f, z), true);
								//Projectile component = gameObject.GetComponent<Projectile>();
							}
						}
					}
				}
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x000270D0 File Offset: 0x000252D0
			public override float GetMaxRange()
			{
				return 7f;
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x000270E8 File Offset: 0x000252E8
			public override float GetMinReadyRange()
			{
				return 7f;
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x00027100 File Offset: 0x00025300
			public override bool IsReady()
			{
				AIActor aiActor = this.m_aiActor;
				bool flag = aiActor == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody targetRigidbody = aiActor.TargetRigidbody;
					Vector2? vector = (targetRigidbody != null) ? new Vector2?(targetRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_aiActor.TargetRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			// Token: 0x06000443 RID: 1091 RVA: 0x000271A0 File Offset: 0x000253A0
			public bool IsInRange(AIActor enemy)
			{
				bool flag = enemy == null;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					SpeculativeRigidbody specRigidbody = enemy.specRigidbody;
					Vector2? vector = (specRigidbody != null) ? new Vector2?(specRigidbody.UnitCenter) : null;
					flag2 = (vector == null);
				}
				return !flag2 && Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, enemy.specRigidbody.UnitCenter) <= this.GetMinReadyRange();
			}

			private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration)
			{

				float num = -1f;
				List<AIActor> activeEnemies = this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag3 = activeEnemies == null | activeEnemies.Count <= 0;
				bool flag4 = !flag3;
				if (flag4)
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, this.m_aiActor.sprite.WorldCenter, out num, null);
					bool flag5 = nearestEnemy && num < 10f;
					bool flag6 = flag5;
					if (flag6)
					{
						bool flag7 = this.IsInRange(nearestEnemy);
						bool flag8 = flag7;
						if (flag8)
						{
							bool flag9 = !nearestEnemy.IsHarmlessEnemy && nearestEnemy.IsNormalEnemy && !nearestEnemy.healthHaver.IsDead && nearestEnemy != this.m_aiActor;
							bool flag10 = flag9;
							if (flag10)
							{
								Vector2 worldBottomLeft = this.m_aiActor.specRigidbody.sprite.WorldBottomLeft;
								Vector2 unitCenter = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
								float z = BraveMathCollege.Atan2Degrees((unitCenter - worldBottomLeft).normalized);
								float elapsed = 0f;
								BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, z, this.m_aiActor.transform.position + new Vector3(0,0,0));
								yield return null;
								while (elapsed < duration)
								{
									elapsed += BraveTime.DeltaTime;
									this.ContinueFiringBeam(beam, source, z, this.m_aiActor.transform.position);
									yield return null;
								}
								this.CeaseBeam(beam);
								yield break;
							}
						}
					}
				}


			}

			private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
			{
				Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
				GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = source;
				BeamController component2 = gameObject.GetComponent<BeamController>();
				component2.Owner = source;
				component2.HitsPlayers = false;
				component2.HitsEnemies = true;
				Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
				component2.Direction = v;
				component2.Origin = vector;
				return component2;
			}

			private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint)
			{
				Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
				beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
				beam.Origin = vector;
				beam.LateUpdatePosition(vector);
			}

			private void CeaseBeam(BeamController beam)
			{
				beam.CeaseAttack();
			}

			// Token: 0x04000249 RID: 585
			public string attackAnimation = "idle";

			// Token: 0x0400024A RID: 586
			private bool isAttacking;

			// Token: 0x0400024B RID: 587
			private float attackCooldown = 2.5f;

			// Token: 0x0400024C RID: 588
			private float attackDuration = 0.01f;

			// Token: 0x0400024D RID: 589
			private float attackTimer;

			// Token: 0x0400024E RID: 590
			private float attackCooldownTimer;

			// Token: 0x0400024F RID: 591
			private PlayerController Owner;

			// Token: 0x04000250 RID: 592
			private List<AIActor> roomEnemies = new List<AIActor>();
		}
	}
}







