using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class NotReallyADebuff : BraveBehaviour
    {
        private void Start()
        {
            this.healthHaver.OnPreDeath += HealthHaver_OnPreDeath;
        }

        private void HealthHaver_OnPreDeath(UnityEngine.Vector2 obj)
        {
            
        }
    }
}
