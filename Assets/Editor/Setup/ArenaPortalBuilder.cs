using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the portal in the town and the screen where the player picks an arena.
    /// </summary>
    public static class ArenaPortalBuilder
    {
        private static readonly Vector2 PortalPosition = TownLayout.Portal;
        private static readonly Vector2 RowSize = new Vector2(700f, 88f);

        /// <summary>Builds the portal and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, ArenaRun run, ArenaDefinition[] arenas)
        {
            InventoryPanel screen = BuildScreen(canvas, run, arenas);
            BuildPortal(material, screen);
            return screen;
        }

        private static void BuildPortal(Material material, InventoryPanel screen)
        {
            GameObject portal = new GameObject("Arena Portal");
            portal.transform.position = PortalPosition;

            SpriteRenderer renderer = portal.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/portal");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            LightFactory.Point("Portal Glow", portal.transform, new Vector2(0f, 1.5f), new Color(0.6f, 0.5f, 1f), 1.4f, 6f, 0.12f);

            // Solid stone base so the player cannot stand on the portal.
            BoxCollider2D body = portal.AddComponent<BoxCollider2D>();
            body.size = new Vector2(2.9f, 0.9f);
            body.offset = new Vector2(0f, 0.45f);

            // Interaction range reaching the front of the base.
            CircleCollider2D trigger = portal.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.7f;
            trigger.offset = new Vector2(0f, 0.3f);

            Interactable interactable = portal.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = SpriteLoader.Load("World/portal");
        }

        private static InventoryPanel BuildScreen(Transform canvas, ArenaRun run, ArenaDefinition[] arenas)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Portal Screen", "Portal", out root, out panel);

            for (int i = 0; i < arenas.Length; i++) BuildRow(frame, run, panel, arenas[i], i);
            return panel;
        }

        private static void BuildRow(Transform frame, ArenaRun run, InventoryPanel screen, ArenaDefinition arena, int index)
        {
            Vector2 topLeft = new Vector2(0f, 1f);
            Image row = UiFactory.CreateImage(arena.displayName + " Row", frame, null, topLeft,
                new Vector2(240f, -100f - index * (RowSize.y + 8f)), RowSize);
            row.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            UiFactory.CreateText("Name", row.transform, arena.displayName, 40, TextAnchor.MiddleLeft, topLeft, new Vector2(24f, -4f), new Vector2(420f, 50f));
            Text best = UiFactory.CreateText("Best", row.transform, string.Empty, 28, TextAnchor.MiddleLeft, topLeft, new Vector2(24f, -52f), new Vector2(420f, 34f));
            best.color = new Color(1f, 1f, 1f, 0.7f);

            UiFactory.CreateText("Enter", row.transform, "Enter", 36, TextAnchor.MiddleRight, new Vector2(1f, 0.5f), new Vector2(-24f, 0f), new Vector2(200f, 60f));

            ArenaRowView view = row.gameObject.AddComponent<ArenaRowView>();
            view.arena = arena;
            view.bestText = best;

            EnterArenaButton button = row.gameObject.AddComponent<EnterArenaButton>();
            button.run = run;
            button.arena = arena;
            button.screen = screen;
            row.gameObject.AddComponent<PressScaleFeedback>();
        }
    }
}
