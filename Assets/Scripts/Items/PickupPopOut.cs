using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Makes a new pickup hop a short distance in a random direction when it appears.
    /// </summary>
    public class PickupPopOut : MonoBehaviour
    {
        [Tooltip("How far the pickup lands from where it spawned.")]
        public float distance = 0.7f;

        [Tooltip("Duration of the hop in seconds.")]
        public float duration = 0.25f;

        private void Start()
        {
            Vector2 from = transform.position;
            Vector2 to = from + Random.insideUnitCircle.normalized * distance;
            StartCoroutine(Tween.MoveTo(transform, from, to, duration, Easing.EaseOutQuad));
        }
    }
}
