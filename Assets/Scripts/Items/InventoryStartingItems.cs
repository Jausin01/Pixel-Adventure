using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Debug helper: puts a few items in the inventory when the game starts, so the inventory screen can be tested.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class InventoryStartingItems : MonoBehaviour
    {
        /// <summary>An item and an amount.</summary>
        [System.Serializable]
        public class ItemAmount
        {
            public ItemDefinition item;
            public int amount = 1;
        }

        public ItemAmount[] items;

        private void Start()
        {
            Inventory inventory = GetComponent<Inventory>();
            for (int i = 0; i < items.Length; i++) inventory.Add(items[i].item, items[i].amount);
        }
    }
}
