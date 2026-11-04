using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Provides the material used by every sprite. It reacts to 2D lights, so a sprite is only as bright as the light on it.
    /// </summary>
    public static class SpriteMaterialProvider
    {
        private const string Path = ProjectConstants.ArtFolder + "/SpriteUnlit.mat";
        private const string ShaderName = "Universal Render Pipeline/2D/Sprite-Lit-Default";

        /// <summary>Returns the shared sprite material, creating it or switching it to the lit shader when needed.</summary>
        public static Material GetOrCreate()
        {
            Shader shader = Shader.Find(ShaderName);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(Path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Path);
            }
            else if (material.shader != shader)
            {
                material.shader = shader;
                EditorUtility.SetDirty(material);
            }
            return material;
        }
    }
}
