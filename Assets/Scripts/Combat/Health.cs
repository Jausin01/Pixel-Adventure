using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Hit points of a character. Applies damage and reports changes through events;
    /// what happens on damage or death is handled by other components that listen to those events.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable, IMeter
    {
        [Tooltip("Maximum hit points.")]
        public float maxHealth = 100f;

        /// <summary>Raised after damage is applied. Arguments: amount and the world position the damage came from.</summary>
        public event Action<float, Vector2> Damaged;

        /// <summary>Raised once when health reaches zero.</summary>
        public event Action Died;

        /// <summary>Raised when health is restored to full after death.</summary>
        public event Action Revived;

        /// <inheritdoc/>
        public event Action FractionChanged;

        /// <summary>Current hit points.</summary>
        public float CurrentHealth { get; private set; }

        /// <inheritdoc/>
        public bool IsAlive { get { return CurrentHealth > 0f; } }

        /// <inheritdoc/>
        public float Fraction { get { return CurrentHealth / maxHealth; } }

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
            if (FractionChanged != null) FractionChanged();
            if (CurrentHealth <= 0f && Died != null) Died();
        }

        /// <summary>Restores some health, up to the maximum. Does nothing when dead.</summary>
        public void Heal(float amount)
        {
            if (!IsAlive || amount <= 0f) return;

            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
            if (FractionChanged != null) FractionChanged();
        }

        /// <summary>Restores health to full.</summary>
        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            if (FractionChanged != null) FractionChanged();
            if (Revived != null) Revived();
        }
    }
}
