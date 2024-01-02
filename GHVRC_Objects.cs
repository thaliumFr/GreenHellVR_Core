using Enums;
using System;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Reflection;

namespace GreenHellVR_Core
{
    public static class GHVRC_Objects
    {
        static readonly string BundlesFolder = Path.Combine(Directory.GetParent(Plugin.instance.modPath).FullName, "Bundles");

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
        /// Extracts all the embedded ressources in compilation in the same folder the mod .dll is in
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
                FileStream fileStream = new FileStream(Path.Combine(BundlesFolder, asset), FileMode.Create);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();
            }
        }

        #region LoadAssetBundle
        /// <summary>
        /// Load an asset bundle from the computer asynchronously
        /// </summary>
        /// <param name="path">Absolute path to the location of the Asset Bundle</param>
        /// <returns>AssetBundle</returns>
        [Obsolete("it's not really obselete but async methods just doesn't work for now")]
        public static IEnumerator LoadAssetBundleAsync(string path, Action<AssetBundle> callback)
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

            Plugin.Log.LogInfo($"Bundle {bundleRequest.assetBundle.name} loaded");
            callback(bundleRequest.assetBundle);
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
            Plugin.Log.LogInfo($"Bundle {Bundle.name} loaded");
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
        public static IEnumerator LoadAssetFromBundleAsync<T>(AssetBundle bundle, string AssetName, Action<T> callback) where T : Object
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<T>(AssetName);
            yield return new WaitUntil(() => request.isDone);
            Plugin.Log.LogInfo($"Asset loaded");
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
        public static IEnumerator LoadAssetFromBundleAsync(AssetBundle bundle, string AssetName)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(AssetName);
            yield return new WaitUntil(() => request.isDone);
            Plugin.Log.LogInfo($"Asset loaded");
            yield return request.asset;
        }

        public static T LoadAssetFromBundle<T>(AssetBundle bundle, string AssetName) where T : Object
        {
            T asset = bundle.LoadAsset<T>(AssetName);
            Plugin.Log.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }

        public static Object LoadAssetFromBundle(AssetBundle bundle, string AssetName)
        {
            Object asset = bundle.LoadAsset(AssetName);
            Plugin.Log.LogInfo($"Asset {AssetName} loaded");
            return asset;
        }
        #endregion
        public static void SpawnGHVRObject(ItemID id, Transform pos)
        {
            Item item = ItemsManager.Get().CreateItem(id, true, pos);
            Plugin.Log.LogInfo(id.ToString() + " has been created");
        }

        public static void SpawnGHVRObject(ItemID id, Vector3 pos, Quaternion rot)
        {
            ItemsManager.Get().CreateItem(id, true, pos, rot);
            Plugin.Log.LogInfo(id.ToString() + " has been created");
        }
    }
}
