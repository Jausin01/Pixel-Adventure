using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the slot rows up to date with what is worn and which slot is selected.
    /// </summary>
    public class EquipmentSlotList : MonoBehaviour
    {
        public Equipment equipment;
        public EquipmentBrowser browser;

        [Tooltip("One row per slot, in the order of EquipmentSlots.All.")]
        public EquipmentSlotRowView[] rows;

        private void OnEnable()
        {
            equipment.Changed += Refresh;
            browser.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            equipment.Changed -= Refresh;
            browser.Changed -= Refresh;
        }

        private void Refresh()
        {
            for (int i = 0; i < rows.Length; i++)
            {
                EquipmentSlot slot = rows[i].slot;
                rows[i].Show(equipment.GetPiece(slot), slot == browser.Selected);
            }
        }
    }
}
