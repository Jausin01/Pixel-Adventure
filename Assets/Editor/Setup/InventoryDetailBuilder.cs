using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the detail panel on the right of the inventory window: large icon, name, tier,
    /// category, description, effect and the Use button.
    /// </summary>
    public static class InventoryDetailBuilder
    {
        private static readonly Vector2 Position = new Vector2(872f, -90f);
        private static readonly Vector2 PanelSize = new Vector2(288f, 530f);
        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);

        /// <summary>Builds the panel.</summary>
        public static void Build(Transform frame, InventoryBrowser browser, ItemUser itemUser, QuickItem quick)
        {
            Image panel = UiFactory.CreateImage("Detail Panel", frame, null, TopLeft, Position, PanelSize);
            panel.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            GameObject content = new GameObject("Content", typeof(RectTransform));
            content.transform.SetParent(panel.transform, false);
            Stretch((RectTransform)content.transform);

            InventoryDetailPanel detail = panel.gameObject.AddComponent<InventoryDetailPanel>();
            detail.browser = browser;
            detail.itemUser = itemUser;
            detail.content = content;

            Text hint = UiFactory.CreateText("Empty Hint", panel.transform, "Select an item", 28, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, PanelSize);
            hint.color = new Color(1f, 1f, 1f, 0.5f);
            detail.emptyHint = hint.gameObject;

            Image slot = UiFactory.CreateImage("Icon Frame", content.transform, SpriteLoader.Load("Hud/slot"), TopLeft, new Vector2(48f, -20f), new Vector2(192f, 192f));
            slot.raycastTarget = false;
            detail.icon = CreateIcon(slot.transform);

            detail.nameText = CreateText(content.transform, "Name", 32, new Vector2(16f, -224f), new Vector2(256f, 40f));
            detail.tierText = CreateText(content.transform, "Tier", 26, new Vector2(16f, -262f), new Vector2(256f, 32f));
            detail.infoText = CreateText(content.transform, "Info", 22, new Vector2(16f, -294f), new Vector2(256f, 30f));
            detail.infoText.color = new Color(1f, 1f, 1f, 0.7f);
            detail.descriptionText = CreateText(content.transform, "Description", 22, new Vector2(16f, -330f), new Vector2(256f, 100f));
            detail.descriptionText.alignment = TextAnchor.UpperLeft;
            detail.effectText = CreateText(content.transform, "Effect", 24, new Vector2(16f, -432f), new Vector2(256f, 90f));
            detail.effectText.alignment = TextAnchor.UpperLeft;
            detail.effectText.color = new Color(0.6f, 1f, 0.6f, 1f);

            detail.useButton = BuildUseButton(content.transform, browser, itemUser);
            detail.quickButton = BuildQuickButton(content.transform, browser, quick);
        }

        private static Image CreateIcon(Transform parent)
        {
            Image icon = UiFactory.CreateImage("Icon", parent, null, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(150f, 150f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
            return icon;
        }

        private static Text CreateText(Transform parent, string name, int size, Vector2 position, Vector2 area)
        {
            return UiFactory.CreateText(name, parent, string.Empty, size, TextAnchor.MiddleLeft, TopLeft, position, area);
        }

        private static GameObject BuildUseButton(Transform parent, InventoryBrowser browser, ItemUser itemUser)
        {
            Image button = UiFactory.CreateImage("Use Button", parent, null, new Vector2(0.5f, 0f), new Vector2(-69f, 16f), new Vector2(130f, 48f));
            button.color = new Color(0.6f, 0.45f, 0.15f, 1f);

            UiFactory.CreateText("Label", button.transform, "Use", 28, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(130f, 48f));

            UseItemButton action = button.gameObject.AddComponent<UseItemButton>();
            action.browser = browser;
            action.itemUser = itemUser;
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button.gameObject;
        }

        private static GameObject BuildQuickButton(Transform parent, InventoryBrowser browser, QuickItem quick)
        {
            Image button = UiFactory.CreateImage("Quick Button", parent, null, new Vector2(0.5f, 0f), new Vector2(69f, 16f), new Vector2(130f, 48f));
            button.color = new Color(0.25f, 0.45f, 0.6f, 1f);

            UiFactory.CreateText("Label", button.transform, "Quick", 28, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(130f, 48f));

            SetQuickItemButton action = button.gameObject.AddComponent<SetQuickItemButton>();
            action.browser = browser;
            action.quick = quick;
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button.gameObject;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
