using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GreenHellVR_Core
{
    internal class GreenHellVR_Core_UI
    {
        public static TMP_FontAsset defaultFontAsset;
        public static Button buttonPrefab;

        public static void CopyTextSettings(TextMeshProUGUI source, TextMeshProUGUI dest)
        {
            
            dest.font = source.font;
            dest.fontSize = source.fontSize;
            dest.color = source.color;
            dest.fontMaterial = source.fontMaterial;
            dest.alpha = source.alpha;
        }

        public static GameObject CreateText(string text, Transform parent)
        {
            GameObject go = new("Text");
            TextMeshProUGUI label = go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.font = defaultFontAsset;
            label.fontSize *= 0.75f;
            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static GameObject CreateText(string text)
        {
            GameObject go = new ("Text");
            TextMeshProUGUI label = go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.font = defaultFontAsset;
            label.fontSize *= 0.75f;

            return go;
        }

        public static Button CreateButton(string text, UnityAction buttonAction = null)
        {
            if (buttonPrefab == null)
            {
                throw new MissingReferenceException("button prefab null : you are probably using this ressource too early");
            }

            Button button = Object.Instantiate(buttonPrefab);
            TextMeshProUGUI txtComp = button.GetComponentInChildren<TextMeshProUGUI>();
            if (txtComp != null)
            {
                txtComp.text = text;
            }
            if (buttonAction != null) { button.onClick.AddListener(buttonAction); }
            return button;
        }
    }

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    class UIMenu: MonoBehaviour
    {
        public string title;
        public Canvas canvas;
        public TextMeshProUGUI titleText;

        public UIMenu(string title, Vector2 Size)
        {
            this.title = title;
            TryGetComponent(out canvas);
            titleText = GreenHellVR_Core_UI.CreateText(title);
            titleText.transform.parent = canvas.transform;


        }

        void Awake()
        {

        }
    }
}
