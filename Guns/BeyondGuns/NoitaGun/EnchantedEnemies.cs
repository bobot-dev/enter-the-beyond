using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class EnchantedEnemies
    {
		public static void Init ()
		{
			ETGMod.AIActor.OnPostStart += MakeChampion;
		}


		private static void MakeChampion(AIActor target)
		{
			float value = UnityEngine.Random.value;
			if (!target.CompanionOwner && !EnchantedEnemies.BannedEnemies.Contains(target.EnemyGuid) && !target.healthHaver.IsBoss && GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.HasGun(BotsItemIds.Wand))
			{
				if ((double)value < (BeyondSettings.Instance.debug ? 0.5 : 0.05))
				{
					DoChampionStuff(target);
					EnchantedEnemies.IsEnchanted.Add(target);

					return;
				}
			}
		}

		private static void DoChampionStuff(AIActor target)
		{
			float value = UnityEngine.Random.value;
			if (target.healthHaver != null)
			{


				Material material = target.sprite.renderer.material;

				var mat = new Material(BeyondPrefabs.AHHH.LoadAsset<Shader>("Chained"));
				mat.SetTexture("_MainTex", material.mainTexture);
				mat.SetTexture("_Gradient", ResourceExtractor.GetTextureFromResource("BotsMod/sprites/gradient.png"));

				target.sprite.renderer.material = mat;
				target.sprite.usesOverrideMaterial = true;
				//material.SetFloat("_PhantomGradientScale", 1);

				target.behaviorSpeculator.CooldownScale *= 0.2f;

				var partObj = UnityEngine.Object.Instantiate(BeyondPrefabs.BotsAssetBundle.LoadAsset<GameObject>("ParticleSystemObj 1"));

				partObj.transform.position = target.sprite.WorldCenter;
				//partObj.transform.position = target.transform.position;
				partObj.transform.parent = target.transform;

				//partObj.transform.localScale /= 2;

				//ParticleSystem partSystem = Tools.BotsAssetBundle.LoadAsset<GameObject>("ParticleSystemObj").GetComponent<ParticleSystem>();

				target.healthHaver.OnPreDeath += delegate (Vector2 obj)
				{
					SpellReward(target.sprite.WorldCenter);
				};

			}

			
		}

		public static void SpellReward(Vector2 worldCenter)
		{
			//LootEngine.SpawnItem(StaticSpellReferences.spellLootTable.SelectByWeight(false), worldCenter, Vector2.up, 1f, true, false, false);
			//GenericLootTable singleItemRewardTable = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable;

			GenericLootTable singleItemRewardTable = StaticSpellReferences.spellLootTable;



			var gameObject2 = LootEngine.SpawnItem(StaticSpellReferences.spellLootTable.SelectByWeight(false), worldCenter, Vector2.up, 1f, true, false, false);
			//var gameObject2 = singleItemRewardTable.GetCompiledCollectionButCooler().SelectByWeightButGood(worldCenter, Vector2.up, 1f, true, false, false);
			IPlayerInteractable[] interfacesInChildren = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(gameObject2.gameObject);
			for (int i = 0; i < interfacesInChildren.Length; i++)
			{
				GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren[i]);
			}
		}


		public static string[] BannedEnemies = new string[]
		{
			"nope"
		};

		public static List<AIActor> IsEnchanted = new List<AIActor>();
	}
}
