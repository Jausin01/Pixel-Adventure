using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Says what kind of enemy this is and announces its death, so quests can count kills.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyIdentity : MonoBehaviour
    {
        [Tooltip("Unique kind of enemy, for example brown_rat.")]
        public string id;

        [Tooltip("Group the enemy belongs to, for example rat. A quest asking for a group counts every member.")]
        public string family;

        /// <summary>Id of the arena that spawned this enemy. Set by the wave director.</summary>
        [HideInInspector] public string arenaId;

        /// <summary>Raised when any enemy dies.</summary>
        public static event Action<EnemyIdentity> Killed;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable() { health.Died += OnDied; }

        private void OnDisable() { health.Died -= OnDied; }

        private void OnDied()
        {
            if (Killed != null) Killed(this);
        }
    }
}
