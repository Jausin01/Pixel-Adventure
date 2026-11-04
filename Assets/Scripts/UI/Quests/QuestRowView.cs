using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the quest in one slot of the board: what to hand in, the goals (green once met), the reward and its state.
    /// Refreshes whenever the inventory, the quest log or the board changes.
    /// </summary>
    public class QuestRowView : MonoBehaviour
    {
        public QuestBoard board;
        public int slot;
        public QuestLog log;
        public Inventory inventory;
        public Text titleText;
        public Text requirementsText;
        public Text goalsText;
        public Text rewardText;
        public Image rewardIcon;
        public Graphic background;

        private static readonly Color Normal = new Color(0.22f, 0.16f, 0.13f, 1f);
        private static readonly Color Ready = new Color(0.2f, 0.32f, 0.16f, 1f);
        private static readonly Color Empty = new Color(0.12f, 0.1f, 0.09f, 1f);

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            log.Changed += OnQuestChanged;
            board.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
            log.Changed -= OnQuestChanged;
            board.Changed -= Refresh;
        }

        private void OnInventoryChanged(ItemDefinition item, int count) { Refresh(); }

        private void OnQuestChanged(QuestDefinition changed) { Refresh(); }

        private void Refresh()
        {
            QuestDefinition quest = board.Slots[slot];
            rewardIcon.enabled = quest != null;
            if (quest == null)
            {
                titleText.text = requirementsText.text = goalsText.text = rewardText.text = string.Empty;
                background.color = Empty;
                return;
            }

            bool ready = log.HasRequirements(quest);
            titleText.text = ready ? quest.title + "  <color=#ffe680>!</color>" : quest.title;
            requirementsText.text = RequirementsLine(quest);
            goalsText.text = GoalsLine(quest);
            background.color = ready ? Ready : Normal;

            ItemAmount main = quest.rewards[0];
            rewardIcon.sprite = main.item.icon;
            rewardText.text = RewardLine(quest);
        }

        // The main reward is shown by its icon, so the text gives its amount and the money on top of it.
        private string RewardLine(QuestDefinition quest)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < quest.rewards.Length; i++)
            {
                ItemAmount reward = quest.rewards[i];
                if (text.Length > 0) text.Append("\n");
                text.Append(reward.item == quest.rewards[0].item && i == 0 ? "x" : "+").Append(reward.amount);
            }
            return text.ToString();
        }

        private string GoalsLine(QuestDefinition quest)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < quest.kills.Length; i++)
            {
                QuestDefinition.KillGoal goal = quest.kills[i];
                int have = log.Kills(quest, i);
                string place = goal.arena != null ? " (" + goal.arena.displayName + ")" : string.Empty;
                Append(text, "Kill " + goal.label + place + " " + have + "/" + goal.count, have >= goal.count, "#ffd27a");
            }

            for (int i = 0; i < quest.waves.Length; i++)
            {
                QuestDefinition.WaveGoal goal = quest.waves[i];
                int have = log.BestWave(goal);
                Append(text, goal.arena.displayName + " wave " + have + "/" + goal.wave, have >= goal.wave, "#ffd27a");
            }
            return text.ToString();
        }

        private string RequirementsLine(QuestDefinition quest)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < quest.requirements.Length; i++)
            {
                ItemAmount need = quest.requirements[i];
                int have = Mathf.Min(inventory.Count(need.item), need.amount);
                Append(text, need.item.displayName + " " + have + "/" + need.amount, have >= need.amount, "#ffffff");
            }
            return text.ToString();
        }

        private static void Append(StringBuilder text, string line, bool met, string unmetColor)
        {
            if (text.Length > 0) text.Append("   ");
            text.Append("<color=").Append(met ? "#9be58a" : unmetColor).Append(">").Append(line).Append("</color>");
        }
    }
}
