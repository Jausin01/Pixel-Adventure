using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Stores the items an owner carries in a limited number of slots.
    /// Stackable items share one slot per item type; equipment takes one slot per piece.
    /// Does not display or save anything itself.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [Tooltip("Number of slots.")]
        public int capacity = 60;

        private readonly List<InventorySlot> slots = new List<InventorySlot>();

        /// <summary>Raised after the contents change. Arguments: the item and how many of it are now carried.</summary>
        public event Action<ItemDefinition, int> Changed;

        /// <summary>The occupied slots, in order.</summary>
        public IReadOnlyList<InventorySlot> Slots { get { return slots; } }

        /// <summary>Returns how many of an item are carried, across all slots.</summary>
        public int Count(ItemDefinition item)
        {
            int total = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item == item) total += slots[i].Count;
            }
            return total;
        }

        /// <summary>Adds items. Returns false (and adds nothing) when there is not enough room.</summary>
        public bool Add(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;

            if (item.IsStackable)
            {
                InventorySlot existing = FindSlot(item);
                if (existing != null)
                {
                    if (existing.Count + amount > item.maxStack) return false;
                    existing.Count += amount;
                }
                else if (slots.Count < capacity && amount <= item.maxStack) slots.Add(new InventorySlot(item, amount));
                else return false;
            }
            else
            {
                if (slots.Count + amount > capacity) return false;
                for (int i = 0; i < amount; i++) slots.Add(new InventorySlot(item, 1));
            }

            RaiseChanged(item);
            return true;
        }

        /// <summary>Removes items. Returns false (and removes nothing) when there are not enough.</summary>
        public bool Remove(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0 || Count(item) < amount) return false;

            int remaining = amount;
            for (int i = slots.Count - 1; i >= 0 && remaining > 0; i--)
            {
                if (slots[i].Item != item) continue;

                int taken = Mathf.Min(slots[i].Count, remaining);
                slots[i].Count -= taken;
                remaining -= taken;
                if (slots[i].Count == 0) slots.RemoveAt(i);
            }

            RaiseChanged(item);
            return true;
        }

        private InventorySlot FindSlot(ItemDefinition item)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item == item) return slots[i];
            }
            return null;
        }

        private void RaiseChanged(ItemDefinition item)
        {
            if (Changed != null) Changed(item, Count(item));
        }
    }
}
