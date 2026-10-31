using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Makes an object float beside a target with a smooth follow and a gentle bobbing motion.
    /// The object switches sides depending on which way the target faces.
    /// </summary>
    public class HoverFollower : MonoBehaviour
    {
        [Tooltip("Transform to follow (usually the player).")]
        public Transform target;

        [Tooltip("Used to place the object on the side the target faces. Optional.")]
        public PlayerFacing facing;

        [Tooltip("Resting offset from the target (X is mirrored when the target faces left).")]
        public Vector2 offset = new Vector2(0.75f, 0.95f);

        [Tooltip("Approximate time in seconds to catch up with the target.")]
        public float smoothTime = 0.12f;

        [Tooltip("Vertical bobbing amplitude in world units.")]
        public float bobAmount = 0.06f;

        [Tooltip("Bobbing speed.")]
        public float bobSpeed = 3f;

        /// <summary>When true the object stops following (used while a weapon is attacking).</summary>
        public bool Paused { get; set; }

        private Vector3 velocity;

        private void Update()
        {
            if (Paused || target == null) return;

            float side = (facing != null && facing.Current == FacingDirection.Left) ? -1f : 1f;
            float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            Vector3 goal = target.position + new Vector3(offset.x * side, offset.y + bob, 0f);
            transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
        }
    }
}
