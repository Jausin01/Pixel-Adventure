using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Restores the attached <see cref="Health"/> a fixed time after it dies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class RespawnTimer : MonoBehaviour
    {
        [Tooltip("Seconds between death and respawn.")]
        public float delay = 1.2f;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
        }

        private void OnDied()
        {
            StartCoroutine(RespawnAfterDelay());
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            health.ResetHealth();
        }
    }
}
