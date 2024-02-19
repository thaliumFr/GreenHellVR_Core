using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace GreenHellVR_Core
{
    public static class GHVRC_UI
    {
        static readonly Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        public static TMP_FontAsset defaultFontAsset = TMP_FontAsset.CreateFontAsset(ArialFont);

        public static GameObject CreateText(string text, Transform parent = null)
        {
            GameObject go = new("Text")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            TextMeshProUGUI label = go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.font = defaultFontAsset;
            label.fontSize *= 0.75f;
            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static GameObject CreateCanvas(Transform parent = null)
        {
            GameObject go = new("Canvas")
            {
                layer = LayerMask.NameToLayer("UI")
            };

            go.AddComponent<Canvas>();

            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static void ToggleActive(GameObject go)
        {
            if (go == null) return;
            go.SetActive(!go.activeSelf);
            Plugin.Log.LogInfo($"{go.name} is now {(go.activeSelf ? "active" : "inactive")}");
        }

        public static GameObject CreateButton(string text, Transform Parent = null, UnityAction buttonAction = null)
        {
            GameObject buttonGO = new(text);
            RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();
            Button btnComp = buttonGO.AddComponent<Button>();

            if (Parent != null) { rectTransform.SetParent(Parent); }
            if (buttonAction != null) { btnComp.onClick.AddListener(buttonAction); }
            
            CreateText(text, rectTransform);

            Plugin.Log.LogInfo("Button created");

            return buttonGO;
        }

        public static void CopyTextProperties(TMP_Text source, ref TMP_Text destination)
        {
            destination.fontSize = source.fontSize;
            destination.font = source.font;
            destination.fontStyle = source.fontStyle;
            destination.fontWeight = source.fontWeight;
            destination.transform.localScale = source.transform.localScale;
            //destination.color = source.color;
        }
    }
}
