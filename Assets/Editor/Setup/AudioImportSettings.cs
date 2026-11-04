using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives every file under Assets/Audio mobile-friendly import settings:
    /// short effects are decompressed on load as mono; music streams from disk.
    /// </summary>
    public class AudioImportSettings : AssetPostprocessor
    {
        private void OnPreprocessAudio()
        {
            if (!assetPath.StartsWith("Assets/Audio/")) return;

            AudioImporter importer = (AudioImporter)assetImporter;
            bool music = assetPath.StartsWith("Assets/Audio/Music/");

            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            settings.loadType = music ? AudioClipLoadType.Streaming : AudioClipLoadType.DecompressOnLoad;
            settings.compressionFormat = AudioCompressionFormat.Vorbis;
            settings.quality = music ? 0.6f : 0.7f;
            settings.preloadAudioData = !music;
            importer.defaultSampleSettings = settings;
            importer.forceToMono = !music;
        }
    }
}
