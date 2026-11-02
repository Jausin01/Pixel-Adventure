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

            ItemCatalogBuilder.Build();
            Material material = SpriteMaterialProvider.GetOrCreate();

            WorldBuilder.Build(material);
            GameObject player = PlayerBuilder.Build(material);
            InventoryBuilder.Attach(player);
            StarterItemsBuilder.Attach(player);
            PlayerVitalsBuilder.Attach(player);

            SwordVisual swordVisual;
            WeaponHolder weapons = WeaponsBuilder.Build(material, player, out swordVisual);

            ToolHolder tools = ToolsBuilder.Build(material, player);
            ActionRouter router = ActionRouterBuilder.Build(player, weapons, tools);
            KeyboardInputBuilder.Build(weapons, router);

            ResourceWorldBuilder.Build(material);
            OreWorldBuilder.Build(material);
            TrainingDummyBuilder.Build(material);
            MonsterWorldBuilder.Build(material, player.transform);
            CameraBuilder.Build(player.transform);

            Transform canvas = CanvasBuilder.Build();
            PlayerHudBuilder.Build(canvas, player);
            VirtualJoystick joystick = JoystickBuilder.Build(canvas);
            player.GetComponent<PlayerMoveInput>().joystick = joystick;
            ActionButtonsBuilder.Build(canvas, router, player.GetComponent<NearbyHarvestDetector>());

            InventoryPanel inventoryScreen = InventoryScreenBuilder.Build(canvas, player.GetComponent<Inventory>(), player.GetComponent<ItemUser>());
            MenuBuilder.Build(canvas, weapons, inventoryScreen);
            inventoryScreen.transform.SetAsLastSibling(); // draw the inventory over every other control
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
