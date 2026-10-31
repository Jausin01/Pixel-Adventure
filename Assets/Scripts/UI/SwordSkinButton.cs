using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that selects a sword color.
    /// </summary>
    public class SwordSkinButton : PointerDownAction
    {
        public SwordVisual sword;

        [Tooltip("Index of the skin this button selects.")]
        public int skinIndex;

        /// <inheritdoc/>
        protected override void Execute()
        {
            sword.SetSkin(skinIndex);
        }
    }
}
