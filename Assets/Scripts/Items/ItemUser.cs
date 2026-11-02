using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Lets the player use consumable items from the inventory: applies the item's effect and removes one.
    /// </summary>
    [RequireComponent(typeof(Inventory), typeof(Health), typeof(Energy))]
    public class ItemUser : MonoBehaviour
    {
        private Inventory inventory;
        private Health health;
        private Energy energy;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            health = GetComponent<Health>();
            energy = GetComponent<Energy>();
        }

        /// <summary>True when the item is a consumable with an effect, the player carries it and is alive.</summary>
        public bool CanUse(ItemDefinition item)
        {
            return item != null
                && item.category == ItemCategory.Consumable
                && (item.healthRestored > 0f || item.energyRestored > 0f)
                && health.IsAlive
                && inventory.Count(item) > 0;
        }

        /// <summary>Uses one of the item. Returns false when it cannot be used.</summary>
        public bool Use(ItemDefinition item)
        {
            if (!CanUse(item)) return false;

            if (item.healthRestored > 0f) health.Heal(item.healthRestored);
            if (item.energyRestored > 0f) energy.Restore(item.energyRestored);
            return inventory.Remove(item, 1);
        }
    }
}
