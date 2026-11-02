using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The state of the inventory screen: which category tab is active, which items it shows and which one is selected.
    /// The grid, tabs and detail panel only read from here and report player input back.
    /// </summary>
    public class InventoryBrowser : MonoBehaviour
    {
        public Inventory inventory;

        private readonly List<InventorySlot> visible = new List<InventorySlot>();
        private bool hasFilter;
        private ItemCategory filter;

        /// <summary>Raised when the visible items, the filter or the selection change.</summary>
        public event Action Changed;

        /// <summary>Slots that match the active tab, in order.</summary>
        public IReadOnlyList<InventorySlot> Visible { get { return visible; } }

        /// <summary>The selected slot, or null.</summary>
        public InventorySlot Selected { get; private set; }

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            Rebuild();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        /// <summary>Shows every item.</summary>
        public void ShowAll()
        {
            hasFilter = false;
            Rebuild();
        }

        /// <summary>Shows only items of one category.</summary>
        public void ShowCategory(ItemCategory category)
        {
            hasFilter = true;
            filter = category;
            Rebuild();
        }

        /// <summary>True when the given tab is the active one.</summary>
        public bool IsActive(bool all, ItemCategory category)
        {
            return all ? !hasFilter : hasFilter && filter == category;
        }

        /// <summary>Selects a slot.</summary>
        public void Select(InventorySlot slot)
        {
            Selected = slot;
            RaiseChanged();
        }

        private void OnInventoryChanged(ItemDefinition item, int total)
        {
            Rebuild();
        }

        private void Rebuild()
        {
            visible.Clear();
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                InventorySlot slot = inventory.Slots[i];
                if (!hasFilter || slot.Item.category == filter) visible.Add(slot);
            }

            if (Selected == null || !visible.Contains(Selected)) Selected = visible.Count > 0 ? visible[0] : null;
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
