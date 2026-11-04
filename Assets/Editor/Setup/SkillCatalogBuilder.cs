using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Defines every skill of the game as assets under Assets/Data/Skills. Run before anything that needs skills.
    /// To add a skill, add a line to <see cref="BuildSkills"/>.
    /// </summary>
    public static class SkillCatalogBuilder
    {
        private const string Folder = "Assets/Data/Skills";

        /// <summary>Creates or updates all skill assets and returns them in teaching order.</summary>
        public static SkillDefinition[] Build()
        {
            AssetFolderUtil.Ensure(Folder);
            BuildSkills();
            AssetDatabase.SaveAssets();
            string[] ids =
            {
                "spin_slash", "piercing_thrust", "sword_throw", "ground_slam",
                "hatchet_volley", "skull_splitter",
                "javelin", "triple_thrust", "spear_rain",
                "multishot", "piercing_shot",
                "fan_of_knives", "shadow_flurry",
                "titan_smash", "seismic_slam",
                "fireball", "chain_lightning", "meteor_shower"
            };
            SkillDefinition[] skills = new SkillDefinition[ids.Length];
            for (int i = 0; i < ids.Length; i++) skills[i] = Load(ids[i]);
            return skills;
        }

        /// <summary>Loads a skill asset by id, or null.</summary>
        public static SkillDefinition Load(string id)
        {
            return AssetDatabase.LoadAssetAtPath<SkillDefinition>(Folder + "/" + id + ".asset");
        }

        private static void BuildSkills()
        {
            Add("spin_slash", "Spin Slash", "Swords/sword_silver_diag", SkillType.SpinSlash,
                "The sword circles you twice, cutting everything around.", 30f, 5f, 1.5f, 0);
            Add("piercing_thrust", "Piercing Thrust", "Swords/sword_blue_up", SkillType.PiercingThrust,
                "A long stab towards the nearest enemy that goes through everything in its path.", 20f, 3f, 2f, 2);
            Add("sword_throw", "Sword Throw", "Swords/sword_red_diag", SkillType.SwordThrow,
                "Hurl the sword at an enemy. It hits on the way out and again on the way back.", 25f, 6f, 1.8f, 3);
            Add("ground_slam", "Ground Slam", "Swords/sword_black_up", SkillType.GroundSlam,
                "Crash the sword down in front of you and damage everything in a wide circle.", 40f, 9f, 3f, 5);

            // Axe: throwing and crushing
            Add("hatchet_volley", "Hatchet Volley", "Weapons/axe_iron", SkillType.HatchetVolley, WeaponClass.Axe,
                "Hurl three spinning hatchets in a fan.", 25f, 6f, 1.2f, 0);
            Add("skull_splitter", "Skull Splitter", "Weapons/axe_ruby", SkillType.SkullSplitter, WeaponClass.Axe,
                "Leap the axe high and cleave the ground. A shockwave follows the blow.", 40f, 9f, 3f, 3);

            // Spear: reach and speed
            Add("javelin", "Javelin", "Items/spear_iron", SkillType.Javelin, WeaponClass.Spear,
                "Throw the spear. It flies through every enemy in its path.", 25f, 5f, 2.2f, 0);
            Add("triple_thrust", "Triple Thrust", "Items/spear_steel", SkillType.TripleThrust, WeaponClass.Spear,
                "Three stabs in a row, each reaching further than the last.", 30f, 6f, 1.4f, 2);
            Add("spear_rain", "Spear Rain", "Items/spear_ruby", SkillType.SpearRain, WeaponClass.Spear,
                "Spears fall from the sky over the nearest enemy.", 45f, 11f, 1.1f, 4);

            // Bow: volleys and precision
            Add("multishot", "Multishot", "Bow/arrow", SkillType.Multishot, WeaponClass.Bow,
                "Two quick volleys of five arrows each.", 30f, 7f, 1f, 0);
            Add("piercing_shot", "Piercing Shot", "Bow/arrow", SkillType.PiercingShot, WeaponClass.Bow,
                "One heavy arrow that pierces everything in line.", 25f, 5f, 3f, 3);

            // Daggers: knives and speed
            Add("fan_of_knives", "Fan of Knives", "Effects/knife", SkillType.FanOfKnives, WeaponClass.Dagger,
                "Throw a wide spread of knives all around the front.", 25f, 5f, 1.3f, 0);
            Add("shadow_flurry", "Shadow Flurry", "Items/daggers_ruby", SkillType.ShadowFlurry, WeaponClass.Dagger,
                "Dart around the nearest enemy and stab it from every side.", 35f, 8f, 0.9f, 3);

            // Hammer: ground control
            Add("titan_smash", "Titan Smash", "Items/hammer_iron", SkillType.TitanSmash, WeaponClass.Hammer,
                "Smash the ground in front of you. A second shockwave rolls outward.", 40f, 9f, 2.5f, 0);
            Add("seismic_slam", "Seismic Slam", "Items/hammer_ruby", SkillType.SeismicSlam, WeaponClass.Hammer,
                "Pound the earth beneath you and send three rings of force outward.", 45f, 10f, 1.5f, 3);

            // Staff: magic
            Add("fireball", "Fireball", "Effects/fireball", SkillType.Fireball, WeaponClass.Staff,
                "A ball of fire that explodes where it lands.", 25f, 5f, 3f, 0);
            Add("chain_lightning", "Chain Lightning", "Items/staff_oak", SkillType.ChainLightning, WeaponClass.Staff,
                "Lightning leaps from enemy to enemy, weaker with every jump.", 30f, 7f, 2f, 2);
            Add("meteor_shower", "Meteor Shower", "Effects/explosion", SkillType.MeteorShower, WeaponClass.Staff,
                "Call meteors down on the area around the nearest enemy.", 50f, 12f, 1.8f, 5);
        }

        private static void Add(string id, string name, string icon, SkillType type, string description,
            float energyCost, float cooldown, float damageMultiplier, int goldCost)
        {
            Add(id, name, icon, type, WeaponClass.Sword, description, energyCost, cooldown, damageMultiplier, goldCost);
        }

        private static void Add(string id, string name, string icon, SkillType type, WeaponClass weaponClass, string description,
            float energyCost, float cooldown, float damageMultiplier, int goldCost)
        {
            string path = Folder + "/" + id + ".asset";
            SkillDefinition skill = AssetDatabase.LoadAssetAtPath<SkillDefinition>(path);
            if (skill == null)
            {
                skill = ScriptableObject.CreateInstance<SkillDefinition>();
                AssetDatabase.CreateAsset(skill, path);
            }

            skill.id = id;
            skill.displayName = name;
            skill.description = description;
            skill.icon = SpriteLoader.Load(icon);
            skill.weaponClass = weaponClass;
            skill.type = type;
            skill.energyCost = energyCost;
            skill.cooldown = cooldown;
            skill.damageMultiplier = damageMultiplier;
            skill.goldCost = goldCost;
            EditorUtility.SetDirty(skill);
        }
    }
}
