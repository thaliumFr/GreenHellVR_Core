using BepInEx;
using BepInEx.Logging;
using GreenHellVR_Core.Items;
using GreenHellVR_Core.Systems;
using GreenHellVR_Core_Include.Items.Objects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        public static UnityAction<Scene, LoadSceneMode> SceneManager_sceneLoaded { get; private set; }

        public void Awake()
        {
            // Plugin startup logic
            instance = this;
            Log = Logger;
            modPath = Path.GetDirectoryName(Info.Location);
            assembly = Assembly.GetExecutingAssembly();

#if !INCLUDE
            Harmony harmony = new("com.thalium.ghvr_core");

            SceneManager.sceneLoaded += OnSceneLoaded;
            
            harmony.PatchAll(instance.assembly);

            // Load Config
            ConfigManager.Awake();
            ConfigManager.LoadConfig();


            GHVRC_Objects.ExtractEmbeddedRessources();
            GHVRC_Objects.InstatiateModManager();

            GHVRC_ItemsManager.Initialize();

            TestReceipes();
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME}[{PluginInfo.PLUGIN_VERSION}] is loaded!");
#endif
        }

        public void TestReceipes()
        {
            CraftingSystems.AddReceipe(Enums.ItemID.WalkieTalkie, new Dictionary<Enums.ItemID, int>
            {
                { Enums.ItemID.Small_Stick, 2 },
            });
        }

#if !INCLUDE
        private static void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            Log.LogInfo($"Scene {scene.name} loaded");
            //Plugin.SceneManager_sceneLoaded.Invoke(scene, sceneMode);
            GHVRC_Objects.InstatiateModManager();
        }
#endif
    }
}