using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Sends the player back to where the game started, with full health, a moment after dying.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Tooltip("Seconds between death and respawn.")]
        public float delay = 1.5f;

        /// <summary>While true the respawn waits after the delay. Set by the game over panel until the player confirms.</summary>
        public bool HoldRespawn { get; set; }

        private Health health;
        private Rigidbody2D body;
        private Vector2 spawnPoint;

        private void Awake()
        {
            health = GetComponent<Health>();
            body = GetComponent<Rigidbody2D>();
            spawnPoint = transform.position;
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
            while (HoldRespawn) yield return null;
            transform.position = spawnPoint;
            if (body != null) body.position = spawnPoint;
            health.ResetHealth();
        }
    }
}
