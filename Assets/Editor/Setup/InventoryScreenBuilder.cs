using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the inventory screen: a dimmed backdrop (tap to close) and a window containing a title,
    /// a close button, category tabs, a scrolling item grid and a detail panel.
    /// The window parts are made by their own builders; this class only puts them together.
    /// </summary>
    public static class InventoryScreenBuilder
    {
        /// <summary>Size of the window in canvas units.</summary>
        public static readonly Vector2 WindowSize = new Vector2(1180f, 640f);

        /// <summary>Builds the screen. Its window starts hidden.</summary>
        public static InventoryPanel Build(Transform parent, Inventory inventory, ItemUser itemUser)
        {
            GameObject root = new GameObject("Inventory Screen", typeof(RectTransform));
            root.transform.SetParent(parent, false);
            Stretch((RectTransform)root.transform);
            InventoryPanel panel = root.AddComponent<InventoryPanel>();

            InventoryBrowser browser = root.AddComponent<InventoryBrowser>();
            browser.inventory = inventory;

            GameObject window = new GameObject("Window", typeof(RectTransform));
            window.transform.SetParent(root.transform, false);
            Stretch((RectTransform)window.transform);
            panel.window = window;

            BuildBackdrop(window.transform, panel);
            Transform frame = BuildFrame(window.transform);
            BuildTitle(frame);
            BuildCloseButton(frame, panel);

            InventoryTabsBuilder.Build(frame, browser);
            InventoryGridBuilder.Build(frame, browser, inventory.capacity);
            InventoryDetailBuilder.Build(frame, browser, itemUser);
            return panel;
        }

        private static void BuildBackdrop(Transform parent, InventoryPanel panel)
        {
            GameObject go = new GameObject("Backdrop", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Stretch((RectTransform)go.transform);
            go.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.65f);
            go.AddComponent<CloseInventoryButton>().panel = panel;
        }

        private static Transform BuildFrame(Transform parent)
        {
            Image frame = UiFactory.CreateImage("Frame", parent, null, new Vector2(0.5f, 0.5f), Vector2.zero, WindowSize);
            frame.color = new Color(0.14f, 0.1f, 0.08f, 0.98f);
            return frame.transform;
        }

        private static void BuildTitle(Transform frame)
        {
            UiFactory.CreateText("Title", frame, "Inventory", 42, TextAnchor.MiddleLeft,
                new Vector2(0f, 1f), new Vector2(24f, -12f), new Vector2(400f, 64f));
        }

        private static void BuildCloseButton(Transform frame, InventoryPanel panel)
        {
            Image close = UiFactory.CreateImage("Close Button", frame, SpriteLoader.Load("Hud/close"), new Vector2(1f, 1f),
                new Vector2(-16f, -8f), new Vector2(64f, 64f));
            close.gameObject.AddComponent<CloseInventoryButton>().panel = panel;
            close.gameObject.AddComponent<PressScaleFeedback>();
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
