using BepInEx;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using BepInEx.Bootstrap;
using System.Collections.Generic;
using GreenHellVR_Core;
using UnityEngine.Events;
using BepInEx.Logging;

namespace GreenHellVR_Core
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public string modPath;
        public Assembly assembly;
        internal static ManualLogSource Log;

        public UnityAction<Scene, LoadSceneMode> SceneManager_sceneLoaded { get; private set; }

        public void Awake()
        {
            // Plugin startup logic
            instance = this;
            modPath = Path.GetDirectoryName(Info.Location);
            assembly = Assembly.GetExecutingAssembly();
            GHVRC_Objects.ExtractEmbeddedRessources();

            Harmony harmony = new("com.thalium.ghvr_core");
            Log = Logger;

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
        }

        /// <summary>
        /// Creates a gameobject and attaches the CoreModObject component to is so we can interact with the game
        /// </summary>
        public void InstatiateModManager()
        {
            Logger.LogInfo($"Instantiating Manager gameobject");

            GameObject modManager = Instantiate(new GameObject("[ModManager] CoreMod"));
            modManager.AddComponent<CoreModObject>();
            DontDestroyOnLoad(modManager);

            Logger.LogInfo($"Manager gameobject created");
        }
    }
}


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

        /*bool flag2 = false; // just for fun but it doesn't work (default: false)
        __instance.m_Singleplayer.gameObject.SetActive(flag2);
        __instance.m_Multiplayer.gameObject.SetActive(flag2);
        __instance.m_StartStory.gameObject.SetActive(!flag2);
        __instance.m_StartSurvival.gameObject.SetActive(!flag2);
        __instance.m_StartChallenge.gameObject.SetActive(!flag2);
        __instance.m_LoadGame.gameObject.SetActive(!flag2);*/

        Transform QuitBtn = __instance.m_Quit.transform;
        VRCanvasHelper canvasHelper = VRCanvasHelper.Get();



        GameObject ModsCanvasGO = Object.Instantiate(new GameObject("modsCanvas"));
        RectTransform ModsCanvasRect = ModsCanvasGO.AddComponent<RectTransform>();
        Canvas ModsCanvasComp = ModsCanvasGO.AddComponent<Canvas>();

        Plugin.Log.LogInfo("ModsCanvas created");

        canvasHelper.SetupVRCanvas(ModsCanvasComp, ModsCanvasRect, new Vector3(0.003f, 0.003f, 0.003f));
        canvasHelper.SetVrCanvasToPositon(ModsCanvasRect, new Vector3(81.178f, 97.683f, 182.291f), new Vector3(0f, -135f, 0f));

        Plugin.Log.LogInfo("ModsCanvas set up");

        GameObject modsListBtnGO = GHVRC_UI.CreateButton("Mods list", QuitBtn.parent, () => Plugin.Log.LogInfo("Button pressed"));
        modsListBtnGO.transform.position = Vector3.down * 2;

        Plugin.Log.LogInfo("mods list button created at " + modsListBtnGO.transform.position.ToString());

        __instance.m_GameVersion.text += $" (MODDED)\n{LoadedPlugins.Count} mods loaded";
    }
}

[HarmonyPatch(typeof(MainMenu), "UpdateState")]
class MainMenu_UpdateState_Fix
{
    static bool Prefix(ref MainMenuState ___m_State, MainMenu __instance)
    {

        bool SkipIntro = true;
        if (SkipIntro && (___m_State == MainMenuState.CompanyLogo || ___m_State == MainMenuState.SecondCompanyLogo || ___m_State == MainMenuState.GameLogo))
        {
            ___m_State = MainMenuState.MainMenu;
            VrInputManager.Get().enabled = false;
            GreenHellGame.GetVRCanvasHelper().SetVrCanvasToPositon(MainMenuManager.Get().m_RectTransform, new Vector3(82.178f, 97.683f, 182.291f), new Vector3(0f, -135f, 0f));
            __instance.m_BG.gameObject.SetActive(value: true);
            __instance.m_CompanyLogo.gameObject.SetActive(value: false);
            __instance.m_SecondCompanyLogo.gameObject.SetActive(value: false);
            __instance.m_GameLogo.gameObject.SetActive(value: false);
            __instance.m_Buttons.SetActive(value: true);
            Color black = __instance.m_BG.color;
            black.a = 1f;
            __instance.m_BG.color = black;
            GreenHellGame.Instance.m_Settings.ApplySettings();
            GreenHellGame.Instance.m_InitialSequenceComplete = true;
            Plugin.Log.LogInfo("Intro skipped");
        }

        Logger.Log("UpdateState fix");
        return true;
    }
}


/*
if (pos == null){
    Vector3 forward;
    Vector3 vector;
    forward = Player.Get().GetHeadTransform().forward;
    vector = Player.Get().GetHeadTransform().position + 0.5f * forward;
    
    vector = (Physics.Raycast(vector, forward, out RaycastHit raycastHit, 3f) ? raycastHit.point : (vector + forward * 2f));
    ItemsManager.Get().CreateItem(ItemID.metal_axe, true, vector - forward * 0.2f, Player.Get().transform.rotation);
}
 */