using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Places monsters in the test scene, outside their sight range of the player's start point.
    /// </summary>
    public static class MonsterWorldBuilder
    {
        /// <summary>Builds the monsters.</summary>
        public static void Build(Material material, Transform player)
        {
            MonsterBuilder.Build(MonsterSpec.BrownRat(), material, player, new Vector2(7f, -1f));
            MonsterBuilder.Build(MonsterSpec.WhiteRat(), material, player, new Vector2(-7f, -2f));
            MonsterBuilder.Build(MonsterSpec.Bat(), material, player, new Vector2(3f, 7f));
        }
    }
}
