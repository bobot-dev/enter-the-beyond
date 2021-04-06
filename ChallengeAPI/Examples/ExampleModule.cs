using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChallengeAPI
{
    public class ExampleModule : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            //-------------SETUP----------
            //You need to init ChallengeBuilder for ChallengeAPI to work.
            ChallengeBuilder.Init();
            //If Debug Mode is enabled, the challenge mode will only have the most recent challenge that was added.
            ChallengeBuilder.EnableDebugMode();
            //-------------BUILDING CHALLENGES----------
            //Builds a basic challenge.
            ChallengeBuilder.BuildChallenge<ExampleChallengeModifier>("ChallengeAPI/Resources/exampleChallengeFrame.png", "SpecialAPI Tests Stuff", true, null, null, null, true, true);
            //Builds a challenge and sets it's variable. Chamber's Curse challenge was made by Some Bunny by the way!
            ChallengeDataEntry chamberCurseChallenge = ChallengeBuilder.BuildChallenge<ChambersCurseModifier>("ChallengeAPI/Resources/curseOfTheChamberFrame.png", "Chamber's Curse", true, new List<ChallengeModifier> { 
                ChallengeBuilder.ChallengeManagerPrefab.FindChallenge<SkyRocketChallengeModifier>().challenge }, null, null, true, true); //If you want the challenge to only 
            // appear in normal challenge mode or only appear in double challenge mode you can change one of the trues at the end of the function to false. The first true handles if the challenge appears in normal challenge mode and the second one handles if
            // it appears in double challenge mode
            // You can also make the challenge not be compatible with other challenges using the Mutually exclusive argument (the List<ChallengeModifier> thing). For example Chamber's Curse challenge isn't compatible with Gull's Revenge challenge.
            (chamberCurseChallenge.challenge as ChambersCurseModifier).dragunBoulder = Gungeon.Game.Enemies["dragun_advanced"].GetComponent<DraGunController>().skyBoulder;
            //------------BUILDING A BOSS CHALLENGE----------
            //Builds a special boss challenges and DOESN'T ADD IT TO CHALLENGE POOL.
            ChallengeDataEntry exampleBossChallenge = ChallengeBuilder.BuildChallenge<ExampleBossChallenge>("ChallengeAPI/Resources/exampleBossChallengeFrame.png", "Example Boss Challenge", true, null, null, null, false, false); // The false, false
            //                                                                                                                                                                         at the end prevent the challenge from being added to the challenge pool.
            //Sets the boss challenge's healthMultiplier field making it multiply the health of all bosses by 1.5;
            (exampleBossChallenge.challenge as ExampleBossChallenge).healthMultiplier = 1.5f;
            //Builds the boss challenge. If a boss has a boss challenge attached to it, the game will ONLY choose the challenges from the boss challenge, ignoring any other challenges.
            //For example this boss challenge will make the game always choose the Example Boss Challenge and the Chamber's Curse challenge when the player is fighting the Bullet King or the Beholster in challenge mode.
            ChallengeBuilder.BuildBossChallenge("Example Boss Challenge", new List<AIActor> { Gungeon.Game.Enemies["bullet_king"], Gungeon.Game.Enemies["beholster"] }, 2, new List<ChallengeDataEntry> { chamberCurseChallenge, exampleBossChallenge }, true, 
                true);
            //Or for example this boss challenge will make the game always choose the Example Boss Challenge OR the Chamber's Curse challenge when the player is fighting the Gorgun. The reason it only chooses one of the challenges is because this boss
            // has it's numChallengesToUse argument set to one.                                     (numChallengesToUse argument is the 1 below )
            ChallengeBuilder.BuildBossChallenge("Example Boss Challenge 2", new List<AIActor> { Gungeon.Game.Enemies["gorgun"] }, 1, new List<ChallengeDataEntry> { chamberCurseChallenge, exampleBossChallenge }, true, true);
        }

        public override void Exit()
        {
        }
    }
}
