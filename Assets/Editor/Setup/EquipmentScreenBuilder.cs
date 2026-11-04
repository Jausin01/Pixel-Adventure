using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the equipment screen: the shared window shell plus the slot list, the list of items
    /// that fit the selected slot, and the status panel. Only puts the parts together.
    /// </summary>
    public static class EquipmentScreenBuilder
    {
        /// <summary>Builds the screen. Its window starts hidden.</summary>
        public static InventoryPanel Build(Transform parent, Equipment equipment, Inventory inventory, CharacterStats stats)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(parent, "Equipment Screen", "Equipment", out root, out panel);

            EquipmentBrowser browser = root.AddComponent<EquipmentBrowser>();

            EquipmentSlotListBuilder.Build(frame, equipment, browser);
            EquipmentCandidateListBuilder.Build(frame, equipment, inventory, browser);
            EquipmentStatusBuilder.Build(frame, equipment, stats, browser);
            return panel;
        }
    }
}
