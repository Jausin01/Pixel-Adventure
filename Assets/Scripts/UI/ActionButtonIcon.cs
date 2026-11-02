using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the icon of the tool the action button will use. The icon is hidden when the button attacks.
    /// </summary>
    public class ActionButtonIcon : MonoBehaviour
    {
        public NearbyHarvestDetector detector;
        public Image icon;
        public Sprite axeIcon;
        public Sprite pickaxeIcon;
        public Sprite shovelIcon;

        private void OnEnable()
        {
            detector.ToolChanged += Apply;
            Apply(detector.CurrentTool);
        }

        private void OnDisable()
        {
            detector.ToolChanged -= Apply;
        }

        private void Apply(ToolType tool)
        {
            Sprite sprite = null;
            switch (tool)
            {
                case ToolType.Axe: sprite = axeIcon; break;
                case ToolType.Pickaxe: sprite = pickaxeIcon; break;
                case ToolType.Shovel: sprite = shovelIcon; break;
            }

            icon.sprite = sprite;
            icon.enabled = sprite != null;
        }
    }
}
