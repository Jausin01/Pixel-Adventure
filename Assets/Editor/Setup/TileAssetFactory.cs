using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates (or updates) the Tile asset for a sprite of the art folder, so tilemaps can use it.
    /// </summary>
    public static class TileAssetFactory
    {
        private const string Folder = "Assets/Data/Tiles";

        /// <summary>Returns the tile for a sprite path such as "Tiles/grass_0" or "Arenas/cave/floor_1".</summary>
        public static Tile Get(string spritePath)
        {
            Directory.CreateDirectory(Folder);
            string path = Folder + "/" + spritePath.Replace('/', '_') + ".asset";

            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                AssetDatabase.CreateAsset(tile, path);
            }

            tile.sprite = SpriteLoader.Load(spritePath);
            tile.colliderType = Tile.ColliderType.None;
            EditorUtility.SetDirty(tile);
            return tile;
        }
    }
}
