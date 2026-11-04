using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Draws the secondary weapon button: the worn weapon's icon, a sweeping cooldown overlay and a dimmed icon
    /// while it cannot be thrown. Hidden icon when nothing is worn.
    /// </summary>
    public class SecondaryButtonView : MonoBehaviour
    {
        public SecondaryThrower thrower;
        public Image icon;
        [Tooltip("Filled radial image that shrinks as the cooldown runs out.")]
        public Image cooldownOverlay;

        private void Update()
        {
            ItemDefinition item = thrower.Item;
            icon.enabled = item != null;
            if (item == null)
            {
                cooldownOverlay.fillAmount = 0f;
                return;
            }

            icon.sprite = item.icon;
            icon.color = thrower.CanThrow ? Color.white : new Color(0.55f, 0.55f, 0.55f, 1f);
            cooldownOverlay.fillAmount = thrower.CooldownFraction;
        }
    }
}
