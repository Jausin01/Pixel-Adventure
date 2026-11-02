using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Helpers for short-lived sprite effects (slashes, bites...).
    /// </summary>
    public static class SpriteEffect
    {
        /// <summary>Creates an object with a sprite renderer.</summary>
        public static SpriteRenderer Create(string name, Sprite sprite, Material material, Vector2 position, int sortingOrder)
        {
            GameObject go = new GameObject(name, typeof(SpriteRenderer));
            go.transform.position = position;

            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sharedMaterial = material;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        /// <summary>Sets only the alpha of a renderer's color.</summary>
        public static void SetAlpha(SpriteRenderer renderer, float alpha)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }

        /// <summary>Fades a renderer to transparent and then destroys its object.</summary>
        public static IEnumerator FadeOut(SpriteRenderer renderer, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && renderer != null)
            {
                elapsed += Time.deltaTime;
                SpriteEffect.SetAlpha(renderer, 1f - Mathf.Clamp01(elapsed / duration));
                yield return null;
            }

            if (renderer != null) Object.Destroy(renderer.gameObject);
        }
    }
}
