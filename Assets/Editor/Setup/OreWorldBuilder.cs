using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Places one rock of each ore type in a row in the test scene. Needs the items created by ItemCatalogBuilder.
    /// </summary>
    public static class OreWorldBuilder
    {
        // Rock sprite name, id of the item it drops.
        private static readonly string[][] Ores =
        {
            new[] { "silver", "silver_ingot" },
            new[] { "ruby", "ruby" },
            new[] { "emerald", "emerald" },
            new[] { "diamond", "diamond" },
            new[] { "gold", "gold_ingot" },
            new[] { "amethyst", "amethyst" },
            new[] { "amber", "amber" },
            new[] { "coal", "coal" },
            new[] { "rose_quartz", "rose_quartz" },
            new[] { "sapphire", "sapphire" }
        };

        /// <summary>Builds the ore row.</summary>
        public static void Build(Material material)
        {
            const float spacing = 1.5f;
            float startX = -(Ores.Length - 1) * spacing / 2f;

            for (int i = 0; i < Ores.Length; i++)
            {
                ItemDefinition item = ItemAssetFactory.Load(Ores[i][1]);
                DropEntry[] drops = { new DropEntry { item = item, minAmount = 1, maxAmount = 2, chance = 1f } };
                OreBuilder.Build(material, new Vector2(startX + i * spacing, -4f), Ores[i][0], drops);
            }
        }
    }
}
