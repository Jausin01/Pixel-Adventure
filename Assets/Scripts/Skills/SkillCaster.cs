using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Casts the skills placed in the skill slots for the weapon in hand: checks that the weapon is free, that the skill
    /// is ready and that there is enough energy, then asks the matching <see cref="ISkillExecutor"/> on the weapon to
    /// perform it.
    /// </summary>
    public class SkillCaster : MonoBehaviour
    {
        public WeaponHolder weapons;
        public SkillLoadout loadout;
        public SkillCooldowns cooldowns;
        public CharacterStats stats;
        public Energy energy;
        public Health health;

        /// <summary>True when the skill in the slot can be cast right now.</summary>
        public bool CanCast(int index)
        {
            SkillDefinition skill = loadout.Get(weapons.ActiveClass, index);
            IWeapon weapon = weapons.Active.GetComponent<IWeapon>();
            return skill != null
                && weapon != null
                && health.IsAlive
                && !weapon.IsBusy
                && cooldowns.Remaining(skill) <= 0f
                && energy.Current >= skill.energyCost
                && FindExecutor(skill.type) != null;
        }

        /// <summary>Casts the skill in a slot. Returns false (and spends nothing) when it cannot be cast.</summary>
        public bool TryCast(int index)
        {
            if (!CanCast(index)) return false;

            WeaponClass weaponClass = weapons.ActiveClass;
            SkillDefinition skill = loadout.Get(weaponClass, index);
            if (!energy.TrySpend(skill.energyCost)) return false;

            cooldowns.Begin(skill);
            Sfx.Play(SkillSound(skill.type));
            FindExecutor(skill.type).Execute(skill, stats.AttackFor(weaponClass) * skill.damageMultiplier);
            return true;
        }

        private static string SkillSound(SkillType type)
        {
            switch (type)
            {
                case SkillType.SpinSlash: case SkillType.HatchetVolley: return "axe_spin";
                case SkillType.PiercingThrust: case SkillType.TripleThrust: return "spear_swing";
                case SkillType.SwordThrow: case SkillType.Javelin: return "boomerang";
                case SkillType.GroundSlam: case SkillType.SkullSplitter: case SkillType.TitanSmash: return "stomp";
                case SkillType.SpearRain: return "rain_fall";
                case SkillType.Multishot: case SkillType.PiercingShot: return "bow_shoot";
                case SkillType.FanOfKnives: case SkillType.ShadowFlurry: return "dagger_slash";
                case SkillType.SeismicSlam: return "shockwave";
                case SkillType.Fireball: return "fireball_cast";
                case SkillType.ChainLightning: return "thunder";
                case SkillType.MeteorShower: return "meteor_fall";
                default: return null;
            }
        }

        private ISkillExecutor FindExecutor(SkillType type)
        {
            ISkillExecutor[] executors = weapons.Active.GetComponents<ISkillExecutor>();
            for (int i = 0; i < executors.Length; i++)
            {
                if (executors[i].Type == type) return executors[i];
            }
            return null;
        }
    }
}
