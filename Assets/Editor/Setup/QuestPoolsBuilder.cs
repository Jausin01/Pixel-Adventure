using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Defines what the quest generator may pick from, as one asset. Run after the item and arena catalogs.
    /// To add materials, enemies or rewards, add a line to the matching list.
    /// </summary>
    public static class QuestPoolsBuilder
    {
        private const string Path = "Assets/Data/QuestPools.asset";

        /// <summary>Creates or updates the pools asset and returns it.</summary>
        public static QuestPools Build(ArenaDefinition[] arenas)
        {
            AssetFolderUtil.Ensure("Assets/Data");

            QuestPools pools = AssetDatabase.LoadAssetAtPath<QuestPools>(Path);
            if (pools == null)
            {
                pools = ScriptableObject.CreateInstance<QuestPools>();
                AssetDatabase.CreateAsset(pools, Path);
            }

            ArenaDefinition ratFields = Arena(arenas, "rat_fields");
            ArenaDefinition batCave = Arena(arenas, "bat_cave");

            pools.currency = ItemAssetFactory.Load(ProjectConstants.CurrencyId);
            pools.arenas = arenas;

            pools.materials = new[]
            {
                Material("coal", 4, 8, 0), Material("wood", 4, 8, 0), Material("apple", 3, 6, 0), Material("orange", 3, 6, 0),
                Material("silver_ingot", 3, 6, 1), Material("amber", 2, 4, 2), Material("amethyst", 2, 4, 2),
                Material("gold_ingot", 1, 2, 4), Material("ruby", 1, 2, 6), Material("emerald", 1, 2, 8), Material("sapphire", 1, 2, 8)
            };

            pools.enemies = new[]
            {
                Enemy("rat", "Rats", ratFields, 8, 14, 0),
                Enemy("bat", "Bats", batCave, 6, 12, 0),
                Enemy(string.Empty, "Monsters", null, 15, 25, 0),
                Enemy("brown_rat", "Brown Rats", null, 6, 10, 2),
                Enemy("white_rat", "White Rats", null, 6, 10, 2),
                Enemy("zombie", "Zombies", null, 6, 10, 1),
                Enemy("skeleton", "Skeletons", null, 6, 10, 1),
                Enemy("slime", "Slimes", null, 6, 10, 2),
                Enemy("spider", "Spiders", null, 8, 14, 1),
                Enemy("moss_brute", "Moss Brutes", null, 5, 9, 2),
                Enemy("shadow_boar", "Shadow Boars", null, 5, 9, 2),
                Enemy("cave_gnawer", "Cave Gnawers", null, 6, 10, 1),
                Enemy("hollow_knight", "Hollow Knights", null, 4, 8, 3),
                Enemy("wraith", "Wraiths", null, 4, 8, 3),
                Enemy("rat_king", "Rat King", ratFields, 1, 2, 3),
                Enemy("thorn_beast", "Thorn Beast", null, 1, 2, 4),
                Enemy("stone_colossus", "Stone Colossus", null, 1, 2, 5),
                Enemy("horned_tyrant", "Horned Tyrant", null, 1, 2, 5),
                Enemy("ashen_drake", "Ashen Drake", null, 1, 2, 6),
                Enemy("ogre_king", "Ogre King", null, 1, 2, 5)
            };

            pools.rewards = new[]
            {
                Reward("gold_ingot", 1, 2, 3f, 0),
                Reward("leather_cap", 1, 1, 2f, 0), Reward("leather_trousers", 1, 1, 2f, 0),
                Reward("leather_vest", 1, 1, 2f, 2), Reward("hunter_bow", 1, 1, 2f, 2),
                Reward("lucky_ring", 1, 1, 1.5f, 3), Reward("great_sword", 1, 1, 1.5f, 5),
                Reward("gold_ring", 1, 1, 1f, 6), Reward("ruby_sword", 1, 1, 1f, 9),
                Reward("hand_axe", 1, 1, 1.5f, 1), Reward("iron_spear", 1, 1, 1.5f, 1), Reward("iron_shuriken", 1, 1, 1.5f, 1), Reward("wooden_boomerang", 1, 1, 1.5f, 1), Reward("twin_daggers", 1, 1, 1.5f, 1), Reward("war_hammer", 1, 1, 1.5f, 1), Reward("oak_staff", 1, 1, 1.5f, 1),
                Reward("steel_shuriken", 1, 1, 1f, 4), Reward("bone_boomerang", 1, 1, 1f, 4),
                Reward("steel_spear", 1, 1, 1f, 5), Reward("battle_axe", 1, 1, 1f, 5), Reward("steel_daggers", 1, 1, 1f, 5), Reward("steel_hammer", 1, 1, 1f, 5), Reward("amethyst_staff", 1, 1, 1f, 5),
                Reward("ruby_spear", 1, 1, 0.7f, 9), Reward("ruby_axe", 1, 1, 0.7f, 9), Reward("ruby_daggers", 1, 1, 0.7f, 9), Reward("ruby_hammer", 1, 1, 0.7f, 9), Reward("ruby_staff", 1, 1, 0.7f, 9)
            };

            EditorUtility.SetDirty(pools);
            AssetDatabase.SaveAssets();
            return pools;
        }

        private static ArenaDefinition Arena(ArenaDefinition[] arenas, string id)
        {
            for (int i = 0; i < arenas.Length; i++)
            {
                if (arenas[i].id == id) return arenas[i];
            }
            Debug.LogError("Quest pools: arena not found: " + id);
            return null;
        }

        private static QuestPools.MaterialOption Material(string itemId, int min, int max, int minDone)
        {
            return new QuestPools.MaterialOption { item = ItemAssetFactory.Load(itemId), min = min, max = max, minDone = minDone };
        }

        private static QuestPools.EnemyOption Enemy(string enemy, string label, ArenaDefinition arena, int min, int max, int minDone)
        {
            return new QuestPools.EnemyOption { enemy = enemy, label = label, arena = arena, min = min, max = max, minDone = minDone };
        }

        private static QuestPools.RewardOption Reward(string itemId, int min, int max, float weight, int minDone)
        {
            return new QuestPools.RewardOption { item = ItemAssetFactory.Load(itemId), min = min, max = max, weight = weight, minDone = minDone };
        }
    }
}
