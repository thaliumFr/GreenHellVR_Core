using GreenHellVR_Core_Include.Items.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GreenHellVR_Core.Items
{
    public class CoreModObject : MonoBehaviour
    {
        public static CoreModObject Instance { get; private set; }

        GameObject debugMenuGO;

        public static CoreModObject Get()
        {
            return Instance;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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

        public void Update()
        {
            List<InputDevice> devices = [];
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
            if (devices.Count != 1)
            {
                Plugin.Log.LogWarning($"Expected 1 left hand device, found {devices.Count}");
            }

            if (ConfigManager.Instance.ConfigMonkeySpawnDebug.Value)
            {
                if (Input.GetKeyDown(KeyCode.M) ||((devices.Count == 1 && devices[0].TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool isPressed) && isPressed)))
                {
                    DebugMonkey.SpawnMonkey();
                }
            }
            

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Plugin.Log.LogInfo("Force pause menu");
                MenuInGameManager.Get().ForcePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.F3))
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