using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The player's music and effects volume (0 to 1). Saved in PlayerPrefs, separate from the game save.
    /// </summary>
    public static class VolumeSettings
    {
        private const string MusicKey = "volume_music";
        private const string SfxKey = "volume_sfx";

        private static float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        private static float sfx = PlayerPrefs.GetFloat(SfxKey, 1f);

        /// <summary>Raised when either volume changes.</summary>
        public static event Action Changed;

        /// <summary>Music volume, 0 to 1.</summary>
        public static float Music { get { return music; } }

        /// <summary>Sound effects volume, 0 to 1.</summary>
        public static float Sfx { get { return sfx; } }

        /// <summary>Sets the music volume and saves it.</summary>
        public static void SetMusic(float value)
        {
            music = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MusicKey, music);
            if (Changed != null) Changed();
        }

        /// <summary>Sets the effects volume and saves it.</summary>
        public static void SetSfx(float value)
        {
            sfx = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(SfxKey, sfx);
            if (Changed != null) Changed();
        }
    }
}
