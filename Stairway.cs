using Dungeonator;
using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class StairWay : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Stairway";
            string resourceName = "BotsMod/sprites/StairWay";
            GameObject obj = new GameObject();
            //var item = BotsModule.WarCrime2;//obj.AddComponent<PirmalShotgrub>().GetComponent<PickupObject>();
            var item = obj.AddComponent<StairWay>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //ItemBuilder.AddAnimatedSpriteToObject(itemName, new List<string> { "BotsMod/sprites/Spells/ShittySpellTempSprite_001", "BotsMod/sprites/Spells/ShittySpellTempSprite_002", "BotsMod/sprites/Spells/ShittySpellTempSprite_003", "BotsMod/sprites/Spells/ShittySpellTempSprite_004", "BotsMod/sprites/Spells/ShittySpellTempSprite_005", "BotsMod/sprites/Spells/ShittySpellTempSprite_006", "BotsMod/sprites/Spells/ShittySpellTempSprite_007", "BotsMod/sprites/Spells/ShittySpellTempSprite_008", "BotsMod/sprites/Spells/ShittySpellTempSprite_009", "BotsMod/sprites/Spells/ShittySpellTempSprite_010", "BotsMod/sprites/Spells/ShittySpellTempSprite_011", "BotsMod/sprites/Spells/ShittySpellTempSprite_012", }, obj);
            


            string shortDesc = "Ascend";
            string longDesc = "Creates a ladder to a secret shop that trades guns for items.\n\nheads up this item works the npc is just a tad unfinished thus the broken text";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.B;

            //PrayerKeep = RoomFactory.BuildFromResource("Planetside/Resources/ShrineRooms/PrayerRooms/PrayerRoomKeep.room").room;
            Tools.BeyondItems.Add(item.PickupObjectId);
            StairWay.InitRooms();
            BotsItemIds.Stairway = item.PickupObjectId;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnNewFloorLoaded += PlaceLadder;
            
            StairWay.IncrementFlag(player, typeof(StairWay));
        }

        void PlaceLadder(PlayerController player)
        {
            RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;

            var forgeDungeon = DungeonDatabase.GetOrLoadByName("base_nakatomi");
            var hellDrag = FakePrefab.Clone(forgeDungeon.PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("OFFICE_13_Fire_escape_01")).First().overrideExactRoom.placedObjects
            .Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<UsableBasicWarp>()[0].gameObject);
            UnityEngine.Object.Destroy(hellDrag.GetComponent<UsableBasicWarp>());
            var shopObj = DungeonPlaceableUtility.InstantiateDungeonPlaceable(hellDrag.gameObject, entrance, new IntVector2(1, 8), false);


            
            shopObj.AddComponent<StairWayWarpComp>().targetRoomName = ModRoomPrefabs.StairWayRoom.name;

            //GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(shopObj.GetComponent<TalkDoerLite>());


            /*DungeonPlaceable dungeonPlaceable = BraveResources.Load("Global Prefabs/Merchant_Rat_Placeable", ".asset") as DungeonPlaceable;
            GameObject gObj = dungeonPlaceable.InstantiateObject(entrance, new IntVector2(1, 3), false, false);
            IPlayerInteractable[] interfacesInChildren = gObj.GetInterfacesInChildren<IPlayerInteractable>();
            for (int j = 0; j < interfacesInChildren.Length; j++)
            {
                entrance.RegisterInteractable(interfacesInChildren[j]);
            }*/
        }

        public override DebrisObject Drop(PlayerController player)
        {
            StairWay.DecrementFlag(player, typeof(StairWay));
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            StairWay.DecrementFlag(GameManager.Instance.PrimaryPlayer, typeof(StairWay));
            base.OnDestroy();
        }

        public static ProceduralFlowModifierData GenerateNewMrocData(PrototypeDungeonRoom RequiredRoom)
        {
            string name = RequiredRoom.name.ToString();
            if (RequiredRoom.name.ToString() == null)
            {
                name = "EmergencyAnnotationName";
            }
            ProceduralFlowModifierData PrayerRoomMines = new ProceduralFlowModifierData()
            {
                annotation = name,
                DEBUG_FORCE_SPAWN = true,
                OncePerRun = false,
                placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>()
                {
                    ProceduralFlowModifierData.FlowModifierPlacementType.NO_LINKS
                },
                roomTable = null,
                exactRoom = RequiredRoom,
                IsWarpWing = false,
                RequiresMasteryToken = false,
                chanceToLock = 0,
                selectionWeight = 2,
                chanceToSpawn = 1,
                RequiredValidPlaceable = null,
                prerequisites = new DungeonPrerequisite[]
                {
                    new DungeonGenToolbox.AdvancedDungeonPrerequisite
                    {
                       advancedAdvancedPrerequisiteType = DungeonGenToolbox.AdvancedDungeonPrerequisite.AdvancedAdvancedPrerequisiteType.PASSIVE_ITEM_FLAG,
                       requiredPassiveFlag = typeof(StairWay),
                       prerequisiteType = DungeonPrerequisite.PrerequisiteType.ENCOUNTER,
                       //requiredTileset = Tileset,
                       requireTileset = false
                    }
                },
                CanBeForcedSecret = false,
                RandomNodeChildMinDistanceFromEntrance = 0,
                exactSecondaryRoom = null,
                framedCombatNodes = 0,

            };
            return PrayerRoomMines;
        }

        public static void InitRooms()
        {
            SharedInjectionData injector = ScriptableObject.CreateInstance<SharedInjectionData>();
            injector.UseInvalidWeightAsNoInjection = true;
            injector.PreventInjectionOfFailedPrerequisites = false;
            injector.IsNPCCell = false;
            injector.IgnoreUnmetPrerequisiteEntries = false;
            injector.OnlyOne = false;
            injector.ChanceToSpawnOne = 1f;
            injector.AttachedInjectionData = new List<SharedInjectionData>();
            injector.InjectionData = new List<ProceduralFlowModifierData>
            {
                GenerateNewMrocData(ModRoomPrefabs.StairWayRoom),
                //GenerateNewMrocData(PrayerProper, GlobalDungeonData.ValidTilesets.GUNGEON),
                //GenerateNewMrocData(PrayerMines, GlobalDungeonData.ValidTilesets.MINEGEON),
                //GenerateNewMrocData(PrayerHollow, GlobalDungeonData.ValidTilesets.CATACOMBGEON),
                //GenerateNewMrocData(PrayerForge, GlobalDungeonData.ValidTilesets.FORGEGEON),
                //GenerateNewMrocData(PrayerHell, GlobalDungeonData.ValidTilesets.HELLGEON),
                //GenerateNewMrocData(PrayerSewer, GlobalDungeonData.ValidTilesets.SEWERGEON),
                //GenerateNewMrocData(PrayerAbbey, GlobalDungeonData.ValidTilesets.CATHEDRALGEON),
            };
            injector.name = "angel rooms";
            SharedInjectionData BaseInjection = SaveAPI.SaveTools.LoadAssetFromAnywhere<SharedInjectionData>("Base Shared Injection Data");
            if (BaseInjection.AttachedInjectionData == null)
            {
                BaseInjection.AttachedInjectionData = new List<SharedInjectionData>();
            }
            BaseInjection.AttachedInjectionData.Add(injector);
        }

    }
}
