using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Data describing one town quest: items to hand in, enemies to defeat, waves to reach, and items received in return.
    /// Create assets from the Assets/Create menu.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Quest", fileName = "NewQuest")]
    public class QuestDefinition : ScriptableObject
    {
        /// <summary>Kill a number of enemies, optionally only of one kind and/or inside one arena.</summary>
        [System.Serializable]
        public class KillGoal
        {
            [Tooltip("Enemy id or family (for example brown_rat or rat). Empty counts every enemy.")]
            public string enemy;

            [Tooltip("Name shown to the player, for example Rats.")]
            public string label;

            [Tooltip("Only kills inside this arena count. Empty means anywhere.")]
            public ArenaDefinition arena;

            public int count = 1;
        }

        /// <summary>Clear a wave of an arena at least once.</summary>
        [System.Serializable]
        public class WaveGoal
        {
            public ArenaDefinition arena;
            public int wave = 1;
        }

        [Tooltip("Unique identifier, used for saving.")]
        public string id;

        [Tooltip("Name shown on the quest board.")]
        public string title;

        [Tooltip("Items taken from the player when the quest is handed in.")]
        public ItemAmount[] requirements;

        [Tooltip("Enemies to defeat before the quest can be handed in. Progress counts from the start of the game.")]
        public KillGoal[] kills;

        [Tooltip("Waves to reach before the quest can be handed in.")]
        public WaveGoal[] waves;

        [Tooltip("Items given to the player.")]
        public ItemAmount[] rewards;
    }
}
