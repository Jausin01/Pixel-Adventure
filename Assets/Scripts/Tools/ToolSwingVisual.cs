using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shows the frames of a tool swing. Frame 0 is the tool raised, the last frame is the tool at the point of impact.
    /// The tool is mirrored to swing toward the left.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ToolSwingVisual : MonoBehaviour
    {
        [Tooltip("Swing frames in order.")]
        public Sprite[] frames;

        private SpriteRenderer spriteRenderer;

        /// <summary>Number of frames in the swing.</summary>
        public int FrameCount { get { return frames.Length; } }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        /// <summary>Shows one frame.</summary>
        public void Show(int frame, bool mirrored)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = frames[Mathf.Clamp(frame, 0, frames.Length - 1)];
            spriteRenderer.flipX = mirrored;
        }

        /// <summary>Hides the tool.</summary>
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}
