namespace MobileRpg
{
    /// <summary>
    /// One occupied slot of an inventory: an item and how many of it the slot holds.
    /// </summary>
    public class InventorySlot
    {
        /// <summary>The item in the slot.</summary>
        public readonly ItemDefinition Item;

        /// <summary>How many of the item the slot holds.</summary>
        public int Count;

        /// <summary>Creates a slot.</summary>
        public InventorySlot(ItemDefinition item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
