using GreenHellVR_Core_Include.Items.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

#pragma warning disable CS0436 // Type conflicts with imported type

namespace GreenHellVR_Core.Items
{
    public class CoreModObject : MonoBehaviour
    {
        private static CoreModObject instance;
        AssetBundle assetBundle;

        GameObject monkey;
        GameObject debugMenuGO;

        public static CoreModObject Get()
        {
            return instance;
        }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Plugin.Log.LogInfo("Destroyed CoreModObject as an instance already exists");
                Destroy(gameObject);

            }

        }

        public void Start()
        {

            /*instance.debugMenuGO = DebugMenu.ConstructPanel(out DebugMenu debugMenu);
            instance.debugMenuGO.transform.position = Player.Get().GetHeadTransform().position + Player.Get().GetHeadTransform().forward.normalized;
            instance.debugMenuGO.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f));*/
        }

        void GetMonkeyBundle(Action<AssetBundle> action) {
            if (instance.assetBundle == null)
            {
                StartCoroutine(GHVRC_Objects.LoadAssetBundleAsync("GreenHellVR_Core.monkeybundle", (bundle) =>
                {
                    instance.assetBundle = bundle;
                    action(bundle);
                }));
            }
            else
            {
                action(instance.assetBundle);
            }
        }

        void LoadMonkey()
        {
            GetMonkeyBundle((assetBundle) =>
            {
                if (assetBundle != null && instance.monkey == null)
                {
                    StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<GameObject>(assetBundle, "monkey", (monkey) => instance.monkey ??= monkey));
                }
            });
        }

        /// <summary>
        /// Just spawns in front of the player the famous Blender3D monkey model
        /// </summary>
        private void SpawnMonkey()
        {
            if (instance.monkey == null)
            {
                Plugin.Log.LogInfo("no monkey found, attempting to reload monkey...");
                LoadMonkey();

                if (instance.monkey == null)
                {
                    Plugin.Log.LogError("monkey reload failed");
                    return;
                }

                Plugin.Log.LogMessage("monkey reload successful");
            }

            Transform playerTransform = Player.Get().GetHeadTransform();
            float distance = 1f;

            Logger.Log("Instatiate Monkey");
            GameObject monkeyGO = Instantiate(instance.monkey, playerTransform.position + distance * playerTransform.forward.normalized, Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f)));

            List<GrabPoint> grabPoints = [];
            foreach (Transform child in monkeyGO.transform)
            {
                GrabPoint gp = child.gameObject.AddComponent<GrabPoint>();
                gp.HandPose = HandPoseId.Default;
                gp.MaxDegreeDifferenceAllowed = 60f;
                gp.enabled = true;
                grabPoints.Add(gp);
            }

            Plugin.Log.LogInfo($"Found {grabPoints.Count} grab points on monkey");

            GuidComponent guidComp = monkeyGO.AddComponent<GuidComponent>();

            Item item = monkeyGO.AddComponent<Item>();
            item.m_GUID = guidComp.GetGuid().ToString();
            item.Initialize(false);

            Grabbable grab = monkeyGO.AddComponent<Grabbable>();
            grab.DualGrabSupport = false;
            grab.GrabPoints = grabPoints;
            grab.Initialize();

            Logger.Log($"monkey spawned at {monkeyGO.transform.position}");
        }

        public void Update()
        {
            List<InputDevice> devices = [];
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
            if (devices.Count != 1)
            {
                Plugin.Log.LogWarning($"Expected 1 left hand device, found {devices.Count}");
            }

            if (
                Input.GetKeyDown(KeyCode.M) || 
                (devices.Count == 1 && devices[0].TryGetFeatureValue(CommonUsages.menuButton, out bool isPressed) && isPressed)
            )
            {
                SpawnMonkey();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Plugin.Log.LogInfo("Force pause menu");
                MenuInGameManager.Get().ForcePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //GHVRC_Objects.ToggleActive(instance.debugMenuGO);
            }
        }
    }
}



/*
canvas.renderMode = RenderMode.WorldSpace;
CameraManager cameraManager = CameraManager.Get();
canvas.worldCamera = (((cameraManager != null) ? cameraManager.m_MainCamera : null) ?? Camera.main);
canvasRectTransform.localScale = localScale;
canvasRectTransform.rotation = Quaternion.identity;
*/