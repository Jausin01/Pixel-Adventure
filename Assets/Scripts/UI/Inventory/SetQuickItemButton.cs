namespace MobileRpg
{
    /// <summary>
    /// Touch button that puts the item selected in the inventory on the quick button.
    /// </summary>
    public class SetQuickItemButton : PointerDownAction
    {
        public InventoryBrowser browser;
        public QuickItem quick;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (browser.Selected != null) quick.Select(browser.Selected.Item);
        }
    }
}
