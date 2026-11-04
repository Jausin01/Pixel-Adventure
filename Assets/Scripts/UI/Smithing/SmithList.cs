using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the rows of the blacksmith's list up to date with the current page.
    /// </summary>
    public class SmithList : MonoBehaviour
    {
        public SmithBrowser browser;

        [Tooltip("Pooled rows, one per row of a page. Unused rows are hidden.")]
        public SmithRowView[] rows;

        public Text pageText;

        [Tooltip("Shown when the player owns no gear.")]
        public GameObject emptyHint;

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
            bool any = false;
            for (int i = 0; i < rows.Length; i++)
            {
                InventorySlot piece = browser.PieceAt(i);
                if (piece == null)
                {
                    rows[i].Hide();
                    continue;
                }

                any = true;
                rows[i].Show(piece, piece == browser.Selected, browser.IsWorn(piece));
            }

            pageText.text = (browser.Page + 1) + "/" + browser.PageCount;
            emptyHint.SetActive(!any);
        }
    }
}
