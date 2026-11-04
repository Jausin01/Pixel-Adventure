using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the inventory screen: the shared window shell plus category tabs, a scrolling item grid
    /// and a detail panel. The parts are made by their own builders; this class only puts them together.
    /// </summary>
    public static class InventoryScreenBuilder
    {
        /// <summary>Builds the screen. Its window starts hidden.</summary>
        public static InventoryPanel Build(Transform parent, Inventory inventory, ItemUser itemUser)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(parent, "Inventory Screen", "Inventory", out root, out panel);

            InventoryBrowser browser = root.AddComponent<InventoryBrowser>();
            browser.inventory = inventory;

            InventoryTabsBuilder.Build(frame, browser);
            InventoryGridBuilder.Build(frame, browser, inventory.GetComponent<RunLedger>(), inventory.capacity);
            InventoryDetailBuilder.Build(frame, browser, itemUser, inventory.GetComponent<QuickItem>());
            return panel;
        }
    }
}
