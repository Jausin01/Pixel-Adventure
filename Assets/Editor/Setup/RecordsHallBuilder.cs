using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the records monument in the town and the screen that lists the best wave reached in every arena.
    /// </summary>
    public static class RecordsHallBuilder
    {
        private static readonly Vector2 RowSize = new Vector2(900f, 90f);

        /// <summary>Builds the monument and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, ArenaDefinition[] arenas)
        {
            InventoryPanel screen = BuildScreen(canvas, arenas);
            TownNpcFactory.Create("Records Hall", "World/records_monument", material, TownLayout.RecordsHall, screen, true);
            return screen;
        }

        private static InventoryPanel BuildScreen(Transform canvas, ArenaDefinition[] arenas)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Records Screen", "Hall of Records", out root, out panel);

            Vector2 topLeft = new Vector2(0f, 1f);
            for (int i = 0; i < arenas.Length; i++)
            {
                Image row = UiFactory.CreateImage(arenas[i].displayName + " Record", frame, null, topLeft,
                    new Vector2(140f, -100f - i * (RowSize.y + 10f)), RowSize);
                row.color = new Color(0.22f, 0.16f, 0.13f, 1f);

                UiFactory.CreateText("Name", row.transform, arenas[i].displayName, 38, TextAnchor.MiddleLeft,
                    new Vector2(0f, 0.5f), new Vector2(24f, 0f), new Vector2(480f, 70f));
                Text best = UiFactory.CreateText("Best", row.transform, string.Empty, 34, TextAnchor.MiddleRight,
                    new Vector2(1f, 0.5f), new Vector2(-24f, 0f), new Vector2(340f, 70f));

                ArenaRowView view = row.gameObject.AddComponent<ArenaRowView>();
                view.arena = arenas[i];
                view.bestText = best;
            }
            return panel;
        }
    }
}
