using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Debug helper: puts a few items in the inventory when a new game starts, so the inventory screen can be tested.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class InventoryStartingItems : MonoBehaviour
    {
        public ItemAmount[] items;

        private void Start()
        {
            if (SaveFile.Exists) return; // a saved game decides what the player owns

            Inventory inventory = GetComponent<Inventory>();
            for (int i = 0; i < items.Length; i++) inventory.Add(items[i].item, items[i].amount);
        }
    }
}
