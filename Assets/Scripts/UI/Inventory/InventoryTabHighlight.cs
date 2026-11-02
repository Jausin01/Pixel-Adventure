using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Colors a tab's background differently while its category is the active one.
    /// </summary>
    public class InventoryTabHighlight : MonoBehaviour
    {
        public InventoryBrowser browser;
        public Image background;
        public bool showAll;
        public ItemCategory category;
        public Color normalColor = new Color(0.25f, 0.18f, 0.14f, 1f);
        public Color activeColor = new Color(0.55f, 0.38f, 0.2f, 1f);

        private void OnEnable()
        {
            browser.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            browser.Changed -= Refresh;
        }

        private void Refresh()
        {
            background.color = browser.IsActive(showAll, category) ? activeColor : normalColor;
        }
    }
}
