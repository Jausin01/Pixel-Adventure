using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the flying sword weapon.
    /// </summary>
    public static class SwordBuilder
    {
        private static readonly string[] Colors = { "silver", "blue", "red", "black", "green" };

        /// <summary>Builds the sword and wires its components to the player.</summary>
        public static GameObject Build(Material material, GameObject player, HitStop hitStop)
        {
            GameObject sword = new GameObject("Sword");
            sword.transform.position = player.transform.position + new Vector3(0.75f, 0.95f, 0f);

            SpriteRenderer renderer = sword.AddComponent<SpriteRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = 10;

            SwordVisual visual = sword.AddComponent<SwordVisual>();
            visual.skins = BuildSkins();

            PlayerFacing facing = player.GetComponent<PlayerFacing>();

            HoverFollower follower = sword.AddComponent<HoverFollower>();
            follower.target = player.transform;
            follower.facing = facing;

            SwordAttack attack = sword.AddComponent<SwordAttack>();
            attack.owner = player.transform;
            attack.facing = facing;
            attack.visual = visual;
            attack.follower = follower;
            attack.hitStop = hitStop;
            return sword;
        }

        private static SwordSkin[] BuildSkins()
        {
            SwordSkin[] skins = new SwordSkin[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                string prefix = "Swords/sword_" + Colors[i] + "_";
                skins[i] = new SwordSkin
                {
                    name = Colors[i],
                    up = SpriteLoader.Load(prefix + "up"),
                    diagonal = SpriteLoader.Load(prefix + "diag"),
                    smear = SpriteLoader.Load(prefix + "smear"),
                    side = SpriteLoader.Load(prefix + "side")
                };
            }
            return skins;
        }
    }
}
