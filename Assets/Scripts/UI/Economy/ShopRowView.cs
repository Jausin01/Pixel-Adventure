using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows one line of the merchant's screen: how many of the item the player carries and the price
    /// (green when it can be paid, or when there is something to sell). Refreshes when the inventory changes.
    /// </summary>
    public class ShopRowView : MonoBehaviour
    {
        public ItemDefinition item;
        public Merchant merchant;
        public Inventory inventory;
        public Text ownedText;
        public Text priceText;
        public bool selling;
        public int buyPrice;

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            Refresh();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        private void OnInventoryChanged(ItemDefinition changed, int count) { Refresh(); }

        private void Refresh()
        {
            int owned = inventory.Count(item);
            ownedText.text = "Owned: " + owned;

            if (selling)
            {
                string color = owned > 0 ? "#9be58a" : "#8a8a8a";
                priceText.text = "<color=" + color + ">Sell +" + item.sellValue + "</color>";
            }
            else
            {
                string color = merchant.CanAfford(buyPrice) ? "#9be58a" : "#ff8a7a";
                priceText.text = "<color=" + color + ">Buy " + buyPrice + "</color>";
            }
        }
    }
}
