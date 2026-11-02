using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Fills the slot views with the items the <see cref="InventoryBrowser"/> says are visible.
    /// Views beyond the visible items are shown empty.
    /// </summary>
    public class InventoryGrid : MonoBehaviour
    {
        public InventoryBrowser browser;

        [Tooltip("One view per inventory slot, in order.")]
        public InventorySlotView[] slotViews;

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
            for (int i = 0; i < slotViews.Length; i++)
            {
                if (i < browser.Visible.Count)
                {
                    InventorySlot slot = browser.Visible[i];
                    slotViews[i].Show(slot, slot == browser.Selected);
                }
                else
                {
                    slotViews[i].Clear();
                }
            }
        }
    }
}
