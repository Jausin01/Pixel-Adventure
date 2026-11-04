using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Connects the arena run to the stages of an arena that has them (the Mortal Arena): shows the first stage when the
    /// run starts and transforms into the next one a moment after each wave is cleared.
    /// </summary>
    public class ArenaStageController : MonoBehaviour
    {
        public ArenaRun run;
        public WaveDirector director;
        public Transform player;

        [Tooltip("Seconds after a wave is cleared before the arena starts to change.")]
        public float delay = 0.3f;

        private ArenaStages stages;
        private CameraFollow cameraFollow;

        private void OnEnable()
        {
            run.Entered += OnEntered;
            run.Ended += OnEnded;
            director.WaveCleared += OnWaveCleared;
        }

        private void OnDisable()
        {
            run.Entered -= OnEntered;
            run.Ended -= OnEnded;
            director.WaveCleared -= OnWaveCleared;
        }

        private void OnEntered(ArenaSite site)
        {
            stages = site.GetComponentInChildren<ArenaStages>();
            if (stages == null) return;

            stages.ShowImmediate(0);
            if (cameraFollow == null && Camera.main != null) cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }

        private void OnEnded()
        {
            stages = null;
        }

        // Wave N was fought in stage (N - 1); wave N + 1 is fought in stage N.
        private void OnWaveCleared(int wave)
        {
            if (stages == null) return;
            StartCoroutine(MorphAfterDelay(wave));
        }

        private System.Collections.IEnumerator MorphAfterDelay(int wave)
        {
            yield return new WaitForSeconds(delay);
            if (stages != null) stages.MorphTo(wave, player, cameraFollow);
        }
    }
}
