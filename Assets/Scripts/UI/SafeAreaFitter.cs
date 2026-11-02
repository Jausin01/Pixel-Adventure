using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Resizes this UI element to the part of the screen that is not covered by notches, camera
    /// cut-outs or rounded corners. Put all touch controls inside it.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Rect appliedArea;
        private Vector2Int appliedResolution;

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
        }

        private void Update()
        {
            if (Screen.safeArea == appliedArea && appliedResolution.x == Screen.width && appliedResolution.y == Screen.height) return;
            Apply();
        }

        private void Apply()
        {
            appliedArea = Screen.safeArea;
            appliedResolution = new Vector2Int(Screen.width, Screen.height);

            Vector2 screen = new Vector2(Screen.width, Screen.height);
            rectTransform.anchorMin = appliedArea.position / screen;
            rectTransform.anchorMax = (appliedArea.position + appliedArea.size) / screen;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
