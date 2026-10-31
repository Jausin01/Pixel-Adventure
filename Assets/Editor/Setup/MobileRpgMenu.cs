using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Menu entries under Tools / Mobile RPG. Contains no logic: each entry calls one setup class.
    /// </summary>
    public static class MobileRpgMenu
    {
        [MenuItem("Tools/Mobile RPG/Setup Everything")]
        public static void SetupEverything()
        {
            ProjectSettingsApplier.Apply();
            ArtReimporter.Reimport();
            SceneBuilder.Build();
            Debug.Log("Mobile RPG: setup finished. Press Play.");
        }

        [MenuItem("Tools/Mobile RPG/1 - Apply Project Settings")]
        public static void ApplySettings() { ProjectSettingsApplier.Apply(); }

        [MenuItem("Tools/Mobile RPG/2 - Reimport Art")]
        public static void ReimportArt() { ArtReimporter.Reimport(); }

        [MenuItem("Tools/Mobile RPG/3 - Build Test Scene")]
        public static void BuildScene() { SceneBuilder.Build(); }
    }
}
