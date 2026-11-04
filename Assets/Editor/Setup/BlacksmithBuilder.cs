using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the blacksmith in the town and the screen where gear is upgraded and reforged.
    /// The list of the player's gear is on the left, the selected piece with its costs and buttons on the right.
    /// Needs the inventory, equipment and items to exist already.
    /// </summary>
    public static class BlacksmithBuilder
    {
        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);
        private static readonly Vector2 RowSize = new Vector2(560f, 88f);
        private static readonly Color RowColor = new Color(0.22f, 0.16f, 0.13f, 1f);
        private static readonly Color ButtonColor = new Color(0.45f, 0.3f, 0.15f, 1f);

        /// <summary>Builds the blacksmith and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player)
        {
            Blacksmith blacksmith = player.AddComponent<Blacksmith>();
            blacksmith.currency = ItemAssetFactory.Load(ProjectConstants.CurrencyId);

            InventoryPanel screen = BuildScreen(canvas, player, blacksmith);
            BuildNpc(material, screen);
            return screen;
        }

        private static void BuildNpc(Material material, InventoryPanel screen)
        {
            GameObject npc = new GameObject("Blacksmith");
            npc.transform.position = TownLayout.Blacksmith;

            SpriteRenderer renderer = npc.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/blacksmith");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            CircleCollider2D trigger = npc.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;

            Interactable interactable = npc.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = SpriteLoader.Load("Tools/pickaxe_0");
        }

        private static InventoryPanel BuildScreen(Transform canvas, GameObject player, Blacksmith blacksmith)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Blacksmith Screen", "Blacksmith", out root, out panel);

            SmithBrowser browser = root.AddComponent<SmithBrowser>();
            browser.inventory = player.GetComponent<Inventory>();
            browser.equipment = player.GetComponent<Equipment>();

            BuildList(frame, root, browser);
            BuildDetails(frame, root, browser, blacksmith, player.GetComponent<Inventory>());
            return panel;
        }

        private static void BuildList(Transform frame, GameObject root, SmithBrowser browser)
        {
            SmithList list = root.AddComponent<SmithList>();
            list.browser = browser;
            list.rows = new SmithRowView[SmithBrowser.PageSize];

            for (int i = 0; i < SmithBrowser.PageSize; i++) list.rows[i] = BuildRow(frame, browser, i);

            Text empty = UiFactory.CreateText("Empty Hint", frame, "You have no gear yet.", 30, TextAnchor.UpperLeft, TopLeft, new Vector2(40f, -100f), new Vector2(520f, 50f));
            empty.color = new Color(1f, 1f, 1f, 0.6f);
            list.emptyHint = empty.gameObject;

            float y = -84f - SmithBrowser.PageSize * (RowSize.y + 6f);
            BuildPageButton(frame, browser, "<", -1, new Vector2(24f, y));
            list.pageText = UiFactory.CreateText("Page", frame, "1/1", 30, TextAnchor.MiddleCenter, TopLeft, new Vector2(24f + 80f, y), new Vector2(400f, 56f));
            BuildPageButton(frame, browser, ">", 1, new Vector2(24f + 480f, y));
        }

        private static SmithRowView BuildRow(Transform frame, SmithBrowser browser, int index)
        {
            Vector2 position = new Vector2(24f, -84f - index * (RowSize.y + 6f));
            Image background = UiFactory.CreateImage("Piece Row " + index, frame, null, TopLeft, position, RowSize);
            background.color = RowColor;

            Outline outline = background.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 0.9f, 0.4f, 1f);
            outline.effectDistance = new Vector2(4f, -4f);
            outline.enabled = false;

            Image icon = UiFactory.CreateImage("Icon", background.transform, null, new Vector2(0f, 0.5f), new Vector2(12f, 0f), new Vector2(64f, 64f));
            icon.preserveAspect = true;
            icon.raycastTarget = false;

            Text name = UiFactory.CreateText("Name", background.transform, string.Empty, 30, TextAnchor.MiddleLeft, TopLeft, new Vector2(88f, -8f), new Vector2(460f, 42f));
            Text sub = UiFactory.CreateText("Sub", background.transform, string.Empty, 24, TextAnchor.MiddleLeft, TopLeft, new Vector2(88f, -52f), new Vector2(460f, 30f));
            sub.color = new Color(1f, 1f, 1f, 0.7f);

            SmithRowView view = background.gameObject.AddComponent<SmithRowView>();
            view.icon = icon;
            view.nameText = name;
            view.subText = sub;
            view.selectionOutline = outline;

            background.gameObject.AddComponent<SmithRowSelector>().browser = browser;
            return view;
        }

        private static void BuildPageButton(Transform frame, SmithBrowser browser, string label, int direction, Vector2 position)
        {
            Image button = UiFactory.CreateImage("Page " + label, frame, null, TopLeft, position, new Vector2(80f, 56f));
            button.color = ButtonColor;
            UiFactory.CreateText("Label", button.transform, label, 32, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 56f)).raycastTarget = false;

            SmithPageButton action = button.gameObject.AddComponent<SmithPageButton>();
            action.browser = browser;
            action.direction = direction;
            button.gameObject.AddComponent<PressScaleFeedback>();
        }

        private static void BuildDetails(Transform frame, GameObject root, SmithBrowser browser, Blacksmith blacksmith, Inventory inventory)
        {
            GameObject details = new GameObject("Details", typeof(RectTransform));
            details.transform.SetParent(frame, false);
            RectTransform rect = (RectTransform)details.transform;
            rect.anchorMin = TopLeft;
            rect.anchorMax = TopLeft;
            rect.pivot = TopLeft;
            rect.anchoredPosition = new Vector2(612f, -84f);
            rect.sizeDelta = new Vector2(544f, 540f);

            Image icon = UiFactory.CreateImage("Icon", details.transform, null, TopLeft, Vector2.zero, new Vector2(96f, 96f));
            icon.preserveAspect = true;
            icon.raycastTarget = false;

            Text name = UiFactory.CreateText("Name", details.transform, string.Empty, 34, TextAnchor.MiddleLeft, TopLeft, new Vector2(112f, 0f), new Vector2(430f, 48f));
            Text info = UiFactory.CreateText("Info", details.transform, string.Empty, 24, TextAnchor.UpperLeft, TopLeft, new Vector2(112f, -52f), new Vector2(430f, 56f));
            info.color = new Color(1f, 1f, 1f, 0.75f);
            Text stats = UiFactory.CreateText("Stats", details.transform, string.Empty, 30, TextAnchor.MiddleLeft, TopLeft, new Vector2(0f, -112f), new Vector2(544f, 44f));

            Text upgrade = UiFactory.CreateText("Upgrade Cost", details.transform, string.Empty, 26, TextAnchor.UpperLeft, TopLeft, new Vector2(0f, -170f), new Vector2(544f, 80f));
            Image upgradeButton = BuildActionButton(details.transform, "Upgrade Button", "Upgrade", new Vector2(0f, -256f), 260f);

            Text reforge = UiFactory.CreateText("Reforge Cost", details.transform, string.Empty, 24, TextAnchor.UpperLeft, TopLeft, new Vector2(0f, -344f), new Vector2(544f, 100f));
            Text reforgeLabel;
            Image reforgeButton = BuildActionButton(details.transform, "Reforge Button", "Reforge", new Vector2(0f, -452f), 400f, out reforgeLabel);

            UpgradeButton upgradeAction = upgradeButton.gameObject.AddComponent<UpgradeButton>();
            upgradeAction.browser = browser;
            upgradeAction.blacksmith = blacksmith;

            ReforgeButton reforgeAction = reforgeButton.gameObject.AddComponent<ReforgeButton>();
            reforgeAction.browser = browser;
            reforgeAction.blacksmith = blacksmith;
            reforgeAction.label = reforgeLabel;

            Text hint = UiFactory.CreateText("Select Hint", frame, "Select a piece of gear.", 30, TextAnchor.UpperLeft, TopLeft, new Vector2(636f, -100f), new Vector2(500f, 50f));
            hint.color = new Color(1f, 1f, 1f, 0.6f);

            SmithDetailPanel panel = root.AddComponent<SmithDetailPanel>();
            panel.browser = browser;
            panel.blacksmith = blacksmith;
            panel.inventory = inventory;
            panel.content = details;
            panel.emptyHint = hint.gameObject;
            panel.icon = icon;
            panel.nameText = name;
            panel.infoText = info;
            panel.statsText = stats;
            panel.upgradeText = upgrade;
            panel.reforgeText = reforge;
        }

        private static Image BuildActionButton(Transform parent, string name, string label, Vector2 position, float width)
        {
            Text unused;
            return BuildActionButton(parent, name, label, position, width, out unused);
        }

        private static Image BuildActionButton(Transform parent, string name, string label, Vector2 position, float width, out Text labelText)
        {
            Image button = UiFactory.CreateImage(name, parent, null, TopLeft, position, new Vector2(width, 64f));
            button.color = ButtonColor;
            labelText = UiFactory.CreateText("Label", button.transform, label, 30, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(width, 64f));
            labelText.raycastTarget = false;
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button;
        }
    }
}
