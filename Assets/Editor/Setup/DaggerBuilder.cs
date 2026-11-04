using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the twin daggers weapon. It starts inactive; the weapon holder activates it when twin daggers are worn.
    /// </summary>
    public static class DaggerBuilder
    {
        private static readonly string[] Variants = { "iron", "steel", "ruby" };

        /// <summary>Builds the twin daggers and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            HoverFollower follower;
            AimRotation aim;
            GameObject weapon = HeldWeaponFactory.Create("Daggers", "dagger", Variants, material, player, out follower, out aim);

            DaggerAttack attack = weapon.AddComponent<DaggerAttack>();
            attack.owner = player.transform;
            attack.facing = player.GetComponent<PlayerFacing>();
            attack.follower = follower;
            attack.aim = aim;
            attack.hitStop = hitStop;
            return weapon;
        }
    }
}
