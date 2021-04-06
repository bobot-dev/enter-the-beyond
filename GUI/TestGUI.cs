using ETGGUI.Inspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	class TestGUI
	{

		// Token: 0x06009F04 RID: 40708 RVA: 0x00059CEB File Offset: 0x00057EEB

		public void Start()
		{
			TestGUI.WindowRect = new Rect(500f, 0f, 450f, 900f);
			TestGUI.baseInspector = new GenericComponentInspector();
		}

		// Token: 0x06009F05 RID: 40709 RVA: 0x00003C7F File Offset: 0x00001E7F

		public void Update()
		{
		}

		// Token: 0x06009F06 RID: 40710 RVA: 0x00059D1A File Offset: 0x00057F1A
		public static void OnGUI()
		{
			TestGUI.WindowRect = UnityEngine.GUI.Window(14, TestGUI.WindowRect, new GUI.WindowFunction(WindowFunction), "Dumb Unreadable Mess");
			UnityEngine.GUI.backgroundColor = Color.green;
		}

		// Token: 0x06009F07 RID: 40711 RVA: 0x003E4B68 File Offset: 0x003E2D68
		public static void WindowFunction(int windowID)
		{
			TestGUI.ScrollPos = GUILayout.BeginScrollView(TestGUI.ScrollPos, new GUILayoutOption[0]);
			if (TestGUI.targetObject != null)
			{
				foreach (Component component in TestGUI.targetObject.GetComponents<Component>())
				{
					if (!(component == null))
					{
						GenericComponentInspector genericComponentInspector;
						if (TestGUI.ComponentInspectorRegistry.TryGetValue(component.GetType(), out genericComponentInspector))
						{
							genericComponentInspector.OnGUI(component);
						}
						else
						{
							TestGUI.baseInspector.OnGUI(component);
						}
					}
				}
			}
			GUILayout.EndScrollView();
			UnityEngine.GUI.DragWindow();
			UnityEngine.GUI.color = Color.green;
		}

		// Token: 0x06009F08 RID: 40712 RVA: 0x00003C7F File Offset: 0x00001E7F

		public void OnOpen()
		{
		}

		// Token: 0x06009F09 RID: 40713 RVA: 0x00003C7F File Offset: 0x00001E7F

		public void OnClose()
		{
		}

		// Token: 0x06009F0A RID: 40714 RVA: 0x00003C7F File Offset: 0x00001E7F

		public void OnDestroy()
		{
		}


		// Token: 0x06009F0C RID: 40716 RVA: 0x0000355B File Offset: 0x0000175B

		public TestGUI()
		{
		}

		// Token: 0x06009F0D RID: 40717 RVA: 0x003E4C58 File Offset: 0x003E2E58

		static TestGUI()
		{
			TestGUI.ComponentInspectorRegistry = new Dictionary<Type, GenericComponentInspector>();
			TestGUI.PropertyInspectorRegistry = new Dictionary<Type, IBasePropertyInspector>
		{
			{
				typeof(Vector2),
				new VectorPropertyInspector()
			},
			{
				typeof(Vector3),
				new VectorPropertyInspector()
			},
			{
				typeof(Vector4),
				new VectorPropertyInspector()
			},
			{
				typeof(float),
				new FloatPropertyInspector()
			},
			{
				typeof(bool),
				new BoolPropertyInspector()
			}
		};
		}


		public static Dictionary<Type, GenericComponentInspector> ComponentInspectorRegistry;

		public static Dictionary<Type, IBasePropertyInspector> PropertyInspectorRegistry;

		public static GenericComponentInspector baseInspector;

		public static GameObject targetObject;

		private static Rect WindowRect;

		private static Vector2 ScrollPos;

	}
}


