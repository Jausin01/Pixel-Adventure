using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Spawns item pickups when a resource is fully harvested.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class ResourceDropper : MonoBehaviour
    {
        [Tooltip("Possible drops.")]
        public DropEntry[] drops;

        [Tooltip("Material used by the spawned pickups.")]
        public Material material;

        [Tooltip("Height above the object's base where drops appear.")]
        public float spawnHeight = 0.3f;

        private ResourceDurability durability;

        private void Awake()
        {
            durability = GetComponent<ResourceDurability>();
        }

        private void OnEnable()
        {
            durability.Depleted += OnDepleted;
        }

        private void OnDisable()
        {
            durability.Depleted -= OnDepleted;
        }

        private void OnDepleted()
        {
            DropRoller.Roll(drops, (Vector2)transform.position + Vector2.up * spawnHeight, material);
        }
    }
}
