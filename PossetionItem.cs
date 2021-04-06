using ItemAPI;
using UnityEngine;
using GungeonAPI;
using Dungeonator;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using System.Linq;
using System;
using System.Collections;
using System.Reflection;
using System.IO;
using MultiplayerBasicExample;

namespace BotsMod
{
    class PossetionItem : PlayerItem
	{

		public static void Init()
		{
			//The name of the item
			string itemName = "Possession";
			string resourceName = "BotsMod/sprites/wip";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<PossetionItem>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Possess The Weak";
			string longDesc = "Possess the nearest enemy";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 100);
			item.consumable = false;
			item.quality = ItemQuality.SPECIAL;
			
		}

		private static void PostProcessProjectile(Projectile projectile, float number)
		{
			projectile.OnHitEnemy += OnHit;
		}

		private static void OnHit(Projectile projectile, SpeculativeRigidbody target, bool fatal)
		{
		}

		protected override void DoEffect(PlayerController user)
		{
			
			
			GameManager.Instance.StartCoroutine(HandleDoTheStupidThingImDoingOutOfSpiteDirectedAtNilt(user));
			return;
			if (victum == null)
			{

				
			}
			else if (victum != null)
			{
				victum.healthHaver.ApplyDamage(10000000, Vector2.zero, "stop soft locking", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
			}
		}


		private void OnDeath(Vector2 obj)
		{
			victum.healthHaver.OnDamaged -= OnDamaged;
			victum.healthHaver.OnDeath -= OnDeath;
		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			base.LastOwner.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Host Destroyed", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
		}

		static AIActor victum;



		public override bool CanBeUsed(PlayerController user)
		{

			return true;
			RoomHandler absoluteRoom = user.transform.position.GetAbsoluteRoom();
			List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			var badguy = user.CurrentRoom.GetNearestEnemy(user.CenterPosition, out dumbNumber, false, true);
			if (activeEnemies.Count < 0 && !badguy.IsNormalEnemy)
			{
				return false;
			}

			
		}
		static float dumbNumber = -1;

		private IEnumerator HandleDoTheStupidThingImDoingOutOfSpiteDirectedAtNilt(PlayerController user)//, AIActor enemy)
		{
			RoomHandler absoluteRoom = user.transform.position.GetAbsoluteRoom();
			List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			
			victum = user.CurrentRoom.GetNearestEnemy(user.sprite.WorldCenter, out dumbNumber, false, true);
			
			BotsModule.Log(victum.name, BotsModule.LOST_COLOR);
			//victum = activeEnemies[0];
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i] != victum)
				{
					activeEnemies[i].OverrideTarget = victum.specRigidbody;
					activeEnemies[i].CanTargetEnemies = true;
				}

			}

			//var targetObject = enemy.gameObject;
			BotsMindControlEffect orAddComponent = victum.gameObject.GetOrAddComponent<BotsMindControlEffect>();
			orAddComponent.owner = (user);

			if (victum != null && user.CurrentRoom != null)
			{
				user.ReceivesTouchDamage = false;
				//user.IsVisible = false;
				user.IsEthereal = true;
				user.SetIsFlying(true, "SpiteDirectedAtNilt", true, false);

				victum.HitByEnemyBullets = true;

				victum.healthHaver.ForceSetCurrentHealth(1);
				victum.CanTargetPlayers = false;

				user.IsGunLocked = true;
				//var range = user.CurrentGun.DefaultModule.projectiles[0].baseData.range;
				//user.CurrentGun.DefaultModule.projectiles[0].baseData.range = -1;
				//user.AdditionalCanDodgeRollWhileFlying.Equals(true);
				//bool CmaeraOverridden = (GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition | GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition | GameManager.Instance.MainCameraController.ManualControl);

				//Pixelator.Instance.DoFinalNonFadedLayer = true;
				//Pixelator.Instance.FadeToColor(DarkFadeTime, new Color(0, 0, 0, 0.6f), true, DarkhHoldTime);
				//BraveTime.SetTimeScaleMultiplier(0.15f, glitchBombSpawnObject);
				//if (!CmaeraOverridden)
				//{
				//	GameManager.Instance.MainCameraController.SetManualControl(true, false);
				//	Pixelator.Instance.LerpToLetterbox(0.35f, 1f);
				//}
				//user.transform.position = victum.transform.position;

				//victum.MovementSpeed = user.stats.MovementSpeed;

				victum.IgnoreForRoomClear = true;

				//victum.gameObject.AddComponent<KillOnRoomClear>();

				victum.CanTargetPlayers = false;
				victum.healthHaver.OnDamaged += OnDamaged;
				victum.healthHaver.OnPreDeath += OnDeath;

				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(victum.sprite);
				outlineMaterial.SetColor("_OverrideColor", new Color(84f, 6f, 107f));
				
				while (victum != null)
				{
					//if (enemy.specRigidbody.Position.X != user.specRigidbody.Position.X && enemy.specRigidbody.Position.Y != user.specRigidbody.Position.Y)
					//if (enemy.specRigidbody.Position.m_position != user.specRigidbody.Position.m_position)
					//{


					//victum.specRigidbody.Position = user.specRigidbody.Position;




					

					float num = Vector2.Distance(user.CenterPosition, victum.CenterPosition);
					if (num > 1)
					{
						victum.specRigidbody.Position = new Position(user.CenterPosition);
						BotsModule.Log("Moved " + victum.name, BotsModule.TEXT_COLOR);
					}
						//}


						//targetObject.transform.position = BraveMathCollege.ClampToBounds((targetObject.transform.position += movementDirection), roomBottomLeft + Vector2.one, roomTopRight - Vector2.one);
						//GameManager.Instance.MainCameraController.OverridePosition = targetObject.transform.position;
						yield return null;
				}
				user.ReceivesTouchDamage = true;
				user.IsVisible = true;
				user.IsEthereal = false;
				user.SetIsFlying(false, "SpiteDirectedAtNilt", true, false);
				user.IsGunLocked = false;
				//user.CurrentGun.DefaultModule.projectiles[0].baseData.range = range;
				//user.AdditionalCanDodgeRollWhileFlying.Equals(false);

				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i] != victum)
					{
						activeEnemies[i].OverrideTarget = null;
						activeEnemies[i].CanTargetEnemies = false;
					}

				}

				//if (!CmaeraOverridden)
				//{
				//	GameManager.Instance.MainCameraController.SetManualControl(false, true);
				//	Pixelator.Instance.LerpToLetterbox(0.5f, 0f);
				//}
			}
		}
	}
}
