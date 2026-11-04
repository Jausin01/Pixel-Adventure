using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Asks the player to defeat a number of enemies of one kind.
    /// </summary>
    public class KillGoalModule : IQuestModule
    {
        /// <inheritdoc/>
        public bool Apply(QuestDraft draft, QuestPools pools)
        {
            List<QuestPools.EnemyOption> options = new List<QuestPools.EnemyOption>();
            for (int i = 0; pools.enemies != null && i < pools.enemies.Length; i++)
            {
                if (pools.enemies[i].minDone <= draft.done) options.Add(pools.enemies[i]);
            }
            if (options.Count == 0) return false;

            QuestPools.EnemyOption option = options[Random.Range(0, options.Count)];
            int count = Mathf.Max(1, Mathf.RoundToInt(Random.Range(option.min, option.max + 1) * draft.scale));

            draft.kills.Add(new QuestDefinition.KillGoal { enemy = option.enemy, label = option.label, arena = option.arena, count = count });
            draft.points += count * 1.5f;
            if (draft.title == null) draft.title = "Hunt: " + option.label;
            return true;
        }
    }
}
