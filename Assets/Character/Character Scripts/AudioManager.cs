using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip Player")]
    public AudioClip[] Background;
    public AudioClip[] PlayerAttack;
    public AudioClip[] PlayerBlock;
    public AudioClip[] PlayerHurt;

    [Header("Audio Clip Slime")]
    public AudioClip[] SlimeEat;
    public AudioClip[] SlimeHurt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager: musicSource is not assigned in the Inspector.");
            return;
        }

        if (Background == null || Background.Length == 0)
        {
            Debug.LogWarning("AudioManager: Background array is empty or null.");
            return;
        }

        // Use a single clip (first item). Use Random.Range(...) if you want a random track.
        musicSource.clip = Background[0];
        musicSource.loop = true;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
