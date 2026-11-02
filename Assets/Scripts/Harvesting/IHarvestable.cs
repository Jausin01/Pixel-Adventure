using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Implemented by world objects that tools can work on (trees, ore, chests).
    /// Deliberately separate from <see cref="IDamageable"/> so weapons never hit them.
    /// </summary>
    public interface IHarvestable
    {
        /// <summary>The tool this object responds to.</summary>
        ToolType RequiredTool { get; }

        /// <summary>True when there is nothing left to harvest.</summary>
        bool IsDepleted { get; }

        /// <summary>World position of the object.</summary>
        Vector2 Position { get; }

        /// <summary>Works on the object with a tool. A wrong tool has no effect.</summary>
        /// <param name="tool">Tool being used.</param>
        /// <param name="power">Amount of durability removed by a correct tool.</param>
        /// <param name="source">World position the hit comes from.</param>
        void Harvest(ToolType tool, float power, Vector2 source);
    }
}
