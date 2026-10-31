using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Stores hit points and reports changes through events. It contains no visuals or reactions:
    /// other components subscribe to the events to implement them.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [Tooltip("Maximum hit points.")]
        public float maxHealth = 100f;

        /// <summary>Raised after damage is applied. Arguments: damage amount, source position.</summary>
        public event Action<float, Vector2> Damaged;

        /// <summary>Raised once when hit points reach zero.</summary>
        public event Action Died;

        /// <summary>Raised when health is restored after death.</summary>
        public event Action Revived;

        /// <summary>Current hit points.</summary>
        public float CurrentHealth { get; private set; }

        /// <inheritdoc/>
        public bool IsAlive { get { return CurrentHealth > 0f; } }

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        /// <inheritdoc/>
        public void TakeDamage(float amount, Vector2 source)
        {
            if (!IsAlive) return;

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            if (Damaged != null) Damaged(amount, source);
            if (CurrentHealth <= 0f && Died != null) Died();
        }

        /// <summary>Restores full health and raises <see cref="Revived"/>.</summary>
        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            if (Revived != null) Revived();
        }
    }
}
