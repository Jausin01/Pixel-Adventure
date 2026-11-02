using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates (or updates) item definition assets under Assets/Data/Items.
    /// </summary>
    public static class ItemAssetFactory
    {
        private const string Folder = "Assets/Data/Items";

        /// <summary>Returns the item asset described by the spec, creating or updating it.</summary>
        public static ItemDefinition GetOrCreate(ItemSpec spec)
        {
            AssetFolderUtil.Ensure(Folder);

            string path = Folder + "/" + spec.id + ".asset";
            ItemDefinition item = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);
            if (item == null)
            {
                item = ScriptableObject.CreateInstance<ItemDefinition>();
                AssetDatabase.CreateAsset(item, path);
            }

            item.id = spec.id;
            item.displayName = spec.displayName;
            item.description = spec.description;
            item.icon = SpriteLoader.Load(spec.iconPath);
            item.category = spec.category;
            item.tier = ItemTierFactory.Load(spec.tierId);
            item.healthRestored = spec.healthRestored;
            item.energyRestored = spec.energyRestored;
            EditorUtility.SetDirty(item);
            return item;
        }

        /// <summary>Loads an existing item asset by id, or null.</summary>
        public static ItemDefinition Load(string id)
        {
            return AssetDatabase.LoadAssetAtPath<ItemDefinition>(Folder + "/" + id + ".asset");
        }
    }
}
