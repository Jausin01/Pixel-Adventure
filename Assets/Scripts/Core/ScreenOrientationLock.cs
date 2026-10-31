using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Restricts the game to landscape orientation (left or right, auto-rotating between them).
    /// </summary>
    public class ScreenOrientationLock : MonoBehaviour
    {
        private void Awake()
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}
