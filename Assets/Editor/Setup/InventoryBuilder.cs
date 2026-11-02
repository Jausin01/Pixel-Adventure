using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player an inventory.
    /// </summary>
    public static class InventoryBuilder
    {
        /// <summary>Adds the inventory (and its console logger) to the player.</summary>
        public static void Attach(GameObject player)
        {
            player.AddComponent<Inventory>();
            player.AddComponent<InventoryLogger>();
        }
    }
}
