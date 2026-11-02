using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws one slot of the inventory grid: the item's icon, its stack count, a tint for its tier
    /// and an outline when selected.
    /// </summary>
    public class InventorySlotView : MonoBehaviour
    {
        [Tooltip("The slot background.")]
        public Image background;
        public Image icon;
        public Text countText;

        [Tooltip("Outline shown while the slot is selected.")]
        public Outline selectionOutline;

        /// <summary>The inventory slot currently shown, or null when this view is empty.</summary>
        public InventorySlot Slot { get; private set; }

        /// <summary>Shows an occupied slot.</summary>
        public void Show(InventorySlot slot, bool selected)
        {
            Slot = slot;
            icon.enabled = true;
            icon.sprite = slot.Item.icon;
            countText.text = slot.Count > 1 ? "x" + slot.Count : string.Empty;
            background.color = slot.Item.tier != null ? Color.Lerp(Color.white, slot.Item.tier.color, 0.55f) : Color.white;
            selectionOutline.enabled = selected;
        }

        /// <summary>Shows an empty slot.</summary>
        public void Clear()
        {
            Slot = null;
            icon.enabled = false;
            countText.text = string.Empty;
            background.color = Color.white;
            selectionOutline.enabled = false;
        }
    }
}
