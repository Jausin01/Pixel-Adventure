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

        private Vector3 velocity;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
        }
    }
}
