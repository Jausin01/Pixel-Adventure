using System.Collections.Generic;
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

            ItemCatalogBuilder.Build();
            SkillDefinition[] skills = SkillCatalogBuilder.Build();
            Material material = SpriteMaterialProvider.GetOrCreate();
            ArenaDefinition[] arenaDefinitions = ArenaCatalogBuilder.Build(material);
            QuestPools questPools = QuestPoolsBuilder.Build(arenaDefinitions);

            // The arena scenes come first: building them replaces the open scene, and the town is built last.
            ArenaWorldBuilder.Build(material, arenaDefinitions);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            UnityEngine.Rendering.Universal.Light2D[] streetLamps = TownWorldBuilder.Build(material);
            GameObject player = PlayerBuilder.Build(material);
            LightingBuilder.AttachPlayerLight(player);
            InventoryBuilder.Attach(player);
            StarterItemsBuilder.Attach(player);
            PlayerVitalsBuilder.Attach(player);

            SwordVisual swordVisual;
            WeaponHolder weapons = WeaponsBuilder.Build(material, player, out swordVisual);

            Equipment equipment = EquipmentBuilder.Attach(player, weapons);
            SkillSystemBuilder.Attach(player, weapons);
            SecondaryBuilder.Attach(material, player);
            HotbarBuilder.AttachQuickItem(player);
            QuestLog questLog = QuestLogBuilder.Attach(player, questPools);

            ToolHolder tools = ToolsBuilder.Build(material, player);
            ActionRouter router = ActionRouterBuilder.Build(player, weapons, tools);
            KeyboardInputBuilder.Build(router, player);

            ResourceWorldBuilder.Build(material);
            OreWorldBuilder.Build(material);
            TrainingDummyBuilder.Build(material);

            ArenaRun arenaRun = ArenaRunBuilder.Build(player);
            LightingBuilder.BuildTown(streetLamps, arenaRun);
            AudioBuilder.Build(player, arenaRun);
            Inventory stash = StashBuilder.CreateStorage();
            GameSaverBuilder.Attach(player, stash);
            CameraBuilder.Build(player.transform);

            Transform canvas = CanvasBuilder.Build();
            PlayerHudBuilder.Build(canvas, player);
            CurrencyHudBuilder.Build(canvas, player);
            VirtualJoystick joystick = JoystickBuilder.Build(canvas);
            player.GetComponent<PlayerMoveInput>().joystick = joystick;
            ActionButtonsBuilder.Build(canvas, router, player.GetComponent<NearbyHarvestDetector>(), player);

            ArenaHudBuilder.Build(canvas, arenaRun);

            InventoryPanel inventoryScreen = InventoryScreenBuilder.Build(canvas, player.GetComponent<Inventory>(), player.GetComponent<ItemUser>());
            InventoryPanel equipmentScreen = EquipmentScreenBuilder.Build(canvas, equipment, player.GetComponent<Inventory>(), player.GetComponent<CharacterStats>());
            InventoryPanel mainMenu = MainMenuBuilder.Build(canvas);
            MenuBuilder.Build(canvas, inventoryScreen, equipmentScreen, mainMenu);
            HotbarBuilder.Build(canvas, player);
            InventoryPanel portalScreen = ArenaPortalBuilder.Build(material, canvas, arenaRun, arenaDefinitions);
            InventoryPanel teacherScreen = SkillTeacherBuilder.Build(material, canvas, player, skills);
            InventoryPanel merchantScreen = MerchantBuilder.Build(material, canvas, player);
            InventoryPanel stashScreen = StashBuilder.Build(material, canvas, player, stash);
            InventoryPanel smithScreen = BlacksmithBuilder.Build(material, canvas, player);
            InventoryPanel questScreen = QuestBoardBuilder.Build(material, canvas, questLog, player.GetComponent<Inventory>());
            InventoryPanel doctorScreen = DoctorBuilder.Build(material, canvas, player);
            InventoryPanel saveScreen = SavePointBuilder.Build(material, canvas, player);
            InventoryPanel recordsScreen = RecordsHallBuilder.Build(material, canvas, arenaDefinitions);
            inventoryScreen.transform.SetAsLastSibling(); // draw the screens over every other control
            portalScreen.transform.SetAsLastSibling();
            questScreen.transform.SetAsLastSibling();
            teacherScreen.transform.SetAsLastSibling();
            merchantScreen.transform.SetAsLastSibling();
            smithScreen.transform.SetAsLastSibling();
            stashScreen.transform.SetAsLastSibling();
            doctorScreen.transform.SetAsLastSibling();
            saveScreen.transform.SetAsLastSibling();
            recordsScreen.transform.SetAsLastSibling();
            equipmentScreen.transform.SetAsLastSibling();
            mainMenu.transform.SetAsLastSibling(); // the main menu covers the game screens
            canvas.Find("Game Over").SetAsLastSibling(); // the game over screen covers everything else
            EventSystemBuilder.Build();

            DeviceSettingsBuilder.Build();
            Save(arenaDefinitions);
        }

        private static void Save(ArenaDefinition[] arenas)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProjectConstants.ScenePath));
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), ProjectConstants.ScenePath);
            RegisterScenes(arenas);
            AssetDatabase.SaveAssets();
        }

        /// <summary>Puts the town first in the build settings, followed by every arena scene.</summary>
        private static void RegisterScenes(ArenaDefinition[] arenas)
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>
            {
                new EditorBuildSettingsScene(ProjectConstants.ScenePath, true)
            };
            for (int i = 0; i < arenas.Length; i++)
            {
                scenes.Add(new EditorBuildSettingsScene(ArenaWorldBuilder.ScenePath(arenas[i]), true));
            }
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
