using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Loads sprites from the art folder by relative path.
    /// </summary>
    public static class SpriteLoader
    {
        /// <summary>Loads "Assets/Art/{relativePath}.png" (for example "Player/idle_down").</summary>
        public static Sprite Load(string relativePath)
        {
            string path = ProjectConstants.ArtFolder + "/" + relativePath + ".png";
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) Debug.LogWarning("Sprite not found: " + path);
            return sprite;
        }

        /// <summary>Loads numbered frames, for example ("Player/walk_down_", 4) loads walk_down_0..3.</summary>
        public static Sprite[] LoadFrames(string prefix, int count)
        {
            Sprite[] frames = new Sprite[count];
            for (int i = 0; i < count; i++) frames[i] = Load(prefix + i);
            return frames;
        }
    }
}
