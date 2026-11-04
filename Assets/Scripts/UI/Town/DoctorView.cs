using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the doctor's offer on the doctor's screen: the price, or why the treatment is not available.
    /// Refreshes whenever the inventory, health or energy change.
    /// </summary>
    public class DoctorView : MonoBehaviour
    {
        public Doctor doctor;
        public Inventory inventory;
        public Health health;
        public Energy energy;
        public Text statusText;
        public Graphic button;

        private static readonly Color Ready = new Color(0.2f, 0.32f, 0.16f, 1f);
        private static readonly Color Unavailable = new Color(0.22f, 0.16f, 0.13f, 1f);

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            health.FractionChanged += Refresh;
            energy.FractionChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
            health.FractionChanged -= Refresh;
            energy.FractionChanged -= Refresh;
        }

        private void OnInventoryChanged(ItemDefinition item, int count) { Refresh(); }

        private void Refresh()
        {
            button.color = doctor.CanTreat ? Ready : Unavailable;

            if (!doctor.NeedsTreatment) statusText.text = "You are in perfect shape.";
            else if (doctor.CanTreat) statusText.text = "Heal: " + doctor.cost + " " + doctor.currency.displayName;
            else statusText.text = "<color=#ff8a7a>Need " + doctor.cost + " " + doctor.currency.displayName + "</color>";
        }
    }
}
