using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps the damage of every main weapon in line with the <see cref="CharacterStats"/>.
    /// </summary>
    public class WeaponDamageLink : MonoBehaviour
    {
        public CharacterStats stats;
        public WeaponHolder holder;

        private void OnEnable() { stats.Changed += Apply; }

        private void OnDisable() { stats.Changed -= Apply; }

        private void Start() { Apply(); }

        private void Apply()
        {
            for (int i = 0; i < holder.weapons.Length; i++)
            {
                IWeapon weapon = holder.weapons[i].GetComponent<IWeapon>();
                if (weapon != null) weapon.Damage = stats.AttackFor(weapon.Class);
            }
        }
    }
}
