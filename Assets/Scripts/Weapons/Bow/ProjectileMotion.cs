using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Moves an object in a straight line, rotates it to face its travel direction,
    /// and destroys it when its lifetime runs out.
    /// </summary>
    public class ProjectileMotion : MonoBehaviour
    {
        private Vector2 direction = Vector2.right;
        private float speed;
        private float remainingLife;

        /// <summary>Starts the flight.</summary>
        /// <param name="flightDirection">Travel direction.</param>
        /// <param name="flightSpeed">Speed in world units per second.</param>
        /// <param name="lifetime">Seconds before the object is destroyed.</param>
        public void Launch(Vector2 flightDirection, float flightSpeed, float lifetime)
        {
            direction = flightDirection.normalized;
            speed = flightSpeed;
            remainingLife = lifetime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            remainingLife -= Time.deltaTime;
            if (remainingLife <= 0f) Destroy(gameObject);
        }
    }
}
