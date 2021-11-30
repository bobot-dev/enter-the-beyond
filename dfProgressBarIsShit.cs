using ItemAPI;
using NpcApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    public class dfProgressBarIsShit : dfProgressBar
    {
        public dfProgressBarIsShit(string ProgressSprite, string BackgroundSprite)
        {
            var Atlas = Tools.shared_auto_001.LoadAsset<GameObject>("GameUIAtlas").GetComponent<dfAtlas>();
            


            anchorStyle = dfAnchorStyle.CenterHorizontal;
            IsEnabled = true;
            isVisible = false;
            isInteractive = true;
            tooltip = "";
            pivot = dfPivotPoint.TopLeft;
            zindex = 3;
            color = UnityEngine.Color.white;
            disabledColor = UnityEngine.Color.white;
            size = new Vector2(36, 36);
            minSize = new Vector2(0, 0);
            maxSize = new Vector2(0, 0);
            clipChildren = false;
            inverseClipChildren = false;
            tabIndex = -1;
            canFocus = false;
            autoFocus = false;
            
            renderOrder = 77;
            isLocalized = false;
            hotZoneScale = new Vector2(1, 1);
            allowSignalEvents = true;
            PrecludeUpdateCycle = false;
            atlas = Atlas;
            backgroundSprite = Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(BackgroundSprite)).name;
            progressSprite = Atlas.AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(ProgressSprite)).name;
            progressColor = UnityEngine.Color.white;
            rawValue = 1;
            minValue = 0;
            maxValue = 1;
            fillMode = dfProgressFillMode.Fill;
            progressFillDirection = dfFillDirection.Vertical;
            padding = new RectOffset{ bottom = 0, left = 0, top = 0, right = 0 };
            actAsSlider = false;

        }

        public static dfProgressBar Init(string ProgressSprite, string BackgroundSprite)
        {
            var thing = FakePrefab.Clone(GameUIRoot.Instance.p_playerReloadBar.statusBarPoison.gameObject).GetComponent<dfProgressBar>();

            ETGModConsole.Log(GameUIRoot.Instance.p_playerReloadBar.statusBarPoison.transform.parent.name);
            thing.transform.parent = GameUIRoot.Instance.p_playerReloadBar.statusBarPoison.transform.parent;

            FieldInfo progressSprite = typeof(dfProgressBar).GetField("progressSprite", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo backgroundSprite = typeof(dfProgressBar).GetField("backgroundSprite", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo atlas = typeof(dfProgressBar).GetField("atlas", BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo cachedManager = typeof(dfControl).GetField("cachedManager", BindingFlags.NonPublic | BindingFlags.Instance);

            cachedManager.SetValue(thing, (cachedManager.GetValue(GameUIRoot.Instance.p_playerReloadBar.statusBarPoison) as dfGUIManager));

            ETGModConsole.Log((cachedManager.GetValue(GameUIRoot.Instance.p_playerReloadBar.statusBarPoison) as dfGUIManager).ToString());
            ETGModConsole.Log((cachedManager.GetValue(thing) as dfGUIManager).ToString());

            progressSprite.SetValue(thing, (atlas.GetValue(thing) as dfAtlas).AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(ProgressSprite)).name);
            backgroundSprite.SetValue(thing, (atlas.GetValue(thing) as dfAtlas).AddNewItemToAtlas(ItemAPI.ResourceExtractor.GetTextureFromResource(BackgroundSprite)).name);
            ETGModConsole.Log("AAA");
            return thing;
        }

        
    }
}
