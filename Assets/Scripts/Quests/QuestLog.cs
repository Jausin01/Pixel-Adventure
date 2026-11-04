using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Tracks kill progress for the quests on the board and performs the hand-in.
    /// Does not display or save anything itself.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class QuestLog : MonoBehaviour
    {
        public QuestBoard board;

        private readonly Dictionary<string, int> kills = new Dictionary<string, int>();
        private Inventory inventory;

        /// <summary>Raised when a quest's kill progress changes or a quest is handed in.</summary>
        public event Action<QuestDefinition> Changed;

        // Resolved on first use: other components may ask before this object's Awake has run.
        private Inventory Items
        {
            get
            {
                if (inventory == null) inventory = GetComponent<Inventory>();
                return inventory;
            }
        }

        private void OnEnable() { EnemyIdentity.Killed += OnEnemyKilled; }

        private void OnDisable() { EnemyIdentity.Killed -= OnEnemyKilled; }

        /// <summary>Kill progress as (key, count) pairs, for saving.</summary>
        public IEnumerable<KeyValuePair<string, int>> KillProgress { get { return kills; } }

        /// <summary>Sets a saved kill count (used when loading a save).</summary>
        public void RestoreKills(string key, int count)
        {
            kills[key] = count;
        }

        /// <summary>How many enemies of the goal were defeated so far, capped at the goal's count.</summary>
        public int Kills(QuestDefinition quest, int goalIndex)
        {
            int count;
            kills.TryGetValue(Key(quest, goalIndex), out count);
            return Mathf.Min(count, quest.kills[goalIndex].count);
        }

        /// <summary>The best wave cleared in the goal's arena, capped at the goal's wave.</summary>
        public int BestWave(QuestDefinition.WaveGoal goal)
        {
            return Mathf.Min(ArenaRecords.Best(goal.arena.id), goal.wave);
        }

        /// <summary>True when every kill and wave goal of the quest is met.</summary>
        public bool GoalsMet(QuestDefinition quest)
        {
            for (int i = 0; i < quest.kills.Length; i++)
            {
                if (Kills(quest, i) < quest.kills[i].count) return false;
            }
            for (int i = 0; i < quest.waves.Length; i++)
            {
                if (BestWave(quest.waves[i]) < quest.waves[i].wave) return false;
            }
            return true;
        }

        /// <summary>True when the player carries every item and has met every goal the quest asks for.</summary>
        public bool HasRequirements(QuestDefinition quest)
        {
            if (!GoalsMet(quest)) return false;

            for (int i = 0; i < quest.requirements.Length; i++)
            {
                if (Items.Count(quest.requirements[i].item) < quest.requirements[i].amount) return false;
            }
            return true;
        }

        /// <summary>
        /// Takes the required items, gives the rewards and lets the board replace the quest. Returns false (and changes
        /// nothing) when the quest is not on the board, something is missing or the rewards do not fit in the inventory.
        /// </summary>
        public bool TryTurnIn(QuestDefinition quest)
        {
            bool done = TryTurnInCore(quest);
            if (done) Sfx.Play("quest_complete"); 
            return done;
        }

        private bool TryTurnInCore(QuestDefinition quest)
        {
            if (board.IndexOf(quest) < 0 || !HasRequirements(quest)) return false;

            Take(quest.requirements);
            if (!GiveAll(quest.rewards))
            {
                Give(quest.requirements);
                return false;
            }

            ClearProgress(quest);
            board.Complete(quest);
            if (Changed != null) Changed(quest);
            return true;
        }

        private void OnEnemyKilled(EnemyIdentity enemy)
        {
            QuestDefinition[] quests = board.Slots;
            for (int q = 0; q < quests.Length; q++)
            {
                QuestDefinition quest = quests[q];
                if (quest == null) continue;

                bool changed = false;
                for (int i = 0; i < quest.kills.Length; i++)
                {
                    QuestDefinition.KillGoal goal = quest.kills[i];
                    if (!Matches(goal, enemy) || Kills(quest, i) >= goal.count) continue;

                    string key = Key(quest, i);
                    int count;
                    kills.TryGetValue(key, out count);
                    kills[key] = count + 1;
                    changed = true;
                }
                if (changed && Changed != null) Changed(quest);
            }
        }

        private static bool Matches(QuestDefinition.KillGoal goal, EnemyIdentity enemy)
        {
            bool kind = string.IsNullOrEmpty(goal.enemy) || goal.enemy == enemy.id || goal.enemy == enemy.family;
            bool place = goal.arena == null || goal.arena.id == enemy.arenaId;
            return kind && place;
        }

        private void ClearProgress(QuestDefinition quest)
        {
            for (int i = 0; i < quest.kills.Length; i++) kills.Remove(Key(quest, i));
        }

        /// <summary>Save key of one kill goal.</summary>
        public static string Key(QuestDefinition quest, int goalIndex)
        {
            return quest.id + "#" + goalIndex;
        }

        private void Take(ItemAmount[] items)
        {
            for (int i = 0; i < items.Length; i++) Items.Remove(items[i].item, items[i].amount);
        }

        private void Give(ItemAmount[] items)
        {
            for (int i = 0; i < items.Length; i++) Items.Add(items[i].item, items[i].amount);
        }

        private bool GiveAll(ItemAmount[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (Items.Add(items[i].item, items[i].amount)) continue;

                for (int j = 0; j < i; j++) Items.Remove(items[j].item, items[j].amount); // undo the ones already given
                return false;
            }
            return true;
        }
    }
}
