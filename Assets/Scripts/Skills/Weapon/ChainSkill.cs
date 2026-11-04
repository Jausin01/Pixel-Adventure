using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Lightning that leaps from the weapon to the nearest enemy, then from enemy to enemy, weaker with every jump
    /// (Chain Lightning).
    /// </summary>
    public class ChainSkill : WeaponSkillBase
    {
        public Sprite boltSprite;
        public int jumps = 5;
        public float aimRange = 7f;
        public float jumpRange = 3.5f;
        [Tooltip("Damage kept after each jump (0.85 = 15% less every time).")]
        public float damageKept = 0.85f;
        public float jumpDelay = 0.1f;
        public float recover = 0.15f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Transform first = TargetFinder.FindNearest(owner.position, aimRange, owner);
            Vector2 direction = AimDirection(aimRange);
            Point(direction);
            yield return Tween.MoveTo(transform, transform.position, Chest + direction * 0.5f, 0.1f, Easing.EaseOutQuad);

            Vector2 from = (Vector2)transform.position + direction * 0.5f;
            List<IDamageable> visited = new List<IDamageable>();
            Collider2D current = first != null ? first.GetComponentInChildren<Collider2D>() : null;

            for (int i = 0; i < jumps && current != null; i++)
            {
                IDamageable damageable = current.GetComponentInParent<IDamageable>();
                Vector2 spot = current.bounds.center;
                if (damageable != null && damageable.IsAlive)
                {
                    visited.Add(damageable);
                    damageable.TakeDamage(damage, from);
                    if (hitStop != null) hitStop.Trigger();
                }

                DrawBolt(from, spot);
                Puff(sparkSprite, spot, 1.3f, 0.2f, 1.6f);

                damage *= damageKept;
                from = spot;
                current = FindNext(spot, visited);
                yield return new WaitForSeconds(jumpDelay);
            }

            yield return new WaitForSeconds(recover);
        }

        private Collider2D FindNext(Vector2 from, List<IDamageable> visited)
        {
            Collider2D best = null;
            float bestDistance = float.MaxValue;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(from, jumpRange);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D other = colliders[i];
                if (other.isTrigger || other.transform.IsChildOf(owner)) continue;

                IDamageable damageable = other.GetComponentInParent<IDamageable>();
                if (damageable == null || !damageable.IsAlive || visited.Contains(damageable)) continue;

                float distance = Vector2.Distance(from, other.bounds.center);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = other;
                }
            }
            return best;
        }

        private void DrawBolt(Vector2 from, Vector2 to)
        {
            if (boltSprite == null) return;
            Vector2 delta = to - from;
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            EffectFade.Spawn(boltSprite, SpriteMaterial, (from + to) * 0.5f, angle, new Vector2(delta.magnitude, 1.5f), 0.2f, 1f, 9);
        }
    }
}
