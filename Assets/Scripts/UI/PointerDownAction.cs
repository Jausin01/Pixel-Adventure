using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Base class for touch buttons that act the moment a finger touches them, without waiting for release.
    /// Subclasses implement <see cref="Execute"/>.
    /// </summary>
    public abstract class PointerDownAction : MonoBehaviour, IPointerDownHandler
    {
        /// <inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            Execute();
        }

        /// <summary>The action to perform when the button is touched.</summary>
        protected abstract void Execute();
    }
}
