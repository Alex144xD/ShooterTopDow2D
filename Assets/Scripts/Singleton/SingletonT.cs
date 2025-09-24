using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Esto garantiza que la m�sica se reanude si por alguna raz�n no suena
        if (audioSource != null)
        {
            audioSource.loop = true;
            if (audioSource.clip != backgroundMusic)
                audioSource.clip = backgroundMusic;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    private void OnEnable()
    {
        // Vuelve a verificar al cargar escena
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
    }
}
