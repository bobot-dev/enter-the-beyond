using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace BotsMod
{
    class CustomFire
    {
        public static void Init()
        {
            pinkFire = FakePrefab.Clone((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/GlobalGreenFireSystem"), Vector3.zero, Quaternion.identity));
            pinkFire.GetComponent<ParticleSystemRenderer>().material = new Material(pinkFire.GetComponent<ParticleSystemRenderer>().material);
            pinkFire.GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", ResourceExtractor.GetTextureFromResource("BotsMod/sprites/fire_purple_effect_ver1_sheet_11x4.png"));
            pinkFire.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissiveColor", new Color32(227, 73, 155, 255));
        }


        public static GameObject pinkFire;
    }

    
}
