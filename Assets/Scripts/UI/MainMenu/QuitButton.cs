using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Touch button that closes the game (stops Play mode in the editor).
    /// </summary>
    public class QuitButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override void Execute()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
