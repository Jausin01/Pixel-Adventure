using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the staff weapon. It starts inactive; the weapon holder activates it when a staff is worn.
    /// </summary>
    public static class StaffBuilder
    {
        private static readonly string[] Variants = { "oak", "amethyst", "ruby" };

        /// <summary>Builds the staff and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            HoverFollower follower;
            AimRotation aim;
            GameObject weapon = HeldWeaponFactory.Create("Staff", "staff", Variants, material, player, out follower, out aim);

            StaffAttack attack = weapon.AddComponent<StaffAttack>();
            attack.owner = player.transform;
            attack.facing = player.GetComponent<PlayerFacing>();
            attack.follower = follower;
            attack.aim = aim;
            attack.boltSprite = SpriteLoader.Load("Effects/magic_bolt");
            return weapon;
        }
    }
}
