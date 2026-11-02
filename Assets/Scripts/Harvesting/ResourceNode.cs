using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Makes an object harvestable with one specific tool. Forwards correct hits to <see cref="ResourceDurability"/>.
    /// </summary>
    [RequireComponent(typeof(ResourceDurability))]
    public class ResourceNode : MonoBehaviour, IHarvestable
    {
        [Tooltip("The only tool that works on this resource.")]
        public ToolType requiredTool = ToolType.Axe;

        private ResourceDurability durability;

        /// <inheritdoc/>
        public ToolType RequiredTool { get { return requiredTool; } }

        /// <inheritdoc/>
        public bool IsDepleted { get { return durability.IsDepleted; } }

        /// <inheritdoc/>
        public Vector2 Position { get { return transform.position; } }

        private void Awake()
        {
            durability = GetComponent<ResourceDurability>();
        }

        /// <inheritdoc/>
        public void Harvest(ToolType tool, float power, Vector2 source)
        {
            if (tool != requiredTool) return;
            durability.ApplyHit(power, source);
        }
    }
}
