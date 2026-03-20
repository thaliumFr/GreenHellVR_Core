using GreenHellVR_Core_Include.Items.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GreenHellVR_Core.Items
{
    public class DebugMonkey
    {
        static AssetBundle ModAssetBundle;

        static GameObject monkey;
        static Material debugMaterial;


        static void GetMonkeyBundle(Action<AssetBundle> action)
        {
            if (ModAssetBundle == null)
            {
                Plugin.Log.LogInfo("Loading AssetBundle for DebugMonkey...");
                CoreModObject.Instance.StartCoroutine(GHVRC_Objects.LoadAssetBundleAsync("GreenHellVR_Core.monkeybundle", (bundle) =>
                {
                    ModAssetBundle = bundle;
                    action(bundle);
                }));
            }
            else
            {
                action(ModAssetBundle);
            }
        }

        static void LoadMonkey()
        {
            GetMonkeyBundle((assetBundle) =>
            {
                if (assetBundle != null && monkey == null)
                {
                    CoreModObject.Instance.StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<GameObject>(assetBundle, "monkey", (_monkey) => {
                        monkey ??= _monkey;

                    }));
                    CoreModObject.Instance.StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<Material>(assetBundle, "rainbow.mat", (_mat) => debugMaterial ??= _mat));
                }
            });
        }


        /// <summary>
        /// Just spawns in front of the player the famous Blender3D monkey model
        /// </summary>
        public static void SpawnMonkey()
        {
            if (monkey == null)
            {
                Plugin.Log.LogInfo("no monkey found, attempting to load monkey...");
                LoadMonkey();

                if (monkey == null)
                {
                    Plugin.Log.LogError("monkey reload failed");
                    return;
                }

                Plugin.Log.LogMessage("monkey reload successful");
            }

            Transform playerTransform = Player.Get().GetHeadTransform();
            float distance = 1.5f;

            Logger.Log("Instatiate Monkey");
            GameObject monkeyGO = UnityEngine.Object.Instantiate(monkey, playerTransform.position + distance * playerTransform.forward.normalized, Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            monkeyGO.transform.localScale *= 0.5f;
            monkeyGO.GetComponent<Renderer>().material = debugMaterial;

            List<GrabPoint> grabPoints = [];
            foreach (Transform child in monkeyGO.transform)
            {
                GrabPoint gp = child.gameObject.AddComponent<GrabPoint>();
                gp.HandPose = HandPoseId.Notebook;
                gp.MaxDegreeDifferenceAllowed = 60f;
                gp.enabled = true;
                grabPoints.Add(gp);
            }

            Plugin.Log.LogInfo($"Found {grabPoints.Count} grab points on monkey");

            GuidComponent guidComp = monkeyGO.AddComponent<GuidComponent>();


            ItemToy item = monkeyGO.AddComponent<ItemToy>();


            item.m_Info = new ItemToyInfo() { 
                m_CanBePlacedInStorage = true,
                m_BackpackPocket = Enums.BackpackPocket.Storage,
                m_CantDestroy = true,
                m_Item = item,
                m_InventoryScale = 1f,
                m_Type = Enums.ItemType.Item,
            };

            item.m_GUID = guidComp.GetGuid().ToString();
            item.m_InfoName = "Monkey";


            item.Initialize(false);
            ItemsManager.Get().RegisterItem(item);


            Grabbable grab = monkeyGO.AddComponent<Grabbable>();
            grab.DualGrabSupport = true;
            grab.GrabPoints = grabPoints;
            grab.grabType = GrabAttachType.GrabPoint;
            grab.Initialize();

            Logger.Log($"monkey spawned at {monkeyGO.transform.position}");
        }
    }
}
