using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Nudges the object away from the damage source for a short moment when the attached
    /// <see cref="Health"/> takes damage. The position the object has when it spawns is used as its resting place.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class HitShake : MonoBehaviour
    {
        [Tooltip("How far the object is pushed, in world units.")]
        public float distance = 0.12f;

        [Tooltip("How long the push lasts, in seconds.")]
        public float duration = 0.08f;

        private Health health;
        private Vector3 home;
        private Coroutine running;

        private void Awake()
        {
            health = GetComponent<Health>();
            home = transform.position;
        }

        private void OnEnable()
        {
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= OnDamaged;
            if (running != null) StopCoroutine(running);
            running = null;
            transform.position = home;
        }

        private void OnDamaged(float amount, Vector2 source)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Shake(source));
        }

        private IEnumerator Shake(Vector2 source)
        {
            Vector2 away = ((Vector2)transform.position - source).normalized;
            transform.position = home + (Vector3)(away * distance);
            yield return new WaitForSeconds(duration);
            transform.position = home;
            running = null;
        }
    }
}
