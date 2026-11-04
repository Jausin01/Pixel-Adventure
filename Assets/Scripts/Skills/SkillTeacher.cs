using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Sells skills: takes the price in the currency item from the inventory and teaches the skill.
    /// Does not display anything.
    /// </summary>
    [RequireComponent(typeof(Inventory), typeof(SkillUnlocks))]
    public class SkillTeacher : MonoBehaviour
    {
        [Tooltip("The item skills are paid with (gold ingots).")]
        public ItemDefinition currency;

        private Inventory inventory;
        private SkillUnlocks unlocks;

        /// <summary>True when the player can pay for the skill and has not learned it yet.</summary>
        public bool CanLearn(SkillDefinition skill)
        {
            return !Unlocks.IsUnlocked(skill) && Items.Count(currency) >= skill.goldCost;
        }

        /// <summary>Pays for and learns the skill. Returns false (and changes nothing) when it cannot.</summary>
        public bool TryLearn(SkillDefinition skill)
        {
            bool done = TryLearnCore(skill);
            if (done) Sfx.Play("skill_learn"); else Sfx.Play("ui_error"); 
            return done;
        }

        private bool TryLearnCore(SkillDefinition skill)
        {
            if (!CanLearn(skill)) return false;

            if (skill.goldCost > 0) Items.Remove(currency, skill.goldCost);
            return Unlocks.Unlock(skill);
        }

        // Resolved on first use: other components may ask before this object's Awake has run.
        private Inventory Items
        {
            get
            {
                if (inventory == null) inventory = GetComponent<Inventory>();
                return inventory;
            }
        }

        private SkillUnlocks Unlocks
        {
            get
            {
                if (unlocks == null) unlocks = GetComponent<SkillUnlocks>();
                return unlocks;
            }
        }
    }
}
