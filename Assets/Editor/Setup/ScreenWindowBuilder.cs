using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the shared shell of a full-screen menu: a dimmed backdrop (tap to close), a framed window,
    /// a title and a close button. Menus put their own content inside the returned frame.
    /// </summary>
    public static class ScreenWindowBuilder
    {
        /// <summary>Size of the window in canvas units.</summary>
        public static readonly Vector2 WindowSize = new Vector2(1180f, 640f);

        /// <summary>Builds the shell. The window starts hidden (the panel hides it at start).</summary>
        /// <param name="root">The always-active root object; extra always-active components belong here.</param>
        /// <param name="panel">The component that opens and closes the window.</param>
        /// <returns>The frame to put content in.</returns>
        public static Transform Build(Transform parent, string name, string title, out GameObject root, out InventoryPanel panel)
        {
            root = new GameObject(name, typeof(RectTransform));
            root.transform.SetParent(parent, false);
            Stretch((RectTransform)root.transform);
            panel = root.AddComponent<InventoryPanel>();

            GameObject window = new GameObject("Window", typeof(RectTransform));
            window.transform.SetParent(root.transform, false);
            Stretch((RectTransform)window.transform);
            panel.window = window;

            BuildBackdrop(window.transform, panel);
            Transform frame = BuildFrame(window.transform);
            UiFactory.CreateText("Title", frame, title, 42, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(24f, -12f), new Vector2(400f, 64f));
            BuildCloseButton(frame, panel);
            return frame;
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
            frame.gameObject.AddComponent<ScaleToFit>().designSize = WindowSize;
            return frame.transform;
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
