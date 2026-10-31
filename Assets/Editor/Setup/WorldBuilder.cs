using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the ground and the invisible walls around the play area.
    /// </summary>
    public static class WorldBuilder
    {
        private const float Width = 30f;
        private const float Height = 20f;

        /// <summary>Builds the ground and walls.</summary>
        public static void Build(Material material)
        {
            GameObject ground = new GameObject("Ground");
            SpriteRenderer renderer = ground.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/ground");
            renderer.sharedMaterial = material;
            renderer.drawMode = SpriteDrawMode.Tiled;
            renderer.size = new Vector2(Width, Height);
            renderer.sortingOrder = -100;

            CreateWall("Wall Top", new Vector2(0f, Height / 2f + 0.5f), new Vector2(Width + 2f, 1f));
            CreateWall("Wall Bottom", new Vector2(0f, -Height / 2f - 0.5f), new Vector2(Width + 2f, 1f));
            CreateWall("Wall Left", new Vector2(-Width / 2f - 0.5f, 0f), new Vector2(1f, Height + 2f));
            CreateWall("Wall Right", new Vector2(Width / 2f + 0.5f, 0f), new Vector2(1f, Height + 2f));
        }

        private static void CreateWall(string name, Vector2 position, Vector2 size)
        {
            GameObject wall = new GameObject(name);
            wall.transform.position = position;
            wall.AddComponent<BoxCollider2D>().size = size;
        }
    }
}
