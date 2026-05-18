using UnityEngine;

/// <summary>
/// Singleton. Assign AudioClips in the Inspector, then call AudioManager.Instance.PlayX() from anywhere.
/// All clips are optional — missing clips are silently skipped so the game never crashes.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Card Sounds")]
    public AudioClip cardDealSound;
    public AudioClip cardFlipSound;

    [Header("Action Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip chipPlaceSound;
    public AudioClip doubleDownSound;

    [Header("Result Sounds")]
    public AudioClip winSound;
    public AudioClip blackjackSound;
    public AudioClip loseSound;
    public AudioClip bustSound;
    public AudioClip pushSound;

    [Header("Ambient")]
    public AudioClip ambientCasinoLoop;

    [Header("Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 0.3f;

    // Pitch randomization so repeated card sounds feel natural
    [Range(0f, 0.15f)] public float pitchVariance = 0.08f;

    private AudioSource sfxSource;
    private AudioSource ambientSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
    }

    void Start()
    {
        PlayAmbient();
        SubscribeToGameEvents();
    }

    void SubscribeToGameEvents()
    {
        BlackjackGameManager gm = FindObjectOfType<BlackjackGameManager>();
        if (gm == null) return;

        gm.OnCardDealt    += PlayCardDeal;
        gm.OnCardFlipped  += PlayCardFlip;
        gm.OnBetPlaced    += PlayChipPlace;
        gm.OnPlayerWin    += PlayWin;
        gm.OnBlackjack    += PlayBlackjack;
        gm.OnPlayerLose   += PlayLose;
        gm.OnBust         += PlayBust;
        gm.OnPush         += PlayPush;
        gm.OnDoubleDown   += PlayDoubleDown;
        gm.OnButtonClick  += PlayButtonClick;
    }

    // ── Public play methods (called directly or via events) ──────────────────

    public void PlayCardDeal()    => PlaySFX(cardDealSound, randomPitch: true);
    public void PlayCardFlip()    => PlaySFX(cardFlipSound, randomPitch: true);
    public void PlayButtonClick() => PlaySFX(buttonClickSound);
    public void PlayChipPlace()   => PlaySFX(chipPlaceSound);
    public void PlayDoubleDown()  => PlaySFX(doubleDownSound);
    public void PlayWin()         => PlaySFX(winSound);
    public void PlayBlackjack()   => PlaySFX(blackjackSound);
    public void PlayLose()        => PlaySFX(loseSound);
    public void PlayBust()        => PlaySFX(bustSound);
    public void PlayPush()        => PlaySFX(pushSound);

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ambientSource.volume = ambientVolume * masterVolume;
    }

    // ── Internal helpers ─────────────────────────────────────────────────────

    void PlaySFX(AudioClip clip, bool randomPitch = false)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.pitch = randomPitch
            ? 1f + Random.Range(-pitchVariance, pitchVariance)
            : 1f;
        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    void PlayAmbient()
    {
        if (ambientCasinoLoop == null || ambientSource == null) return;
        ambientSource.clip = ambientCasinoLoop;
        ambientSource.volume = ambientVolume * masterVolume;
        ambientSource.Play();
    }
}
