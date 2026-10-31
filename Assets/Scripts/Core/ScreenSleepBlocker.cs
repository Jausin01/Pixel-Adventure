using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Prevents the device screen from turning off while the game is running.
    /// </summary>
    public class ScreenSleepBlocker : MonoBehaviour
    {
        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
