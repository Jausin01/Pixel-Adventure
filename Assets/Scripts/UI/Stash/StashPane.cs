using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// One half of the stash screen: a paged grid showing one inventory. Tapping a slot sends the whole slot to the other inventory.
    /// Must stay on an object that is always active.
    /// </summary>
    public class StashPane : MonoBehaviour
    {
        public Inventory source;

        [Tooltip("The inventory that receives what is tapped here.")]
        public Inventory other;

        public string label;

        [Tooltip("Pooled slot views, one per cell of a page.")]
        public InventorySlotView[] cells;

        public Text titleText;
        public Text pageText;

        private int page;

        private void OnEnable()
        {
            source.Changed += OnSourceChanged;
            Refresh();
        }

        private void OnDisable()
        {
            source.Changed -= OnSourceChanged;
        }

        /// <summary>Sends a slot of this pane to the other inventory.</summary>
        public void Transfer(InventorySlot slot)
        {
            ItemTransfer.Move(source, other, slot);
        }

        /// <summary>Moves to another page (+1 next, -1 previous), staying inside the list.</summary>
        public void ChangePage(int direction)
        {
            page = Mathf.Clamp(page + direction, 0, PageCount - 1);
            Refresh();
        }

        private int PageCount { get { return Mathf.Max(1, Mathf.CeilToInt(source.Slots.Count / (float)cells.Length)); } }

        private void OnSourceChanged(ItemDefinition item, int total)
        {
            Refresh();
        }

        private void Refresh()
        {
            page = Mathf.Clamp(page, 0, PageCount - 1);

            for (int i = 0; i < cells.Length; i++)
            {
                int index = page * cells.Length + i;
                if (index < source.Slots.Count) cells[i].Show(source.Slots[index], false);
                else cells[i].Clear();
            }

            titleText.text = label + "  " + source.Slots.Count + "/" + source.capacity;
            pageText.text = (page + 1) + "/" + PageCount;
        }
    }
}
