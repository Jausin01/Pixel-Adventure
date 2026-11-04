using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Selects a slot row when it is tapped.
    /// </summary>
    [RequireComponent(typeof(EquipmentSlotRowView))]
    public class EquipmentSlotRowSelector : MonoBehaviour, IPointerClickHandler
    {
        public EquipmentBrowser browser;

        private EquipmentSlotRowView view;

        private void Awake()
        {
            view = GetComponent<EquipmentSlotRowView>();
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            browser.Select(view.slot);
        }
    }
}
