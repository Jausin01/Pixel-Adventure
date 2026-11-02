using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Finds harvestable objects near a point.
    /// </summary>
    public static class HarvestTargetFinder
    {
        /// <summary>Returns the nearest object that still has something to harvest, or null.</summary>
        /// <param name="origin">Point to search around.</param>
        /// <param name="range">Search radius.</param>
        /// <param name="filter">Only objects needing this tool are considered. <see cref="ToolType.None"/> accepts any.</param>
        public static IHarvestable FindNearest(Vector2 origin, float range, ToolType filter)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, range);
            IHarvestable best = null;
            float bestSqrDistance = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                IHarvestable candidate = colliders[i].GetComponentInParent<IHarvestable>();
                if (candidate == null || candidate.IsDepleted) continue;
                if (filter != ToolType.None && candidate.RequiredTool != filter) continue;

                float sqrDistance = (candidate.Position - origin).sqrMagnitude;
                if (sqrDistance < bestSqrDistance)
                {
                    bestSqrDistance = sqrDistance;
                    best = candidate;
                }
            }

            return best;
        }
    }
}
