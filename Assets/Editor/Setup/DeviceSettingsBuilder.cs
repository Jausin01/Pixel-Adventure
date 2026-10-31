using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the object that holds the device-level runtime settings.
    /// </summary>
    public static class DeviceSettingsBuilder
    {
        /// <summary>Builds the object with frame rate, screen sleep and orientation components.</summary>
        public static void Build()
        {
            GameObject go = new GameObject("Device Settings");
            go.AddComponent<FrameRateLimiter>();
            go.AddComponent<ScreenSleepBlocker>();
            go.AddComponent<ScreenOrientationLock>();
        }
    }
}
