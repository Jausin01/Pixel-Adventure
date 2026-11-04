using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Remembers the best wave reached in each arena.
    /// </summary>
    public static class ArenaRecords
    {
        /// <summary>The most waves cleared in the arena, or 0.</summary>
        public static int Best(string arenaId)
        {
            return PlayerPrefs.GetInt(Key(arenaId), 0);
        }

        /// <summary>Stores the result when it beats the current record.</summary>
        public static void Submit(string arenaId, int wavesCleared)
        {
            if (wavesCleared <= Best(arenaId)) return;

            PlayerPrefs.SetInt(Key(arenaId), wavesCleared);
            PlayerPrefs.Save();
        }

        private static string Key(string arenaId)
        {
            return "arena_best_" + arenaId;
        }
    }
}
