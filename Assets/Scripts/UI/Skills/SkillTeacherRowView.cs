using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows one skill on the teacher's screen: its price, or whether it is learned and which slot it sits in.
    /// Refreshes whenever the inventory, the learned skills or the slots change.
    /// </summary>
    public class SkillTeacherRowView : MonoBehaviour
    {
        public SkillDefinition skill;
        public SkillTeacher teacher;
        public Inventory inventory;
        public SkillUnlocks unlocks;
        public SkillLoadout loadout;
        public Text statusText;
        public Graphic background;

        private static readonly Color Normal = new Color(0.22f, 0.16f, 0.13f, 1f);
        private static readonly Color Equipped = new Color(0.2f, 0.32f, 0.16f, 1f);

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            unlocks.Changed += Refresh;
            loadout.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
            unlocks.Changed -= Refresh;
            loadout.Changed -= Refresh;
        }

        private void OnInventoryChanged(ItemDefinition item, int count) { Refresh(); }

        private void Refresh()
        {
            background.color = Normal;

            if (!unlocks.IsUnlocked(skill))
            {
                string color = teacher.CanLearn(skill) ? "#9be58a" : "#ff8a7a";
                statusText.text = "<color=" + color + ">Learn: " + skill.goldCost + " " + teacher.currency.displayName + "</color>";
                return;
            }

            int slot = loadout.SlotOf(skill);
            if (slot < 0)
            {
                statusText.text = "Tap to equip";
                return;
            }

            statusText.text = "Equipped (slot " + (slot + 1) + ")";
            background.color = Equipped;
        }
    }
}
