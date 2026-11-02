using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Base class for enemy attacks. Handles the cooldown, the "attacking" state and applying damage.
    /// Subclasses implement the attack's animation and timing in <see cref="Perform"/>.
    /// Attacks are telegraphed: the target position is chosen at the start, so the player can dodge.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public abstract class EnemyAttack : MonoBehaviour
    {
        /// <summary>Height above a character's feet where attacks are aimed and hits are measured.</summary>
        protected const float BodyHeight = 0.4f;

        [Tooltip("Damage dealt to the target.")]
        public float damage = 8f;

        [Tooltip("Seconds between the end of one attack and the start of the next.")]
        public float cooldown = 1.2f;

        /// <summary>True while an attack is playing.</summary>
        public bool IsAttacking { get; private set; }

        /// <summary>The enemy's own health, so a dead enemy stops attacking.</summary>
        protected Health OwnHealth { get; private set; }

        private float nextAttackTime;

        private void Awake()
        {
            OwnHealth = GetComponent<Health>();
        }

        private void OnDisable()
        {
            IsAttacking = false;
        }

        /// <summary>Starts an attack on the target if the enemy is alive and the cooldown is over.</summary>
        public void TryAttack(Transform target)
        {
            if (IsAttacking || !OwnHealth.IsAlive || Time.time < nextAttackTime) return;
            StartCoroutine(Run(target));
        }

        /// <summary>Plays the attack: wind-up, strike and recovery. Call <see cref="TryDamage"/> at the strike moment.</summary>
        protected abstract IEnumerator Perform(Transform target);

        /// <summary>Damages the target if its body is within <paramref name="radius"/> of <paramref name="center"/>.</summary>
        protected bool TryDamage(Transform target, Vector2 center, float radius)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null || !damageable.IsAlive) return false;

            Vector2 body = (Vector2)target.position + Vector2.up * BodyHeight;
            if ((body - center).sqrMagnitude > radius * radius) return false;

            damageable.TakeDamage(damage, center);
            return true;
        }

        private IEnumerator Run(Transform target)
        {
            IsAttacking = true;
            yield return Perform(target);
            IsAttacking = false;
            nextAttackTime = Time.time + cooldown;
        }
    }
}
