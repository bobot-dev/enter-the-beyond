using FrostAndGunfireItems;
using GungeonAPI;
using ItemAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using Pathfinding;
using System.Collections;
using Dungeonator;

using CustomCharacters;
using System.IO;
using Ionic.Zip;

using SaveAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
//using AmmonomiconAPI;
using HutongGames.PlayMaker;
using BotsMod.NPCs;
using BotsMod.ToolsAndStuff;
using LiveRecolor;
using NpcApi;
using ChamberGunApi;
using static BotsMod.RandomComps;
using AmmonomiconAPI;
//using ChallengeAPI;

namespace BotsMod
{
    class Commands
    {


        public static Chest KeyChest;

        static dfAtlas testAtlas;

        public static tk2dSpriteAnimation LostAltSkinAnimator;

        protected static AutocompletionSettings _GiveAutocompletionSettings;

        protected static AutocompletionSettings _ItemIdAutocompletionSettings;

        protected static AutocompletionSettings _SpawnAutocompletionSettings;

        public static void Init()
        {

            var devilLootTable = LootTableAPI.LootTableTools.CreateLootTable();
            devilLootTable.AddItemToPool(349);
            devilLootTable.AddItemToPool(279);
            devilLootTable.AddItemToPool(500);
            devilLootTable.AddItemToPool(350);

            //alphabetSoupSynergyProcessor.Entries[];
            _SpawnAutocompletionSettings = new AutocompletionSettings(delegate (string input)
            {
                List<string> list = new List<string>();
                foreach (string text in Game.Enemies.IDs)
                {
                    if (text.AutocompletionMatch(input.ToLower()))
                    {
                        list.Add(text.Replace("gungeon:", ""));
                    }
                }
                return list.ToArray();
            });
            _ItemIdAutocompletionSettings = new AutocompletionSettings(delegate (string input)
            {
                List<string> list = new List<string>();
                foreach (var obj in PickupObjectDatabase.Instance.Objects)
                {
                    if (obj.PickupObjectId.ToString().AutocompletionMatch(obj.PickupObjectId.ToString().ToLower()))
                    {
                        list.Add(obj.PickupObjectId.ToString());
                    }
                }
                return list.ToArray();
            });

            _GiveAutocompletionSettings = new AutocompletionSettings(delegate (string input)
            {
                List<string> list = new List<string>();
                foreach (string text in Game.Items.IDs)
                {
                    if (text.AutocompletionMatch(input.ToLower()))
                    {
                        Console.WriteLine(string.Format("INPUT {0} KEY {1} MATCH!", input, text));
                        list.Add(text.Replace("gungeon:", ""));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("INPUT {0} KEY {1} NO MATCH!", input, text));
                    }
                }
                return list.ToArray();
            });



            ETGModConsole.Commands.AddGroup("bot");

            ETGModConsole.Commands.GetGroup("bot").AddUnit("randomize", delegate (string[] args)
            {
                EtgRandomizerController.Init();
            });



