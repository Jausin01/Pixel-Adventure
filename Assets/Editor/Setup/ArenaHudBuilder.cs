using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the arena interface: the wave counter at the top and the "next wave or leave" panel in the middle.
    /// </summary>
    public static class ArenaHudBuilder
    {
        private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

        /// <summary>Builds the wave counter and the decision panel.</summary>
        public static void Build(Transform parent, ArenaRun run)
        {
            WaveDirector director = run.director;
            BuildWaveCounter(parent, run, director);
            BuildDecisionPanel(parent, run, director);
            BuildGameOver(parent, run);
        }

        private static void BuildWaveCounter(Transform parent, ArenaRun run, WaveDirector director)
        {
            GameObject container = CreateContainer("Wave HUD", parent);

            Text label = UiFactory.CreateText("Wave Label", container.transform, string.Empty, 40, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 1f), new Vector2(0f, -16f), new Vector2(700f, 56f));

            WaveHud hud = container.AddComponent<WaveHud>();
            hud.run = run;
            hud.director = director;
            hud.visual = label.gameObject;
            hud.label = label;
        }

        private static void BuildDecisionPanel(Transform parent, ArenaRun run, WaveDirector director)
        {
            GameObject container = CreateContainer("Wave Decision", parent);

            Image panel = UiFactory.CreateImage("Panel", container.transform, null, Center, Vector2.zero, new Vector2(560f, 280f));
            panel.color = new Color(0.14f, 0.1f, 0.08f, 0.95f);

            Text title = UiFactory.CreateText("Title", panel.transform, string.Empty, 44, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 1f), new Vector2(0f, -24f), new Vector2(520f, 64f));

            CreateButton(panel.transform, "Next Wave", new Vector2(-135f, 20f), new Color(0.3f, 0.6f, 0.3f)).AddComponent<NextWaveButton>().run = run;
            CreateButton(panel.transform, "Leave", new Vector2(135f, 20f), new Color(0.65f, 0.3f, 0.2f)).AddComponent<LeaveArenaButton>().run = run;

            WaveDecisionPanel decision = container.AddComponent<WaveDecisionPanel>();
            decision.run = run;
            decision.director = director;
            decision.visual = panel.gameObject;
            decision.title = title;
        }

        private static void BuildGameOver(Transform parent, ArenaRun run)
        {
            GameObject container = CreateContainer("Game Over", parent);

            // Dark screen over everything, with the title, the summary and the button.
            Image screen = UiFactory.CreateImage("Screen", container.transform, null, Center, Vector2.zero, Vector2.zero);
            RectTransform rect = screen.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            screen.color = new Color(0.04f, 0.02f, 0.03f, 0.88f);
            CanvasGroup group = screen.gameObject.AddComponent<CanvasGroup>();

            Text title = UiFactory.CreateText("Title", screen.transform, "GAME OVER", 96, TextAnchor.MiddleCenter, Center, new Vector2(0f, 120f), new Vector2(900f, 130f));
            title.color = new Color(0.85f, 0.2f, 0.2f);
            Text summary = UiFactory.CreateText("Summary", screen.transform, string.Empty, 40, TextAnchor.MiddleCenter, Center, new Vector2(0f, 10f), new Vector2(700f, 120f));

            GameOverPanel panel = container.AddComponent<GameOverPanel>();
            panel.run = run;
            panel.respawn = run.player.GetComponent<PlayerRespawn>();
            panel.visual = screen.gameObject;
            panel.group = group;
            panel.summary = summary;

            Image button = UiFactory.CreateImage("Return Button", screen.transform, null, Center, new Vector2(0f, -120f), new Vector2(380f, 90f));
            button.color = new Color(0.65f, 0.3f, 0.2f);
            UiFactory.CreateText("Label", button.transform, "Return to Town", 36, TextAnchor.MiddleCenter, Center, Vector2.zero, new Vector2(380f, 90f));
            button.gameObject.AddComponent<PressScaleFeedback>();
            button.gameObject.AddComponent<ReturnToTownButton>().panel = panel;
        }

        // An invisible object that fills the screen, so children can anchor to its edges.
        private static GameObject CreateContainer(string name, Transform parent)
        {
            GameObject container = new GameObject(name, typeof(RectTransform));
            container.transform.SetParent(parent, false);

            RectTransform rect = (RectTransform)container.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return container;
        }

        private static GameObject CreateButton(Transform parent, string label, Vector2 offset, Color color)
        {
            Image button = UiFactory.CreateImage(label + " Button", parent, null, new Vector2(0.5f, 0f), offset, new Vector2(240f, 84f));
            button.color = color;
            UiFactory.CreateText("Label", button.transform, label, 34, TextAnchor.MiddleCenter, Center, Vector2.zero, new Vector2(240f, 84f));
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button.gameObject;
        }
    }
}
