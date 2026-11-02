using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the player object with all of its components.
    /// </summary>
    public static class PlayerBuilder
    {
        /// <summary>Builds the player. The joystick reference is assigned later by the scene builder.</summary>
        public static GameObject Build(Material material)
        {
            GameObject player = new GameObject("Player");

            SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("Player/idle_down");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot; // sort by the feet, not the sprite center

            Rigidbody2D body = player.AddComponent<Rigidbody2D>();
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.4f, 0.25f);
            collider.offset = new Vector2(0f, 0.12f);

            PlayerMoveInput input = player.AddComponent<PlayerMoveInput>();
            player.AddComponent<PlayerMovement>();
            PlayerFacing facing = player.AddComponent<PlayerFacing>();

            PlayerSpriteAnimator animator = player.AddComponent<PlayerSpriteAnimator>();
            animator.input = input;
            animator.facing = facing;
            animator.walkDown = SpriteLoader.LoadFrames("Player/walk_down_", 4);
            animator.walkUp = SpriteLoader.LoadFrames("Player/walk_up_", 4);
            animator.walkLeft = SpriteLoader.LoadFrames("Player/walk_left_", 4);
            animator.walkRight = SpriteLoader.LoadFrames("Player/walk_right_", 4);

            CreateShadow(player.transform, material);
            return player;
        }

        private static void CreateShadow(Transform parent, Material material)
        {
            GameObject shadow = new GameObject("Shadow");
            shadow.transform.SetParent(parent, false);
            SpriteRenderer renderer = shadow.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/shadow");
            renderer.sharedMaterial = material;
            renderer.sortingOrder = -1;
        }
    }
}
