using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Chooses which sword sprite to display. The sword is shown in one of 8 directions (45 degree steps)
    /// by picking the right drawn frame and flipping it, so pixels never get rotated and stay crisp.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwordVisual : MonoBehaviour
    {
        [Tooltip("Available sword colors.")]
        public SwordSkin[] skins;

        [Tooltip("Index of the selected skin.")]
        public int skinIndex = 0;

        private SpriteRenderer spriteRenderer;
        private Vector2 direction = Vector2.up;
        private bool smear;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Refresh();
        }

        /// <summary>Selects a skin by index and redraws the sword with it.</summary>
        public void SetSkin(int index)
        {
            if (skins == null || skins.Length == 0) return;
            skinIndex = Mathf.Clamp(index, 0, skins.Length - 1);
            Refresh();
        }

        /// <summary>Shows the sword pointing along a direction.</summary>
        /// <param name="newDirection">Direction the blade should point to.</param>
        /// <param name="useSmear">True to use the stretched motion frame on diagonals.</param>
        public void Show(Vector2 newDirection, bool useSmear)
        {
            direction = newDirection;
            smear = useSmear;
            Refresh();
        }

        /// <summary>Shows the resting pose: blade pointing up.</summary>
        public void ShowIdle()
        {
            Show(Vector2.up, false);
        }

        private void Refresh()
        {
            if (skins == null || skins.Length == 0 || spriteRenderer == null) return;

            SwordSkin skin = skins[Mathf.Clamp(skinIndex, 0, skins.Length - 1)];

            // 0 right, 1 up-right, 2 up, 3 up-left, 4 left, 5 down-left, 6 down, 7 down-right
            int octant = Mathf.RoundToInt(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg / 45f);
            octant = ((octant % 8) + 8) % 8;

            Sprite sprite;
            bool flipX = false;
            bool flipY = false;
            switch (octant)
            {
                case 0: sprite = skin.side; break;
                case 1: sprite = skin.diagonal; break;
                case 2: sprite = skin.up; break;
                case 3: sprite = skin.diagonal; flipX = true; break;
                case 4: sprite = skin.side; flipX = true; break;
                case 5: sprite = skin.diagonal; flipX = true; flipY = true; break;
                case 6: sprite = skin.up; flipY = true; break;
                default: sprite = skin.diagonal; flipY = true; break;
            }

            bool isDiagonal = octant == 1 || octant == 3 || octant == 5 || octant == 7;
            if (smear && isDiagonal && skin.smear != null) sprite = skin.smear;

            spriteRenderer.sprite = sprite;
            spriteRenderer.flipX = flipX;
            spriteRenderer.flipY = flipY;
        }
    }
}
