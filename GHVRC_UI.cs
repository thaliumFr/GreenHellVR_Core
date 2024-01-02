using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GreenHellVR_Core
{
    public static class GHVRC_UI
    {
        public static TMP_FontAsset defaultFontAsset;

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

        public static TextMeshProUGUI CreateText(string text)
        {
            TextMeshProUGUI label = new()
            {
                text = text,
                font = defaultFontAsset
            };
            label.fontSize *= 0.75f;

            return label;
        }

        public static GameObject CreateButton(string text, Transform Parent = null, UnityAction buttonAction = null)
        {
            GameObject buttonGO = Object.Instantiate(new GameObject(text), Vector3.zero, Quaternion.identity);
            RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();

            if (Parent != null) { rectTransform.SetParent(Parent); }
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            Button btnComp = buttonGO.AddComponent<Button>();
            
            GameObject BtnText = Object.Instantiate(new GameObject("text"), Vector3.zero, Quaternion.identity, buttonGO.transform);
            TextMeshProUGUI txtComp = BtnText.AddComponent<TextMeshProUGUI>();

            txtComp.text = text;
            if (buttonAction != null) { btnComp.onClick.AddListener(buttonAction); }

            Plugin.Log.LogInfo("Button created");

            return buttonGO;
        }
    }
}
