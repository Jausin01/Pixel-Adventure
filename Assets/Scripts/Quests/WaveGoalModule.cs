using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Asks the player to reach a wave in an arena, a little deeper than their current record.
    /// </summary>
    public class WaveGoalModule : IQuestModule
    {
        /// <inheritdoc/>
        public bool Apply(QuestDraft draft, QuestPools pools)
        {
            if (pools.arenas == null || pools.arenas.Length == 0) return false;

            ArenaDefinition arena = pools.arenas[Random.Range(0, pools.arenas.Length)];
            int wave = Mathf.Max(pools.minWave, ArenaRecords.Best(arena.id) + Random.Range(1, 4));

            draft.waves.Add(new QuestDefinition.WaveGoal { arena = arena, wave = wave });
            draft.points += wave * 3f;
            if (draft.title == null) draft.title = "Endurance: " + arena.displayName;
            return true;
        }
    }
}
