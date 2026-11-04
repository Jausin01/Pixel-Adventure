using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Flies a boomerang out along a direction, slowing as it goes, then back to its owner.
    /// It passes through enemies and can hurt each of them once on the way out and once on the way back.
    /// A wall makes it turn back early. It destroys itself when it returns.
    /// </summary>
    public class BoomerangFlight : MonoBehaviour
    {
        private readonly HitScanner scanner = new HitScanner { pushes = false };
        private Transform owner;
        private Vector2 direction;
        private float range;
        private float speed;
        private float damage;
        private float hitRadius;
        private float catchHeight;
        private Action onReturned;
        private float travelled;
        private bool returning;
        private float age;

        private const float MaxAge = 6f;

        /// <summary>Starts the flight.</summary>
        /// <param name="thrower">The one who threw it. Never hurt, and where it returns to.</param>
        /// <param name="flightDirection">Unit direction of the throw.</param>
        /// <param name="flightRange">How far it goes before turning back.</param>
        /// <param name="flightSpeed">Speed at the start of the flight, in world units per second.</param>
        /// <param name="hitDamage">Damage of each hit.</param>
        /// <param name="radius">Radius of the hit circle.</param>
        /// <param name="catchOffset">Height above the thrower's feet where it is caught.</param>
        /// <param name="returned">Called when the boomerang is caught or lost.</param>
        public void Launch(Transform thrower, Vector2 flightDirection, float flightRange, float flightSpeed,
            float hitDamage, float radius, float catchOffset, Action returned)
        {
            owner = thrower;
            direction = flightDirection.normalized;
            range = flightRange;
            speed = flightSpeed;
            damage = hitDamage;
            hitRadius = radius;
            catchHeight = catchOffset;
            onReturned = returned;
        }

        private void OnDestroy()
        {
            if (onReturned != null) onReturned();
        }

        private void Update()
        {
            age += Time.deltaTime;
            if (owner == null || age > MaxAge)
            {
                Destroy(gameObject);
                return;
            }

            if (returning) FlyBack();
            else FlyOut();

            scanner.Scan(transform.position, hitRadius, damage, owner);
        }

        private void FlyOut()
        {
            float slowdown = Mathf.Lerp(1f, 0.35f, travelled / range);
            float step = speed * slowdown * Time.deltaTime;
            transform.position += (Vector3)(direction * step);
            travelled += step;

            if (travelled >= range || HitsWall()) TurnBack();
        }

        private void FlyBack()
        {
            Vector2 goal = (Vector2)owner.position + Vector2.up * catchHeight;
            Vector2 toGoal = goal - (Vector2)transform.position;
            float step = speed * 1.3f * Time.deltaTime;

            if (toGoal.magnitude <= step + 0.3f)
            {
                Destroy(gameObject);
                return;
            }
            transform.position += (Vector3)(toGoal.normalized * step);
        }

        private void TurnBack()
        {
            returning = true;
            scanner.Reset(); // enemies hit on the way out can be hit again on the way back
        }

        // Solid scenery (anything that is neither the owner nor something that can be hurt) stops the outward flight.
        private bool HitsWall()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius * 0.5f);
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
