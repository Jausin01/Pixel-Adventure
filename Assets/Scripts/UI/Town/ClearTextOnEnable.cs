using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Empties a text every time the screen it belongs to is opened (a result message that should not linger).
    /// </summary>
    public class ClearTextOnEnable : MonoBehaviour
    {
        public Text text;

        private void OnEnable()
        {
            text.text = string.Empty;
        }
    }
}
