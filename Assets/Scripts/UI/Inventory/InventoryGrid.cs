using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Fills the slot views with the items the <see cref="InventoryBrowser"/> says are visible.
    /// Views beyond the visible items are shown empty.
    /// </summary>
    public class InventoryGrid : MonoBehaviour
    {
        public InventoryBrowser browser;

        [Tooltip("Used to mark the items collected during the current arena run.")]
        public RunLedger ledger;

        [Tooltip("One view per inventory slot, in order.")]
        public InventorySlotView[] slotViews;

        private void OnEnable()
        {
            browser.Changed += Refresh;
            ledger.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            browser.Changed -= Refresh;
            ledger.Changed -= Refresh;
        }

        private void Refresh()
        {
            HashSet<InventorySlot> atRisk = FindSlotsAtRisk();

            for (int i = 0; i < slotViews.Length; i++)
            {
                if (i < browser.Visible.Count)
                {
                    InventorySlot slot = browser.Visible[i];
                    slotViews[i].Show(slot, slot == browser.Selected, atRisk.Contains(slot));
                }
                else
                {
                    slotViews[i].Clear();
                }
            }
        }

        // Marks the slots holding items collected during the run. A stackable item has one slot; for items that do
        // not stack, the most recently added copies are the ones marked.
        private HashSet<InventorySlot> FindSlotsAtRisk()
        {
            HashSet<InventorySlot> marked = new HashSet<InventorySlot>();
            Dictionary<ItemDefinition, int> remaining = new Dictionary<ItemDefinition, int>();
            IReadOnlyList<InventorySlot> all = browser.inventory.Slots;

            for (int i = all.Count - 1; i >= 0; i--)
            {
                ItemDefinition item = all[i].Item;

                int left;
                if (!remaining.TryGetValue(item, out left)) left = ledger.GainedCount(item);
                if (left <= 0) continue;

                marked.Add(all[i]);
                remaining[item] = item.IsStackable ? 0 : left - 1;
            }
            return marked;
        }
    }
}
