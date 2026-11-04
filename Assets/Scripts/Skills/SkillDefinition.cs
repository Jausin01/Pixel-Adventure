using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Data describing one skill (Spin Slash...). Skills belong to a weapon class and cost energy and cooldown.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Skill", fileName = "NewSkill")]
    public class SkillDefinition : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique identifier, used for saving.")]
        public string id;
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;

        [Header("Rules")]
        [Tooltip("Weapons that can use this skill.")]
        public WeaponClass weaponClass = WeaponClass.Sword;

        [Tooltip("Which behavior runs when the skill is cast.")]
        public SkillType type;

        [Header("Teaching")]
        [Tooltip("Gold ingots the teacher charges to teach this skill. 0 for skills known from the start.")]
        public int goldCost;

        [Header("Balance")]
        [Tooltip("Energy spent per cast.")]
        public float energyCost = 30f;

        [Tooltip("Seconds before the skill can be cast again.")]
        public float cooldown = 5f;

        [Tooltip("Damage per hit as a multiple of the weapon attack.")]
        public float damageMultiplier = 1.5f;
    }
}
