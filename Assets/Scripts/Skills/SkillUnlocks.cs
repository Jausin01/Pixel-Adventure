using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The skills the character has learned.
    /// </summary>
    public class SkillUnlocks : MonoBehaviour
    {
        private readonly HashSet<SkillDefinition> unlocked = new HashSet<SkillDefinition>();

        /// <summary>Raised when a skill is learned.</summary>
        public event Action Changed;

        /// <summary>Every learned skill.</summary>
        public IEnumerable<SkillDefinition> Unlocked { get { return unlocked; } }

        /// <summary>True when the skill has been learned.</summary>
        public bool IsUnlocked(SkillDefinition skill)
        {
            return skill != null && unlocked.Contains(skill);
        }

        /// <summary>Learns a skill. Returns false when it was already known.</summary>
        public bool Unlock(SkillDefinition skill)
        {
            if (skill == null || !unlocked.Add(skill)) return false;

            if (Changed != null) Changed();
            return true;
        }
    }
}
