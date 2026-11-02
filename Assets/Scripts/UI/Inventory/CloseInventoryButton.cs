namespace MobileRpg
{
    /// <summary>
    /// Touch button that closes the inventory screen.
    /// </summary>
    public class CloseInventoryButton : PointerDownAction
    {
        public InventoryPanel panel;

        /// <inheritdoc/>
        protected override void Execute()
        {
            panel.Close();
        }
    }
}
