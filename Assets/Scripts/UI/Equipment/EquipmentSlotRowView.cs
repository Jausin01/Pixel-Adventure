using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws one row of the slot list: the worn item's icon (or a silhouette when empty), the slot name,
    /// the item name and an outline when selected.
    /// </summary>
    public class EquipmentSlotRowView : MonoBehaviour
    {
        public Image icon;
        public Text slotText;
        public Text itemText;
        public Outline selectionOutline;

        [Tooltip("Dark silhouette shown while the slot is empty.")]
        public Sprite silhouette;

        [Tooltip("The slot this row represents.")]
        public EquipmentSlot slot;

        /// <summary>Shows the row. The piece may be null for an empty slot.</summary>
        public void Show(InventorySlot piece, bool selected)
        {
            ItemDefinition item = piece != null ? piece.Item : null;
            icon.sprite = item != null ? item.icon : silhouette;
            slotText.text = EquipmentSlots.Label(slot);
            itemText.text = item != null ? piece.Gear.DisplayName : "Empty";
            itemText.color = item != null && item.tier != null ? item.tier.color : new Color(1f, 1f, 1f, 0.5f);
            selectionOutline.enabled = selected;
        }
    }
}
