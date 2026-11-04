using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Freezes the game while this object is active. Put it on a window that pauses the game when shown.
    /// </summary>
    public class PauseWhileOpen : MonoBehaviour
    {
        /// <summary>True while the pausing window is shown.</summary>
        public static bool IsOpen { get; private set; }

        private void OnEnable()
        {
            IsOpen = true;
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            IsOpen = false;
            Time.timeScale = 1f;
        }
    }
}
