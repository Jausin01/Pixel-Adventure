using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Selects a slot when it is tapped. A tap that turns into a drag is not a click,
    /// so scrolling the grid never selects anything.
    /// </summary>
    [RequireComponent(typeof(InventorySlotView))]
    public class InventorySlotSelector : MonoBehaviour, IPointerClickHandler
    {
        public InventoryBrowser browser;

        private InventorySlotView view;

        private void Awake()
        {
            view = GetComponent<InventorySlotView>();
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (view.Slot != null) browser.Select(view.Slot);
        }
    }
}
