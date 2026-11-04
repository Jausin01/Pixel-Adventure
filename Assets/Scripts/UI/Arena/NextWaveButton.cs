namespace MobileRpg
{
    /// <summary>
    /// Touch button that starts the next wave.
    /// </summary>
    public class NextWaveButton : PointerDownAction
    {
        public ArenaRun run;

        /// <inheritdoc/>
        protected override void Execute()
        {
            run.NextWave();
        }
    }
}
