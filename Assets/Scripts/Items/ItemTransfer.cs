namespace MobileRpg
{
    /// <summary>
    /// Moves a whole slot from one inventory to another (backpack to stash and back). A piece of gear keeps its level
    /// and nature on the way.
    /// </summary>
    public static class ItemTransfer
    {
        /// <summary>Moves the slot. Returns false (and changes nothing) when the target has no room for it.</summary>
        public static bool Move(Inventory from, Inventory to, InventorySlot slot)
        {
            if (slot == null || !from.Contains(slot)) return false;

            if (slot.Gear != null)
            {
                from.Detach(slot);
                if (to.Insert(slot)) return true;

                from.Insert(slot);
                return false;
            }

            ItemDefinition item = slot.Item;
            int count = slot.Count;
            if (!to.Add(item, count)) return false;

            from.Remove(item, count);
            return true;
        }
    }
}
