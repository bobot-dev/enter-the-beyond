using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class LeshysCamera : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Leshy's Camera";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<LeshysCamera>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "";
            string longDesc = "";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);

            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void OnPreDrop(PlayerController user)
        {
            base.OnPreDrop(user);
        }

        protected override void DoEffect(PlayerController user)
        {
			GameManager.Instance.StartCoroutine(DoCameraEffect(user));

			base.DoEffect(user);
        }

		IEnumerator DoCameraEffect(PlayerController user)
        {
			var enemies = user.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All);
			if (enemies != null)
            {
				

				Pixelator.Instance.TimedFreezeFrame(0.3f, 0.125f);
				bool CmaeraOverridden = (GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition | GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition | GameManager.Instance.MainCameraController.ManualControl);

				if (!CmaeraOverridden)
				{
					GameManager.Instance.MainCameraController.SetManualControl(true, false);
				}


				float distance;
				//var target = user.CurrentRoom.GetNearestEnemyInDirection(user.sprite.WorldCenter, user.unadjustedAimPoint, 16, out distance, false);
				var target = user.CurrentRoom.GetNearestEnemy(user.sprite.WorldCenter, out distance, false);


				if (target != null)
				{
					StopTime(user, enemies);
					var waitTime = 0.7f;
					GameManager.Instance.MainCameraController.OverridePosition = target.specRigidbody.UnitCenter;//Vector3.Lerp(GameManager.Instance.MainCameraController.transform.position, target.specRigidbody.UnitCenter, waitTime);


					yield return new WaitForSeconds(waitTime);
					GameManager.Instance.MainCameraController.OverrideZoomScale = 2;
					yield return new WaitForSeconds(0.7f);
				}

				//Pixelator.Instance.FadeToColor(0.2f, Color.white, true, 0.1f);
				yield return new WaitForSeconds(3f);
				GameManager.Instance.MainCameraController.SetManualControl(false, false);
				GameManager.Instance.MainCameraController.SetZoomScaleImmediate(1);
				//Tools.GenerateTexture2DFromRenderTexture
			}
			yield return null;
		}

		public override bool CanBeUsed(PlayerController user)
		{

			return user.CurrentRoom != null && user.IsInCombat;
		}

		float effectRadius = 50;


		void StopTime(PlayerController user, List<AIActor> activeEnemies)
        {
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor = activeEnemies[i];
					if (aiactor.IsNormalEnemy)
					{
						float num = Vector2.Distance(user.CenterPosition, aiactor.CenterPosition);
						if (num <= this.effectRadius)
						{
							this.AffectEnemy(aiactor);							
						}
					}
				}
			}
			List<ProjectileTrapController> allProjectileTraps = StaticReferenceManager.AllProjectileTraps;
			for (int j = 0; j < allProjectileTraps.Count; j++)
			{
				ProjectileTrapController projectileTrapController = allProjectileTraps[j];
				if (projectileTrapController && projectileTrapController.isActiveAndEnabled)
				{
					float num2 = Vector2.Distance(user.CenterPosition, projectileTrapController.shootPoint.position);
					if (num2 <= this.effectRadius)
					{
						this.AffectProjectileTrap(projectileTrapController);
					}
				}
			}
			List<ForgeHammerController> allForgeHammers = StaticReferenceManager.AllForgeHammers;
			for (int k = 0; k < allForgeHammers.Count; k++)
			{
				ForgeHammerController forgeHammerController = allForgeHammers[k];
				if (forgeHammerController && forgeHammerController.isActiveAndEnabled)
				{
					float num3 = Vector2.Distance(user.CenterPosition, forgeHammerController.sprite.WorldCenter);
					if (num3 <= this.effectRadius)
					{
						this.AffectForgeHammer(forgeHammerController);
					}
				}
			}
			List<BaseShopController> allShops = StaticReferenceManager.AllShops;
			for (int l = 0; l < allShops.Count; l++)
			{
				BaseShopController baseShopController = allShops[l];
				float num4 = Vector2.Distance(user.CenterPosition, baseShopController.CenterPosition);
				if (num4 <= this.effectRadius)
				{
					this.AffectShop(baseShopController);
				}
			}
			List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
			for (int m = 0; m < allMajorBreakables.Count; m++)
			{
				MajorBreakable majorBreakable = allMajorBreakables[m];
				if (majorBreakable.specRigidbody && majorBreakable.specRigidbody.PrimaryPixelCollider != null)
				{
					float num5 = Vector2.Distance(user.CenterPosition, majorBreakable.specRigidbody.UnitCenter);
					if (num5 <= this.effectRadius)
					{
						this.AffectMajorBreakable(majorBreakable);
						
					}
				}
			}
			this.AffectPlayers(GameManager.Instance.PrimaryPlayer);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
            {
				this.AffectPlayers(GameManager.Instance.SecondaryPlayer);
			}
		}

		void AffectEnemy(AIActor target)
		{
			if (!base.IsCurrentlyActive)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
			}
			target.StartCoroutine(this.ProcessSlow(target));
		}

		void AffectForgeHammer(ForgeHammerController target)
		{
			if (!base.IsCurrentlyActive)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
			}
			target.StartCoroutine(this.ProcessHammerSlow(target));
		}
		void AffectProjectileTrap(ProjectileTrapController target)
		{
			if (!base.IsCurrentlyActive)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
			}
			target.StartCoroutine(this.ProcessTrapSlow(target));
		}
		
		void AffectShop(BaseShopController target)
		{
			if (target && target.shopkeepFSM)
			{
				AIAnimator component = target.shopkeepFSM.GetComponent<AIAnimator>();
				if (!base.IsCurrentlyActive)
				{
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleActive());
				}
				target.StartCoroutine(this.ProcessShopSlow(target, component));
			}
		}
		
		void AffectMajorBreakable(MajorBreakable target)
		{
			if (target.behaviorSpeculator)
			{
				target.StartCoroutine(this.ProcessBehaviorSpeculatorSlow(target.behaviorSpeculator));
			}
		}
		void AffectPlayers(PlayerController target)
		{
			if (target)
			{
				target.StartCoroutine(this.ProcessPlayerSlow(target));
			}
		}

		float InTime = 0;
		float HoldTime = 4.5f;
		float OutTime = 2;
		float MaxTimeModifier = 0;

		private IEnumerator HandleActive()
		{
			this.IsCurrentlyActive = true;
			this.m_activeDuration = this.InTime + this.HoldTime + this.OutTime;
			while (this.m_activeElapsed < this.m_activeDuration)
			{
				this.m_activeElapsed += BraveTime.DeltaTime;
				yield return null;
			}
			this.IsCurrentlyActive = false;
			yield break;
		}

		private IEnumerator ProcessPlayerSlow(PlayerController target)
		{
			float elapsed = 0f;
			if (this.InTime > 0f)
			{
				target.SetInputOverride("camera");
				while (elapsed < this.InTime)
				{
					elapsed += BraveTime.DeltaTime;
					
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.HoldTime > 0f)
			{
				while (elapsed < this.HoldTime)
				{
					elapsed += BraveTime.DeltaTime;					
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					elapsed += BraveTime.DeltaTime;					
					yield return null;
				}
				
			}
			if (target)
			{
				target.ClearInputOverride("camera");
			}
			yield break;
		}

		private IEnumerator ProcessSlow(AIActor target)
		{
			float elapsed = 0f;
			if (this.InTime > 0f)
			{
				while (elapsed < this.InTime)
				{
					if (!target || target.healthHaver.IsDead)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					float t = elapsed / this.InTime;
					target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.HoldTime > 0f)
			{
				while (elapsed < this.HoldTime)
				{
					if (!target || target.healthHaver.IsDead)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = this.MaxTimeModifier;
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					if (!target || target.healthHaver.IsDead)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					float t2 = elapsed / this.OutTime;
					target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
					yield return null;
				}
			}
			if (target)
			{
				target.LocalTimeScale = 1f;
			}
			yield break;
		}

		private IEnumerator ProcessHammerSlow(ForgeHammerController target)
		{
			float elapsed = 0f;
			if (this.InTime > 0f)
			{
				while (elapsed < this.InTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, elapsed / this.InTime);
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.HoldTime > 0f)
			{
				while (elapsed < this.HoldTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = this.MaxTimeModifier;
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
					yield return null;
				}
			}
			if (target)
			{
				target.LocalTimeScale = 1f;
			}
			yield break;
		}

		private IEnumerator ProcessTrapSlow(ProjectileTrapController target)
		{
			float elapsed = 0f;
			if (this.InTime > 0f)
			{
				while (elapsed < this.InTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, elapsed / this.InTime);
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.HoldTime > 0f)
			{
				while (elapsed < this.HoldTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = this.MaxTimeModifier;
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
					yield return null;
				}
			}
			if (target)
			{
				target.LocalTimeScale = 1f;
			}
			yield break;
		}

		private IEnumerator ProcessShopSlow(BaseShopController target, AIAnimator shopkeep)
		{
			float elapsed = 0f;
			if (this.HoldTime + this.InTime > 0f)
			{
				while (elapsed < this.HoldTime + this.InTime)
				{
					elapsed += BraveTime.DeltaTime;
					shopkeep.aiAnimator.FpsScale = this.MaxTimeModifier;
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					elapsed += BraveTime.DeltaTime;
					shopkeep.aiAnimator.FpsScale = Mathf.Lerp(this.MaxTimeModifier, 1f, elapsed / this.OutTime);
					yield return null;
				}
			}
			shopkeep.aiAnimator.FpsScale = 1f;			
			yield break;
		}

		private IEnumerator ProcessBehaviorSpeculatorSlow(BehaviorSpeculator target)
		{
			float elapsed = 0f;
			AIAnimator aiAnimator = (!target) ? null : target.aiAnimator;
			if (this.InTime > 0f)
			{
				while (elapsed < this.InTime)
				{
					if (!target)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					float t = elapsed / this.InTime;
					target.LocalTimeScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
					if (aiAnimator)
					{
						aiAnimator.FpsScale = Mathf.Lerp(1f, this.MaxTimeModifier, t);
					}
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.HoldTime > 0f)
			{
				while (elapsed < this.HoldTime)
				{
					if (!target)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					target.LocalTimeScale = this.MaxTimeModifier;
					if (aiAnimator)
					{
						aiAnimator.FpsScale = this.MaxTimeModifier;
					}
					yield return null;
				}
			}
			elapsed = 0f;
			if (this.OutTime > 0f)
			{
				while (elapsed < this.OutTime)
				{
					if (!target)
					{
						break;
					}
					elapsed += BraveTime.DeltaTime;
					float t2 = elapsed / this.OutTime;
					target.LocalTimeScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
					if (aiAnimator)
					{
						aiAnimator.FpsScale = Mathf.Lerp(this.MaxTimeModifier, 1f, t2);
					}
					yield return null;
				}
			}
			if (aiAnimator)
			{
				aiAnimator.FpsScale = 1f;
			}
			if (target)
			{
				target.LocalTimeScale = 1f;
			}
			yield break;
		}


	}
}

