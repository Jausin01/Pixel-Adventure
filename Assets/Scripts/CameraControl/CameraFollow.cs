using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Smoothly keeps the camera centered on a target in the XY plane.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("Transform to follow.")]
        public Transform target;

        [Tooltip("Approximate time in seconds the camera takes to catch up.")]
        public float smoothTime = 0.08f;

        [Tooltip("Distance beyond which the camera jumps instead of gliding.")]
        public float SnapDistance = 20f;

        private Vector3 velocity;
        private Vector3 basePosition;
        private bool started;
        private float shakeStrength;
        private float shakeEnd;

        /// <summary>Shakes the view. A stronger or longer call replaces a weaker one still playing.</summary>
        /// <param name="strength">Largest offset in world units.</param>
        /// <param name="duration">Seconds the shake lasts.</param>
        public void Shake(float strength, float duration)
        {
            if (strength < shakeStrength && Time.time < shakeEnd) return;

            shakeStrength = strength;
            shakeEnd = Time.time + duration;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            // The followed position is kept apart from the shake, so the shake never drags the camera away.
            if (!started)
            {
                basePosition = transform.position;
                started = true;
            }

            Vector3 goal = new Vector3(target.position.x, target.position.y, basePosition.z);

            // After a teleport, jump straight to the target instead of sweeping across the world.
            if ((goal - basePosition).sqrMagnitude > SnapDistance * SnapDistance)
            {
                basePosition = goal;
                velocity = Vector3.zero;
            }
            else
            {
                basePosition = Vector3.SmoothDamp(basePosition, goal, ref velocity, smoothTime);
            }

            Vector2 shake = Time.time < shakeEnd ? Random.insideUnitCircle * shakeStrength : Vector2.zero;
            transform.position = basePosition + (Vector3)shake;
        }
    }
}
