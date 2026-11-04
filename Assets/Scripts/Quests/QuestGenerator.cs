using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Creates random quests by combining one to three modules, then sizing a reward to the difficulty.
    /// A new quest never has the same signature as one of the quests already on the board.
    /// </summary>
    public class QuestGenerator
    {
        private const int Attempts = 40;

        private readonly QuestPools pools;
        private readonly IQuestModule[] modules = { new ItemGoalModule(), new KillGoalModule(), new WaveGoalModule() };

        /// <summary>Creates a generator that draws from the given pools.</summary>
        public QuestGenerator(QuestPools pools)
        {
            this.pools = pools;
        }

        /// <summary>
        /// Makes a new quest that differs from every quest in <paramref name="existing"/>, or returns null
        /// when no different quest could be made.
        /// </summary>
        /// <param name="done">Quests completed so far; makes quests bigger and unlocks options.</param>
        /// <param name="id">Unique id for the new quest.</param>
        public QuestDefinition Generate(IList<QuestDefinition> existing, int done, string id)
        {
            HashSet<string> taken = new HashSet<string>();
            for (int i = 0; i < existing.Count; i++)
            {
                if (existing[i] != null) taken.Add(QuestSignature.Of(existing[i]));
            }

            for (int attempt = 0; attempt < Attempts; attempt++)
            {
                QuestDefinition quest = TryMake(done, id);
                if (quest == null) continue;

                if (!taken.Contains(QuestSignature.Of(quest))) return quest;
                Object.Destroy(quest);
            }
            return null;
        }

        private QuestDefinition TryMake(int done, string id)
        {
            QuestDraft draft = new QuestDraft
            {
                done = done,
                scale = Mathf.Min(pools.maxGrowth, 1f + done * pools.growthPerQuest)
            };

            int wanted = PickModuleCount();
            List<IQuestModule> order = new List<IQuestModule>(modules);
            for (int i = 0; i < wanted && order.Count > 0; i++)
            {
                int pick = Random.Range(0, order.Count);
                order[pick].Apply(draft, pools);
                order.RemoveAt(pick);
            }
            if (draft.IsEmpty) return null;

            QuestDefinition quest = ScriptableObject.CreateInstance<QuestDefinition>();
            quest.id = id;
            quest.title = draft.title;
            quest.requirements = draft.requirements.ToArray();
            quest.kills = draft.kills.ToArray();
            quest.waves = draft.waves.ToArray();
            quest.rewards = RollRewards(draft);
            return quest;
        }

        // One goal is the most common; three at once is rare.
        private static int PickModuleCount()
        {
            float roll = Random.value;
            if (roll < 0.5f) return 1;
            return roll < 0.85f ? 2 : 3;
        }

        private ItemAmount[] RollRewards(QuestDraft draft)
        {
            List<ItemAmount> rewards = new List<ItemAmount>();

            QuestPools.RewardOption special = PickReward(draft.done);
            if (special != null) rewards.Add(new ItemAmount { item = special.item, amount = Random.Range(special.min, special.max + 1) });

            int money = Mathf.Max(1, Mathf.RoundToInt(draft.points * pools.currencyPerPoint));
            if (pools.currency != null) rewards.Add(new ItemAmount { item = pools.currency, amount = money });
            return rewards.ToArray();
        }

        private QuestPools.RewardOption PickReward(int done)
        {
            float total = 0f;
            for (int i = 0; pools.rewards != null && i < pools.rewards.Length; i++)
            {
                if (IsEligible(pools.rewards[i], done)) total += pools.rewards[i].weight;
            }
            if (total <= 0f) return null;

            float roll = Random.value * total;
            for (int i = 0; i < pools.rewards.Length; i++)
            {
                if (!IsEligible(pools.rewards[i], done)) continue;

                roll -= pools.rewards[i].weight;
                if (roll <= 0f) return pools.rewards[i];
            }
            return null;
        }

        private static bool IsEligible(QuestPools.RewardOption option, int done)
        {
            return option.item != null && option.weight > 0f && option.minDone <= done;
        }
    }
}
