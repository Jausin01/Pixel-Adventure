using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Caps the frame rate and disables vertical sync so mobile devices run at a
    /// predictable, battery-friendly speed.
    /// </summary>
    public class FrameRateLimiter : MonoBehaviour
    {
        [Tooltip("Target frames per second.")]
        public int targetFps = 60;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFps;
        }
    }
}
