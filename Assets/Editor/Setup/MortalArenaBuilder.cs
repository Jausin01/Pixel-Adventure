using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Builds the scene of the Mortal Arena: one area holding the look of several other arenas stacked on top of each
    /// other. <see cref="ArenaStages"/> shows one at a time and transforms between them as the bosses fall.
    /// </summary>
    public static class MortalArenaBuilder
    {
        /// <summary>The theme id that marks an arena as a stacked one.</summary>
        public const string ThemeId = "mortal";

        // The look of each stage, in the order they appear. Wave N is fought in stage N - 1, then it wraps around.
        private static readonly string[] StageThemes = { "farm", "cornfield", "cave", "dungeon" };

        private static readonly Vector2 Size = new Vector2(24f, 18f);

        /// <summary>Builds the arena at a position in the current (empty) scene.</summary>
        public static void Build(Material material, ArenaDefinition arena, Vector2 center, int index)
        {
            GameObject root = new GameObject(arena.displayName);
            root.transform.position = center;

            Vector2 spawnPoint = new Vector2(0f, -Size.y / 2f + 3f);
            GameObject spawn = new GameObject("Player Spawn");
            spawn.transform.SetParent(root.transform, false);
            spawn.transform.localPosition = spawnPoint;

            ArenaSite site = root.AddComponent<ArenaSite>();
            site.arena = arena;
            site.playerSpawn = spawn.transform;
            site.halfSize = Size / 2f - Vector2.one;

            ArenaStages stages = root.AddComponent<ArenaStages>();
            stages.site = site;
            stages.sparkSprite = SpriteLoader.Load("Effects/spark");
            stages.sparkMaterial = material;
            stages.stages = new ArenaStages.Stage[StageThemes.Length];

            for (int i = 0; i < StageThemes.Length; i++)
            {
                ArenaThemes.Theme theme = ArenaThemes.Copy(ArenaThemes.Get(StageThemes[i]), Size);
                stages.stages[i] = BuildStage(root.transform, material, theme, spawnPoint, 200 + index * 10 + i);
            }

            site.ambientColor = stages.stages[0].ambientColor;
            site.ambientIntensity = stages.stages[0].ambientIntensity;
        }

        private static ArenaStages.Stage BuildStage(Transform parent, Material material, ArenaThemes.Theme theme, Vector2 spawnPoint, int seed)
        {
            GameObject stage = new GameObject("Stage " + theme.id);
            stage.transform.SetParent(parent, false);

            ArenaWorldBuilder.BuildFloor(stage.transform, material, theme);

            GameObject walls = new GameObject("Walls");
            walls.transform.SetParent(stage.transform, false);
            WorldBuilder.CreateWalls(walls.transform, theme.size);

            GameObject props = new GameObject("Props");
            props.transform.SetParent(stage.transform, false);
            theme.decorate(new ArenaScenery(props.transform, material, theme.size, spawnPoint, seed));

            stage.SetActive(false);
            return new ArenaStages.Stage { root = stage, props = props.transform, ambientColor = theme.ambientColor, ambientIntensity = theme.ambientIntensity };
        }
    }
}
