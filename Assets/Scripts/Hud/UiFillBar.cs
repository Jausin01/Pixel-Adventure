using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Fill bar drawn on the UI canvas. The fill image must be a child of this bar, anchored to the left edge
    /// and stretched vertically. It is cut in whole source pixels so the bar changes in crisp steps.
    /// </summary>
    public class UiFillBar : MonoBehaviour, IFillBar
    {
        [Tooltip("Image that shows the filled part of the bar.")]
        public Image fill;

        [Tooltip("The full (100%) sprite of the bar.")]
        public Sprite fullSprite;

        private HorizontalSpriteCrop crop;

        private void Awake()
        {
            crop = new HorizontalSpriteCrop(fullSprite);
        }

        private void OnDestroy()
        {
            crop.Release();
        }

        /// <inheritdoc/>
        public void SetFraction(float fraction)
        {
            int pixels = Mathf.RoundToInt(Mathf.Clamp01(fraction) * crop.SourceWidth);
            fill.enabled = pixels > 0;
            if (pixels == 0) return;

            fill.sprite = crop.Get(pixels);

            float fullWidth = ((RectTransform)transform).rect.width;
            RectTransform rect = fill.rectTransform;
            rect.sizeDelta = new Vector2(fullWidth * pixels / crop.SourceWidth, rect.sizeDelta.y);
        }
    }
}
