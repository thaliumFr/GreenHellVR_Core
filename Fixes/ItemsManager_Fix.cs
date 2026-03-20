using GreenHellVR_Core.Items;
using GreenHellVR_Core.Systems;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenHellVR_Core.Fixes
{
    [HarmonyPatch(typeof(ItemsManager), "Initialize")]
    public class ItemsManager_Initialize_Fix
    {
        public static void Postfix()
        {
            GHVRC_ItemsManager.Initialize();
        }
    }

    [HarmonyPatch(typeof(ItemsManager), "OnObjectMoved")]
    public class ItemsManager_OnObjectMovedError_Fix
    {
        public static bool Prefix(Item item)
        {
            string key = item?.name?? "null";
            if (item == null)
            {
                
                
            }
            if (CoreModObject.itemCounts.ContainsKey(key))
            {
                CoreModObject.itemCounts[key]++;
            }
            else
            {
                CoreModObject.itemCounts.Add(key, 1);
            }
            return item != null;
        }
    }

    [HarmonyPatch(typeof(ItemsManager), "InitCraftingData")]
    public class ItemsManager_InitCraftingData_Fix
    {
        public static void Prefix(ItemsManager __instance, ref Dictionary<int, ItemInfo> ___m_ItemInfos)
        {
            Plugin.Log.LogInfo("Patching ItemsManager.InitCraftingData to fix crafting recipes...");

            foreach (CraftingSystems.Receipe receipe in CraftingSystems.ModReceipes)
            {
                ItemInfo item = ___m_ItemInfos[(int)receipe.Result];
                Dictionary<int, int> ingredients = receipe.Ingredients.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value);

                item.m_Components = (DictionaryOfIntAndInt)ingredients;


            }
        }
    }
}
