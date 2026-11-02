using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Fill bar drawn in the game world (for example above an enemy). The filled sprite is cut
    /// from the right in whole pixels, so the bar changes in crisp pixel steps.
    /// </summary>
    public class WorldFillBar : MonoBehaviour, IFillBar
    {
        [Tooltip("Renderer that shows the filled part of the bar.")]
        public SpriteRenderer fill;

        [Tooltip("The full (100%) sprite of the bar.")]
        public Sprite fullSprite;

        private HorizontalSpriteCrop crop;

        private void Awake()
        {
            crop = new HorizontalSpriteCrop(fullSprite);

            // The cropped sprites have their pivot on the left edge, so place the fill at the bar's left edge.
            float halfWidth = fullSprite.rect.width / fullSprite.pixelsPerUnit / 2f;
            fill.transform.localPosition = new Vector3(-halfWidth, 0f, 0f);
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
            if (pixels > 0) fill.sprite = crop.Get(pixels);
        }
    }
}
