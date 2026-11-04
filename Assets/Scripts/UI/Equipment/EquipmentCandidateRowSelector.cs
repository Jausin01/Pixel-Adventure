using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Equips the row's piece in the selected slot when tapped. A tap that turns into a drag is not a click,
    /// so scrolling the list never equips anything.
    /// </summary>
    [RequireComponent(typeof(EquipmentCandidateRowView))]
    public class EquipmentCandidateRowSelector : MonoBehaviour, IPointerClickHandler
    {
        public Equipment equipment;
        public EquipmentBrowser browser;

        private EquipmentCandidateRowView view;

        private void Awake()
        {
            view = GetComponent<EquipmentCandidateRowView>();
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (view.Piece != null) equipment.Equip(view.Piece, browser.Selected);
        }
    }
}
