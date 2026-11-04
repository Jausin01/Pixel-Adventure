using System;
using System.Collections.Generic;

namespace MobileRpg
{
    /// <summary>
    /// Everything stored in the save file. Plain data; items, skills and quests are stored by id.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public List<ItemEntry> inventory = new List<ItemEntry>();
        public List<ItemEntry> stash = new List<ItemEntry>();
        public List<WornEntry> equipment = new List<WornEntry>();
        public List<string> unlockedSkills = new List<string>();
        public List<LoadoutEntry> loadout = new List<LoadoutEntry>();
        public List<QuestEntry> quests = new List<QuestEntry>();
        public List<KillEntry> questKills = new List<KillEntry>();
        public string quickItem;
        public int questsDone;
        public int questSerial;

        [Serializable]
        public class ItemEntry
        {
            public string id;
            public int count;

            // Gear only: the individual state of the piece.
            public int level;
            public int reforges;
            public string nature;
        }

        [Serializable]
        public class WornEntry
        {
            public int slot;
            public string id;
            public int level;
            public int reforges;
            public string nature;
        }

        /// <summary>One generated quest on the board.</summary>
        [Serializable]
        public class QuestEntry
        {
            public string id;
            public string title;
            public List<QuestAmount> requirements = new List<QuestAmount>();
            public List<QuestKillGoal> kills = new List<QuestKillGoal>();
            public List<QuestWaveGoal> waves = new List<QuestWaveGoal>();
            public List<QuestAmount> rewards = new List<QuestAmount>();
        }

        [Serializable]
        public class QuestAmount
        {
            public string itemId;
            public int amount;
        }

        [Serializable]
        public class QuestKillGoal
        {
            public string enemy;
            public string label;
            public string arenaId;
            public int count;
        }

        [Serializable]
        public class QuestWaveGoal
        {
            public string arenaId;
            public int wave;
        }

        [Serializable]
        public class KillEntry
        {
            public string key;
            public int count;
        }

        [Serializable]
        public class LoadoutEntry
        {
            public int weaponClass;
            public int index;
            public string skillId;
        }
    }
}
