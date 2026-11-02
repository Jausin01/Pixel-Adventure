using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates (or updates) item tier assets under Assets/Data/Tiers.
    /// </summary>
    public static class ItemTierFactory
    {
        private const string Folder = "Assets/Data/Tiers";

        /// <summary>Returns the tier asset with the given id, creating it when missing.</summary>
        public static ItemTier GetOrCreate(string id, string displayName, Color color, int order)
        {
            AssetFolderUtil.Ensure(Folder);

            string path = Folder + "/" + id + ".asset";
            ItemTier tier = AssetDatabase.LoadAssetAtPath<ItemTier>(path);
            if (tier == null)
            {
                tier = ScriptableObject.CreateInstance<ItemTier>();
                AssetDatabase.CreateAsset(tier, path);
            }

            tier.displayName = displayName;
            tier.color = color;
            tier.order = order;
            EditorUtility.SetDirty(tier);
            return tier;
        }

        /// <summary>Loads a tier asset by id, or null.</summary>
        public static ItemTier Load(string id)
        {
            return AssetDatabase.LoadAssetAtPath<ItemTier>(Folder + "/" + id + ".asset");
        }
    }
}
