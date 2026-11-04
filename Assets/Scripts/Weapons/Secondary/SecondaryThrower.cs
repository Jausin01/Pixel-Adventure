using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Throws the secondary weapon worn in the Secondary slot (a shuriken, a boomerang or a bow) at the nearest enemy,
    /// or straight ahead when none is near. Each has its own cooldown; the boomerang is ready again when it comes back.
    /// The thrown object looks like the item's icon.
    /// </summary>
    public class SecondaryThrower : MonoBehaviour
    {
        [Header("References")]
        public Equipment equipment;
        public CharacterStats stats;
        public PlayerFacing facing;
        public Health health;
        public Material material;

        [Header("Throwing")]
        [Tooltip("How far an enemy can be to be auto-aimed.")]
        public float aimRange = 7f;
        [Tooltip("Height above the thrower's feet where throws start and where the boomerang is caught.")]
        public float chestHeight = 0.5f;
        public int sortingOrder = 9;

        [Header("Shuriken")]
        public float shurikenSpeed = 13f;
        public float shurikenLifetime = 1.1f;
        public float shurikenRadius = 0.2f;
        public float shurikenCooldown = 0.45f;

        [Header("Bow")]
        [Tooltip("Arrow shown when a bow is worn in the Secondary slot.")]
        public Sprite arrowSprite;
        public float arrowSpeed = 14f;
        public float arrowLifetime = 1.3f;
        public float arrowRadius = 0.2f;
        public float arrowCooldown = 0.55f;

        [Header("Boomerang")]
        public float boomerangRange = 4.2f;
        public float boomerangSpeed = 10f;
        public float boomerangRadius = 0.4f;
        [Tooltip("Pause after it is caught, before it can be thrown again.")]
        public float boomerangCooldown = 0.3f;

        private float readyAt;
        private float cooldownLength = 1f;
        private bool boomerangInFlight;

        /// <summary>The item in the Secondary slot, or null.</summary>
        public ItemDefinition Item { get { return equipment.Get(EquipmentSlot.Secondary); } }

        /// <summary>How much of the cooldown is left, from 1 (just thrown) to 0 (ready). Stays 1 while the boomerang is away.</summary>
        public float CooldownFraction
        {
            get
            {
                if (boomerangInFlight) return 1f;
                return Mathf.Clamp01((readyAt - Time.time) / cooldownLength);
            }
        }

        /// <summary>True when something is worn in the slot, the thrower is alive and the cooldown is over.</summary>
        public bool CanThrow { get { return Item != null && health.IsAlive && !boomerangInFlight && Time.time >= readyAt; } }

        /// <summary>Throws the worn weapon. Returns false when it cannot be thrown right now.</summary>
        public bool Throw()
        {
            if (!CanThrow) return false;

            ItemDefinition item = Item;
            Vector2 origin = (Vector2)transform.position + Vector2.up * chestHeight;
            Transform target = TargetFinder.FindNearest(transform.position, aimRange, transform);
            Vector2 direction = target != null ? ((Vector2)target.position + Vector2.up * 0.3f - origin).normalized : facing.Vector;
            if (direction.sqrMagnitude < 0.001f) direction = facing.Vector;

            if (item.equipKind == EquipmentKind.Boomerang) { Sfx.Play("boomerang"); ThrowBoomerang(item, origin, direction); }
            else if (item.equipKind == EquipmentKind.Bow) { Sfx.Play("bow_shoot"); ShootArrow(origin, direction); }
            else { Sfx.Play("shuriken_throw"); ThrowShuriken(item, origin, direction); }
            return true;
        }

        private void ThrowShuriken(ItemDefinition item, Vector2 origin, Vector2 direction)
        {
            GameObject shuriken = CreateProjectile("Shuriken", item, origin + direction * 0.4f);
            shuriken.AddComponent<Spinner>();
            shuriken.AddComponent<ProjectileMotion>().Launch(direction, shurikenSpeed, shurikenLifetime);
            ProjectileImpact impact = shuriken.AddComponent<ProjectileImpact>();
            impact.Configure(stats.SecondaryAttack, shurikenRadius, transform);
            impact.pushes = false;
            impact.hitSound = "shuriken_hit";

            cooldownLength = shurikenCooldown;
            readyAt = Time.time + shurikenCooldown;
        }

        private void ShootArrow(Vector2 origin, Vector2 direction)
        {
            GameObject arrow = new GameObject("Arrow", typeof(SpriteRenderer));
            arrow.transform.position = new Vector3(origin.x + direction.x * 0.4f, origin.y + direction.y * 0.4f, 0f);

            SpriteRenderer sprite = arrow.GetComponent<SpriteRenderer>();
            sprite.sprite = arrowSprite;
            sprite.sharedMaterial = material;
            sprite.sortingOrder = sortingOrder;

            arrow.AddComponent<ProjectileMotion>().Launch(direction, arrowSpeed, arrowLifetime);
            ProjectileImpact impact = arrow.AddComponent<ProjectileImpact>();
            impact.Configure(stats.SecondaryAttack, arrowRadius, transform);
            impact.pushes = false;

            cooldownLength = arrowCooldown;
            readyAt = Time.time + arrowCooldown;
        }

        private void ThrowBoomerang(ItemDefinition item, Vector2 origin, Vector2 direction)
        {
            GameObject boomerang = CreateProjectile("Boomerang", item, origin + direction * 0.4f);
            boomerang.AddComponent<Spinner>();
            boomerang.AddComponent<BoomerangFlight>().Launch(transform, direction, boomerangRange, boomerangSpeed,
                stats.SecondaryAttack, boomerangRadius, chestHeight, OnBoomerangReturned);

            boomerangInFlight = true;
        }

        private void OnBoomerangReturned()
        {
            boomerangInFlight = false;
            cooldownLength = boomerangCooldown;
            readyAt = Time.time + boomerangCooldown;
        }

        private GameObject CreateProjectile(string name, ItemDefinition item, Vector2 position)
        {
            GameObject projectile = new GameObject(name, typeof(SpriteRenderer));
            projectile.transform.position = new Vector3(position.x, position.y, 0f);

            SpriteRenderer sprite = projectile.GetComponent<SpriteRenderer>();
            sprite.sprite = item.icon;
            sprite.sharedMaterial = material;
            sprite.sortingOrder = sortingOrder;
            return projectile;
        }
    }
}
