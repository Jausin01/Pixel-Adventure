using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps a list of weapon objects, activates only the selected one and forwards attacks to it.
    /// Each weapon object must contain a component implementing <see cref="IWeapon"/>.
    /// The selected weapon follows the equipped item: see <see cref="EquippedWeaponLink"/>.
    /// </summary>
    public class WeaponHolder : MonoBehaviour
    {
        [Tooltip("Weapon root objects. Only the selected one is active.")]
        public GameObject[] weapons;

        [Tooltip("Index of the weapon selected at start, when no equipped item has chosen one.")]
        public int startIndex = 0;

        private IWeapon[] components;
        private int activeIndex;
        private bool selected;

        private void Awake()
        {
            components = new IWeapon[weapons.Length];
            for (int i = 0; i < weapons.Length; i++) components[i] = weapons[i].GetComponent<IWeapon>();
        }

        private void Start()
        {
            if (selected) return;

            activeIndex = Mathf.Clamp(startIndex, 0, weapons.Length - 1);
            ApplyActiveWeapon();
        }

        /// <summary>The weapon object that is currently selected.</summary>
        public GameObject Active { get { return weapons[activeIndex]; } }

        /// <summary>The weapon class of the selected weapon.</summary>
        public WeaponClass ActiveClass { get { return components[activeIndex].Class; } }

        /// <summary>The weapon of a class, or null when there is none.</summary>
        public GameObject Find(WeaponClass weaponClass)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (components[i] != null && components[i].Class == weaponClass) return weapons[i];
            }
            return null;
        }

        private static string SwingSound(WeaponClass weaponClass)
        {
            switch (weaponClass)
            {
                case WeaponClass.Sword: return "sword_swing";
                case WeaponClass.Axe: return "axe_swing";
                case WeaponClass.Spear: return "spear_swing";
                case WeaponClass.Bow: return "bow_shoot";
                case WeaponClass.Dagger: return "dagger_slash";
                case WeaponClass.Hammer: return "hammer_swing";
                default: return "staff_cast";
            }
        }

        /// <summary>Attacks with the selected weapon.</summary>
        public void Attack()
        {
            IWeapon weapon = components[activeIndex];
            if (weapon == null) return;

            bool wasBusy = weapon.IsBusy;
            weapon.RequestAttack();
            if (!wasBusy && weapon.IsBusy) Sfx.Play(SwingSound(weapon.Class));
        }

        /// <summary>
        /// Selects the weapon of a class. Does nothing when there is no such weapon or it is already selected.
        /// The new weapon appears where the previous one was.
        /// </summary>
        public void Select(WeaponClass weaponClass)
        {
            selected = true;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (components[i] == null || components[i].Class != weaponClass) continue;
                if (i == activeIndex) return;

                Vector3 position = weapons[activeIndex].transform.position;
                activeIndex = i;
                weapons[activeIndex].transform.position = position;
                ApplyActiveWeapon();
                return;
            }
        }

        private void ApplyActiveWeapon()
        {
            for (int i = 0; i < weapons.Length; i++) weapons[i].SetActive(i == activeIndex);
        }
    }
}
