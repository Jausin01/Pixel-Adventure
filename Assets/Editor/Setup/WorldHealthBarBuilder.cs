using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates a health bar that floats above a character in the game world.
    /// The bar is only visible while the character is hurt but alive.
    /// </summary>
    public static class WorldHealthBarBuilder
    {
        private const int BackgroundOrder = 20;

        /// <summary>Builds a bar as a child of <paramref name="parent"/>.</summary>
        /// <param name="localPosition">Position of the bar's center relative to the parent.</param>
        public static void Build(Transform parent, Material material, Health health, Vector2 localPosition)
        {
            Sprite fullSprite = SpriteLoader.Load("Hud/hp_full");

            GameObject bar = new GameObject("Health Bar");
            bar.transform.SetParent(parent, false);
            bar.transform.localPosition = localPosition;

            // All sprites live under "Visual" so the whole bar can be hidden by switching it off.
            GameObject visual = new GameObject("Visual");
            visual.transform.SetParent(bar.transform, false);
            CreateRenderer(visual, SpriteLoader.Load("Hud/hp_empty"), material, BackgroundOrder);

            GameObject fillObject = new GameObject("Fill");
            fillObject.transform.SetParent(visual.transform, false);
            SpriteRenderer fill = CreateRenderer(fillObject, fullSprite, material, BackgroundOrder + 1);

            WorldFillBar fillBar = bar.AddComponent<WorldFillBar>();
            fillBar.fill = fill;
            fillBar.fullSprite = fullSprite;

            bar.AddComponent<MeterBarBinder>().source = health;

            WorldBarVisibility visibility = bar.AddComponent<WorldBarVisibility>();
            visibility.source = health;
            visibility.visual = visual;
        }

        private static SpriteRenderer CreateRenderer(GameObject go, Sprite sprite, Material material, int order)
        {
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sharedMaterial = material;
            renderer.sortingOrder = order;
            return renderer;
        }
    }
}
