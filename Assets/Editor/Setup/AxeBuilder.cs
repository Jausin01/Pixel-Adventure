using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the axe weapon. It starts inactive; the weapon holder activates it when an axe is worn.
    /// </summary>
    public static class AxeBuilder
    {
        private static readonly string[] Metals = { "iron", "steel", "ruby" };

        /// <summary>Builds the axe and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            HoverFollower follower;
            AimRotation aim;
            GameObject axe = HeldWeaponFactory.Create("Axe", "axe", Metals, material, player, out follower, out aim);

            AxeAttack attack = axe.AddComponent<AxeAttack>();
            attack.owner = player.transform;
            attack.facing = player.GetComponent<PlayerFacing>();
            attack.follower = follower;
            attack.aim = aim;
            attack.hitStop = hitStop;
            return axe;
        }
    }
}
