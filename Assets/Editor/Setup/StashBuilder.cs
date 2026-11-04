using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the stash chest in the town and its screen: the backpack on the left, the stash on the right.
    /// Tapping a slot sends it to the other side. The stash is a second inventory with a larger capacity.
    /// </summary>
    public static class StashBuilder
    {
        private const int StashCapacity = 120;
        private const int Columns = 6;
        private const int Rows = 5;
        private const float CellSize = 84f;
        private const float CellSpacing = 6f;

        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);
        private static readonly Vector2 GridSize = new Vector2(Columns * CellSize + (Columns - 1) * CellSpacing, Rows * CellSize + (Rows - 1) * CellSpacing);
        private static readonly Color ButtonColor = new Color(0.45f, 0.3f, 0.15f, 1f);

        /// <summary>Creates the storage the chest opens. It lives on its own object, apart from the player's backpack.</summary>
        public static Inventory CreateStorage()
        {
            Inventory stash = new GameObject("Stash Storage").AddComponent<Inventory>();
            stash.capacity = StashCapacity;
            return stash;
        }

        /// <summary>Builds the chest and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player, Inventory stash)
        {
            InventoryPanel screen = BuildScreen(canvas, player.GetComponent<Inventory>(), stash);
            BuildChest(material, screen);
            return screen;
        }

        private static void BuildChest(Material material, InventoryPanel screen)
        {
            GameObject chest = new GameObject("Stash Chest");
            chest.transform.position = TownLayout.Stash;

            SpriteRenderer renderer = chest.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/stash_chest");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            CircleCollider2D trigger = chest.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;

            Interactable interactable = chest.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = SpriteLoader.Load("World/stash_chest");
        }

        private static InventoryPanel BuildScreen(Transform canvas, Inventory backpack, Inventory stash)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Stash Screen", "Stash", out root, out panel);

            BuildPane(frame, root, "Backpack", backpack, stash, 24f);
            BuildPane(frame, root, "Stash", stash, backpack, 24f + GridSize.x + 64f);
            return panel;
        }

        private static void BuildPane(Transform frame, GameObject root, string label, Inventory source, Inventory other, float x)
        {
            GameObject paneObject = new GameObject(label + " Pane");
            paneObject.transform.SetParent(root.transform, false);
            StashPane pane = paneObject.AddComponent<StashPane>();
            pane.source = source;
            pane.other = other;
            pane.label = label;

            pane.titleText = UiFactory.CreateText(label + " Title", frame, label, 32, TextAnchor.MiddleLeft, TopLeft, new Vector2(x, -72f), new Vector2(GridSize.x, 44f));
            pane.cells = BuildGrid(frame, pane, x);

            float buttonsY = -126f - GridSize.y - 8f;
            BuildPageButton(frame, pane, "<", -1, new Vector2(x, buttonsY));
            pane.pageText = UiFactory.CreateText(label + " Page", frame, "1/1", 30, TextAnchor.MiddleCenter, TopLeft, new Vector2(x + 80f, buttonsY), new Vector2(GridSize.x - 160f, 56f));
            BuildPageButton(frame, pane, ">", 1, new Vector2(x + GridSize.x - 80f, buttonsY));
        }

        private static InventorySlotView[] BuildGrid(Transform frame, StashPane pane, float x)
        {
            GameObject grid = new GameObject("Grid", typeof(RectTransform), typeof(GridLayoutGroup));
            grid.transform.SetParent(frame, false);

            RectTransform rect = (RectTransform)grid.transform;
            rect.anchorMin = TopLeft;
            rect.anchorMax = TopLeft;
            rect.pivot = TopLeft;
            rect.anchoredPosition = new Vector2(x, -122f);
            rect.sizeDelta = GridSize;

            GridLayoutGroup layout = grid.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(CellSize, CellSize);
            layout.spacing = new Vector2(CellSpacing, CellSpacing);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = Columns;
            layout.childAlignment = TextAnchor.UpperLeft;

            Sprite slotSprite = SpriteLoader.Load("Hud/slot");
            InventorySlotView[] cells = new InventorySlotView[Columns * Rows];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = InventoryGridBuilder.CreateSlotView(grid.transform, slotSprite, CellSize);
                cells[i].gameObject.AddComponent<StashCellSelector>().pane = pane;
            }
            return cells;
        }

        private static void BuildPageButton(Transform frame, StashPane pane, string label, int direction, Vector2 position)
        {
            Image button = UiFactory.CreateImage("Page " + label, frame, null, TopLeft, position, new Vector2(80f, 56f));
            button.color = ButtonColor;
            UiFactory.CreateText("Label", button.transform, label, 32, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 56f)).raycastTarget = false;

            StashPageButton action = button.gameObject.AddComponent<StashPageButton>();
            action.pane = pane;
            action.direction = direction;
            button.gameObject.AddComponent<PressScaleFeedback>();
        }
    }
}
