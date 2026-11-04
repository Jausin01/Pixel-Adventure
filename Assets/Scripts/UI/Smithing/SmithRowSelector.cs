using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Selects the row's piece when tapped.
    /// </summary>
    [RequireComponent(typeof(SmithRowView))]
    public class SmithRowSelector : MonoBehaviour, IPointerClickHandler
    {
        public SmithBrowser browser;

        private SmithRowView view;

        private void Awake()
        {
            view = GetComponent<SmithRowView>();
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (view.Piece != null) browser.Select(view.Piece);
        }
    }
}
