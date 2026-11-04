using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The consumable the player picked for the quick button (a potion, a fruit...). Remembers the choice and uses
    /// the item through <see cref="ItemUser"/>. While nothing is picked, the first consumable the player gets is used.
    /// </summary>
    [RequireComponent(typeof(Inventory), typeof(ItemUser))]
    public class QuickItem : MonoBehaviour
    {
        private Inventory inventory;
        private ItemUser itemUser;

        /// <summary>The picked item, or null.</summary>
        public ItemDefinition Item { get; private set; }

        /// <summary>How many of the picked item the player carries.</summary>
        public int Count { get { return Item != null ? inventory.Count(Item) : 0; } }

        /// <summary>True when the picked item can be used right now.</summary>
        public bool CanUse { get { return itemUser.CanUse(Item); } }

        /// <summary>Raised when the picked item changes.</summary>
        public event Action Changed;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            itemUser = GetComponent<ItemUser>();
        }

        private void OnEnable()
        {
            if (inventory == null) inventory = GetComponent<Inventory>();
            inventory.Changed += OnInventoryChanged;
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        /// <summary>Picks an item for the quick button. Only consumables can be picked.</summary>
        public void Select(ItemDefinition item)
        {
            if (item == null || item.category != ItemCategory.Consumable || item == Item) return;

            Item = item;
            if (Changed != null) Changed();
        }

        /// <summary>Uses one of the picked item. Returns false when it cannot be used.</summary>
        public bool Use()
        {
            return itemUser.Use(Item);
        }

        private void OnInventoryChanged(ItemDefinition item, int total)
        {
            if (Item == null && total > 0) Select(item);
        }
    }
}
