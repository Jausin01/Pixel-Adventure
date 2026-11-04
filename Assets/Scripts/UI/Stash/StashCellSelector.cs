using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Sends the cell's slot to the other inventory when tapped.
    /// </summary>
    [RequireComponent(typeof(InventorySlotView))]
    public class StashCellSelector : MonoBehaviour, IPointerClickHandler
    {
        public StashPane pane;

        private InventorySlotView view;

        private void Awake()
        {
            view = GetComponent<InventorySlotView>();
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (view.Slot != null) pane.Transfer(view.Slot);
        }
    }
}
