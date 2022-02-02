using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;


namespace BotsMod
{
    public static class VFXToolbox
    {
        public static void BuildVFXList()
        {
            blasphemySlash = (ETGMod.Databases.Items["wonderboy"] as Gun).muzzleFlashEffects;
            //beyondSlash = VFXBuilder.CreateMuzzleflash("beyond_slash", beyondSpritePaths, 8, new IntVector2(30, 45), tk2dBaseSprite.Anchor.MiddleLeft, new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero }, false, false, false, false, 0, VFXAlignment.Fixed, true, 10,
            //    new  Color32(123, 246, 236, 255), 1.55f);
            
        }


        public static VFXPool beyondSlash;

        public static VFXPool blasphemySlash;

        private static List<string> beyondSpritePaths = new List<string>
        {
            "drifter_slash_001",
            "drifter_slash_002",
            "drifter_slash_003",
        };
    }
}
