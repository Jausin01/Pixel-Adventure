using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps a list of weapon objects, activates only the selected one and forwards attacks to it.
    /// Each weapon object must contain a component implementing <see cref="IWeapon"/>.
    /// </summary>
    public class WeaponHolder : MonoBehaviour
    {
        [Tooltip("Weapon root objects. Only the selected one is active.")]
        public GameObject[] weapons;

        [Tooltip("Index of the weapon selected at start.")]
        public int startIndex = 0;

        private IWeapon[] components;
        private int activeIndex;

        private void Awake()
        {
            components = new IWeapon[weapons.Length];
            for (int i = 0; i < weapons.Length; i++) components[i] = weapons[i].GetComponent<IWeapon>();
        }

        private void Start()
        {
            activeIndex = Mathf.Clamp(startIndex, 0, weapons.Length - 1);
            ApplyActiveWeapon();
        }

        /// <summary>Attacks with the selected weapon.</summary>
        public void Attack()
        {
            if (components[activeIndex] != null) components[activeIndex].RequestAttack();
        }

        /// <summary>
        /// Selects the next weapon in the list. Ignored while the current weapon is attacking.
        /// The new weapon appears where the previous one was.
        /// </summary>
        public void SwapNext()
        {
            if (weapons.Length < 2) return;
            if (components[activeIndex] != null && components[activeIndex].IsBusy) return;

            Vector3 position = weapons[activeIndex].transform.position;
            activeIndex = (activeIndex + 1) % weapons.Length;
            weapons[activeIndex].transform.position = position;
            ApplyActiveWeapon();
        }

        private void ApplyActiveWeapon()
        {
            for (int i = 0; i < weapons.Length; i++) weapons[i].SetActive(i == activeIndex);
        }
    }
}
