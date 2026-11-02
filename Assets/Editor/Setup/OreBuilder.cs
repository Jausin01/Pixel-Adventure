using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates one mineable ore rock. It crumbles from the top as it is mined, leaving rubble.
    /// </summary>
    public static class OreBuilder
    {
        /// <summary>Builds an ore node.</summary>
        /// <param name="spriteName">Sprite name inside the Ores art folder.</param>
        /// <param name="drops">What the ore drops when fully mined.</param>
        public static GameObject Build(Material material, Vector2 position, string spriteName, DropEntry[] drops)
        {
            GameObject ore = new GameObject("Ore " + spriteName);
            ore.transform.position = position;

            Sprite sprite = SpriteLoader.Load("Ores/" + spriteName);
            SpriteRenderer renderer = ore.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            BoxCollider2D collider = ore.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.8f, 0.5f);
            collider.offset = new Vector2(0f, 0.3f);

            ResourceDurability durability = ore.AddComponent<ResourceDurability>();
            durability.maxDurability = 4f;

            ResourceNode node = ore.AddComponent<ResourceNode>();
            node.requiredTool = ToolType.Pickaxe;

            StackedSpriteCropper cropper = ore.AddComponent<StackedSpriteCropper>();
            cropper.lowerRenderer = renderer;
            cropper.lowerSource = sprite;
            cropper.stumpPixels = 4;

            ore.AddComponent<ResourceShake>();
            ore.AddComponent<ResourceColliderToggle>();
            ore.AddComponent<ResourceRegrowth>().delay = 45f;

            ResourceDropper dropper = ore.AddComponent<ResourceDropper>();
            dropper.drops = drops;
            dropper.material = material;
            return ore;
        }
    }
}
