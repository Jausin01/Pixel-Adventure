using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the save crystal in the town and the screen where the player saves the game by hand.
    /// Needs the game saver to exist already.
    /// </summary>
    public static class SavePointBuilder
    {
        private static readonly Vector2 ButtonSize = new Vector2(600f, 120f);

        /// <summary>Builds the crystal and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player)
        {
            InventoryPanel screen = BuildScreen(canvas, player.GetComponent<GameSaver>());
            GameObject crystal = TownNpcFactory.Create("Save Crystal", "World/save_crystal", material, TownLayout.SavePoint, screen, true);
            LightFactory.Point("Crystal Glow", crystal.transform, new Vector2(0f, 0.8f), new Color(0.4f, 0.7f, 1f), 1f, 3.5f, 0.1f);
            return screen;
        }

        private static InventoryPanel BuildScreen(Transform canvas, GameSaver saver)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Save Screen", "Save Crystal", out root, out panel);

            Text intro = UiFactory.CreateText("Intro", frame, "Your progress is saved automatically.\nTouch the crystal to save right now.", 32,
                TextAnchor.MiddleCenter, new Vector2(0.5f, 1f), new Vector2(0f, -120f), new Vector2(1000f, 120f));
            intro.color = new Color(1f, 1f, 1f, 0.8f);

            Text result = UiFactory.CreateText("Result", frame, string.Empty, 36, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), new Vector2(0f, -170f), new Vector2(900f, 60f));
            root.AddComponent<ClearTextOnEnable>().text = result;

            Image button = UiFactory.CreateImage("Save Button", frame, null, new Vector2(0.5f, 0.5f), new Vector2(0f, -30f), ButtonSize);
            button.color = new Color(0.2f, 0.32f, 0.16f, 1f);
            UiFactory.CreateText("Label", button.transform, "Save Game", 44, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, ButtonSize);

            SaveGameButton action = button.gameObject.AddComponent<SaveGameButton>();
            action.saver = saver;
            action.statusText = result;
            button.gameObject.AddComponent<PressScaleFeedback>();
            return panel;
        }
    }
}
