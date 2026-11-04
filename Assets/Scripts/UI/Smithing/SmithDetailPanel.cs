using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the selected piece at the blacksmith: name, level, nature, final stats and what an upgrade or a reforge costs.
    /// </summary>
    public class SmithDetailPanel : MonoBehaviour
    {
        public SmithBrowser browser;
        public Blacksmith blacksmith;
        public Inventory inventory;

        [Tooltip("Everything that is shown when a piece is selected.")]
        public GameObject content;

        [Tooltip("Hint shown when nothing is selected.")]
        public GameObject emptyHint;

        public Image icon;
        public Text nameText;
        public Text infoText;
        public Text statsText;
        public Text upgradeText;
        public Text reforgeText;

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
            InventorySlot piece = browser.Selected;
            content.SetActive(piece != null);
            emptyHint.SetActive(piece == null);
            if (piece == null) return;

            GearState gear = piece.Gear;
            icon.sprite = piece.Item.icon;
            nameText.text = gear.DisplayName;
            nameText.color = piece.Item.tier != null ? piece.Item.tier.color : Color.white;

            infoText.text = "Level " + gear.Level + "/" + GearState.MaxLevel + "   Reforged " + gear.Reforges + "x\nNature: " + gear.NatureText;
            statsText.text = ItemDescriber.Effects(piece).Replace("\n", "   ");

            SmithCost upgrade = blacksmith.UpgradeCost(piece);
            upgradeText.text = upgrade == null ? "Fully upgraded" : "Upgrade to +" + (gear.Level + 1) + "\n" + upgrade.Describe(inventory, blacksmith.currency);

            SmithCost reforge = blacksmith.ReforgeCost(piece);
            reforgeText.text = "Reforge: back to +0, +" + Mathf.RoundToInt(GearState.ReforgeBonus * 100f) + "% forever, new random nature\n"
                + (reforge == null ? string.Empty : reforge.Describe(inventory, blacksmith.currency));
        }
    }
}
