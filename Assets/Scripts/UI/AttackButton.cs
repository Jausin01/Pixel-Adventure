namespace MobileRpg
{
    /// <summary>
    /// Touch button for the main action: attacks, or uses a tool when a resource is nearby.
    /// </summary>
    public class AttackButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override string ClickSound { get { return null; } }

        public ActionRouter router;

        /// <inheritdoc/>
        protected override void Execute()
        {
            router.Perform();
        }
    }
}
