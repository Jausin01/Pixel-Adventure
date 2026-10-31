using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Applies damage to every living damageable inside a circle, hitting each target only once
    /// until <see cref="Reset"/> is called. Create one per attack.
    /// </summary>
    public sealed class HitScanner
    {
        private readonly List<IDamageable> alreadyHit = new List<IDamageable>();

        /// <summary>Forgets previously hit targets so they can be hit again.</summary>
        public void Reset()
        {
            alreadyHit.Clear();
        }

        /// <summary>
        /// Damages every new target overlapping the circle.
        /// </summary>
        /// <param name="center">Circle center in world space.</param>
        /// <param name="radius">Circle radius.</param>
        /// <param name="damage">Damage to apply.</param>
        /// <returns>True if at least one new target was hit.</returns>
        public bool Scan(Vector2 center, float radius, float damage)
        {
            bool hitAny = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);

            for (int i = 0; i < colliders.Length; i++)
            {
                IDamageable damageable = colliders[i].GetComponentInParent<IDamageable>();
                if (damageable == null || !damageable.IsAlive || alreadyHit.Contains(damageable)) continue;

                alreadyHit.Add(damageable);
                damageable.TakeDamage(damage, center);
                hitAny = true;
            }

            return hitAny;
        }
    }
}
