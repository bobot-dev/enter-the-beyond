using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Debuffs
    {
        public static GameActorDecayEffect decayEffect = new GameActorDecayEffect
        {
            initalAmount = 1,
            SpeedMultiplier = 0.1f,
            CooldownMultiplier = 10f,
            effectIdentifier = "decay",
            duration = 5,
            
            AppliesTint = false,
            DamagePerSecondToEnemies = 5

        };
    }
}
