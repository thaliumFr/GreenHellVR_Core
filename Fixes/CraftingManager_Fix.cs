using Enums;
using GreenHellVR_Core.Items;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHellVR_Core.Fixes
{
    [HarmonyPatch(typeof(CraftingManager), "CheckResult")]
    class CraftingManager_Fix
    {
        [HarmonyPostfix]
        static void Postfix(CraftingManager __instance)
        {
            /*
            bool flag = true;
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            foreach (ItemInfo itemInfo in Items.GHVRC_ItemsManager.customItems)
            {
                Dictionary<int, int> components = itemInfo.m_Components;
                using (Dictionary<int, int>.KeyCollection.Enumerator enumerator3 = dictionary.Keys.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        ItemID itemID = (ItemID)enumerator3.Current;
                        if (!components.ContainsKey((int)itemID) || dictionary[(int)itemID] > components[(int)itemID])
                        {
                            flag = false;
                        }
                    }
                }

                if (flag)
                {
                    __instance.m_Results.Add(itemInfo);
                }
            }
            */
        }


    }
}
