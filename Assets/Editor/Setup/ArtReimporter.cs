using UnityEditor;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Forces every art asset to be imported again so <see cref="PixelArtImporter"/> rules are applied.
    /// </summary>
    public static class ArtReimporter
    {
        /// <summary>Reimports all assets in the art folder.</summary>
        public static void Reimport()
        {
            AssetDatabase.ImportAsset(ProjectConstants.ArtFolder, ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
        }
    }
}
