using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A "nature" a piece of gear is born with (Sharp, Brittle...). Multiplies the piece's attack, defense and health bonuses,
    /// for better or for worse. A multiplier of 1 leaves the stat unchanged.
    /// </summary>
    [Serializable]
    public class GearNature
    {
        [Tooltip("Unique identifier, used for saving.")]
        public string id;

        [Tooltip("Name shown before the item name. Natures that change nothing show no prefix.")]
        public string displayName;

        public float attack = 1f;
        public float defense = 1f;
        public float maxHealth = 1f;

        [Tooltip("Relative chance of being rolled.")]
        public float weight = 1f;

        /// <summary>True when the nature changes nothing.</summary>
        public bool IsNeutral { get { return attack == 1f && defense == 1f && maxHealth == 1f; } }

        /// <summary>The word put before the item name, or an empty string for a neutral nature.</summary>
        public string Prefix { get { return IsNeutral ? string.Empty : displayName; } }

        /// <summary>True when the nature changes at least one stat the item actually has.</summary>
        public bool FitsItem(ItemDefinition item)
        {
            if (IsNeutral) return true;
            return (attack != 1f && item.attackBonus > 0f) || (defense != 1f && item.defenseBonus > 0f) || (maxHealth != 1f && item.maxHealthBonus > 0f);
        }

        /// <summary>The multipliers as text, for example "+15% ATK  -10% HP".</summary>
        public string Describe()
        {
            List<string> parts = new List<string>();
            AddPart(parts, "ATK", attack);
            AddPart(parts, "DEF", defense);
            AddPart(parts, "HP", maxHealth);
            return parts.Count > 0 ? string.Join("  ", parts.ToArray()) : "no effect";
        }

        private static void AddPart(List<string> parts, string label, float multiplier)
        {
            if (multiplier == 1f) return;

            int percent = Mathf.RoundToInt((multiplier - 1f) * 100f);
            parts.Add((percent > 0 ? "+" : string.Empty) + percent + "% " + label);
        }
    }
}
