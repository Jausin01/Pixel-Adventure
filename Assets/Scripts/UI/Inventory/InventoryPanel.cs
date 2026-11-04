using System;
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

        /// <summary>Number of inventory windows currently open.</summary>
        public static int OpenCount { get; private set; }

        private bool counted;

        private void Start()
        {
            window.SetActive(false);
        }

        /// <summary>Shows the inventory screen.</summary>
        public void Open() { SetOpen(true); }

        /// <summary>Hides the inventory screen.</summary>
        public void Close() { SetOpen(false); }

        /// <summary>Opens the screen if closed, closes it if open.</summary>
        public void Toggle() { SetOpen(!window.activeSelf); }

        private void SetOpen(bool open)
        {
            window.SetActive(open);
            if (open == counted) return;
            counted = open;
            OpenCount += open ? 1 : -1;
        }

        private void OnDisable()
        {
            if (!counted) return;
            counted = false;
            OpenCount--;
        }
    }
}
