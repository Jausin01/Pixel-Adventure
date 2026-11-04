using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Saves a monster as a prefab that the wave director can spawn: it does not respawn on its own,
    /// disappears after dying and drops items.
    /// </summary>
    public static class EnemyPrefabBuilder
    {
        private const string Folder = "Assets/Prefabs/Enemies";

        /// <summary>Creates (or replaces) the prefab and returns it.</summary>
        /// <param name="scale">Size multiplier, for bosses.</param>
        /// <param name="drops">What the monster can drop.</param>
        public static GameObject Build(MonsterSpec spec, Material material, float scale, DropEntry[] drops)
        {
            AssetFolderUtil.Ensure(Folder);

            GameObject monster = MonsterBuilder.Build(spec, material, null, Vector2.zero, false);
            monster.transform.localScale = Vector3.one * scale;
            monster.AddComponent<DespawnOnDeath>();

            EnemyIdentity identity = monster.AddComponent<EnemyIdentity>();
            identity.id = spec.name.ToLowerInvariant().Replace(" ", "_");
            identity.family = identity.id.Contains("rat") ? "rat" : identity.id;

            EnemyLootDropper loot = monster.AddComponent<EnemyLootDropper>();
            loot.drops = drops;
            loot.material = material;

            string path = Folder + "/" + spec.name.Replace(" ", string.Empty) + ".prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(monster, path);
            Object.DestroyImmediate(monster);
            return prefab;
        }

        /// <summary>Shortcut for a drop table entry using an item id from the catalog.</summary>
        public static DropEntry Drop(string itemId, float chance, int minAmount = 1, int maxAmount = 1)
        {
            return new DropEntry { item = ItemAssetFactory.Load(itemId), chance = chance, minAmount = minAmount, maxAmount = maxAmount };
        }
    }
}
