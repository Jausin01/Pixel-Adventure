using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the scrolling grid of item slots in the middle of the inventory window.
    /// </summary>
    public static class InventoryGridBuilder
    {
        private const int Columns = 6;
        private const float SlotSize = 96f;
        private const float SlotSpacing = 8f;
        private static readonly Vector2 Position = new Vector2(224f, -90f);
        private static readonly Vector2 ViewportSize = new Vector2(628f, 530f);

        /// <summary>Builds the scroll area with <paramref name="slotCount"/> slots.</summary>
        public static void Build(Transform frame, InventoryBrowser browser, int slotCount)
        {
            RectTransform viewport = CreateViewport(frame);
            RectTransform content = CreateContent(viewport);
            CreateScroll(frame, viewport, content);

            Sprite slotSprite = SpriteLoader.Load("Hud/slot");
            InventorySlotView[] views = new InventorySlotView[slotCount];
            for (int i = 0; i < slotCount; i++) views[i] = CreateSlot(content, slotSprite, browser);

            InventoryGrid grid = content.gameObject.AddComponent<InventoryGrid>();
            grid.browser = browser;
            grid.slotViews = views;
        }

        private static RectTransform CreateViewport(Transform frame)
        {
            // Nearly transparent but raycastable, so dragging on the gaps between slots still scrolls.
            Image viewport = UiFactory.CreateImage("Viewport", frame, null, new Vector2(0f, 1f), Position, ViewportSize);
            viewport.color = new Color(0f, 0f, 0f, 0.004f);
            viewport.gameObject.AddComponent<RectMask2D>();
            return viewport.rectTransform;
        }

        private static RectTransform CreateContent(RectTransform viewport)
        {
            GameObject go = new GameObject("Content", typeof(RectTransform), typeof(GridLayoutGroup), typeof(ContentSizeFitter));
            go.transform.SetParent(viewport, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

            GridLayoutGroup layout = go.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(SlotSize, SlotSize);
            layout.spacing = new Vector2(SlotSpacing, SlotSpacing);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = Columns;
            layout.childAlignment = TextAnchor.UpperLeft;

            go.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return rect;
        }

        private static void CreateScroll(Transform frame, RectTransform viewport, RectTransform content)
        {
            ScrollRect scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 30f;
        }

        private static InventorySlotView CreateSlot(Transform parent, Sprite slotSprite, InventoryBrowser browser)
        {
            Vector2 center = new Vector2(0.5f, 0.5f);
            Image background = UiFactory.CreateImage("Slot", parent, slotSprite, center, Vector2.zero, new Vector2(SlotSize, SlotSize));

            Outline outline = background.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 0.85f, 0.3f, 1f);
            outline.effectDistance = new Vector2(4f, -4f);
            outline.enabled = false;

            Image icon = UiFactory.CreateImage("Icon", background.transform, null, center, Vector2.zero, new Vector2(SlotSize - 24f, SlotSize - 24f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
            icon.enabled = false;

            Text count = UiFactory.CreateText("Count", background.transform, string.Empty, 26, TextAnchor.LowerRight,
                new Vector2(1f, 0f), new Vector2(-6f, 4f), new Vector2(90f, 34f));

            InventorySlotView view = background.gameObject.AddComponent<InventorySlotView>();
            view.background = background;
            view.icon = icon;
            view.countText = count;
            view.selectionOutline = outline;

            background.gameObject.AddComponent<InventorySlotSelector>().browser = browser;
            return view;
        }
    }
}
