using MonoMod.RuntimeDetour;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.IO;

namespace ModdedItemWeightBalancer
{
    class ModdedItemWeightController
    {

        public static bool MIWCFullyInited;
        //public static bool MIWCEnabled;
        public static Dictionary<PickupObject, Assembly> MIWCModItemDict;
        public static Dictionary<MIWCData, ETGModule> MIWCActualModItemDict;

        public static Dictionary<string, int> moddedItemCount;

        public static void Init()
        {
            try
            {
                new Hook(typeof(ItemDB).GetMethod("Add", new Type[] { typeof(PickupObject), typeof(bool), typeof(string) }), typeof(ModdedItemWeightController).GetMethod("MIWCAddItemToDict"));
                moddedItemCount = new Dictionary<string, int>();
                MIWCModItemDict = new Dictionary<PickupObject, Assembly>();
                ETGMod.StartGlobalCoroutine(DelayedDictionaryInit());
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
            }
           
        }

        public static IEnumerator DelayedDictionaryInit()
        {
            yield return null;
            MIWCFullyInited = true;
            MIWCGetActualModItemDict();
        }


        public static void MIWCAddItemToDictWithoutHook(PickupObject item)
        {

            StackFrame[] frames = new StackTrace().GetFrames();
            int current = 1;
            while (frames[current].GetMethod().DeclaringType.Assembly == typeof(ETGMod).Assembly)
            {
                current++;
                if (current >= frames.Length)
                {
                    
                }
            }
            if (!MIWCModItemDict.ContainsKey(item))
            {
                MIWCModItemDict.Add(item, frames[current].GetMethod().DeclaringType.Assembly);
            }
            if (MIWCFullyInited)
            {
                MIWCGetActualModItemDict();
            }
        }

        public static int MIWCAddItemToDict(Func<ItemDB, PickupObject, bool, string, int> orig, ItemDB self, PickupObject item, bool b, string floor)
        {

            

            int result = orig(self, item, b, floor);

            StackFrame[] frames = new StackTrace().GetFrames();
            int current = 1;
            while (frames[current].GetMethod().DeclaringType.Assembly == typeof(ETGMod).Assembly)
            {
                current++;
                if (current >= frames.Length)
                {
                    return result;
                }
            }
            if (!MIWCModItemDict.ContainsKey(item))
            {
                MIWCModItemDict.Add(item, frames[current].GetMethod().DeclaringType.Assembly);
            }
            if (MIWCFullyInited)
            {
                MIWCGetActualModItemDict();
            }
            return result;
        }

        public static bool MIWCGetActualModItemDict()
        {
            if (MIWCModItemDict == null)
            {
                ETGModConsole.Log("[MIWC] Error! Tried getting actual mod item dict when normal mod item dict is null!");
                return false;
            }
            MIWCActualModItemDict = new Dictionary<MIWCData, ETGModule>();
            foreach (KeyValuePair<PickupObject, Assembly> pair in MIWCModItemDict)
            {
                foreach (ETGModule module in ETGMod.AllMods)
                {
                    if (module.GetType().Assembly == pair.Value)
                    {
                        try
                        {
                            //ETGModConsole.Log(pair.Key.EncounterNameOrDisplayName);
                            MIWCActualModItemDict.Add(new MIWCData
                            {
                                encounterOrDisplayName = pair.Key.EncounterNameOrDisplayName,
                                objectName = pair.Key.name.Replace("(Clone)", ""),
                                type = pair.Key.GetType(),
                                baseItem = pair.Key,
                                id =
                                pair.Key.PickupObjectId
                            }, module);
                        }
                        catch { }
                        break;
                    }
                }
            }
            return true;
        }



        public static void CheckModdedItems()
        {
            moddedItemCount = new Dictionary<string, int>();
            foreach (var player in GameManager.Instance.AllPlayers)
            {
                foreach(var item in GetAllPlayerItems(player))
                {
                    if (MIWCActualModItemDict == null)
                    {
                        if (!MIWCGetActualModItemDict())
                        {
                            return;
                        }
                    }
                    
                    MIWCData match = GetMatch(item);
                    /*if (match != null)
                    {
                        ETGModConsole.Log($"match not null");
                    }

                    if (MIWCActualModItemDict.ContainsKey(match))
                    {
                        ETGModConsole.Log($"has the key");
                    }

                    if (MIWCActualModItemDict[match].Metadata != null)
                    {
                        ETGModConsole.Log($"meta data is there");
                    }

                    if (!string.IsNullOrEmpty(MIWCActualModItemDict[match].Metadata.Name))
                    {
                        ETGModConsole.Log($"name exists");
                    }*/


                    if (match != null && MIWCActualModItemDict.ContainsKey(match) && MIWCActualModItemDict[match] != null && MIWCActualModItemDict[match].Metadata != null && !string.IsNullOrEmpty(MIWCActualModItemDict[match].Metadata.Name))
                    {
                        if (moddedItemCount.ContainsKey(MIWCActualModItemDict[match].Metadata.Name))
                        {
                            moddedItemCount[MIWCActualModItemDict[match].Metadata.Name] += 1;
                        }
                        else
                        {
                            moddedItemCount.Add(MIWCActualModItemDict[match].Metadata.Name, 1);
                        }
                        //ETGModConsole.Log($"added item to modded list {MIWCActualModItemDict[match].Metadata.Name}");
                        //MIWCActualModItemDict[match].Metadata.Name
                    }
                    else if (item.PickupObjectId <= 823)
                    {
                        if (moddedItemCount.ContainsKey("Gungeon"))
                        {
                            moddedItemCount["Gungeon"] += 1;
                        }
                        else
                        {
                            moddedItemCount.Add("Gungeon", 1);
                        }
                    }
                }
            }

            foreach(var info in moddedItemCount)
            {
                ETGModConsole.Log($"[{info.Key}]: {info.Value}");
            }
        }


        public static List<PickupObject> GetAllPlayerItems(PlayerController player)
        {
            var items = new List<PickupObject>();

            foreach (var item in player.passiveItems)
            {
                if (item != null)
                {
                    items.Add(item);
                }
            }
            foreach (var item in player.activeItems)
            {
                if (item != null)
                {
                    items.Add(item);
                }
            }
            foreach (var item in player.inventory.AllGuns)
            {
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        public static MIWCData GetMatch(PickupObject po)
        {
            if (MIWCActualModItemDict == null)
            {
                if (!MIWCGetActualModItemDict())
                {
                    ETGModConsole.Log("god fucking damn it");
                    return null;
                }
            }
            foreach (MIWCData data in MIWCActualModItemDict.Keys)
            {
                if (data.encounterOrDisplayName == po.EncounterNameOrDisplayName && data.objectName == po.name.Replace("(Clone)", "") && data.type == po.GetType() && data.id == po.PickupObjectId)
                {
                    return data;
                }
            }
            return null;
        }

    }



    public class MIWCData
    {
        public string objectName;
        public Type type;
        public string encounterOrDisplayName;
        public int id;
        public PickupObject baseItem;
    }
}
