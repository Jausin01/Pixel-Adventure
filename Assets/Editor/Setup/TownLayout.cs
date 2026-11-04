using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Where everything in the town stands. Units are world units, y points up.
    /// The plaza with the portal sits in the middle, the shops line the north road and the yard, storehouse and ores fill the south.
    /// </summary>
    public static class TownLayout
    {
        /// <summary>Size of the walkable town.</summary>
        public static readonly Vector2 Size = new Vector2(36f, 22f);

        /// <summary>How far the grass reaches beyond the walls, so the camera never sees the void.</summary>
        public const int GrassMargin = 9;

        public static readonly Vector2 PlazaCenter = new Vector2(0f, -2f);
        public const float PlazaRadius = 4f;

        /// <summary>Vertical range of the east-west road, as its bottom and top edges.</summary>
        public static readonly Vector2 RoadY = new Vector2(1.5f, 3.5f);

        /// <summary>Horizontal range of the north-south road, as its left and right edges.</summary>
        public static readonly Vector2 RoadX = new Vector2(-1f, 1f);

        public static readonly Vector2 PlayerSpawn = new Vector2(0f, -5f);
        public static readonly Vector2 Portal = PlazaCenter + new Vector2(0f, -0.4f);

        // Buildings (base center) along the north side, facing south.
        public const float NorthBaseY = 6.5f;
        public static readonly Vector2 GuildHall = new Vector2(-12.5f, NorthBaseY);
        public static readonly Vector2 Dojo = new Vector2(-4.5f, NorthBaseY);
        public static readonly Vector2 Market = new Vector2(4.5f, NorthBaseY);
        public static readonly Vector2 Forge = new Vector2(12.5f, NorthBaseY);
        public static readonly Vector2 Storehouse = new Vector2(-12f, -4f);

        /// <summary>Height of the people standing in front of the shops: on the doorstep, away from the road.</summary>
        public const float NpcY = 5.3f;

        // People and objects standing in front of their buildings.
        public static readonly Vector2 QuestBoard = PlazaCenter + new Vector2(3.6f, -0.6f);
        public static readonly Vector2 SkillTeacher = new Vector2(-4.5f, NpcY);
        public static readonly Vector2 Merchant = new Vector2(4.5f, NpcY);
        public static readonly Vector2 Blacksmith = new Vector2(12.5f, NpcY);
        public static readonly Vector2 Anvil = new Vector2(15.2f, NpcY - 0.1f);
        public static readonly Vector2 Doctor = PlazaCenter + new Vector2(2.4f, -2.6f);
        public static readonly Vector2 SavePoint = PlazaCenter + new Vector2(-2.4f, -2.6f);
        public static readonly Vector2 RecordsHall = PlazaCenter + new Vector2(-3.6f, -0.6f);
        public static readonly Vector2 Stash = new Vector2(-12f, -5.4f);

        // Training yard in the south-east corner, fenced except on its west side.
        public static readonly Rect Yard = new Rect(9f, -9f, 8f, 5f);
        public static readonly Vector2 DummyA = new Vector2(11.5f, -6.6f);
        public static readonly Vector2 DummyB = new Vector2(14.5f, -6.6f);

        // Ores in a block in the south-west corner.
        public static readonly Vector2 OreOrigin = new Vector2(-16.5f, -8.5f);
        public const float OreSpacingX = 1.5f;
        public const float OreSpacingY = 1.5f;
        public const int OreColumns = 5;

        /// <summary>World position of the ore with the given index (filled row by row from the origin).</summary>
        public static Vector2 OrePosition(int index)
        {
            return OreOrigin + new Vector2((index % OreColumns) * OreSpacingX, -(index / OreColumns) * OreSpacingY);
        }

        public static readonly Vector2[] Trees =
        {
            new Vector2(-16.5f, 0.5f), new Vector2(16.5f, 0.5f), new Vector2(16.8f, -2.2f),
            new Vector2(-4.5f, -8.5f), new Vector2(5f, -8.5f)
        };

        public static readonly Vector2[] Lamps =
        {
            new Vector2(-4.8f, -0.2f), new Vector2(4.8f, -0.2f), new Vector2(-4.8f, -3.8f), new Vector2(4.8f, -3.8f)
        };
    }
}
