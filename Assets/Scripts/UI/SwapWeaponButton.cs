using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that switches to the next weapon.
    /// </summary>
    public class SwapWeaponButton : PointerDownAction
    {
        public WeaponHolder weapons;

        /// <inheritdoc/>
        protected override void Execute()
        {
            weapons.SwapNext();
        }
    }
}
