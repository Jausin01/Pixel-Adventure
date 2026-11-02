using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shows a sprite (optionally two parts, for example a trunk with a canopy above it) cut from the top down in whole
    /// pixels as durability drops. When depleted only a small stump of the lower part stays visible.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class StackedSpriteCropper : MonoBehaviour
    {
        [Header("Lower part (bottom of the object)")]
        public SpriteRenderer lowerRenderer;
        public Sprite lowerSource;

        [Header("Upper part (optional, sits on top of the lower part)")]
        public SpriteRenderer upperRenderer;
        public Sprite upperSource;

        [Tooltip("Height in pixels of the stump left when depleted.")]
        public int stumpPixels = 4;

        private ResourceDurability durability;
        private readonly Dictionary<int, Sprite> lowerCache = new Dictionary<int, Sprite>();
        private readonly Dictionary<int, Sprite> upperCache = new Dictionary<int, Sprite>();

        private void Awake()
        {
            durability = GetComponent<ResourceDurability>();
        }

        private void OnEnable()
        {
            durability.Changed += Apply;
        }

        private void OnDisable()
        {
            durability.Changed -= Apply;
        }

        private void Start()
        {
            Apply(durability.Remaining01);
        }

        private void OnDestroy()
        {
            DestroyCache(lowerCache);
            DestroyCache(upperCache);
        }

        private void Apply(float remaining01)
        {
            int lowerHeight = (int)lowerSource.rect.height;
            int upperHeight = upperSource != null ? (int)upperSource.rect.height : 0;
            int stump = Mathf.Min(stumpPixels, lowerHeight);

            int visible = stump + Mathf.RoundToInt(remaining01 * (lowerHeight + upperHeight - stump));
            int lowerVisible = Mathf.Min(visible, lowerHeight);
            int upperVisible = Mathf.Clamp(visible - lowerHeight, 0, upperHeight);

            lowerRenderer.sprite = GetCropped(lowerSource, lowerVisible, lowerCache);
            if (upperRenderer != null)
            {
                upperRenderer.enabled = upperVisible > 0;
                if (upperVisible > 0) upperRenderer.sprite = GetCropped(upperSource, upperVisible, upperCache);
            }
        }

        private static Sprite GetCropped(Sprite source, int height, Dictionary<int, Sprite> cache)
        {
            Sprite cropped;
            if (cache.TryGetValue(height, out cropped)) return cropped;

            // Keep the bottom 'height' pixels, so the cut happens at the top.
            Rect full = source.textureRect;
            Rect rect = new Rect(full.x, full.y, full.width, height);
            cropped = Sprite.Create(source.texture, rect, new Vector2(0.5f, 0f), source.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            cache[height] = cropped;
            return cropped;
        }

        private static void DestroyCache(Dictionary<int, Sprite> cache)
        {
            foreach (Sprite sprite in cache.Values) Destroy(sprite);
            cache.Clear();
        }
    }
}
