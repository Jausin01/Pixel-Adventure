using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the icon of what the action button will do: the tool for a nearby resource, or the symbol of a nearby portal or board.
    /// The icon is hidden when the button attacks.
    /// </summary>
    public class ActionButtonIcon : MonoBehaviour
    {
        public NearbyHarvestDetector detector;
        public NearbyInteractableDetector interactions;
        public Image icon;
        public Sprite axeIcon;
        public Sprite pickaxeIcon;
        public Sprite shovelIcon;

        private void OnEnable()
        {
            detector.ToolChanged += OnToolChanged;
            interactions.CurrentChanged += OnInteractableChanged;
            Refresh();
        }

        private void OnDisable()
        {
            detector.ToolChanged -= OnToolChanged;
            interactions.CurrentChanged -= OnInteractableChanged;
        }

        private void OnToolChanged(ToolType tool) { Refresh(); }

        private void OnInteractableChanged(Interactable interactable) { Refresh(); }

        // Tools win over interactables, matching the order used by ActionRouter.
        private void Refresh()
        {
            Sprite sprite = null;
            switch (detector.CurrentTool)
            {
                case ToolType.Axe: sprite = axeIcon; break;
                case ToolType.Pickaxe: sprite = pickaxeIcon; break;
                case ToolType.Shovel: sprite = shovelIcon; break;
            }

            if (sprite == null && interactions.Current != null) sprite = interactions.Current.icon;

            icon.sprite = sprite;
            icon.enabled = sprite != null;
        }
    }
}
