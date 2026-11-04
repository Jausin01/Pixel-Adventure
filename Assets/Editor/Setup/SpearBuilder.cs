using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the spear weapon. It starts inactive; the weapon holder activates it when a spear is worn.
    /// </summary>
    public static class SpearBuilder
    {
        private static readonly string[] Metals = { "iron", "steel", "ruby" };

        /// <summary>Builds the spear and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            HoverFollower follower;
            AimRotation aim;
            GameObject spear = HeldWeaponFactory.Create("Spear", "spear", Metals, material, player, out follower, out aim);

            SpearAttack attack = spear.AddComponent<SpearAttack>();
            attack.owner = player.transform;
            attack.facing = player.GetComponent<PlayerFacing>();
            attack.follower = follower;
            attack.aim = aim;
            attack.hitStop = hitStop;
            return spear;
        }
    }
}
