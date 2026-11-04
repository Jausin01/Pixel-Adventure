using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the main menu: shown when the game starts and from the gear menu during play.
    /// Continue, New Game (with confirmation), music and effects volume, and Quit. Pauses the game while open.
    /// </summary>
    public static class MainMenuBuilder
    {
        private static readonly Vector2 ButtonSize = new Vector2(420f, 96f);
        private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

        /// <summary>Builds the screen. Returns it so the caller can order it on top.</summary>
        public static InventoryPanel Build(Transform canvas)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Main Menu", "Menu", out root, out panel);
            panel.window.AddComponent<PauseWhileOpen>();
            root.AddComponent<OpenOnStart>();

            Text title = UiFactory.CreateText("Game Title", frame, "PIXEL ADVENTURE", 68, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 1f), new Vector2(0f, -90f), new Vector2(1000f, 100f));
            title.color = new Color(1f, 0.85f, 0.45f);

            Image continueButton = CreateButton(frame, "Continue Button", "Continue", new Vector2(-300f, 20f), new Color(0.2f, 0.32f, 0.16f));
            continueButton.gameObject.AddComponent<CloseInventoryButton>().panel = panel;

            Image newGame = CreateButton(frame, "New Game Button", "New Game", new Vector2(-300f, -90f), new Color(0.4f, 0.22f, 0.12f));
            NewGameButton newGameAction = newGame.gameObject.AddComponent<NewGameButton>();
            newGameAction.label = newGame.GetComponentInChildren<Text>();

            Image quit = CreateButton(frame, "Quit Button", "Quit", new Vector2(-300f, -200f), new Color(0.25f, 0.25f, 0.3f));
            quit.gameObject.AddComponent<QuitButton>();

            Text options = UiFactory.CreateText("Options Title", frame, "Options", 36, TextAnchor.MiddleCenter, Center, new Vector2(300f, 90f), new Vector2(500f, 50f));
            options.color = new Color(1f, 1f, 1f, 0.7f);

            BuildVolumeRow(frame, "Music", true, 10f);
            BuildVolumeRow(frame, "Effects", false, -100f);
            return panel;
        }

        private static void BuildVolumeRow(Transform frame, string title, bool music, float y)
        {
            Vector2 buttonSize = new Vector2(80f, 80f);
            Image minus = CreateButton(frame, title + " Down", "-", new Vector2(130f, y), new Color(0.3f, 0.2f, 0.14f), buttonSize, 56);
            minus.gameObject.AddComponent<VolumeStepButton>().Configure(music, -0.1f);

            Image plus = CreateButton(frame, title + " Up", "+", new Vector2(470f, y), new Color(0.3f, 0.2f, 0.14f), buttonSize, 56);
            plus.gameObject.AddComponent<VolumeStepButton>().Configure(music, 0.1f);

            Text label = UiFactory.CreateText(title + " Label", frame, title, 36, TextAnchor.MiddleCenter, Center, new Vector2(300f, y), new Vector2(240f, 80f));
            VolumeLabel view = label.gameObject.AddComponent<VolumeLabel>();
            view.text = label;
            view.title = title;
            view.music = music;
        }

        private static Image CreateButton(Transform parent, string name, string text, Vector2 position, Color color)
        {
            return CreateButton(parent, name, text, position, color, ButtonSize, 44);
        }

        private static Image CreateButton(Transform parent, string name, string text, Vector2 position, Color color, Vector2 size, int fontSize)
        {
            Image button = UiFactory.CreateImage(name, parent, null, Center, position, size);
            button.color = color;
            UiFactory.CreateText("Label", button.transform, text, fontSize, TextAnchor.MiddleCenter, Center, Vector2.zero, size);
            button.gameObject.AddComponent<PressScaleFeedback>();
            return button;
        }
    }
}
