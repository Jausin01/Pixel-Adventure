using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// An item lying in the world. Holds what it is and how many; adds itself to an inventory when collected.
    /// </summary>
    public class ItemPickup : MonoBehaviour
    {
        /// <summary>The item this pickup gives.</summary>
        public ItemDefinition Item { get; private set; }

        /// <summary>How many of the item it gives.</summary>
        public int Amount { get; private set; }

        /// <summary>Sets what the pickup contains.</summary>
        public void Initialize(ItemDefinition item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        /// <summary>
        /// Gives the item to an inventory and removes the pickup from the world.
        /// Does nothing (and returns false) when the inventory is full.
        /// </summary>
        public bool Collect(Inventory inventory)
        {
            if (!inventory.Add(Item, Amount)) return false;

            Destroy(gameObject);
            return true;
        }
    }
}
