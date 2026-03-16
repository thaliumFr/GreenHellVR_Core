using Enums;
using GreenHellVR_Core;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using GreenHellVR_Core.Items;

#pragma warning disable CS0436 // Type conflicts with imported type

namespace GreenHellVR_Core_Include.Items.Objects
{
    public static class GHVRC_Objects
    {
        public static readonly string BundlesFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if !INCLUDE
        /// <summary>
        /// Creates a gameobject and attaches the CoreModObject component to is so we can interact with the game
        /// </summary>
        public static void InstatiateModManager()
        {
            Plugin.Log.LogInfo($"Instantiating Manager gameobject");

            GameObject modManager = Object.Instantiate(new GameObject("[ModManager] CoreMod"));
            modManager.AddComponent<CoreModObject>();

            Plugin.Log.LogInfo($"Manager gameobject created");
        }
#endif

        /// <summary>
        /// Checks if the Asset bundle directory exists and if not it will create one
        /// </summary>
        static void CheckAssetBundle()
        {
            Plugin.Log.LogInfo($"Checking for the bundle folder");
            if (!Directory.Exists(BundlesFolder))
            {
                Plugin.Log.LogInfo($"Creating the bundles folder");
                Directory.CreateDirectory(BundlesFolder);
            }
        }

        /// <summary>
        /// Extracts all the embedded ressources in compilation to the Asset bundles folder
        /// </summary>
        public static void ExtractEmbeddedRessources()
        {
            CheckAssetBundle();
            Assembly assembly = Assembly.GetCallingAssembly();
            Plugin.Log.LogInfo($"Extracting Assets from {assembly.FullName}");
            foreach (string asset in assembly.GetManifestResourceNames())
            {
                Plugin.Log.LogInfo($"Extracting {asset}");
                Stream stream = assembly.GetManifestResourceStream(asset);
                FileStream fileStream = new(Path.Combine(BundlesFolder, asset), FileMode.Create);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();
            }
        }

        public static void ToggleActive(GameObject go)
        {
            if (go == null) return;
            go.SetActive(!go.activeSelf);
            Plugin.Log.LogDebug($"{go.name} is now {(go.activeSelf ? "active" : "inactive")}");
        }

        #region LoadAssetBundle
        /// <summary>
        /// Load an asset bundle from the computer asynchronously
        /// </summary>
        /// <param name="path">path to the bundle (ex: MyNamespace.myBundle)</param>
        /// <returns>AssetBundle</returns>
        public static IEnumerator LoadAssetBundleAsync(string path, Action<AssetBundle> callback = null)
        {
            string fullPath = Path.Combine(BundlesFolder, path);
            Plugin.Log.LogInfo($"Checking for bundle: {fullPath}");
            if (path == null || !File.Exists(fullPath))
            {
                Logger.LogError("Bundle not found");
                yield return null;
            }

            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(fullPath);
            yield return new WaitUntil(() => bundleRequest.isDone);

            if (bundleRequest.assetBundle == null)
            {
                Logger.LogError("Bundle not found");
                yield return null;
            }

            Plugin.Log.LogInfo($"Bundle {bundleRequest.assetBundle.name} loaded");
            callback?.Invoke(bundleRequest.assetBundle);
            yield return null;

        }

        /// <summary>
        /// Load an asset bundle from the computer
        /// </summary>
        /// <param name="path">path to the bundle (ex: MyNamespace.myBundle)</param>
        /// <returns>AssetBundle</returns>
        public static AssetBundle LoadAssetBundle(string path)
        {
            string fullPath = Path.Combine(BundlesFolder, path);
            Plugin.Log.LogInfo($"Checking for bundle: {fullPath}");
            if (path == null || !File.Exists(fullPath))
            {
                Logger.LogError("Bundle not found");
                return null;
            }

            AssetBundle Bundle = AssetBundle.LoadFromFile(fullPath);

            if (Bundle == null)
            {
                Logger.LogError("Bundle not found");
                return null;
            }

            Plugin.Log.LogInfo($"Bundle {Bundle.name} loaded");
            return Bundle;
        }

        #endregion

        #region Load Asset from Bundle
        /// <summary>
        /// Loads an asset from a previously loaded bundle, but asynchronously. Bundle will be sent into the callback once loaded
        /// </summary>
        /// <typeparam name="T">Type of the object you want to get returned - needs to inherit from UnityEngine.Object</typeparam>
        /// <param name="bundle">The Bundle you want to load the asset from</param>
        /// <param name="AssetName">the name of the asset (ex: monkey)</param>
        /// <returns>Asset of type given type T</returns>
        public static IEnumerator LoadAssetFromBundleAsync<T>(AssetBundle bundle, string AssetName, Action<T> callback) where T : Object
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<T>(AssetName);
            yield return new WaitUntil(() => request.isDone);
            Plugin.Log.LogInfo($"Asset loaded");
            callback?.Invoke((T)request.asset);
            yield return null;
        }

        /// <summary>
        /// Loads an asset from a previously loaded bundle, but asynchronously. Bundle will be sent into the callback once loaded
        /// </summary>
        /// <typeparam name="T">Type of the object you want to get returned - needs to inherit from UnityEngine.Object</typeparam>
        /// <param name="bundle">The Bundle you want to load the asset from</param>
        /// <param name="AssetName">the name of the asset (ex: monkey)</param>
        /// <returns>Asset of type given type T</returns>
        public static T LoadAssetFromBundle<T>(AssetBundle bundle, string AssetName) where T : Object
        {
            T asset = bundle.LoadAsset<T>(AssetName);
            Plugin.Log.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }

        /// <summary>
        /// Loads an asset from a previously loaded bundle. Bundle will be sent into the callback once loaded
        /// </summary>
        /// <param name="bundle">The Bundle you want to load the asset from</param>
        /// <param name="AssetName">the name of the asset (ex: monkey)</param>
        /// <returns>Asset of generic type UnityEngine.Object</returns>
        public static Object LoadAssetFromBundle(AssetBundle bundle, string AssetName)
        {
            Object asset = bundle.LoadAsset(AssetName);
            Plugin.Log.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }
        #endregion


    }
}
