using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws one piece of gear in the blacksmith's list: icon, name with nature and level, whether it is worn,
    /// and an outline when selected.
    /// </summary>
    public class SmithRowView : MonoBehaviour
    {
        public Image icon;
        public Text nameText;
        public Text subText;
        public Outline selectionOutline;

        /// <summary>The piece shown, or null while the row is hidden.</summary>
        public InventorySlot Piece { get; private set; }

        /// <summary>Shows the row for a piece.</summary>
        public void Show(InventorySlot piece, bool selected, bool worn)
        {
            Piece = piece;
            gameObject.SetActive(true);
            icon.sprite = piece.Item.icon;
            nameText.text = piece.Gear.DisplayName;
            nameText.color = piece.Item.tier != null ? piece.Item.tier.color : Color.white;
            subText.text = worn ? "Worn" : string.Empty;
            selectionOutline.enabled = selected;
        }

        /// <summary>Hides the row.</summary>
        public void Hide()
        {
            Piece = null;
            gameObject.SetActive(false);
        }
    }
}
