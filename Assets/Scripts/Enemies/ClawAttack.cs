using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A claw slash. A faded copy of the claws shows where the hit will land, then they slash forward and deal damage.
    /// The slash is aimed along one of the four axes.
    /// </summary>
    public class ClawAttack : EnemyAttack
    {
        [Header("Appearance")]
        public Sprite clawSprite;
        public Material material;
        public int sortingOrder = 15;

        [Header("Timing (seconds)")]
        public float windupTime = 0.45f;
        public float strikeTime = 0.1f;
        public float fadeTime = 0.12f;

        [Header("Shape")]
        [Tooltip("Distance from the enemy where the claws appear.")]
        public float reach = 0.7f;
        [Tooltip("How far the claws travel during the slash.")]
        public float sweep = 0.35f;
        [Tooltip("Radius of the hit check, measured around the middle of the slash.")]
        public float hitRadius = 0.5f;
        [Range(0f, 1f)]
        [Tooltip("Opacity of the warning shown during the wind-up.")]
        public float warningAlpha = 0.35f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(Transform target)
        {
            Vector2 origin = (Vector2)transform.position + Vector2.up * 0.25f;
            Vector2 toTarget = (Vector2)target.position + Vector2.up * BodyHeight - origin;
            Vector2 direction = SnapToAxis(toTarget);

            Vector2 start = origin + direction * reach;
            Vector2 end = start + direction * sweep;

            SpriteRenderer claws = SpriteEffect.Create("Claws", clawSprite, material, start, sortingOrder);
            claws.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            SpriteEffect.SetAlpha(claws, warningAlpha);
            Destroy(claws.gameObject, windupTime + strikeTime + fadeTime + 0.5f); // safety net

            yield return new WaitForSeconds(windupTime);
            if (!OwnHealth.IsAlive)
            {
                Destroy(claws.gameObject);
                yield break;
            }

            SpriteEffect.SetAlpha(claws, 1f);
            TryDamage(target, (start + end) / 2f, hitRadius);
            yield return Tween.MoveTo(claws.transform, start, end, strikeTime, Easing.EaseOutQuad);
            yield return SpriteEffect.FadeOut(claws, fadeTime);
        }

        private static Vector2 SnapToAxis(Vector2 vector)
        {
            if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.y)) return new Vector2(Mathf.Sign(vector.x), 0f);
            return new Vector2(0f, Mathf.Sign(vector.y));
        }
    }
}
