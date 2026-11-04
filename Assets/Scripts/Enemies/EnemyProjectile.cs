using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A projectile fired by an enemy. Flies straight, hurts its target on touch and disappears on walls or when its time runs out.
    /// </summary>
    public class EnemyProjectile : MonoBehaviour
    {
        private Transform target;
        private Transform owner;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private float damage;
        private float radius;
        private float spin;

        /// <summary>Starts the flight.</summary>
        /// <param name="shooter">The enemy that fired it. Its own colliders never stop the projectile.</param>
        /// <param name="victim">The only thing it can hurt (usually the player).</param>
        /// <param name="flightDirection">Unit direction.</param>
        /// <param name="flightSpeed">World units per second.</param>
        /// <param name="lifetime">Seconds before it disappears.</param>
        /// <param name="hitDamage">Damage dealt on touch.</param>
        /// <param name="hitRadius">Touch distance to the victim's body.</param>
        /// <param name="spinDegrees">Spin in degrees per second. 0 makes it face its direction.</param>
        public void Launch(Transform shooter, Transform victim, Vector2 flightDirection, float flightSpeed, float lifetime,
            float hitDamage, float hitRadius, float spinDegrees)
        {
            owner = shooter;
            target = victim;
            direction = flightDirection.normalized;
            speed = flightSpeed;
            remainingLife = lifetime;
            damage = hitDamage;
            radius = hitRadius;
            spin = spinDegrees;

            if (spin == 0f) transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            if (spin != 0f) transform.Rotate(0f, 0f, spin * Time.deltaTime);

            remainingLife -= Time.deltaTime;
            if (remainingLife <= 0f || HitsWall())
            {
                Destroy(gameObject);
                return;
            }

            IDamageable damageable = target != null ? target.GetComponent<IDamageable>() : null;
            if (damageable == null || !damageable.IsAlive) return;

            Vector2 body = (Vector2)target.position + Vector2.up * 0.4f;
            if (((Vector2)transform.position - body).sqrMagnitude > radius * radius) return;

            damageable.TakeDamage(damage, transform.position);
            Destroy(gameObject);
        }

        // Anything solid that is not the shooter and cannot be damaged (scenery).
        private bool HitsWall()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius * 0.5f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D other = colliders[i];
                if (other.isTrigger || other.transform.IsChildOf(owner)) continue;
                if (other.GetComponentInParent<IDamageable>() == null) return true;
            }
            return false;
        }
    }
}
