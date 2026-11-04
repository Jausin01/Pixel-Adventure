using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace MobileRpg
{
    /// <summary>
    /// An arena made of several looks ("stages") stacked in the same place. Only one is shown at a time; changing
    /// stage is a transformation: the old ground and props dissolve while the new ones take their place, the screen
    /// shakes, sparks fly and the light changes colour. The new props only become solid when it is over, and a player
    /// standing where one appeared is moved out of it.
    /// </summary>
    public class ArenaStages : MonoBehaviour
    {
        /// <summary>One look of the arena.</summary>
        [System.Serializable]
        public class Stage
        {
            [Tooltip("Parent of the whole stage: ground, walls and props.")]
            public GameObject root;
            [Tooltip("Parent of the props. Their colliders are off while the stage is appearing.")]
            public Transform props;
            public Color ambientColor = Color.white;
            public float ambientIntensity = 1f;
        }

        public ArenaSite site;
        public Stage[] stages;
        public Sprite sparkSprite;
        public Material sparkMaterial;

        [Header("Transformation")]
        public float duration = 2.6f;
        public float shakeStrength = 0.12f;
        [Tooltip("Sparks created per second during the transformation.")]
        public float sparksPerSecond = 40f;

        /// <summary>Number of stages.</summary>
        public int Count { get { return stages.Length; } }

        /// <summary>True while a transformation is playing.</summary>
        public bool Morphing { get; private set; }

        private int current;

        /// <summary>Shows one stage at once, with no transformation.</summary>
        public void ShowImmediate(int index)
        {
            StopAllCoroutines();
            Morphing = false;
            current = index % stages.Length;

            for (int i = 0; i < stages.Length; i++)
            {
                SetAlpha(stages[i], 1f);
                SetPropColliders(stages[i], true);
                stages[i].root.SetActive(i == current);
            }
            ApplyAmbient(stages[current].ambientColor, stages[current].ambientIntensity);
        }

        /// <summary>Plays the transformation into another stage.</summary>
        public void MorphTo(int index, Transform player, CameraFollow cameraFollow)
        {
            index %= stages.Length;
            if (index == current || Morphing) return;
            StartCoroutine(Morph(index, player, cameraFollow));
        }

        private IEnumerator Morph(int next, Transform player, CameraFollow cameraFollow)
        {
            Morphing = true;
            Sfx.Play("arena_transform");
            Stage from = stages[current];
            Stage to = stages[next];

            SetPropColliders(from, false); // nothing blocks the way through the old props while they fade
            SetPropColliders(to, false);
            SetAlpha(to, 0f);
            to.root.SetActive(true);

            Color startColor = site.ambientColor;
            float startIntensity = site.ambientIntensity;
            float sparkTimer = 0f;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float progress = t / duration;
                SetAlpha(from, 1f - progress);
                SetAlpha(to, progress);
                ApplyAmbient(Color.Lerp(startColor, to.ambientColor, progress), Mathf.Lerp(startIntensity, to.ambientIntensity, progress));

                if (cameraFollow != null) cameraFollow.Shake(shakeStrength, 0.1f);

                sparkTimer += Time.deltaTime * sparksPerSecond;
                while (sparkTimer >= 1f)
                {
                    sparkTimer -= 1f;
                    Spark(player.position);
                }
                yield return null;
            }

            SetAlpha(to, 1f);
            from.root.SetActive(false);
            SetAlpha(from, 1f);
            SetPropColliders(to, true);
            ApplyAmbient(to.ambientColor, to.ambientIntensity);
            MoveOutOfProps(player, to);

            current = next;
            Morphing = false;
        }

        private void ApplyAmbient(Color color, float intensity)
        {
            site.ambientColor = color;
            site.ambientIntensity = intensity;
        }

        private void Spark(Vector2 around)
        {
            if (sparkSprite == null) return;

            Vector2 position = around + Random.insideUnitCircle * 9f;
            EffectFade.Spawn(sparkSprite, sparkMaterial, position, Random.Range(0f, 90f), Vector2.one * Random.Range(0.8f, 1.8f),
                Random.Range(0.4f, 0.9f), 0.2f, 20);
        }

        private static void SetAlpha(Stage stage, float alpha)
        {
            SpriteRenderer[] sprites = stage.root.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < sprites.Length; i++)
            {
                Color color = sprites[i].color;
                color.a = alpha;
                sprites[i].color = color;
            }

            Tilemap[] maps = stage.root.GetComponentsInChildren<Tilemap>(true);
            for (int i = 0; i < maps.Length; i++)
            {
                Color color = maps[i].color;
                color.a = alpha;
                maps[i].color = color;
            }

            Light2D[] lights = stage.root.GetComponentsInChildren<Light2D>(true);
            for (int i = 0; i < lights.Length; i++) lights[i].enabled = alpha > 0.5f;
        }

        private static void SetPropColliders(Stage stage, bool enabled)
        {
            Collider2D[] colliders = stage.props.GetComponentsInChildren<Collider2D>(true);
            for (int i = 0; i < colliders.Length; i++) colliders[i].enabled = enabled;
        }

        // A prop may have appeared on top of the player: push the player to the nearest free spot.
        private static void MoveOutOfProps(Transform player, Stage stage)
        {
            Collider2D[] colliders = stage.props.GetComponentsInChildren<Collider2D>();
            Vector2 start = player.position;
            if (IsFree(colliders, start)) return;

            for (float radius = 0.5f; radius <= 8f; radius += 0.5f)
            {
                for (int step = 0; step < 16; step++)
                {
                    float angle = step * Mathf.PI * 2f / 16f;
                    Vector2 point = start + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    if (!IsFree(colliders, point)) continue;

                    player.position = point;
                    Rigidbody2D body = player.GetComponent<Rigidbody2D>();
                    if (body != null) body.position = point;
                    return;
                }
            }
        }

        private static bool IsFree(Collider2D[] colliders, Vector2 point)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Bounds bounds = colliders[i].bounds;
                bounds.Expand(0.8f);
                if (bounds.Contains(new Vector3(point.x, point.y, bounds.center.z))) return false;
            }
            return true;
        }
    }
}
