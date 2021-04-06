using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class Class2 : MonoBehaviour
    {
		public static GUIStyle style;
		public static Rect windowRect = new Rect(20, 20, 120, 50);

		public static void OnGUI()
		{
			GUI.skin.window = style;
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window");
		}

		private static Vector2 ScrollPos;




		static void DoMyWindow(int windowID)
		{

			GUILayout.Box(ItemAPI.ResourceExtractor.BuildStringFromEmbeddedResource("BotsMod/sprites/ammonomicon_enemy_lpb_001.png"), new GUILayoutOption[0]);

			GUILayout.Label("text go here :)");
			GUILayout.Label("text go here :) 2");
			GUILayout.Label("text go here :) 3");
			GUILayout.Label("text go here :) 4");

			ScrollPos = GUILayout.BeginScrollView(ScrollPos, new GUILayoutOption[0]);

			GUILayout.EndScrollView();
			UnityEngine.GUI.DragWindow();
			UnityEngine.GUI.color = Color.green;
		}
	}
}
