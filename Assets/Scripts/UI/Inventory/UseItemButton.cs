namespace MobileRpg
{
    /// <summary>
    /// Touch button that uses the item selected in the inventory.
    /// </summary>
    public class UseItemButton : PointerDownAction
    {
        public InventoryBrowser browser;
        public ItemUser itemUser;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (browser.Selected != null) itemUser.Use(browser.Selected.Item);
        }
    }
}
