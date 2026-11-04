using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The skills placed in the four skill slots, kept separately for each weapon class.
    /// Only learned skills of the matching class can be placed.
    /// </summary>
    [RequireComponent(typeof(SkillUnlocks))]
    public class SkillLoadout : MonoBehaviour
    {
        /// <summary>Number of skill slots.</summary>
        public const int SlotCount = 4;

        private readonly Dictionary<WeaponClass, SkillDefinition[]> slots = new Dictionary<WeaponClass, SkillDefinition[]>();
        private SkillUnlocks unlocks;

        /// <summary>Raised when any slot changes.</summary>
        public event Action Changed;

        private void Awake()
        {
            unlocks = GetComponent<SkillUnlocks>();
        }

        /// <summary>The skill in a slot, or null.</summary>
        public SkillDefinition Get(WeaponClass weaponClass, int index)
        {
            SkillDefinition[] row;
            return index >= 0 && index < SlotCount && slots.TryGetValue(weaponClass, out row) ? row[index] : null;
        }

        /// <summary>
        /// Places a learned skill in a slot. A skill sits in one slot at a time. Returns false when the skill is not
        /// learned, belongs to another weapon class or the index is out of range.
        /// </summary>
        public bool Equip(WeaponClass weaponClass, int index, SkillDefinition skill)
        {
            if (index < 0 || index >= SlotCount || skill == null || skill.weaponClass != weaponClass || !unlocks.IsUnlocked(skill)) return false;

            SkillDefinition[] row = GetRow(weaponClass);
            for (int i = 0; i < SlotCount; i++)
            {
                if (row[i] == skill) row[i] = null;
            }

            row[index] = skill;
            RaiseChanged();
            return true;
        }

        /// <summary>The slot (0 to 3) holding the skill, or -1 when it is not placed.</summary>
        public int SlotOf(SkillDefinition skill)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (Get(skill.weaponClass, i) == skill) return i;
            }
            return -1;
        }

        /// <summary>
        /// Removes the skill from its slot, or places it in the first free slot.
        /// Returns false when it could not be placed (not learned, or every slot is taken).
        /// </summary>
        public bool Toggle(SkillDefinition skill)
        {
            int current = SlotOf(skill);
            if (current >= 0)
            {
                Clear(skill.weaponClass, current);
                return true;
            }

            for (int i = 0; i < SlotCount; i++)
            {
                if (Get(skill.weaponClass, i) == null) return Equip(skill.weaponClass, i, skill);
            }
            return false;
        }

        /// <summary>Empties a slot.</summary>
        public void Clear(WeaponClass weaponClass, int index)
        {
            if (index < 0 || index >= SlotCount || Get(weaponClass, index) == null) return;

            GetRow(weaponClass)[index] = null;
            RaiseChanged();
        }

        private SkillDefinition[] GetRow(WeaponClass weaponClass)
        {
            SkillDefinition[] row;
            if (!slots.TryGetValue(weaponClass, out row))
            {
                row = new SkillDefinition[SlotCount];
                slots[weaponClass] = row;
            }
            return row;
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
