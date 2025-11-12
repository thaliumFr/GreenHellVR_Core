using GreenHellVR_Core.Objects;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

#pragma warning disable CS0436 // Type conflicts with imported type

namespace GreenHellVR_Core
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
            instance.assetBundle ??= GHVRC_Objects.LoadAssetBundle("GreenHellVR_Core.monkeybundle");
            if (instance.assetBundle != null && instance.monkey == null)
            {
                StartCoroutine(GHVRC_Objects.LoadAssetFromBundleAsync<GameObject>(instance.assetBundle, "monkey", OnMonkeyLoaded));
            }


            instance.debugMenuGO = DebugMenu.ConstructPanel(out DebugMenu debugMenu);
            instance.debugMenuGO.transform.position = Player.Get().GetHeadTransform().position + Player.Get().GetHeadTransform().forward.normalized;
            instance.debugMenuGO.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        }

        public void OnMonkeyLoaded(GameObject monkey)
        {
            instance.monkey ??= monkey;
        }

        /// <summary>
        /// Just spawns in front of the player the famous Blender3D monkey model
        /// </summary>
        private void SpawnMonkey()
        {
            if (instance.monkey == null)
            {
                Plugin.Log.LogError("no monkey found");
                return;
            }

            Transform playerTransform = Player.Get().GetHeadTransform();
            float distance = 1f;

            Logger.Log("Instatiate Monkey");
            GameObject monkeyGO = Instantiate(instance.monkey, playerTransform.position + distance * playerTransform.forward.normalized, Quaternion.identity * Quaternion.Euler(new Vector3(-90f, 0f, 0f)));

            monkeyGO.GetComponent<Rigidbody>().mass = 2f;

            PhysicMaterial physicMaterial = new()
            {
                staticFriction = 2f,
                bounciness = .25f,
                dynamicFriction = 2f,
                frictionCombine = PhysicMaterialCombine.Maximum,
            };

            SphereCollider SC = monkeyGO.AddComponent<SphereCollider>();
            SC.radius = 1f;
            SC.material = physicMaterial;


            Logger.Log($"monkey spawned at {monkeyGO.transform.position}");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
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
                GHVRC_Objects.ToggleActive(instance.debugMenuGO);
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