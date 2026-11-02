namespace MobileRpg
{
    /// <summary>
    /// Broad kind of an item. Decides how the item is stored and which inventory tab shows it.
    /// The numeric values are saved in item assets, so only add new values at the end.
    /// </summary>
    public enum ItemCategory
    {
        /// <summary>Crafting materials (wood, ore). Stack in one slot.</summary>
        Material = 0,

        /// <summary>Items that are used up (fruit, potions). Stack in one slot.</summary>
        Consumable = 1,

        /// <summary>Armor and accessories. Never stack: every piece takes its own slot.</summary>
        Equipment = 2,

        /// <summary>Weapons. Never stack: every piece takes its own slot.</summary>
        Weapon = 3,

        /// <summary>Items needed by quests. Stack in one slot.</summary>
        Quest = 4
    }
}
