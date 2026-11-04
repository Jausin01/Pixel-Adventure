using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws one inventory piece that can be put in the selected slot: icon, name (with nature and level) and its bonuses.
    /// </summary>
    public class EquipmentCandidateRowView : MonoBehaviour
    {
        public Image icon;
        public Text nameText;
        public Text bonusText;

        /// <summary>The piece shown, or null while the row is hidden.</summary>
        public InventorySlot Piece { get; private set; }

        /// <summary>Shows the row for a piece of gear.</summary>
        public void Show(InventorySlot piece)
        {
            Piece = piece;
            ItemDefinition item = piece.Item;
            gameObject.SetActive(true);
            icon.sprite = item.icon;
            nameText.text = piece.Gear.DisplayName;
            nameText.color = item.tier != null ? item.tier.color : Color.white;
            bonusText.text = ItemDescriber.Effects(piece).Replace("\n", "   ");
        }

        /// <summary>Hides the row.</summary>
        public void Hide()
        {
            Piece = null;
            gameObject.SetActive(false);
        }
    }
}
