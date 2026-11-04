using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Appears at each checkpoint and lets the player choose: fight the next wave or leave with the loot.
    /// This component must stay on an always-active object; the visible part is a separate child.
    /// </summary>
    public class WaveDecisionPanel : MonoBehaviour
    {
        public ArenaRun run;
        public WaveDirector director;
        public GameObject visual;
        public Text title;

        private void OnEnable()
        {
            run.WaveBreak += OnWaveBreak;
            director.WaveStarted += OnWaveStarted;
            run.Ended += Hide;
            visual.SetActive(false);
        }

        private void OnDisable()
        {
            run.WaveBreak -= OnWaveBreak;
            director.WaveStarted -= OnWaveStarted;
            run.Ended -= Hide;
        }

        private void OnWaveBreak(int wave, bool checkpoint)
        {
            if (!checkpoint) return;

            title.text = "Checkpoint! Wave " + wave + " cleared";
            visual.SetActive(true);
        }

        private void OnWaveStarted(int wave) { Hide(); }

        private void Hide()
        {
            visual.SetActive(false);
        }
    }
}
