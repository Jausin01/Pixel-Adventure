using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Implemented by anything that can receive damage from weapons or projectiles.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>True while the object can still receive damage and be targeted.</summary>
        bool IsAlive { get; }

        /// <summary>Applies damage to the object.</summary>
        /// <param name="amount">Amount of damage.</param>
        /// <param name="source">World position the damage came from.</param>
        void TakeDamage(float amount, Vector2 source);
    }
}
