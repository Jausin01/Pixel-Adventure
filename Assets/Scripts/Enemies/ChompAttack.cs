using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A bite. Two rows of fangs appear faded above and below where the target stands, then snap shut.
    /// The place is chosen when the attack starts, so the player can step away during the wind-up.
    /// </summary>
    public class ChompAttack : EnemyAttack
    {
        [Header("Appearance")]
        [Tooltip("Fangs pointing down. The lower row is the same sprite flipped.")]
        public Sprite fangSprite;
        public Material material;
        public int sortingOrder = 15;

        [Header("Timing (seconds)")]
        public float windupTime = 0.5f;
        public float closeTime = 0.08f;
        public float holdTime = 0.15f;
        public float fadeTime = 0.1f;

        [Header("Shape")]
        [Tooltip("Distance of each row of fangs from the bite point while open.")]
        public float openGap = 0.55f;
        [Tooltip("Distance of each row of fangs from the bite point once closed.")]
        public float closedGap = 0.12f;
        public float hitRadius = 0.45f;
        [Range(0f, 1f)]
        [Tooltip("Opacity of the warning shown during the wind-up.")]
        public float warningAlpha = 0.35f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(Transform target)
        {
            Vector2 bitePoint = (Vector2)target.position + Vector2.up * BodyHeight;

            SpriteRenderer upper = SpriteEffect.Create("Fangs Upper", fangSprite, material, bitePoint + Vector2.up * openGap, sortingOrder);
            SpriteRenderer lower = SpriteEffect.Create("Fangs Lower", fangSprite, material, bitePoint + Vector2.down * openGap, sortingOrder);
            lower.flipY = true;
            SpriteEffect.SetAlpha(upper, warningAlpha);
            SpriteEffect.SetAlpha(lower, warningAlpha);

            float lifetime = windupTime + closeTime + holdTime + fadeTime + 0.5f;
            Destroy(upper.gameObject, lifetime); // safety net
            Destroy(lower.gameObject, lifetime);

            yield return new WaitForSeconds(windupTime);
            if (!OwnHealth.IsAlive)
            {
                Destroy(upper.gameObject);
                Destroy(lower.gameObject);
                yield break;
            }

            SpriteEffect.SetAlpha(upper, 1f);
            SpriteEffect.SetAlpha(lower, 1f);

            StartCoroutine(Tween.MoveTo(upper.transform, bitePoint + Vector2.up * openGap, bitePoint + Vector2.up * closedGap, closeTime, Easing.EaseInQuad));
            yield return Tween.MoveTo(lower.transform, bitePoint + Vector2.down * openGap, bitePoint + Vector2.down * closedGap, closeTime, Easing.EaseInQuad);

            TryDamage(target, bitePoint, hitRadius);
            yield return new WaitForSeconds(holdTime);

            StartCoroutine(SpriteEffect.FadeOut(upper, fadeTime));
            yield return SpriteEffect.FadeOut(lower, fadeTime);
        }
    }
}
