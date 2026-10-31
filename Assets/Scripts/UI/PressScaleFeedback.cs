using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Shrinks a UI element slightly while it is pressed, as visual feedback for touch.
    /// </summary>
    public class PressScaleFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Scale applied while pressed.")]
        public float pressedScale = 0.92f;

        /// <inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * pressedScale;
        }

        /// <inheritdoc/>
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }
    }
}
