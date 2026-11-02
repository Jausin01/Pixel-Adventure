using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Pushes a character away from whatever hit it. It only computes the push velocity;
    /// the movement component applies it.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class Knockback : MonoBehaviour
    {
        [Tooltip("Initial push speed in world units per second.")]
        public float strength = 5f;

        [Tooltip("How fast the push fades, in units per second squared.")]
        public float decay = 18f;

        /// <summary>Current push velocity.</summary>
        public Vector2 Velocity { get; private set; }

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= OnDamaged;
            Velocity = Vector2.zero;
        }

        private void Update()
        {
            Velocity = Vector2.MoveTowards(Velocity, Vector2.zero, decay * Time.deltaTime);
        }

        private void OnDamaged(float amount, Vector2 source)
        {
            Vector2 away = (Vector2)transform.position - source;
            if (away.sqrMagnitude < 0.0001f) away = Vector2.up;
            Velocity = away.normalized * strength;
        }
    }
}
