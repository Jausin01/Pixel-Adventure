using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Energy (action points) spent by skills. Regenerates over time.
    /// </summary>
    public class Energy : MonoBehaviour, IMeter
    {
        [Tooltip("Maximum energy.")]
        public float maxEnergy = 100f;

        [Tooltip("Energy regained per second.")]
        public float regenPerSecond = 8f;

        /// <summary>Current energy.</summary>
        public float Current { get; private set; }

        /// <inheritdoc/>
        public float Fraction { get { return Current / maxEnergy; } }

        /// <inheritdoc/>
        public event Action FractionChanged;

        private void Awake()
        {
            Current = maxEnergy;
        }

        private void Update()
        {
            if (Current >= maxEnergy) return;

            Current = Mathf.Min(maxEnergy, Current + regenPerSecond * Time.deltaTime);
            RaiseChanged();
        }

        /// <summary>Spends energy. Returns false (and spends nothing) when there is not enough.</summary>
        public bool TrySpend(float amount)
        {
            if (Current < amount) return false;

            Current -= amount;
            RaiseChanged();
            return true;
        }

        /// <summary>Restores some energy, up to the maximum.</summary>
        public void Restore(float amount)
        {
            if (amount <= 0f) return;

            Current = Mathf.Min(maxEnergy, Current + amount);
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            if (FractionChanged != null) FractionChanged();
        }
    }
}
