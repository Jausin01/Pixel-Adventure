namespace MobileRpg
{
    /// <summary>
    /// Tracks which step of a looping combo comes next and resets to the first step
    /// when too much time passes between attacks.
    /// </summary>
    public sealed class ComboCounter
    {
        private readonly int stepCount;
        private readonly float resetTime;
        private int step;
        private float lastCompletedTime = -999f;

        /// <param name="stepCount">Number of steps in the combo.</param>
        /// <param name="resetTime">Seconds after which the combo restarts from the first step.</param>
        public ComboCounter(int stepCount, float resetTime)
        {
            this.stepCount = stepCount;
            this.resetTime = resetTime;
        }

        /// <summary>Returns the step to perform now, restarting the combo if it expired.</summary>
        /// <param name="now">Current time in seconds.</param>
        public int Peek(float now)
        {
            if (now - lastCompletedTime > resetTime) step = 0;
            return step;
        }

        /// <summary>Marks the current step as finished and advances to the next one.</summary>
        /// <param name="now">Current time in seconds.</param>
        public void Complete(float now)
        {
            step = (step + 1) % stepCount;
            lastCompletedTime = now;
        }
    }
}
