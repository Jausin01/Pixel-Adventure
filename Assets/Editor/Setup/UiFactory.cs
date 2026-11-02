using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Helper to create UI images with anchors and sizes.
    /// </summary>
    public static class UiFactory
    {
        /// <summary>Creates a UI image under a parent.</summary>
        /// <param name="anchor">Anchor and pivot (for example (1,0) for bottom-right).</param>
        /// <param name="position">Anchored position.</param>
        /// <param name="size">Size in canvas units.</param>
        public static Image CreateImage(string name, Transform parent, Sprite sprite, Vector2 anchor, Vector2 position, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            return image;
        }

        /// <summary>Creates a round button background with an icon centered on it.</summary>
        /// <param name="anchor">Anchor and pivot of the button.</param>
        /// <param name="iconSize">Size of the icon in canvas units.</param>
        public static Image CreateIconButton(string name, Transform parent, Sprite circle, Sprite icon, Vector2 anchor, Vector2 position, float size, float iconSize)
        {
            Image button = CreateImage(name, parent, circle, anchor, position, new Vector2(size, size));
            button.color = new Color(1f, 1f, 1f, 0.85f);
            button.gameObject.AddComponent<PressScaleFeedback>();

            Image iconImage = CreateImage("Icon", button.transform, icon, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(iconSize, iconSize));
            iconImage.raycastTarget = false;
            iconImage.preserveAspect = true;
            return button;
        }

        /// <summary>Creates a text label with a thin outline so it stays readable on any background.</summary>
        public static Text CreateText(string name, Transform parent, string content, int fontSize, TextAnchor alignment, Vector2 anchor, Vector2 position, Vector2 size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(Outline));
            go.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            Text text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = content;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }
    }
}
