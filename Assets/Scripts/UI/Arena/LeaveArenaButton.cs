namespace MobileRpg
{
    /// <summary>
    /// Touch button that leaves the arena and keeps everything collected.
    /// </summary>
    public class LeaveArenaButton : PointerDownAction
    {
        public ArenaRun run;

        /// <inheritdoc/>
        protected override void Execute()
        {
            run.Leave();
        }
    }
}
