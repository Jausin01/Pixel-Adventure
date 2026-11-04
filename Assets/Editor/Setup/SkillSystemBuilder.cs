using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player the ability to learn and cast weapon skills: unlocks, slots, cooldowns and the caster,
    /// plus the Spin Slash behavior on the sword and the desktop number-key shortcuts.
    /// Needs health, energy, stats and the weapons to exist already, and the skills made by <see cref="SkillCatalogBuilder"/>.
    /// </summary>
    public static class SkillSystemBuilder
    {
        /// <summary>Adds the skill components and returns the caster.</summary>
        public static SkillCaster Attach(GameObject player, WeaponHolder weapons)
        {
            GameObject swordObject = weapons.weapons[0];
            SwordAttack sword = swordObject.GetComponent<SwordAttack>();

            AddSkill<SpinSlashSkill>(swordObject, player, sword);
            AddSkill<PiercingThrustSkill>(swordObject, player, sword);
            AddSkill<SwordThrowSkill>(swordObject, player, sword);
            AddSkill<GroundSlamSkill>(swordObject, player, sword);

            AddOtherWeaponSkills(player, weapons, sword.hitStop);

            player.AddComponent<SkillUnlocks>();
            player.AddComponent<SkillLoadout>();
            SkillCooldowns cooldowns = player.AddComponent<SkillCooldowns>();

            SkillCaster caster = player.AddComponent<SkillCaster>();
            caster.weapons = weapons;
            caster.loadout = player.GetComponent<SkillLoadout>();
            caster.cooldowns = cooldowns;
            caster.stats = player.GetComponent<CharacterStats>();
            caster.energy = player.GetComponent<Energy>();
            caster.health = player.GetComponent<Health>();

            StartingSkills starter = player.AddComponent<StartingSkills>();
            starter.skills = new[]
            {
                SkillCatalogBuilder.Load("spin_slash"), SkillCatalogBuilder.Load("hatchet_volley"), SkillCatalogBuilder.Load("javelin"),
                SkillCatalogBuilder.Load("multishot"), SkillCatalogBuilder.Load("fan_of_knives"), SkillCatalogBuilder.Load("titan_smash"),
                SkillCatalogBuilder.Load("fireball")
            };

            new GameObject("Skill Keyboard Input").AddComponent<SkillKeyboardInput>().caster = caster;
            return caster;
        }

        // Weapon order in the holder: sword, bow, axe, spear, daggers, hammer, staff.
        private static void AddOtherWeaponSkills(GameObject player, WeaponHolder weapons, HitStop hitStop)
        {
            GameObject bow = weapons.weapons[1], axe = weapons.weapons[2], spear = weapons.weapons[3];
            GameObject daggers = weapons.weapons[4], hammer = weapons.weapons[5], staff = weapons.weapons[6];

            FanSkill hatchets = Add<FanSkill>(axe, player, hitStop, SkillType.HatchetVolley);
            hatchets.projectileSprite = SpriteLoader.Load("Weapons/axe_iron");
            hatchets.count = 3; hatchets.spreadDegrees = 36f; hatchets.speed = 9f; hatchets.lifetime = 0.7f;
            hatchets.hitRadius = 0.5f; hatchets.spinDegrees = 900f; hatchets.pierce = true;

            BurstStrikeSkill splitter = Add<BurstStrikeSkill>(axe, player, hitStop, SkillType.SkullSplitter);
            splitter.mode = BurstStrikeSkill.StrikeMode.Slam;
            splitter.hitRadius = 1.3f; splitter.shockRadius = 2.4f; splitter.shockFraction = 0.5f;

            FanSkill javelin = Add<FanSkill>(spear, player, hitStop, SkillType.Javelin);
            javelin.projectileSprite = SpriteLoader.Load("Weapons/spear_iron");
            javelin.count = 1; javelin.speed = 17f; javelin.lifetime = 0.8f; javelin.hitRadius = 0.45f; javelin.pierce = true;

            BurstStrikeSkill triple = Add<BurstStrikeSkill>(spear, player, hitStop, SkillType.TripleThrust);
            triple.mode = BurstStrikeSkill.StrikeMode.Thrust;
            triple.strikes = 3; triple.reach = 2.2f; triple.hitRadius = 0.7f;

            RainSkill rain = Add<RainSkill>(spear, player, hitStop, SkillType.SpearRain);
            rain.dropSprite = SpriteLoader.Load("Weapons/spear_steel");
            rain.drops = 9; rain.areaRadius = 2.2f; rain.hitRadius = 0.7f;

            FanSkill multishot = Add<FanSkill>(bow, player, hitStop, SkillType.Multishot);
            multishot.projectileSprite = SpriteLoader.Load("Bow/arrow");
            multishot.count = 5; multishot.spreadDegrees = 50f; multishot.volleys = 2; multishot.speed = 14f;
            multishot.lifetime = 1.1f; multishot.hitRadius = 0.25f;

            FanSkill piercing = Add<FanSkill>(bow, player, hitStop, SkillType.PiercingShot);
            piercing.projectileSprite = SpriteLoader.Load("Bow/arrow");
            piercing.spriteScale = 1.6f; piercing.count = 1; piercing.speed = 20f; piercing.lifetime = 1f;
            piercing.hitRadius = 0.4f; piercing.pierce = true; piercing.windup = 0.2f;

            FanSkill knives = Add<FanSkill>(daggers, player, hitStop, SkillType.FanOfKnives);
            knives.projectileSprite = SpriteLoader.Load("Effects/knife");
            knives.count = 9; knives.spreadDegrees = 150f; knives.speed = 12f; knives.lifetime = 0.45f;
            knives.hitRadius = 0.3f; knives.windup = 0.08f;

            BurstStrikeSkill flurry = Add<BurstStrikeSkill>(daggers, player, hitStop, SkillType.ShadowFlurry);
            flurry.mode = BurstStrikeSkill.StrikeMode.Flurry;
            flurry.strikes = 8; flurry.interval = 0.07f; flurry.hitRadius = 0.9f; flurry.aimRange = 5f;

            BurstStrikeSkill titan = Add<BurstStrikeSkill>(hammer, player, hitStop, SkillType.TitanSmash);
            titan.mode = BurstStrikeSkill.StrikeMode.Slam;
            titan.hitRadius = 1.4f; titan.shockRadius = 3.2f; titan.shockFraction = 0.6f; titan.raiseTime = 0.35f;

            Add<NovaSkill>(hammer, player, hitStop, SkillType.SeismicSlam);

            FanSkill fireball = Add<FanSkill>(staff, player, hitStop, SkillType.Fireball);
            fireball.projectileSprite = SpriteLoader.Load("Effects/fireball");
            fireball.count = 1; fireball.speed = 10f; fireball.lifetime = 1f; fireball.hitRadius = 0.45f;
            fireball.explodeRadius = 1.6f; fireball.spriteScale = 1.3f; fireball.windup = 0.18f;

            ChainSkill chain = Add<ChainSkill>(staff, player, hitStop, SkillType.ChainLightning);
            chain.boltSprite = SpriteLoader.Load("Effects/bolt");

            RainSkill meteors = Add<RainSkill>(staff, player, hitStop, SkillType.MeteorShower);
            meteors.dropSprite = SpriteLoader.Load("Effects/fireball");
            meteors.burstSprite = SpriteLoader.Load("Effects/explosion");
            meteors.drops = 10; meteors.areaRadius = 2.6f; meteors.hitRadius = 1f; meteors.blastScale = 0.9f;
        }

        private static T Add<T>(GameObject weapon, GameObject player, HitStop hitStop, SkillType type) where T : WeaponSkillBase
        {
            T skill = weapon.AddComponent<T>();
            skill.skillType = type;
            skill.owner = player.transform;
            skill.facing = player.GetComponent<PlayerFacing>();
            skill.follower = weapon.GetComponent<HoverFollower>();
            skill.aim = weapon.GetComponent<AimRotation>();
            skill.idleRotator = weapon.GetComponent<IdleFacingRotator>();
            skill.hitStop = hitStop;
            skill.dustSprite = SpriteLoader.Load("Effects/dust");
            skill.sparkSprite = SpriteLoader.Load("Effects/spark");
            return skill;
        }

        private static void AddSkill<T>(GameObject swordObject, GameObject player, SwordAttack sword) where T : SwordSkillBase
        {
            T skill = swordObject.AddComponent<T>();
            skill.owner = player.transform;
            skill.sword = sword;
            skill.visual = swordObject.GetComponent<SwordVisual>();
            skill.follower = swordObject.GetComponent<HoverFollower>();
            skill.facing = player.GetComponent<PlayerFacing>();
            skill.hitStop = sword.hitStop;
        }
    }
}
