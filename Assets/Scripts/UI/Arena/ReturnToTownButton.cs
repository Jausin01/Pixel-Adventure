namespace MobileRpg
{
    /// <summary>
    /// Touch button on the game over screen that sends the player back to the town.
    /// </summary>
    public class ReturnToTownButton : PointerDownAction
    {
        public GameOverPanel panel;

        /// <inheritdoc/>
        protected override void Execute()
        {
            panel.Confirm();
        }
    }
}
