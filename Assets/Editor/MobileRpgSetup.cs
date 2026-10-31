using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

// Menu: Tools > Mobile RPG > Setup Everything
// Sets mobile project settings, re-imports the art, builds the test scene (Assets/Scenes/Hub.unity) and wires every reference.
public static class MobileRpgSetup
{
    const string ART = "Assets/Art/";
    const int PPU = 16;
    const int REF_W = 256;    // pixel-perfect reference resolution (change to 320x180 for a wider view)
    const int REF_H = 144;

    static readonly string[] SkinNames = { "silver", "blue", "red", "black", "green" };

    [MenuItem("Tools/Mobile RPG/Setup Everything")]
    public static void SetupEverything()
    {
        ApplyProjectSettings();
        ReimportArt();
        BuildScene();
    }

    [MenuItem("Tools/Mobile RPG/1 - Apply Project Settings")]
    public static void ApplyProjectSettings()
    {
        PlayerSettings.productName = "MobileRPG";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;

        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // sprites with a higher Y are drawn behind (top-down depth sorting)
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0f, 1f, 0f);

        Debug.Log("[MobileRPG] Project settings applied (landscape, IL2CPP, ARM64, Y-sorting).");
    }

    [MenuItem("Tools/Mobile RPG/2 - Reimport Art")]
    public static void ReimportArt()
    {
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset("Assets/Art", ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
    }

    [MenuItem("Tools/Mobile RPG/3 - Build Test Scene")]
    public static void BuildScene()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Material mat = SpriteMaterial();

        // ---------------------------------------------------------------- world
        GameObject ground = new GameObject("Ground");
        SpriteRenderer gsr = ground.AddComponent<SpriteRenderer>();
        gsr.sprite = S("World/ground.png");
        gsr.sharedMaterial = mat;
        gsr.drawMode = SpriteDrawMode.Tiled;
        gsr.size = new Vector2(30f, 20f);
        gsr.sortingOrder = -100;

        GameObject bounds = new GameObject("Bounds");
        AddWall(bounds, new Vector2(31f, 1f), new Vector2(0f, 10.5f));
        AddWall(bounds, new Vector2(31f, 1f), new Vector2(0f, -10.5f));
        AddWall(bounds, new Vector2(1f, 21f), new Vector2(-15.5f, 0f));
        AddWall(bounds, new Vector2(1f, 21f), new Vector2(15.5f, 0f));

        // --------------------------------------------------------------- player
        GameObject player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        SpriteRenderer psr = player.AddComponent<SpriteRenderer>();
        psr.sprite = S("Player/walk_down_0.png");
        psr.sharedMaterial = mat;
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        BoxCollider2D pcol = player.AddComponent<BoxCollider2D>();
        pcol.size = new Vector2(0.4f, 0.25f);
        pcol.offset = new Vector2(0f, 0.12f);
        PlayerController pc = player.AddComponent<PlayerController>();
        PlayerSpriteAnimator anim = player.AddComponent<PlayerSpriteAnimator>();
        anim.player = pc;
        anim.walkDown = Frames("walk_down");
        anim.walkUp = Frames("walk_up");
        anim.walkLeft = Frames("walk_left");
        anim.walkRight = Frames("walk_right");

        GameObject shadow = new GameObject("Shadow");
        shadow.transform.SetParent(player.transform, false);
        shadow.transform.localPosition = new Vector3(0f, 0.08f, 0f);
        SpriteRenderer ssr = shadow.AddComponent<SpriteRenderer>();
        ssr.sprite = S("World/shadow.png");
        ssr.sharedMaterial = mat;
        ssr.sortingOrder = -1;

        // ---------------------------------------------------------------- sword
        GameObject swordGo = new GameObject("FlyingSword");
        swordGo.transform.position = new Vector3(0.75f, 0.95f, 0f);
        SpriteRenderer wsr = swordGo.AddComponent<SpriteRenderer>();
        wsr.sharedMaterial = mat;
        wsr.sortingOrder = 10;
        FlyingSword sword = swordGo.AddComponent<FlyingSword>();
        sword.player = pc;
        sword.skins = new SwordSkin[SkinNames.Length];
        for (int i = 0; i < SkinNames.Length; i++)
        {
            string c = SkinNames[i];
            SwordSkin sk = new SwordSkin();
            sk.name = c;
            sk.up = S("Swords/sword_" + c + "_up.png");
            sk.diag = S("Swords/sword_" + c + "_diag.png");
            sk.smear = S("Swords/sword_" + c + "_smear.png");
            sk.side = S("Swords/sword_" + c + "_side.png");
            sword.skins[i] = sk;
        }
        sword.skinIndex = 0;
        wsr.sprite = sword.skins[0].up;

        // ------------------------------------------------------------------ bow
        GameObject bowGo = new GameObject("FlyingBow");
        bowGo.transform.position = new Vector3(0.8f, 0.8f, 0f);
        SpriteRenderer bsr = bowGo.AddComponent<SpriteRenderer>();
        bsr.sharedMaterial = mat;
        bsr.sortingOrder = 10;
        bsr.sprite = S("Bow/bow_idle.png");
        FlyingBow bow = bowGo.AddComponent<FlyingBow>();
        bow.player = pc;
        bow.idle = S("Bow/bow_idle.png");
        bow.nock = S("Bow/bow_nock.png");
        bow.draw = S("Bow/bow_draw.png");
        bow.release = S("Bow/bow_release.png");
        bow.arrowSprite = S("Bow/arrow.png");
        bowGo.SetActive(false);

        GameObject holderGo = new GameObject("Weapons");
        WeaponHolder holder = holderGo.AddComponent<WeaponHolder>();
        holder.sword = sword;
        holder.bow = bow;

        // -------------------------------------------------------------- dummies
        MakeDummy(mat, new Vector3(3.5f, 1.5f, 0f), "TrainingDummy_1");
        MakeDummy(mat, new Vector3(-3.5f, -1.0f, 0f), "TrainingDummy_2");

        // --------------------------------------------------------------- camera
        GameObject camGo = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        camGo.tag = "MainCamera";
        camGo.transform.position = new Vector3(0f, 0f, -10f);
        Camera cam = camGo.GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = REF_H / 2f / PPU;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.08f, 0.1f, 0.08f);
        CameraFollow follow = camGo.AddComponent<CameraFollow>();
        follow.target = player.transform;
        AddPixelPerfect(camGo);

        // ------------------------------------------------------------------ UI
        BuildUI(holder);

        // ------------------------------------------------------------ bootstrap
        new GameObject("GameBootstrap").AddComponent<GameBootstrap>();

        // -------------------------------------------------------------- prefabs
        Directory.CreateDirectory("Assets/Prefabs");
        Directory.CreateDirectory("Assets/Scenes");
        PrefabUtility.SaveAsPrefabAssetAndConnect(player, "Assets/Prefabs/Player.prefab", InteractionMode.AutomatedAction);

        // ----------------------------------------------------------------- save
        string scenePath = "Assets/Scenes/Hub.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
        Selection.activeGameObject = player;
        Debug.Log("[MobileRPG] Scene built: " + scenePath + ". Press Play (WASD/arrows + Space, or touch).");
    }

    // ------------------------------------------------------------------ helpers
    static Sprite S(string rel)
    {
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(ART + rel);
        if (s == null) Debug.LogError("[MobileRPG] Missing sprite: " + ART + rel);
        return s;
    }

    static Sprite[] Frames(string prefix)
    {
        Sprite[] a = new Sprite[4];
        for (int i = 0; i < 4; i++) a[i] = S("Player/" + prefix + "_" + i + ".png");
        return a;
    }

    static Material SpriteMaterial()
    {
        string path = ART + "SpriteUnlit.mat";
        Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null)
        {
            m = new Material(Shader.Find("Sprites/Default"));
            AssetDatabase.CreateAsset(m, path);
        }
        return m;
    }

    static void AddWall(GameObject parent, Vector2 size, Vector2 offset)
    {
        BoxCollider2D b = parent.AddComponent<BoxCollider2D>();
        b.size = size;
        b.offset = offset;
    }

    static void MakeDummy(Material mat, Vector3 pos, string name)
    {
        GameObject d = new GameObject(name);
        d.transform.position = pos;
        SpriteRenderer sr = d.AddComponent<SpriteRenderer>();
        sr.sprite = S("World/dummy.png");
        sr.sharedMaterial = mat;
        CircleCollider2D c = d.AddComponent<CircleCollider2D>();
        c.radius = 0.4f;
        c.offset = new Vector2(0f, 0.45f);
        d.AddComponent<TrainingDummy>();
    }

    static void AddPixelPerfect(GameObject cam)
    {
        // URP's Pixel Perfect Camera, added by name so this script never breaks if the package changes.
        System.Type t = System.Type.GetType("UnityEngine.Rendering.Universal.PixelPerfectCamera, Unity.RenderPipelines.Universal.Runtime");
        if (t == null)
        {
            Debug.LogWarning("[MobileRPG] Pixel Perfect Camera not found. Add it manually: Camera > Add Component > Pixel Perfect Camera (PPU 16, ref 256x144).");
            return;
        }
        Component c = cam.AddComponent(t);
        SerializedObject so = new SerializedObject(c);
        SetInt(so, "m_AssetsPPU", PPU);
        SetInt(so, "m_RefResolutionX", REF_W);
        SetInt(so, "m_RefResolutionY", REF_H);
        SerializedProperty snap = so.FindProperty("m_PixelSnapping");
        if (snap != null) snap.boolValue = true;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void SetInt(SerializedObject so, string prop, int v)
    {
        SerializedProperty p = so.FindProperty(prop);
        if (p != null) p.intValue = v;
    }

    // ---------------------------------------------------------------------- UI
    static RectTransform UIImage(string name, Transform parent, Sprite sprite, Color color, bool raycast,
                                 Vector2 aMin, Vector2 aMax, Vector2 pivot, Vector2 pos, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = (RectTransform)go.transform;
        rt.anchorMin = aMin;
        rt.anchorMax = aMax;
        rt.pivot = pivot;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
        img.color = color;
        img.raycastTarget = raycast;
        return rt;
    }

    static void BuildUI(WeaponHolder holder)
    {
        GameObject canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        Vector2 mid = new Vector2(0.5f, 0.5f);
        Color white = Color.white;

        // floating joystick (left half of the screen)
        RectTransform zone = UIImage("JoystickZone", canvasGo.transform, null, new Color(1f, 1f, 1f, 0.004f), true,
                                     new Vector2(0f, 0f), new Vector2(0.5f, 1f), mid, Vector2.zero, Vector2.zero);
        RectTransform jbase = UIImage("Base", zone, Sprite256("UI/joystick_base.png"), new Color(1f, 1f, 1f, 0.55f), false, mid, mid, mid, Vector2.zero, new Vector2(260f, 260f));
        RectTransform knob = UIImage("Knob", jbase, Sprite256("UI/joystick_knob.png"), new Color(1f, 1f, 1f, 0.8f), false, mid, mid, mid, Vector2.zero, new Vector2(110f, 110f));
        VirtualJoystick js = zone.gameObject.AddComponent<VirtualJoystick>();
        js.baseRect = jbase;
        js.knobRect = knob;

        // attack button (bottom right)
        Vector2 br = new Vector2(1f, 0f);
        Sprite circle = Sprite256("UI/button_circle.png");
        RectTransform atk = UIImage("AttackButton", canvasGo.transform, circle, new Color(1f, 1f, 1f, 0.85f), true, br, br, mid, new Vector2(-135f, 135f), new Vector2(190f, 190f));
        UIImage("Icon", atk, S("Swords/sword_silver_diag.png"), white, false, mid, mid, mid, Vector2.zero, new Vector2(112f, 112f));
        TouchButton tb = atk.gameObject.AddComponent<TouchButton>();
        tb.kind = TouchButton.Kind.Attack;
        tb.weapons = holder;

        // 4 sword-skin buttons (placeholders for the future skills)
        int[] skins = { 0, 1, 2, 4 };
        Vector2[] pos = { new Vector2(-335f, 105f), new Vector2(-305f, 235f), new Vector2(-215f, 325f), new Vector2(-90f, 355f) };
        for (int i = 0; i < 4; i++)
        {
            RectTransform b = UIImage("SkinButton" + (i + 1), canvasGo.transform, circle, new Color(1f, 1f, 1f, 0.75f), true, br, br, mid, pos[i], new Vector2(110f, 110f));
            UIImage("Icon", b, S("Swords/sword_" + SkinNames[skins[i]] + "_diag.png"), white, false, mid, mid, mid, Vector2.zero, new Vector2(68f, 68f));
            TouchButton t = b.gameObject.AddComponent<TouchButton>();
            t.kind = TouchButton.Kind.Skin;
            t.skinIndex = skins[i];
            t.weapons = holder;
        }

        // weapon swap button (top right): sword <-> bow
        Vector2 tr = new Vector2(1f, 1f);
        RectTransform swap = UIImage("SwapButton", canvasGo.transform, circle, new Color(1f, 1f, 1f, 0.8f), true, tr, tr, mid, new Vector2(-95f, -95f), new Vector2(120f, 120f));
        UIImage("Icon", swap, S("Bow/bow_idle.png"), white, false, mid, mid, mid, Vector2.zero, new Vector2(76f, 76f));
        TouchButton sb = swap.gameObject.AddComponent<TouchButton>();
        sb.kind = TouchButton.Kind.Swap;
        sb.weapons = holder;

        // event system (matches whichever input backend the project uses)
        GameObject es = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
        InputSystemUIInputModule mod = es.AddComponent<InputSystemUIInputModule>();
        System.Reflection.MethodInfo assign = mod.GetType().GetMethod("AssignDefaultActions");
        if (assign != null) assign.Invoke(mod, null);
#else
        es.AddComponent<StandaloneInputModule>();
#endif
    }

    // UI sprites are Single-mode sprites imported by PixelArtImporter.
    static Sprite Sprite256(string rel)
    {
        return S(rel);
    }
}
