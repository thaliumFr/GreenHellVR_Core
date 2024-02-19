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
using System;
using Object = UnityEngine.Object;
using BepInEx.Configuration;



namespace GreenHellVR_Core
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public string modPath;
        public Assembly assembly;
        internal static ManualLogSource Log;

        // Events
        [Header("Events")]
        public Action OnDie;
        public UnityAction<Scene, LoadSceneMode> SceneManager_sceneLoaded { get; private set; }

        public void Awake()
        {
            // Plugin startup logic
            instance = this;
            Log = Logger;
            modPath = Path.GetDirectoryName(Info.Location);
            assembly = Assembly.GetExecutingAssembly();
            GHVRC_Objects.ExtractEmbeddedRessources();


            Harmony harmony = new("com.thalium.ghvr_core");

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager_sceneLoaded += onSceneLoaded;
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Load Config
            ConfigManager.Awake();
            ConfigManager.LoadConfig();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");

            InstatiateModManager();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Log.LogInfo("Force pause menu");
                MenuInGameManager.Get().ForcePauseMenu();
            }
        }

        private void onSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            Logger.LogInfo($"Scene {scene.name} loaded");

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

        bool flag2 = ConfigManager.Instance.ConfigFullUI.Value; // just for fun but it doesn't work (default: false)
        __instance.m_Singleplayer.gameObject.SetActive(flag2);
        __instance.m_Multiplayer.gameObject.SetActive(flag2);
        __instance.m_StartStory.gameObject.SetActive(!flag2);
        __instance.m_StartSurvival.gameObject.SetActive(!flag2);
        __instance.m_StartChallenge.gameObject.SetActive(!flag2);
        __instance.m_LoadGame.gameObject.SetActive(!flag2);

        // Create Mods List canvas

        /// Title label

        GameObject titleGO = GHVRC_UI.CreateText($"Loaded Mods :", __instance.m_Buttons.transform.parent);
        TMP_Text titleTxt = titleGO.GetComponent<TMP_Text>();
        titleGO.transform.rotation = __instance.m_Buttons.transform.rotation;
        titleGO.transform.position = __instance.m_Continue.transform.position;
        GHVRC_UI.CopyTextProperties(__instance.m_Quit.GetComponentInChildren<TMP_Text>(), ref titleTxt);
        titleGO.transform.Translate(Vector3.left * 0.5f, Space.Self);
        Plugin.Log.LogInfo($"Adding Title to the mods list at coords [{titleGO.transform.position}]");

        /// Loaded Mods

        int i = 1;
        foreach (var item in LoadedPlugins)
        {
            GameObject txtGO = GHVRC_UI.CreateText($"- {item.Key}", __instance.m_Buttons.transform.parent);
            TMP_Text txt = txtGO.GetComponent<TMP_Text>();
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

[HarmonyPatch(typeof(MainMenu), "UpdateState")]
class MainMenu_UpdateState_Fix
{
    static bool Prefix(ref MainMenuState ___m_State, MainMenu __instance)
    {
        bool SkipIntro = ConfigManager.Instance.ConfigSkipIntro.Value;
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
            GreenHellGame.Instance.m_InitialSequenceComplete = SkipIntro;
            Plugin.Log.LogInfo("Intro skipped");
        }

        Logger.Log("UpdateState fix");
        return true;
    }
}


[HarmonyPatch(typeof(Player), nameof(Player.Die))]
class Player_Die_OnDie
{
    static void Postfix()
    {
        Plugin.instance.OnDie();
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