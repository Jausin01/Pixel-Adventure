using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Automatically applies pixel-art import settings to every texture inside the art folder:
    /// point filtering, no compression and 16 pixels per unit. UI art uses smooth filtering and 100 pixels per unit.
    /// </summary>
    public class PixelArtImporter : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(ProjectConstants.ArtFolder)) return;

            TextureImporter importer = (TextureImporter)assetImporter;
            bool isUi = assetPath.Contains("/UI/");

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = isUi ? FilterMode.Bilinear : FilterMode.Point;
            importer.spritePixelsPerUnit = isUi ? 100 : ProjectConstants.PixelsPerUnit;

            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteAlignment = (int)SpriteAlignment.Center;

            bool bottomPivot = assetPath.Contains("/Player/") || assetPath.EndsWith("dummy.png");
            if (bottomPivot) settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;

            importer.SetTextureSettings(settings);
        }
    }
}
