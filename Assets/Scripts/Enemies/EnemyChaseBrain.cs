using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Decides what an enemy does: stand still when the target is far, chase it when it is seen,
    /// and attack when close. It only gives orders to the movement and attack components.
    /// </summary>
    [RequireComponent(typeof(EnemyMovement), typeof(EnemyAttack), typeof(Health))]
    public class EnemyChaseBrain : MonoBehaviour
    {
        [Tooltip("The thing to chase (usually the player).")]
        public Transform target;

        [Tooltip("Distance at which the enemy notices the target.")]
        public float sightRange = 6f;

        [Tooltip("Distance at which the enemy stops and attacks.")]
        public float attackRange = 1f;

        [Tooltip("Seconds an enemy walks along an obstacle after bumping into it, before heading for the target again.")]
        public float detourTime = 0.6f;

        private EnemyMovement movement;
        private EnemyAttack attack;
        private Health ownHealth;
        private Health targetHealth;
        private Vector2 detourDirection;
        private float detourEnd;
        private float lastBumpTime = -10f;
        private float detourSide = 1f;

        private void Awake()
        {
            movement = GetComponent<EnemyMovement>();
            attack = GetComponent<EnemyAttack>();
            ownHealth = GetComponent<Health>();
        }

        private void Start()
        {
            if (target != null) targetHealth = target.GetComponent<Health>();
        }

        private void Update()
        {
            movement.Direction = Vector2.zero;

            bool canAct = ownHealth.IsAlive && target != null && !attack.IsAttacking
                && (targetHealth == null || targetHealth.IsAlive);
            if (!canAct) return;

            Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;
            float distance = toTarget.magnitude;

            if (distance > sightRange) return;

            if (distance > attackRange)
            {
                // Straight at the target, unless a recent bump asked for a detour around an obstacle.
                movement.Direction = Time.time < detourEnd ? detourDirection : toTarget / distance;
                return;
            }

            attack.TryAttack(target);
        }

        // Runs only while touching something. Solid scenery (no Health) makes the enemy re-plan once:
        // it slides along the obstacle for a moment instead of pushing into it every frame.
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Time.time < detourEnd || target == null) return;
            if (collision.collider.GetComponentInParent<Health>() != null) return;

            Vector2 normal = collision.GetContact(0).normal;
            Vector2 tangent = new Vector2(-normal.y, normal.x);
            Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;

            // Bumping again right after a detour means that side is blocked too: try the other one.
            if (Time.time - lastBumpTime < detourTime * 2f) detourSide = -detourSide;
            else detourSide = Vector2.Dot(tangent, toTarget) >= 0f ? 1f : -1f;

            detourDirection = (tangent * detourSide + normal * 0.3f).normalized;
            detourEnd = Time.time + detourTime;
            lastBumpTime = Time.time;
        }
    }
}
