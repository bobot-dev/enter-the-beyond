using CustomCharacters;
using Gungeon;
using ItemAPI;
using PrefabAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class BeyondRailgun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Beyond Railgun", "beyond_railgun");
			Game.Items.Rename("outdated_gun_mods:beyond_railgun", "bot:beyond_railgun");
			gun.gameObject.AddComponent<BeyondRailgun>();
			gun.SetShortDescription("My Point Is...");
			gun.SetLongDescription("This gun fires a high speed metal spike with an explosive charge inside, the explosion it creates weakens the defence of whatever is caught in its blast radius.\n\nA powerful railgun designed to shoot a metal spike loaded with a powerful explosive stolen from some a reality of chaotic space combat.");
			GunExt.SetupSprite(gun, null, "beyond_railgun_idle_001", 8);


			tk2dSpriteAnimationClip animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_railgun_reload");
			float[] offsetsX = new float[] { -0.8125f, -0.8750f, -0.9375f, -0.8125f, -0.7500f, -0.7500f, -0.6875f, -0.6875f };
			float[] offsetsY = new float[] { -0.5000f, -0.5000f, -0.5000f, -0.5000f, -0.5625f, -0.5000f, -0.4375f, -0.5000f };
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip.frames[1].triggerEvent = true;
			animationclip.frames[1].eventAudio = "Play_WPN_tiger_swipe_01";

			animationclip.frames[3].triggerEvent = true;
			animationclip.frames[3].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[5].triggerEvent = true;
			animationclip.frames[5].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[7].triggerEvent = true;
			animationclip.frames[7].eventAudio = "Play_WPN_Railgun_Click";

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_railgun_fire");
			offsetsX = new float[] { -0.8750f, -0.9375f, -0.8750f };
			offsetsY = new float[] { -0.4375f, -0.3750f, -0.4375f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_railgun_charge");
			offsetsX = new float[] { -0.6875f, -0.6875f, -0.6875f, -0.6875f, -0.6875f, -0.6875f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f, -0.7500f };
			offsetsY = new float[] { -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f, -0.2500f };
			animationclip.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			animationclip.loopStart = 10;
			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}


			animationclip.frames[2].triggerEvent = true;
			animationclip.frames[2].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[3].triggerEvent = true;
			animationclip.frames[3].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[4].triggerEvent = true;
			animationclip.frames[4].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[5].triggerEvent = true;
			animationclip.frames[5].eventAudio = "Play_WPN_Railgun_Click";

			/*animationclip.frames[10].triggerEvent = true;
			animationclip.frames[10].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[10].triggerEvent = true;
			animationclip.frames[10].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[11].triggerEvent = true;
			animationclip.frames[11].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[12].triggerEvent = true;
			animationclip.frames[12].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[13].triggerEvent = true;
			animationclip.frames[13].eventAudio = "Play_WPN_Railgun_Click";

			animationclip.frames[15].triggerEvent = true;
			animationclip.frames[15].eventAudio = "Play_WPN_Railgun_Click";
			*/
			animationclip = gun.sprite.spriteAnimator.GetClipByName("beyond_railgun_idle");
			offsetsX = new float[] { -0.6875f };
			offsetsY = new float[] { -0.2500f };

			for (int i = 0; i < offsetsX.Length && i < offsetsY.Length && i < animationclip.frames.Length; i++)
			{
				int id = animationclip.frames[i].spriteId; tk2dSpriteDefinition def = animationclip.frames[i].spriteCollection.spriteDefinitions[id];
				Vector3 offset = new Vector2(offsetsX[i], offsetsY[i]);
				def.position0 += offset;
				def.position1 += offset;
				def.position2 += offset;
				def.position3 += offset;
			}

			gun.barrelOffset.localPosition = new Vector3(0.5f, 0.125f, 0f);
			gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(15) as Gun, true, false);

			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1f;

			gun.DefaultModule.cooldownTime = 0.1f;
			gun.InfiniteAmmo = false;
			gun.DefaultModule.numberOfShotsInClip = 1;

			gun.SetBaseMaxAmmo(25);
			gun.gunHandedness = GunHandedness.HiddenOneHanded;

			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
			gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Emp Railgun Spear", "BotsMod/sprites/CustomGunAmmoTypes/beyond_railgun_clip_001", "BotsMod/sprites/CustomGunAmmoTypes/beyond_railgun_clip_002");



			gun.quality = PickupObject.ItemQuality.A;

			ETGMod.Databases.Items.Add(gun, null, "ANY");


			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.baseData.speed = 250;
			projectile.gameObject.SetActive(false);

			var trail = FakePrefab.Clone((PickupObjectDatabase.GetById(370) as Gun).DefaultModule.chargeProjectiles[1].Projectile.gameObject.GetComponentInChildren<TrailController>().gameObject);
			trail.transform.parent = projectile.transform;
			var projTrail = trail.GetComponent<TrailController>();
			trail.SetActive(true);


			projTrail.DispersalParticleSystemPrefab = BeyondPrefabs.AHHH.LoadAsset<GameObject>("VFX_Beyond_Railgun_Dispersal");


			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			var spear = projectile.gameObject.AddComponent<EmpSpearBuff>();


			var spikeVfx = PrefabBuilder.BuildObject("VFX_StickyRailgunSpike_001");


			//to any modders looking at this code do not use it please its fucking awful


			var baseSpriteID = SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_001.png"), SpriteBuilder.itemCollection, "railgun_spike_001");

			var spikeSprite = spikeVfx.AddComponent<tk2dSprite>();
			spikeSprite.SetSprite(SpriteBuilder.itemCollection, baseSpriteID);
			spikeSprite.SortingOrder = 0;

			spikeSprite.IsPerpendicular = true;

			spikeVfx.GetComponent<BraveBehaviour>().sprite = spikeSprite;


			var sAnimator = spikeVfx.AddComponent<tk2dSpriteAnimator>();
			var sprite10 = SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_010.png"), spikeSprite.Collection, "railgun_spike_010");
			var anim = SpriteHandler.AddAnimation(sAnimator, spikeSprite.Collection, new List<int>
			{
				baseSpriteID,
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_002.png"), spikeSprite.Collection, "railgun_spike_002"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_003.png"), spikeSprite.Collection, "railgun_spike_003"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_004.png"), spikeSprite.Collection, "railgun_spike_004"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_005.png"), spikeSprite.Collection, "railgun_spike_005"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_006.png"), spikeSprite.Collection, "railgun_spike_006"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_007.png"), spikeSprite.Collection, "railgun_spike_007"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_008.png"), spikeSprite.Collection, "railgun_spike_008"),
				SpriteHandler.AddSpriteToCollection(ResourceExtractor.GetTextureFromResource("BotsMod/sprites/VFX/Spike/railgun_spike_009.png"), spikeSprite.Collection, "railgun_spike_009"),
				sprite10,
				sprite10,
				sprite10,
				sprite10,
				sprite10,
				sprite10,

			}, "idle", tk2dSpriteAnimationClip.WrapMode.Once, 8);

			

			sAnimator.playAutomatically = true;

			

			FieldInfo _animationStyle = typeof(BuffVFXAnimator).GetField("animationStyle", BindingFlags.NonPublic | BindingFlags.Instance);

			var vfxAnimator = spikeVfx.AddComponent<BuffVFXAnimator>();
			vfxAnimator.motionPeriod = 1;
			vfxAnimator.ChanceOfApplication = 1;
			vfxAnimator.persistsOnDeath = true;
			vfxAnimator.AdditionalPierceDepth = 0;
			vfxAnimator.UsesVFXToSpawnOnDeath = false;
			vfxAnimator.VFXToSpawnOnDeath = new VFXPool { effects = new VFXComplex[0], type = VFXPoolType.None };
			vfxAnimator.NonPoolVFX = null;
			vfxAnimator.DoesSparks = false;
			vfxAnimator.SparksModule = new GlobalSparksModule
			{
				emitType = GlobalSparksDoer.EmitRegionStyle.RADIAL,
				RatePerSecond = 20,
				sparksType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT
			};
			vfxAnimator.tetrominoType = TetrisBuff.TetrisType.BLOCK;

			_animationStyle.SetValue(vfxAnimator, 2);

			

			//spear.vfx = (PickupObjectDatabase.GetById(4) as Gun).DefaultModule.projectiles[0].gameObject.GetComponent<OrbitalProjDebuff>().vfx;
			spear.vfx = spikeVfx;
			spear.empEffect = new EmpActorEffect
			{
				AffectsEnemies = true,
				AffectsPlayers = false,
				AppliesDeathTint = false,
				AppliesOutlineTint = true,
				OutlineTintColor = new Color(0.276f, 1, 1.03f),
				AppliesTint = false,
				DamageMultiplier = 2,
				duration = 5,
				DamagePerSecondToEnemies = 0f,
				stackMode = GameActorEffect.EffectStackingMode.Refresh,
				resistanceType = EffectResistanceType.None,
				effectIdentifier = "emp",
				SpeedMultiplier = 0.8f,
				CooldownMultiplier = 1.7f,
				flameNumPerSquareUnit = 10,
			};
			projectile.SetProjectileSpriteRight("beyond_railgun_spike_001", 12, 4, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 2, false, false, 0, 0);
			projectile.shouldRotate = true;
			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				new ProjectileModule.ChargeProjectile
				{
					Projectile = projectile,
					AmmoCost = 1,
					ChargeTime = 0.8f,
				},
			};
			Tools.BeyondItems.Add(gun.PickupObjectId);
			MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);

			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			material.SetColor("_EmissiveColor", new Color32(255, 69, 245, 255));
			material.SetFloat("_EmissiveColorPower", 1.55f);
			material.SetFloat("_EmissivePower", 55);
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

		}
	}
}
