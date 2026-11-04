using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the right-side controls: a large attack button in the bottom-right corner, four skill slots
    /// in an arc around it.
    /// </summary>
    public static class ActionButtonsBuilder
    {
        private const float CornerMargin = 30f;
        private const float AttackSize = 170f;
        private const float SkillSize = 88f;
        private const float SkillGap = 16f;

        // Angles (degrees, 0 = right, 90 = up) of the skill slots around the attack button.
        private static readonly float[] SkillAngles = { 90f, 125f, 160f, 195f };

        /// <summary>Builds all buttons.</summary>
        public static void Build(Transform parent, ActionRouter router, NearbyHarvestDetector detector, GameObject player)
        {
            Sprite circle = SpriteLoader.Load("UI/button_circle");

            Vector2 attackCenter = BuildAttackButton(parent, circle, router, detector, player.GetComponent<NearbyInteractableDetector>());
            BuildSkillSlots(parent, circle, attackCenter, player);
        }

        private static Vector2 BuildAttackButton(Transform parent, Sprite circle, ActionRouter router, NearbyHarvestDetector detector, NearbyInteractableDetector interactions)
        {
            Vector2 bottomRight = new Vector2(1f, 0f);
            Image attack = UiFactory.CreateImage("Attack Button", parent, circle, bottomRight,
                new Vector2(-CornerMargin, CornerMargin), new Vector2(AttackSize, AttackSize));
            attack.color = new Color(1f, 0.55f, 0.45f, 0.85f);
            attack.gameObject.AddComponent<AttackButton>().router = router;
            attack.gameObject.AddComponent<PressScaleFeedback>();
            AddToolIcon(attack, detector, interactions);

            // Center of the button measured from the bottom-right corner of the parent.
            float half = AttackSize / 2f;
            return new Vector2(-CornerMargin - half, CornerMargin + half);
        }

        private static void BuildSkillSlots(Transform parent, Sprite circle, Vector2 attackCenter, GameObject player)
        {
            SkillCaster caster = player.GetComponent<SkillCaster>();
            float radius = AttackSize / 2f + SkillSize / 2f + SkillGap;

            for (int i = 0; i < SkillAngles.Length; i++)
            {
                float angle = SkillAngles[i] * Mathf.Deg2Rad;
                Vector2 center = attackCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                // The slot's pivot is its bottom-right corner, so offset from the center.
                Vector2 corner = center + new Vector2(SkillSize / 2f, -SkillSize / 2f);
                Image slot = UiFactory.CreateImage("Skill Slot " + (i + 1), parent, circle, new Vector2(1f, 0f), corner, new Vector2(SkillSize, SkillSize));
                slot.color = new Color(1f, 1f, 1f, 0.45f);
                BuildSkillControls(slot, circle, caster, player, i);
            }
        }

        private static void BuildSkillControls(Image slot, Sprite circle, SkillCaster caster, GameObject player, int index)
        {
            Image icon = UiFactory.CreateImage("Skill Icon", slot.transform, null, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(SkillSize - 24f, SkillSize - 24f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
            icon.enabled = false;

            Image overlay = UiFactory.CreateImage("Cooldown Overlay", slot.transform, circle, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(SkillSize, SkillSize));
            overlay.color = new Color(0f, 0f, 0f, 0.65f);
            overlay.raycastTarget = false;
            overlay.type = Image.Type.Filled;
            overlay.fillMethod = Image.FillMethod.Radial360;
            overlay.fillOrigin = (int)Image.Origin360.Top;
            overlay.fillClockwise = false;
            overlay.fillAmount = 0f;

            SkillButton button = slot.gameObject.AddComponent<SkillButton>();
            button.caster = caster;
            button.index = index;
            slot.gameObject.AddComponent<PressScaleFeedback>();

            SkillButtonView view = slot.gameObject.AddComponent<SkillButtonView>();
            view.caster = caster;
            view.loadout = player.GetComponent<SkillLoadout>();
            view.cooldowns = player.GetComponent<SkillCooldowns>();
            view.index = index;
            view.icon = icon;
            view.cooldownOverlay = overlay;
        }

        private static void AddToolIcon(Image button, NearbyHarvestDetector detector, NearbyInteractableDetector interactions)
        {
            Image icon = UiFactory.CreateImage("Tool Icon", button.transform, null, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(100f, 100f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;
            icon.enabled = false;

            ActionButtonIcon iconControl = button.gameObject.AddComponent<ActionButtonIcon>();
            iconControl.detector = detector;
            iconControl.interactions = interactions;
            iconControl.icon = icon;
            iconControl.axeIcon = SpriteLoader.Load("Tools/axe_0");
            iconControl.pickaxeIcon = SpriteLoader.Load("Tools/pickaxe_0");
            iconControl.shovelIcon = SpriteLoader.Load("Tools/shovel_0");
        }
    }
}
