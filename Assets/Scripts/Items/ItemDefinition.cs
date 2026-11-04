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

        [Header("Trade")]
        [Tooltip("What the merchant pays for one of this item, in the currency. 0 means it cannot be sold.")]
        public int sellValue;

        [Tooltip("When several are dropped at once, spawn one pickup holding all of them (for the currency).")]
        public bool dropAsSingleStack;

        [Header("Consumable effect")]
        [Tooltip("Health restored when used.")]
        public float healthRestored;

        [Tooltip("Energy restored when used.")]
        public float energyRestored;

        [Header("Equipment")]
        [Tooltip("Which equipment slot can wear this item. None for items that cannot be worn.")]
        public EquipmentKind equipKind = EquipmentKind.None;

        [Tooltip("Attack added while worn.")]
        public float attackBonus;

        [Tooltip("Defense added while worn.")]
        public float defenseBonus;

        [Tooltip("Maximum health added while worn.")]
        public float maxHealthBonus;

        [Tooltip("The natures a piece of this item can be born with. Leave empty for items that are not gear.")]
        public GearNatureTable natures;

        [Tooltip("Weapons with several looks (swords, axes, spears): index of the skin shown while equipped.")]
        public int swordSkinIndex;

        /// <summary>True when several of this item share one inventory slot.</summary>
        public bool IsStackable { get { return category != ItemCategory.Equipment && category != ItemCategory.Weapon; } }
    }
}
