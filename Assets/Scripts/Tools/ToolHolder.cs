using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the player's tools and uses the one that matches a requested type.
    /// </summary>
    public class ToolHolder : MonoBehaviour
    {
        public ToolSwing[] tools;

        /// <summary>Uses the tool of the given type. Returns false when the player has no such tool.</summary>
        public bool Use(ToolType type)
        {
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i].tool != type) continue;
                tools[i].Use();
                return true;
            }
            return false;
        }
    }
}
