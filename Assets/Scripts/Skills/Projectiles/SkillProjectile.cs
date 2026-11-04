using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A projectile fired by a skill. Flies straight, hurts what it touches and stops at walls.
    /// It can pierce (keep going after hitting), spin, and blow up in a circle where it ends.
    /// </summary>
    public class SkillProjectile : MonoBehaviour
    {
        private readonly HitScanner scanner = new HitScanner();
        private Transform owner;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private float damage;
        private float radius;
        private bool pierce;
        private float explodeRadius;
        private float spin;
        private Sprite burstSprite;

        /// <summary>Starts the flight.</summary>
        /// <param name="shooter">Never hurt by the projectile.</param>
        /// <param name="flightDirection">Unit direction.</param>
        /// <param name="flightSpeed">World units per second.</param>
        /// <param name="lifetime">Seconds before it disappears.</param>
        /// <param name="hitDamage">Damage of each hit.</param>
        /// <param name="hitRadius">Radius of the hit circle.</param>
        /// <param name="piercing">True to keep flying after hitting something.</param>
        /// <param name="explosionRadius">Radius of the blast where it ends. 0 for none.</param>
        /// <param name="spinDegrees">Spin in degrees per second. 0 makes it face its direction instead.</param>
        /// <param name="blastSprite">Shown at the blast. Optional.</param>
        public void Launch(Transform shooter, Vector2 flightDirection, float flightSpeed, float lifetime, float hitDamage,
            float hitRadius, bool piercing, float explosionRadius, float spinDegrees, Sprite blastSprite)
        {
            owner = shooter;
            direction = flightDirection.normalized;
            speed = flightSpeed;
            remainingLife = lifetime;
            damage = hitDamage;
            radius = hitRadius;
            pierce = piercing;
            explodeRadius = explosionRadius;
            spin = spinDegrees;
            burstSprite = blastSprite;

            if (spin == 0f) transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            if (spin != 0f) transform.Rotate(0f, 0f, spin * Time.deltaTime);

            bool hit = scanner.Scan(transform.position, radius, damage, owner);
            if (hit && !pierce)
            {
                End();
                return;
            }

            remainingLife -= Time.deltaTime;
            if (remainingLife <= 0f || HitsWall()) End();
        }

        // Anything solid that is neither the shooter nor something that can be hurt.
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

        private void End()
        {
            if (explodeRadius > 0f)
            {
                Sfx.Play("fireball_explosion");
                scanner.Scan(transform.position, explodeRadius, damage, owner); // targets already hit are not hurt twice
                if (burstSprite != null)
                {
                    Material material = GetComponent<SpriteRenderer>().sharedMaterial;
                    EffectFade.Spawn(burstSprite, material, transform.position, 0f, Vector2.one * (explodeRadius / 0.8f), 0.3f, 1.3f, 9);
                }
            }
            Destroy(gameObject);
        }
    }
}
