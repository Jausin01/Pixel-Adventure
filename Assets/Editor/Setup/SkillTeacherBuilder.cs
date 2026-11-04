using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the skill teacher in the town and the screen where skills are bought and placed in the skill slots.
    /// Needs the skill system, the inventory and the items to exist already.
    /// </summary>
    public static class SkillTeacherBuilder
    {
        private static readonly Vector2 TeacherPosition = TownLayout.SkillTeacher;
        private static readonly Vector2 RowSize = new Vector2(1132f, 100f);

        /// <summary>Builds the teacher and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player, SkillDefinition[] skills)
        {
            SkillTeacher teacher = player.AddComponent<SkillTeacher>();
            teacher.currency = ItemAssetFactory.Load("gold_ingot");

            InventoryPanel screen = BuildScreen(canvas, player, teacher, skills);
            BuildNpc(material, screen);
            return screen;
        }

        private static void BuildNpc(Material material, InventoryPanel screen)
        {
            GameObject npc = new GameObject("Skill Teacher");
            npc.transform.position = TeacherPosition;

            SpriteRenderer renderer = npc.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/skill_teacher");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            CircleCollider2D trigger = npc.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;

            Interactable interactable = npc.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = SpriteLoader.Load("Swords/sword_silver_diag");
        }

        private static InventoryPanel BuildScreen(Transform canvas, GameObject player, SkillTeacher teacher, SkillDefinition[] skills)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Skill Teacher Screen", "Skill Teacher", out root, out panel);

            string[] classNames = System.Enum.GetNames(typeof(WeaponClass));
            SkillClassTabs tabs = frame.gameObject.AddComponent<SkillClassTabs>();
            tabs.groups = new GameObject[classNames.Length];
            tabs.tabBackgrounds = new Image[classNames.Length];

            float tabWidth = (1132f - (classNames.Length - 1) * 8f) / classNames.Length;
            for (int c = 0; c < classNames.Length; c++)
            {
                GameObject group = new GameObject(classNames[c] + " Skills", typeof(RectTransform));
                RectTransform groupRect = group.GetComponent<RectTransform>();
                groupRect.SetParent(frame, false);
                groupRect.anchorMin = Vector2.zero;
                groupRect.anchorMax = Vector2.one;
                groupRect.offsetMin = Vector2.zero;
                groupRect.offsetMax = Vector2.zero;
                tabs.groups[c] = group;

                int row = 0;
                for (int i = 0; i < skills.Length; i++)
                {
                    if ((int)skills[i].weaponClass == c) BuildRow(groupRect, player, teacher, skills[i], row++);
                }

                Image tab = UiFactory.CreateImage(classNames[c] + " Tab", frame, null, new Vector2(0f, 1f),
                    new Vector2(24f + c * (tabWidth + 8f), -84f), new Vector2(tabWidth, 56f));
                UiFactory.CreateText("Label", tab.transform, classNames[c], 26, TextAnchor.MiddleCenter,
                    new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(tabWidth, 56f));
                SkillClassTabButton button = tab.gameObject.AddComponent<SkillClassTabButton>();
                button.tabs = tabs;
                button.index = c;
                tabs.tabBackgrounds[c] = tab;
            }
            return panel;
        }

        private static void BuildRow(Transform frame, GameObject player, SkillTeacher teacher, SkillDefinition skill, int index)
        {
            Vector2 topLeft = new Vector2(0f, 1f);
            Image background = UiFactory.CreateImage(skill.displayName + " Row", frame, null, topLeft,
                new Vector2(24f, -156f - index * (RowSize.y + 8f)), RowSize);

            Image icon = UiFactory.CreateImage("Icon", background.transform, skill.icon, new Vector2(0f, 0.5f), new Vector2(16f, 0f), new Vector2(72f, 72f));
            icon.preserveAspect = true;
            icon.raycastTarget = false;

            UiFactory.CreateText("Name", background.transform, skill.displayName, 36, TextAnchor.MiddleLeft, topLeft, new Vector2(108f, -4f), new Vector2(560f, 48f));
            Text description = UiFactory.CreateText("Description", background.transform, skill.description, 24, TextAnchor.UpperLeft, topLeft, new Vector2(108f, -54f), new Vector2(760f, 40f));
            description.color = new Color(1f, 1f, 1f, 0.7f);

            Text status = UiFactory.CreateText("Status", background.transform, string.Empty, 30, TextAnchor.MiddleRight, new Vector2(1f, 0.5f), new Vector2(-20f, 0f), new Vector2(340f, 60f));

            SkillTeacherRowView view = background.gameObject.AddComponent<SkillTeacherRowView>();
            view.skill = skill;
            view.teacher = teacher;
            view.inventory = player.GetComponent<Inventory>();
            view.unlocks = player.GetComponent<SkillUnlocks>();
            view.loadout = player.GetComponent<SkillLoadout>();
            view.statusText = status;
            view.background = background;

            SkillTeacherRowButton button = background.gameObject.AddComponent<SkillTeacherRowButton>();
            button.skill = skill;
            button.teacher = teacher;
            button.unlocks = view.unlocks;
            button.loadout = view.loadout;
            background.gameObject.AddComponent<PressScaleFeedback>();
        }
    }
}
