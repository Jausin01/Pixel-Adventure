using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the column of equipment slot rows on the left of the equipment window.
    /// </summary>
    public static class EquipmentSlotListBuilder
    {
        private const float RowWidth = 340f;
        private const float RowHeight = 70f;
        private const float RowSpacing = 6f;
        private static readonly Vector2 Origin = new Vector2(24f, -84f);
        private static readonly Vector2 TopLeft = new Vector2(0f, 1f);

        /// <summary>Builds one row per equipment slot.</summary>
        public static void Build(Transform frame, Equipment equipment, EquipmentBrowser browser)
        {
            EquipmentSlot[] slots = EquipmentSlots.All;
            EquipmentSlotRowView[] rows = new EquipmentSlotRowView[slots.Length];
            for (int i = 0; i < slots.Length; i++) rows[i] = CreateRow(frame, browser, slots[i], i);

            EquipmentSlotList list = frame.gameObject.AddComponent<EquipmentSlotList>();
            list.equipment = equipment;
            list.browser = browser;
            list.rows = rows;
        }

        private static EquipmentSlotRowView CreateRow(Transform frame, EquipmentBrowser browser, EquipmentSlot slot, int index)
        {
            Vector2 position = new Vector2(Origin.x, Origin.y - index * (RowHeight + RowSpacing));
            Image background = UiFactory.CreateImage(slot + " Row", frame, null, TopLeft, position, new Vector2(RowWidth, RowHeight));
            background.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            Outline outline = background.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 0.85f, 0.3f, 1f);
            outline.effectDistance = new Vector2(3f, -3f);
            outline.enabled = false;

            Image slotFrame = UiFactory.CreateImage("Slot", background.transform, SpriteLoader.Load("Hud/slot"), TopLeft, new Vector2(3f, -3f), new Vector2(64f, 64f));
            slotFrame.raycastTarget = false;
            Image icon = UiFactory.CreateImage("Icon", slotFrame.transform, null, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(48f, 48f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;

            Text slotText = UiFactory.CreateText("Slot Name", background.transform, string.Empty, 20, TextAnchor.MiddleLeft, TopLeft, new Vector2(78f, -4f), new Vector2(255f, 28f));
            slotText.color = new Color(1f, 1f, 1f, 0.65f);
            Text itemText = UiFactory.CreateText("Item Name", background.transform, string.Empty, 26, TextAnchor.MiddleLeft, TopLeft, new Vector2(78f, -32f), new Vector2(255f, 34f));

            EquipmentSlotRowView view = background.gameObject.AddComponent<EquipmentSlotRowView>();
            view.slot = slot;
            view.icon = icon;
            view.slotText = slotText;
            view.itemText = itemText;
            view.selectionOutline = outline;
            view.silhouette = SpriteLoader.Load(SilhouettePath(slot));

            background.gameObject.AddComponent<EquipmentSlotRowSelector>().browser = browser;
            return view;
        }

        private static string SilhouettePath(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon: return "Hud/slot_weapon";
                case EquipmentSlot.Secondary: return "Hud/slot_secondary";
                case EquipmentSlot.Head: return "Hud/slot_head";
                case EquipmentSlot.Body: return "Hud/slot_body";
                case EquipmentSlot.Legs: return "Hud/slot_legs";
                default: return "Hud/slot_accessory";
            }
        }
    }
}
