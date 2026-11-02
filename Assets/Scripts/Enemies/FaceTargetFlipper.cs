using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Mirrors a side-view sprite so it always looks toward a target.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class FaceTargetFlipper : MonoBehaviour
    {
        public Transform target;

        [Tooltip("True when the original sprite faces right.")]
        public bool spriteFacesRight = true;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (target == null) return;

            float difference = target.position.x - transform.position.x;
            if (Mathf.Abs(difference) < 0.05f) return;

            spriteRenderer.flipX = spriteFacesRight ? difference < 0f : difference > 0f;
        }
    }
}
