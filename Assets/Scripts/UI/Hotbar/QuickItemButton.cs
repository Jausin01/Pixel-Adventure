namespace MobileRpg
{
    /// <summary>
    /// Touch button that uses the quick item.
    /// </summary>
    public class QuickItemButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override string ClickSound { get { return null; } }

        public QuickItem quick;

        /// <inheritdoc/>
        protected override void Execute()
        {
            quick.Use();
        }
    }
}
