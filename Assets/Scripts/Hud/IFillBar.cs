namespace MobileRpg
{
    /// <summary>
    /// A bar that can show a value from 0 to 1 (health, stamina, cooldown...).
    /// </summary>
    public interface IFillBar
    {
        /// <summary>Shows the given fraction, from 0 (empty) to 1 (full).</summary>
        void SetFraction(float fraction);
    }
}
