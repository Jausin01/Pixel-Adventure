using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Finds the closest living <see cref="IDamageable"/> within a range.
    /// </summary>
    public static class TargetFinder
    {
        /// <summary>
        /// Returns the transform of the nearest living damageable object, or null if none is in range.
        /// </summary>
        /// <param name="origin">World position to search from.</param>
        /// <param name="range">Search radius.</param>
        public static Transform FindNearest(Vector2 origin, float range, Transform ignoredRoot = null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, range);
            Transform best = null;
            float bestSqrDistance = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (ignoredRoot != null && colliders[i].transform.IsChildOf(ignoredRoot)) continue;

                IDamageable damageable = colliders[i].GetComponentInParent<IDamageable>();
                if (damageable == null || !damageable.IsAlive) continue;

                Component component = damageable as Component;
                if (component == null) continue;

                float sqrDistance = ((Vector2)component.transform.position - origin).sqrMagnitude;
                if (sqrDistance < bestSqrDistance)
                {
                    bestSqrDistance = sqrDistance;
                    best = component.transform;
                }
            }

            return best;
        }
    }
}
