using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Creates item pickups in the world.
    /// </summary>
    public static class ItemSpawner
    {
        /// <summary>Spawns one pickup.</summary>
        /// <param name="item">What the pickup gives.</param>
        /// <param name="amount">How many.</param>
        /// <param name="position">World position.</param>
        /// <param name="material">Sprite material to use.</param>
        public static ItemPickup Spawn(ItemDefinition item, int amount, Vector2 position, Material material)
        {
            GameObject go = new GameObject("Pickup " + item.displayName,
                typeof(SpriteRenderer), typeof(ItemPickup), typeof(PickupPopOut), typeof(PickupMagnet));
            go.transform.position = position;

            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            renderer.sprite = item.icon;
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            ItemPickup pickup = go.GetComponent<ItemPickup>();
            pickup.Initialize(item, amount);
            return pickup;
        }
    }
}
