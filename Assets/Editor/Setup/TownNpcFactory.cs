using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates a town character or object the player can use with the action button: a sprite and an interaction trigger.
    /// </summary>
    public static class TownNpcFactory
    {
        /// <summary>Builds the object at a position and links it to the screen it opens.</summary>
        /// <param name="solid">True to also give it a small solid body the player cannot walk through.</param>
        public static GameObject Create(string name, string spritePath, Material material, Vector2 position, InventoryPanel screen, bool solid)
        {
            GameObject npc = new GameObject(name);
            npc.transform.position = position;

            SpriteRenderer renderer = npc.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load(spritePath);
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            if (solid)
            {
                BoxCollider2D body = npc.AddComponent<BoxCollider2D>();
                body.size = new Vector2(0.7f, 0.4f);
                body.offset = new Vector2(0f, 0.2f);
            }

            CircleCollider2D trigger = npc.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;

            Interactable interactable = npc.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = renderer.sprite;
            return npc;
        }
    }
}
