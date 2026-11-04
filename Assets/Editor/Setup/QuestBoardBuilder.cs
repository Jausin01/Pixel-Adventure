using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the quest board in the town and the screen listing the quests.
    /// </summary>
    public static class QuestBoardBuilder
    {
        private static readonly Vector2 BoardPosition = TownLayout.QuestBoard;
        private static readonly Vector2 RowSize = new Vector2(550f, 124f);
        private const int Columns = 2;
        private const int RowsPerColumn = 4;

        /// <summary>Builds the board and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, QuestLog log, Inventory inventory)
        {
            InventoryPanel screen = BuildScreen(canvas, log, inventory);
            BuildBoard(material, screen);
            return screen;
        }

        private static void BuildBoard(Material material, InventoryPanel screen)
        {
            GameObject board = new GameObject("Quest Board");
            board.transform.position = BoardPosition;

            SpriteRenderer renderer = board.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/quest_board");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            // Solid legs so the player cannot walk through the board.
            BoxCollider2D body = board.AddComponent<BoxCollider2D>();
            body.size = new Vector2(1.7f, 0.5f);
            body.offset = new Vector2(0f, 0.35f);

            CircleCollider2D trigger = board.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.4f;
            trigger.offset = new Vector2(0f, 0.4f);

            Interactable interactable = board.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = SpriteLoader.Load("Hud/icon_quest");
        }

        private static InventoryPanel BuildScreen(Transform canvas, QuestLog log, Inventory inventory)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Quest Screen", "Quest Board", out root, out panel);

            for (int i = 0; i < log.board.slotCount; i++) BuildRow(frame, log, inventory, i);
            return panel;
        }

        private static void BuildRow(Transform frame, QuestLog log, Inventory inventory, int index)
        {
            int column = index / RowsPerColumn;
            int row = index % RowsPerColumn;
            if (column >= Columns) return;

            Vector2 topLeft = new Vector2(0f, 1f);
            Vector2 position = new Vector2(24f + column * (RowSize.x + 8f), -88f - row * (RowSize.y + 8f));
            Image background = UiFactory.CreateImage("Quest Row " + index, frame, null, topLeft, position, RowSize);

            Text title = UiFactory.CreateText("Title", background.transform, string.Empty, 30, TextAnchor.MiddleLeft, topLeft, new Vector2(16f, -8f), new Vector2(410f, 44f));
            Text requirements = UiFactory.CreateText("Requirements", background.transform, string.Empty, 24, TextAnchor.MiddleLeft, topLeft, new Vector2(16f, -54f), new Vector2(410f, 34f));
            Text goals = UiFactory.CreateText("Goals", background.transform, string.Empty, 24, TextAnchor.MiddleLeft, topLeft, new Vector2(16f, -88f), new Vector2(410f, 32f));

            Image reward = UiFactory.CreateImage("Reward", background.transform, null, new Vector2(1f, 1f), new Vector2(-24f, -6f), new Vector2(56f, 56f));
            reward.preserveAspect = true;
            reward.raycastTarget = false;
            Text rewardText = UiFactory.CreateText("Reward Text", background.transform, string.Empty, 22, TextAnchor.UpperRight, new Vector2(1f, 1f), new Vector2(-16f, -64f), new Vector2(100f, 52f));
            rewardText.color = new Color(1f, 0.9f, 0.5f, 1f);

            QuestRowView view = background.gameObject.AddComponent<QuestRowView>();
            view.board = log.board;
            view.slot = index;
            view.log = log;
            view.inventory = inventory;
            view.titleText = title;
            view.requirementsText = requirements;
            view.goalsText = goals;
            view.rewardText = rewardText;
            view.rewardIcon = reward;
            view.background = background;

            TurnInQuestButton button = background.gameObject.AddComponent<TurnInQuestButton>();
            button.board = log.board;
            button.slot = index;
            button.log = log;
            background.gameObject.AddComponent<PressScaleFeedback>();
        }
    }
}
