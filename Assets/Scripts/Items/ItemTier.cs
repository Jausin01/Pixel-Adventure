using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A rarity tier for items (for example a card suit). Defines the tier's name, color and ranking.
    /// Create assets from the Assets/Create menu to add or rename tiers.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Item Tier", fileName = "NewTier")]
    public class ItemTier : ScriptableObject
    {
        [Tooltip("Name shown to the player.")]
        public string displayName;

        [Tooltip("Color used for the tier's name and slot tint.")]
        public Color color = Color.white;

        [Tooltip("Ranking, lowest first. Used for sorting.")]
        public int order;

        [Header("Smithing")]
        [Tooltip("Materials the blacksmith needs for one upgrade step of gear in this tier (multiplied by the step).")]
        public ItemAmount[] smithMaterials;

        [Tooltip("Money the blacksmith needs for one upgrade step (multiplied by the step).")]
        public int smithShards;
    }
}
