namespace MobileRpg
{
    /// <summary>
    /// Implemented by every weapon the player can hold, so the weapon holder can use any of them the same way.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>The weapon class this weapon belongs to. Decides which equipped item selects it and which skills apply.</summary>
        WeaponClass Class { get; }

        /// <summary>Damage of one hit. Kept in line with the character's stats.</summary>
        float Damage { set; }

        /// <summary>Set by skills that take over the weapon. While true the weapon cannot attack and counts as busy.</summary>
        bool Locked { get; set; }

        /// <summary>True while an attack sequence is running (the weapon cannot be swapped out).</summary>
        bool IsBusy { get; }

        /// <summary>Starts an attack, or queues the next one if an attack is already running.</summary>
        void RequestAttack();
    }
}
