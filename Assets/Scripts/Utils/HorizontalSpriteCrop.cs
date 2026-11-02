using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Produces versions of a sprite cut from the left in whole pixels (the right part is removed).
    /// Created sprites are cached and must be released with <see cref="Release"/>.
    /// The cropped sprites have their pivot on the left edge, so the left side stays in place.
    /// </summary>
    public sealed class HorizontalSpriteCrop
    {
        private readonly Sprite source;
        private readonly Dictionary<int, Sprite> cache = new Dictionary<int, Sprite>();

        /// <summary>Creates a cropper for a sprite.</summary>
        public HorizontalSpriteCrop(Sprite source)
        {
            this.source = source;
        }

        /// <summary>Width of the original sprite in pixels.</summary>
        public int SourceWidth { get { return (int)source.rect.width; } }

        /// <summary>Returns the sprite keeping only the left <paramref name="visibleWidth"/> pixels.</summary>
        public Sprite Get(int visibleWidth)
        {
            int width = Mathf.Clamp(visibleWidth, 1, SourceWidth);

            Sprite cropped;
            if (cache.TryGetValue(width, out cropped)) return cropped;

            Rect full = source.textureRect;
            Rect rect = new Rect(full.x, full.y, width, full.height);
            cropped = Sprite.Create(source.texture, rect, new Vector2(0f, 0.5f), source.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            cache[width] = cropped;
            return cropped;
        }

        /// <summary>Destroys every sprite created by this cropper.</summary>
        public void Release()
        {
            foreach (Sprite sprite in cache.Values) Object.Destroy(sprite);
            cache.Clear();
        }
    }
}