            ETGModConsole.Commands.GetGroup("bot").AddUnit("floorVisualWhateverTheFucks", delegate (string[] args)
            {
                var bundles = new string[]
                {
                        "base_bullethell",
                        "base_castle",
                        "base_catacombs",
                        "base_cathedral",
                        "base_forge",
                        "base_foyer",
                        "base_gungeon",
                        "base_mines",
                        "base_nakatomi",
                        "base_resourcefulrat",
                        "base_sewer",
                        "base_tutorial",
                        "finalscenario_bullet",
                        "finalscenario_convict",
                        "finalscenario_coop",
                        "finalscenario_guide",
                        "finalscenario_pilot",
                        "finalscenario_robot",
                        "finalscenario_soldier"
                };

                foreach (var path in bundles)
                {
                    BotsModule.Log($"---===={DungeonDatabase.GetOrLoadByName(path).name}====---");
                    foreach (var thing in DungeonDatabase.GetOrLoadByName(path).decoSettings.standardRoomVisualSubtypes.elements)
                    {
                        BotsModule.Log($"  {thing.annotation}");
                    }
                }
            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("playerAnimInfo", delegate (string[] args)
            {
                foreach (var clip in GameManager.Instance.PrimaryPlayer.spriteAnimator.Library.clips)
                {
                    BotsModule.Log($"{clip.name} : {clip.wrapMode} [{clip.fps}] - [{clip.frames.Length}]", "#4dff00");
                    for (int i = 0; i < clip.frames.Length; i++)
                    {
                        //if (clip.frames[i].invulnerableFrame)
                        //{
                        //    BotsModule.Log($"{clip.name} {i}: invulnerableFrame", "#4dff00");
                        //}
                        //
                        //if (!clip.frames[i].groundedFrame)
                        //{
                        //    BotsModule.Log($"{clip.name} {i}: groundedFrame", "#ffc400");
                        //}

                        if (!string.IsNullOrEmpty(clip.frames[i].eventAudio))
                        {
                            BotsModule.Log($"{clip.name} {i}: eventAudio: \"{clip.frames[i].eventAudio}\"", "#ffc400");
                        }
                    }
                }
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("setUpUI", delegate (string[] args)
            {

                GUIhandler handler = GameManager.Instance.PrimaryPlayer.gameObject.GetOrAddComponent<GUIhandler>();
                handler.enabled = true;

            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("findTheStupidFuckingThing", delegate (string[] args)
            {
                try
                {
                    foreach (var fuck in UnityEngine.Object.FindObjectsOfType<CharacterSelectFacecardIdleDoer>())
                    {
                        foreach (var comp in fuck.transform.parent.gameObject.GetComponentsInChildren<Component>())
                        {
                            BotsModule.Log($"      ---====[{comp.gameObject.name}]: {comp.GetType()}====---");
                            foreach (PropertyInfo inf in comp.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                            {
                                if (inf.CanRead)
                                {
                                    object val = inf.GetValue(comp, new object[0]);
                                    BotsModule.Log($"  [{inf.Name}]: {val}");
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    BotsModule.Log(ex.ToString());
                }


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("checkLootTable", delegate (string[] args)
            {
                BotsModule.Log($"Number of items in loot table: {StaticSpellReferences.spellLootTable.defaultItemDrops.elements.Count}");
                foreach (var item in StaticSpellReferences.spellLootTable.defaultItemDrops.elements)
                {
                    BotsModule.Log($"[{item.pickupId}]: {item.rawGameObject.name}");
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("ladder", delegate (string[] args)
            {
                var forgeDungeon = DungeonDatabase.GetOrLoadByName("base_nakatomi");
                var hellDrag = forgeDungeon.PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("OFFICE_13_Fire_escape_01")).First().overrideExactRoom.placedObjects
                .Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<UsableBasicWarp>()[0];

                ;


                //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(shopObj.GetComponent<TalkDoerLite>());

                //BotsModule.Log();
                foreach (var item in GameManager.Instance.PrimaryPlayer.CurrentRoom.GetNearbyInteractables(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, 100))
                {

                    BotsModule.Log($"[{item}]");
                    foreach (var child in (item as TalkDoerLite).transform)
                    {
                        if (child != null)
                            BotsModule.Log($"[{child}]");
                    }
                }

            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("floorRoomCount", delegate (string[] args)
            {

                string[] BundlePrereqs = new string[]
                {
                        "base_bullethell",
                        "base_castle",
                        "base_catacombs",
                        "base_cathedral",
                        "base_forge",
                        "base_foyer",
                        "base_gungeon",
                        "base_mines",
                        "base_nakatomi",
                        "base_resourcefulrat",
                        "base_sewer",
                        "base_tutorial",
                        "finalscenario_bullet",
                        "finalscenario_convict",
                        "finalscenario_coop",
                        "finalscenario_guide",
                        "finalscenario_pilot",
                        "finalscenario_robot",
                        "finalscenario_soldier"
                };
                foreach (string floorName in BundlePrereqs)
                {
                    foreach (var flow in DungeonDatabase.GetOrLoadByName(floorName).PatternSettings.flows)
                    {
                        BotsModule.Log($"[{floorName}] {flow.name}: {flow.AllNodes.Count}");
                    }

                }

            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("cgRoomTest", delegate (string[] args)
            {
                RoomHandler absoluteRoom = GameManager.Instance.PrimaryPlayer.transform.position.GetAbsoluteRoom();
                //var targetRoom = BraveUtility.RandomElement<WeightedRoom>(BeyondPrefabs.CastleRoomTable.includedRooms.elements).room;
                var targetRoom = (PickupObjectDatabase.GetById(625) as PaydayDrillItem).GenericFallbackCombatRoom;

                GameManager.Instance.Dungeon.StartCoroutine((new CgController { DelayPreExpansion = 2.5f, DelayPostExpansionPreEnemies = 2 }).HandleSeamlessTransitionToCombatRoom(absoluteRoom, targetRoom));

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("cgRoomTestEnd", delegate (string[] args)
            {
                CgController.aaaa = 4;

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("blockthisoverhead", delegate (string[] args)
            {
                //var overVfx = new GameObject("deez nuts");
                //var bar = new dfProgressBarIsShit("BotsMod/sprites/VFX/chained_full.png", "BotsMod/sprites/VFX/chained_empty.png");

                //backgroundSprite = Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(BackgroundSprite)).name;
                //progressSprite = Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(ProgressSprite)).name;

                //bar.transform.parent = GameUIRoot.Instance.p_playerReloadBar.statusBarPoison.transform.parent;

                GameUIRoot.Instance.p_playerReloadBar.statusBarPoison = dfProgressBarIsShit.Init("BotsMod/sprites/VFX/chained_full.png", "BotsMod/sprites/VFX/chained_empty.png");


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("addSpellSlots", delegate (string[] args)
            {

                if (GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.GetComponent<Wand>() != null)
                {
                    GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.GetComponent<Wand>().spellSlots += int.Parse(args[0]);
                    BotsModule.Log($"Added {int.Parse(args[0])} Slots to {GameManager.Instance.PrimaryPlayer.CurrentGun.EncounterNameOrDisplayName}.");
                }
                else
                {
                    BotsModule.Log("Please Hold a Wand");
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("setTimeScale", delegate (string[] args)
            {
                Time.timeScale = float.Parse(args[0]);

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("playanimation", delegate (string[] args)
            {
                GameManager.Instance.PrimaryPlayer.gameObject.GetComponent<CustomCharacter>().overrideAnimation = args[0];
                GameManager.Instance.PrimaryPlayer.spriteAnimator.Play(args[0]);

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("magnificence_check", delegate (string[] args)
            {
                BotsModule.Log($"Generated: {GameManager.Instance.Dungeon.GeneratedMagnificence}");
                BotsModule.Log($"Heart: {RewardManager.AdditionalHeartTierMagnificence}");
                BotsModule.Log($"Floor + Normal: {GameManager.Instance.PrimaryPlayer.stats.Magnificence}");
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("addCoinToUi", delegate (string[] args)
            {
                foreach (var item in PickupObjectDatabase.Instance.Objects)
                {

                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("dumpPlayerSprites", delegate (string[] args)
            {
                CollectionDumper.DumpCollection(GameManager.Instance.PrimaryPlayer.sprite.Collection);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("excludedFromShops", delegate (string[] args)
            {
                foreach (var item in PickupObjectDatabase.Instance.Objects)
                {
                    if (item != null && item is PassiveItem && (item as PassiveItem).ItemRespectsHeartMagnificence)
                    {
                        BotsModule.Log("respect " + item.EncounterNameOrDisplayName);
                    }

                    if (item != null && item.ItemSpansBaseQualityTiers)
                    {
                        BotsModule.Log("span " + item.EncounterNameOrDisplayName);
                    }
                }
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("dumpStuff", delegate (string[] args)
            {
                CollectionDumper.DumpCollection((PickupObjectDatabase.GetById(542) as Gun).DefaultModule.projectiles[0].gameObject.GetComponent<StrafeBleedBuff>().vfx.gameObject.GetComponent<tk2dSprite>().Collection);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("masterRound", delegate (string[] args)
            {
                BotsModule.Log(GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun.ToString());
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("setPerks", delegate (string[] args)
            {

                GameManager.Instance.PrimaryPlayer.CurrentGun.GetComponent<TheMessanger>().firstPerk = TheMessanger.FirstSlotPerks.MovingTarget;
                GameManager.Instance.PrimaryPlayer.CurrentGun.GetComponent<TheMessanger>().secondPerk = TheMessanger.SecondSlotPerks.Desperado;
            });

            ETGModConsole.Commands.AddUnit("exportPlayerTexture", delegate (string[] args)
            {
                File.WriteAllBytes(ETGMod.ResourcesDirectory + GameManager.Instance.PrimaryPlayer.characterIdentity.ToString() + "spritesheet.png", ((Texture2D)GameManager.Instance.PrimaryPlayer.sprite.renderer.material.GetTexture("_MainTex")).EncodeToPNG());
            });

            ETGModConsole.Commands.AddUnit("getAllInteractablesInRoom", delegate (string[] args)
            {
                foreach (var obj in GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRoomInteractables())
                {
                    BotsModule.Log(obj.ToString());
                    BotsModule.Log(obj.GetDistanceToPoint(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter).ToString());
                }
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("npc", delegate (string[] args)
            {
                BotsModule.Log("0");
                if (GameManager.Instance.PrimaryPlayer.CurrentRoom == null)
                {
                    BotsModule.Log("shop did a null");
                }
                foreach (var australian in Tools.shared_auto_002.LoadAsset<DungeonPlaceable>("Generic Jailed NPC").variantTiers)
                {
                    var shopObj = DungeonPlaceableUtility.InstantiateDungeonPlaceable(australian.nonDatabasePlaceable, GameManager.Instance.PrimaryPlayer.CurrentRoom, ((GameManager.Instance.PrimaryPlayer.CurrentRoom.area.UnitCenter + new Vector2(UnityEngine.Random.Range(8, -8), UnityEngine.Random.Range(8, -8)))
                        - GameManager.Instance.PrimaryPlayer.CurrentRoom.area.basePosition.ToVector2()).ToIntVector2(), false);
                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(shopObj.GetComponent<TalkDoerLite>());
                }




                //
                //File.WriteAllText("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Enter the Gungeon\\Resources\\EnterTheBeyond/fuckingdie.txt", UnityEngine.JsonUtility.ToJson(shopObj.GetComponent<PlayMakerFSM>(), true));


                BotsModule.Log("2");
                return;
                var room = RoomFactory.BuildFromResource("BotsMod/rooms/customnpctest.room");


                RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.STANDARD);

                Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions + new IntVector2(10, 10));



                GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(2, 4)).ToVector2(), false, false);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("getItemInfo", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }



                //var obj = Tools.LoadAssetFromAnywhere(args[0].Replace("_", " "));

                var obj = Tools.brave.LoadAsset<GameObject>(args[0]);

                if (obj != null)
                {
                    foreach (var comp in (obj as GameObject).GetComponents<Component>())
                    {
                        BotsModule.Log($"{comp.GetType()}: {comp.name}");
                    }
                }
                else
                {
                    BotsModule.Log("no object found under \"" + args[0].Replace("_", " ") + "\"");
                }

            });



            ETGModConsole.Commands.GetGroup("bot").AddUnit("past_kill", delegate (string[] args)
            {

                GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST, true);
                GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME, true);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("past_killnt", delegate (string[] args)
            {

                GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST, false);
            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("past", delegate (string[] args)
            {

                GameManager.Instance.LoadCustomLevel("botfs_lost");
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("model", delegate (string[] args)
            {

                var obj = UnityEngine.Object.Instantiate(BeyondPrefabs.BotsAssetBundle.LoadAsset<GameObject>("TestModel"), GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, new Quaternion());

                obj.transform.localScale = new Vector3(3, 5, 5);

                obj.AddComponent<RandomComps.MakeObjSpin>();
            });



            ETGModConsole.Commands.GetGroup("bot").AddUnit("AddPartsToEnemies", delegate (string[] args)
            {

                LightningRounds.ApplyActionToNearbyEnemiesWithALimit(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter, 10, 30, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
                {

                    if (enemy && enemy.healthHaver)
                    {

                        var partSystem = UnityEngine.Object.Instantiate(BeyondPrefabs.BotsAssetBundle.LoadAsset<GameObject>("ParticleSystemObj"));
                        partSystem.transform.position = enemy.sprite.WorldCenter;
                        partSystem.transform.parent = enemy.transform;
                        partSystem.GetComponent<ParticleSystem>().gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
                        BotsModule.Log(enemy.name);

                    }


                });
            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("burn", delegate (string[] args)
            {

                LightningRounds.ApplyActionToNearbyEnemiesWithALimit(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter, 10, 30, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All), delegate (AIActor enemy, float dist)
                {

                    if (enemy && enemy.healthHaver)
                    {
                        //-2510877616333010786
                        //-4066334418742256460
                        enemy.ApplyEffect(new CustomGameActorFireEffect
                        {
                            AffectsEnemies = true,
                            AffectsPlayers = false,
                            AppliesDeathTint = true,
                            AppliesOutlineTint = false,
                            AppliesTint = true,
                            PlaysVFXOnActor = false,
                            DamagePerSecondToEnemies = 2,
                            DeathTintColor = new Color(0.388235331f, 0.388235331f, 0.388235331f),//new Color32(128, 0, 128, 255),
                            TintColor = new Color32(0, 157, 255, 255),
                            duration = 10,
                            effectIdentifier = "BeyondFire",
                            flameBuffer = new Vector2(0.0625f, 0.3f),
                            flameFpsVariation = 15,
                            flameMoveChance = 1,

                        });

                    }


                });
            });



            ETGModConsole.Commands.GetGroup("bot").AddUnit("breachRoom", delegate (string[] args)
            {
                var orLoadByName_Orig5 = SpecialDungeon.GetOrLoadByNameOrig("Base_ResourcefulRat");
                var room = RoomFactory.BuildFromResource("BotsMod/rooms/breachRoom.room");
                room.overriddenTilesets = GlobalDungeonData.ValidTilesets.HELLGEON;
                RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);



                Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);
                GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(3, 4)).ToVector2(), false, false);

                //UnityEngine.Object.Instantiate(SpriteBuilder.SpriteFromResource("BotsMod/sprites/TestBreachRoomTexture"), new Vector3(creepyRoom.area.basePosition.x, creepyRoom.area.basePosition.y), Quaternion.identity);
            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("beyond", delegate (string[] args)
            {

                GameManager.Instance.LoadCustomLevel("beyond");
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("randomize_gun", delegate (string[] args)
            {

                CompletlyRandomGun.DoRandomizeGun(CompletlyRandomGun.completlyRandomGun);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("shield", delegate (string[] args)
            {

                //var shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                //var shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1,1,1), Quaternion.identity);
                var shield3 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1, -1, -1), Quaternion.identity);
                //shield2.gameObject.AddComponent<ShiledShader1>();
                //shield3.gameObject.AddComponent<ShiledShader2>();




                var stupidfuckingshopleasedie = shield3.gameObject.AddComponent<BaseShopController>();


                stupidfuckingshopleasedie.baseShopType = BaseShopController.AdditionalShopType.NONE;
                stupidfuckingshopleasedie.ShopCostModifier = 1000;
                stupidfuckingshopleasedie.shopItemsGroup2 = Tools.LoadShopTable("Shop_Curse_Items_01");
                stupidfuckingshopleasedie.shopItems = Tools.LoadShopTable("Shop_Blank_Items_01");
                stupidfuckingshopleasedie.spawnGroupTwoItem2Chance = 1;

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("time", delegate (string[] args)
            {
                BotsModule.Log(StringTableManager.EvaluateReplacementToken("%PLAYTIMEHOURS"));

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("checkNails", delegate (string[] args)
            {
                BotsModule.Log(NailMagnet.magnets.Count.ToString());
                foreach (var mag in NailMagnet.magnets)
                {
                    BotsModule.Log(mag.transform.position.ToString());
                }


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("refreshOutline", delegate (string[] args)
            {

                SpriteOutlineManager.RemoveOutlineFromSprite(GameManager.Instance.PrimaryPlayer.sprite);
                SpriteOutlineManager.AddOutlineToSprite(GameManager.Instance.PrimaryPlayer.sprite, GameManager.Instance.PrimaryPlayer.outlineColor, 0.1f, 0.5f, SpriteOutlineManager.OutlineType.NORMAL);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("refreshOutline2", delegate (string[] args)
            {

                SpriteOutlineManager.RemoveOutlineFromSprite(GameManager.Instance.PrimaryPlayer.sprite);
                SpriteOutlineManager.AddOutlineToSprite(GameManager.Instance.PrimaryPlayer.sprite, GameManager.Instance.PrimaryPlayer.outlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.EEVEE);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("spawnitems", delegate (string[] args)
            {

                var chest = Chest.Spawn(KeyChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));

                var contents = new List<PickupObject>
                    {
                        PickupObjectDatabase.GetById(145),
                        PickupObjectDatabase.GetById(478),
                        PickupObjectDatabase.GetById(551),
                        PickupObjectDatabase.GetById(467),
                    };

                var pos = new List<Vector3>
                    {
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(1,0),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(-1,0),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(0,1),
                        GameManager.Instance.PrimaryPlayer.sprite.WorldCenter + new Vector2(0,-1),
                    };

                List<DebrisObject> list = new List<DebrisObject>();

                for (int i = 0; i < contents.Count; i++)
                {
                    List<DebrisObject> list2 = LootEngine.SpewLoot(new List<GameObject> { contents[i].gameObject }, pos[i]);
                    list.AddRange(list2);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        if (list2[j])
                        {
                            list2[j].PreventFallingInPits = true;
                        }
                        if (!(list2[j].GetComponent<Gun>() != null))
                        {
                            if (!(list2[j].GetComponent<CurrencyPickup>() != null))
                            {
                                if (list2[j].specRigidbody != null)
                                {
                                    list2[j].specRigidbody.CollideWithOthers = false;
                                    DebrisObject debrisObject = list2[j];
                                    debrisObject.OnTouchedGround += (Action<DebrisObject>)typeof(Chest).GetMethod("BecomeViableItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(chest, new object[] { debrisObject });
                                    //debrisObject.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnTouchedGround, new Action<DebrisObject>(this.BecomeViableItem));
                                }
                            }
                        }
                    }
                }

            });



            ETGModConsole.Commands.GetGroup("bot").AddUnit("doFunnyShader", delegate (string[] args)
            {
                Material[] array = GameManager.Instance.PrimaryPlayer.SetOverrideShader(ShaderCache.Acquire("Brave/Internal/FinalGunRoom_BG_02"));
                for (int i = 0; i < array.Length; i++)
                {
                    if (!(array[i] == null))
                    {
                        //

                        array[i].SetTexture("_ThunderProgress", Tools.brave.LoadAsset<Texture2D>("testgrad4"));
                        array[i].SetFloat("_Octaves", 10f);
                        array[i].SetFloat("_Frequency", 1.5f);
                        array[i].SetFloat("_Amplitude", 1f);
                        array[i].SetFloat("_Lacunarity", 1.5f);
                        array[i].SetFloat("_Lacunarity", 1f);
                        array[i].SetFloat("_Persistence", 0.7f);
                        array[i].SetFloat("_SteppyStep", 0.1f);
                        array[i].SetFloat("_ThunderProgress", 0f);
                        array[i].SetVector("_Offset", Vector4.zero);

                    }
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("findshops", delegate (string[] args)
            {
                foreach (var shop in UnityEngine.Object.FindObjectsOfType<BaseShopController>())
                {
                    BotsModule.Log(shop.gameObject.name + " has been found at " + shop.gameObject.transform.position);

                    foreach (Transform child in shop.gameObject.transform)
                    {
                        BotsModule.Log(child.name + ": " + child.gameObject.GetType() + " (" + child.gameObject.activeInHierarchy + ")");
                    }

                }

                BotsModule.Log("==========================");
                foreach (var shop in StaticReferenceManager.AllShops)
                {
                    BotsModule.Log(shop.name);
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("listlibraries", delegate (string[] args)
            {
                if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee)
                {
                    foreach (var libary in GameManager.Instance.PrimaryPlayer.GetComponent<CharacterAnimationRandomizer>().AnimationLibraries)
                    {
                        BotsModule.Log($"{libary}");
                    }
                }
                else
                {
                    BotsModule.Log("This command requires you are playing paradox");
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("flytest", delegate (string[] args)
            {
                var room = RoomFactory.BuildFromResource("BotsMod/rooms/robotflytestroom.room");
                RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);

                Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



                GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("FoyerSelectThing", delegate (string[] args)
            {
                foreach (var foyerSelectFlag in UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>())
                {
                    if (!foyerSelectFlag.name.ToLower().Contains("convict"))
                    {
                        continue;
                    }
                    BotsModule.Log($"============[{foyerSelectFlag.name}: Start]============");

                    foreach (var comp in foyerSelectFlag.GetComponents<Component>())
                    {
                        BotsModule.Log($"=========[{comp.name}: {comp.GetType()}]=========");


                        PropertyInfo[] properties = comp.GetType().GetProperties();

                        foreach (PropertyInfo propertyInfo in properties)
                        {
                            string text = propertyInfo.DeclaringType.FullName + "::" + propertyInfo.Name;
                            if (propertyInfo.MemberType != MemberTypes.Method && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
                            {

                                try
                                {
                                    object value = ReflectionHelper.GetValue(propertyInfo, comp);
                                    BotsModule.Log(propertyInfo.Name + ": " + value.ToStringIfNoString());
                                }
                                catch (Exception message)
                                {
                                    Debug.LogWarning("FoyerSelectThing: THIS LITTLE SHIT BROKE IT > " + text);
                                    Debug.LogWarning(message);
                                }
                            }
                        }
                    }

                    BotsModule.Log($"============[{foyerSelectFlag.name}: End]============");

                }
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("KillPillarsAreDumb", delegate (string[] args)
            {

                foreach (var comp in EnemyDatabase.GetOrLoadByGuid("3f11bbbc439c4086a180eb0fb9990cb4").GetComponent<AIActorDummy>().realPrefab.GetComponentsInChildren<Component>())
                {
                    BotsModule.Log($"=========[{comp.name}: {comp.GetType()}]=========");


                    /*PropertyInfo[] properties = comp.GetType().GetProperties();

                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        string text = propertyInfo.DeclaringType.FullName + "::" + propertyInfo.Name;
                        if (propertyInfo.MemberType != MemberTypes.Method && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead)
                        {

                            try
                            {
                                object value = ReflectionHelper.GetValue(propertyInfo, comp);
                                BotsModule.Log(propertyInfo.Name + ": " + value.ToStringIfNoString());
                            }
                            catch (Exception message)
                            {
                                Debug.LogWarning("KillPillarsAreDumb: THIS LITTLE SHIT BROKE IT > " + text);
                                Debug.LogWarning(message);
                            }
                        }
                    }*/
                }



            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("shop", delegate (string[] args)
            {



                var devilLootTable2 = UnityEngine.Object.Instantiate(Tools.shared_auto_001.LoadAsset<GenericLootTable>("Shop_Key_Items_01"));
                devilLootTable2.defaultItemDrops.elements.Clear();

                devilLootTable2.AddItemToPool(60);
                devilLootTable2.AddItemToPool(125);
                devilLootTable.AddItemToPool(434);
                devilLootTable2.AddItemToPool(271);
                devilLootTable2.AddItemToPool(407);
                devilLootTable2.AddItemToPool(571);
                devilLootTable2.AddItemToPool(33);
                devilLootTable2.AddItemToPool(17);
                devilLootTable2.AddItemToPool(347);
                devilLootTable2.AddItemToPool(90);
                devilLootTable2.AddItemToPool(336);
                devilLootTable2.AddItemToPool(285);

                var room = RoomFactory.BuildFromResource("BotsMod/rooms/npctest.room");


                RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(room, null, DungeonData.LightGenerationStyle.FORCE_COLOR);

                Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);



                GameManager.Instance.PrimaryPlayer.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

                foreach (var shop in UnityEngine.Object.FindObjectsOfType<BaseShopController>())
                {
                    BotsModule.Log(shop.gameObject.name);
                    if (shop.gameObject.name.Contains("Merchant_Key"))
                    {
                        BotsModule.Log("found");
                        shop.baseShopType = (BaseShopController.AdditionalShopType)CustomEnums.CustomAdditionalShopType.DEVIL_DEAL;

                        shop.shopItems = devilLootTable;

                    }

                }


                return;
                //GameObject asset = null;
                //foreach (var bundle in StaticReferences.AssetBundles.Values)
                //{
                //    asset = bundle.LoadAsset<GameObject>("merchant_rat_placeable");
                //    if (asset)
                //       break;
                //}

                //var shield1 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                //var shield2 = UnityEngine.Object.Instantiate(BotsModule.overseerShield, GameManager.Instance.PrimaryPlayer.gameObject.transform.position + new Vector3(-1,1,1), Quaternion.identity);
                //var dumbFuckingRodent = BraveResources.Load<GameObject>("Merchant_Rat");

                var sharedAssets1 = ResourceManager.LoadAssetBundle("shared_auto_001");
                var asset = sharedAssets1.LoadAsset<GameObject>("Merchant_Key");





                asset.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(asset);
                UnityEngine.Object.DontDestroyOnLoad(asset);

                UnityEngine.Object.Instantiate(asset, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);
                //ToolsCharApi.Print($"Added {asset.name} to room.");


                GameObject dumbFuckingRodent = UnityEngine.Object.Instantiate(asset);
                dumbFuckingRodent.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodent);
                UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodent);

                UnityEngine.Object.Instantiate(dumbFuckingRodent, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);

                return;
                dumbFuckingRodent.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodent);
                UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodent);

                UnityEngine.Object.DestroyImmediate(dumbFuckingRodent.GetComponent<BaseShopController>());

                var dumbFuckingRodentsDumbFuckingShop = dumbFuckingRodent.AddComponent<BaseShopController>();

                var itemPos1 = new GameObject("EAST item pos");
                itemPos1.transform.localPosition = new Vector3(1.125f, 2.375f, 1f);
                itemPos1.transform.position = new Vector3(1.125f, 2.375f, 1f);
                itemPos1.transform.localScale = new Vector3(1f, 1f, 1f);



                var itemPos2 = new GameObject("WEST item pos");
                itemPos2.transform.localPosition = new Vector3(2.625f, 1f, 1f);
                itemPos2.transform.position = new Vector3(1.125f, 2.375f, 1f);
                itemPos2.transform.localScale = new Vector3(1f, 1f, 1f);


                var itemPos3 = new GameObject("NORTH item pos");
                itemPos3.transform.localPosition = new Vector3(4.125f, 2.375f, 1f);
                itemPos3.transform.position = new Vector3(1.125f, 2.375f, 1f);
                itemPos3.transform.localScale = new Vector3(1f, 1f, 1f);

                dumbFuckingRodentsDumbFuckingShop.placeableHeight = 5;
                dumbFuckingRodentsDumbFuckingShop.placeableWidth = 5;
                dumbFuckingRodentsDumbFuckingShop.difficulty = 0;
                dumbFuckingRodentsDumbFuckingShop.isPassable = true;
                dumbFuckingRodentsDumbFuckingShop.baseShopType = BaseShopController.AdditionalShopType.KEY;
                dumbFuckingRodentsDumbFuckingShop.FoyerMetaShopForcedTiers = false;
                dumbFuckingRodentsDumbFuckingShop.BeetleExhausted = false;
                dumbFuckingRodentsDumbFuckingShop.ExampleBlueprintPrefab = null;
                dumbFuckingRodentsDumbFuckingShop.shopItemsGroup2 = null;


                //BotsMod/JSONShit/PlayMakerFSM #11743.json
                var playMakerFSMTest = File.ReadAllText(ETGMod.ResourcesDirectory + "/EnterTheBeyond/PlayMakerFSM #11743.json");

                //BotsModule.Log(text, BotsModule.TEXT_COLOR);

                PlayMakerFSM playMakerFSM = new PlayMakerFSM();

                JsonUtility.FromJsonOverwrite(playMakerFSMTest, playMakerFSM);

                dumbFuckingRodentsDumbFuckingShop.shopkeepFSM = playMakerFSM;

                //dumbFuckingRodentsDumbFuckingShop.shopItems = LootTables.testShopLable;

                dumbFuckingRodentsDumbFuckingShop.shopItems = Tools.LoadShopTable("Shop_Curse_Items_01");
                dumbFuckingRodentsDumbFuckingShop.spawnPositions = new Transform[]
                {
                        itemPos1.transform,
                        itemPos2.transform,
                        itemPos3.transform,

                };
                //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem1Chance = 0.5f;
                //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem2Chance = 0.5f;
                //dumbFuckingRodentsDumbFuckingShop.spawnGroupTwoItem3Chance = 0.5f;

                dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cloak", new GameObject("StupidFuckingShadowCunt"));
                dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab);
                UnityEngine.Object.DontDestroyOnLoad(dumbFuckingRodentsDumbFuckingShop.shopItemShadowPrefab);


                dumbFuckingRodentsDumbFuckingShop.ShopCostModifier = 1000;

                itemPos1.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos1);
                UnityEngine.Object.DontDestroyOnLoad(itemPos1);

                itemPos2.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos2);
                UnityEngine.Object.DontDestroyOnLoad(itemPos2);

                itemPos3.SetActive(false);
                ItemAPI.FakePrefab.MarkAsFakePrefab(itemPos3);
                UnityEngine.Object.DontDestroyOnLoad(itemPos3);

                var swapper = UnityEngine.Object.Instantiate(dumbFuckingRodent, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity).GetComponent<BaseShopController>();
                int i = 0;
                foreach (Transform transform in swapper.spawnPositions)
                {
                    if (transform == null)
                    {
                        BotsModule.Log(i + " : is null ;-;");
                    }
                    else if (transform != null)
                    {
                        BotsModule.Log(i + " : isn't null :)");
                    }
                    i++;
                }

                //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();
                //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("nomorebadguys", delegate (string[] args)
            {
                var user = GameManager.Instance.PrimaryPlayer;

                if (user.CurrentRoom != null)
                {
                    List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

                    foreach (AIActor enemy in activeEnemies)
                    {
                        if (enemy != null)
                        {
                            enemy.CanTargetPlayers = false;
                        }

                    }
                }



            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("spawn", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1, 2))
                {
                    return;
                }
                if (!GameManager.Instance.PrimaryPlayer)
                {
                    ETGModConsole.Log("Couldn't access Player Controller", false);
                    return;
                }
                string text = args[0];
                if (!Game.Items.ContainsID(text))
                {
                    ETGModConsole.Log(string.Format("Invalid item ID {0}!", text), false);
                    return;
                }
                ETGModConsole.Log(string.Concat(new object[]
                {
                        "Attempting to spawn item ID ",
                        args[0],
                        " (numeric ",
                        text,
                        "), class ",
                        Game.Items.Get(text).GetType()
                }), false);
                if (args.Length == 2)
                {
                    int num = int.Parse(args[1]);
                    for (int i = 0; i < num; i++)
                    {
                        IPlayerInteractable[] interfacesInChildren2 = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(LootEngine.SpawnItem(Game.Items[text].gameObject, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Vector2.zero, 0).gameObject);
                        for (int j = 0; j < interfacesInChildren2.Length; j++)
                        {
                            GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren2[j]);
                        }


                    }
                    return;
                }
                var gameObject2 = LootEngine.SpawnItem(Game.Items[text].gameObject, GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Vector2.zero, 0);
                IPlayerInteractable[] interfacesInChildren = GameObjectExtensions.GetInterfacesInChildren<IPlayerInteractable>(gameObject2.gameObject);
                for (int i = 0; i < interfacesInChildren.Length; i++)
                {
                    GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(interfacesInChildren[i]);
                }

            }, _GiveAutocompletionSettings);

            ETGModConsole.Commands.GetGroup("bot").AddUnit("lost_unlocks", delegate (string[] args)
            {

                foreach (var stat in GameStatsManager.Instance.m_characterStats)
                {
                    BotsModule.Log($"{stat.Key}");
                }

                BotsModule.Log("Killed Past: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST), BotsModule.LOST_COLOR);
                BotsModule.Log("Killed Past Alt: " + GameStatsManager.Instance.GetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME), BotsModule.LOST_COLOR);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("swapperActive", delegate (string[] args)
            {
                try
                {

                    foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<tk2dSprite>())
                    {
                        if (dumbpeiceofshit.gameObject.name.Contains("costume"))
                        {
                            BotsModule.Log($"{dumbpeiceofshit.gameObject.name}: {dumbpeiceofshit.gameObject.transform.position}");
                            if (dumbpeiceofshit.gameObject.activeSelf == false)
                            {

                                dumbpeiceofshit.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    BotsModule.Log("swapper dump broke", "#eb1313");
                    BotsModule.Log(string.Format(e + ""), "#eb1313");
                }
            });
            /*
             * 

             * 
             * 
            */

            ETGModConsole.Commands.GetGroup("bot").AddUnit("doReloadThing", delegate (string[] args)
            {

                var reloadText = GameManager.Instance.PrimaryPlayer.CurrentGun.gameObject.AddComponent<CustomReloadText>();

                reloadText.customReloadMessage = "deez nuts";

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("ihateunity", delegate (string[] args)
            {
                int i = 0;
                foreach (var comp in GameManager.Instance.PrimaryPlayer.gameObject.GetComponentsInChildren<MonoBehaviour>())
                {
                    BotsModule.Log(comp.GetType().ToString());
                    try
                    {
                        var path = "C:\\Users\\noaht\\Documents\\unity\\stop crashing pls\\Assets\\fuck\\" + i + comp.GetType().ToString() + ".txt";

                        if (!File.Exists(path))
                        {
                            var f = File.Create(path);
                            f.Close();
                        }
                        File.WriteAllText(path, UnityEngine.JsonUtility.ToJson(comp));
                        i++;
                    }
                    catch (Exception e)
                    {
                        BotsModule.Log(e.ToString());
                    }



                }

            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("findgun", delegate (string[] args)
            {


                foreach (var dumbpeiceofshit in UnityEngine.Object.FindObjectsOfType<DebrisObject>())
                {
                    if (dumbpeiceofshit.name.ToLower().Contains("gun"))
                    {
                        BotsModule.Log($"{dumbpeiceofshit.name}");
                        //GetComponentInChildren
                        foreach (var comp in dumbpeiceofshit.GetComponents<Component>())
                        {
                            BotsModule.Log($"=========[{comp.name}: {comp.GetType()}]=========");
                        }
                        BotsModule.Log($"children comps:");


                        foreach (var comp in dumbpeiceofshit.GetComponentsInChildren<Component>())
                        {
                            BotsModule.Log($"=========[{comp.name}: {comp.GetType()}]=========");
                        }

                    }


                }

            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("findfoyershit", delegate (string[] args)
            {


                foreach (var a in UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>())
                {
                    BotsModule.Log("From the foreach for array: " + a.name);
                }

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("findthenode", delegate (string[] args)
            {

                int i = 0;
                foreach (var room in GungeonAPI.OfficialFlows.GetDungeonPrefab("base_resourcefulrat").PatternSettings.flows[0].AllNodes[18].overrideRoomTable.includedRooms.elements)
                {
                    BotsModule.Log(room.room.name);
                    if (room.room.placedObjects != null)
                    {
                        foreach (var placeable in room.room.placedObjects)
                        {
                            if (placeable.nonenemyBehaviour != null)
                            {
                                BotsModule.Log(placeable.nonenemyBehaviour.name);
                            }

                        }
                    }
                }
                //BotsModule.Log("aaaa");
                foreach (var node in GungeonAPI.OfficialFlows.GetDungeonPrefab("base_resourcefulrat").PatternSettings.flows[0].AllNodes)
                {
                    if (node.overrideExactRoom != null && node.overrideExactRoom.placedObjects != null)
                    {
                        foreach (var placeable in node.overrideExactRoom.placedObjects)
                        {
                            if (placeable.nonenemyBehaviour != null)
                            {
                                BotsModule.Log(placeable.nonenemyBehaviour.name);
                            }

                        }
                    }
                }



            });



            

            ETGModConsole.Commands.GetGroup("bot").AddUnit("listitemchance", delegate (string[] args)
            {
                for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
                {
                    if (PickupObjectDatabase.Instance.Objects[i] != null)
                    {
                        //GetItemForPlayer
                        var player = GameManager.Instance.PrimaryPlayer;
                        if (PickupObjectDatabase.Instance.Objects[i] is Gun)
                        {
                            var gunClass = (PickupObjectDatabase.Instance.Objects[i] as Gun).gunClass;
                            int num4 = (!(player == null)) ? player.inventory.ContainsGunOfClass(gunClass, true) : 0;
                            float modifierForClass = LootDataGlobalSettings.Instance.GetModifierForClass(gunClass);
                            ETGModConsole.Log($"Name: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].EncounterNameOrDisplayName}</color>, Id: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].PickupObjectId}</color>, Chance: <color={"#03fc0b"}>{RewardManager.GetMultiplierForItem(PickupObjectDatabase.Instance.Objects[i], player, false)}/{Mathf.Pow(modifierForClass, (float)num4)}</color>, Class: <color={"#03fc0b"}>{gunClass}</color>");

                        }
                        else
                        {

                            ETGModConsole.Log($"Name: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].EncounterNameOrDisplayName}</color>, Id: <color={"#03fc0b"}>{PickupObjectDatabase.Instance.Objects[i].PickupObjectId}</color>, Chance: <color={"#03fc0b"}>{RewardManager.GetMultiplierForItem(PickupObjectDatabase.Instance.Objects[i], player, false)}</color>");
                        }




                    }
                }



            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("crime", delegate (string[] args)
            {
                AssetBundle assetBundle3 = ResourceManager.LoadAssetBundle("foyer_002");
                var idk = assetBundle3.LoadAsset<GameObject>("costume_bullet_shotgun_psspecial_alt");
                var swapper = UnityEngine.GameObject.Instantiate(idk, GameManager.Instance.PrimaryPlayer.sprite.WorldTopCenter, Quaternion.identity);
                //swapper.CostumeSprite = swapper.gameObject.GetComponent<tk2dSprite>();
                //swapper.AlternateCostumeSprite = SpriteBuilder.SpriteFromResource("BotsMod/sprites/cosmic_sludge").GetComponent<tk2dSprite>();

                GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(swapper.GetComponent<CharacterCostumeSwapper>());
                BotsModule.Log("Position: " + swapper.transform.position);

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("audioSwitch", delegate (string[] args)
            {
                var player = GameManager.Instance.PrimaryPlayer;
                player.OverridePlayerSwitchState = args[0];
                ETGModConsole.Log(args[0]);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("room", delegate (string[] args)
            {
                BotsModule.Log(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRoomName(), BotsModule.TEXT_COLOR);


            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("chest", delegate (string[] args)
            {
                Chest.Spawn(KeyChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));
            });


            ETGModConsole.Commands.GetGroup("bot").AddUnit("giveSoulHeart", delegate (string[] args)
            {
                SoulHeartController.soulHeartCount++;
                //GameManager.Instance.PrimaryPlayer.healthHaver.SetHealthMaximum(GameManager.Instance.PrimaryPlayer.healthHaver.GetMaxHealth() + 1);
                GameUIRoot.Instance.UpdatePlayerHealthUI(GameManager.Instance.PrimaryPlayer, GameManager.Instance.PrimaryPlayer.healthHaver);
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("item_info", new Action<string[]>(ItemInfo), _ItemIdAutocompletionSettings);
            ETGModConsole.Commands.GetGroup("bot").AddUnit("enemy_info", new Action<string[]>(EnemyInfo), _SpawnAutocompletionSettings);



            ETGModConsole.Commands.GetGroup("bot").AddUnit("quickstartinfo", delegate (string[] args)
            {
                GameManager.Options.LastPlayedCharacter = (PlayableCharacters)CustomPlayableCharacters.Lost;
                BotsModule.Log(GameManager.Options.LastPlayedCharacter.ToString());
                BotsModule.Log(GameManager.Options.PreferredQuickstartCharacter.ToString());
                BotsModule.Log(GameManager.LastUsedPlayerPrefab.name);

                //BotsModule.Log(GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>().characterIdentity.ToString());
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("docool", delegate (string[] args)
            {
                GameManager.Instance.Dungeon.roomMaterialDefinitions[GameManager.Instance.PrimaryPlayer.CurrentRoom.RoomVisualSubtype] = BeyondPrefabs.shared_auto_002.LoadAsset<DungeonMaterial>("Boss_Cathedral_StainedGlass_Lights");
            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("exportFloorToPng", delegate (string[] args)
            {
                GungeonAPI.Tools.LogDungeonToPNGFile();

            });

            ETGModConsole.Commands.GetGroup("bot").AddUnit("exportRoomToPng", delegate (string[] args)
            {
                GungeonAPI.Tools.LogRoomHandlerToPNGFile(GameManager.Instance.PrimaryPlayer.CurrentRoom);
            });

           

            ETGModConsole.Commands.GetGroup("bot").AddUnit("asset_bundle_objects", delegate (string[] args)
            {
                ETGModConsole.Log("===================================");
                foreach (string str in BeyondPrefabs.AHHH.GetAllAssetNames())
                {
                    ETGModConsole.Log(BeyondPrefabs.AHHH.name + ": " + str);
                }
                ETGModConsole.Log("===================================");
                var part = UnityEngine.Object.Instantiate<GameObject>(BeyondPrefabs.AHHH.LoadAsset<GameObject>("Cylinder"), GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, Quaternion.identity);

                if (part.layer != LayerMask.NameToLayer("Unpixelated"))
                {
                    part.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
                }

                //part.AddComponent<MakeObjSpin>();
            });

            ETGModConsole.Commands.AddGroup("bot_saveapi");
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_flag", delegate (string[] args)
            {
                ETGModConsole.Log("CustomDungeonFlags.BOT_EFFIGY_POWERED's value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_flag", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED, bool.Parse(args[0]));
                ETGModConsole.Log("CustomDungeonFlags.BOT_EFFIGY_POWERED's new value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.BOT_EFFIGY_POWERED).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_stat", delegate (string[] args)
            {
                ETGModConsole.Log("CustomTrackedStats.BOT_EFFIGY_POWERED's value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_stat", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetStat(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedStats.BOT_EFFIGY_POWERED's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("increment_stat", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.RegisterStatChange(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedStats.BOT_EFFIGY_POWERED's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_maximum", delegate (string[] args)
            {
                ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_maximum", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.UpdateMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's new value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("get_char_flag", delegate (string[] args)
            {
                ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
            });
            ETGModConsole.Commands.GetGroup("bot_saveapi").AddUnit("set_char_flag", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG, bool.Parse(args[0]));
                ETGModConsole.Log("CustomDungeonFlags.BOT_LOST_UNLOCKED's new value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
            });

            ETGModConsole.Commands.AddGroup("floor", args =>
            {
            });
            ETGModConsole.Commands.GetGroup("floor").AddUnit("load", LoadFloor);

            ETGModConsole.Commands.AddGroup("past", args =>
            {
            });
            ETGModConsole.Commands.GetGroup("past").AddUnit("load", LoadPast);
        }

        private static void LoadFloor(string[] obj)
        {
            GameManager.Instance.LoadCustomLevel(BeyondDungeon.BeyondDefinition.dungeonSceneName);
        }

        private static void LoadPast(string[] obj)
        {
            GameManager.Instance.LoadCustomLevel(LostPastDungeon.LostPastDefinition.dungeonSceneName);
        }

        private static void ItemInfo(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1))
            {
                return;
            }
            string text = args[0];

            foreach (var comp in PickupObjectDatabase.GetById(int.Parse(text)).gameObject.GetComponents<Component>())
            {
                ETGModConsole.Log($"{comp.GetType()}: {comp.name}");
            }


        }

        public static void EnemyInfo(string[] args)
        {
            if (!ETGModConsole.ArgCount(args, 1, 1))
            {
                return;
            }
            string text = args[0];
            if (!Game.Enemies.ContainsID(text))
            {
                ETGModConsole.Log(string.Format("Enemy with ID {0} doesn't exist", text), false);
                return;
            }
            AIActor enemyPrefab = Game.Enemies[text];
            //ETGModConsole.Log(text, false);

            EnemyTools.DebugInformation(enemyPrefab.behaviorSpeculator);


        }
    }
}
