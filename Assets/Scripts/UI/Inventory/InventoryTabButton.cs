namespace MobileRpg
{
    /// <summary>
    /// Touch button for one category tab of the inventory.
    /// </summary>
    public class InventoryTabButton : PointerDownAction
    {
        public InventoryBrowser browser;

        [UnityEngine.Tooltip("When true the tab shows every item and the category is ignored.")]
        public bool showAll;

        public ItemCategory category;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (showAll) browser.ShowAll();
            else browser.ShowCategory(category);
        }
    }
}
