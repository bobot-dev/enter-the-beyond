using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class InitSynergies
    {
        public static void Init()
        {
            CustomSynergies.Add("Lower Case R Test", new List<string> { "lower_case_r", "sunglasses" }, null, false);
            CustomSynergies.Add("Full Circuit", new List<string> { "bot:lightning_rounds", "shock_rounds" }, null, false);
        }
    }
}
