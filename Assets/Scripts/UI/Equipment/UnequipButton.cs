namespace MobileRpg
{
    /// <summary>
    /// Touch button that takes off the item in the selected slot.
    /// </summary>
    public class UnequipButton : PointerDownAction
    {
        public Equipment equipment;
        public EquipmentBrowser browser;

        /// <inheritdoc/>
        protected override void Execute()
        {
            equipment.Unequip(browser.Selected);
        }
    }
}
