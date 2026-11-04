using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the record of one arena on the portal screen.
    /// </summary>
    public class ArenaRowView : MonoBehaviour
    {
        public ArenaDefinition arena;
        public Text bestText;

        private void OnEnable()
        {
            bestText.text = "Best: " + ArenaRecords.Best(arena.id) + " waves";
        }
    }
}
