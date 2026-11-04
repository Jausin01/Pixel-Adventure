using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// The look of each kind of arena: size, ground and border tiles, and its props.
    /// An arena picks a theme by id (<see cref="ArenaDefinition.theme"/>). To add a theme, generate its art into
    /// Assets/Art/Arenas/{id}/ and add an entry here.
    /// </summary>
    public static class ArenaThemes
    {
        /// <summary>Description of one theme.</summary>
        public class Theme
        {
            public string id;
            public Vector2 size;
            public int floorVariants = 4;
            public int wallVariants = 2;
            public Color ambientColor = Color.white;
            public float ambientIntensity = 1f;
            public Action<ArenaScenery> decorate;
        }

        private static readonly Dictionary<string, Theme> All = new Dictionary<string, Theme>
        {
            { "cave", new Theme { id = "cave", size = new Vector2(28f, 20f), ambientColor = new Color(0.5f, 0.6f, 1f), ambientIntensity = 0.3f, decorate = Cave } },
            { "dungeon", new Theme { id = "dungeon", size = new Vector2(24f, 18f), ambientColor = new Color(0.72f, 0.66f, 0.95f), ambientIntensity = 0.36f, decorate = Dungeon } },
            { "cornfield", new Theme { id = "cornfield", size = new Vector2(34f, 22f), ambientColor = new Color(1f, 0.76f, 0.52f), ambientIntensity = 0.8f, decorate = Cornfield } },
            { "farm", new Theme { id = "farm", size = new Vector2(32f, 22f), ambientColor = new Color(0.55f, 0.65f, 1f), ambientIntensity = 0.42f, decorate = Farm } }
        };

        /// <summary>The theme with this id. Falls back to the cave when it does not exist.</summary>
        public static Theme Get(string id)
        {
            Theme theme;
            if (id != null && All.TryGetValue(id, out theme)) return theme;

            Debug.LogWarning("Unknown arena theme '" + id + "', using cave.");
            return All["cave"];
        }

        /// <summary>A copy of a theme with another size (used by arenas that stack several themes in one area).</summary>
        public static Theme Copy(Theme source, Vector2 size)
        {
            return new Theme
            {
                id = source.id, size = size, floorVariants = source.floorVariants, wallVariants = source.wallVariants,
                ambientColor = source.ambientColor, ambientIntensity = source.ambientIntensity, decorate = source.decorate
            };
        }

        private static string Art(string theme, string name)
        {
            return "Arenas/" + theme + "/" + name;
        }

        private static readonly ArenaScenery.PropLight CrystalGlow = new ArenaScenery.PropLight { color = new Color(0.4f, 0.7f, 1f), intensity = 1.3f, radius = 4f, height = 0.7f };
        private static readonly ArenaScenery.PropLight MushroomGlow = new ArenaScenery.PropLight { color = new Color(0.3f, 1f, 0.9f), intensity = 0.8f, radius = 2.5f, height = 0.4f };
        private static readonly ArenaScenery.PropLight FireGlow = new ArenaScenery.PropLight { color = new Color(1f, 0.6f, 0.25f), intensity = 1.5f, radius = 5.5f, flicker = 0.2f, height = 1.4f };
        private static readonly ArenaScenery.PropLight BarnGlow = new ArenaScenery.PropLight { color = new Color(1f, 0.8f, 0.5f), intensity = 1.1f, radius = 5f, flicker = 0.08f, height = 0.9f };

        private static void Cave(ArenaScenery s)
        {
            s.ScatterSolid(Art("cave", "stalagmite"), 10, new Vector2(0.6f, 0.4f), 3f);
            s.ScatterSolid(Art("cave", "rock"), 8, new Vector2(0.8f, 0.4f), 2.5f);
            s.ScatterSolid(Art("cave", "crystal"), 5, new Vector2(0.6f, 0.4f), 4f, CrystalGlow);
            s.ScatterDecor(new[] { Art("cave", "mushrooms") }, 12, MushroomGlow);
        }

        private static void Dungeon(ArenaScenery s)
        {
            foreach (float x in new[] { -6f, 6f })
            {
                foreach (float y in new[] { -2.5f, 3.5f }) s.Solid(Art("dungeon", "pillar"), new Vector2(x, y), new Vector2(0.9f, 0.5f));
            }
            foreach (float x in new[] { -10f, 10f })
            {
                foreach (float y in new[] { -6.5f, 6.5f }) s.Solid(Art("dungeon", "brazier"), new Vector2(x, y), new Vector2(0.6f, 0.4f), FireGlow);
            }
            foreach (float x in new[] { -6f, 0f, 6f }) s.Solid(Art("dungeon", "banner"), new Vector2(x, 7.8f), new Vector2(0.4f, 0.3f));

            s.ScatterSolid(Art("dungeon", "rubble"), 6, new Vector2(0.8f, 0.4f), 3f);
            s.ScatterDecor(new[] { Art("dungeon", "bones") }, 9);
        }

        private static void Cornfield(ArenaScenery s)
        {
            string[] corn = { Art("cornfield", "corn_a"), Art("cornfield", "corn_b") };
            for (int i = 0; i < 30; i++) s.ScatterSolid(corn[i % 2], 1, new Vector2(0.9f, 0.4f), 2.2f);

            s.ScatterSolid(Art("cornfield", "scarecrow"), 2, new Vector2(0.5f, 0.3f), 5f);
            s.ScatterSolid(Art("cornfield", "hay"), 6, new Vector2(0.9f, 0.4f), 3f);
            s.ScatterDecor(new[] { Art("cornfield", "pumpkin") }, 8);
        }

        private static void Farm(ArenaScenery s)
        {
            s.Solid(Art("farm", "barn"), new Vector2(0f, 7.2f), new Vector2(3.6f, 2.6f), BarnGlow);

            // Two short fences leave a gap in the middle of the yard.
            for (int i = 0; i < 5; i++)
            {
                foreach (float side in new[] { -1f, 1f })
                {
                    s.Solid("Town/fence_h", new Vector2(side * (3.5f + i), 1.5f), new Vector2(1f, 0.3f));
                }
            }

            CornPlot(s, new Vector2(-9f, -4f), 6, 4);
            CornPlot(s, new Vector2(9f, -4f), 6, 4);
            CornPlot(s, new Vector2(0f, -7f), 7, 2);

            s.ScatterSolid(Art("farm", "hay_round"), 7, new Vector2(0.9f, 0.4f), 3f);
            s.ScatterSolid(Art("farm", "trough"), 3, new Vector2(1.2f, 0.4f), 4f);
            s.ScatterSolid(Art("farm", "cart"), 3, new Vector2(1.1f, 0.4f), 4f);
            s.ScatterSolid("Town/barrel", 4, new Vector2(0.6f, 0.4f), 3f);
            s.ScatterDecor(new[] { "Town/flowers_red", "Town/flowers_yellow", "Town/flowers_blue" }, 24);
        }

        // A tilled plot: dirt furrows with tall corn rows. The corn is solid, so lanes between rows can be walked.
        private static void CornPlot(ArenaScenery s, Vector2 center, int columns, int rows)
        {
            for (int cx = 0; cx < columns; cx++)
            {
                for (int ry = 0; ry < rows; ry++)
                {
                    Vector2 pos = center + new Vector2((cx - (columns - 1) * 0.5f) * 1f, (ry - (rows - 1) * 0.5f) * 1f);
                    s.Decor(Art("farm", "furrow"), pos);
                    if (cx % 2 == 0) s.Solid(Art("farm", "corn_row"), pos + new Vector2(0f, -0.4f), new Vector2(0.5f, 0.3f));
                }
            }
        }
    }
}
