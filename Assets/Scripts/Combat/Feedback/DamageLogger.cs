using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Debug helper: writes a console message every time the attached <see cref="Health"/> takes damage.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DamageLogger : MonoBehaviour
    {
        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= OnDamaged;
        }

        private void OnDamaged(float amount, Vector2 source)
        {
            Debug.Log(name + " took " + amount + " damage (" + health.CurrentHealth + "/" + health.maxHealth + ")");
        }
    }
}
