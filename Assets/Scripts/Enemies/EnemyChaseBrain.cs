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

        private EnemyMovement movement;
        private EnemyAttack attack;
        private Health ownHealth;
        private Health targetHealth;

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
                movement.Direction = toTarget / distance;
                return;
            }

            attack.TryAttack(target);
        }
    }
}
