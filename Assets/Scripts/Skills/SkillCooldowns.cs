using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Tracks the cooldown of every skill, by skill (so moving a skill between slots does not reset it).
    /// </summary>
    public class SkillCooldowns : MonoBehaviour
    {
        private readonly Dictionary<SkillDefinition, float> readyAt = new Dictionary<SkillDefinition, float>();

        /// <summary>Seconds left before the skill can be cast again (0 when ready).</summary>
        public float Remaining(SkillDefinition skill)
        {
            float time;
            return readyAt.TryGetValue(skill, out time) ? Mathf.Max(0f, time - Time.time) : 0f;
        }

        /// <summary>Starts the skill's cooldown.</summary>
        public void Begin(SkillDefinition skill)
        {
            readyAt[skill] = Time.time + skill.cooldown;
        }
    }
}
