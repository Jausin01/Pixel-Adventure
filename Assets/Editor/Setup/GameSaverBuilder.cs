using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the game database (every item, skill and arena) and gives the player the component that saves and loads.
    /// Run after items, skills, arenas, the quest log and the run ledger exist.
    /// </summary>
    public static class GameSaverBuilder
    {
        private const string DatabasePath = "Assets/Data/GameDatabase.asset";

        /// <summary>Adds the saver to the player. The stash storage is saved with it.</summary>
        public static void Attach(GameObject player, Inventory stash)
        {
            GameSaver saver = player.AddComponent<GameSaver>();
            saver.database = BuildDatabase();
            saver.ledger = player.GetComponent<RunLedger>();
            saver.stash = stash;
        }

        private static GameDatabase BuildDatabase()
        {
            GameDatabase database = AssetDatabase.LoadAssetAtPath<GameDatabase>(DatabasePath);
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<GameDatabase>();
                AssetDatabase.CreateAsset(database, DatabasePath);
            }

            database.items = FindAll<ItemDefinition>("Assets/Data/Items");
            database.skills = FindAll<SkillDefinition>("Assets/Data/Skills");
            database.arenas = FindAll<ArenaDefinition>("Assets/Data/Arenas");
            EditorUtility.SetDirty(database);
            return database;
        }

        private static T[] FindAll<T>(string folder) where T : Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folder });
            T[] found = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) found[i] = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[i]));
            return found;
        }
    }
}
