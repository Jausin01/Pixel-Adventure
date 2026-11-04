using System.Collections.Generic;

namespace MobileRpg
{
    /// <summary>
    /// A quest being assembled by the generator's modules. Each module adds its goals and its share of difficulty.
    /// </summary>
    public class QuestDraft
    {
        public readonly List<ItemAmount> requirements = new List<ItemAmount>();
        public readonly List<QuestDefinition.KillGoal> kills = new List<QuestDefinition.KillGoal>();
        public readonly List<QuestDefinition.WaveGoal> waves = new List<QuestDefinition.WaveGoal>();

        /// <summary>Name of the quest. The first module that sets it wins.</summary>
        public string title;

        /// <summary>Difficulty, used to size the reward.</summary>
        public float points;

        /// <summary>Number of quests completed so far; unlocks options.</summary>
        public int done;

        /// <summary>Multiplier applied to amounts as the player progresses.</summary>
        public float scale = 1f;

        /// <summary>True when no module added anything.</summary>
        public bool IsEmpty { get { return requirements.Count == 0 && kills.Count == 0 && waves.Count == 0; } }
    }
}
