namespace MobileRpg
{
    /// <summary>
    /// Static entry point for sound effects. Safe to call from anywhere: it does nothing when no
    /// <see cref="SfxPlayer"/> exists or the id is unknown.
    /// </summary>
    public static class Sfx
    {
        /// <summary>The active player, registered by <see cref="SfxPlayer"/>.</summary>
        public static SfxPlayer Player;

        /// <summary>Plays the sound with the given id.</summary>
        public static void Play(string id)
        {
            if (Player != null && !string.IsNullOrEmpty(id)) Player.Play(id);
        }
    }
}
