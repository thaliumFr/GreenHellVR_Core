using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using TMPro;
using BepInEx.Bootstrap;
using System.Collections.Generic;
using GreenHellVR_Core;
using UnityEngine.UI;
using Enums;

namespace GreenHellVR_Core
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        string modPath;
        Assembly assembly;

        private Canvas debugCanvas;

        public void Awake()
        {
            // Plugin startup logic
            instance = this;
            modPath = Path.GetDirectoryName(Info.Location);
            assembly = Assembly.GetExecutingAssembly();
            ExtractEmbeddedRessources();

            Harmony harmony = new("com.thalium.ghvr_core");

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");

            string ScenesNames = "Scenes :\n";
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                ScenesNames += $"Scene {i} : {SceneManager.GetSceneAt(i).name}\n";
            }
            Logger.LogInfo(ScenesNames);
            
        }

        private void CreateDebugCanvas()
        {
            GameObject debugCanvasGO = new GameObject("debugCanvas");
            Canvas debugcanvas = debugCanvasGO.AddComponent<Canvas>();
            debugcanvas.renderMode = RenderMode.ScreenSpaceCamera;
            debugCanvas.worldCamera = Camera.main;

            Button modsListBtn = GreenHellVR_Core_UI.CreateButton("Mods list");
            modsListBtn.onClick.AddListener(() => Debug.Log("Button pressed"));
        }

        /// <summary>
        /// Creates a gameobject and attaches the CoreModObject component to is so we can interact with the game
        /// </summary>
        private void InstatiateModManager()
        {
            Logger.LogInfo($"Instantiating Manager gameobject");

            GameObject modManager = Instantiate(new GameObject("[ModManager] CoreMod"));
            modManager.AddComponent<CoreModObject>();
            DontDestroyOnLoad(modManager);

            Logger.LogInfo($"Manager gameobject created");
        }

        /// <summary>
        /// Extracts all the embedded ressources in compilation in the same folder the mod .dll is in
        /// </summary>
        private void ExtractEmbeddedRessources()
        {
            Logger.LogInfo("Extracting Assets");
            foreach (string asset in assembly.GetManifestResourceNames())
            {
                Logger.LogInfo($"Extracting {asset}");
                Stream stream = assembly.GetManifestResourceStream(asset);
                FileStream fileStream = new FileStream(Path.Combine(modPath, asset), FileMode.Create);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();
            }
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Logger.LogInfo($"New scene loaded: {scene.name} (ID {scene.buildIndex})");
            CoreModObject[] obj = FindObjectsOfType<CoreModObject>();

            if (obj.Length == 0)
            {
                InstatiateModManager();
            }
            Logger.LogInfo($"ModManagers length : {obj.Length}");

            if (scene.name == "MainMenu")
            {
                
            }
        }

        #region LoadAssetBundle
        /// <summary>
        /// Load an asset bundle from the computer asynchronously
        /// </summary>
        /// <param name="path">Absolute path to the location of the Asset Bundle</param>
        /// <returns>AssetBundle</returns>
        [Obsolete("it's not really obselete but async methods just doesn't work for now")]
        public IEnumerator LoadAssetBundleAsync(string path, Action<AssetBundle> callback)
        {
            string fullPath = Path.Combine(modPath, path);
            Logger.LogInfo($"Checking for bundle: {fullPath}");
            if (path == null || !File.Exists(fullPath))
            {
                Logger.LogError("Bundle not found");
                yield return null;
            }

            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(fullPath);
            yield return new WaitUntil(() => bundleRequest.isDone);

            Logger.LogInfo($"Bundle {bundleRequest.assetBundle.name} loaded");
            callback(bundleRequest.assetBundle);
            yield return null;

        }

        /// <summary>
        /// Load an asset bundle from the computer
        /// </summary>
        /// <param name="path">path to the bundle (ex: MyNamespace.myBundle)</param>
        /// <returns>AssetBundle</returns>
        public AssetBundle LoadAssetBundle(string path)
        {
            string fullPath = Path.Combine(modPath, path);
            Logger.LogInfo($"Checking for bundle: {fullPath}");
            if (path == null || !File.Exists(fullPath))
            {
                Logger.LogError("Bundle not found");
                return null;
            }
            
            AssetBundle Bundle = AssetBundle.LoadFromFile(fullPath);
            Logger.LogInfo($"Bundle {Bundle.name} loaded");
            return Bundle;
        }
        #endregion

        #region Load Asset from Bundle
        /// <summary>
        /// Loads an asset from a previously loaded bundle
        /// </summary>
        /// <typeparam name="T">Type of the object you want to get returned - needs to inherit from UnityEngine.Object</typeparam>
        /// <param name="bundle">The Bundle you want to load the asset from</param>
        /// <param name="AssetName">the name of the asset (ex: monkey)</param>
        /// <returns>Asset of type T</returns>
        [Obsolete("it's not really obselete but async methods just doesn't work for now")]
        public IEnumerator LoadAssetFromBundleAsync<T>(AssetBundle bundle, string AssetName, Action<T> callback) where T : Object
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<T>(AssetName);
            yield return new WaitUntil(() => request.isDone);
            Logger.LogInfo($"Asset loaded");
            callback((T)request.asset);
            yield return null;
        }


        /// <summary>
        /// Loads an asset from a previously loaded bundle asynchronously
        /// </summary>
        /// <typeparam name="T">Type of the object you want to get returned - needs to inherit from UnityEngine.Object</typeparam>
        /// <param name="bundle">The Bundle you want to load the asset from</param>
        /// <param name="AssetName">the name of the asset (ex: monkey)</param>
        /// <returns>Asset of type UnityEngine.Object</returns>
        [Obsolete("it's not really obselete but async methods just doesn't work for now")]
        public IEnumerator LoadAssetFromBundleAsync(AssetBundle bundle, string AssetName)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(AssetName);
            yield return new WaitUntil(() => request.isDone);
            Logger.LogInfo($"Asset loaded");
            yield return request.asset;
        }

        public T LoadAssetFromBundle<T>(AssetBundle bundle, string AssetName) where T : Object
        {
            T asset = bundle.LoadAsset<T>(AssetName);
            Logger.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }

        public Object LoadAssetFromBundle(AssetBundle bundle, string AssetName)
        {
            Object asset = bundle.LoadAsset(AssetName);
            Logger.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }
        #endregion
        public void SpawnGHVRObject(ItemID id)
        {
            
        }
    }
}


