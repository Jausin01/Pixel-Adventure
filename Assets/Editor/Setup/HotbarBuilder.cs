using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the two hot buttons under the menu gear: the secondary weapon and the quick item.
    /// </summary>
    public static class HotbarBuilder
    {
        private const float Margin = 30f;
        private const float MenuButtonSize = 90f;
        private const float Gap = 16f;
        private const float ButtonSize = 100f;
        private const float IconSize = 64f;

        /// <summary>Adds the quick item to the player. Run before the inventory screen is built.</summary>
        public static QuickItem AttachQuickItem(GameObject player)
        {
            return player.AddComponent<QuickItem>();
        }

        /// <summary>Builds both buttons.</summary>
        public static void Build(Transform parent, GameObject player)
        {
            Sprite circle = SpriteLoader.Load("UI/button_circle");
            float firstY = -(Margin + MenuButtonSize + Gap);
            float secondY = firstY - ButtonSize - Gap;

            BuildSecondary(parent, circle, new Vector2(-Margin, firstY), player.GetComponent<SecondaryThrower>());
            BuildQuick(parent, circle, new Vector2(-Margin, secondY), player.GetComponent<QuickItem>());
        }

        private static void BuildSecondary(Transform parent, Sprite circle, Vector2 position, SecondaryThrower thrower)
        {
            Image button = CreateButton("Secondary Button", parent, circle, position);
            button.gameObject.AddComponent<SecondaryButton>().thrower = thrower;

            Image icon = CreateIcon(button.transform);
            Image overlay = CreateCooldownOverlay(button.transform, circle);

            SecondaryButtonView view = button.gameObject.AddComponent<SecondaryButtonView>();
            view.thrower = thrower;
            view.icon = icon;
            view.cooldownOverlay = overlay;
        }

        private static void BuildQuick(Transform parent, Sprite circle, Vector2 position, QuickItem quick)
        {
            Image button = CreateButton("Quick Item Button", parent, circle, position);
            button.gameObject.AddComponent<QuickItemButton>().quick = quick;

            Image icon = CreateIcon(button.transform);
            Text count = UiFactory.CreateText("Count", button.transform, string.Empty, 30, TextAnchor.LowerRight,
                new Vector2(1f, 0f), new Vector2(-6f, 4f), new Vector2(60f, 36f));

            QuickItemView view = button.gameObject.AddComponent<QuickItemView>();
            view.quick = quick;
            view.icon = icon;
            view.countText = count;
        }

        private static Image CreateButton(string name, Transform parent, Sprite circle, Vector2 position)
        {
            Image button = UiFactory.CreateImage(name, parent, circle, new Vector2(1f, 1f), position, new Vector2(ButtonSize, ButtonSize));
            button.color = new Color(1f, 1f, 1f, 0.6f);
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button;
        }

        private static Image CreateIcon(Transform parent)
        {
            Image icon = UiFactory.CreateImage("Icon", parent, null, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(IconSize, IconSize));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
            icon.enabled = false;
            return icon;
        }

        private static Image CreateCooldownOverlay(Transform parent, Sprite circle)
        {
            Image overlay = UiFactory.CreateImage("Cooldown Overlay", parent, circle, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(ButtonSize, ButtonSize));
            overlay.color = new Color(0f, 0f, 0f, 0.65f);
            overlay.raycastTarget = false;
            overlay.type = Image.Type.Filled;
            overlay.fillMethod = Image.FillMethod.Radial360;
            overlay.fillOrigin = (int)Image.Origin360.Top;
            overlay.fillClockwise = false;
            overlay.fillAmount = 0f;
            return overlay;
        }
    }
}
