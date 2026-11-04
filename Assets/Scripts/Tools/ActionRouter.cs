using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Decides what the action button does: use a tool when a matching resource is nearby, use a nearby portal or board, otherwise attack.
    /// </summary>
    public class ActionRouter : MonoBehaviour
    {
        public WeaponHolder weapons;
        public ToolHolder tools;
        public NearbyHarvestDetector detector;
        public NearbyInteractableDetector interactions;

        /// <summary>Performs the action that fits the current surroundings.</summary>
        public void Perform()
        {
            ToolType tool = detector.CurrentTool;
            if (tool != ToolType.None && tools.Use(tool)) return;

            Interactable interactable = interactions.Current;
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            weapons.Attack();
        }
    }
}
