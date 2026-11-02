using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Defines every item and tier of the game as assets. Run before anything that needs items.
    /// To add an item, add a line to <see cref="BuildItems"/>.
    /// </summary>
    public static class ItemCatalogBuilder
    {
        /// <summary>Creates or updates all tier and item assets.</summary>
        public static void Build()
        {
            BuildTiers();
            BuildItems();
            UnityEditor.AssetDatabase.SaveAssets();
        }

        private static void BuildTiers()
        {
            ItemTierFactory.GetOrCreate("clubs", "Clubs", new Color(0.78f, 0.78f, 0.78f), 0);
            ItemTierFactory.GetOrCreate("diamonds", "Diamonds", new Color(0.45f, 0.8f, 1f), 1);
            ItemTierFactory.GetOrCreate("hearts", "Hearts", new Color(1f, 0.45f, 0.55f), 2);
            ItemTierFactory.GetOrCreate("spades", "Spades", new Color(1f, 0.82f, 0.3f), 3);
            ItemTierFactory.GetOrCreate("joker", "Joker", new Color(0.8f, 0.5f, 1f), 4);
        }

        private static void BuildItems()
        {
            Add("wood", "Wood", "Items/wood", ItemCategory.Material, "clubs", "Rough timber from the forest. Useful for building and crafting.");
            Add("coal", "Coal", "Items/coal", ItemCategory.Material, "clubs", "Black and brittle. Burns hot and long.");
            Add("silver_ingot", "Silver Ingot", "Items/silver_ingot", ItemCategory.Material, "diamonds", "A bar of polished silver.");
            Add("amber", "Amber", "Items/amber", ItemCategory.Material, "diamonds", "Warm orange stone with an old glow inside.");
            Add("amethyst", "Amethyst", "Items/amethyst", ItemCategory.Material, "diamonds", "A purple crystal that hums faintly.");
            Add("gold_ingot", "Gold Ingot", "Items/gold_ingot", ItemCategory.Material, "hearts", "A heavy bar of pure gold.");
            Add("ruby", "Ruby", "Items/ruby", ItemCategory.Material, "hearts", "A deep red gem, warm to the touch.");
            Add("emerald", "Emerald", "Items/emerald", ItemCategory.Material, "hearts", "A green gem as clear as spring water.");
            Add("rose_quartz", "Rose Quartz", "Items/rose_quartz", ItemCategory.Material, "hearts", "A soft pink stone said to bring luck.");
            Add("sapphire", "Sapphire", "Items/sapphire", ItemCategory.Material, "hearts", "A deep blue gem that catches the light.");
            Add("diamond", "Diamond", "Items/diamond", ItemCategory.Material, "spades", "The hardest stone there is.");

            Add("apple", "Apple", "Items/apple", ItemCategory.Consumable, "clubs", "A crisp red apple.", 15f, 0f);
            Add("orange", "Orange", "Items/orange", ItemCategory.Consumable, "clubs", "A juicy orange full of sweetness.", 0f, 20f);

            Add("good_sword", "Good Sword", "Swords/sword_silver_up", ItemCategory.Weapon, "clubs", "A reliable blade for any adventurer.");
            Add("great_sword", "Great Sword", "Swords/sword_blue_up", ItemCategory.Weapon, "hearts", "A balanced blade with a cold blue edge.");
            Add("ruby_sword", "Ruby Sword", "Swords/sword_red_up", ItemCategory.Weapon, "spades", "A blade set with a living flame.");
        }

        private static void Add(string id, string name, string icon, ItemCategory category, string tier, string description,
            float healthRestored = 0f, float energyRestored = 0f)
        {
            ItemAssetFactory.GetOrCreate(new ItemSpec
            {
                id = id, displayName = name, iconPath = icon, category = category, tierId = tier,
                description = description, healthRestored = healthRestored, energyRestored = energyRestored
            });
        }
    }
}
