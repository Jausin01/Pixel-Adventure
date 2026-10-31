using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Creates arrow projectiles. Holds the arrow's appearance and stats.
    /// </summary>
    public class ArrowSpawner : MonoBehaviour
    {
        [Header("Appearance")]
        public Sprite arrowSprite;
        public Material material;
        public int sortingOrder = 9;

        [Header("Stats")]
        public float damage = 8f;
        public float speed = 11f;
        public float lifetime = 1.4f;
        public float impactRadius = 0.18f;

        [Header("Spawning")]
        [Tooltip("Colliders under this transform (the shooter) are ignored by the arrows.")]
        public Transform shooter;
        [Tooltip("Distance in front of the origin where the arrow appears.")]
        public float spawnDistance = 0.45f;

        /// <summary>Spawns one arrow flying along a direction.</summary>
        /// <param name="origin">World position the shot comes from.</param>
        /// <param name="direction">Unit direction of the shot.</param>
        public void Fire(Vector2 origin, Vector2 direction)
        {
            GameObject arrow = new GameObject("Arrow", typeof(SpriteRenderer), typeof(ProjectileMotion), typeof(ProjectileImpact));

            SpriteRenderer sprite = arrow.GetComponent<SpriteRenderer>();
            sprite.sprite = arrowSprite;
            sprite.sharedMaterial = material;
            sprite.sortingOrder = sortingOrder;

            Vector2 position = origin + direction * spawnDistance;
            arrow.transform.position = new Vector3(position.x, position.y, 0f);

            arrow.GetComponent<ProjectileImpact>().Configure(damage, impactRadius, shooter);
            arrow.GetComponent<ProjectileMotion>().Launch(direction, speed, lifetime);
        }
    }
}
