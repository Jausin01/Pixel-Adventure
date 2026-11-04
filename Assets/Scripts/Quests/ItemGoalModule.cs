using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Asks the player to hand in one or two kinds of material.
    /// </summary>
    public class ItemGoalModule : IQuestModule
    {
        /// <inheritdoc/>
        public bool Apply(QuestDraft draft, QuestPools pools)
        {
            List<QuestPools.MaterialOption> options = new List<QuestPools.MaterialOption>();
            for (int i = 0; pools.materials != null && i < pools.materials.Length; i++)
            {
                if (pools.materials[i].item != null && pools.materials[i].minDone <= draft.done) options.Add(pools.materials[i]);
            }
            if (options.Count == 0) return false;

            int kinds = Mathf.Min(options.Count, Random.value < 0.5f ? 1 : 2);
            for (int n = 0; n < kinds; n++)
            {
                int pick = Random.Range(0, options.Count);
                QuestPools.MaterialOption option = options[pick];
                options.RemoveAt(pick);

                int amount = Mathf.Max(1, Mathf.RoundToInt(Random.Range(option.min, option.max + 1) * draft.scale));
                draft.requirements.Add(new ItemAmount { item = option.item, amount = amount });
                draft.points += amount * Mathf.Max(1, option.item.sellValue) * 0.5f;
                if (draft.title == null) draft.title = "Delivery: " + option.item.displayName;
            }
            return true;
        }
    }
}
