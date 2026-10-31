namespace MobileRpg
{
    /// <summary>
    /// Implemented by every weapon the player can hold, so the weapon holder can use any of them the same way.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>True while an attack sequence is running (the weapon cannot be swapped out).</summary>
        bool IsBusy { get; }

        /// <summary>Starts an attack, or queues the next one if an attack is already running.</summary>
        void RequestAttack();
    }
}
