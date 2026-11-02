using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the column of category tabs on the left of the inventory window.
    /// </summary>
    public static class InventoryTabsBuilder
    {
        private const float TabWidth = 180f;
        private const float TabHeight = 64f;
        private const float TabSpacing = 8f;
        private static readonly Vector2 Origin = new Vector2(24f, -90f);

        /// <summary>Builds one tab per category, plus "All".</summary>
        public static void Build(Transform frame, InventoryBrowser browser)
        {
            CreateTab(frame, browser, 0, "All", true, ItemCategory.Material);
            CreateTab(frame, browser, 1, "Weapons", false, ItemCategory.Weapon);
            CreateTab(frame, browser, 2, "Equipment", false, ItemCategory.Equipment);
            CreateTab(frame, browser, 3, "Consumables", false, ItemCategory.Consumable);
            CreateTab(frame, browser, 4, "Materials", false, ItemCategory.Material);
            CreateTab(frame, browser, 5, "Quest", false, ItemCategory.Quest);
        }

        private static void CreateTab(Transform frame, InventoryBrowser browser, int index, string label, bool showAll, ItemCategory category)
        {
            Vector2 position = new Vector2(Origin.x, Origin.y - index * (TabHeight + TabSpacing));
            Image background = UiFactory.CreateImage(label + " Tab", frame, null, new Vector2(0f, 1f), position, new Vector2(TabWidth, TabHeight));

            UiFactory.CreateText("Label", background.transform, label, 28, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(TabWidth, TabHeight));

            InventoryTabButton button = background.gameObject.AddComponent<InventoryTabButton>();
            button.browser = browser;
            button.showAll = showAll;
            button.category = category;

            InventoryTabHighlight highlight = background.gameObject.AddComponent<InventoryTabHighlight>();
            highlight.browser = browser;
            highlight.background = background;
            highlight.showAll = showAll;
            highlight.category = category;
        }
    }
}
