using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>Which attack a monster uses.</summary>
    public enum MonsterAttackKind
    {
        Claws,
        Chomp,
        Boss
    }

    /// <summary>
    /// Describes one kind of monster: art, stats and attack. <see cref="MonsterBuilder"/> turns it into an object.
    /// </summary>
    public class MonsterSpec
    {
        public string name;
        public string framesPrefix;          // for example "Enemies/rat_brown_"
        public int frameCount = 3;
        public string effectSprite;          // attack sprite, for example "Effects/claws_brown"
        public MonsterAttackKind attack;

        public float maxHealth;
        public float moveSpeed;
        public float damage;
        public float attackRange;
        public float sightRange = 6f;

        public Vector2 colliderSize;
        public Vector2 colliderOffset;
        public float healthBarHeight;

        public bool animateWhenIdle;
        public bool spriteFacesRight;        // true: mirror the sprite to look at the target

        public BossMoves bossMoves;          // bosses only: the moves they pick from
        public string projectileSprite;      // bosses only: what their volley fires, for example "Effects/rock"

        /// <summary>Brown rat: slow, tough, slashes with claws.</summary>
        public static MonsterSpec BrownRat()
        {
            return new MonsterSpec
            {
                name = "Brown Rat", framesPrefix = "Enemies/rat_brown_", effectSprite = "Effects/claws_brown",
                attack = MonsterAttackKind.Claws, maxHealth = 40f, moveSpeed = 1.8f, damage = 8f, attackRange = 1f,
                colliderSize = new Vector2(0.8f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.8f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>White rat: faster and more fragile than the brown one.</summary>
        public static MonsterSpec WhiteRat()
        {
            return new MonsterSpec
            {
                name = "White Rat", framesPrefix = "Enemies/rat_white_", effectSprite = "Effects/claws_white",
                attack = MonsterAttackKind.Claws, maxHealth = 30f, moveSpeed = 2.4f, damage = 6f, attackRange = 1f,
                colliderSize = new Vector2(0.8f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.8f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>Bat: flies fast and bites.</summary>
        public static MonsterSpec Bat()
        {
            return new MonsterSpec
            {
                name = "Bat", framesPrefix = "Enemies/bat_", effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Chomp, maxHealth = 25f, moveSpeed = 2.8f, damage = 6f, attackRange = 0.9f,
                colliderSize = new Vector2(0.9f, 0.35f), colliderOffset = new Vector2(0f, 0.44f),
                healthBarHeight = 1.0f, animateWhenIdle = true, spriteFacesRight = false
            };
        }

        /// <summary>Zombie: slow and tough, claws at close range.</summary>
        public static MonsterSpec Zombie()
        {
            return new MonsterSpec
            {
                name = "Zombie", framesPrefix = "Enemies/zombie_", effectSprite = "Effects/claws_green",
                attack = MonsterAttackKind.Claws, maxHealth = 75f, moveSpeed = 1.2f, damage = 12f, attackRange = 1.1f,
                colliderSize = new Vector2(0.6f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 1.1f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>Skeleton: quick and brittle, slashes with a rusty blade.</summary>
        public static MonsterSpec Skeleton()
        {
            return new MonsterSpec
            {
                name = "Skeleton", framesPrefix = "Enemies/skeleton_", effectSprite = "Effects/claws_bone",
                attack = MonsterAttackKind.Claws, maxHealth = 45f, moveSpeed = 2.2f, damage = 9f, attackRange = 1.1f,
                colliderSize = new Vector2(0.5f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 1.1f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>Slime: bounces along slowly, bites with dripping jaws. Keeps bouncing when idle.</summary>
        public static MonsterSpec Slime()
        {
            return new MonsterSpec
            {
                name = "Slime", framesPrefix = "Enemies/slime_", effectSprite = "Effects/slime_drips",
                attack = MonsterAttackKind.Chomp, maxHealth = 55f, moveSpeed = 1.4f, damage = 7f, attackRange = 0.9f,
                colliderSize = new Vector2(0.8f, 0.45f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.85f, animateWhenIdle = true, spriteFacesRight = false
            };
        }

        /// <summary>Spider: very fast and fragile, bites.</summary>
        public static MonsterSpec Spider()
        {
            return new MonsterSpec
            {
                name = "Spider", framesPrefix = "Enemies/spider_", effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Chomp, maxHealth = 22f, moveSpeed = 3.0f, damage = 5f, attackRange = 0.9f,
                colliderSize = new Vector2(0.9f, 0.35f), colliderOffset = new Vector2(0f, 0.2f),
                healthBarHeight = 0.7f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>Moss brute: tough forest bruiser.</summary>
        public static MonsterSpec MossBrute()
        {
            return new MonsterSpec
            {
                name = "Moss Brute", framesPrefix = "Enemies/moss_brute_", frameCount = 4, effectSprite = "Effects/claws_green",
                attack = MonsterAttackKind.Claws, maxHealth = 90f, moveSpeed = 1.3f, damage = 13f, attackRange = 1.2f,
                colliderSize = new Vector2(0.7f, 0.45f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 1.2f, animateWhenIdle = false, spriteFacesRight = false
            };
        }

        /// <summary>Shadow boar: fast charger that gores.</summary>
        public static MonsterSpec ShadowBoar()
        {
            return new MonsterSpec
            {
                name = "Shadow Boar", framesPrefix = "Enemies/shadow_boar_", frameCount = 4, effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Chomp, maxHealth = 60f, moveSpeed = 2.6f, damage = 10f, attackRange = 1.0f,
                colliderSize = new Vector2(0.8f, 0.45f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 1.0f, animateWhenIdle = false, spriteFacesRight = false
            };
        }

        /// <summary>Cave gnawer: quick biter from the dark.</summary>
        public static MonsterSpec CaveGnawer()
        {
            return new MonsterSpec
            {
                name = "Cave Gnawer", framesPrefix = "Enemies/cave_gnawer_", frameCount = 4, effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Chomp, maxHealth = 40f, moveSpeed = 2.7f, damage = 8f, attackRange = 0.9f,
                colliderSize = new Vector2(0.7f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.9f, animateWhenIdle = false, spriteFacesRight = false
            };
        }

        /// <summary>Hollow knight: armored and slow, hits hard.</summary>
        public static MonsterSpec HollowKnight()
        {
            return new MonsterSpec
            {
                name = "Hollow Knight", framesPrefix = "Enemies/hollow_knight_", frameCount = 4, effectSprite = "Effects/claws_bone",
                attack = MonsterAttackKind.Claws, maxHealth = 110f, moveSpeed = 1.4f, damage = 15f, attackRange = 1.2f,
                colliderSize = new Vector2(0.6f, 0.45f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 1.3f, animateWhenIdle = false, spriteFacesRight = false
            };
        }

        /// <summary>Wraith: floating ghost that never stops moving.</summary>
        public static MonsterSpec Wraith()
        {
            return new MonsterSpec
            {
                name = "Wraith", framesPrefix = "Enemies/wraith_", frameCount = 4, effectSprite = "Effects/claws_bone",
                attack = MonsterAttackKind.Claws, maxHealth = 50f, moveSpeed = 2.0f, damage = 11f, attackRange = 1.1f,
                colliderSize = new Vector2(0.6f, 0.4f), colliderOffset = new Vector2(0f, 0.45f),
                healthBarHeight = 1.3f, animateWhenIdle = true, spriteFacesRight = false
            };
        }

        /// <summary>Ogre King boss.</summary>
        public static MonsterSpec OgreKing()
        {
            return new MonsterSpec
            {
                name = "Ogre King", framesPrefix = "Enemies/ogre_king_", frameCount = 4, effectSprite = "Effects/claws_brown",
                attack = MonsterAttackKind.Boss, maxHealth = 420f, moveSpeed = 1.4f, damage = 20f, attackRange = 3.2f, sightRange = 9f,
                colliderSize = new Vector2(1.4f, 0.8f), colliderOffset = new Vector2(0f, 0.4f),
                healthBarHeight = 3.6f, animateWhenIdle = false, spriteFacesRight = false,
                bossMoves = BossMoves.Slam | BossMoves.Charge | BossMoves.Rain, projectileSprite = "Effects/rock"
            };
        }

        /// <summary>Stone Colossus boss: slow and enormous.</summary>
        public static MonsterSpec StoneColossus()
        {
            return new MonsterSpec
            {
                name = "Stone Colossus", framesPrefix = "Enemies/stone_colossus_", frameCount = 4, effectSprite = "Effects/claws_bone",
                attack = MonsterAttackKind.Boss, maxHealth = 560f, moveSpeed = 1.0f, damage = 24f, attackRange = 3.2f, sightRange = 9f,
                colliderSize = new Vector2(1.6f, 0.9f), colliderOffset = new Vector2(0f, 0.4f),
                healthBarHeight = 3.8f, animateWhenIdle = false, spriteFacesRight = false,
                bossMoves = BossMoves.Slam | BossMoves.Rain | BossMoves.Volley, projectileSprite = "Effects/rock"
            };
        }

        /// <summary>Ashen Drake boss: fast, bites.</summary>
        public static MonsterSpec AshenDrake()
        {
            return new MonsterSpec
            {
                name = "Ashen Drake", framesPrefix = "Enemies/ashen_drake_", frameCount = 4, effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Boss, maxHealth = 380f, moveSpeed = 1.8f, damage = 18f, attackRange = 3.2f, sightRange = 9f,
                colliderSize = new Vector2(1.5f, 0.8f), colliderOffset = new Vector2(0f, 0.5f),
                healthBarHeight = 3.6f, animateWhenIdle = false, spriteFacesRight = false,
                bossMoves = BossMoves.Charge | BossMoves.Volley | BossMoves.Rain, projectileSprite = "Effects/fireball"
            };
        }

        /// <summary>Horned Tyrant boss.</summary>
        public static MonsterSpec HornedTyrant()
        {
            return new MonsterSpec
            {
                name = "Horned Tyrant", framesPrefix = "Enemies/horned_tyrant_", frameCount = 4, effectSprite = "Effects/claws_brown",
                attack = MonsterAttackKind.Boss, maxHealth = 480f, moveSpeed = 1.6f, damage = 22f, attackRange = 3.2f, sightRange = 9f,
                colliderSize = new Vector2(1.4f, 0.8f), colliderOffset = new Vector2(0f, 0.4f),
                healthBarHeight = 3.8f, animateWhenIdle = false, spriteFacesRight = false,
                bossMoves = BossMoves.Slam | BossMoves.Charge | BossMoves.Volley, projectileSprite = "Effects/fireball"
            };
        }

        /// <summary>Thorn Beast boss.</summary>
        public static MonsterSpec ThornBeast()
        {
            return new MonsterSpec
            {
                name = "Thorn Beast", framesPrefix = "Enemies/thorn_beast_", frameCount = 4, effectSprite = "Effects/claws_green",
                attack = MonsterAttackKind.Boss, maxHealth = 340f, moveSpeed = 1.9f, damage = 17f, attackRange = 3.2f, sightRange = 9f,
                colliderSize = new Vector2(1.4f, 0.8f), colliderOffset = new Vector2(0f, 0.4f),
                healthBarHeight = 3.4f, animateWhenIdle = false, spriteFacesRight = false,
                bossMoves = BossMoves.Charge | BossMoves.Volley | BossMoves.Rain, projectileSprite = "Effects/thorn"
            };
        }
    }
}
