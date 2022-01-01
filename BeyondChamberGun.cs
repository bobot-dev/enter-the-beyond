using ChamberGunApi;
using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class BeyondChamberGun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Chamber Gun", "beyond_chamber_gun");
			Game.Items.Rename("outdated_gun_mods:beyond_chamber_gun", "bot:beyond_chamber_gun");
			gun.gameObject.AddComponent<BeyondChamberGun>();
			gun.SetShortDescription("");
			gun.SetLongDescription("a gun only for testing");
			GunExt.SetupSprite(gun, null, "beyond_chamber_gun_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.shootAnimation, 12);


			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0.6f;
			//gun.barrelOffset.transform.localPosition = new Vector3(0, -0.3125f, 0f);
			gun.barrelOffset.transform.localPosition = new Vector3(1.0625f, 0.5f, 0f);

			//SpriteBuilder.SpriteFromResource("BotsMod/sprites/Debug/c", gun.barrelOffset.transform.gameObject);

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 10;

			gun.SetBaseMaxAmmo(500);
			gun.gunHandedness = GunHandedness.OneHanded;

			gun.quality = PickupObject.ItemQuality.EXCLUDED;

			ETGMod.Databases.Items.Add(gun, null, "ANY");

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);

			

			List<IntVector2?> offsets = Tools.ConstructListOfSameValues<IntVector2?>(new IntVector2(20, 0), 5);
			List<IntVector2?> colliderSize = Tools.ConstructListOfSameValues<IntVector2?>(new IntVector2(8, 4), 5);

			projectile.shouldRotate = true;

			projectile.baseData.damage = 15;
			//projectile.angularVelocityVariance = 0;

			projectile.AnimateProjectile(new List<string>{ "beyond_chamber_gun_projectile_001", "beyond_chamber_gun_projectile_002", "beyond_chamber_gun_projectile_003", "beyond_chamber_gun_projectile_004", "beyond_chamber_gun_projectile_005" }, 12, tk2dSpriteAnimationClip.WrapMode.LoopSection, 3, 
				new List<IntVector2> { new IntVector2(8, 4), new IntVector2(15, 4), new IntVector2(22, 4), new IntVector2(29, 4), new IntVector2(29, 4) }, Tools.ConstructListOfSameValues(false, 5), Tools.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 5),
				Tools.ConstructListOfSameValues(true, 5), Tools.ConstructListOfSameValues(false, 5), Tools.ConstructListOfSameValues<Vector3?>(null, 7), colliderSize, offsets,
				Tools.ConstructListOfSameValues<Projectile>(null, 7));
			var id = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName("beyond_chamber_gun_projectile_001");
			projectile.baseData.speed = 40;
			projectile.baseData.UsesCustomAccelerationCurve = true;
			projectile.baseData.AccelerationCurve = new AnimationCurve
			{
				postWrapMode = WrapMode.Clamp,
				preWrapMode = WrapMode.Clamp,
				keys = new Keyframe[]
                {
					new Keyframe
                    {
						time = 0f,
						value = 0f,
						inTangent = 0f,
						outTangent = 0f			
					},
					new Keyframe
					{
						time = 1f,
						value = 1f,
						inTangent = 2f,
						outTangent = 2f
					},
				}
			};
			projectile.baseData.CustomAccelerationCurveDuration = 0.3f;
			projectile.baseData.IgnoreAccelCurveTime = 0f;

			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					ChargeTime = 0.4f,
					AmmoCost = 1,
					Projectile = projectile,
				}
			};

			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			gun.DefaultModule.angleVariance = 0;

			MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{


				if (sharedMaterials[i].shader == EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material.shader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			//Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			Material material = new Material(ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteDefinition("beyond_chamber_gun_projectile_001").material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));

			/*material.SetFloat("_Cutoff", 0.5f);
			material.SetFloat("_DepthOffset", 0f);

			material.SetFloat("_EmissiveColorPower", 0.7f);
			material.SetFloat("_EmissivePower", 40);
			material.SetFloat("_HueShift", 0);
			material.SetFloat("_Perpendicular", 0);
			material.SetFloat("_SaturationShift", 0);
			material.SetFloat("_TimeHueShiftFactor", 0f);
			material.SetFloat("_ValueShift", 0f);
			material.SetFloat("_VertexColor", 0f);


			material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			//material.SetColor("_EmissiveColor", new Color(1, 0, 0, 1));*/



			gun.AddAsChamberGunForme("EnterTheBeyond", (int)CustomValidTilesets.BEYOND, new List<int>() { BotsItemIds.BeyondMasteryToken }, 4.5f);




			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

		}
	}
}
