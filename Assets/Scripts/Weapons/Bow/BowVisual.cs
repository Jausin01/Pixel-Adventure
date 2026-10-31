using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shows the bow sprite that matches each stage of the shooting animation.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BowVisual : MonoBehaviour
    {
        public Sprite idle;
        public Sprite nock;
        public Sprite draw;
        public Sprite release;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ShowIdle();
        }

        /// <summary>Bow at rest.</summary>
        public void ShowIdle() { spriteRenderer.sprite = idle; }

        /// <summary>Arrow placed on the string.</summary>
        public void ShowNock() { spriteRenderer.sprite = nock; }

        /// <summary>String fully drawn back.</summary>
        public void ShowDraw() { spriteRenderer.sprite = draw; }

        /// <summary>String just released.</summary>
        public void ShowRelease() { spriteRenderer.sprite = release; }
    }
}
