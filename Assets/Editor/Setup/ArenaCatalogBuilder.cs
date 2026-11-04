using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Defines the enemy prefabs and every arena of the game as assets. Run after the item catalog.
    /// To add an arena, add a method like <see cref="BuildRatFields"/> and list it in <see cref="Build"/>.
    /// </summary>
    public static class ArenaCatalogBuilder
    {
        private const string Folder = "Assets/Data/Arenas";

        /// <summary>Creates or updates the enemies and arenas and returns the arenas.</summary>
        public static ArenaDefinition[] Build(Material material)
        {
            AssetFolderUtil.Ensure(Folder);

            GameObject brownRat = EnemyPrefabBuilder.Build(MonsterSpec.BrownRat(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.7f, 1, 3), EnemyPrefabBuilder.Drop("coal", 0.4f), EnemyPrefabBuilder.Drop("wood", 0.3f), EnemyPrefabBuilder.Drop("apple", 0.15f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject whiteRat = EnemyPrefabBuilder.Build(MonsterSpec.WhiteRat(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.7f, 2, 4), EnemyPrefabBuilder.Drop("coal", 0.3f), EnemyPrefabBuilder.Drop("silver_ingot", 0.2f), EnemyPrefabBuilder.Drop("orange", 0.15f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject bat = EnemyPrefabBuilder.Build(MonsterSpec.Bat(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.7f, 2, 5), EnemyPrefabBuilder.Drop("amber", 0.2f), EnemyPrefabBuilder.Drop("amethyst", 0.2f), EnemyPrefabBuilder.Drop("orange", 0.15f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject zombie = EnemyPrefabBuilder.Build(MonsterSpec.Zombie(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.8f, 3, 6), EnemyPrefabBuilder.Drop("apple", 0.2f), EnemyPrefabBuilder.Drop("wood", 0.3f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject skeleton = EnemyPrefabBuilder.Build(MonsterSpec.Skeleton(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.8f, 3, 6), EnemyPrefabBuilder.Drop("silver_ingot", 0.25f), EnemyPrefabBuilder.Drop("coal", 0.3f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject slime = EnemyPrefabBuilder.Build(MonsterSpec.Slime(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.7f, 2, 5), EnemyPrefabBuilder.Drop("orange", 0.25f), EnemyPrefabBuilder.Drop("apple", 0.2f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject spider = EnemyPrefabBuilder.Build(MonsterSpec.Spider(), material, 1f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.7f, 2, 4), EnemyPrefabBuilder.Drop("amethyst", 0.2f), EnemyPrefabBuilder.Drop("amber", 0.15f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            });
            GameObject ratKing = EnemyPrefabBuilder.Build(RatKingSpec(), material, 2f, new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 25, 40), EnemyPrefabBuilder.Drop("gold_ingot", 1f, 1, 2), EnemyPrefabBuilder.Drop("ruby", 0.5f), EnemyPrefabBuilder.Drop("leather_vest", 0.5f)
            });

            GameObject mossBrute = EnemyPrefabBuilder.Build(MonsterSpec.MossBrute(), material, 1f, RegularDrops(4, 8, "wood", "apple"));
            GameObject shadowBoar = EnemyPrefabBuilder.Build(MonsterSpec.ShadowBoar(), material, 1f, RegularDrops(3, 6, "coal", "orange"));
            GameObject caveGnawer = EnemyPrefabBuilder.Build(MonsterSpec.CaveGnawer(), material, 1f, RegularDrops(2, 5, "coal", "amber"));
            GameObject hollowKnight = EnemyPrefabBuilder.Build(MonsterSpec.HollowKnight(), material, 1f, RegularDrops(5, 9, "silver_ingot", "coal"));
            GameObject wraith = EnemyPrefabBuilder.Build(MonsterSpec.Wraith(), material, 1f, RegularDrops(4, 8, "amethyst", "amber"));
            GameObject ogreKing = EnemyPrefabBuilder.Build(MonsterSpec.OgreKing(), material, 1f, BossDrops("ruby"));
            GameObject stoneColossus = EnemyPrefabBuilder.Build(MonsterSpec.StoneColossus(), material, 1f, BossDrops("sapphire"));
            GameObject ashenDrake = EnemyPrefabBuilder.Build(MonsterSpec.AshenDrake(), material, 1f, BossDrops("emerald"));
            GameObject hornedTyrant = EnemyPrefabBuilder.Build(MonsterSpec.HornedTyrant(), material, 1f, BossDrops("ruby"));
            GameObject thornBeast = EnemyPrefabBuilder.Build(MonsterSpec.ThornBeast(), material, 1f, BossDrops("emerald"));

            AssetDatabase.SaveAssets();
            Enemies enemies = new Enemies
            {
                brownRat = brownRat, whiteRat = whiteRat, bat = bat, ratKing = ratKing,
                zombie = zombie, skeleton = skeleton, slime = slime, spider = spider,
                mossBrute = mossBrute, shadowBoar = shadowBoar, caveGnawer = caveGnawer, hollowKnight = hollowKnight, wraith = wraith,
                ogreKing = ogreKing, stoneColossus = stoneColossus, ashenDrake = ashenDrake, hornedTyrant = hornedTyrant, thornBeast = thornBeast
            };
            return new[] { BuildRatFields(enemies), BuildBatCave(enemies), BuildDungeon(enemies), BuildFarm(enemies), BuildMortalArena(enemies) };
        }

        /// <summary>Every enemy prefab, so each arena can pick the ones that fit its theme.</summary>
        private class Enemies
        {
            public GameObject brownRat, whiteRat, bat, ratKing, zombie, skeleton, slime, spider;
            public GameObject mossBrute, shadowBoar, caveGnawer, hollowKnight, wraith;
            public GameObject ogreKing, stoneColossus, ashenDrake, hornedTyrant, thornBeast;
        }

        private static DropEntry[] RegularDrops(int minGold, int maxGold, string itemA, string itemB)
        {
            return new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 0.8f, minGold, maxGold), EnemyPrefabBuilder.Drop(itemA, 0.3f), EnemyPrefabBuilder.Drop(itemB, 0.2f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f), EnemyPrefabBuilder.Drop("health_potion", 0.06f), EnemyPrefabBuilder.Drop("energy_potion", 0.05f)
            };
        }

        private static DropEntry[] BossDrops(string gem)
        {
            return new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 30, 50), EnemyPrefabBuilder.Drop("gold_ingot", 1f, 1, 3), EnemyPrefabBuilder.Drop(gem, 0.6f), EnemyPrefabBuilder.Drop("health_potion", 0.6f), EnemyPrefabBuilder.Drop("energy_potion", 0.5f)
            };
        }

        private static MonsterSpec RatKingSpec()
        {
            MonsterSpec spec = MonsterSpec.BrownRat();
            spec.name = "Rat King";
            spec.maxHealth = 400f;
            spec.damage = 14f;
            spec.moveSpeed = 1.5f;
            spec.attackRange = 1.4f;
            return spec;
        }

        private static ArenaDefinition BuildRatFields(Enemies e)
        {
            ArenaDefinition arena = GetOrCreate("rat_fields", "Rat Fields", "cornfield");
            arena.enemies = new[]
            {
                Entry(e.whiteRat, 1, 1, 3f), Entry(e.brownRat, 2, 1, 2f), Entry(e.shadowBoar, 3, 3, 2f), Entry(e.zombie, 3, 3, 2f), Entry(e.mossBrute, 4, 5, 1.5f), Entry(e.bat, 2, 4, 1f)
            };
            arena.baseBudget = 3f;
            arena.budgetPerWave = 2f;
            arena.maxAlive = 14;
            arena.bossEvery = 5;
            arena.bosses = new[] { e.ratKing, e.thornBeast };
            arena.clearRewards = new[] { EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 5, 10), EnemyPrefabBuilder.Drop("wood", 1f, 1, 2), EnemyPrefabBuilder.Drop("apple", 0.3f), EnemyPrefabBuilder.Drop("health_potion", 0.3f), EnemyPrefabBuilder.Drop("energy_potion", 0.25f) };
            return Save(arena);
        }

        private static ArenaDefinition BuildBatCave(Enemies e)
        {
            ArenaDefinition arena = GetOrCreate("bat_cave", "Bat Cave", "cave");
            arena.enemies = new[]
            {
                Entry(e.bat, 2, 1, 3f), Entry(e.spider, 1, 2, 3f), Entry(e.whiteRat, 1, 1, 1f), Entry(e.slime, 3, 4, 2f), Entry(e.caveGnawer, 2, 2, 2f), Entry(e.wraith, 4, 6, 1f)
            };
            arena.baseBudget = 4f;
            arena.budgetPerWave = 2.5f;
            arena.maxAlive = 16;
            arena.bossEvery = 5;
            arena.bosses = new[] { e.stoneColossus, e.hornedTyrant };
            arena.clearRewards = new[] { EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 8, 14), EnemyPrefabBuilder.Drop("coal", 1f, 1, 3), EnemyPrefabBuilder.Drop("amethyst", 0.25f), EnemyPrefabBuilder.Drop("health_potion", 0.3f), EnemyPrefabBuilder.Drop("energy_potion", 0.25f) };
            return Save(arena);
        }

                private static ArenaDefinition BuildDungeon(Enemies e)
        {
            ArenaDefinition arena = GetOrCreate("old_dungeon", "Old Dungeon", "dungeon");
            arena.enemies = new[]
            {
                Entry(e.skeleton, 2, 1, 4f), Entry(e.spider, 1, 2, 2f), Entry(e.zombie, 3, 4, 2f), Entry(e.bat, 2, 3, 1f), Entry(e.hollowKnight, 4, 5, 1.5f), Entry(e.wraith, 4, 6, 1.5f)
            };
            arena.baseBudget = 5f;
            arena.budgetPerWave = 3f;
            arena.maxAlive = 16;
            arena.bossEvery = 5;
            arena.bosses = new[] { e.ashenDrake, e.stoneColossus };
            arena.clearRewards = new[] { EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 10, 18), EnemyPrefabBuilder.Drop("silver_ingot", 0.6f, 1, 2), EnemyPrefabBuilder.Drop("ruby", 0.1f), EnemyPrefabBuilder.Drop("health_potion", 0.3f), EnemyPrefabBuilder.Drop("energy_potion", 0.25f) };
            return Save(arena);
        }

        private static ArenaDefinition BuildFarm(Enemies e)
        {
            ArenaDefinition arena = GetOrCreate("haunted_farm", "Haunted Farm", "farm");
            arena.enemies = new[]
            {
                Entry(e.zombie, 3, 1, 3f), Entry(e.slime, 3, 1, 2f), Entry(e.brownRat, 2, 1, 2f), Entry(e.whiteRat, 1, 2, 1f), Entry(e.mossBrute, 4, 4, 1.5f), Entry(e.shadowBoar, 3, 3, 1.5f), Entry(e.wraith, 4, 7, 1f)
            };
            arena.baseBudget = 4f;
            arena.budgetPerWave = 2.5f;
            arena.maxAlive = 15;
            arena.bossEvery = 5;
            arena.bosses = new[] { e.ogreKing, e.thornBeast };
            arena.clearRewards = new[] { EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 7, 12), EnemyPrefabBuilder.Drop("apple", 0.5f, 1, 3), EnemyPrefabBuilder.Drop("orange", 0.4f, 1, 3), EnemyPrefabBuilder.Drop("health_potion", 0.3f), EnemyPrefabBuilder.Drop("energy_potion", 0.25f) };
            return Save(arena);
        }

        // Every wave is a single boss, and the arena itself changes between them (see ArenaStages).
        private static ArenaDefinition BuildMortalArena(Enemies e)
        {
            ArenaDefinition arena = GetOrCreate("mortal_arena", "Mortal Arena", MortalArenaBuilder.ThemeId);
            arena.enemies = new ArenaDefinition.EnemyEntry[0];
            arena.baseBudget = 0f;
            arena.budgetPerWave = 0f;
            arena.maxAlive = 3;
            arena.healthGrowth = 0.12f;
            arena.damageGrowth = 0.06f;
            arena.bossEvery = 1;
            arena.bosses = new[] { e.ogreKing, e.thornBeast, e.stoneColossus, e.ashenDrake, e.hornedTyrant };
            arena.clearRewards = new[]
            {
                EnemyPrefabBuilder.Drop(ProjectConstants.CurrencyId, 1f, 25, 40), EnemyPrefabBuilder.Drop("gold_ingot", 0.5f, 1, 2), EnemyPrefabBuilder.Drop("ruby", 0.2f), EnemyPrefabBuilder.Drop("health_potion", 0.8f), EnemyPrefabBuilder.Drop("energy_potion", 0.6f)
            };
            return Save(arena);
        }

        private static ArenaDefinition.EnemyEntry Entry(GameObject prefab, int cost, int firstWave, float weight)
        {
            return new ArenaDefinition.EnemyEntry { prefab = prefab, cost = cost, firstWave = firstWave, weight = weight };
        }

        private static ArenaDefinition GetOrCreate(string id, string displayName, string theme)
        {
            string path = Folder + "/" + id + ".asset";
            ArenaDefinition arena = AssetDatabase.LoadAssetAtPath<ArenaDefinition>(path);
            if (arena == null)
            {
                arena = ScriptableObject.CreateInstance<ArenaDefinition>();
                AssetDatabase.CreateAsset(arena, path);
            }

            arena.id = id;
            arena.displayName = displayName;
            arena.theme = theme;
            arena.sceneName = ProjectConstants.ArenaScenePrefix + id;
            arena.healthGrowth = 0.1f;
            arena.damageGrowth = 0.05f;
            arena.spawnInterval = 0.5f;
            arena.checkpointEvery = 5;
            return arena;
        }

        private static ArenaDefinition Save(ArenaDefinition arena)
        {
            EditorUtility.SetDirty(arena);
            return arena;
        }
    }
}
