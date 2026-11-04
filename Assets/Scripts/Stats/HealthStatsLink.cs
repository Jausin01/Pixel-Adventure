using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the character's <see cref="Health"/> (maximum and defense) in line with its <see cref="CharacterStats"/>.
    /// </summary>
    public class HealthStatsLink : MonoBehaviour
    {
        public CharacterStats stats;
        public Health health;

        private void OnEnable() { stats.Changed += Apply; }

        private void OnDisable() { stats.Changed -= Apply; }

        private void Start() { Apply(); }

        private void Apply()
        {
            health.SetMaxHealth(stats.MaxHealth);
            health.Defense = stats.Defense;
        }
    }
}
