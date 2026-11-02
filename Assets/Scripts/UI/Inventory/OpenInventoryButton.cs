namespace MobileRpg
{
    /// <summary>
    /// Touch button that opens or closes the inventory screen.
    /// </summary>
    public class OpenInventoryButton : PointerDownAction
    {
        public InventoryPanel panel;

        /// <inheritdoc/>
        protected override void Execute()
        {
            panel.Toggle();
        }
    }
}
