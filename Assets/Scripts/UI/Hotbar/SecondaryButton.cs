namespace MobileRpg
{
    /// <summary>
    /// Touch button that throws the secondary weapon.
    /// </summary>
    public class SecondaryButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override string ClickSound { get { return null; } }

        public SecondaryThrower thrower;

        /// <inheritdoc/>
        protected override void Execute()
        {
            thrower.Throw();
        }
    }
}
