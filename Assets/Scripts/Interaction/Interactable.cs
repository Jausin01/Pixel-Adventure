using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Something in the world the player can use with the action button when standing next to it
    /// (a portal, a quest board). Opens a screen. Needs a trigger collider on the same object.
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        [Tooltip("The screen opened by the interaction.")]
        public InventoryPanel screen;

        [Tooltip("Icon shown on the action button while the player is close enough.")]
        public Sprite icon;

        /// <summary>Performs the interaction.</summary>
        public void Interact()
        {
            screen.Open();
        }
    }
}
