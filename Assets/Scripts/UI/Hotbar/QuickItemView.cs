using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws the quick item button: the item's icon and how many are left, dimmed when it cannot be used.
    /// </summary>
    public class QuickItemView : MonoBehaviour
    {
        public QuickItem quick;
        public Image icon;
        public Text countText;

        private void Update()
        {
            ItemDefinition item = quick.Item;
            icon.enabled = item != null;
            countText.enabled = item != null;
            if (item == null) return;

            icon.sprite = item.icon;
            icon.color = quick.CanUse ? Color.white : new Color(0.55f, 0.55f, 0.55f, 1f);
            countText.text = quick.Count.ToString();
        }
    }
}
