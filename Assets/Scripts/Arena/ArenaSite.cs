using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A place in the world where an arena run takes place: its rules, where the player arrives and how large it is.
    /// </summary>
    public class ArenaSite : MonoBehaviour
    {
        public ArenaDefinition arena;

        [Tooltip("Where the player appears when entering.")]
        public Transform playerSpawn;

        [Tooltip("Color of the ambient light while the player is here.")]
        public Color ambientColor = Color.white;

        [Tooltip("Brightness of the ambient light while the player is here (1 = fully lit).")]
        public float ambientIntensity = 1f;

        [Tooltip("Half of the area's width and height, measured from this object. Enemies spawn inside it.")]
        public Vector2 halfSize = new Vector2(14f, 9f);
    }
}
