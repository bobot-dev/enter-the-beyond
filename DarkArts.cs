using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class DarkArts : PlayerItem
    {

		public DarkArts()
		{
			this.collisionDamage = 50f;
			this.stunDuration = 1f;
			this.momentaryPause = 0.25f;
			this.finalDelay = 0.5f;
			this.sequentialValidUses = 3;
			this.actorsPassed = new List<AIActor>();
			this.breakablesPassed = new List<MajorBreakable>();
			this.bulletsPassed = new List<Projectile>();
		}

		public static void Init()
		{
			//The name of the item
			string itemName = "Dark Arts";
			string resourceName = "BotsMod/sprites/darkarts";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<DarkArts>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "todo: rename item";
			string longDesc = "";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 3);
			item.consumable = false;
			item.quality = ItemQuality.A;
			item.poofVFX = (PickupObjectDatabase.GetById(462) as ConsumableStealthItem).poofVfx;
			Tools.BeyondItems.Add(item.PickupObjectId);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", base.gameObject);
			if (this.m_isDashing)
			{
				return;
			}
			base.StartCoroutine(this.HandleDash(user));
		}

		private IEnumerator HandleDash(PlayerController user)
		{

			this.m_isDashing = true;

			StatModifier tempSPeedBoost = new StatModifier();
			tempSPeedBoost.amount = 1.4f;
			tempSPeedBoost.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
			tempSPeedBoost.statToBoost = PlayerStats.StatType.MovementSpeed;
			user.ownerlessStatModifiers.Add(tempSPeedBoost);
			user.stats.RecalculateStats(user, false, false);


			if (this.poofVFX != null)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}
			Vector2 startPosition = user.sprite.WorldCenter;
			this.actorsPassed.Clear();
			this.breakablesPassed.Clear();
			//user.IsVisible = false;
			//user.SetInputOverride("darkarts");
			user.healthHaver.IsVulnerable = false;
			user.IsGunLocked = true;
			/*GameObject trailInstance = UnityEngine.Object.Instantiate<GameObject>(this.trailVFXPrefab, user.sprite.WorldCenter.ToVector3ZUp(0f), Quaternion.identity);
			trailInstance.transform.parent = user.transform;
			TrailController trail = trailInstance.GetComponent<TrailController>();
			trail.boneSpawnOffset = user.sprite.WorldCenter - user.specRigidbody.Position.UnitPosition;*/

			PixelCollider playerHitbox = user.specRigidbody.HitboxPixelCollider;
			playerHitbox.CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox) | CollisionMask.LayerToMask(CollisionLayer.Projectile);
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.DarkArtsPreCollision));

			user.SetIsStealthed(true, "darkarts");
			user.ChangeSpecialShaderFlag(1, 1f);
			user.ToggleGunRenderers(false, "darkarts");
			this.renderer.enabled = true;
			this.transform.localPosition = new Vector3(-0.125f, 0.125f, 0f);
			this.transform.localRotation = Quaternion.Euler(0f, 0f, 280f);


			float duration = 3;
			float elapsed = -BraveTime.DeltaTime;

			

			while (elapsed < duration)
			{
				user.healthHaver.IsVulnerable = false;
				elapsed += BraveTime.DeltaTime;
				//float adjSpeed = Mathf.Min(this.dashSpeed, adjDashDistance / BraveTime.DeltaTime);
				//user.specRigidbody.Velocity = dashDirection.normalized * adjSpeed;
				yield return null;
			}

			
			if (this.poofVFX != null)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}


			user.ownerlessStatModifiers.Remove(tempSPeedBoost);
			user.stats.RecalculateStats(user, false, false);

			user.SetIsStealthed(false, "darkarts");
			user.ChangeSpecialShaderFlag(1, 0f);
			user.ToggleGunRenderers(true, "darkarts");

			this.StartCoroutine(this.EndAndDamage(new List<AIActor>(this.actorsPassed), new List<MajorBreakable>(this.breakablesPassed), new List<Projectile>(this.bulletsPassed), user, Vector2.zero, startPosition, user.sprite.WorldCenter));
			this.renderer.enabled = false;
			user.IsGunLocked = false;
			playerHitbox.CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox) | CollisionMask.LayerToMask(CollisionLayer.Projectile);
			SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.DarkArtsPreCollision));
			
			
			this.m_isDashing = false;
			//trail.DisconnectFromSpecRigidbody();
			yield break;
		}

		private IEnumerator EndAndDamage(List<AIActor> actors, List<MajorBreakable> breakables, List<Projectile> projectiles, PlayerController user, Vector2 dashDirection, Vector2 startPosition, Vector2 endPosition)
		{			
			user.healthHaver.IsVulnerable = true;
			for (int i = 0; i < actors.Count; i++)
			{
				if (actors[i])
				{
					actors[i].LocalTimeScale = 1f;
					DisableVFX(actors[i]);
					actors[i].healthHaver.ApplyDamage(this.collisionDamage, dashDirection, "darkarts", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				}
			}
			for (int j = 0; j < breakables.Count; j++)
			{
				if (breakables[j])
				{
					breakables[j].ApplyDamage(100f, dashDirection, false, false, false);
				}
			}
			for (int j = 0; j < projectiles.Count; j++)
			{
				if (projectiles[j])
				{
					projectiles[j].ForceDestruction();
				}
			}
			yield break;
		}
		
		private void DarkArtsPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (otherRigidbody.projectile != null)
			{
				PhysicsEngine.SkipCollision = true;
				if (!this.bulletsPassed.Contains(otherRigidbody.projectile))
				{
					//otherRigidbody.projectile.baseData.speed = 0;

					//otherRigidbody.projectile.UpdateSpeed();
					otherRigidbody.projectile.gameObject.GetOrAddComponent<DarkArtsSlowDown>().overrideTimeScale = 0;
					this.bulletsPassed.Add(otherRigidbody.projectile);
				}
			}
			if (otherRigidbody.aiActor != null)
			{
				PhysicsEngine.SkipCollision = true;
				if (!this.actorsPassed.Contains(otherRigidbody.aiActor))
				{
					//otherRigidbody.aiActor.DelayActions(1f);
					EnableVFX(otherRigidbody.aiActor, new Color(73, 0, 122));
					otherRigidbody.aiActor.LocalTimeScale = 0.01f;
					this.actorsPassed.Add(otherRigidbody.aiActor);
				}
			}
			if (otherRigidbody.majorBreakable != null)
			{
				PhysicsEngine.SkipCollision = true;
				if (!this.breakablesPassed.Contains(otherRigidbody.majorBreakable))
				{
					this.breakablesPassed.Add(otherRigidbody.majorBreakable);
				}
			}
		}

		public void EnableVFX(AIActor target, Color color)
		{

			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", color);
			}


		}
		public void DisableVFX(AIActor target)
		{

			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
			if (outlineMaterial != null)
			{
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			}


		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
		public float collisionDamage;
		public float stunDuration;
		public float momentaryPause;
		public float finalDelay;
		public int sequentialValidUses;
		//public GameObject trailVFXPrefab;
		public GameObject poofVFX;
		private bool m_isDashing;
		private List<AIActor> actorsPassed;
		private List<MajorBreakable> breakablesPassed;
		private List<Projectile> bulletsPassed;
	}

	public class DarkArtsSlowDown : MonoBehaviour
    {
		public int overrideTimeScale = 1;
    }
}
