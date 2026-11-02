using UnityEngine;
using UnityEngine.Rendering;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates one choppable tree: a trunk and a canopy that get cut from the top down as it is chopped.
    /// </summary>
    public static class TreeBuilder
    {
        /// <summary>Builds a tree.</summary>
        /// <param name="canopyIndex">Which canopy sprite (0 to 4).</param>
        /// <param name="trunkIndex">Which trunk sprite (0 to 4).</param>
        /// <param name="drops">What the tree drops when felled.</param>
        public static GameObject Build(Material material, Vector2 position, int canopyIndex, int trunkIndex, DropEntry[] drops)
        {
            GameObject tree = new GameObject("Tree");
            tree.transform.position = position;

            // The group makes trunk and canopy sort against the player as one object.
            tree.AddComponent<SortingGroup>();

            Sprite trunkSprite = SpriteLoader.Load("Trees/trunk_" + trunkIndex);
            Sprite canopySprite = SpriteLoader.Load("Trees/canopy_" + canopyIndex);
            SpriteRenderer trunk = CreatePart("Trunk", tree.transform, trunkSprite, material, Vector2.zero, 0);
            SpriteRenderer canopy = CreatePart("Canopy", tree.transform, canopySprite, material, Vector2.up, 1);

            BoxCollider2D collider = tree.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.5f, 0.3f);
            collider.offset = new Vector2(0f, 0.15f);

            ResourceDurability durability = tree.AddComponent<ResourceDurability>();
            durability.maxDurability = 5f;

            ResourceNode node = tree.AddComponent<ResourceNode>();
            node.requiredTool = ToolType.Axe;

            StackedSpriteCropper cropper = tree.AddComponent<StackedSpriteCropper>();
            cropper.lowerRenderer = trunk;
            cropper.lowerSource = trunkSprite;
            cropper.upperRenderer = canopy;
            cropper.upperSource = canopySprite;

            tree.AddComponent<ResourceShake>();
            tree.AddComponent<ResourceColliderToggle>();
            tree.AddComponent<ResourceRegrowth>();

            ResourceDropper dropper = tree.AddComponent<ResourceDropper>();
            dropper.drops = drops;
            dropper.material = material;
            return tree;
        }

        private static SpriteRenderer CreatePart(string name, Transform parent, Sprite sprite, Material material, Vector2 localPosition, int order)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sharedMaterial = material;
            renderer.sortingOrder = order;
            return renderer;
        }
    }
}
