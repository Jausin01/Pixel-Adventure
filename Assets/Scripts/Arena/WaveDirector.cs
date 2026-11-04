using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobileRpg
{
    /// <summary>
    /// Runs the waves of an arena. Each wave gets a budget that grows with the wave number; the director spends it on
    /// enemies chosen from the arena's list, makes later enemies tougher, and reports when the wave is cleared.
    /// It does not decide what happens between waves; other components listen to its events.
    /// </summary>
    public class WaveDirector : MonoBehaviour
    {
        [Tooltip("The enemies' target.")]
        public Transform player;

        [Header("Placement")]
        [Tooltip("Enemies never appear closer to the player than this.")]
        public float minSpawnDistance = 6f;

        /// <summary>Raised when a wave begins. Argument: the wave number.</summary>
        public event Action<int> WaveStarted;

        /// <summary>Raised when every enemy of a wave is dead. Argument: the wave number.</summary>
        public event Action<int> WaveCleared;

        /// <summary>Raised when a boss appears. Argument: the boss.</summary>
        public event Action<Health> BossSpawned;

        /// <summary>The current (or last) wave number. 0 before the first wave.</summary>
        public int Wave { get; private set; }

        /// <summary>True while a wave is being fought.</summary>
        public bool WaveInProgress { get { return waveRoutine != null; } }

        private readonly List<Health> alive = new List<Health>();
        private ArenaSite site;
        private Coroutine waveRoutine;

        /// <summary>Prepares a new run in an arena. Any previous run is ended.</summary>
        public void BeginRun(ArenaSite newSite)
        {
            EndRun();
            site = newSite;
            Wave = 0;
        }

        /// <summary>Starts the next wave. Ignored while a wave is running or when no run is active.</summary>
        public void StartNextWave()
        {
            if (site == null || waveRoutine != null) return;

            Wave++;
            waveRoutine = StartCoroutine(RunWave());
        }

        /// <summary>Stops the run and removes every enemy.</summary>
        public void EndRun()
        {
            if (waveRoutine != null) StopCoroutine(waveRoutine);
            waveRoutine = null;

            for (int i = 0; i < alive.Count; i++)
            {
                if (alive[i] != null) Destroy(alive[i].gameObject);
            }
            alive.Clear();
            site = null;
        }

        private IEnumerator RunWave()
        {
            ArenaDefinition arena = site.arena;
            if (WaveStarted != null) WaveStarted(Wave);

            float budget = arena.baseBudget + arena.budgetPerWave * (Wave - 1);
            if (arena.bossEvery > 0 && Wave % arena.bossEvery == 0 && arena.bosses != null && arena.bosses.Length > 0)
            {
                int turn = Wave / arena.bossEvery - 1;
                Health boss = Spawn(arena.bosses[turn % arena.bosses.Length]);
                if (BossSpawned != null) BossSpawned(boss);
            }

            while (true)
            {
                while (budget > 0f && alive.Count < arena.maxAlive)
                {
                    ArenaDefinition.EnemyEntry entry = Pick(arena, budget);
                    if (entry == null)
                    {
                        budget = 0f;
                        break;
                    }

                    budget -= entry.cost;
                    Spawn(entry.prefab);
                    yield return new WaitForSeconds(arena.spawnInterval);
                }

                if (budget <= 0f && alive.Count == 0) break;
                yield return null;
            }

            waveRoutine = null;
            RewardPlayer(arena);
            if (WaveCleared != null) WaveCleared(Wave);
        }

        // Picks an enemy that is unlocked and affordable, favoring the ones with more weight.
        private ArenaDefinition.EnemyEntry Pick(ArenaDefinition arena, float budget)
        {
            float totalWeight = 0f;
            for (int i = 0; i < arena.enemies.Length; i++)
            {
                if (IsCandidate(arena.enemies[i], budget)) totalWeight += arena.enemies[i].weight;
            }
            if (totalWeight <= 0f) return null;

            float roll = Random.value * totalWeight;
            for (int i = 0; i < arena.enemies.Length; i++)
            {
                if (!IsCandidate(arena.enemies[i], budget)) continue;

                roll -= arena.enemies[i].weight;
                if (roll <= 0f) return arena.enemies[i];
            }
            return null;
        }

        private bool IsCandidate(ArenaDefinition.EnemyEntry entry, float budget)
        {
            return entry.prefab != null && entry.firstWave <= Wave && entry.cost <= budget && entry.weight > 0f;
        }

        private Health Spawn(GameObject prefab)
        {
            ArenaDefinition arena = site.arena;
            GameObject enemy = Instantiate(prefab, FindSpawnPoint(), Quaternion.identity);

            EnemyIdentity identity = enemy.GetComponent<EnemyIdentity>();
            if (identity != null) identity.arenaId = arena.id;

            Health health = enemy.GetComponent<Health>();
            health.SetMaxHealth(health.maxHealth * (1f + arena.healthGrowth * (Wave - 1)));
            health.Died += () => alive.Remove(health);
            alive.Add(health);

            EnemyAttack attack = enemy.GetComponent<EnemyAttack>();
            if (attack != null) attack.damage *= 1f + arena.damageGrowth * (Wave - 1);

            EnemyChaseBrain brain = enemy.GetComponent<EnemyChaseBrain>();
            if (brain != null)
            {
                brain.target = player;
                brain.sightRange = float.MaxValue; // arena enemies always hunt the player
            }

            FaceTargetFlipper flipper = enemy.GetComponent<FaceTargetFlipper>();
            if (flipper != null) flipper.target = player;
            return health;
        }

        // A random point inside the arena that is not too close to the player and not inside a solid prop.
        private Vector2 FindSpawnPoint()
        {
            Vector2 center = site.transform.position;
            Vector2 point = center;

            for (int attempt = 0; attempt < 8; attempt++)
            {
                point = center + new Vector2(Random.Range(-site.halfSize.x, site.halfSize.x), Random.Range(-site.halfSize.y, site.halfSize.y));
                bool farEnough = Vector2.Distance(point, player.position) >= minSpawnDistance;
                if (farEnough && Physics2D.OverlapCircle(point, 0.4f) == null) break;
            }
            return point;
        }

        private void RewardPlayer(ArenaDefinition arena)
        {
            if (arena.clearRewards == null || arena.clearRewards.Length == 0) return;
            DropRoller.Roll(arena.clearRewards, player.position, RewardMaterial());
        }

        private Material RewardMaterial()
        {
            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            return sprite != null ? sprite.sharedMaterial : null;
        }
    }
}
