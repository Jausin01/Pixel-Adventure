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
                Entry("apple", 10), Entry("orange", 5), Entry("health_potion", 3), Entry("energy_potion", 3), Entry("wood", 5), Entry("coal", 5), Entry("gold_ingot", 5), Entry(ProjectConstants.CurrencyId, 200),
                Entry("good_sword", 1), Entry("short_bow", 1), Entry("hand_axe", 1), Entry("iron_spear", 1),
                Entry("twin_daggers", 1), Entry("war_hammer", 1), Entry("oak_staff", 1),
                Entry("iron_shuriken", 1), Entry("wooden_boomerang", 1), Entry("leather_vest", 1)
            };
        }

        private static ItemAmount Entry(string id, int amount)
        {
            return new ItemAmount { item = ItemAssetFactory.Load(id), amount = amount };
        }
    }
}
