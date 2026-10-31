using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that makes the player's current weapon attack.
    /// </summary>
    public class AttackButton : PointerDownAction
    {
        public WeaponHolder weapons;

        /// <inheritdoc/>
        protected override void Execute()
        {
            weapons.Attack();
        }
    }
}
