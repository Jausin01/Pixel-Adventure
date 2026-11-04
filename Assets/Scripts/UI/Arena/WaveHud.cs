using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the current wave number, the next checkpoint and the arena's record while a run is active.
    /// This component must stay on an always-active object; the visible part is a separate child.
    /// </summary>
    public class WaveHud : MonoBehaviour
    {
        public ArenaRun run;
        public WaveDirector director;
        public GameObject visual;
        public Text label;

        private void OnEnable()
        {
            run.Entered += OnEntered;
            run.Ended += OnEnded;
            director.WaveStarted += OnWaveStarted;
            run.WaveBreak += OnWaveBreak;
            visual.SetActive(false);
        }

        private void OnDisable()
        {
            run.Entered -= OnEntered;
            run.Ended -= OnEnded;
            director.WaveStarted -= OnWaveStarted;
            run.WaveBreak -= OnWaveBreak;
        }

        private void OnEntered(ArenaSite site)
        {
            visual.SetActive(true);
            label.text = site.arena.displayName;
        }

        private void OnEnded()
        {
            visual.SetActive(false);
        }

        private void OnWaveStarted(int wave)
        {
            ArenaDefinition arena = run.Current.arena;
            string text = "Wave " + wave;

            int every = arena.checkpointEvery;
            if (every > 0) text += "   Checkpoint " + (wave + every - 1) / every * every;

            label.text = text + "   Best " + ArenaRecords.Best(arena.id);
        }

        private void OnWaveBreak(int wave, bool checkpoint)
        {
            label.text = "Wave " + wave + " cleared";
        }
    }
}
