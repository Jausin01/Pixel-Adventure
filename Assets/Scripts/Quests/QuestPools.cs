using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Everything the quest generator may pick from: materials to ask for, enemies to hunt, arenas to push through
    /// and rewards to give. Options unlock as the player completes more quests.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Quest Pools", fileName = "QuestPools")]
    public class QuestPools : ScriptableObject
    {
        /// <summary>An item a quest may ask the player to hand in.</summary>
        [System.Serializable]
        public class MaterialOption
        {
            public ItemDefinition item;
            public int min = 3;
            public int max = 6;

            [Tooltip("Quests completed before this option can appear.")]
            public int minDone;
        }

        /// <summary>An enemy (or group) a quest may ask the player to defeat.</summary>
        [System.Serializable]
        public class EnemyOption
        {
            [Tooltip("Enemy id or family. Empty counts every enemy.")]
            public string enemy;
            public string label;

            [Tooltip("Restrict the kills to one arena. Empty means anywhere.")]
            public ArenaDefinition arena;
            public int min = 5;
            public int max = 10;
            public int minDone;
        }

        /// <summary>An item a quest may give as its main reward.</summary>
        [System.Serializable]
        public class RewardOption
        {
            public ItemDefinition item;
            public int min = 1;
            public int max = 1;
            public float weight = 1f;
            public int minDone;
        }

        public MaterialOption[] materials;
        public EnemyOption[] enemies;

        [Tooltip("Arenas a quest may ask the player to push to a deeper wave.")]
        public ArenaDefinition[] arenas;
        public RewardOption[] rewards;

        [Tooltip("The money item. Every quest also pays some of it.")]
        public ItemDefinition currency;

        [Tooltip("Currency paid per point of quest difficulty.")]
        public float currencyPerPoint = 1.5f;

        [Tooltip("Lowest wave a wave goal can ask for.")]
        public int minWave = 3;

        [Tooltip("How much quests grow with each completed quest: amounts are multiplied by 1 + done * this, up to the cap.")]
        public float growthPerQuest = 0.05f;
        public float maxGrowth = 3f;
    }
}
