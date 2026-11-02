using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// One possible drop: an item, a quantity range and the chance to drop at all.
    /// </summary>
    [System.Serializable]
    public class DropEntry
    {
        public ItemDefinition item;

        [Tooltip("Minimum amount dropped when the roll succeeds.")]
        public int minAmount = 1;

        [Tooltip("Maximum amount dropped when the roll succeeds (inclusive).")]
        public int maxAmount = 1;

        [Tooltip("Chance (0 to 1) that this entry drops.")]
        [Range(0f, 1f)]
        public float chance = 1f;
    }
}
