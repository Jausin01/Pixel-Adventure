using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates a monster from a <see cref="MonsterSpec"/>: body, health and feedback, movement, brain, attack and health bar.
    /// </summary>
    public static class MonsterBuilder
    {
        /// <summary>Builds a monster that chases <paramref name="target"/>.</summary>
        public static GameObject Build(MonsterSpec spec, Material material, Transform target, Vector2 position)
        {
            GameObject monster = new GameObject(spec.name);
            monster.transform.position = position;

            Sprite[] frames = SpriteLoader.LoadFrames(spec.framesPrefix, spec.frameCount);
            SpriteRenderer renderer = monster.AddComponent<SpriteRenderer>();
            renderer.sprite = frames[0];
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            BoxCollider2D collider = monster.AddComponent<BoxCollider2D>();
            collider.size = spec.colliderSize;
            collider.offset = spec.colliderOffset;

            SpriteRenderer shadow = CreateShadow(monster.transform, material);

            Health health = monster.AddComponent<Health>();
            health.maxHealth = spec.maxHealth;
            monster.AddComponent<HitFlash>().target = renderer;
            monster.AddComponent<Knockback>();

            DeathVisibility death = monster.AddComponent<DeathVisibility>();
            death.renderers = new[] { renderer, shadow };
            death.colliders = new Collider2D[] { collider };
            monster.AddComponent<RespawnTimer>().delay = 10f;

            EnemyMovement movement = monster.AddComponent<EnemyMovement>();
            movement.moveSpeed = spec.moveSpeed;

            EnemySpriteAnimator animator = monster.AddComponent<EnemySpriteAnimator>();
            animator.movement = movement;
            animator.frames = frames;
            animator.animateWhenIdle = spec.animateWhenIdle;

            if (spec.spriteFacesRight)
            {
                FaceTargetFlipper flipper = monster.AddComponent<FaceTargetFlipper>();
                flipper.target = target;
                flipper.spriteFacesRight = true;
            }

            AddAttack(monster, spec, material);

            EnemyChaseBrain brain = monster.AddComponent<EnemyChaseBrain>();
            brain.target = target;
            brain.sightRange = spec.sightRange;
            brain.attackRange = spec.attackRange;

            WorldHealthBarBuilder.Build(monster.transform, material, health, new Vector2(0f, spec.healthBarHeight));
            return monster;
        }

        private static void AddAttack(GameObject monster, MonsterSpec spec, Material material)
        {
            Sprite effect = SpriteLoader.Load(spec.effectSprite);

            if (spec.attack == MonsterAttackKind.Claws)
            {
                ClawAttack claws = monster.AddComponent<ClawAttack>();
                claws.clawSprite = effect;
                claws.material = material;
                claws.damage = spec.damage;
            }
            else
            {
                ChompAttack chomp = monster.AddComponent<ChompAttack>();
                chomp.fangSprite = effect;
                chomp.material = material;
                chomp.damage = spec.damage;
            }
        }

        private static SpriteRenderer CreateShadow(Transform parent, Material material)
        {
            GameObject go = new GameObject("Shadow");
            go.transform.SetParent(parent, false);
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/shadow");
            renderer.sharedMaterial = material;
            renderer.sortingOrder = -1;
            return renderer;
        }
    }
}
