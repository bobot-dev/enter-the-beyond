﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GungeonAPI;
using SaveAPI;


namespace CustomCharacters
{
    public class CustomCharacterData
    {
        public PlayableCharacters baseCharacter = PlayableCharacters.Pilot;
        public PlayableCharacters identity;
        public CustomDungeonFlags unlockFlag;
        public string name, nameShort, nickname, nameInternal, altGun;
        public Dictionary<PlayerStats.StatType, float> stats;
        public List<Texture2D> sprites, altSprites, foyerCardSprites, punchoutSprites, punchoutFaceCards, loadoutSprites;
        public List<string> loadoutSpriteNames = new List<string>();
        public Texture2D altPlayerSheet, playerSheet, minimapIcon, bossCard, junkanWinPic, pastWinPic;
        public Texture2D faceCard;
        public List<Tuple<PickupObject, bool>> loadout;
        public int characterID, metaCost;
        public float health = 3, armor = 0;
        public tk2dSpriteAnimation AlternateCostumeLibrary;
        public bool removeFoyerExtras;
        public bool useGlow;
        public Color emissiveColor;
        public float emissiveColorPower, emissivePower, emissiveThresholdSensitivity;

        public CustomCharacterController customCharacterController;

    }

    public class CustomCharacter : MonoBehaviour
    {
        public CustomCharacterData data;
        private bool checkedGuns = false;
        private bool failedToFindData = false;
        private List<int> infiniteGunIDs = new List<int>();

        void Start()
        {
            GetData();
            GameManager.Instance.OnNewLevelFullyLoaded += StartInfiniteGunCheck;
            if (!GameManager.Instance.IsFoyer)
            {
                StartInfiniteGunCheck();
            }
        }

        void GetData()
        {
            try
            {
                var gameobjName = this.gameObject.name.ToLower().Replace("(clone)", "").Trim();
                foreach (var cc in CharacterBuilder.storedCharacters.Keys)
                {
                    if (cc.ToLower().Equals(gameobjName))
                        data = CharacterBuilder.storedCharacters[cc].First;
                }
            }
            catch
            {
                failedToFindData = true;
            }
            if (data == null) failedToFindData = true;
        }

        public void StartInfiniteGunCheck()
        {
            StartCoroutine("CheckInfiniteGuns");
        }

        public IEnumerator CheckInfiniteGuns()
        {
            while (!checkedGuns)
            {
                ToolsGAPI.Print("    Data check");
                if (data == null)
                {
                    ToolsGAPI.PrintError("Couldn't find a character data object for this player!");
                    yield return new WaitForSeconds(.1f);
                }

                ToolsGAPI.Print("    Loadout check");
                var loadout = data.loadout;
                if (loadout == null)
                {
                    checkedGuns = true;
                    yield break;
                }

                var player = GetComponent<PlayerController>();
                if (player?.inventory?.AllGuns == null)
                {
                    ToolsGAPI.PrintError("Player or inventory not found");
                    yield return new WaitForSeconds(.1f);
                }
                ToolsGAPI.Print($"Doing infinite gun check on {player.name}");

                this.infiniteGunIDs = GetInfiniteGunIDs();
                ToolsGAPI.Print("    Gun check");
                foreach (var gun in player.inventory.AllGuns)
                {
                    if (infiniteGunIDs.Contains(gun.PickupObjectId))
                    {
                        if (!Hooks.gunBackups.ContainsKey(gun.PickupObjectId))
                        {
                            var backup = new Hooks.GunBackupData()
                            {
                                InfiniteAmmo = gun.InfiniteAmmo,
                                PreventStartingOwnerFromDropping = gun.PreventStartingOwnerFromDropping,
                                CanBeDropped = gun.CanBeDropped,
                                PersistsOnDeath = gun.PersistsOnDeath
                            };
                            Hooks.gunBackups.Add(gun.PickupObjectId, backup);
                            var prefab = PickupObjectDatabase.GetById(gun.PickupObjectId) as Gun;
                            prefab.InfiniteAmmo = true;
                            prefab.PersistsOnDeath = true;
                            prefab.CanBeDropped = false;
                            prefab.PreventStartingOwnerFromDropping = true;
                        }

                        gun.InfiniteAmmo = true;
                        gun.PersistsOnDeath = true;
                        gun.CanBeDropped = false;
                        gun.PreventStartingOwnerFromDropping = true;
                        ToolsGAPI.Print($"        {gun.name} is infinite now.");
                    }
                }
                checkedGuns = true;
                yield break;
            }
        }

        public List<int> GetInfiniteGunIDs()
        {
            var infiniteGunIDs = new List<int>();
            if (data == null) GetData();
            if (data == null || failedToFindData) return infiniteGunIDs;
            foreach (var item in data.loadout)
            {
                var g = item?.First?.GetComponent<Gun>();
                if (g && item.Second)
                    infiniteGunIDs.Add(g.PickupObjectId);
            }
            return infiniteGunIDs;
        }

        //This handles the dueling laser problem
        void FixedUpdate()
        {
            if (GameManager.Instance.IsLoadingLevel || GameManager.Instance.IsPaused) return;
            if (data == null) return;

            foreach (var gun in GetComponent<PlayerController>().inventory.AllGuns)
            {
                if (gun.InfiniteAmmo && infiniteGunIDs.Contains(gun.PickupObjectId))
                {
                    gun.ammo = gun.AdjustedMaxAmmo;
                    gun.RequiresFundsToShoot = false;

                    if (gun.UsesRechargeLikeActiveItem)
                    {
                        if (gun.RemainingActiveCooldownAmount > 0)
                        {
                            gun.RemainingActiveCooldownAmount = Mathf.Max(0f, gun.RemainingActiveCooldownAmount - 25f * BraveTime.DeltaTime);
                        }
                    }

                }
            }
        }

        void OnDestroy()
        {
            GameManager.Instance.OnNewLevelFullyLoaded -= StartInfiniteGunCheck;
        }
    }
}
