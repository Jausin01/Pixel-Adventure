using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Calls objects down from the sky over an area around the nearest enemy (or in front of the player):
    /// a rain of spears, a meteor shower. Each drop shows a marker, then falls and hits a circle.
    /// </summary>
    public class RainSkill : WeaponSkillBase
    {
        public Sprite dropSprite;
        public int drops = 8;
        [Tooltip("Radius of the area the drops land in.")]
        public float areaRadius = 2.2f;
        public float hitRadius = 0.8f;
        public float aimRange = 6f;
        [Tooltip("How far in front of the player the area is centred when no enemy is near.")]
        public float distance = 3f;
        public float dropHeight = 5f;
        public float fallTime = 0.22f;
        [Tooltip("How long the marker shows before the drop starts falling.")]
        public float warning = 0.3f;
        public float interval = 0.12f;
        [Tooltip("Size of the blast effect at each landing.")]
        public float blastScale = 1f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
            Vector2 center = target != null ? (Vector2)target.position : (Vector2)owner.position + facing.Vector * distance;

            Point(Vector2.up);
            yield return Tween.MoveTo(transform, transform.position, Chest + Vector2.up * 1.1f, 0.2f, Easing.EaseOutQuad);

            for (int i = 0; i < drops; i++)
            {
                Vector2 spot = center + Random.insideUnitCircle * areaRadius;
                StartCoroutine(Drop(spot, damage));
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(warning + fallTime);
        }

        private IEnumerator Drop(Vector2 spot, float damage)
        {
            Puff(dustSprite, spot, 1.4f, warning + fallTime, 0.6f);
            yield return new WaitForSeconds(warning);

            GameObject drop = new GameObject("Falling Drop", typeof(SpriteRenderer));
            SpriteRenderer renderer = drop.GetComponent<SpriteRenderer>();
            renderer.sprite = dropSprite;
            renderer.sharedMaterial = SpriteMaterial;
            renderer.sortingOrder = 9;
            drop.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

            Vector2 from = spot + Vector2.up * dropHeight;
            yield return Tween.MoveTo(drop.transform, from, spot, fallTime, Easing.EaseInQuad);

            Hit(new HitScanner(), spot, hitRadius, damage);
            Puff(burstSprite != null ? burstSprite : sparkSprite, spot, blastScale, 0.3f, 1.4f);
            Destroy(drop);
        }
    }
}
