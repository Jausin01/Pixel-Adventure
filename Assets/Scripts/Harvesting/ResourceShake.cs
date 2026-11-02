using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shakes a resource sideways when it is hit.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class ResourceShake : MonoBehaviour
    {
        [Tooltip("How far the object moves, in world units.")]
        public float distance = 0.06f;

        [Tooltip("Duration of the shake in seconds.")]
        public float duration = 0.08f;

        private ResourceDurability durability;
        private Vector3 home;
        private Coroutine running;

        private void Awake()
        {
            durability = GetComponent<ResourceDurability>();
            home = transform.position;
        }

        private void OnEnable()
        {
            durability.Hit += OnHit;
        }

        private void OnDisable()
        {
            durability.Hit -= OnHit;
            if (running != null) StopCoroutine(running);
            running = null;
            transform.position = home;
        }

        private void OnHit(float amount, Vector2 source)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Shake(source));
        }

        private IEnumerator Shake(Vector2 source)
        {
            float side = source.x < home.x ? 1f : -1f;
            transform.position = home + new Vector3(side * distance, 0f, 0f);
            yield return new WaitForSeconds(duration);
            transform.position = home;
            running = null;
        }
    }
}
