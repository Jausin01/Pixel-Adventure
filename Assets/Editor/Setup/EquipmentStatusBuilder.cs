using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the panel on the right of the equipment window: the character's numbers and the Unequip button.
    /// </summary>
    public static class EquipmentStatusBuilder
    {
        private static readonly Vector2 Position = new Vector2(850f, -90f);
        private static readonly Vector2 PanelSize = new Vector2(310f, 530f);
        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);

        /// <summary>Builds the panel.</summary>
        public static void Build(Transform frame, Equipment equipment, CharacterStats stats, EquipmentBrowser browser)
        {
            Image panel = UiFactory.CreateImage("Status Panel", frame, null, TopLeft, Position, PanelSize);
            panel.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            UiFactory.CreateText("Heading", panel.transform, "Status", 34, TextAnchor.MiddleLeft, TopLeft, new Vector2(20f, -14f), new Vector2(270f, 44f));

            Text numbers = UiFactory.CreateText("Numbers", panel.transform, string.Empty, 28, TextAnchor.UpperLeft, TopLeft, new Vector2(20f, -70f), new Vector2(270f, 170f));
            EquipmentStatsPanel statsPanel = panel.gameObject.AddComponent<EquipmentStatsPanel>();
            statsPanel.stats = stats;
            statsPanel.text = numbers;

            Text hint = UiFactory.CreateText("Hint", panel.transform, "Pick a slot, then tap an item to wear it.", 22, TextAnchor.UpperLeft, TopLeft, new Vector2(20f, -260f), new Vector2(270f, 90f));
            hint.color = new Color(1f, 1f, 1f, 0.6f);

            BuildUnequipButton(panel.transform, equipment, browser);
        }

        private static void BuildUnequipButton(Transform parent, Equipment equipment, EquipmentBrowser browser)
        {
            Image button = UiFactory.CreateImage("Unequip Button", parent, null, new Vector2(0.5f, 0f), new Vector2(0f, 16f), new Vector2(260f, 56f));
            button.color = new Color(0.6f, 0.3f, 0.2f, 1f);
            UiFactory.CreateText("Label", button.transform, "Unequip", 28, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(260f, 56f));

            UnequipButton action = button.gameObject.AddComponent<UnequipButton>();
            action.equipment = equipment;
            action.browser = browser;
            button.gameObject.AddComponent<PressScaleFeedback>();
        }
    }
}
