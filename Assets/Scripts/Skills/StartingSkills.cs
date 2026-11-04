using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Teaches the skills known from the start of a new game and places each in the first free slot of its weapon class.
    /// The other skills are bought from the skill teacher.
    /// </summary>
    [RequireComponent(typeof(SkillUnlocks), typeof(SkillLoadout))]
    public class StartingSkills : MonoBehaviour
    {
        public SkillDefinition[] skills;

        private void Start()
        {
            if (SaveFile.Exists) return; // a saved game decides what the player knows

            SkillUnlocks unlocks = GetComponent<SkillUnlocks>();
            SkillLoadout loadout = GetComponent<SkillLoadout>();

            // Every weapon class has its own slots, so each class counts its own.
            int[] nextSlot = new int[System.Enum.GetValues(typeof(WeaponClass)).Length];
            for (int i = 0; i < skills.Length; i++)
            {
                unlocks.Unlock(skills[i]);
                int weaponClass = (int)skills[i].weaponClass;
                if (nextSlot[weaponClass] < SkillLoadout.SlotCount && loadout.Equip(skills[i].weaponClass, nextSlot[weaponClass], skills[i]))
                {
                    nextSlot[weaponClass]++;
                }
            }
        }
    }
}
