namespace MobileRpg
{
    /// <summary>
    /// Touch button for one weapon-class tab on the skill teacher's screen.
    /// </summary>
    public class SkillClassTabButton : PointerDownAction
    {
        public SkillClassTabs tabs;
        public int index;

        /// <inheritdoc/>
        protected override void Execute()
        {
            tabs.Select(index);
        }
    }
}
