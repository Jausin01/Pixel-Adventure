using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the merchant's sell column tidy: only items the player carries are shown, packed from the top.
    /// This component must stay on an always-active object; the rows are children of the window.
    /// </summary>
    public class ShopSellList : MonoBehaviour
    {
        public Inventory inventory;
        public ShopRowView[] rows;

        [Tooltip("Position of the first visible row, in the parent's top-left anchored space.")]
        public Vector2 firstPosition;

        [Tooltip("Vertical distance between rows.")]
        public float rowStep = 98f;

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            Layout();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        private void OnInventoryChanged(ItemDefinition item, int count) { Layout(); }

        private void Layout()
        {
            int visible = 0;
            for (int i = 0; i < rows.Length; i++)
            {
                bool owned = inventory.Count(rows[i].item) > 0;
                rows[i].gameObject.SetActive(owned);
                if (!owned) continue;

                ((RectTransform)rows[i].transform).anchoredPosition = firstPosition + new Vector2(0f, -visible * rowStep);
                visible++;
            }
        }
    }
}
