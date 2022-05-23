using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace BotsMod
{
    class OverseerGlow : BraveBehaviour
	{
		public void Start()
		{
            //base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
            return;
            Material mat = new Material(ShaderCache.Acquire("Brave/LitCutoutUber_ColorEmissive"));
            mat.SetTexture("_MainTexture", base.sprite.renderer.material.GetTexture("_MainTex"));//not sure this is necessary

            mat.SetColor("_EmissiveColor", new Color32(20, 5, 5, 255));//the color thatll glow

            mat.SetFloat("_EmissiveColorPower", 100f);

            mat.SetFloat("_EmissivePower", 100f);

            mat.SetFloat("_EmissiveThresholdSensitivity", 0.03f);//this be what you need to mess with, and the correct value is dependant on the color so youll just have to experiment. you probably want a somewhat higher value tho

            mat.SetFloat("_ValueMinimum", 0.7f);//ignore this
            mat.SetFloat("_ValueMaximum", 0.97f);
            mat.SetFloat("_Perpendicular", 1f);
            mat.SetFloat("_Cutoff", 0.5f);
            mat.SetColor("_OverrideColor", new Color(1, 1, 1, 0));
            mat.SetColor("_OutlineColor", new Color(0, 0, 0, 1));
            mat.EnableKeyword("EMISSIVE_ON");
            mat.DisableKeyword("EMISSIVE_OFF");
            mat.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            mat.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
            mat.DisableKeyword("PALETTE_ON");
            mat.EnableKeyword("PALETTE_OFF");
            mat.DisableKeyword("TINTING_ON");
            mat.EnableKeyword("TINTING_OFF");//untill this
            base.sprite.renderer.material = mat;
        }
    }
}
