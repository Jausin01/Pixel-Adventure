using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Drops items when the attached <see cref="Health"/> dies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyLootDropper : MonoBehaviour
    {
        [Tooltip("Possible drops.")]
        public DropEntry[] drops;

        [Tooltip("Material used by the spawned pickups.")]
        public Material material;

        [Tooltip("Height above the enemy's feet where drops appear.")]
        public float spawnHeight = 0.3f;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable() { health.Died += OnDied; }

        private void OnDisable() { health.Died -= OnDied; }

        private void OnDied()
        {
            DropRoller.Roll(drops, (Vector2)transform.position + Vector2.up * spawnHeight, material);
        }
    }
}
