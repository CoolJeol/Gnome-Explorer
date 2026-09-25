using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]

    public AudioClip[] enemyAttackSounds;
    public AudioClip[] enemyHurtSounds;
    public AudioClip[] enemyDeathSounds;

    public AudioClip[] playerHurtSounds;
    public AudioClip[] playerAttackSounds;
    public AudioClip[] playerBlockSounds;

    [Header("Sound Effect Volume")]

    [Range(0f, 1f)]
    public float enemyAttackVolume = 1f;

    [Range(0f, 1f)]
    public float enemyHurtVolume = 1f;

    [Range(0f, 1f)]
    public float enemyDeathVolume = 1f;

    [Range(0f, 1f)]
    public float playerHurtVolume = 1f;

    [Range(0f, 1f)]
    public float playerAttackVolume = 1f;

    [Range(0f, 1f)]
    public float playerBlockVolume = 1f;

    [Header("Background Music")]

    public AudioClip backgroundMusic;

    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    private AudioSource audioSource;
    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Sound effects
        audioSource = GetComponent<AudioSource>();

        // Music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }

    void Start()
    {
        PlayMusic();
    }

    // -------------------------
    // MUSIC
    // -------------------------

    void PlayMusic()
    {
        if (backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    // -------------------------
    // SOUND EFFECTS
    // -------------------------

    public void PlayEnemyAttack()
    {
        PlayRandom(enemyAttackSounds, enemyAttackVolume);
    }

    public void PlayEnemyHurt()
    {
        PlayRandom(enemyHurtSounds, enemyHurtVolume);
    }

    public void PlayEnemyDeath()
    {
        PlayRandom(enemyDeathSounds, enemyDeathVolume);
    }

    public void PlayPlayerHurt()
    {
        PlayRandom(playerHurtSounds, playerHurtVolume);
    }

    public void PlayPlayerAttack()
    {
        PlayRandom(playerAttackSounds, playerAttackVolume);
    }

    public void PlayPlayerBlock()
    {
        PlayRandom(playerBlockSounds, playerBlockVolume);
    }

    void PlayRandom(AudioClip[] sounds, float volume)
    {
        if (sounds == null || sounds.Length == 0)
            return;

        AudioClip sound = sounds[Random.Range(0, sounds.Length)];

        audioSource.PlayOneShot(sound, volume);
    }
}