using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Disables the resource's colliders while it is depleted, so the player can walk through the stump.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class ResourceColliderToggle : MonoBehaviour
    {
        private ResourceDurability durability;
        private Collider2D[] colliders;

        private void Awake()
        {
            durability = GetComponent<ResourceDurability>();
            colliders = GetComponentsInChildren<Collider2D>();
        }

        private void OnEnable()
        {
            durability.Depleted += OnDepleted;
            durability.Restored += OnRestored;
        }

        private void OnDisable()
        {
            durability.Depleted -= OnDepleted;
            durability.Restored -= OnRestored;
        }

        private void OnDepleted() { SetEnabled(false); }

        private void OnRestored() { SetEnabled(true); }

        private void SetEnabled(bool value)
        {
            for (int i = 0; i < colliders.Length; i++) colliders[i].enabled = value;
        }
    }
}
