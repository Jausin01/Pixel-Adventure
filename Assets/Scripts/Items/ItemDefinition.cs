using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Data describing one kind of item (wood, apple, Good Sword...). Create assets from the Assets/Create menu.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Item", fileName = "NewItem")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique identifier, used for saving.")]
        public string id;

        [Tooltip("Name shown to the player.")]
        public string displayName;

        [TextArea]
        [Tooltip("Text shown in the item details.")]
        public string description;

        [Tooltip("Icon used for the item in the world and in menus.")]
        public Sprite icon;

        [Header("Classification")]
        [Tooltip("Kind of item. Weapons and equipment never stack.")]
        public ItemCategory category = ItemCategory.Material;

        [Tooltip("Rarity tier. Optional.")]
        public ItemTier tier;

        [Tooltip("The most of this item that can be carried in total (stackable items only).")]
        public int maxStack = 100;

        [Header("Consumable effect")]
        [Tooltip("Health restored when used.")]
        public float healthRestored;

        [Tooltip("Energy restored when used.")]
        public float energyRestored;

        /// <summary>True when several of this item share one inventory slot.</summary>
        public bool IsStackable { get { return category != ItemCategory.Equipment && category != ItemCategory.Weapon; } }
    }
}
