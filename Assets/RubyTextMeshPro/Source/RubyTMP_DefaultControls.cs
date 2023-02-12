using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
    public static class RubyTMP_DefaultControls
    {
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const float WIDTH = 160f;
        private const float THICK_HEIGHT = 30f;
        private static readonly Vector2 THICK_ELEMENT_SIZE = new Vector2(RubyTMP_DefaultControls.WIDTH, RubyTMP_DefaultControls.THICK_HEIGHT);

        //private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static readonly Color DEFAULT_SELECTABLE_COLOR = new Color(1f, 1f, 1f, 1f);

        //private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static readonly Color TEXT_COLOR = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static void SetDefaultTextValues(TMP_Text lbl)
        {
            // Set text values we want across UI elements in default controls.
            // Don't set values which are the same as the default values for the Text component,
            // since there's no point in that, and it's good to keep them as consistent as possible.
            lbl.color = RubyTMP_DefaultControls.TEXT_COLOR;
            lbl.fontSize = 14;
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
            {
                return;
            }

            child.transform.SetParent(parent.transform, false);
            RubyTMP_DefaultControls.SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;

            for (int i = 0; i < t.childCount; i++)
            {
                RubyTMP_DefaultControls.SetLayerRecursively(t.GetChild(i).gameObject, layer);
            }
        }

        // Actual controls

        public static GameObject CreateButton(Resources resources)
        {
            GameObject buttonRoot = RubyTMP_DefaultControls.CreateUIElementRoot("Button", RubyTMP_DefaultControls.THICK_ELEMENT_SIZE);

            GameObject childText = new GameObject("Text (RubyTMP)");
            childText.AddComponent<RectTransform>();
            RubyTMP_DefaultControls.SetParentAndAlign(childText, buttonRoot);

            Image image = buttonRoot.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = RubyTMP_DefaultControls.DEFAULT_SELECTABLE_COLOR;

            Button bt = buttonRoot.AddComponent<Button>();
            RubyTMP_DefaultControls.SetDefaultColorTransitionValues(bt);

            RubyTextMeshProUGUI text = childText.AddComponent<RubyTextMeshProUGUI>();
            text.text = "Button";
            text.alignment = TextAlignmentOptions.Center;
            RubyTMP_DefaultControls.SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            return buttonRoot;
        }

        public static GameObject CreateText(Resources resources)
        {
            GameObject go = RubyTMP_DefaultControls.CreateUIElementRoot("Text (RubyTMP)", RubyTMP_DefaultControls.THICK_ELEMENT_SIZE);

            RubyTextMeshProUGUI lbl = go.AddComponent<RubyTextMeshProUGUI>();
            lbl.text = "New Text";
            RubyTMP_DefaultControls.SetDefaultTextValues(lbl);

            return go;
        }
    }
}