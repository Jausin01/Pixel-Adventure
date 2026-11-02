using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the menu in the top-right corner: one gear button that expands to the left into
    /// the inventory, weapon swap and a spare button.
    /// </summary>
    public static class MenuBuilder
    {
        private const float Margin = 30f;
        private const float ButtonSize = 90f;
        private const float IconSize = 60f;
        private const float Spacing = 100f;

        /// <summary>Builds the menu.</summary>
        public static void Build(Transform parent, WeaponHolder weapons, InventoryPanel inventory)
        {
            Sprite circle = SpriteLoader.Load("UI/button_circle");
            Vector2 position = new Vector2(-Margin, -Margin);

            // Entries are created first so the gear button is drawn on top of them.
            Image bag = CreateButton("Inventory Button", parent, circle, "Hud/bag", position);
            bag.gameObject.AddComponent<OpenInventoryButton>().panel = inventory;

            Image swap = CreateButton("Swap Weapon Button", parent, circle, "Bow/bow_idle", position);
            swap.gameObject.AddComponent<SwapWeaponButton>().weapons = weapons;

            Image spare = CreateButton("Spare Button", parent, circle, "Hud/slot", position);
            spare.color = new Color(1f, 1f, 1f, 0.45f);

            Image gear = CreateButton("Menu Button", parent, circle, "Hud/gear", position);
            MenuExpander expander = gear.gameObject.AddComponent<MenuExpander>();
            expander.entries = new[] { (RectTransform)bag.transform, (RectTransform)swap.transform, (RectTransform)spare.transform };
            expander.spacing = Spacing;
            gear.gameObject.AddComponent<MenuToggleButton>().menu = expander;
        }

        private static Image CreateButton(string name, Transform parent, Sprite circle, string iconPath, Vector2 position)
        {
            return UiFactory.CreateIconButton(name, parent, circle, SpriteLoader.Load(iconPath), new Vector2(1f, 1f), position, ButtonSize, IconSize);
        }
    }
}
