using UnityEditor;
using UnityEngine;

// Every texture under Assets/Art is imported as crisp pixel art (16 PPU, Point filter, no compression).
// UI textures (Assets/Art/UI) stay smooth at 100 PPU.
public class PixelArtImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        string p = assetPath.Replace('\\', '/');
        if (!p.StartsWith("Assets/Art/")) return;

        TextureImporter ti = (TextureImporter)assetImporter;
        bool ui = p.StartsWith("Assets/Art/UI/");

        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Single;
        ti.spritePixelsPerUnit = ui ? 100f : 16f;
        ti.filterMode = ui ? FilterMode.Bilinear : FilterMode.Point;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        ti.mipmapEnabled = false;
        ti.alphaIsTransparency = true;
        ti.npotScale = TextureImporterNPOTScale.None;

        TextureImporterSettings s = new TextureImporterSettings();
        ti.ReadTextureSettings(s);
        s.spriteMeshType = SpriteMeshType.FullRect;
        bool feet = p.StartsWith("Assets/Art/Player/") || p.EndsWith("/dummy.png");
        s.spriteAlignment = (int)(feet ? SpriteAlignment.BottomCenter : SpriteAlignment.Center);
        ti.SetTextureSettings(s);
    }
}
