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
using CustomCharacters;
using Pathfinding;
using BotsMod.ToolsAndStuff;

namespace BotsMod
{
	public class TestActive : PlayerItem
	{
		static dfAtlas fuckyou;
		public static void Init()
		{
			//The name of the item
			string itemName = "Test active Item";
			string resourceName = "BotsMod/sprites/wip";
			GameObject obj = new GameObject();
			var item = obj.AddComponent<TestActive>();

			//WandOfWonderItem
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			string shortDesc = "testing item";
			string longDesc = "this item is purly for testing [sprite \"ui_coin\"]";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0);
			item.consumable = false;
			item.quality = ItemQuality.EXCLUDED;

			fuckyou = GameUIRoot.Instance.ConversationBar.portraitSprite.Atlas;
			fuckyou.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/TestHeart.png"), "BrokenHeart");


		}
		private Dictionary<string, StringTableManager.StringCollection> leadMaidan;
		public override void Pickup(PlayerController player)
		{
			
			
			dfLabel nameLabel = GameUIRoot.Instance.notificationController.NameLabel;

			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_POPUP_TITLE")), StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_BODY_B")), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
			


			ETGModConsole.Log("" + StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].Count());
			ETGModConsole.Log("" + StringTableManager.GetSynergyString("#DESTINY_TWO"), false);
			ETGModConsole.Log("" + StringTableManager.GetSynergyString("#DESTINY_ONE"), false);
			//Dictionary<string, StringTableManager.StringCollection> leadMaidan = new Dictionary<string, StringTableManager.StringCollection>();
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);
			StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_FRIENDS"].AddString("Lead maidens are singlehandedly ruining this game. There's no reason an extremely common random enemy should be tons harder than everything else so far including the floor 2 boss even though Lead Maidens started showing up before that fight. It makes every moment not spent fighting a Lead Maiden pointless because whether or not I win depends 99% on whether or not that bullshit miniboss appears. I've never seen one nonboss singlehandedly ruin a game before but this one is doing it extremely efficiently.Beating one Lead Maiden is harder than beating Rabi - Ribi's True Boss Rush. It's fucking unforgivable to have the game change so radically every time that enemy shows up.It kills me in like one fucking hit & it has far more health than everything else I've fought before despite also being bigger & faster than everything else too. Whomever came up with that enemy needs to go jack off to medieval torture porn & get it out of their system.Jesus Christ! I really wish the creators of this game would've just decided whether to make an amazing game or the worst game ever & stuck with it.", 100000000);

			//StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"].AddString("Wow you have less brain cells than a titan main!", 100000);
			//StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].AddString("Wow you have less brain cells than a titan main!", 100000);
			StringTableManager.CoreTable["#INSULT_NAME"].AddString("titan main", 100000);
			StringTableManager.CoreTable["#INSULT_NAME"].AddString("gunslinger main", 100000);
			// = leadMaidan;


			ETGModConsole.Log("" + StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_ENMITY"].Count());

			//ETGModConsole.Log(StringTableManager.EvaluateReplacementToken("%INSULT"));


			//ungeon dungeon = GameManager.Instance.Dungeon;

			//RationItem ration = PickupObjectDatabase.GetById(104).GetComponent<RationItem>();
			//this.healVFX = ration.healVFX;
			//healingAmount = 1;
			//BulletArmorItem bulletArmor = PickupObjectDatabase.GetById(160).GetComponent<BulletArmorItem>();
			//transformSprites = player.AlternateCostumeLibrary;

			base.Pickup(player);
			//player.CurrentGun.OnPostFired += this.OnFired;

			//player.CurrentGun.OnReloadPressed += this.OnReload;

			textures.Add(GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/flags/trans.png"));

		}
		public tk2dSpriteAnimation altSkinThingo;
		public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration)
		{
			TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, stringKey, string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		}

		//public static Dictionary<PlayerController, Dictionary<Type, int>> itemFlags;

		private float x;
		private float y;
		private Vector3 rotateValue;

		List<Texture2D> textures = new List<Texture2D>();

		int value = 0;

		private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration, Vector3 pos,  PlayerOrbital obj, float angle = 0)
		{
			float elapsed = 0f;
			BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, source.CurrentGun.CurrentAngle, pos, obj);
			yield return null;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				this.ContinueFiringBeam(beam, source, angle, pos, obj);
				yield return null;
			}
			this.CeaseBeam(beam);
			yield break;
		}

		private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, PlayerOrbital obj)
		{
			Vector2 vector = obj.transform.position;
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

		private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint, PlayerOrbital obj)
		{
			Vector2 vector = obj.transform.position;
			beam.Direction = BraveMathCollege.DegreesToVector(source.CurrentGun.CurrentAngle, 1f);
			beam.Origin = vector;
			beam.LateUpdatePosition(vector);
		}

		private void CeaseBeam(BeamController beam)
		{
			beam.CeaseAttack();
		}

		private static Dictionary<string, StringTableManager.StringCollection> CoreTableOverride;
		int effectCount = 0;
		Texture2D texture;

		private IEnumerator HandleDoTheStupidThingImDoingOutOfSpiteDirectedAtNilt (PlayerController user)//, AIActor enemy)
		{


			RoomHandler absoluteRoom = user.transform.position.GetAbsoluteRoom();
			List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			victum = activeEnemies[0];
			for (int i = 1; i < activeEnemies.Count; i++)
			{
				activeEnemies[i].OverrideTarget = victum.specRigidbody;
				activeEnemies[i].CanTargetEnemies = true;
			}

			//var targetObject = enemy.gameObject;
			BotsMindControlEffect orAddComponent = victum.gameObject.GetOrAddComponent<BotsMindControlEffect>();
			orAddComponent.owner = (user);

			if (victum != null && user.CurrentRoom != null)
			{
				user.ReceivesTouchDamage = false;
				user.IsVisible = false;
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


				victum.healthHaver.OnDamaged += OnDamaged;
				victum.healthHaver.OnPreDeath += OnDeath;

				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(victum.sprite);
				outlineMaterial.SetColor("_OverrideColor", new Color(84f, 6f, 107f));
				
				while (victum != null)
				{
					//if (enemy.specRigidbody.Position.X != user.specRigidbody.Position.X && enemy.specRigidbody.Position.Y != user.specRigidbody.Position.Y)
					//if (enemy.specRigidbody.Position.m_position != user.specRigidbody.Position.m_position)
					//{
					victum.specRigidbody.Position = user.specRigidbody.Position;
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

				//if (!CmaeraOverridden)
				//{
				//	GameManager.Instance.MainCameraController.SetManualControl(false, true);
				//	Pixelator.Instance.LerpToLetterbox(0.5f, 0f);
				//}
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

		public IEnumerator Destrot()
		{
			//yield return new WaitForSeconds(5);
			BotsModule.Log($"base IEnumerator log message :o");
			yield return this;
		}


		public IEnumerator dofunnybeam(PlayerOrbital obj)
		{
			yield return new WaitForSeconds(5f);
			Gun fuckYouDie = PickupObjectDatabase.GetById(100) as Gun;

			Projectile currentProjectile = fuckYouDie.DefaultModule.GetCurrentProjectile();

			var beam = currentProjectile.GetComponent<BeamController>();
			//beam.AdjustPlayerBeamTint(colorKeys[num], 10000);


			BotsModule.Log(obj.sprite.WorldCenter.ToString());
			BotsModule.Log(obj.transform.localRotation.z.ToString());
			StartCoroutine(this.HandleFireShortBeam(currentProjectile, this.LastOwner, 100, obj.sprite.WorldCenter, obj, obj.transform.localRotation.z));
			yield break;
		}

		public static Action<Action<IEnumerator>> destrotAct;
		bool doneReset = false;
		static AIActor victum;
		GameObject LinkVFXPrefab;
		tk2dTiledSprite extantLink;

		Vector2 pos3 = Vector2.zero;
		Vector2 pos4 = Vector2.zero;

		public override void Update()
		{

			if (Event.current.isKey && Event.current.keyCode.ToString().ToLower() != "none")
			{
				for (int i = 0; i < Event.current.keyCode.ToString().Length; i++)
				{
					var player = base.LastOwner;
					GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0].gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
					Projectile component = gameObject.GetComponent<Projectile>();

					if (component != null)
					{
						component.Owner = player;
						component.Shooter = player.specRigidbody;
						component.baseData.speed /= 4;
						component.sprite.SetSprite(this.GetLetterForWordPosition(Event.current.keyCode.ToString().ToUpper(), i));
					}
				}

			}




			base.Update();
		}


		private string GetLetterForWordPosition(string word, int num = 0)
		{
			char c = word[num];
			switch (c)
			{
				case 'A':
					return "word_projectile_A_001";
				case 'B':
					return "word_projectile_B_001";
				case 'C':
					return "word_projectile_C_001";
				case 'D':
					return "word_projectile_D_001";
				case 'E':
					return "word_projectile_B_004";
				case 'F':
					return "word_projectile_F_001";
				case 'G':
					return "word_projectile_G_001";
				case 'H':
					return "word_projectile_H_001";
				case 'I':
					return "word_projectile_I_001";
				case 'J':
					return "word_projectile_J_001";
				case 'K':
					return "word_projectile_K_001";
				case 'L':
					return "word_projectile_B_003";
				case 'M':
					return "word_projectile_M_001";
				case 'N':
					return "word_projectile_N_001";
				case 'O':
					return "word_projectile_O_001";
				case 'P':
					return "word_projectile_P_001";
				case 'Q':
					return "word_projectile_Q_001";
				case 'R':
					return "word_projectile_R_001";
				case 'S':
					return "word_projectile_S_001";
				case 'T':
					return "word_projectile_B_005";
				case 'U':
					return "word_projectile_B_002";
				case 'V':
					return "word_projectile_V_001";
				case 'W':
					return "word_projectile_W_001";
				case 'X':
					return "word_projectile_X_001";
				case 'Y':
					return "word_projectile_Y_001";
				case 'Z':
					return "word_projectile_Z_001";
				default:
					if (c == '+')
					{
						return "word_projectile_+_001";
					}
					if (c != '1')
					{
						return "word_projectile_B_001";
					}
					return "word_projectile_1_001";
				case 'a':
					return "word_projectile_alpha_001";
				case 'o':
					return "word_projectile_omega_001";
			}
		}

		DebrisObject pain;
		DebrisObject suffering;

		int i = 0;
		protected override void DoEffect(PlayerController user)
		{
			Vector2 vector = user.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
			Vector2 vector2 = user.specRigidbody.HitboxPixelCollider.UnitTopRight;
			//(!this.IsGreenFire) ? GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE : 
			GlobalSparksDoer.DoRandomParticleBurst(6, vector, vector2, Vector3.zero, 0f, 0f, null, null, null, GlobalSparksDoer.SparksType.STRAIGHT_UP_GREEN_FIRE);

			//3882816446524679410
			//-6243670249480530921 - ParticleSystem #4263
			//3838344109541889202 - ParticleSystemRenderer #18562
			//-1724382749291533491 - PlayParticleSystemDuringBossIntro

			GoopDefinition goop = new GoopDefinition
			{

				CanBeIgnited = true,
				selfIgniteDelay = 1,
				SelfIgnites = true,
				damagesEnemies = false,
				damagesPlayers = false,
				goopTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/water_standard_base_001.png"),
				usesLifespan = true,
				playerStepsChangeLifetime = false,
				playerStepsLifetime = 2.5f,
				baseColor32 = new Color32(252, 3, 227, 255),
				igniteColor32 = new Color32(224, 0, 146, 255),
			};

			Vector2 normalized = (user.unadjustedAimPoint.XY() - user.CenterPosition).normalized;
			Vector2 normalized2 = BraveMathCollege.DegreesToVector(5);

			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);

			goopManagerForGoopType.TimedAddGoopLine(user.CenterPosition + Vector2.right *3 , (user.CenterPosition + (normalized * 8 - normalized2)) , 1, 0.3f);
			goopManagerForGoopType.TimedAddGoopLine(user.CenterPosition + Vector2.left * 3, (user.CenterPosition + (normalized * 8 + normalized2)) , 1, 0.3f);

			return;
			GameUIRoot.Instance.heartControllers[0].extantArmors[i].SpriteName = "BrokenHeart";
			GameUIRoot.Instance.heartControllers[0].extantArmors[i].Atlas = fuckyou;

			if (i < (GameUIRoot.Instance.heartControllers[0].extantArmors.Count - 1))
            {
				i++;
            }
			return;

			user.CurrentRoom.ApplyActionToNearbyEnemies(user.CenterPosition, 100f, delegate (AIActor enemy, float dist)
			{
				if (enemy && enemy.healthHaver)
				{
					enemy.OverrideBlackPhantomShader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
					enemy.sprite.renderer.material.SetTexture("_EeveeTex", textures[0]);
					//enemy.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
					//enemy.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
					enemy.CanTargetPlayers = false;

					enemy.BecomeBlackPhantom();
				}
			});



			var linkController = LootEngine.SpewLoot(new List<GameObject> { PickupObjectDatabase.GetById(UnityEngine.Random.Range(0, 824)).gameObject }, GameManager.Instance.PrimaryPlayer.sprite.WorldBottomCenter + Vector2.right)[0].gameObject.AddComponent<LinkControllerComp>();
			var item2 = LootEngine.SpewLoot(new List<GameObject> { PickupObjectDatabase.GetById(UnityEngine.Random.Range(0, 824)).gameObject }, GameManager.Instance.PrimaryPlayer.sprite.WorldBottomCenter + Vector2.left);

			

			var linkVFXPrefab = FakePrefab.Clone((PickupObjectDatabase.GetById(29) as Gun).DefaultModule.projectiles[0].GetComponent<ChainLightningModifier>().LinkVFXPrefab);

			tk2dTiledSprite component = SpawnManager.SpawnVFX(linkVFXPrefab, false).GetComponent<tk2dTiledSprite>();

			linkController.extantLink = component;
			linkController.otherItem = item2[0];
			linkController.LinkVFXPrefab = linkVFXPrefab;

			this.extantLink = component;
			Tools.UpdateLink(linkController.GetComponent<DebrisObject>(), component, item2[0]);

			return;
			foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<PickupObject>())
			{

				//dumbpeiceofshit.Pickup(user);
				if (dumbpeiceofshit.gameObject.GetComponent<DoDamage>() == null)
				{
					var diepls = dumbpeiceofshit.gameObject.AddComponent<DoDamage>();

					diepls.idToCopy = dumbpeiceofshit.PickupObjectId;
				}
				
				
			}
			return;
			CollectionDumper.DumpCollection(user.sprite.Collection);
			if (user.AlternateCostumeLibrary != null)
			{
				CollectionDumper.DumpCollection(user.AlternateCostumeLibrary.clips[0].frames[0].spriteCollection);
			}
			

			//var portal = BraveResources.Load<GameObject>("VFX_PortalDirectlyToHell", ".prefab");
			//var portal = (GameObject)Tools.brave.LoadAsset("VFX_PortalDirectlyToHell");
			var portal = (PickupObjectDatabase.GetById(155) as SpawnObjectPlayerItem).objectToSpawn.GetComponent<BlackHoleDoer>().HellSynergyVFX;
			GameObject portalButReal = UnityEngine.Object.Instantiate<GameObject>(portal, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, 0f));

			portalButReal.GetComponent<MeshRenderer>().material.SetTexture("_PortalTex", ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/lostTransFlag.png"));



			return;
			var num = 0;

			var colorKeys = new List<Color32>();
			colorKeys.Add(new Color32(255, 0, 0, 255));
			colorKeys.Add(new Color32(255, 102, 0, 255));
			colorKeys.Add(new Color32(255, 174, 0, 255));
			colorKeys.Add(new Color32(255, 242, 0, 255));
			colorKeys.Add(new Color32(234, 255, 0, 255));
			colorKeys.Add(new Color32(174, 255, 0, 255));
			colorKeys.Add(new Color32(81, 255, 0, 255));
			colorKeys.Add(new Color32(0, 255, 98, 255));
			colorKeys.Add(new Color32(0, 255, 200, 255));
			colorKeys.Add(new Color32(0, 234, 255, 255));
			colorKeys.Add(new Color32(0, 149, 255, 255));
			colorKeys.Add(new Color32(0, 38, 255, 255));
			colorKeys.Add(new Color32(119, 0, 255, 255));
			colorKeys.Add(new Color32(174, 0, 255, 255));
			colorKeys.Add(new Color32(242, 0, 255, 255));

			for (var i = 0; i < colorKeys.Count*2 - 2; i++)
			{
				LootEngine.TryGivePrefabToPlayer(PickupObjectDatabase.GetById(260).gameObject, user);
			}

			foreach (var obj in UnityEngine.Object.FindObjectsOfType<PlayerOrbital>())
			{
				//obj.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
				//obj.gameActor.RegisterOverrideColor(colorKeys[num], "idfk" + num);
				var tr = obj.gameObject.AddComponent<TrailRenderer>();
				obj.shouldRotate = true;
				tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				tr.receiveShadows = false;
				var mat = new Material(Shader.Find("Sprites/Default"));
				//mat.SetColor(, colorKeys[num]);
				mat.mainTexture = texture;
				mat.SetColor("_Color", colorKeys[num]);
				tr.material = mat;
				tr.time = 1;
				tr.minVertexDistance = 0.5f;
				tr.startWidth = 0.5f;
				tr.endWidth = 0;
				tr.startColor = Color.white;
				tr.endColor = Color.white;


				var alphaKeys = new GradientAlphaKey[2];

				alphaKeys[0].alpha = 1;
				alphaKeys[0].time = 0;
				alphaKeys[1].alpha = 0;
				alphaKeys[1].time = 1;



				//tr.colorGradient = new Gradient
				//{
				//	alphaKeys = alphaKeys,
				//	colorKeys = colorKeys,
				//};
				BotsModule.Log(num.ToString());
				if (num < (colorKeys.Count - 1) && !doneReset)
				{
					num++;
				} 
				else if (num > 0)
				{
					num = 0;
					//doneReset = true;
				}
				else
				{
					
					//doneReset = false;
				}
				StartCoroutine(dofunnybeam(obj));

			}
			//var gayRock = (PickupObjectDatabase.GetById(260) as PlayerOrbitalItem).OrbitalPrefab.gameObject;


			

			/**/

			//PlayerOrbitalItem.CreateOrbital(user, gayRock, false);


			return;
			CollectionDumper.DumpCollection(user.sprite.Collection);
			StartCoroutine(Destrot());

			uint idk;

			AkSoundEngine.GetSwitch("CHR_Player", user.gameObject, out idk);
			//AkSoundEngine.switch

			foreach(var aEvent in user.animationAudioEvents)
			{
				BotsModule.Log(aEvent.eventTag);
				BotsModule.Log(aEvent.eventName);
			}

			BotsModule.Log("" + idk);

			
			foreach (var clip in user.gameActor.spriteAnimator.Library.clips)
			{

				foreach (var frame in clip.frames)
				{
					if (frame.eventAudio.Length > 0)
					{
						BotsModule.Log(clip.name + ": " + frame.eventAudio);
					}

				}
			}


			BotsModule.Log(user.OverridePlayerSwitchState);


			AkSoundEngine.PostEvent("Play_Bot_Hammer", user.gameObject);

			var room = RoomFactory.CreateRoomFromTexture(ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod/sprites/cursething.png"));
			RoomFactory.AddExit(room, new Vector2(room.Width / 2, room.Height), DungeonData.Direction.NORTH);
			RoomFactory.AddExit(room, new Vector2(room.Width / 2, 0), DungeonData.Direction.SOUTH);
			RoomFactory.AddExit(room, new Vector2(room.Width, room.Height / 2), DungeonData.Direction.EAST);
			RoomFactory.AddExit(room, new Vector2(0, room.Height / 2), DungeonData.Direction.WEST);
			RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room);
			Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);

			

			user.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);



			var chest = Chest.Spawn(BotsModule.KeyChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));
			//var chest = Chest.Spawn(GameManager.Instance.RewardManager.B_Chest, new IntVector2((int)user.sprite.WorldCenter.x, (int)user.sprite.WorldCenter.y), user.CurrentRoom, false);

			//chest.lootTable.canDropMultipleItems = true;

			//chest.lootTable.multipleItemDropChances = new WeightedIntCollection();
			//chest.lootTable.multipleItemDropChances.elements = new WeightedInt[1];

			//WeightedInt weightedInt = new WeightedInt();
			//weightedInt.value = 8;
			//weightedInt.weight = 1f;

			//chest.lootTable.multipleItemDropChances.elements[0] = weightedInt;

			chest.IsLocked = false;

			//chest.lootTable.Common_Chance = 0.7f;



			//user.gameObject.AddComponent<SquishyBounceWiggler>();
			//foreach(tk2dSpriteDefinition sprite in user.sprite.Collection.spriteDefinitions)
			//{
			//	BotsModule.Log(sprite.name, BotsModule.LOCKED_CHARACTOR_COLOR);
			//}
			for (int i = 0; i < user.CurrentGun.sprite.Collection.spriteDefinitions.Length; i++)
			{
				var attachPoints = user.CurrentGun.sprite.Collection.spriteDefinitions[i].GetAttachPoints(user.CurrentGun.sprite.Collection, user.CurrentGun.sprite.spriteId);
				foreach(tk2dSpriteDefinition.AttachPoint attachPoint in attachPoints)
				{
					if (attachPoint.name == "PrimaryHand")
					{
						attachPoint.position.x += 0.0625f;
					}
				}

			}
				
			
			



			
			float ignoreThisPls = 0;
			foreach (PlayerItem item in user.activeItems)
			{
				if (item != this && item.CanBeUsed(user) && !item.consumable && !item.IsOnCooldown)
				{
					item.Use(user, out ignoreThisPls);
					item.ClearCooldowns();
				}
			}

			
			/*

			
			if (victum == null)
			{

				GameManager.Instance.StartCoroutine(HandleDoTheStupidThingImDoingOutOfSpiteDirectedAtNilt(user));
			} 
			else if (victum != null)
			{
				victum.healthHaver.ApplyDamage(10000000, Vector2.zero, "fuck you :D", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
			}




			
			return;
			Gun fuckYouDie = PickupObjectDatabase.GetById(595) as Gun;
			Gun fuckYouDie2 = PickupObjectDatabase.GetById(474) as Gun;
			Projectile currentProjectile = fuckYouDie.DefaultModule.GetCurrentProjectile();
			Projectile currentProjectile2 = fuckYouDie2.DefaultModule.GetCurrentProjectile();
			bool flag = currentProjectile.GetComponent<BeamController>() != null;

			if (flag)
			{

				var beam = currentProjectile.GetComponent<BeamController>();
				beam.AdjustPlayerBeamTint(Color.green, 10000);
				user.StartCoroutine(this.HandleFireShortBeam(currentProjectile, user, 10));

			}*/
			
			dfLabel nameLabel = GameUIRoot.Instance.notificationController.NameLabel;
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_TITLE")), StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_BODY")), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
		
			//Tools.AHHH
			//AkSoundEngine.PostEvent(4001772290, user.gameObject);
			//
			//AkSoundEngine.PostEvent("Play_EX_RickRollMusic_01", user.gameObject);
			//user.IsTemporaryEeveeForUnlock = true;
			

			switch (effectCount)
			{
				case 0:
					texture = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\decay_texture.png");
					break;
				case 1:
					texture = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\paradox_test.png");
					break;
				case 2:
					texture = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\nebula.png");
					break;
				case 3:
					texture = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\PerlinHalfRough.png");
					break;
				case 4:
					texture = GungeonAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\PerlinHalfRough.png");
					break;
				case 5:
					effectCount = 0;
					break;
			}
			
			
			
			user.ClearOverrideShader();

			user.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
			Texture2D value2 = user.portalEeveeTex;
			if (this && base.sprite && base.sprite.renderer && base.sprite.renderer.material)
			{
				user.sprite.renderer.material.SetTexture("_EeveeTex", texture);
				user.sprite.renderer.material.SetColor("_OverrideColor", Color.green);
				user.sprite.renderer.material.SetColor("_FlatColor", Color.cyan);
				user.sprite.renderer.material.SetFloat("_Perpendicular", 1);
				user.sprite.renderer.material.SetFloat("_StencilVal", 0);
				user.sprite.renderer.material.SetVector("_SpecialFlags", new Vector4(0, 1, 70, 0));


			}
			effectCount++;
			
			

			if (user.characterIdentity == PlayableCharacters.Eevee)
			{
				
				if (this && base.sprite && base.sprite.renderer && base.sprite.renderer.material)
				{
					base.sprite.renderer.material.SetTexture("_EeveeTex", texture);
				}
			} else
			{

			}

			



			

			//AkSoundEngine.PostEvent("Play_CHR_convict_voice_01", user.gameObject);

			this.DoAmbientTalk(user.transform, new Vector3(1, 2, 0), " (" + 7.ToString() + "[sprite \"hbux_text_icon\"])", 4f);

			Dungeon dungeon = GameManager.Instance.Dungeon;


			Vector2 pos = user.sprite.WorldCenter;

			

	
			
			//Gun Seed = PickupObjectDatabase.GetById(197) as Gun;
			//var sourceProjectile = Seed.DefaultModule.projectiles[0];

			//Vector3 rotation = GameManager.Instance.MainCameraController.Camera.transform.eulerAngles;

			//rotation.x += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; // Standart Left-/Right Arrows and A & D Keys

			//transform.eulerAngles = rotation;
			

			//GameObject gameObject = SpawnManager.SpawnProjectile(sourceProjectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
			Projectile component8 = gameObject.GetComponent<Projectile>();

			bool flag9999 = component8 != null;
			if (flag9999)
			{
				component8.Owner = user;
				component8.sprite.spriteId = 291;
				component8.baseData.speed = 0.5f;
				
				component8.Shooter = user.specRigidbody;
				//component.DefaultTintColor = new Color(0f, 0.45882352941f, 0.02745098039f);
				component8.HasDefaultTint = true;
				component8.baseData.damage = 0;

			}


			


			GameOptions options = new GameOptions();

			options.CurrentLanguage = StringTableManager.GungeonSupportedLanguages.RUBEL_TEST;

			
			ETGModConsole.Log(StringTableManager.EvaluateReplacementToken("%INSULT"));
			ETGModConsole.Log(StringTableManager.GetString("#MASKGUN_ROOMCLEAR_ENMITY"));
			ETGModConsole.Log(StringTableManager.GetString("#MASKGUN_ROOMCLEAR_BEGRUDGING"));
			//Dictionary<string, StringTableManager.StringCollection> diolog = StringTableManager.CoreTable["#MASKGUN_ROOMCLEAR_BEGRUDGING"];
			Dictionary<string, StringTableManager.StringCollection> dialog = new Dictionary<string, StringTableManager.StringCollection>();

			List<string> textList = new List<string>();

			textList = dialog.Select(kvp => kvp.Key).ToList();

			textList = dialog.Keys.ToList();

			int t = 1;

			foreach (string text3 in textList)
			{
				
				ETGModConsole.Log(text3);
				ETGModConsole.Log("" + t);
				t++;
			}


			
			//GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_DEATHS, 1000000f);
			GameStatsManager.Instance.SetStat(TrackedStats.NUMBER_DEATHS, 1000000f);
			Shader shader = Shader.Find("Brave/Internal/GlitchEevee");
			//user.SetOverrideShader(ShaderCache.Acquire("Brave/Internal/GlitchEevee"));
			Camera.main.RenderWithShader(shader, "10");
			Camera.current.RenderWithShader(shader, "10");
			GameManager manager = GameManager.Instance;

			ETGModConsole.Log(user.name, false);
			ETGModConsole.Log(user.ActorName, false);

			
			

			//MoreBreachShrine.roomMagic(user);



			//user.ForceMetalGearMenu = true;
			AkSoundEngine.PostEvent("Play_WPN_earthwormgun_shot_01", base.gameObject);
			//SpriteOutlineManager.RemoveOutlineFromSprite(user.sprite, false);
			//zSpriteOutlineManager.AddOutlineToSprite(user.sprite, Color.red);

			GameManager.PVP_ENABLED = true;

			BulletArmorItem bulletArmor = PickupObjectDatabase.GetById(160).GetComponent<BulletArmorItem>();
			transformSprites = bulletArmor.knightLibrary;
			

			user.SwapToAlternateCostume();
			//Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Rainbow_Chest, npc.sprite.WorldCenter + Vector2.down, npc.sprite.WorldCenter.GetAbsoluteRoom(), true);
			//MoreBreachShrine.roomMagic(user);
			//user.OverrideAnimationLibrary = bulletArmor.knightLibrary;
			//Start();
		}

		Vector2 pos1;
		Vector2 pos2;

		public float rotationSpeed = 10;

		


		int lastClip;

		public tk2dSpriteAnimation transformSprites;

		public float healingAmount;

		public GameObject healVFX;
		tk2dTileMap component;
	}
	public class DoDamage : PickupObject
	{

		private void Start()
		{
			BotsModule.Log("start");
			this.PickupObjectId = idToCopy;
			this.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");

			BotsModule.Log("damaged");
			GameManager.Instance.PrimaryPlayer.healthHaver.ApplyDamage(5f, Vector2.zero, ":P");
		}

		public override void Pickup(PlayerController player)
		{



		}

		public int idToCopy;
	}

	public class FunnySpin : MonoBehaviour
	{
		private void Start()
		{
			if (this.gameObject.GetComponent<PlayerOrbital>() != null)
			{

			}
		}
		public IEnumerator GetOrbitals()
		{
			foreach (var obj in UnityEngine.Object.FindObjectsOfType<FunnySpin>())
			{
				if (obj.gameObject.GetComponent<PlayerOrbital>() != null)
				{
					StartCoroutine(MoveOrbital(obj.gameObject.GetComponent<PlayerOrbital>()));
					yield return new WaitForSeconds(0.3f);
				}

			}
			yield break;
		}

		public IEnumerator MoveOrbital(PlayerOrbital orbital)
		{
			
			while (this.gameObject != null)
			{
				orbital.orbitRadius += 1;
				yield return new WaitForSeconds(1);
				orbital.orbitRadius -= 1;
				yield return new WaitForSeconds(1);
				orbital.orbitRadius -= 1;
				yield return new WaitForSeconds(1);
				orbital.orbitRadius += 1;
				yield return new WaitForSeconds(1);
			}

			yield break;
		}
	}
}
