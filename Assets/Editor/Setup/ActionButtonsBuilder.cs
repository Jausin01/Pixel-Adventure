using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the attack, sword color and weapon swap buttons on the right side of the screen.
    /// </summary>
    public static class ActionButtonsBuilder
    {
        // Skin indexes (into the sword skin list) offered as quick buttons: silver, blue, red, green.
        private static readonly int[] SkinIndexes = { 0, 1, 2, 4 };
        private static readonly Color[] SkinColors =
        {
            new Color(0.85f, 0.85f, 0.9f), new Color(0.4f, 0.6f, 1f), new Color(1f, 0.4f, 0.4f), new Color(0.4f, 0.9f, 0.5f)
        };
        private static readonly Vector2[] SkinPositions =
        {
            new Vector2(-335f, 105f), new Vector2(-305f, 235f), new Vector2(-215f, 325f), new Vector2(-90f, 355f)
        };

        /// <summary>Builds all buttons.</summary>
        public static void Build(Transform canvas, WeaponHolder weapons, SwordVisual sword)
        {
            Sprite circle = SpriteLoader.Load("UI/button_circle");
            Vector2 bottomRight = new Vector2(1f, 0f);
            Vector2 topRight = new Vector2(1f, 1f);

            Image attack = UiFactory.CreateImage("Attack Button", canvas, circle, bottomRight, new Vector2(-135f, 135f), new Vector2(190f, 190f));
            attack.color = new Color(1f, 0.55f, 0.45f, 0.85f);
            attack.gameObject.AddComponent<AttackButton>().weapons = weapons;
            attack.gameObject.AddComponent<PressScaleFeedback>();

            for (int i = 0; i < SkinIndexes.Length; i++)
            {
                Image button = UiFactory.CreateImage("Skin Button " + i, canvas, circle, bottomRight, SkinPositions[i], new Vector2(110f, 110f));
                button.color = SkinColors[i];
                SwordSkinButton action = button.gameObject.AddComponent<SwordSkinButton>();
                action.sword = sword;
                action.skinIndex = SkinIndexes[i];
                button.gameObject.AddComponent<PressScaleFeedback>();
            }

            Image swap = UiFactory.CreateImage("Swap Weapon Button", canvas, circle, topRight, new Vector2(-95f, -95f), new Vector2(120f, 120f));
            swap.color = new Color(1f, 1f, 1f, 0.85f);
            swap.gameObject.AddComponent<SwapWeaponButton>().weapons = weapons;
            swap.gameObject.AddComponent<PressScaleFeedback>();

            Image icon = UiFactory.CreateImage("Icon", swap.transform, SpriteLoader.Load("Bow/bow_idle"), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 80f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
        }
    }
}
