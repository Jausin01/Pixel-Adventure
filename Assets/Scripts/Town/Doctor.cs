using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The town doctor's service: restores all health and energy for a price in the currency item.
    /// Does not display anything.
    /// </summary>
    [RequireComponent(typeof(Inventory), typeof(Health), typeof(Energy))]
    public class Doctor : MonoBehaviour
    {
        [Tooltip("The item the doctor is paid with (gold ingots).")]
        public ItemDefinition currency;

        [Tooltip("How many of the currency item one treatment costs.")]
        public int cost = 1;

        private Inventory inventory;
        private Health health;
        private Energy energy;

        /// <summary>True when health or energy is below the maximum.</summary>
        public bool NeedsTreatment
        {
            get { return Health.Fraction < 0.999f || Energy.Fraction < 0.999f; }
        }

        /// <summary>True when the player is hurt or tired and can pay.</summary>
        public bool CanTreat { get { return NeedsTreatment && Items.Count(currency) >= cost; } }

        /// <summary>Takes the price and restores health and energy. Returns false (and changes nothing) when it cannot.</summary>
        public bool TryTreat()
        {
            bool done = TryTreatCore();
            if (done) Sfx.Play("heal"); else Sfx.Play("ui_error"); 
            return done;
        }

        private bool TryTreatCore()
        {
            if (!CanTreat) return false;

            Items.Remove(currency, cost);
            Health.Heal(Health.maxHealth);
            Energy.Restore(Energy.maxEnergy);
            return true;
        }

        // Resolved on first use: other components may ask before this object's Awake has run.
        private Inventory Items
        {
            get
            {
                if (inventory == null) inventory = GetComponent<Inventory>();
                return inventory;
            }
        }

        private Health Health
        {
            get
            {
                if (health == null) health = GetComponent<Health>();
                return health;
            }
        }

        private Energy Energy
        {
            get
            {
                if (energy == null) energy = GetComponent<Energy>();
                return energy;
            }
        }
    }
}
