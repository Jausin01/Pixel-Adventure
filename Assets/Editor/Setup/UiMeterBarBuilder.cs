using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates one HUD bar (icon + bar) bound to a meter such as health or energy.
    /// </summary>
    public static class UiMeterBarBuilder
    {
        private const float IconGap = 8f;
        private const float BarPixelHeight = 6f;

        /// <summary>Builds the icon and the bar.</summary>
        /// <param name="topLeft">Position of the icon's top-left corner, measured from the parent's top-left corner.</param>
        /// <param name="scale">Canvas units per art pixel.</param>
        /// <param name="source">Component implementing <see cref="IMeter"/> that the bar displays.</param>
        public static void Build(Transform parent, Sprite icon, Sprite empty, Sprite full, Vector2 topLeft, float scale, MonoBehaviour source)
        {
            Vector2 anchor = new Vector2(0f, 1f);
            Vector2 iconSize = icon.rect.size * scale;

            Image iconImage = UiFactory.CreateImage("Icon", parent, icon, anchor, topLeft, iconSize);
            iconImage.raycastTarget = false;

            float barWidth = full.rect.width * scale;
            float barHeight = BarPixelHeight * scale;
            Vector2 barPosition = new Vector2(topLeft.x + iconSize.x + IconGap, topLeft.y - (iconSize.y - barHeight) / 2f);
            Image back = UiFactory.CreateImage("Bar", parent, empty, anchor, barPosition, new Vector2(barWidth, barHeight));
            back.raycastTarget = false;

            Image fill = CreateFill(back.transform, full, barWidth);

            UiFillBar fillBar = back.gameObject.AddComponent<UiFillBar>();
            fillBar.fill = fill;
            fillBar.fullSprite = full;
            back.gameObject.AddComponent<MeterBarBinder>().source = source;
        }

        private static Image CreateFill(Transform parent, Sprite sprite, float width)
        {
            Image fill = UiFactory.CreateImage("Fill", parent, sprite, new Vector2(0f, 0.5f), Vector2.zero, new Vector2(width, 0f));
            fill.raycastTarget = false;

            // Anchored to the left edge and stretched vertically; the script only changes its width.
            RectTransform rect = fill.rectTransform;
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(width, 0f);
            return fill;
        }
    }
}
