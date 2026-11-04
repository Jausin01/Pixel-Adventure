using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the screen-space canvas and the safe-area container that holds all touch controls.
    /// </summary>
    public static class CanvasBuilder
    {
        /// <summary>Builds the canvas (1280x720 reference, expanded to the screen shape) and returns the safe-area container to parent controls to.</summary>
        public static Transform Build()
        {
            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand; // never crops: the 1280x720 reference always fits, extra space is added on the longer side

            return CreateSafeArea(canvasObject.transform);
        }

        private static Transform CreateSafeArea(Transform canvas)
        {
            GameObject safeArea = new GameObject("Safe Area", typeof(RectTransform), typeof(SafeAreaFitter));
            safeArea.transform.SetParent(canvas, false);

            RectTransform rect = (RectTransform)safeArea.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return rect;
        }
    }
}
