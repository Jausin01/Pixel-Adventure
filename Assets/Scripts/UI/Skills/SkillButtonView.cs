using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws one skill slot: the skill's icon, a sweeping cooldown overlay, and a dimmed icon while it cannot be cast.
    /// </summary>
    public class SkillButtonView : MonoBehaviour
    {
        public SkillCaster caster;
        public SkillLoadout loadout;
        public SkillCooldowns cooldowns;
        public int index;

        public Image icon;
        [Tooltip("Filled radial image that shrinks as the cooldown runs out.")]
        public Image cooldownOverlay;

        private void Update()
        {
            SkillDefinition skill = loadout.Get(caster.weapons.ActiveClass, index);
            icon.enabled = skill != null;
            if (skill == null)
            {
                cooldownOverlay.fillAmount = 0f;
                return;
            }

            icon.sprite = skill.icon;
            icon.color = caster.CanCast(index) ? Color.white : new Color(0.55f, 0.55f, 0.55f, 1f);
            cooldownOverlay.fillAmount = skill.cooldown > 0f ? cooldowns.Remaining(skill) / skill.cooldown : 0f;
        }
    }
}
