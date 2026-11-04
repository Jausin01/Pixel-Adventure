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

            DropEntry[][] tables = { plain, appleTree, plain, orangeTree, plain };
            Vector2[] spots = TownLayout.Trees;
            for (int i = 0; i < spots.Length; i++)
            {
                TreeBuilder.Build(material, spots[i], i % 5, i % 5, tables[i % tables.Length]);
            }
        }

        private static DropEntry Drop(ItemDefinition item, int min, int max, float chance)
        {
            return new DropEntry { item = item, minAmount = min, maxAmount = max, chance = chance };
        }
    }
}
