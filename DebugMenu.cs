using GreenHellVR_Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0436 // Type conflicts with imported type

namespace GreenHellVR_Core
{
    public class DebugMenu : MonoBehaviour
    {
        GameObject PanelGO;
        public Canvas canvas;

        Transform playerTransform;

        //Dictionary<string, OptionTypes, object> options = [];

        [Header("Variables")]
        TextMeshProUGUI coords;
        TextMeshProUGUI rot;

        // Use this for initialization
        void Start()
        {
            playerTransform = Player.Get().transform;

            Logger.Log("Debug Menu at " + transform.position.ToString());
        }

        public static GameObject ConstructPanel()
        {
            GameObject PanelGO = GHVRC_UI.CreateCanvas(out Canvas canvas, Player.Get().GetHeadTransform());
            DebugMenu debug = PanelGO.AddComponent<DebugMenu>();
            debug.PanelGO = PanelGO;
            debug.canvas = canvas;

            VerticalLayoutGroup group = PanelGO.AddComponent<VerticalLayoutGroup>();
            group.padding = new RectOffset(25, 25, 25, 25);
            group.spacing = 10f;
            group.childForceExpandHeight = false;
            group.childForceExpandWidth = false;


            // Adding Transform

            GHVRC_UI.CreateText("Player pos :", out TextMeshProUGUI coordsComp, PanelGO.transform);
            debug.coords = coordsComp;

            GHVRC_UI.CreateText("Player rot :", out TextMeshProUGUI rotComp, PanelGO.transform);
            debug.rot = rotComp;


            return Instantiate(PanelGO);
        }

        public static GameObject ConstructPanel(out DebugMenu debug)
        {
            GameObject PanelGO = GHVRC_UI.CreateCanvas(out Canvas canvas, Player.Get().GetHeadTransform());
            debug = PanelGO.AddComponent<DebugMenu>();
            debug.PanelGO = PanelGO;
            debug.canvas = canvas;

            VerticalLayoutGroup group = PanelGO.AddComponent<VerticalLayoutGroup>();
            group.padding = new(25, 25, 25, 25);
            group.spacing = 10f;
            group.childForceExpandHeight = false;
            group.childForceExpandWidth = false;


            // Adding Transform

            GHVRC_UI.CreateText("Player pos :", out TextMeshProUGUI coordsComp, PanelGO.transform);
            debug.coords = coordsComp;

            GHVRC_UI.CreateText("Player rot :", out TextMeshProUGUI rotComp, PanelGO.transform);
            debug.rot = rotComp;


            return Instantiate(PanelGO);
        }

        // Update is called once per frame
        void Update()
        {
            coords.text = $"Player coords :\nx: {playerTransform.position.x} | y: {playerTransform.position.y} | z: {playerTransform.position.z}";
            rot.text = $"Player rotation :\nx: {playerTransform.rotation.x} | y: {playerTransform.rotation.y} | z: {playerTransform.rotation.z}";
            transform.position = playerTransform.position + playerTransform.forward.normalized * 0.1f;
        }
    }
}