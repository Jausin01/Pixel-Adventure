using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the scrolling list in the middle of the equipment window that shows the items fitting the selected slot.
    /// </summary>
    public static class EquipmentCandidateListBuilder
    {
        private const float RowWidth = 450f;
        private const float RowHeight = 72f;
        private const float RowSpacing = 6f;
        private static readonly Vector2 Position = new Vector2(380f, -90f);
        private static readonly Vector2 ViewportSize = new Vector2(RowWidth, 530f);
        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);

        /// <summary>Builds the list with one pooled row per inventory slot.</summary>
        public static void Build(Transform frame, Equipment equipment, Inventory inventory, EquipmentBrowser browser)
        {
            RectTransform viewport = CreateViewport(frame);
            RectTransform content = CreateContent(viewport);
            ScrollRect scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 30f;

            EquipmentCandidateRowView[] rows = new EquipmentCandidateRowView[inventory.capacity];
            for (int i = 0; i < rows.Length; i++) rows[i] = CreateRow(content, equipment, browser);

            Text hint = UiFactory.CreateText("Empty Hint", frame, "Nothing to equip here", 28, TextAnchor.MiddleCenter, TopLeft, Position, ViewportSize);
            hint.color = new Color(1f, 1f, 1f, 0.5f);

            EquipmentCandidateList list = content.gameObject.AddComponent<EquipmentCandidateList>();
            list.equipment = equipment;
            list.inventory = inventory;
            list.browser = browser;
            list.rows = rows;
            list.emptyHint = hint.gameObject;
        }

        private static RectTransform CreateViewport(Transform frame)
        {
            Image viewport = UiFactory.CreateImage("Candidate Viewport", frame, null, TopLeft, Position, ViewportSize);
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
            layout.cellSize = new Vector2(RowWidth, RowHeight);
            layout.spacing = new Vector2(0f, RowSpacing);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 1;

            go.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return rect;
        }

        private static EquipmentCandidateRowView CreateRow(Transform parent, Equipment equipment, EquipmentBrowser browser)
        {
            Image background = UiFactory.CreateImage("Candidate Row", parent, null, TopLeft, Vector2.zero, new Vector2(RowWidth, RowHeight));
            background.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            Image icon = UiFactory.CreateImage("Icon", background.transform, null, TopLeft, new Vector2(8f, -8f), new Vector2(56f, 56f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;

            Text nameText = UiFactory.CreateText("Name", background.transform, string.Empty, 26, TextAnchor.MiddleLeft, TopLeft, new Vector2(76f, -4f), new Vector2(366f, 34f));
            Text bonusText = UiFactory.CreateText("Bonus", background.transform, string.Empty, 22, TextAnchor.MiddleLeft, TopLeft, new Vector2(76f, -38f), new Vector2(366f, 28f));
            bonusText.color = new Color(0.6f, 1f, 0.6f, 1f);

            EquipmentCandidateRowView view = background.gameObject.AddComponent<EquipmentCandidateRowView>();
            view.icon = icon;
            view.nameText = nameText;
            view.bonusText = bonusText;

            EquipmentCandidateRowSelector selector = background.gameObject.AddComponent<EquipmentCandidateRowSelector>();
            selector.equipment = equipment;
            selector.browser = browser;
            return view;
        }
    }
}
