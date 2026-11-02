using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Places choppable trees in the test scene. Needs the items created by ItemCatalogBuilder.
    /// </summary>
    public static class ResourceWorldBuilder
    {
        /// <summary>Builds the trees.</summary>
        public static void Build(Material material)
        {
            ItemDefinition wood = ItemAssetFactory.Load("wood");
            ItemDefinition apple = ItemAssetFactory.Load("apple");
            ItemDefinition orange = ItemAssetFactory.Load("orange");

            DropEntry[] plain = { Drop(wood, 2, 4, 1f) };
            DropEntry[] appleTree = { Drop(wood, 2, 4, 1f), Drop(apple, 1, 3, 1f) };
            DropEntry[] orangeTree = { Drop(wood, 2, 4, 1f), Drop(orange, 1, 3, 1f) };

            TreeBuilder.Build(material, new Vector2(-6f, 3f), 0, 0, plain);
            TreeBuilder.Build(material, new Vector2(-3f, 4f), 1, 1, appleTree);
            TreeBuilder.Build(material, new Vector2(0f, 5f), 2, 2, plain);
            TreeBuilder.Build(material, new Vector2(3f, 4.5f), 3, 3, orangeTree);
            TreeBuilder.Build(material, new Vector2(6f, 3f), 4, 4, plain);
        }

        private static DropEntry Drop(ItemDefinition item, int min, int max, float chance)
        {
            return new DropEntry { item = item, minAmount = min, maxAmount = max, chance = chance };
        }
    }
}
