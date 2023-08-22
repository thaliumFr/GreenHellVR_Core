using UnityEngine;
using BepInEx;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GreenHellVR_Core
{
    public class CoreModObject : MonoBehaviour
    {
        private CoreModObject instance;
        
        GameObject monkey;
        Plugin plugin;

        public CoreModObject Get() {
            return instance;
        }

        public void Awake()
        {
            instance = this;
            plugin = Plugin.instance;
        }

        private void SpawnMonkey()
        {
            if (monkey == null)
            {
                Logger.LogError("no monkey");
                return;
            }

            Vector3 spawnMonkeyPos;
            Player player = Player.Get();

            if (player == null)
            {
                Debug.Log("no player yet -> spawning at origin");
                spawnMonkeyPos = Vector3.zero;
            }
            else
            {
                player.TryGetComponent(out Transform PlayerTransform);
                spawnMonkeyPos = PlayerTransform.position + PlayerTransform.forward + Vector3.up;
            }

            Debug.Log("Instatiate Monkey");

            Instantiate(monkey, spawnMonkeyPos, Quaternion.FromToRotation(Vector3.zero, Vector3.up));
            Debug.Log($"monkey spawned at {spawnMonkeyPos}");
        }

        public void Update()
        {
           if(Input.GetKeyUp(KeyCode.Space)) {
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