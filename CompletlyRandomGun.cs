using System;
using System.Collections.Generic;
using System.Linq;
using CustomCharacters;
using Gungeon;
using GungeonAPI;
//using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace BotsMod
{

	public class CompletlyRandomGun : GunBehaviour
	{
		private bool HasReloaded;


		public static Gun completlyRandomGun;

		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("THIS TEXT SHOULDN'T BE SHOWN", "randomgun");
			Game.Items.Rename("outdated_gun_mods:THIS_TEXT_SHOULDN'T_BE_SHOWN".ToLower(), "bot:random_gun");
			gun.gameObject.AddComponent<CompletlyRandomGun>();
			gun.SetShortDescription("THIS TEXT SHOULDN'T BE SHOWN");
			gun.SetLongDescription("THIS TEXT SHOULDN'T BE SHOWN");
			
			gun.SetupSprite(null, "randomgun_idle_001", 8);


			gun.SetAnimationFPS(gun.shootAnimation, 12);
			gun.SetAnimationFPS(gun.reloadAnimation, 10);

			completlyRandomGun = gun;

			DoRandomizeGun(gun);


			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}

		static ProjectileModule module;

		public static void DoRandomizeGun(Gun gun)
		{


			var rngStats = OkJarvisNowRandomizeThem();

			/*BotsModule.Log("Range: " + rngStats[0]);
			BotsModule.Log("Dmg: " + rngStats[1]);
			BotsModule.Log("Firerate: " + rngStats[2]);
			BotsModule.Log("Spread: " + rngStats[3]);
			BotsModule.Log("MagSize: " + rngStats[4]);
			BotsModule.Log("Reload: " + rngStats[5]);
			BotsModule.Log("Speed: " + rngStats[6]);
			BotsModule.Log("Force: " + rngStats[7]);
			BotsModule.Log("Ammo: " + rngStats[8]);
			BotsModule.Log("AmmoCost: " + rngStats[9]);
			BotsModule.Log("NameSize: " + rngStats[10]);
			BotsModule.Log("DescSize: " + rngStats[11]);
			BotsModule.Log("LongDescSize: " + rngStats[12]);*/


			Gun gun2 = PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun;
			Gun gun3 = PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun;
			Gun gun5 = PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun;


			var m_anim = gun.GetComponent<tk2dSpriteAnimator>();

			//var shootCollection = gun.sprite.Collection.material.mainTexture;
			//var reloadSprite = gun.spriteAnimator.Library.GetClipByName(gun.reloadAnimation).frames[0].spriteCollection.material.mainTexture;
			//var idleSprite = gun.spriteAnimator.Library.GetClipByName(gun.idleAnimation).frames[0].spriteCollection.material.mainTexture;

			tk2dSpriteCollectionData blasphemyCollection = gun.sprite.Collection;//this is cause etg.Database.weapon collection and weapon collection02 were giving me weird problems but this should work for any valid collection
			for (int i = 0; i < blasphemyCollection.spriteDefinitions.Length; i++)
			{
				continue;
				var collectionName = "AHHAHHAHAHAHHHHHHHHHHAHHHHHHHHHHHHHH";

				string defName = blasphemyCollection.spriteDefinitions[i].name;
				//ETGModConsole.Log(defName.ToLower());
				if (defName.ToLower().Contains("randomgun"))// get rid of the statement if you wanna do it for literally every sprite

				{

					var def = blasphemyCollection.spriteDefinitions[i];
					if (def == null) continue;

					var material = def.material == null ? def.materialInst : def.material;
					if (material == null || material.mainTexture == null)
					{
						ItemAPI.Tools.PrintError($"Failed to edit {defName} in {collectionName}: No valid material");
						continue;
					}


					var texture = (Texture2D)material.mainTexture.GetReadable();

					
					material.mainTexture.GetReadable();
					var width = texture.width;
					var height = texture.height;

					var uvs = def.uvs;
					if (def.uvs == null || def.uvs.Length < 4)
					{
						ItemAPI.Tools.PrintError($"Failed to edit {defName} in {collectionName}: Invalid UV's");
						continue;
					}

					var minX = Mathf.RoundToInt(uvs[0].x * width);
					var minY = Mathf.RoundToInt(uvs[0].y * height);
					var X = Mathf.RoundToInt(uvs[3].x * width);
					var Y = Mathf.RoundToInt(uvs[3].y * height);

					var w = width - minX;
					var h = height - minY;

					if (w <= 0 || h <= 0)
					{
						ItemAPI.Tools.PrintError($"Failed to edit {defName} in {collectionName}: To small");
						continue;
					};

					var pixels = texture.GetPixels(minX, minY, w, h);
					//
					var output = new Texture2D(w, h);
					output.SetPixels(pixels);
					output.Apply();

					ToolsGAPI.ExportTexture(output, "BotsDUMPsprites/RandomSprites", def.name);
					if (def.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						output = output.Rotated().Flipped();
					}
					output.name = def.name;
					//BotsModule.Log(output.name, BotsModule.TEXT_COLOR);
					for (int y = minY; y < (minY); y++)
					{
						for (int x = minX; x < (minX); x++)
						{
							output.SetPixel(x, y, new Color(UnityEngine.Random.RandomRange(0.0f, 255.0f), UnityEngine.Random.RandomRange(0.0f, 255.0f), UnityEngine.Random.RandomRange(0.0f, 255.0f)));

						}
					}
					output.Apply();

					//gun.sprite.renderer.material.SetTexture("_MainTex", output);

					ToolsGAPI.ExportTexture(output, "BotsDUMPsprites/RandomSprites", i + " random");

					def.material.SetTexture("_MainTex", output);

				}
			}


			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()_-+=|';:,.<>/?[]{}`~";
			var name = new string (Enumerable.Repeat(chars, (int)rngStats[10]).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
			var desc = new string (Enumerable.Repeat(chars, (int)rngStats[11]).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
			var longDesc = new string (Enumerable.Repeat(chars, (int)rngStats[12]).Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());

			gun.SetName(name);
			gun.SetShortDescription(desc);
			gun.SetLongDescription(longDesc);

			Gun other = PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun;
			if (module == null) 
			{
				
				module = gun.AddProjectileModuleFrom(other, true, false);
			}

			gun.SetBaseMaxAmmo((int)rngStats[8]);
			gun.InfiniteAmmo = false;

			gun.DefaultModule.ammoCost = (int)rngStats[9];

			gun.DefaultModule.shootStyle = (ProjectileModule.ShootStyle)UnityEngine.Random.Range(0, 5);
			gun.reloadTime = rngStats[5];

			gun.DefaultModule.ammoType = (GameUIAmmoType.AmmoType)UnityEngine.Random.Range(0, 14);
			gun.DefaultModule.customAmmoType = other.DefaultModule.customAmmoType;

			gun.finalMuzzleFlashEffects = gun5.muzzleFlashEffects;

			gun.DefaultModule.cooldownTime = rngStats[2];
			gun.DefaultModule.numberOfShotsInClip = (int)rngStats[4];
			gun.quality = PickupObject.ItemQuality.SPECIAL;

			gun.gunClass = GunClass.SHITTY;
			gun.CanBeDropped = true;
			gun.PreventStartingOwnerFromDropping = true;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(other.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			ItemAPI.FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;

			gun.shellsToLaunchOnReload = 14;
			gun.shellCasing = gun3.shellCasing;

			gun.shellsToLaunchOnFire = 0;

			projectile.transform.parent = gun.barrelOffset;
			projectile.hitEffects = gun4.DefaultModule.projectiles[0].hitEffects;

			projectile.baseData.damage = rngStats[1];
			projectile.baseData.speed = rngStats[6];
			projectile.baseData.force = rngStats[7];
			projectile.baseData.range = rngStats[0];


			gun.emptyAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).emptyAnimation;
			gun.idleAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).idleAnimation;
			gun.introAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).introAnimation;
			gun.shootAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).shootAnimation;
			gun.reloadAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).reloadAnimation;
			gun.emptyReloadAnimation = (PickupObjectDatabase.GetById(GetRandomGunThatsNotABeam()) as Gun).emptyReloadAnimation;

			/*BotsModule.Log(gun.emptyAnimation);
			BotsModule.Log(gun.idleAnimation);
			BotsModule.Log(gun.introAnimation);
			BotsModule.Log(gun.shootAnimation);
			BotsModule.Log(gun.reloadAnimation);
			BotsModule.Log(gun.emptyReloadAnimation);

			BotsModule.Log("g15");



			BotsModule.Log("name: " + name);
			BotsModule.Log("desc: " + desc);
			BotsModule.Log("longDesc: " + longDesc);*/
		}

		private static int GetRandomGunThatsNotABeam()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
			{
				if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule != null)
				{

					if ((PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Charged && (PickupObjectDatabase.Instance.Objects[i] as Gun).EncounterNameOrDisplayName != "Test Gun")
					{
						if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
						{
							if (PickupObjectDatabase.Instance.Objects[i].PickupObjectId != completlyRandomGun.PickupObjectId)
							{
								EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
								if (component)
								{
									list.Add(PickupObjectDatabase.Instance.Objects[i].PickupObjectId);
								}
							}

						}
					}
					
				}
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}


		private static float[] OkJarvisNowRandomizeThem()
		{
			float rngRange = 0;
			float rngDmg = 0;
			float rngFirerate = 0;
			float rngSpread = 0;
			float rngMagSize = 0;
			float rngReload = 0;
			float rngSpeed = 0;
			float rngForce = 0;
			float rngAmmo = 0;
			float rngAmmoCost = 0;
			float rngNameSize = 0;
			float rngDescSize = 0;
			float rngLongDescSize = 0;
			float rngShellsReload = 0;
			float rngShellsFire = 0;



			float[] rngStats = new float[13]
			{
				rngRange,
				rngDmg,
				rngFirerate,
				rngSpread,
				rngMagSize,
				rngReload,
				rngSpeed,
				rngForce,
				rngAmmo,
				rngAmmoCost,
				rngNameSize,
				rngDescSize,
				rngLongDescSize,
		};

			var Stats = JarvisGetGunStatsAndMakeThemAnArray();
			var minStats = JarvisGetMinGunStatsAndMakeThemAnArray();

			for (int i = 0; i < rngStats.Length; i++)
			{
				rngStats[i] = UnityEngine.Random.Range(minStats[i], Stats[i]);
			}

			return rngStats;
		}

		private static float[] JarvisGetGunStatsAndMakeThemAnArray()
		{
			List<float> list = new List<float>();

			float Range = 0;
			float Dmg = 0;
			float Firerate = 0;
			float Spread = 0;
			float MagSize = 0;
			float Reload = 0;
			float Speed = 0;
			float Force = 0;
			float Ammo = 0;
			float AmmoCost = 0;
			float NameSize = 0;
			float DescSize = 0;
			float LongDescSize = 0;


			for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
			{
				if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule != null)
				{

					if ((PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Charged && (PickupObjectDatabase.Instance.Objects[i] as Gun).EncounterNameOrDisplayName != "Test Gun")
					{
						if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
						{
							if (PickupObjectDatabase.Instance.Objects[i].PickupObjectId != completlyRandomGun.PickupObjectId)
							{
								EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
								if (component)
								{
									var gun = (PickupObjectDatabase.Instance.Objects[i] as Gun);
									if (gun.PickupObjectId <= 823 || gun.PickupObjectId >= 0 && gun.DefaultModule.projectiles[0] != null && gun.DefaultModule != null)
									{
										

										if (Range < gun.DefaultModule.projectiles[0].baseData.range)
										{
											Range = gun.DefaultModule.projectiles[0].baseData.range;
										}

										if (Dmg < gun.DefaultModule.projectiles[0].baseData.damage)
										{
											Dmg = gun.DefaultModule.projectiles[0].baseData.damage;
										}

										if (Firerate < gun.DefaultModule.cooldownTime)
										{
											Firerate = gun.DefaultModule.cooldownTime;
										}

										if (Spread < gun.DefaultModule.angleVariance)
										{
											Spread = gun.DefaultModule.angleVariance;
										}

										if (MagSize < gun.DefaultModule.numberOfShotsInClip)
										{
											MagSize = gun.DefaultModule.numberOfShotsInClip;
										}

										if (Reload < gun.reloadTime)
										{
											Reload = gun.reloadTime;
										}

										if (Speed < gun.DefaultModule.projectiles[0].baseData.speed)
										{
											Speed = gun.DefaultModule.projectiles[0].baseData.speed;
										}

										if (Force < gun.DefaultModule.projectiles[0].baseData.force)
										{
											Force = gun.DefaultModule.projectiles[0].baseData.force;
										}

										if (Ammo < gun.GetBaseMaxAmmo())
										{
											Ammo = gun.GetBaseMaxAmmo();
										}

										if (AmmoCost < gun.DefaultModule.ammoCost)
										{
											AmmoCost = gun.DefaultModule.ammoCost;
										}

										if (NameSize < gun.encounterTrackable.journalData.PrimaryDisplayName.Length)
										{
											NameSize = gun.encounterTrackable.journalData.PrimaryDisplayName.Length;
										}

										if (LongDescSize < gun.encounterTrackable.journalData.AmmonomiconFullEntry.Length)
										{
											LongDescSize = gun.encounterTrackable.journalData.AmmonomiconFullEntry.Length;
										}

										if (DescSize < gun.encounterTrackable.journalData.NotificationPanelDescription.Length)
										{
											DescSize = gun.encounterTrackable.journalData.NotificationPanelDescription.Length;
										}






									}
								}
							}

						}
					}

				}
			}

			float[] Stats = new float[13] 
			{
				Range,
				Dmg,
				Firerate,
				Spread,
				MagSize,
				Reload,
				Speed,
				Force,
				Ammo,
				AmmoCost,
				NameSize,
				DescSize,
				LongDescSize,
			};

			/*BotsModule.Log("Range: " + Stats[0]);
			BotsModule.Log("Dmg: " + Stats[1]);
			BotsModule.Log("Firerate: " + Stats[2]);
			BotsModule.Log("Spread: " + Stats[3]);
			BotsModule.Log("MagSize: " + Stats[4]);
			BotsModule.Log("Reload: " + Stats[5]);
			BotsModule.Log("Speed: " + Stats[6]);
			BotsModule.Log("Force: " + Stats[7]);
			BotsModule.Log("Ammo: " + Stats[8]);
			BotsModule.Log("AmmoCost: " + Stats[9]);
			BotsModule.Log("NameSize: " + Stats[10]);
			BotsModule.Log("DescSize: " + Stats[11]);
			BotsModule.Log("LongDescSize: " + Stats[12]);*/

			return Stats;
		}

		private static float[] JarvisGetMinGunStatsAndMakeThemAnArray()
		{
			List<float> list = new List<float>();

			float minRange = float.MaxValue;
			float minDmg = float.MaxValue;
			float minFirerate = float.MaxValue;
			float minSpread = float.MaxValue;
			float minMagSize = float.MaxValue;
			float minReload = float.MaxValue;
			float minSpeed = float.MaxValue;
			float minForce = float.MaxValue;
			float minAmmo = float.MaxValue;
			float minAmmoCost = float.MaxValue;
			float minNameSize = float.MaxValue;
			float minLongDescSize = float.MaxValue;
			float minDescSize = float.MaxValue;


			for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
			{
				if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule != null)
				{

					if ((PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam && (PickupObjectDatabase.Instance.Objects[i] as Gun).DefaultModule.shootStyle != ProjectileModule.ShootStyle.Charged && (PickupObjectDatabase.Instance.Objects[i] as Gun).EncounterNameOrDisplayName != "Test Gun")
					{
						if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
						{
							if (PickupObjectDatabase.Instance.Objects[i].PickupObjectId != completlyRandomGun.PickupObjectId)
							{
								EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
								if (component)
								{
									var gun = (PickupObjectDatabase.Instance.Objects[i] as Gun);
									if (gun.PickupObjectId < 823 && gun.PickupObjectId > 0 && gun.DefaultModule.projectiles[0] != null && gun.DefaultModule != null)
									{
										
										if (gun.DefaultModule.projectiles[0] != null && gun.DefaultModule != null && gun != null)
										{
											if (minRange > gun.DefaultModule.projectiles[0].baseData.range)
											{
												minRange = gun.DefaultModule.projectiles[0].baseData.range;
											}

											if (minDmg > gun.DefaultModule.projectiles[0].baseData.damage)
											{
												minDmg = gun.DefaultModule.projectiles[0].baseData.damage;
											}

											if (minFirerate > gun.DefaultModule.cooldownTime)
											{
												minFirerate = gun.DefaultModule.cooldownTime;
											}

											if (minSpread > gun.DefaultModule.angleVariance)
											{
												minSpread = gun.DefaultModule.angleVariance;
											}

											if (minMagSize > gun.DefaultModule.numberOfShotsInClip)
											{
												minMagSize = gun.DefaultModule.numberOfShotsInClip;
											}

											if (minReload > gun.reloadTime)
											{
												minReload = gun.reloadTime;
											}

											if (minSpeed > gun.DefaultModule.projectiles[0].baseData.speed)
											{
												minSpeed = gun.DefaultModule.projectiles[0].baseData.speed;
											}

											if (minForce > gun.DefaultModule.projectiles[0].baseData.force)
											{
												minForce = gun.DefaultModule.projectiles[0].baseData.force;
											}

											if (minAmmo > gun.GetBaseMaxAmmo())
											{
												minAmmo = gun.GetBaseMaxAmmo();
											}

											if (minAmmoCost > gun.DefaultModule.ammoCost)
											{
												minAmmoCost = gun.DefaultModule.ammoCost;
											}


											if (minNameSize > gun.encounterTrackable.journalData.PrimaryDisplayName.Length)
											{
												minNameSize = gun.encounterTrackable.journalData.PrimaryDisplayName.Length;
											}

											if (minLongDescSize > gun.encounterTrackable.journalData.AmmonomiconFullEntry.Length)
											{
												minLongDescSize = gun.encounterTrackable.journalData.AmmonomiconFullEntry.Length;
											}

											if (minDescSize > gun.encounterTrackable.journalData.NotificationPanelDescription.Length)
											{
												minDescSize = gun.encounterTrackable.journalData.NotificationPanelDescription.Length;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			float[] minStats = new float[13]
			{
				minRange,
				minDmg,
				minFirerate,
				minSpread,
				minMagSize,
				minReload,
				minSpeed,
				minForce,
				minAmmo,
				minAmmoCost,
				minNameSize,
				minDescSize,
				minLongDescSize,
			};
			/*BotsModule.Log("MinRange: " + minStats[0]);
			BotsModule.Log("MinDmg: " + minStats[1]);
			BotsModule.Log("MinFirerate: " + minStats[2]);
			BotsModule.Log("MinSpread: " + minStats[3]);
			BotsModule.Log("MinMagSize: " + minStats[4]);
			BotsModule.Log("MinReload: " + minStats[5]);
			BotsModule.Log("MinSpeed: " + minStats[6]);
			BotsModule.Log("MinForce: " + minStats[7]);
			BotsModule.Log("MinAmmo: " + minStats[8]);
			BotsModule.Log("MinAmmoCost: " + minStats[9]);
			BotsModule.Log("MinNameSize: " + minStats[10]);
			BotsModule.Log("MinDescSize: " + minStats[11]);
			BotsModule.Log("MinLongDescSize: " + minStats[12]);*/
			return minStats;
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			base.PostProcessProjectile(projectile);

		}

		protected void Update()
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				bool flag2 = !this.gun.PreventNormalFireAudio;
				if (flag2)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag3)
				{
					this.HasReloaded = true;
				}
			}
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_OBJ_blackhole_close_01", base.gameObject);
			}
		}



		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", base.gameObject);
		}

	}
}
