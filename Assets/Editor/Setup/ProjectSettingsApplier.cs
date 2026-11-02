using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Applies the project settings a pixel-art Android game needs.
    /// </summary>
    public static class ProjectSettingsApplier
    {
        /// <summary>Sets orientation, Android build options and Y-based sprite sorting.</summary>
        public static void Apply()
        {
            PlayerSettings.productName = "Mobile RPG";

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

            // Sprites with a lower Y draw in front, which gives top-down depth.
            Renderer2DSortingConfigurator.Apply();

            AssetDatabase.SaveAssets();
        }
    }
}
