using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobileRpg
{
    /// <summary>
    /// The flow of one arena run: entering from the town, fighting wave after wave, then leaving by choice
    /// (keeping everything) or dying (losing a share of what was collected during the run).
    /// </summary>
    public class ArenaRun : MonoBehaviour
    {
        public Transform player;
        public WaveDirector director;
        public RunLedger ledger;

        [Tooltip("Share of the items collected during the run that is lost on death (0 to 1).")]
        [Range(0f, 1f)]
        public float loseFractionOnDeath = 0.5f;

        [Tooltip("Seconds between arriving and the first wave.")]
        public float firstWaveDelay = 2f;

        [Tooltip("Seconds between a cleared wave and the next one when it is not a checkpoint.")]
        public float breakTime = 3f;

        /// <summary>The arena being played, or null in the town.</summary>
        public ArenaSite Current { get; private set; }

        /// <summary>True when the player may leave right now: between waves, at a checkpoint (or before the first wave).</summary>
        public bool CanLeave
        {
            get { return Current != null && !director.WaveInProgress && IsCheckpoint(director.Wave); }
        }

        /// <summary>Raised when the player enters an arena.</summary>
        public event Action<ArenaSite> Entered;

        /// <summary>
        /// Raised when a wave is cleared. Arguments: the wave number and whether this is a checkpoint
        /// (where the player may leave). Away from checkpoints the next wave starts by itself.
        /// </summary>
        public event Action<int, bool> WaveBreak;

        /// <summary>Raised when the player dies in an arena. Arguments: the wave reached and how many items were lost.</summary>
        public event Action<int, int> Died;

        /// <summary>Raised when the run ends, whether by leaving or dying.</summary>
        public event Action Ended;

        private Health health;
        private Rigidbody2D body;
        private Vector2 returnPoint;
        private bool loading;
        private string pendingUnload;

        private void Awake()
        {
            health = player.GetComponent<Health>();
            body = player.GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (health == null) health = player.GetComponent<Health>();
            health.Died += OnPlayerDied;
            health.Revived += UnloadPending; // the arena stays on screen until the player is back in town
            director.WaveCleared += OnWaveCleared;
        }

        private void OnDisable()
        {
            health.Died -= OnPlayerDied;
            health.Revived -= UnloadPending;
            director.WaveCleared -= OnWaveCleared;
        }

        /// <summary>Loads the arena's scene, moves the player into it and starts the run.</summary>
        public void Enter(ArenaDefinition arena)
        {
            if (Current != null || loading) return;
            StartCoroutine(EnterRoutine(arena));
        }

        private IEnumerator EnterRoutine(ArenaDefinition arena)
        {
            loading = true;
            ResolvePendingUnload(arena.sceneName);

            if (!SceneManager.GetSceneByName(arena.sceneName).isLoaded)
            {
                AsyncOperation load = SceneManager.LoadSceneAsync(arena.sceneName, LoadSceneMode.Additive);
                while (!load.isDone) yield return null;
            }

            ArenaSite site = FindSite(arena.sceneName);
            loading = false;
            if (site == null)
            {
                Debug.LogError("Arena scene '" + arena.sceneName + "' has no ArenaSite. Run Tools > Mobile RPG > Setup Everything.");
                yield break;
            }

            Current = site;
            returnPoint = player.position;
            TeleportTo(site.playerSpawn.position);
            ledger.Begin();
            director.BeginRun(site);

            if (Entered != null) Entered(site);
            StartCoroutine(StartFirstWave(site));
        }

        private static ArenaSite FindSite(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded) return null;

            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                ArenaSite site = roots[i].GetComponentInChildren<ArenaSite>();
                if (site != null) return site;
            }
            return null;
        }

        /// <summary>Unloads a scene left over from a previous run, unless it is the one about to be used again.</summary>
        private void ResolvePendingUnload(string nextScene)
        {
            if (pendingUnload == null) return;

            if (pendingUnload == nextScene) pendingUnload = null;
            else UnloadPending();
        }

        private void UnloadPending()
        {
            if (pendingUnload == null) return;
            if (SceneManager.GetSceneByName(pendingUnload).isLoaded) SceneManager.UnloadSceneAsync(pendingUnload);
            pendingUnload = null;
        }

        /// <summary>Starts the next wave. Used by the checkpoint panel.</summary>
        public void NextWave()
        {
            if (Current != null) director.StartNextWave();
        }

        /// <summary>Leaves the arena and keeps everything. Only possible at a checkpoint.</summary>
        public void Leave()
        {
            if (!CanLeave) return;

            string scene = Current.arena.sceneName;
            ledger.End();
            Finish();
            TeleportTo(returnPoint);
            health.Heal(health.maxHealth);
            pendingUnload = scene;
            UnloadPending();
        }

        private IEnumerator StartFirstWave(ArenaSite site)
        {
            yield return new WaitForSeconds(firstWaveDelay);
            if (Current == site) director.StartNextWave();
        }

        private void OnPlayerDied()
        {
            if (Current == null) return;

            string scene = Current.arena.sceneName;
            int wave = director.Wave;
            int lost = ledger.ApplyPenalty(loseFractionOnDeath);
            Finish(); // PlayerRespawn brings the player back to the town once the game over panel is confirmed
            pendingUnload = scene;
            if (Died != null) Died(wave, lost);
        }

        private void OnWaveCleared(int wave)
        {
            if (Current == null) return;

            ArenaRecords.Submit(Current.arena.id, wave);

            bool checkpoint = IsCheckpoint(wave);
            if (WaveBreak != null) WaveBreak(wave, checkpoint);
            if (!checkpoint) StartCoroutine(NextWaveAfterBreak(Current));
        }

        private IEnumerator NextWaveAfterBreak(ArenaSite site)
        {
            yield return new WaitForSeconds(breakTime);
            if (Current == site) director.StartNextWave();
        }

        private bool IsCheckpoint(int wave)
        {
            int every = Current.arena.checkpointEvery;
            return every > 0 && wave % every == 0;
        }

        private void Finish()
        {
            StopAllCoroutines();
            Current = null;
            director.EndRun();
            if (Ended != null) Ended();
        }

        private void TeleportTo(Vector2 position)
        {
            player.position = position;
            if (body != null) body.position = position;
        }
    }
}
