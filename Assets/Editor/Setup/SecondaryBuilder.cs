using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player the secondary weapon thrower. Needs the equipment and the stats to exist already.
    /// </summary>
    public static class SecondaryBuilder
    {
        /// <summary>Adds the thrower to the player.</summary>
        public static SecondaryThrower Attach(Material material, GameObject player)
        {
            SecondaryThrower thrower = player.AddComponent<SecondaryThrower>();
            thrower.equipment = player.GetComponent<Equipment>();
            thrower.stats = player.GetComponent<CharacterStats>();
            thrower.facing = player.GetComponent<PlayerFacing>();
            thrower.health = player.GetComponent<Health>();
            thrower.material = material;
            thrower.arrowSprite = SpriteLoader.Load("Bow/arrow");
            return thrower;
        }
    }
}
