using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Chooses which music track should play from the game state:
    /// game over, boss fight, arena, menu, or town (day or night).
    /// </summary>
    public class MusicDirector : MonoBehaviour
    {
        public MusicPlayer music;
        public ArenaRun run;
        public WaveDirector director;
        public DayNightCycle dayNight;
        public Health playerHealth;

        [Tooltip("Track used when an arena has no music of its own.")]
        public string fallbackArenaTrack = "old_dungeon";

        private int bossesAlive;
        private int bossTurn;
        private string bossTrack = "boss_1";
        private bool playerDead;

        private void OnEnable()
        {
            director.BossSpawned += OnBossSpawned;
            run.Entered += OnRunBoundary;
            run.Ended += OnRunEnded;
            playerHealth.Died += OnPlayerDied;
            playerHealth.Revived += OnPlayerRevived;
        }

        private void OnDisable()
        {
            director.BossSpawned -= OnBossSpawned;
            run.Entered -= OnRunBoundary;
            run.Ended -= OnRunEnded;
            playerHealth.Died -= OnPlayerDied;
            playerHealth.Revived -= OnPlayerRevived;
        }

        private void OnBossSpawned(Health boss)
        {
            bossesAlive++;
            bossTrack = bossTurn++ % 2 == 0 ? "boss_1" : "boss_2";
            boss.Died += OnBossDied;
        }

        private void OnBossDied()
        {
            if (bossesAlive > 0) bossesAlive--;
        }

        private void OnRunBoundary(ArenaSite site) { bossesAlive = 0; }

        private void OnRunEnded() { bossesAlive = 0; }

        private void OnPlayerDied() { playerDead = true; }

        private void OnPlayerRevived()
        {
            playerDead = false;
            bossesAlive = 0;
        }

        private void Update()
        {
            music.Play(Choose());
        }

        private string Choose()
        {
            if (PauseWhileOpen.IsOpen) return "title";
            if (playerDead) return "game_over";
            if (bossesAlive > 0) return bossTrack;

            if (run.Current != null)
            {
                string id = run.Current.arena.id;
                return music.bank != null && music.bank.Has(id) ? id : fallbackArenaTrack;
            }

            if (InventoryPanel.OpenCount > 0) return "menu";
            return dayNight.IsNight ? "town_night" : "town_day";
        }
    }
}
