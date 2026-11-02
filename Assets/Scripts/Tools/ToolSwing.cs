using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays the swing of one tool toward the nearest matching resource and applies the hit at the impact frame.
    /// Drawing the frames is delegated to <see cref="ToolSwingVisual"/>.
    /// </summary>
    public class ToolSwing : MonoBehaviour
    {
        [Tooltip("Which tool this is.")]
        public ToolType tool = ToolType.Axe;

        [Tooltip("The user (usually the player).")]
        public Transform owner;

        public ToolSwingVisual visual;

        [Tooltip("How far from the owner a resource can be worked on.")]
        public float reach = 1.6f;

        [Tooltip("Durability removed from the resource per swing.")]
        public float power = 1f;

        [Tooltip("Seconds each frame is shown before the impact frame.")]
        public float frameTime = 0.05f;

        [Tooltip("Seconds the impact frame stays on screen.")]
        public float impactHoldTime = 0.1f;

        [Tooltip("Distance from the owner where the tool is drawn, toward the resource.")]
        public float holdDistance = 0.6f;

        [Tooltip("Height of the tool above the owner's feet.")]
        public float holdHeight = 0.5f;

        private bool swinging;

        /// <summary>True while a swing is playing.</summary>
        public bool IsBusy { get { return swinging; } }

        private void OnDisable()
        {
            swinging = false;
            visual.Hide();
        }

        /// <summary>Starts a swing if a matching resource is within reach and no swing is playing.</summary>
        public void Use()
        {
            if (swinging) return;
            StartCoroutine(Swing());
        }

        private IEnumerator Swing()
        {
            swinging = true;

            IHarvestable target = HarvestTargetFinder.FindNearest(owner.position, reach, tool);
            if (target == null)
            {
                swinging = false;
                yield break;
            }

            Vector2 toTarget = target.Position - (Vector2)owner.position;
            bool mirrored = toTarget.x < -0.1f;
            Vector2 direction = toTarget.sqrMagnitude > 0.001f ? toTarget.normalized : Vector2.right;
            transform.position = (Vector2)owner.position + direction * holdDistance + Vector2.up * holdHeight;

            int last = visual.FrameCount - 1;
            for (int frame = 0; frame <= last; frame++)
            {
                visual.Show(frame, mirrored);
                if (frame == last) target.Harvest(tool, power, owner.position);
                yield return new WaitForSeconds(frame == last ? impactHoldTime : frameTime);
            }

            visual.Hide();
            swinging = false;
        }
    }
}
