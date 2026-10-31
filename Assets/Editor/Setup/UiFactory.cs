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
    }
}
