using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000F RID: 15
	public static class Tools
	{
		
		public static void Init()
		{
			bool flag = File.Exists(Tools.defaultLog);
			if (flag)
			{
				File.Delete(Tools.defaultLog);
			}
		}

		
		public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
		{
			bool flag = Tools.verbose || force;
			if (flag)
			{
				string[] array = obj.ToString().Split(new char[]
				{
					'\n'
				});
				foreach (string text in array)
				{
					Tools.LogToConsole(string.Concat(new string[]
					{
						"<color=#",
						color,
						">[",
						Tools.modID,
						"] ",
						text,
						"</color>"
					}));
				}
			}
			Tools.Log<string>(obj.ToString());
		}

		
		public static void PrintRaw<T>(T obj, bool force = false)
		{
			bool flag = Tools.verbose || force;
			if (flag)
			{
				Tools.LogToConsole(obj.ToString());
			}
			Tools.Log<string>(obj.ToString());
		}

		
		public static void PrintError<T>(T obj, string color = "FF0000")
		{
			string[] array = obj.ToString().Split(new char[]
			{
				'\n'
			});
			foreach (string text in array)
			{
				Tools.LogToConsole(string.Concat(new string[]
				{
					"<color=#",
					color,
					">[",
					Tools.modID,
					"] ",
					text,
					"</color>"
				}));
			}
			Tools.Log<string>(obj.ToString());
		}

		
		public static void PrintException(Exception e, string color = "FF0000")
		{
			string text = e.Message + "\n" + e.StackTrace;
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			foreach (string text2 in array)
			{
				Tools.LogToConsole(string.Concat(new string[]
				{
					"<color=#",
					color,
					">[",
					Tools.modID,
					"] ",
					text2,
					"</color>"
				}));
			}
			Tools.Log<string>(e.Message);
			Tools.Log<string>("\t" + e.StackTrace);
		}

		
		public static void Log<T>(T obj)
		{
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, Tools.defaultLog), true))
			{
				streamWriter.WriteLine(obj.ToString());
			}
		}

		
		public static void Log<T>(T obj, string fileName)
		{
			bool flag = !Tools.verbose;
			if (!flag)
			{
				using (StreamWriter streamWriter = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, fileName), true))
				{
					streamWriter.WriteLine(obj.ToString());
				}
			}
		}

		
		public static void LogToConsole(string message)
		{
			message.Replace("\t", "    ");
			ETGModConsole.Log(message, false);
		}

		
		private static void BreakdownComponentsInternal(this GameObject obj, int lvl = 0)
		{
			string text = "";
			for (int i = 0; i < lvl; i++)
			{
				text += "\t";
			}
			Tools.Log<string>(text + obj.name + "...");
			foreach (Component component in obj.GetComponents<Component>())
			{
				string str = text;
				string str2 = "    -";
				Type type = component.GetType();
				Tools.Log<string>(str + str2 + ((type != null) ? type.ToString() : null));
			}
			foreach (Transform transform in obj.GetComponentsInChildren<Transform>())
			{
				bool flag = transform != obj.transform;
				if (flag)
				{
					transform.gameObject.BreakdownComponentsInternal(lvl + 1);
				}
			}
		}

		
		public static void BreakdownComponents(this GameObject obj)
		{
			obj.BreakdownComponentsInternal(0);
		}

		
		public static void ExportTexture(Texture texture, string folder = "")
		{
			string text = Path.Combine(ETGMod.ResourcesDirectory, folder);
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			File.WriteAllBytes(Path.Combine(text, texture.name + DateTime.Now.Ticks.ToString() + ".png"), ((Texture2D)texture).EncodeToPNG());
		}

		
		public static T GetEnumValue<T>(string val) where T : Enum
		{
			return (T)((object)Enum.Parse(typeof(T), val.ToUpper()));
		}

		
		public static void LogPropertiesAndFields<T>(T obj, string header = "")
		{
			Tools.Log<string>(header);
			Tools.Log<string>("=======================");
			bool flag = obj == null;
			if (flag)
			{
				Tools.Log<string>("LogPropertiesAndFields: Null object");
			}
			else
			{
				Type type = obj.GetType();
				Tools.Log<string>(string.Format("Type: {0}", type));
				PropertyInfo[] properties = type.GetProperties();
				Tools.Log<string>(string.Format("{0} Properties: ", typeof(T)));
				foreach (PropertyInfo propertyInfo in properties)
				{
					try
					{
						object value = propertyInfo.GetValue(obj, null);
						string text = value.ToString();
						bool flag2 = ((obj != null) ? obj.GetType().GetGenericTypeDefinition() : null) == typeof(List<>);
						bool flag3 = flag2;
						if (flag3)
						{
							List<object> list = value as List<object>;
							text = string.Format("List[{0}]", list.Count);
							foreach (object obj2 in list)
							{
								text = text + "\n\t\t" + obj2.ToString();
							}
						}
						Tools.Log<string>("\t" + propertyInfo.Name + ": " + text);
					}
					catch
					{
					}
				}
				Tools.Log<string>(string.Format("{0} Fields: ", typeof(T)));
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					Tools.Log<string>(string.Format("\t{0}: {1}", fieldInfo.Name, fieldInfo.GetValue(obj)));
				}
			}
		}

		
		public static void StartTimer(string name)
		{
			string key = name.ToLower();
			bool flag = Tools.timers.ContainsKey(key);
			if (flag)
			{
				Tools.PrintError<string>("Timer " + name + " already exists.", "FF0000");
			}
			else
			{
				Tools.timers.Add(key, Time.realtimeSinceStartup);
			}
		}

		
		public static void StopTimerAndReport(string name)
		{
			string key = name.ToLower();
			bool flag = !Tools.timers.ContainsKey(key);
			if (flag)
			{
				Tools.PrintError<string>("Could not stop timer " + name + ", no such timer exists", "FF0000");
			}
			else
			{
				float num = Tools.timers[key];
				int num2 = (int)((Time.realtimeSinceStartup - num) * 1000f);
				Tools.timers.Remove(key);
				Tools.Print<string>(name + " finished in " + num2.ToString() + "ms", "FFFFFF", false);
			}
		}

		// Token: 0x0400003F RID: 63
		public static bool verbose = false;

		// Token: 0x04000040 RID: 64
		private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "customCharacterLog.txt");

		// Token: 0x04000041 RID: 65
		public static string modID = "ETB";

		// Token: 0x04000042 RID: 66
		private static Dictionary<string, float> timers = new Dictionary<string, float>();
	}
}
