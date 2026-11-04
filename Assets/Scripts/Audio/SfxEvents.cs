using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays the sounds that belong to game-wide events: player hurt, portals, wave results, kills and boss arrivals.
    /// </summary>
    public class SfxEvents : MonoBehaviour
    {
        public ArenaRun run;
        public WaveDirector director;
        public Health playerHealth;

        private void OnEnable()
        {
            playerHealth.Damaged += OnPlayerDamaged;
            run.Entered += OnEntered;
            run.WaveBreak += OnWaveBreak;
            director.BossSpawned += OnBossSpawned;
            EnemyIdentity.Killed += OnEnemyKilled;
        }

        private void OnDisable()
        {
            playerHealth.Damaged -= OnPlayerDamaged;
            run.Entered -= OnEntered;
            run.WaveBreak -= OnWaveBreak;
            director.BossSpawned -= OnBossSpawned;
            EnemyIdentity.Killed -= OnEnemyKilled;
        }

        private void OnPlayerDamaged(float amount, Vector2 source) { Sfx.Play("player_hurt"); }

        private void OnEntered(ArenaSite site) { Sfx.Play("portal_open"); }

        private void OnWaveBreak(int wave, bool checkpoint)
        {
            Sfx.Play(checkpoint ? "checkpoint" : "victory");
        }

        private void OnBossSpawned(Health boss) { Sfx.Play("boss_spawn"); }

        private void OnEnemyKilled(EnemyIdentity enemy)
        {
            bool boss = enemy.GetComponent<BossAttack>() != null;
            Sfx.Play(boss ? "boss_growl" : "death_groan");
            if (!boss) Sfx.Play("sword_kill");
        }
    }
}
