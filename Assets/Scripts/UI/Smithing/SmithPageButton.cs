namespace MobileRpg
{
    /// <summary>
    /// Touch button that flips the blacksmith's list to the next or previous page.
    /// </summary>
    public class SmithPageButton : PointerDownAction
    {
        public SmithBrowser browser;

        [UnityEngine.Tooltip("+1 for the next page, -1 for the previous one.")]
        public int direction = 1;

        /// <inheritdoc/>
        protected override void Execute()
        {
            browser.ChangePage(direction);
        }
    }
}
