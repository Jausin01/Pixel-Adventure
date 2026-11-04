using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the doctor in the town and the screen where the player pays to be healed.
    /// Needs the items, health and energy to exist already.
    /// </summary>
    public static class DoctorBuilder
    {
        private static readonly Vector2 ButtonSize = new Vector2(700f, 130f);

        /// <summary>Builds the doctor and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player)
        {
            Doctor doctor = player.AddComponent<Doctor>();
            doctor.currency = ItemAssetFactory.Load("gold_ingot");

            InventoryPanel screen = BuildScreen(canvas, player, doctor);
            TownNpcFactory.Create("Doctor", "World/doctor", material, TownLayout.Doctor, screen, false);
            return screen;
        }

        private static InventoryPanel BuildScreen(Transform canvas, GameObject player, Doctor doctor)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Doctor Screen", "Doctor", out root, out panel);

            Text intro = UiFactory.CreateText("Intro", frame, "Rest here and I will patch you up.\nAll health and energy restored.", 32,
                TextAnchor.MiddleCenter, new Vector2(0.5f, 1f), new Vector2(0f, -120f), new Vector2(1000f, 120f));
            intro.color = new Color(1f, 1f, 1f, 0.8f);

            Image button = UiFactory.CreateImage("Heal Button", frame, null, new Vector2(0.5f, 0.5f), new Vector2(0f, -40f), ButtonSize);
            Text status = UiFactory.CreateText("Status", button.transform, string.Empty, 40, TextAnchor.MiddleCenter,
                new Vector2(0.5f, 0.5f), Vector2.zero, ButtonSize);
            status.supportRichText = true;

            DoctorView view = button.gameObject.AddComponent<DoctorView>();
            view.doctor = doctor;
            view.inventory = player.GetComponent<Inventory>();
            view.health = player.GetComponent<Health>();
            view.energy = player.GetComponent<Energy>();
            view.statusText = status;
            view.button = button;

            button.gameObject.AddComponent<DoctorHealButton>().doctor = doctor;
            button.gameObject.AddComponent<PressScaleFeedback>();
            return panel;
        }
    }
}
