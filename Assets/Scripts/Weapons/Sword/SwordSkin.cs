using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The set of sprites that make up one sword color. Only three drawn orientations are needed:
    /// the other directions are produced by flipping them.
    /// </summary>
    [System.Serializable]
    public class SwordSkin
    {
        [Tooltip("Display name, for example the color.")]
        public string name;

        [Tooltip("Blade pointing up.")]
        public Sprite up;

        [Tooltip("Blade pointing up and to the right.")]
        public Sprite diagonal;

        [Tooltip("Stretched diagonal frame used as a motion smear while striking.")]
        public Sprite smear;

        [Tooltip("Blade pointing right.")]
        public Sprite side;
    }
}
