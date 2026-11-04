using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Lists every item, skill and arena of the game so a save file can refer to them by id.
    /// Filled by the editor setup; read-only at runtime.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Game Database", fileName = "GameDatabase")]
    public class GameDatabase : ScriptableObject
    {
        public ItemDefinition[] items;
        public SkillDefinition[] skills;
        public ArenaDefinition[] arenas;

        private Dictionary<string, ItemDefinition> itemsById;
        private Dictionary<string, SkillDefinition> skillsById;
        private Dictionary<string, ArenaDefinition> arenasById;

        /// <summary>The item with this id, or null.</summary>
        public ItemDefinition FindItem(string id)
        {
            if (itemsById == null) itemsById = Index(items, x => x.id);
            return Lookup(itemsById, id);
        }

        /// <summary>The skill with this id, or null.</summary>
        public SkillDefinition FindSkill(string id)
        {
            if (skillsById == null) skillsById = Index(skills, x => x.id);
            return Lookup(skillsById, id);
        }

        /// <summary>The arena with this id, or null.</summary>
        public ArenaDefinition FindArena(string id)
        {
            if (arenasById == null) arenasById = Index(arenas, x => x.id);
            return Lookup(arenasById, id);
        }

        private static Dictionary<string, T> Index<T>(T[] list, System.Func<T, string> idOf) where T : Object
        {
            Dictionary<string, T> map = new Dictionary<string, T>();
            for (int i = 0; list != null && i < list.Length; i++)
            {
                if (list[i] != null) map[idOf(list[i])] = list[i];
            }
            return map;
        }

        private static T Lookup<T>(Dictionary<string, T> map, string id) where T : class
        {
            T value;
            return id != null && map.TryGetValue(id, out value) ? value : null;
        }
    }
}
