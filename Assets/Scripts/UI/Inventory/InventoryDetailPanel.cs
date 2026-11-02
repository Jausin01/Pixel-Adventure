using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the details of the selected item: large icon, name, tier, category, description and effect.
    /// </summary>
    public class InventoryDetailPanel : MonoBehaviour
    {
        public InventoryBrowser browser;
        public ItemUser itemUser;

        [Tooltip("Everything that is shown when an item is selected.")]
        public GameObject content;

        [Tooltip("Hint shown when nothing is selected.")]
        public GameObject emptyHint;

        public Image icon;
        public Text nameText;
        public Text tierText;
        public Text infoText;
        public Text descriptionText;
        public Text effectText;

        [Tooltip("The Use button. Only shown for items that can be used.")]
        public GameObject useButton;

        private void OnEnable()
        {
            browser.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            browser.Changed -= Refresh;
        }

        private void Refresh()
        {
            InventorySlot slot = browser.Selected;
            content.SetActive(slot != null);
            emptyHint.SetActive(slot == null);
            if (slot == null) return;

            ItemDefinition item = slot.Item;
            icon.sprite = item.icon;
            nameText.text = item.displayName;

            tierText.text = item.tier != null ? item.tier.displayName : string.Empty;
            tierText.color = item.tier != null ? item.tier.color : Color.white;

            string info = ItemDescriber.CategoryName(item.category);
            if (item.IsStackable) info += "   x" + slot.Count;
            infoText.text = info;

            descriptionText.text = item.description;
            effectText.text = ItemDescriber.Effects(item);
            useButton.SetActive(itemUser.CanUse(item));
        }
    }
}
