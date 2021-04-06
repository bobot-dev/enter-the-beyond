using System.Linq;
using ItemAPI;
using GungeonAPI;
using Steamworks;

namespace Items
{
    public class CelsItems : ETGModule
    {
        public static readonly string Modname = "Cel's Items";
        public static readonly string Version = "One More In The Chamber, Part 1";
        public static readonly string Color = "#FCA4E2";
        public static HellDragZoneController hellDrag;
        public static string SteamUsername;
        public override void Init()
        {

        }

        public override void Start()
        {
            
            ItemAPI.FakePrefabHooks.Init();
            GungeonAPI.FakePrefabHooks.Init();
            ItemAPI.EnemyBuilder.Init();
            ItemAPI.BossBuilder.Init();

            ItemBuilder.Init();
            CrownOfTheChosen.Init();
            if(SteamUsername != string.Empty)
            {
                SteamUsername = SteamFriends.GetPersonaName();
                //CrownChanger.Change();
            }

            
            ETGModConsole.Commands.AddGroup("cel", args =>
            {
            });
            ETGModConsole.Commands.GetGroup("cel").AddUnit("crown_override", this.CrownOverride);

            Log($"{Modname} v{Version} started successfully.", Color);
            Log($"Link to Changelog https://pastebin.com/TPvwpdGJ", Color);
        }
        private void CrownOverride(string[] args)
        {
            if (args.Length < 1)
            {
                ETGModConsole.Log("At least 1 arguments required.");
            }
            else
            {
                SteamUsername = args[0];
                ETGModConsole.Log($"Name : {SteamUsername}");
                
            }
        }
        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        
        public override void Exit()
        {
            
        }
        
       

        //written by @UnstableStrafe#3928 with help from KyleTheScientist, Neighborino, Glorfindel, Retrash, Reto, TheTurtleMelon, TankTheta, Spapi, Eternal Frost, Some Bunny, NotABot, BlazeyKat, ExplosivePanda,
    }
}

