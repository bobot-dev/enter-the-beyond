using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class StuffIStoleFromApacheForChallengeMode
    {
        public static void Init()
        {
            braveResources = ResourceManager.LoadAssetBundle("brave_resources_001");

            ChallengeManagerObject = braveResources.LoadAsset<GameObject>("_ChallengeManager");
            ChallengeMegaManagerObject = braveResources.LoadAsset<GameObject>("_ChallengeMegaManager");
            ChallengeManager component14 = ChallengeManagerObject.GetComponent<ChallengeManager>();
            ChallengeManager component15 = ChallengeMegaManagerObject.GetComponent<ChallengeManager>();

            Challenge_HELPME = component14.PossibleChallenges[21].challenge.gameObject;
            Challenge_HELPME.AddComponent<PrimalShotgrubChallengeModifier>();

            foreach (ChallengeDataEntry entry in component14.PossibleChallenges)
            {
                BotsModule.Log($"({number}): {entry.challenge.DisplayName}");
                number++;
            }

            component14.PossibleChallenges[21].challenge = Challenge_HELPME.GetComponent<PrimalShotgrubChallengeModifier>();
            component15.PossibleChallenges[21].challenge = Challenge_HELPME.GetComponent<PrimalShotgrubChallengeModifier>();
            Challenge_HELPME.name = "Challenge_HELPME";
            Challenge_HELPME.AddComponent<PrimalShotgrubChallengeModifier>();



            component14.PossibleChallenges.Add(new ChallengeDataEntry
            {
                Annotation = "Im so so sorry really...",
                challenge = Challenge_HELPME.GetComponent<PrimalShotgrubChallengeModifier>(),
                excludedTilesets = GlobalDungeonData.ValidTilesets.OFFICEGEON,

                tilesetsWithCustomValues = new List<GlobalDungeonData.ValidTilesets>(0),
                CustomValues = new List<int>(0)
            });
            component15.PossibleChallenges.Add(new ChallengeDataEntry
            {
                Annotation = "Im so so sorry really...",
                challenge = Challenge_HELPME.GetComponent<PrimalShotgrubChallengeModifier>(),
                excludedTilesets = GlobalDungeonData.ValidTilesets.OFFICEGEON,

                tilesetsWithCustomValues = new List<GlobalDungeonData.ValidTilesets>(0),
                CustomValues = new List<int>(0)
            });


        }
        public static int number = 0;
        public static AssetBundle braveResources;
        public static GameObject Challenge_HELPME;
        public static GameObject ChallengeManagerObject;
        public static GameObject ChallengeMegaManagerObject;
    }
}
