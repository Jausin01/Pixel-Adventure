using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Provides the unlit material used by every sprite (no lighting needed in this game).
    /// </summary>
    public static class SpriteMaterialProvider
    {
        private const string Path = ProjectConstants.ArtFolder + "/SpriteUnlit.mat";

        /// <summary>Returns the shared sprite material, creating the asset when missing.</summary>
        public static Material GetOrCreate()
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(Path);
            if (material != null) return material;

            material = new Material(Shader.Find("Sprites/Default"));
            AssetDatabase.CreateAsset(material, Path);
            return material;
        }
    }
}
