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
            Sfx.Play(ClickSound);
            Execute();
        }

        /// <summary>Sound played on touch. Override to return null for buttons with their own sound.</summary>
        protected virtual string ClickSound { get { return "ui_click"; } }

        /// <summary>The action to perform when the button is touched.</summary>
        protected abstract void Execute();
    }
}
