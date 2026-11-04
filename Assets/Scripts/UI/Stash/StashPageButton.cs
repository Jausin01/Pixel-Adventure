namespace MobileRpg
{
    /// <summary>
    /// Touch button that flips one pane of the stash screen to the next or previous page.
    /// </summary>
    public class StashPageButton : PointerDownAction
    {
        public StashPane pane;

        [UnityEngine.Tooltip("+1 for the next page, -1 for the previous one.")]
        public int direction = 1;

        /// <inheritdoc/>
        protected override void Execute()
        {
            pane.ChangePage(direction);
        }
    }
}
