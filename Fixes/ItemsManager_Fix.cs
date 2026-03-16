using GreenHellVR_Core.Items;
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
}
