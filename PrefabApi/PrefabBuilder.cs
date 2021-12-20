using BotsMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace PrefabAPI
{
    public static class PrefabBuilder
    {
        public static void Init()
        {
            if (m_initialized)
            {
                return;
            }
            m_assetBundles = new List<AssetBundle>();
            m_initialized = true;
            builtObjects = new List<GameObject>();
        }

        public static GameObject BuildObject(string name)
        {
            if(m_assetBundles == null || builtObjects == null)
            {
                Init();
            }
            byte[] bundleBytes = AssetBundleBuilder.BuildBundleFile("runtimebundle" + m_bundlesBuilt);
            m_bundlesBuilt++;
            AssetBundle bundle = AssetBundle.LoadFromMemory(bundleBytes);
            ETGModConsole.Log("bundle != null " + (bundle != null));
            m_assetBundles.Add(bundle);
            ETGModConsole.Log("m_assetBundles != null " + (m_assetBundles != null));
            GameObject prefab = bundle.LoadAsset<GameObject>("object");
            ETGModConsole.Log("prefab != null " + (prefab != null));
            prefab.name = name;
            builtObjects.Add(prefab);
            ETGModConsole.Log("builtObjects != null " + (builtObjects != null));
            return prefab;
        }

        public static GameObject Clone(GameObject toClone)
        {
            GameObject clone = BuildObject(toClone.name);// + " Clone");
            Dictionary<Tuple<MemberInfo, object>, int> componentsInChildren = new Dictionary<Tuple<MemberInfo, object>, int>();
            Dictionary<Tuple<MemberInfo, object>, int> gameObjectsInChildren = new Dictionary<Tuple<MemberInfo, object>, int>();
            List<Tuple<MemberInfo, object>> gameObjects = new List<Tuple<MemberInfo, object>>();
            foreach (UnityEngine.Component c in toClone.GetComponents<UnityEngine.Component>())
            {
                if (clone.GetComponent(c.GetType()) == null)
                {
                    clone.AddComponent(c.GetType());
                }
                foreach (PropertyInfo inf in c.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (inf.CanWrite && inf.CanRead)
                    {
                        try
                        {
                            object val = inf.GetValue(c, new object[0]);
                            bool overrideSet = false;
                            if (val is UnityEngine.Component)
                            {
                                if (toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                                {
                                    componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                                    overrideSet = true;
                                }
                            }
                            else if (val is GameObject go)
                            {
                                if (toClone.transform.GetChildObjects().Contains(val as GameObject))
                                {
                                    gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.transform.GetChildObjects().IndexOf(val as GameObject));
                                    overrideSet = true;
                                }
                                else if (go == toClone.gameObject)
                                {
                                    gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                                }
                            }
                            if (!overrideSet)
                            {
                                inf.SetValue(clone.GetComponent(c.GetType()), val, new object[0]);
                            }
                        }
                        catch { }
                    }
                }
                foreach (FieldInfo inf in c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    object val = inf.GetValue(c);
                    bool overrideSet = false;
                    if (val is UnityEngine.Component)
                    {
                        if (toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                        {
                            componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                            overrideSet = true;
                        }
                    }
                    else if (val is GameObject go)
                    {
                        if (toClone.transform.GetChildObjects().Contains(val as GameObject))
                        {
                            gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.transform.GetChildObjects().IndexOf(val as GameObject));
                            overrideSet = true;
                        }
                        else if (go == toClone.gameObject)
                        {
                            gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                        }
                    }
                    if (!overrideSet)
                    {
                        inf.SetValue(clone.GetComponent(c.GetType()), val);
                    }
                }
                foreach (Type inter in c.GetType().GetInterfaces())
                {
                    foreach (PropertyInfo inf in inter.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (inf.CanWrite && inf.CanRead)
                        {
                            try
                            {
                                object val = inf.GetValue(c, new object[0]);
                                bool overrideSet = false;
                                if (val is UnityEngine.Component)
                                {
                                    if (toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                                    {
                                        componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                                        overrideSet = true;
                                    }
                                }
                                else if (val is GameObject go)
                                {
                                    if (toClone.transform.GetChildObjects().Contains(val as GameObject))
                                    {
                                        gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), toClone.transform.GetChildObjects().IndexOf(val as GameObject));
                                        overrideSet = true;
                                    }
                                    else if (go == toClone.gameObject)
                                    {
                                        gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                                    }
                                }
                                if (!overrideSet)
                                {
                                    inf.SetValue(clone.GetComponent(c.GetType()), val, new object[0]);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            foreach (Transform child in toClone.transform)
            {
                GameObject childClone = CloneChild(child.gameObject, toClone, out var cip, out var goip, out var gos);
                componentsInChildren.AddRange(cip);
                gameObjectsInChildren.AddRange(goip);
                gameObjects.AddRange(gos);
                childClone.transform.parent = clone.transform;
                childClone.transform.localPosition = child.localPosition;
                childClone.transform.localRotation = child.localRotation;
                childClone.transform.localScale = child.localScale;
            }
            foreach (KeyValuePair<Tuple<MemberInfo, object>, int> kvp in componentsInChildren)
            {
                MemberInfo mi = kvp.Key.First;
                UnityEngine.Component c = clone.GetComponentsInChildren<UnityEngine.Component>()[kvp.Value];
                if (mi is PropertyInfo)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    try
                    {
                        pi.SetValue(kvp.Key.Second, c, new object[0]);
                    }
                    catch { }
                }
                else if (mi is FieldInfo)
                {
                    FieldInfo fi = mi as FieldInfo;
                    fi.SetValue(kvp.Key.Second, c);
                }
            }
            foreach (KeyValuePair<Tuple<MemberInfo, object>, int> kvp in gameObjectsInChildren)
            {
                MemberInfo mi = kvp.Key.First;
                GameObject go = clone.transform.GetChildObjects()[kvp.Value];
                if (mi is PropertyInfo)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    try
                    {
                        pi.SetValue(kvp.Key.Second, go, new object[0]);
                    }
                    catch { }
                }
                else if (mi is FieldInfo)
                {
                    FieldInfo fi = mi as FieldInfo;
                    fi.SetValue(kvp.Key.Second, go);
                }
            }
            foreach (Tuple<MemberInfo, object> t in gameObjects)
            {
                MemberInfo mi = t.First;
                GameObject go = clone;
                if (mi is PropertyInfo)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    try
                    {
                        pi.SetValue(t.Second, go, new object[0]);
                    }
                    catch { }
                }
                else if (mi is FieldInfo)
                {
                    FieldInfo fi = mi as FieldInfo;
                    fi.SetValue(t.Second, go);
                }
            }
            return clone;
        }

        private static GameObject CloneChild(GameObject toClone, GameObject ultimateParent, out Dictionary<Tuple<MemberInfo, object>, int> componentsInChildren, out Dictionary<Tuple<MemberInfo, object>, int> gameObjectsInChildren, out List<Tuple<MemberInfo,
            object>> gameObjects)
        {
            GameObject clone = BuildObject(toClone.name);// + " Clone");
            clone.transform.parent = ultimateParent.transform;
            componentsInChildren = new Dictionary<Tuple<MemberInfo, object>, int>();
            gameObjectsInChildren = new Dictionary<Tuple<MemberInfo, object>, int>();
            gameObjects = new List<Tuple<MemberInfo, object>>();
            foreach (UnityEngine.Component c in toClone.GetComponents<UnityEngine.Component>())
            {
                if (clone.GetComponent(c.GetType()) == null)
                {
                    clone.AddComponent(c.GetType());
                }
                foreach (PropertyInfo inf in c.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (inf.CanWrite && inf.CanRead)
                    {
                        try
                        {
                            object val = inf.GetValue(c, new object[0]);
                            bool overrideSet = false;
                            if (val is UnityEngine.Component)
                            {
                                if (ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                                {
                                    componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                                    overrideSet = true;
                                }
                            }
                            else if (val is GameObject go)
                            {
                                if (ultimateParent.transform.GetChildObjects().Contains(val as GameObject))
                                {
                                    gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.transform.GetChildObjects().IndexOf(val as GameObject));
                                    overrideSet = true;
                                }
                                else if (go == toClone.gameObject)
                                {
                                    gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                                }
                            }
                            if (!overrideSet)
                            {
                                inf.SetValue(clone.GetComponent(c.GetType()), val, new object[0]);
                            }
                        }
                        catch { }
                    }
                }
                foreach (FieldInfo inf in c.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    object val = inf.GetValue(c);
                    bool overrideSet = false;
                    if (val is UnityEngine.Component)
                    {
                        if (ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                        {
                            componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                            overrideSet = true;
                        }
                    }
                    else if (val is GameObject go)
                    {
                        if (ultimateParent.transform.GetChildObjects().Contains(val as GameObject))
                        {
                            gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.transform.GetChildObjects().IndexOf(val as GameObject));
                            overrideSet = true;
                        }
                        else if (go == toClone.gameObject)
                        {
                            gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                        }
                    }
                    if (!overrideSet)
                    {
                        inf.SetValue(clone.GetComponent(c.GetType()), val);
                    }
                }
                foreach (Type inter in c.GetType().GetInterfaces())
                {
                    foreach (PropertyInfo inf in inter.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (inf.CanWrite && inf.CanRead)
                        {
                            try
                            {
                                object val = inf.GetValue(c, new object[0]);
                                bool overrideSet = false;
                                if (val is UnityEngine.Component)
                                {
                                    if (ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().Contains(val as UnityEngine.Component))
                                    {
                                        componentsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.GetComponentsInChildren<UnityEngine.Component>().ToList().IndexOf((val as UnityEngine.Component)));
                                        overrideSet = true;
                                    }
                                }
                                else if (val is GameObject go)
                                {
                                    if (ultimateParent.transform.GetChildObjects().Contains(val as GameObject))
                                    {
                                        gameObjectsInChildren.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())), ultimateParent.transform.GetChildObjects().IndexOf(val as GameObject));
                                        overrideSet = true;
                                    }
                                    else if (go == toClone.gameObject)
                                    {
                                        gameObjects.Add(new Tuple<MemberInfo, object>(inf, clone.GetComponent(c.GetType())));
                                    }
                                }
                                if (!overrideSet)
                                {
                                    inf.SetValue(clone.GetComponent(c.GetType()), val, new object[0]);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            foreach (Transform child in toClone.transform)
            {
                GameObject childClone = CloneChild(child.gameObject, clone, out var cip, out var goip, out var gos);
                componentsInChildren.AddRange(cip);
                gameObjectsInChildren.AddRange(goip);
                gameObjects.AddRange(gos);
                childClone.transform.parent = clone.transform;
                childClone.transform.localPosition = child.localPosition;
                childClone.transform.localRotation = child.localRotation;
                childClone.transform.localScale = child.localScale;
            }
            return clone;
        }

        private static bool m_initialized;
        private static int m_bundlesBuilt;
        private static List<AssetBundle> m_assetBundles;
        public static List<GameObject> builtObjects;
    }
}
