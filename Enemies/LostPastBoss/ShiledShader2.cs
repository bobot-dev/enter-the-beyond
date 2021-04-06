using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class ShiledShader2 : BraveBehaviour
    {
        public void Start()
        {
            base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
        }
    }
}
