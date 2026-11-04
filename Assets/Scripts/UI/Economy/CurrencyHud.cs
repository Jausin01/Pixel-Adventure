using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows how much money the player carries.
    /// </summary>
    public class CurrencyHud : MonoBehaviour
    {
        public Inventory inventory;
        public ItemDefinition currency;
        public Text amountText;

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            Refresh();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        private void OnInventoryChanged(ItemDefinition item, int count)
        {
            if (item == currency) Refresh();
        }

        private void Refresh()
        {
            amountText.text = inventory.Count(currency).ToString();
        }
    }
}
