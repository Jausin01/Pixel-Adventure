namespace MobileRpg
{
    /// <summary>
    /// Touch button that expands or collapses the menu.
    /// </summary>
    public class MenuToggleButton : PointerDownAction
    {
        public MenuExpander menu;

        /// <inheritdoc/>
        protected override void Execute()
        {
            menu.Toggle();
        }
    }
}
