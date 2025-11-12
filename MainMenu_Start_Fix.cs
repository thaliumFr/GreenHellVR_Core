using BepInEx.Bootstrap;
using GreenHellVR_Core.UI;
using HarmonyLib;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GreenHellVR_Core
{
    [HarmonyPatch(typeof(MainMenu), "Start")]
    class MainMenu_Start_Fix
    {
        static void Postfix(MainMenu __instance)
        {
            Plugin.Log.LogInfo("Setting up MainMenu fix");

            if (GHVRC_UI.defaultFontAsset == null)
            {
                GHVRC_UI.defaultFontAsset = __instance.m_Options.GetComponentInChildren<TextMeshProUGUI>().font;
            }

            Plugin.Log.LogInfo("Setting up MainMenu fix");
            Dictionary<string, BepInEx.PluginInfo> LoadedPlugins = Chainloader.PluginInfos;

            bool flag2 = ConfigManager.Instance.ConfigFullUI.Value; // just for fun but it doesn't work (default: false)
            __instance.m_Singleplayer.gameObject.SetActive(flag2);
            __instance.m_Multiplayer.gameObject.SetActive(flag2);
            __instance.m_StartStory.gameObject.SetActive(!flag2);
            __instance.m_StartSurvival.gameObject.SetActive(!flag2);
            __instance.m_StartChallenge.gameObject.SetActive(!flag2);
            __instance.m_LoadGame.gameObject.SetActive(!flag2);

            // Create Mods List canvas

            /// Title label

            GameObject titleGO = GHVRC_UI.CreateText($"Loaded Mods :", out TextMeshProUGUI titleTxt, __instance.m_Buttons.transform.parent);
            titleGO.transform.rotation = __instance.m_Buttons.transform.rotation;
            titleGO.transform.position = __instance.m_Continue.transform.position;
            GHVRC_UI.CopyTextProperties(__instance.m_Quit.GetComponentInChildren<TMP_Text>(), ref titleTxt);
            titleGO.transform.Translate(Vector3.left * 0.5f, Space.Self);
            Plugin.Log.LogInfo($"Adding Title to the mods list at coords [{titleGO.transform.position}]");

            /// Loaded Mods

            int i = 1;
            foreach (var item in LoadedPlugins)
            {
                GameObject txtGO = GHVRC_UI.CreateText($"- {item.Key}", out TMP_Text txt, __instance.m_Buttons.transform.parent);
                txtGO.transform.rotation = __instance.m_Buttons.transform.rotation;
                txtGO.transform.position = __instance.m_Continue.transform.position;
                GHVRC_UI.CopyTextProperties(__instance.m_Quit.GetComponentInChildren<TMP_Text>(), ref txt);
                txtGO.transform.Translate(Vector3.down * i * 0.05f, Space.Self);
                txtGO.transform.Translate(Vector3.left * 0.6f, Space.Self);
                Plugin.Log.LogInfo($"Adding [{item.Key}] {item.Value} to the mods list at coords [{txtGO.transform.position}]");
                i++;
            }

            // Main menu is at new Vector3(82.178f, 97.683f, 182.291f), new Vector3(0f, -135f, 0f) by default
            Plugin.Log.LogDebug(__instance.m_Buttons.transform.position.ToString());

            __instance.m_GameVersion.text += $" (MODDED)\n{LoadedPlugins.Count} mods loaded";
        }
    }
}