[HarmonyPatch(typeof(MainMenu), "Start")]
class mainMenuFix
{
    static void Postfix(MainMenu __instance)
    {
        Plugin plugin = Plugin.instance;
        AssetBundle bundle = plugin.LoadAssetBundle("GreenHellVR_Core.monkeybundle");
        TMP_FontAsset fontAsset = plugin.LoadAssetFromBundle<TMP_FontAsset>(bundle, "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        GreenHellVR_Core_UI.defaultFontAsset = fontAsset;

        Button buttonPrefab = plugin.LoadAssetFromBundle<Button>(bundle, "Assets/Button.prefab");
        buttonPrefab.onClick.RemoveAllListeners();
        
        GreenHellVR_Core_UI.buttonPrefab = buttonPrefab;
        Debug.Log("Ressources set up");

        Dictionary<string, BepInEx.PluginInfo> LoadedPlugins = Chainloader.PluginInfos;

        /*bool flag2 = false; // just for fun but it doesn't work (default: false)
        __instance.m_Singleplayer.gameObject.SetActive(flag2);
        __instance.m_Multiplayer.gameObject.SetActive(flag2);
        __instance.m_StartStory.gameObject.SetActive(!flag2);
        __instance.m_StartSurvival.gameObject.SetActive(!flag2);
        __instance.m_StartChallenge.gameObject.SetActive(!flag2);
        __instance.m_LoadGame.gameObject.SetActive(!flag2);*/
        
        Transform QuitBtn = __instance.m_Quit.transform;

        Button modsListBtn = GreenHellVR_Core_UI.CreateButton("Mods list");
        modsListBtn.onClick.AddListener(() => Debug.Log("Button pressed"));
        Object.Instantiate(modsListBtn, QuitBtn.parent);
        modsListBtn.transform.position = QuitBtn.position /*+ Vector3.down * 2*/;

        __instance.m_GameVersion.text += $" (MODDED)\n{LoadedPlugins.Count} mods loaded";

        bundle.Unload(false);
    }
    
}