using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Data describing one arena: which enemies appear, how fast the waves grow and what the player earns.
    /// Create assets from the Assets/Create menu.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Arena", fileName = "NewArena")]
    public class ArenaDefinition : ScriptableObject
    {
        /// <summary>One kind of enemy an arena can spawn.</summary>
        [System.Serializable]
        public class EnemyEntry
        {
            public GameObject prefab;

            [Tooltip("Share of the wave budget this enemy uses.")]
            public int cost = 1;

            [Tooltip("First wave in which this enemy can appear.")]
            public int firstWave = 1;

            [Tooltip("Relative chance of being picked among the enemies that fit.")]
            public float weight = 1f;
        }

        [Header("Identity")]
        [Tooltip("Unique identifier, used for records and saving.")]
        public string id;
        public string displayName;

        [Tooltip("Look of the arena: the id of an entry in ArenaThemes (cave, dungeon, cornfield, farm). Used when its scene is built.")]
        public string theme;

        [Tooltip("Name of the scene that holds this arena's ground and ArenaSite. It is loaded when a run starts.")]
        public string sceneName;

        [Header("Enemies")]
        public EnemyEntry[] enemies;

        [Header("Waves")]
        [Tooltip("Budget of the first wave.")]
        public float baseBudget = 3f;

        [Tooltip("Budget added with each following wave.")]
        public float budgetPerWave = 2f;

        [Tooltip("Enemy health added per wave, as a fraction (0.1 = +10% of base health per wave).")]
        public float healthGrowth = 0.1f;

        [Tooltip("Enemy damage added per wave, as a fraction.")]
        public float damageGrowth = 0.05f;

        [Tooltip("Most enemies alive at once. Extra ones wait for a free place.")]
        public int maxAlive = 14;

        [Tooltip("Seconds between spawns inside a wave.")]
        public float spawnInterval = 0.5f;

        [Header("Risk")]
        [Tooltip("The player can leave safely every this many waves. Between checkpoints the next wave starts by itself. 0 means never.")]
        public int checkpointEvery = 5;

        [Header("Boss")]
        [Tooltip("A boss joins every this many waves. 0 for none.")]
        public int bossEvery = 0;

        [Tooltip("Bosses take turns in this order each time one is due.")]
        public GameObject[] bosses;

        [Header("Rewards")]
        [Tooltip("Rolled every time a wave is cleared.")]
        public DropEntry[] clearRewards;
    }
}
