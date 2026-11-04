using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The sprites a single-sprite weapon (axe, spear) can wear, one per metal. The worn item picks which one is shown.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponSkinSet : MonoBehaviour
    {
        [Tooltip("The sprite of each skin, pointing right.")]
        public Sprite[] skins;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>Shows a skin by index. Out of range values use the closest skin.</summary>
        public void SetSkin(int index)
        {
            if (skins == null || skins.Length == 0) return;
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = skins[Mathf.Clamp(index, 0, skins.Length - 1)];
        }
    }
}
