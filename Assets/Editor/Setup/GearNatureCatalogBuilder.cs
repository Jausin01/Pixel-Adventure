using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Defines every gear nature (Sharp, Brittle...) as one asset. To add a nature, add a line to <see cref="BuildNatures"/>.
    /// </summary>
    public static class GearNatureCatalogBuilder
    {
        private const string Path = "Assets/Data/GearNatures.asset";

        /// <summary>Creates or updates the nature table.</summary>
        public static GearNatureTable Build()
        {
            GearNatureTable table = Load();
            if (table == null)
            {
                table = ScriptableObject.CreateInstance<GearNatureTable>();
                AssetDatabase.CreateAsset(table, Path);
            }

            table.natures = BuildNatures();
            EditorUtility.SetDirty(table);
            return table;
        }

        /// <summary>Loads the nature table, or null.</summary>
        public static GearNatureTable Load()
        {
            return AssetDatabase.LoadAssetAtPath<GearNatureTable>(Path);
        }

        // id, name, attack x, defense x, max health x, weight
        private static GearNature[] BuildNatures()
        {
            return new[]
            {
                Nature("plain", "Plain", 1f, 1f, 1f, 25f),
                Nature("sharp", "Sharp", 1.15f, 1f, 1f, 10f),
                Nature("savage", "Savage", 1.25f, 0.9f, 1f, 6f),
                Nature("heavy", "Heavy", 1.2f, 1f, 0.9f, 6f),
                Nature("sturdy", "Sturdy", 1f, 1.2f, 1f, 10f),
                Nature("guardian", "Guardian", 1f, 1.15f, 1.1f, 5f),
                Nature("vital", "Vital", 1f, 1f, 1.2f, 10f),
                Nature("mighty", "Mighty", 1.1f, 1.1f, 1.1f, 3f),
                Nature("legendary", "Legendary", 1.25f, 1.25f, 1.25f, 1f),
                Nature("dull", "Dull", 0.85f, 1f, 1f, 8f),
                Nature("brittle", "Brittle", 1f, 0.85f, 1f, 8f),
                Nature("frail", "Frail", 1f, 1f, 0.85f, 8f),
                Nature("shoddy", "Shoddy", 0.9f, 0.9f, 0.9f, 4f)
            };
        }

        private static GearNature Nature(string id, string name, float attack, float defense, float maxHealth, float weight)
        {
            return new GearNature { id = id, displayName = name, attack = attack, defense = defense, maxHealth = maxHealth, weight = weight };
        }
    }
}
