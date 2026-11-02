using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Expands a row of menu buttons to the left of this button, and collapses them back into it.
    /// Put it on the button that opens the menu. The entries must share its parent and anchors.
    /// </summary>
    public class MenuExpander : MonoBehaviour
    {
        [Tooltip("Buttons that slide out, nearest first.")]
        public RectTransform[] entries;

        [Tooltip("Distance between buttons.")]
        public float spacing = 100f;

        [Tooltip("Seconds the slide takes.")]
        public float duration = 0.15f;

        /// <summary>True while the menu is expanded.</summary>
        public bool IsOpen { get; private set; }

        private RectTransform self;
        private Coroutine running;

        private void Awake()
        {
            self = (RectTransform)transform;
        }

        private void Start()
        {
            SetEntriesActive(false);
        }

        /// <summary>Expands the menu if collapsed, collapses it if expanded.</summary>
        public void Toggle()
        {
            IsOpen = !IsOpen;
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Slide(IsOpen));
        }

        private IEnumerator Slide(bool open)
        {
            if (open) SetEntriesActive(true);

            Vector2[] from = new Vector2[entries.Length];
            for (int i = 0; i < entries.Length; i++) from[i] = entries[i].anchoredPosition;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                Place(from, open, Easing.EaseOutQuad(Mathf.Clamp01(elapsed / duration)));
                yield return null;
            }

            Place(from, open, 1f);
            if (!open) SetEntriesActive(false);
            running = null;
        }

        private void Place(Vector2[] from, bool open, float progress)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                Vector2 target = open ? self.anchoredPosition + new Vector2(-(i + 1) * spacing, 0f) : self.anchoredPosition;
                entries[i].anchoredPosition = Vector2.Lerp(from[i], target, progress);
            }
        }

        private void SetEntriesActive(bool active)
        {
            for (int i = 0; i < entries.Length; i++) entries[i].gameObject.SetActive(active);
        }
    }
}
