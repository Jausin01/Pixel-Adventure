namespace MobileRpg
{
    /// <summary>
    /// One occupied slot of an inventory: an item and how many of it the slot holds.
    /// Gear never stacks, so a gear slot is one individual piece and carries its own <see cref="GearState"/>.
    /// </summary>
    public class InventorySlot
    {
        /// <summary>The item in the slot.</summary>
        public readonly ItemDefinition Item;

        /// <summary>How many of the item the slot holds.</summary>
        public int Count;

        /// <summary>The individual state of a piece of gear, or null for items that are not gear.</summary>
        public readonly GearState Gear;

        /// <summary>Creates a slot. Gear gets a new state with a random nature.</summary>
        public InventorySlot(ItemDefinition item, int count)
        {
            Item = item;
            Count = count;
            if (item.equipKind != EquipmentKind.None) Gear = GearState.CreateNew(item);
        }

        /// <summary>Creates a slot for an existing piece of gear (loading a save, moving a piece around).</summary>
        public InventorySlot(ItemDefinition item, GearState gear)
        {
            Item = item;
            Count = 1;
            Gear = gear;
        }
    }
}
