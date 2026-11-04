using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that casts the skill placed in one skill slot.
    /// </summary>
    public class SkillButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override string ClickSound { get { return null; } }

        public SkillCaster caster;

        [Tooltip("Which skill slot this button casts (0 to 3).")]
        public int index;

        /// <inheritdoc/>
        protected override void Execute()
        {
            caster.TryCast(index);
        }
    }
}
