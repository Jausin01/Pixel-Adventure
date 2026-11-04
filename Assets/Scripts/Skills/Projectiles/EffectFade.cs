using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A short-lived sprite that fades out while growing, then removes itself (dust, sparks, bolts, bursts).
    /// </summary>
    public class EffectFade : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private float duration;
        private float elapsed;
        private Vector3 startScale;
        private Vector3 endScale;

        /// <summary>Creates a fading effect.</summary>
        /// <param name="effectSprite">The sprite to show.</param>
        /// <param name="material">Material of the sprite.</param>
        /// <param name="position">World position.</param>
        /// <param name="rotationDegrees">Rotation around Z.</param>
        /// <param name="scale">Starting scale.</param>
        /// <param name="lifetime">Seconds until it has faded away.</param>
        /// <param name="growth">Scale at the end, as a multiple of the starting scale.</param>
        /// <param name="sortingOrder">Sorting order of the sprite.</param>
        public static EffectFade Spawn(Sprite effectSprite, Material material, Vector2 position, float rotationDegrees,
            Vector2 scale, float lifetime, float growth, int sortingOrder)
        {
            SpriteRenderer renderer = SpriteEffect.Create("Effect", effectSprite, material, position, sortingOrder);
            renderer.transform.rotation = Quaternion.Euler(0f, 0f, rotationDegrees);
            renderer.transform.localScale = new Vector3(scale.x, scale.y, 1f);

            EffectFade fade = renderer.gameObject.AddComponent<EffectFade>();
            fade.sprite = renderer;
            fade.duration = Mathf.Max(0.01f, lifetime);
            fade.startScale = renderer.transform.localScale;
            fade.endScale = fade.startScale * growth;
            return fade;
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            SpriteEffect.SetAlpha(sprite, 1f - progress);
            if (progress >= 1f) Destroy(gameObject);
        }
    }
}
