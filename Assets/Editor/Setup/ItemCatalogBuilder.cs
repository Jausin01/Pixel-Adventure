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
            GearNatureCatalogBuilder.Build();
            BuildTiers();
            BuildItems();
            ConfigureSmithing();
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

        // What the blacksmith asks for one upgrade step of gear in each tier (the price grows with the step).
        private static void ConfigureSmithing()
        {
            SetSmithing("clubs", 15, Amount("coal", 2), Amount("wood", 2));
            SetSmithing("diamonds", 40, Amount("silver_ingot", 2), Amount("amber", 1));
            SetSmithing("hearts", 100, Amount("gold_ingot", 1), Amount("ruby", 1));
            SetSmithing("spades", 250, Amount("gold_ingot", 2), Amount("ruby", 2));
        }

        private static void SetSmithing(string tierId, int shards, params ItemAmount[] materials)
        {
            ItemTier tier = ItemTierFactory.Load(tierId);
            tier.smithShards = shards;
            tier.smithMaterials = materials;
            UnityEditor.EditorUtility.SetDirty(tier);
        }

        private static ItemAmount Amount(string itemId, int amount)
        {
            return new ItemAmount { item = ItemAssetFactory.Load(itemId), amount = amount };
        }

        private static void BuildItems()
        {
            AddCurrency();

            Add("wood", "Wood", "Items/wood", ItemCategory.Material, "clubs", "Rough timber from the forest. Useful for building and crafting.", 0f, 0f, 2);
            Add("coal", "Coal", "Items/coal", ItemCategory.Material, "clubs", "Black and brittle. Burns hot and long.", 0f, 0f, 2);
            Add("silver_ingot", "Silver Ingot", "Items/silver_ingot", ItemCategory.Material, "diamonds", "A bar of polished silver.", 0f, 0f, 6);
            Add("amber", "Amber", "Items/amber", ItemCategory.Material, "diamonds", "Warm orange stone with an old glow inside.", 0f, 0f, 8);
            Add("amethyst", "Amethyst", "Items/amethyst", ItemCategory.Material, "diamonds", "A purple crystal that hums faintly.", 0f, 0f, 8);
            Add("gold_ingot", "Gold Ingot", "Items/gold_ingot", ItemCategory.Material, "hearts", "A heavy bar of pure gold.");
            Add("ruby", "Ruby", "Items/ruby", ItemCategory.Material, "hearts", "A deep red gem, warm to the touch.");
            Add("emerald", "Emerald", "Items/emerald", ItemCategory.Material, "hearts", "A green gem as clear as spring water.");
            Add("rose_quartz", "Rose Quartz", "Items/rose_quartz", ItemCategory.Material, "hearts", "A soft pink stone said to bring luck.");
            Add("sapphire", "Sapphire", "Items/sapphire", ItemCategory.Material, "hearts", "A deep blue gem that catches the light.");
            Add("diamond", "Diamond", "Items/diamond", ItemCategory.Material, "spades", "The hardest stone there is.");

            Add("apple", "Apple", "Items/apple", ItemCategory.Consumable, "clubs", "A crisp red apple.", 15f, 0f);
            Add("orange", "Orange", "Items/orange", ItemCategory.Consumable, "clubs", "A juicy orange full of sweetness.", 0f, 20f);
            Add("health_potion", "Health Potion", "Items/health_potion", ItemCategory.Consumable, "diamonds", "A red brew that closes wounds fast.", 60f, 0f, 10);
            Add("energy_potion", "Energy Potion", "Items/energy_potion", ItemCategory.Consumable, "diamonds", "A blue tonic that refills your energy.", 0f, 60f, 10);

            AddGear("good_sword", "Good Sword", "Swords/sword_silver_up", ItemCategory.Weapon, EquipmentKind.Sword, "clubs", "A reliable blade for any adventurer.", 2f, 0f, 0f, 0);
            AddGear("great_sword", "Great Sword", "Swords/sword_blue_up", ItemCategory.Weapon, EquipmentKind.Sword, "hearts", "A balanced blade with a cold blue edge.", 5f, 0f, 0f, 1);
            AddGear("ruby_sword", "Ruby Sword", "Swords/sword_red_up", ItemCategory.Weapon, EquipmentKind.Sword, "spades", "A blade set with a living flame.", 9f, 0f, 0f, 2);

            AddGear("short_bow", "Short Bow", "Bow/bow_idle", ItemCategory.Weapon, EquipmentKind.Bow, "clubs", "Light and quick to draw.", 2f, 0f, 0f, 0);
            AddGear("hunter_bow", "Hunter's Bow", "Bow/bow_idle", ItemCategory.Weapon, EquipmentKind.Bow, "diamonds", "Strung with fine sinew for longer shots.", 4f, 0f, 0f, 0);

            AddGear("hand_axe", "Hand Axe", "Weapons/axe_iron", ItemCategory.Weapon, EquipmentKind.Axe, "clubs", "Heavy and slow. Every swing lands hard.", 2f, 0f, 0f, 0);
            AddGear("battle_axe", "Battle Axe", "Weapons/axe_steel", ItemCategory.Weapon, EquipmentKind.Axe, "hearts", "A broad steel head that bites deep.", 6f, 0f, 0f, 1);
            AddGear("ruby_axe", "Ruby Axe", "Weapons/axe_ruby", ItemCategory.Weapon, EquipmentKind.Axe, "spades", "A crimson edge that never dulls.", 10f, 0f, 0f, 2);

            AddGear("iron_spear", "Iron Spear", "Items/spear_iron", ItemCategory.Weapon, EquipmentKind.Spear, "clubs", "Long reach. Keep your enemies at arm's length and then some.", 2f, 0f, 0f, 0);
            AddGear("steel_spear", "Steel Spear", "Items/spear_steel", ItemCategory.Weapon, EquipmentKind.Spear, "hearts", "A well balanced spear with a keen steel tip.", 5f, 0f, 0f, 1);
            AddGear("ruby_spear", "Ruby Spear", "Items/spear_ruby", ItemCategory.Weapon, EquipmentKind.Spear, "spades", "Its tip glows like an ember.", 9f, 0f, 0f, 2);

            AddGear("twin_daggers", "Twin Daggers", "Items/daggers_iron", ItemCategory.Weapon, EquipmentKind.Dagger, "clubs", "Two quick blades. Fast hands win fights.", 1f, 0f, 0f, 0);
            AddGear("steel_daggers", "Steel Daggers", "Items/daggers_steel", ItemCategory.Weapon, EquipmentKind.Dagger, "hearts", "Perfectly balanced steel twins.", 3f, 0f, 0f, 1);
            AddGear("ruby_daggers", "Ruby Daggers", "Items/daggers_ruby", ItemCategory.Weapon, EquipmentKind.Dagger, "spades", "They sting like a hornet.", 6f, 0f, 0f, 2);

            AddGear("war_hammer", "War Hammer", "Items/hammer_iron", ItemCategory.Weapon, EquipmentKind.Hammer, "clubs", "Slow and brutal. Flattens whatever it lands on.", 3f, 0f, 0f, 0);
            AddGear("steel_hammer", "Steel Hammer", "Items/hammer_steel", ItemCategory.Weapon, EquipmentKind.Hammer, "hearts", "A steel head that shakes the ground.", 8f, 0f, 0f, 1);
            AddGear("ruby_hammer", "Ruby Hammer", "Items/hammer_ruby", ItemCategory.Weapon, EquipmentKind.Hammer, "spades", "Forged in the heart of a volcano.", 12f, 0f, 0f, 2);

            AddGear("oak_staff", "Oak Staff", "Items/staff_oak", ItemCategory.Weapon, EquipmentKind.Staff, "clubs", "A simple staff that sings with magic.", 2f, 0f, 0f, 0);
            AddGear("amethyst_staff", "Amethyst Staff", "Items/staff_amethyst", ItemCategory.Weapon, EquipmentKind.Staff, "hearts", "The gem hums with power.", 6f, 0f, 0f, 1);
            AddGear("ruby_staff", "Ruby Staff", "Items/staff_ruby", ItemCategory.Weapon, EquipmentKind.Staff, "spades", "Its orb burns with a cold fire.", 10f, 0f, 0f, 2);

            AddGear("iron_shuriken", "Iron Shuriken", "Items/shuriken_iron", ItemCategory.Weapon, EquipmentKind.Shuriken, "clubs", "Thrown at the nearest enemy. Quick to throw again.", 1f, 0f, 0f, 0);
            AddGear("steel_shuriken", "Steel Shuriken", "Items/shuriken_steel", ItemCategory.Weapon, EquipmentKind.Shuriken, "diamonds", "Sharper points and a cleaner spin.", 3f, 0f, 0f, 0);
            AddGear("wooden_boomerang", "Wooden Boomerang", "Items/boomerang_wood", ItemCategory.Weapon, EquipmentKind.Boomerang, "clubs", "Hits on the way out and on the way back.", 1f, 0f, 0f, 0);
            AddGear("bone_boomerang", "Bone Boomerang", "Items/boomerang_bone", ItemCategory.Weapon, EquipmentKind.Boomerang, "diamonds", "Carved from something big. Hits harder.", 3f, 0f, 0f, 0);

            AddGear("leather_cap", "Leather Cap", "Items/cap", ItemCategory.Equipment, EquipmentKind.Head, "clubs", "A simple cap that keeps the sun off.", 0f, 1f, 5f, 0);
            AddGear("leather_vest", "Leather Vest", "Items/vest", ItemCategory.Equipment, EquipmentKind.Body, "clubs", "Stitched leather. Better than nothing.", 0f, 2f, 10f, 0);
            AddGear("leather_trousers", "Leather Trousers", "Items/trousers", ItemCategory.Equipment, EquipmentKind.Legs, "clubs", "Tough trousers for long walks.", 0f, 1f, 5f, 0);
            AddGear("lucky_ring", "Lucky Ring", "Items/ring", ItemCategory.Equipment, EquipmentKind.Accessory, "diamonds", "A plain ring said to bring luck.", 1f, 0f, 5f, 0);
            AddGear("gold_ring", "Gold Ring", "Items/ring", ItemCategory.Equipment, EquipmentKind.Accessory, "hearts", "A heavy ring that hums with strength.", 3f, 1f, 0f, 0);
        }

        // The money of the game. Rename it here; everything else refers to it by id.
        private static void AddCurrency()
        {
            ItemAssetFactory.GetOrCreate(new ItemSpec
            {
                id = ProjectConstants.CurrencyId, displayName = "Rift Shard", iconPath = "Items/rift_shard", category = ItemCategory.Material,
                tierId = "clubs", description = "A sliver of the rift that never closes. Everyone in town accepts it.",
                maxStack = 99999, dropAsSingleStack = true
            });
        }

        private static void Add(string id, string name, string icon, ItemCategory category, string tier, string description,
            float healthRestored = 0f, float energyRestored = 0f, int sellValue = 0)
        {
            ItemAssetFactory.GetOrCreate(new ItemSpec
            {
                id = id, displayName = name, iconPath = icon, category = category, tierId = tier,
                description = description, healthRestored = healthRestored, energyRestored = energyRestored, sellValue = sellValue
            });
        }

        private static void AddGear(string id, string name, string icon, ItemCategory category, EquipmentKind kind, string tier, string description,
            float attack, float defense, float maxHealth, int swordSkinIndex)
        {
            ItemAssetFactory.GetOrCreate(new ItemSpec
            {
                id = id, displayName = name, iconPath = icon, category = category, tierId = tier, description = description,
                equipKind = kind, attack = attack, defense = defense, maxHealth = maxHealth, swordSkinIndex = swordSkinIndex
            });
        }
    }
}
