using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Tracks how much of a resource (tree, ore, chest) is left. Like health, but for objects tools work on.
    /// </summary>
    public class ResourceDurability : MonoBehaviour
    {
        [Tooltip("Amount of work needed to fully harvest the resource.")]
        public float maxDurability = 5f;

        /// <summary>Raised on every hit. Arguments: amount removed and the source position.</summary>
        public event Action<float, Vector2> Hit;

        /// <summary>Raised whenever the remaining amount changes. Argument: remaining fraction from 0 to 1.</summary>
        public event Action<float> Changed;

        /// <summary>Raised once when durability reaches zero.</summary>
        public event Action Depleted;

        /// <summary>Raised when the resource is restored to full.</summary>
        public event Action Restored;

        private float current;

        /// <summary>Remaining durability as a fraction from 0 to 1.</summary>
        public float Remaining01 { get { return current / maxDurability; } }

        /// <summary>True when nothing is left.</summary>
        public bool IsDepleted { get { return current <= 0f; } }

        private void Awake()
        {
            current = maxDurability;
        }

        /// <summary>Removes durability.</summary>
        public void ApplyHit(float amount, Vector2 source)
        {
            if (IsDepleted) return;

            current = Mathf.Max(0f, current - amount);
            if (Hit != null) Hit(amount, source);
            if (Changed != null) Changed(Remaining01);
            if (IsDepleted && Depleted != null) Depleted();
        }

        /// <summary>Brings the resource back to full.</summary>
        public void Restore()
        {
            current = maxDurability;
            if (Changed != null) Changed(Remaining01);
            if (Restored != null) Restored();
        }
    }
}
