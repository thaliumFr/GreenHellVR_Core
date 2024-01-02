using UnityEngine;
using BepInEx;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using System.Collections;

namespace GreenHellVR_Core
{
    public class CoreModObject : MonoBehaviour
    {
        private CoreModObject instance;
        AssetBundle assetBundle;

        GameObject monkey;

        public CoreModObject Get() {
            return instance;
        }

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            assetBundle = GHVRC_Objects.LoadAssetBundle("GreenHellVR_Core.monkeybundle");
            StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<GameObject>(assetBundle, "monkey", OnMonkeyLoaded));
        }


        public void OnMonkeyLoaded(GameObject monkey)
        {
            this.monkey = monkey;
        }

        private void SpawnMonkey()
        {
            if (monkey == null)
            {
                Plugin.Log.LogError("no monkey");
                return;
            }

            Transform playerTransform = Player.Get().GetHeadTransform();
            float distance = 1f;

            Logger.Log("Instatiate Monkey");
            GameObject monkeyGO = Instantiate(monkey, playerTransform.position + distance * playerTransform.forward.normalized, Quaternion.FromToRotation(Vector3.zero, Vector3.up));

            Logger.Log($"monkey spawned at {monkeyGO.transform.position}");
        }

        public void Update(){
            if (Input.GetKeyDown(KeyCode.M))
            {
                SpawnMonkey();
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