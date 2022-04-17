using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class HelixBeam : MonoBehaviour
    {
        public float helixWavelength;
        public float helixAmplitude;
        public float helixBeamOffsetPerSecond;
        public bool invert;
    }

    class CurvedBeam : MonoBehaviour
    {
        public float baseCurve;
        public float curveIntensity;
        public bool invert;
    }
}
