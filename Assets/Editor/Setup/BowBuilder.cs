using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the bow weapon. It starts inactive; the weapon holder activates it when selected.
    /// </summary>
    public static class BowBuilder
    {
        /// <summary>Builds the bow and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player)
        {
            GameObject bow = new GameObject("Bow");
            bow.transform.position = player.transform.position + new Vector3(0.75f, 0.95f, 0f);

            SpriteRenderer renderer = bow.AddComponent<SpriteRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = 10;

            PlayerFacing facing = player.GetComponent<PlayerFacing>();

            BowVisual visual = bow.AddComponent<BowVisual>();
            visual.idle = SpriteLoader.Load("Bow/bow_idle");
            visual.nock = SpriteLoader.Load("Bow/bow_nock");
            visual.draw = SpriteLoader.Load("Bow/bow_draw");
            visual.release = SpriteLoader.Load("Bow/bow_release");

            HoverFollower follower = bow.AddComponent<HoverFollower>();
            follower.target = player.transform;
            follower.facing = facing;

            AimRotation aim = bow.AddComponent<AimRotation>();
            IdleFacingRotator rotator = bow.AddComponent<IdleFacingRotator>();
            rotator.facing = facing;

            ArrowSpawner spawner = bow.AddComponent<ArrowSpawner>();
            spawner.arrowSprite = SpriteLoader.Load("Bow/arrow");
            spawner.material = material;
            spawner.shooter = player.transform;

            BowAttack attack = bow.AddComponent<BowAttack>();
            attack.owner = player.transform;
            attack.facing = facing;
            attack.visual = visual;
            attack.aim = aim;
            attack.idleRotator = rotator;
            attack.spawner = spawner;

            bow.SetActive(false);
            return bow;
        }
    }
}
