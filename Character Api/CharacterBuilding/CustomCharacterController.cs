using BotsMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomCharacters
{
    public class CustomCharacterController : MonoBehaviour
    {
        public CustomCharacterData data;
        public string past;
        public bool hasPast;
        public int metaCost;

        public bool useGlow;
        public Color emissiveColor;
        public float emissiveColorPower, emissivePower, emissiveThresholdSensitivity;
    }

    class CustomCharacterFoyerController : MonoBehaviour
    {
        public int metaCost;
        public bool useGlow;
        public Color emissiveColor;
        public float emissiveColorPower, emissivePower, emissiveThresholdSensitivity;
        public CustomCharacterController customCharacterController;

        private void Start()
        {
            BotsModule.Log(metaCost.ToString());
            //metaCost = customCharacterController.metaCost;
            useGlow = customCharacterController.useGlow;
            emissiveColor = customCharacterController.emissiveColor;
            emissiveColorPower = customCharacterController.emissiveColorPower;
            emissivePower = customCharacterController.emissivePower;
            emissiveThresholdSensitivity = customCharacterController.emissiveThresholdSensitivity;
        }

    }
}
