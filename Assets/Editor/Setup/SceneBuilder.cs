using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Builds the test scene by calling each specialized builder in order.
    /// It only orchestrates; every object type is created by its own builder.
    /// </summary>
    public static class SceneBuilder
    {
        /// <summary>Creates, saves and registers the test scene.</summary>
        public static void Build()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            Material material = SpriteMaterialProvider.GetOrCreate();

            WorldBuilder.Build(material);
            GameObject player = PlayerBuilder.Build(material);

            SwordVisual swordVisual;
            WeaponHolder weapons = WeaponsBuilder.Build(material, player, out swordVisual);

            EnemyBuilder.Build(material);
            CameraBuilder.Build(player.transform);

            Transform canvas = CanvasBuilder.Build();
            VirtualJoystick joystick = JoystickBuilder.Build(canvas);
            player.GetComponent<PlayerMoveInput>().joystick = joystick;
            ActionButtonsBuilder.Build(canvas, weapons, swordVisual);
            EventSystemBuilder.Build();

            DeviceSettingsBuilder.Build();
            Save();
        }

        private static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProjectConstants.ScenePath));
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), ProjectConstants.ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ProjectConstants.ScenePath, true) };
            AssetDatabase.SaveAssets();
        }
    }
}
