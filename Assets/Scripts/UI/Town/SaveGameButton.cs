using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Touch button on the save point's screen: writes the save now and tells the player the result.
    /// </summary>
    public class SaveGameButton : PointerDownAction
    {
        public GameSaver saver;
        public Text statusText;

        /// <inheritdoc/>
        protected override void Execute()
        {
            bool saved = saver.SaveNow();
            Sfx.Play(saved ? "save_bell" : "ui_error");
            statusText.text = saved ? "Game saved." : "You cannot save during a run.";
        }
    }
}
