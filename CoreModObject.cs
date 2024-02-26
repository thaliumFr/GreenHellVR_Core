using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GreenHellVR_Core
{
    public class CoreModObject : MonoBehaviour
    {
        private CoreModObject instance;
        AssetBundle assetBundle;

        GameObject monkey;

        public CoreModObject Get()
        {
            return instance;
        }

        public void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            instance = this;
        }

        public void Start()
        {
            assetBundle ??= GHVRC_Objects.LoadAssetBundle("GreenHellVR_Core.monkeybundle");
            if (assetBundle != null && monkey == null)
            {
                StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<GameObject>(assetBundle, "monkey", OnMonkeyLoaded));
            }
        }

        public void OnMonkeyLoaded(GameObject monkey)
        {
            this.monkey = monkey;
        }

        /// <summary>
        /// Just spawns in front of the player the famous Blender3D monkey model
        /// </summary>
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
            GameObject monkeyGO = Instantiate(monkey, playerTransform.position + distance * playerTransform.forward.normalized, Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f)));

            SphereCollider SC = monkeyGO.AddComponent<SphereCollider>();
            SC.radius = 1f;

            Logger.Log($"monkey spawned at {monkeyGO.transform.position}");
        }

        public void Update()
        {
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