using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the hammer weapon. It starts inactive; the weapon holder activates it when a hammer is worn.
    /// </summary>
    public static class HammerBuilder
    {
        private static readonly string[] Variants = { "iron", "steel", "ruby" };

        /// <summary>Builds the hammer and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            HoverFollower follower;
            AimRotation aim;
            GameObject weapon = HeldWeaponFactory.Create("Hammer", "hammer", Variants, material, player, out follower, out aim);

            HammerAttack attack = weapon.AddComponent<HammerAttack>();
            attack.owner = player.transform;
            attack.facing = player.GetComponent<PlayerFacing>();
            attack.follower = follower;
            attack.aim = aim;
            attack.hitStop = hitStop;
            attack.dustSprite = SpriteLoader.Load("Effects/dust");
            return weapon;
        }
    }
}
