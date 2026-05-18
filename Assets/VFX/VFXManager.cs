using System.Collections;
using UnityEngine;

/// <summary>
/// Creates and manages all particle effects from code — no Particle System prefabs needed in the Editor.
/// Attach to any GameObject in the scene. Positions are set automatically relative to the camera/table.
/// </summary>
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Optional: assign transforms for effect spawn points")]
    public Transform playerCardArea;   // where win burst appears
    public Transform dealerCardArea;   // where bust cross appears
    public Transform tableCenter;      // where blackjack celebration fires

    [Header("Colors")]
    public Color winColor     = new Color(1f, 0.85f, 0f);    // gold
    public Color blackjackColor = new Color(1f, 0.4f, 0f);   // orange
    public Color bustColor    = new Color(0.9f, 0.1f, 0.1f); // red
    public Color dealColor    = new Color(0.8f, 0.9f, 1f);   // light blue
    public Color pushColor    = new Color(0.6f, 0.6f, 0.6f); // grey

    // Runtime-created particle systems
    private ParticleSystem winBurst;
    private ParticleSystem blackjackCelebration;
    private ParticleSystem bustEffect;
    private ParticleSystem dealTrail;
    private ParticleSystem pushEffect;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        BuildAllEffects();
    }

    void Start()
    {
        SubscribeToGameEvents();
    }

    void SubscribeToGameEvents()
    {
        BlackjackGameManager gm = FindObjectOfType<BlackjackGameManager>();
        if (gm == null) return;

        gm.OnCardDealt   += PlayDealEffect;
        gm.OnPlayerWin   += PlayWinEffect;
        gm.OnBlackjack   += PlayBlackjackEffect;
        gm.OnBust        += PlayBustEffect;
        gm.OnPush        += PlayPushEffect;
    }

    // ── Public trigger methods ────────────────────────────────────────────────

    public void PlayDealEffect()      => Burst(dealTrail, GetSpawnPoint(playerCardArea));
    public void PlayWinEffect()       => Burst(winBurst, GetSpawnPoint(playerCardArea));
    public void PlayBlackjackEffect() => StartCoroutine(BlackjackSequence());
    public void PlayBustEffect()      => Burst(bustEffect, GetSpawnPoint(playerCardArea));
    public void PlayPushEffect()      => Burst(pushEffect, GetSpawnPoint(tableCenter));

    // ── Internal helpers ──────────────────────────────────────────────────────

    Vector3 GetSpawnPoint(Transform anchor)
    {
        if (anchor != null) return anchor.position;
        // Fallback: in front of camera at table level
        Camera cam = Camera.main;
        if (cam != null) return cam.transform.position + cam.transform.forward * 1.5f + Vector3.down * 0.3f;
        return Vector3.zero;
    }

    void Burst(ParticleSystem ps, Vector3 position)
    {
        if (ps == null) return;
        ps.transform.position = position;
        ps.Play();
    }

    IEnumerator BlackjackSequence()
    {
        Burst(blackjackCelebration, GetSpawnPoint(tableCenter));
        yield return new WaitForSeconds(0.3f);
        Burst(winBurst, GetSpawnPoint(playerCardArea));
    }

    // ── Particle system factory ───────────────────────────────────────────────

    void BuildAllEffects()
    {
        winBurst           = BuildBurst("VFX_WinBurst",         winColor,        count: 60, speed: 4f,  size: 0.08f, lifetime: 1.5f);
        blackjackCelebration = BuildBurst("VFX_Blackjack",      blackjackColor,  count: 120, speed: 6f, size: 0.1f,  lifetime: 2f);
        bustEffect         = BuildBurst("VFX_Bust",             bustColor,       count: 30, speed: 2f,  size: 0.06f, lifetime: 1f);
        dealTrail          = BuildBurst("VFX_Deal",             dealColor,       count: 15, speed: 3f,  size: 0.04f, lifetime: 0.6f);
        pushEffect         = BuildBurst("VFX_Push",             pushColor,       count: 20, speed: 1.5f, size: 0.05f, lifetime: 0.8f);
    }

    ParticleSystem BuildBurst(string goName, Color color, int count, float speed, float size, float lifetime)
    {
        GameObject go = new GameObject(goName);
        go.transform.SetParent(transform);

        ParticleSystem ps = go.AddComponent<ParticleSystem>();

        // Stop it immediately — we play it manually
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.startLifetime  = lifetime;
        main.startSpeed     = new ParticleSystem.MinMaxCurve(speed * 0.6f, speed);
        main.startSize      = new ParticleSystem.MinMaxCurve(size * 0.5f, size);
        main.startColor     = new ParticleSystem.MinMaxGradient(color, color * 0.7f);
        main.gravityModifier = 0.3f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.playOnAwake    = false;
        main.loop           = false;

        var emission = ps.emission;
        emission.enabled = true;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, count)
        });
        emission.rateOverTime = 0;

        var shape = ps.shape;
        shape.enabled     = true;
        shape.shapeType   = ParticleSystemShapeType.Sphere;
        shape.radius      = 0.15f;

        // Simple additive glow via particle renderer
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material   = new Material(Shader.Find("Particles/Standard Unlit"));
        if (renderer.material != null)
            renderer.material.color = color;

        return ps;
    }
}
