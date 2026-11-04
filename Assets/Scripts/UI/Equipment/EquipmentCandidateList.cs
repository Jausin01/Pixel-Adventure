using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Lists the inventory items that fit the selected slot, one row each.
    /// </summary>
    public class EquipmentCandidateList : MonoBehaviour
    {
        public Equipment equipment;
        public Inventory inventory;
        public EquipmentBrowser browser;

        [Tooltip("Pooled rows. Unused rows are hidden.")]
        public EquipmentCandidateRowView[] rows;

        [Tooltip("Shown when nothing fits the selected slot.")]
        public GameObject emptyHint;

        private void OnEnable()
        {
            equipment.Changed += Refresh;
            inventory.Changed += OnInventoryChanged;
            browser.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            equipment.Changed -= Refresh;
            inventory.Changed -= OnInventoryChanged;
            browser.Changed -= Refresh;
        }

        private void OnInventoryChanged(ItemDefinition item, int total)
        {
            Refresh();
        }

        private void Refresh()
        {
            int shown = 0;
            for (int i = 0; i < inventory.Slots.Count && shown < rows.Length; i++)
            {
                InventorySlot piece = inventory.Slots[i];
                if (EquipmentSlots.Accepts(browser.Selected, piece.Item)) rows[shown++].Show(piece);
            }

            for (int i = shown; i < rows.Length; i++) rows[i].Hide();
            emptyHint.SetActive(shown == 0);
        }
    }
}
