using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using Button = UnityEngine.UI.Button;

namespace GreenHellVR_Core.UI
{
    public static class GHVRC_UI
    {
        static readonly Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        public static TMP_FontAsset defaultFontAsset = TMP_FontAsset.CreateFontAsset(ArialFont);

        public static GameObject CreateText(string text, out TextMeshProUGUI label, Transform parent = null)
        {
            GameObject go = new("Text")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            label = go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.font = defaultFontAsset;
            label.fontSize *= 0.75f;
            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static GameObject CreateText(string text, out TMP_Text label, Transform parent = null)
        {
            GameObject go = new("Text")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            label = go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.font = defaultFontAsset;
            label.fontSize *= 0.75f;
            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

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

        public static GameObject CreateCanvas(out Canvas canvas, Transform parent = null)
        {
            GameObject go = new("Canvas")
            {
                layer = LayerMask.NameToLayer("UI"),
            };

            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static GameObject CreateCanvas(Transform parent = null)
        {
            GameObject go = new("Canvas")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            Canvas canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            if (parent != null) { go.transform.parent = parent; }

            return go;
        }

        public static GameObject CreateButton(string text, out Button btnComp, Transform Parent = null, UnityAction buttonAction = null)
        {
            GameObject buttonGO = new(text);
            RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();
            btnComp = buttonGO.AddComponent<Button>();

            if (Parent != null) { rectTransform.SetParent(Parent); }
            if (buttonAction != null) { btnComp.onClick.AddListener(buttonAction); }

            CreateText(text, rectTransform);

            Plugin.Log.LogInfo("Button created");

            return buttonGO;
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

        public static GameObject CreateTextInput(string Placeholder, out TMP_InputField field, Transform Parent = null, UnityAction<string> OnChange = null)
        {
            GameObject InputGO = new(Placeholder);
            RectTransform rectTransform = InputGO.AddComponent<RectTransform>();
            field = InputGO.AddComponent<TMP_InputField>();

            if (Parent != null) { rectTransform.SetParent(Parent); }
            if (OnChange != null) { field.onEndEdit.AddListener(OnChange); }

            CreateText(Placeholder, rectTransform);

            Plugin.Log.LogInfo("Button created");

            return InputGO;
        }

        public static GameObject CreateTextInput(string Placeholder, Transform Parent = null, UnityAction<string> OnChange = null)
        {
            GameObject InputGO = new(Placeholder);
            RectTransform rectTransform = InputGO.AddComponent<RectTransform>();
            TMP_InputField field = InputGO.AddComponent<TMP_InputField>();

            if (Parent != null) { rectTransform.SetParent(Parent); }
            if (OnChange != null) { field.onEndEdit.AddListener(OnChange); }

            CreateText(Placeholder, rectTransform);

            Plugin.Log.LogInfo("Button created");

            return InputGO;
        }

#if DEBUG
        [Obsolete("Please use Field Inputs for now")]
        public static GameObject CreateVector3Input(string name, ref Vector3 vect)
        {
            GameObject parent = new(name);

            // Label

            GameObject LabelGO = CreateText(name, parent.transform);


            // X

            return parent;
        }
#endif

        public static void CopyTextProperties(TMP_Text source, ref TMP_Text destination)
        {
            destination.fontSize = source.fontSize;
            destination.font = source.font;
            destination.fontStyle = source.fontStyle;
            destination.fontWeight = source.fontWeight;
            destination.transform.localScale = source.transform.localScale;
            destination.color = source.color.ColorWithAlpha(1f);
        }

        public static void CopyTextProperties(TMP_Text source, ref TextMeshProUGUI destination)
        {
            destination.fontSize = source.fontSize;
            destination.font = source.font;
            destination.fontStyle = source.fontStyle;
            destination.fontWeight = source.fontWeight;
            destination.transform.localScale = source.transform.localScale;
            destination.color = source.color.ColorWithAlpha(1f);
        }

        public static void CopyTextProperties(TextMeshProUGUI source, ref TMP_Text destination)
        {
            destination.fontSize = source.fontSize;
            destination.font = source.font;
            destination.fontStyle = source.fontStyle;
            destination.fontWeight = source.fontWeight;
            destination.transform.localScale = source.transform.localScale;
            destination.color = source.color.ColorWithAlpha(1f);
        }

        public static void CopyTextProperties(TextMeshProUGUI source, ref TextMeshProUGUI destination)
        {
            destination.fontSize = source.fontSize;
            destination.font = source.font;
            destination.fontStyle = source.fontStyle;
            destination.fontWeight = source.fontWeight;
            destination.transform.localScale = source.transform.localScale;
            destination.color = source.color.ColorWithAlpha(1f);
        }
    }
}
