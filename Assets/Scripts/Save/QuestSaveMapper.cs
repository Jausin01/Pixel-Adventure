using System.Collections.Generic;

namespace MobileRpg
{
    /// <summary>
    /// Converts generated quests to and from their save entries. Items and arenas are stored by id.
    /// </summary>
    public static class QuestSaveMapper
    {
        /// <summary>Describes a quest as plain data.</summary>
        public static SaveData.QuestEntry ToEntry(QuestDefinition quest)
        {
            SaveData.QuestEntry entry = new SaveData.QuestEntry { id = quest.id, title = quest.title };

            for (int i = 0; i < quest.requirements.Length; i++) entry.requirements.Add(ToAmount(quest.requirements[i]));
            for (int i = 0; i < quest.rewards.Length; i++) entry.rewards.Add(ToAmount(quest.rewards[i]));
            for (int i = 0; i < quest.kills.Length; i++)
            {
                QuestDefinition.KillGoal goal = quest.kills[i];
                entry.kills.Add(new SaveData.QuestKillGoal
                {
                    enemy = goal.enemy, label = goal.label, count = goal.count,
                    arenaId = goal.arena != null ? goal.arena.id : string.Empty
                });
            }
            for (int i = 0; i < quest.waves.Length; i++)
            {
                entry.waves.Add(new SaveData.QuestWaveGoal { arenaId = quest.waves[i].arena.id, wave = quest.waves[i].wave });
            }
            return entry;
        }

        /// <summary>
        /// Rebuilds a quest. Returns null when an item or arena it refers to no longer exists,
        /// so the board generates a replacement.
        /// </summary>
        public static QuestDefinition FromEntry(SaveData.QuestEntry entry, GameDatabase database)
        {
            List<ItemAmount> requirements = new List<ItemAmount>();
            List<ItemAmount> rewards = new List<ItemAmount>();
            if (!ReadAmounts(entry.requirements, database, requirements) || !ReadAmounts(entry.rewards, database, rewards)) return null;

            List<QuestDefinition.KillGoal> kills = new List<QuestDefinition.KillGoal>();
            for (int i = 0; i < entry.kills.Count; i++)
            {
                SaveData.QuestKillGoal goal = entry.kills[i];
                ArenaDefinition arena = string.IsNullOrEmpty(goal.arenaId) ? null : database.FindArena(goal.arenaId);
                if (!string.IsNullOrEmpty(goal.arenaId) && arena == null) return null;

                kills.Add(new QuestDefinition.KillGoal { enemy = goal.enemy, label = goal.label, count = goal.count, arena = arena });
            }

            List<QuestDefinition.WaveGoal> waves = new List<QuestDefinition.WaveGoal>();
            for (int i = 0; i < entry.waves.Count; i++)
            {
                ArenaDefinition arena = database.FindArena(entry.waves[i].arenaId);
                if (arena == null) return null;

                waves.Add(new QuestDefinition.WaveGoal { arena = arena, wave = entry.waves[i].wave });
            }

            QuestDefinition quest = UnityEngine.ScriptableObject.CreateInstance<QuestDefinition>();
            quest.id = entry.id;
            quest.title = entry.title;
            quest.requirements = requirements.ToArray();
            quest.rewards = rewards.ToArray();
            quest.kills = kills.ToArray();
            quest.waves = waves.ToArray();
            return quest;
        }

        private static SaveData.QuestAmount ToAmount(ItemAmount amount)
        {
            return new SaveData.QuestAmount { itemId = amount.item.id, amount = amount.amount };
        }

        private static bool ReadAmounts(List<SaveData.QuestAmount> source, GameDatabase database, List<ItemAmount> target)
        {
            for (int i = 0; i < source.Count; i++)
            {
                ItemDefinition item = database.FindItem(source[i].itemId);
                if (item == null) return false;

                target.Add(new ItemAmount { item = item, amount = source[i].amount });
            }
            return true;
        }
    }
}
