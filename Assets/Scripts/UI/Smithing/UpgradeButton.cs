namespace MobileRpg
{
    /// <summary>
    /// Touch button that upgrades the selected piece one level.
    /// </summary>
    public class UpgradeButton : PointerDownAction
    {
        public SmithBrowser browser;
        public Blacksmith blacksmith;

        /// <inheritdoc/>
        protected override void Execute()
        {
            blacksmith.TryUpgrade(browser.Selected);
        }
    }
}
