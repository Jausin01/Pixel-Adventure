using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Detects what a projectile touches. The projectile damages the first target it reaches
    /// and is destroyed on any solid collider (targets and walls alike).
    /// </summary>
    public class ProjectileImpact : MonoBehaviour
    {
        private float damage;
        private float radius;
        private Transform ignored;

        /// <summary>When false the target hit is not pushed back.</summary>
        public bool pushes = true;

        /// <summary>Sets up the impact detection.</summary>
        /// <param name="impactDamage">Damage dealt to a damageable target.</param>
        /// <param name="impactRadius">Radius of the detection circle.</param>
        /// <param name="ignoredRoot">Colliders under this transform (the shooter) are ignored.</param>
        /// <summary>Sound played when a target is hit.</summary>
        public string hitSound = "hit_light";

        public void Configure(float impactDamage, float impactRadius, Transform ignoredRoot)
        {
            damage = impactDamage;
            radius = impactRadius;
            ignored = ignoredRoot;
        }

        private void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D other = colliders[i];
                if (other.isTrigger) continue;
                if (ignored != null && other.transform.IsChildOf(ignored)) continue;

                IDamageable damageable = other.GetComponentInParent<IDamageable>();
                if (damageable != null && damageable.IsAlive)
                {
                    Knockback.Suppressed = !pushes;
                    Sfx.Play(hitSound);
                    try { damageable.TakeDamage(damage, transform.position); }
                    finally { Knockback.Suppressed = false; }
                }

                Destroy(gameObject);
                return;
            }
        }
    }
}
