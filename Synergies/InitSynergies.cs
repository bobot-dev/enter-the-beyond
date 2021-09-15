using Gungeon;
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
            CustomSynergies.Add("Full Circuit", new List<string> { "bot:lightning_rounds", "shock_rounds" }, null, true);
            CustomSynergies.Add("The Marksman", new List<string> { "bot:hells_revolver", "iron_coin" }, null, false);

            /*AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessor = (PickupObjectDatabase.GetById(Game.Items["bot:hells_revolver"].PickupObjectId) as Gun).gameObject.AddComponent<AdvancedTransformGunSy/*nergyProcessor>();
            advancedTransformGunSynergyProcessor.NonSynergyGunId = Game.Items["bot:hells_revolver"].PickupObjectId;
            advancedTransformGunSynergyProcessor.SynergyGunId = Game.Items["bot:hells_revolver+the_marksman"].PickupObjectId;
            advancedTransformGunSynergyProcessor.SynergyToCheck = "The Marksman";*/

            //CustomSynergies.Add("Forme Test", new List<string> { "bot:lost_sidearm", "shock_rounds" }, null, false);
        }
    }
}
