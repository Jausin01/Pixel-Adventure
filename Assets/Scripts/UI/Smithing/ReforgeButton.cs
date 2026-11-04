using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that reforges the selected piece. A reforge can make the piece worse, so the first touch only arms
    /// the button; a second touch within a few seconds does the reforge.
    /// </summary>
    public class ReforgeButton : PointerDownAction
    {
        public SmithBrowser browser;
        public Blacksmith blacksmith;
        public Text label;

        [Tooltip("Seconds the button stays armed after the first touch.")]
        public float confirmTime = 3f;

        private InventorySlot armedPiece;
        private float armedUntil;

        private void Update()
        {
            if (armedPiece != null && (browser.Selected != armedPiece || Time.unscaledTime > armedUntil)) Disarm();
        }

        private void OnDisable()
        {
            Disarm();
        }

        /// <inheritdoc/>
        protected override void Execute()
        {
            InventorySlot piece = browser.Selected;
            if (piece == null || !blacksmith.CanAfford(blacksmith.ReforgeCost(piece)))
            {
                Disarm();
                return;
            }

            if (armedPiece != piece)
            {
                armedPiece = piece;
                armedUntil = Time.unscaledTime + confirmTime;
                label.text = "Tap again to confirm";
                return;
            }

            blacksmith.TryReforge(piece);
            Disarm();
        }

        private void Disarm()
        {
            armedPiece = null;
            if (label != null) label.text = "Reforge";
        }
    }
}
