using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shown when the player dies in an arena: fades in a "game over" screen with the wave reached and the items lost,
    /// and keeps the player where they fell until they choose to return to the town.
    /// This component must stay on an always-active object; the visible part is a separate child.
    /// </summary>
    public class GameOverPanel : MonoBehaviour
    {
        public ArenaRun run;
        public PlayerRespawn respawn;
        public GameObject visual;
        public CanvasGroup group;
        public Text summary;

        [Tooltip("Seconds the screen takes to fade in.")]
        public float fadeTime = 0.8f;

        private Coroutine fade;

        private void OnEnable()
        {
            run.Died += OnDied;
            visual.SetActive(false);
        }

        private void OnDisable()
        {
            run.Died -= OnDied;
        }

        /// <summary>Hides the screen and lets the player go back to the town.</summary>
        public void Confirm()
        {
            if (!visual.activeSelf) return;

            visual.SetActive(false);
            respawn.HoldRespawn = false;
        }

        private void OnDied(int wave, int itemsLost)
        {
            respawn.HoldRespawn = true;
            summary.text = "Reached wave " + wave + "\nItems lost: " + itemsLost;

            visual.SetActive(true);
            if (fade != null) StopCoroutine(fade);
            fade = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            group.alpha = 0f;
            for (float elapsed = 0f; elapsed < fadeTime; elapsed += Time.unscaledDeltaTime)
            {
                group.alpha = elapsed / fadeTime;
                yield return null;
            }
            group.alpha = 1f;
        }
    }
}
