using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the character's final numbers (health, attack, defense).
    /// </summary>
    public class EquipmentStatsPanel : MonoBehaviour
    {
        public CharacterStats stats;
        public Text text;

        private void OnEnable()
        {
            stats.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            stats.Changed -= Refresh;
        }

        private void Refresh()
        {
            text.text = "Max HP: " + (int)stats.MaxHealth
                + "\nSword ATK: " + (int)stats.SwordAttack
                + "\nAxe ATK: " + (int)stats.AttackFor(WeaponClass.Axe)
                + "\nSpear ATK: " + (int)stats.AttackFor(WeaponClass.Spear)
                + "\nBow ATK: " + (int)stats.AttackFor(WeaponClass.Bow)
                + "\nDagger ATK: " + (int)stats.AttackFor(WeaponClass.Dagger)
                + "\nHammer ATK: " + (int)stats.AttackFor(WeaponClass.Hammer)
                + "\nStaff ATK: " + (int)stats.AttackFor(WeaponClass.Staff)
                + "\nThrown ATK: " + (int)stats.SecondaryAttack
                + "\nDefense: " + (int)stats.Defense;
        }
    }
}
