using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates one scene per arena, dressed with its theme: tiled floor, a decorative border, invisible walls,
    /// props and the ArenaSite that describes it. The scenes are loaded additively when a run starts,
    /// far from the town and from each other so they never overlap.
    /// </summary>
    public static class ArenaWorldBuilder
    {
        private const float Spacing = 80f;
        private const int BorderCells = 10;

        /// <summary>Builds and saves the scene of every arena definition. Leaves an empty scene open.</summary>
        public static void Build(Material material, ArenaDefinition[] arenas)
        {
            Directory.CreateDirectory(ProjectConstants.ArenaSceneFolder);

            for (int i = 0; i < arenas.Length; i++)
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                BuildArena(material, arenas[i], new Vector2(Spacing * (i + 1), 0f), i);
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), ScenePath(arenas[i]));
            }
        }

        /// <summary>Path of the saved scene of an arena.</summary>
        public static string ScenePath(ArenaDefinition arena)
        {
            return ProjectConstants.ArenaSceneFolder + "/" + arena.sceneName + ".unity";
        }

        private static void BuildArena(Material material, ArenaDefinition arena, Vector2 center, int index)
        {
            if (arena.theme == MortalArenaBuilder.ThemeId)
            {
                MortalArenaBuilder.Build(material, arena, center, index);
                return;
            }

            ArenaThemes.Theme theme = ArenaThemes.Get(arena.theme);

            GameObject root = new GameObject(arena.displayName);
            root.transform.position = center;

            BuildFloor(root.transform, material, theme);
            WorldBuilder.CreateWalls(root.transform, theme.size);

            Vector2 spawnPoint = new Vector2(0f, -theme.size.y / 2f + 3f);
            GameObject spawn = new GameObject("Player Spawn");
            spawn.transform.SetParent(root.transform, false);
            spawn.transform.localPosition = spawnPoint;

            ArenaSite site = root.AddComponent<ArenaSite>();
            site.arena = arena;
            site.playerSpawn = spawn.transform;
            site.ambientColor = theme.ambientColor;
            site.ambientIntensity = theme.ambientIntensity;
            site.halfSize = theme.size / 2f - Vector2.one;

            GameObject props = new GameObject("Props");
            props.transform.SetParent(root.transform, false);
            theme.decorate(new ArenaScenery(props.transform, material, theme.size, spawnPoint, 100 + index));
        }

        // Floor tiles inside the play area, border tiles outside it so the camera never sees empty space.
        public static void BuildFloor(Transform parent, Material material, ArenaThemes.Theme theme)
        {
            GameObject gridObject = new GameObject("Ground");
            gridObject.transform.SetParent(parent, false);
            gridObject.AddComponent<Grid>();

            GameObject mapObject = new GameObject("Tilemap");
            mapObject.transform.SetParent(gridObject.transform, false);
            Tilemap map = mapObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = mapObject.AddComponent<TilemapRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = -100;

            Tile[] floor = new Tile[theme.floorVariants];
            for (int i = 0; i < floor.Length; i++) floor[i] = TileAssetFactory.Get("Arenas/" + theme.id + "/floor_" + i);
            Tile[] wall = new Tile[theme.wallVariants];
            for (int i = 0; i < wall.Length; i++) wall[i] = TileAssetFactory.Get("Arenas/" + theme.id + "/wall_" + i);

            int halfX = Mathf.CeilToInt(theme.size.x / 2f) + BorderCells;
            int halfY = Mathf.CeilToInt(theme.size.y / 2f) + BorderCells;
            System.Random random = new System.Random(5);

            for (int x = -halfX; x < halfX; x++)
            {
                for (int y = -halfY; y < halfY; y++)
                {
                    bool inside = Mathf.Abs(x + 0.5f) < theme.size.x / 2f && Mathf.Abs(y + 0.5f) < theme.size.y / 2f;
                    Tile[] set = inside ? floor : wall;
                    map.SetTile(new Vector3Int(x, y, 0), set[random.Next(set.Length)]);
                }
            }
        }
    }
}
