using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shrinks a fixed-size window so it always fits inside its parent (for example a small phone safe area).
    /// It never enlarges the window.
    /// </summary>
    public class ScaleToFit : MonoBehaviour
    {
        [Tooltip("Size the window was designed for, in canvas units.")]
        public Vector2 designSize = new Vector2(1180f, 640f);

        [Tooltip("Free space kept around the window, in canvas units.")]
        public float margin = 16f;

        private RectTransform parent;
        private Vector2 lastParentSize;

        private void OnEnable()
        {
            Apply();
        }

        private void Update()
        {
            if (parent != null && parent.rect.size != lastParentSize) Apply();
        }

        private void Apply()
        {
            if (parent == null) parent = transform.parent as RectTransform;
            if (parent == null) return;

            lastParentSize = parent.rect.size;
            float fitX = (lastParentSize.x - margin * 2f) / designSize.x;
            float fitY = (lastParentSize.y - margin * 2f) / designSize.y;
            float scale = Mathf.Clamp(Mathf.Min(fitX, fitY), 0.3f, 1f);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
