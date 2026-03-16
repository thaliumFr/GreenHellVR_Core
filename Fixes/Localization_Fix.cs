using GreenHellVR_Core.Systems;
using GreenHellVR_Core_Include.Items.Objects;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace GreenHellVR_Core.Fixes
{
    [HarmonyPatch(typeof(Localization), "ParseScript", [])]
    class Localization_Fix
    {
        public static void Postfix(Localization __instance)
        {
            LocalizationSystem.ReadAllLocalizationFiles();

            Traverse localizationTraverse = Traverse.Create(__instance).Field("m_LocalizedTexts");
            SortedDictionary<string, string> localizedTexts = localizationTraverse.GetValue<SortedDictionary<string, string>>();

            // Add custom localized texts to the game's localization system
            if (localizedTexts == null) return;
            foreach (var localVal in LocalizationSystem.localizationDictionary)
            {
                if (!localizedTexts.ContainsKey(localVal.Key))
                {
                    localizedTexts.Add(localVal.Key, localVal.Value);
                    Plugin.Log.LogInfo($"Added localization: {localVal.Key} = {localVal.Value}");
                }
            }

            localizationTraverse.SetValue(localizedTexts);
        }
    }
}
