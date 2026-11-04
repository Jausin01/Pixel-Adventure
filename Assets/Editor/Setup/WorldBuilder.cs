using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the invisible walls that keep the player and enemies inside a play area.
    /// </summary>
    public static class WorldBuilder
    {
        /// <summary>Creates four invisible walls just outside a rectangle centered on the parent.</summary>
        public static void CreateWalls(Transform parent, Vector2 size)
        {
            CreateWall(parent, "Wall Top", new Vector2(0f, size.y / 2f + 0.5f), new Vector2(size.x + 2f, 1f));
            CreateWall(parent, "Wall Bottom", new Vector2(0f, -size.y / 2f - 0.5f), new Vector2(size.x + 2f, 1f));
            CreateWall(parent, "Wall Left", new Vector2(-size.x / 2f - 0.5f, 0f), new Vector2(1f, size.y + 2f));
            CreateWall(parent, "Wall Right", new Vector2(size.x / 2f + 0.5f, 0f), new Vector2(1f, size.y + 2f));
        }

        private static void CreateWall(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject wall = new GameObject(name);
            wall.transform.SetParent(parent, false);
            wall.transform.localPosition = position;
            wall.AddComponent<BoxCollider2D>().size = size;
        }
    }
}
