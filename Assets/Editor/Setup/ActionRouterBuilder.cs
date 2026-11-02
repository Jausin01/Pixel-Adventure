using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the resource detector on the player and the router that decides between weapon and tool.
    /// </summary>
    public static class ActionRouterBuilder
    {
        /// <summary>Builds the router.</summary>
        public static ActionRouter Build(GameObject player, WeaponHolder weapons, ToolHolder tools)
        {
            NearbyHarvestDetector detector = player.AddComponent<NearbyHarvestDetector>();

            GameObject go = new GameObject("Action Router");
            ActionRouter router = go.AddComponent<ActionRouter>();
            router.weapons = weapons;
            router.tools = tools;
            router.detector = detector;
            return router;
        }
    }
}
