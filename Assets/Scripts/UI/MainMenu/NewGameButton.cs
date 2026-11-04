using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Erases the save and restarts the game. Needs a second tap within a few seconds to confirm.
    /// </summary>
    public class NewGameButton : PointerDownAction
    {
        public Text label;
        public string idleText = "New Game";
        public string confirmText = "Tap again to erase save";
        public float confirmSeconds = 3f;

        private float armedUntil;
        private bool armed;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (!armed)
            {
                armed = true;
                armedUntil = Time.unscaledTime + confirmSeconds;
                label.text = confirmText;
                return;
            }

            SaveFile.Delete();
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Update()
        {
            if (armed && Time.unscaledTime > armedUntil) Disarm();
        }

        private void OnDisable() { Disarm(); }

        private void Disarm()
        {
            armed = false;
            label.text = idleText;
        }
    }
}
