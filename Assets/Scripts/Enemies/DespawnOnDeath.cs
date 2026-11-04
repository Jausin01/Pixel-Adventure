using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Removes the object a short time after the attached <see cref="Health"/> dies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DespawnOnDeath : MonoBehaviour
    {
        [Tooltip("Seconds between death and removal.")]
        public float delay = 1f;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable() { health.Died += OnDied; }

        private void OnDisable() { health.Died -= OnDied; }

        private void OnDied()
        {
            Destroy(gameObject, delay);
        }
    }
}
