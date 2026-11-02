using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Restores a resource some time after it is depleted, so the world never runs out.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class ResourceRegrowth : MonoBehaviour
    {
        [Tooltip("Seconds between depletion and regrowth.")]
        public float delay = 30f;

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
            StartCoroutine(RestoreAfterDelay());
        }

        private IEnumerator RestoreAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            durability.Restore();
        }
    }
}
