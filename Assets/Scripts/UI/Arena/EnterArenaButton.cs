namespace MobileRpg
{
    /// <summary>
    /// Touch button that closes the portal screen and enters the chosen arena.
    /// </summary>
    public class EnterArenaButton : PointerDownAction
    {
        public ArenaRun run;
        public ArenaDefinition arena;
        public InventoryPanel screen;

        /// <inheritdoc/>
        protected override void Execute()
        {
            screen.Close();
            run.Enter(arena);
        }
    }
}
