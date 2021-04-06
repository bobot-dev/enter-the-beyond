using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GungeonAPI
{
    public static class GungeonAP
    {
        public static void Init()
        {
            ToolsGAPI.Init();
            StaticReferences.Init();
            FakePrefabHooks.Init();
            ShrineFactory.Init();
            DungeonHandler.Init();
        }
    }
}
