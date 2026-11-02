using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Debug: gives the player a few items at the start so the inventory screen has something to show.
    /// Needs the items created by <see cref="ItemCatalogBuilder"/>.
    /// </summary>
    public static class StarterItemsBuilder
    {
        /// <summary>Adds the starting items component to the player.</summary>
        public static void Attach(GameObject player)
        {
            InventoryStartingItems starter = player.AddComponent<InventoryStartingItems>();
            starter.items = new[]
            {
                Entry("apple", 10), Entry("orange", 5), Entry("wood", 5), Entry("ruby", 2),
                Entry("good_sword", 1), Entry("great_sword", 1), Entry("ruby_sword", 1)
            };
        }

        private static InventoryStartingItems.ItemAmount Entry(string id, int amount)
        {
            return new InventoryStartingItems.ItemAmount { item = ItemAssetFactory.Load(id), amount = amount };
        }
    }
}
