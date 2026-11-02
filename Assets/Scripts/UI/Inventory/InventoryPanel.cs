using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Opens and closes the inventory screen.
    /// This component must stay on an object that is always active; the window is a separate child object.
    /// </summary>
    public class InventoryPanel : MonoBehaviour
    {
        [Tooltip("The window object that is shown and hidden.")]
        public GameObject window;

        private void Start()
        {
            window.SetActive(false);
        }

        /// <summary>Shows the inventory screen.</summary>
        public void Open() { window.SetActive(true); }

        /// <summary>Hides the inventory screen.</summary>
        public void Close() { window.SetActive(false); }

        /// <summary>Opens the screen if closed, closes it if open.</summary>
        public void Toggle() { window.SetActive(!window.activeSelf); }
    }
}
