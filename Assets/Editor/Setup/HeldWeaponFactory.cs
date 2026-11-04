using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the parts every hovering weapon shares: the object with its sprite and skins, the follower and the aim rotation.
    /// The weapon-specific builders add their attack on top. The object starts inactive.
    /// </summary>
    public static class HeldWeaponFactory
    {
        /// <summary>Creates the weapon object. The skins are art files named <c>Weapons/&lt;prefix&gt;_&lt;variant&gt;</c>.</summary>
        public static GameObject Create(string name, string prefix, string[] variants, Material material, GameObject player,
            out HoverFollower follower, out AimRotation aim)
        {
            GameObject weapon = new GameObject(name);
            weapon.transform.position = player.transform.position + new Vector3(0.75f, 0.95f, 0f);

            SpriteRenderer renderer = weapon.AddComponent<SpriteRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = 10;
            renderer.sprite = SpriteLoader.Load("Weapons/" + prefix + "_" + variants[0]);

            WeaponSkinSet skins = weapon.AddComponent<WeaponSkinSet>();
            skins.skins = new Sprite[variants.Length];
            for (int i = 0; i < variants.Length; i++) skins.skins[i] = SpriteLoader.Load("Weapons/" + prefix + "_" + variants[i]);

            follower = weapon.AddComponent<HoverFollower>();
            follower.target = player.transform;
            follower.facing = player.GetComponent<PlayerFacing>();

            aim = weapon.AddComponent<AimRotation>();
            weapon.SetActive(false);
            return weapon;
        }
    }
}
